using System.Collections;
using System.Diagnostics;
using Delimon.Win32.IO;
using SizeExplorer.Controls;
using SizeExplorer.FileSystem;
using SizeExplorer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using FileAccess = System.IO.FileAccess;
using FileMode = System.IO.FileMode;
using FileShare = System.IO.FileShare;

namespace SizeExplorer.Core
{
	public static class FileSizeHelper
	{
		public static ulong Build(ISizeNode node)
		{
			ulong fsoCount = 1;
			var enumerators = new Stack<FileSystemEnumerator>();
			var nodes = new Stack<ISizeNode>();
			enumerators.Push(new FileSystemEnumerator(node.Path));
			nodes.Push(node);

			while (enumerators.Count > 0)
			{
				var e = enumerators.Pop();
				var n = nodes.Pop();

				while ((e.MoveNext()))
				{
					var dir = e.Current;
					if (dir.Name == "." || dir.Name == "..") continue;

					var fp = Path.Combine(n.Path, dir.Name);
					if (IsHardLink(fp))
					{
						Console.WriteLine("{0}: {1}", fp, dir.Attributes);
						continue;
					}

					var newNode = new SizeNode(fp, dir.Name) { IsFile = !dir.IsDirectory };
					AddChildNode(n, newNode);

					if (dir.IsDirectory)
					{
						fsoCount++;
						enumerators.Push(e);
						nodes.Push(n);
						enumerators.Push(new FileSystemEnumerator(newNode.Path));
						nodes.Push(newNode);
						break;
					}
				}
			}

			return fsoCount;
		}

		public static void BuildTreeAtRoot(ISizeNode node, UpdateViewItemCallback addCallback, UpdateViewItemCallback updateCallback)
		{
			var dirs = Directory.GetDirectories(node.Path);
			foreach (var dNode in from dir in dirs where !IsHardLink(dir) select new SizeNode(new DirectoryInfo(dir)))
			{
				node.UpdateCallback = updateCallback;
				AddChildNode(node, dNode);
				Task.Factory.StartNew((o) =>
				{
					var n = o as ISizeNode;
					if (n == null) return;
					if (addCallback != null)
						addCallback(null, n);
				}, dNode);
				BuildTree(dNode, updateCallback);
			}

			var files = Directory.GetFiles(node.Path);
			foreach (var fNode in from file in files where !IsHardLink(file) select new SizeNode(new FileInfo(file)))
			{
				fNode.UpdateCallback = updateCallback;
				fNode.IsFile = true;
				AddChildNode(node, fNode);
			}
		}

		public static void BuildTree(ISizeNode node, UpdateViewItemCallback callback)
		{
			var dirs = Directory.GetDirectories(node.Path);
			foreach (var dNode in from dir in dirs where !IsHardLink(dir) select new SizeNode(new DirectoryInfo(dir)))
			{
				node.UpdateCallback = callback;
				AddChildNode(node, dNode);
				BuildTree(dNode, callback);
			}

			var files = Directory.GetFiles(node.Path);
			foreach (var fNode in from file in files where !IsHardLink(file) select new SizeNode(new FileInfo(file)))
			{
				fNode.UpdateCallback = callback;
				fNode.IsFile = true;
				AddChildNode(node, fNode);
			}
		}

		public static void CalculateSize(ISizeNode node, Action<ISizeNode> callback)
		{
			var enums = new Stack<IEnumerator<ISizeNode>>();
			var nodes = new Stack<ISizeNode>();
			enums.Push(node.Children.GetEnumerator());
			nodes.Push(node);

			while (enums.Count > 0)
			{
				var e = enums.Pop();
				var n = nodes.Pop();

				while (e.MoveNext())
				{
					var child = e.Current;
					if (child.IsFile)
					{
						var size = GetFileSizeOnDisk(node.Path);
						n.AddSize(size);
					}
					else
					{
						enums.Push(e);
						enums.Push(child.Children.GetEnumerator());
						nodes.Push(n);
						nodes.Push(child);
						break;
					}
				}

				callback(n);
			}
		}

		public static void CalculateSize(ISizeNode node)
		{
			if (!FileSystemExists(node.Path))
				return;

			if (!node.IsFile)
			{
				Parallel.For(0, node.Children.Count, (i, state) =>
				{
					var child = node.Children[i];
					if (IsHardLink(child.Path)) return;

					CalculateSize(child);
					node.AddSize(child.Size);
				});

				foreach (var c in node.Children.Where(c => c.UpdateCallback != null))
				{
					c.UpdateCallback(c.BindItem, c);
				}
			}
			else
			{
				var size = GetFileSizeOnDisk(node.Path);
				node.AddSize(size);
			}
		}

		public static void AddChildNode(ISizeNode parent, ISizeNode child)
		{
			parent.Children.Add(child);
			child.Parent = parent;
		}

		/// <summary>
		/// Calculate file size on disk.
		/// </summary>
		/// <param name="path">File info instance of the specific file</param>
		/// <returns>
		/// The file size on disk. 
		/// If return value is less than zero, error occurs: 
		/// -1 - Param error. 
		/// -2 - Win32 error.
		/// </returns>
		public static ulong GetFileSizeOnDisk(string path)
		{
			uint sectorsPerCluster, bytesPerSector;

			if (string.IsNullOrWhiteSpace(path))
				return 0;	//	Param error.

			var info = new FileInfo(path);
			if (info.Directory == null) return 0;

			var root = info.Directory.Root;
			if (!root.EndsWith("\\"))
				root = root + "\\";
			GetDiskSpaceInfo(root, out sectorsPerCluster, out bytesPerSector);

			var clusterSize = sectorsPerCluster * bytesPerSector;

			uint hosize;
			var losize = Win32Native.GetCompressedFileSize(info.FullName, out hosize);
			if (losize == 0)
			{
				var err = Marshal.GetLastWin32Error();
				if (err == 0)
					return 0;
			}

			var size = (ulong)hosize << 32 | losize;
			return ((size + clusterSize - 1) / clusterSize) * clusterSize;
		}

		private static readonly object SyncObject = new object();
		public static void GetDiskSpaceInfo(string root, out uint sectorsPerCluster, out uint bytesPerSector)
		{
			var info = SizeExplorerRuntime.DiskSizeInfo.FirstOrDefault(i => i.Root.Equals(root));
			if (info.Equals(DiskSizeInfoCache.Empty))
			{
				lock (SyncObject)
				{
					if (info.Equals(DiskSizeInfoCache.Empty))
					{
						uint dummy;
						if (!Win32Native.GetDiskFreeSpace(root, out sectorsPerCluster, out bytesPerSector, out dummy, out dummy))
							throw new Win32Exception(Marshal.GetLastWin32Error());

						SizeExplorerRuntime.DiskSizeInfo.Add(new DiskSizeInfoCache
						{
							Root = root,
							BytesPerSector = bytesPerSector,
							SectorsPerCluster = sectorsPerCluster
						});

						return;
					}
				}
			}

			sectorsPerCluster = info.SectorsPerCluster;
			bytesPerSector = info.BytesPerSector;
		}

		/// <summary>
		/// Check if a file is hard link (Junction/Symbolic link/Mount Point).
		/// </summary>
		/// <param name="info">The file info instance of the specific file.</param>
		/// <returns>If a file is hard link, or error occurs, return result is true.</returns>
		public static bool IsHardLink(string info)
		{
			if (string.IsNullOrWhiteSpace(info))
				return true;

			//			return new ReparsePoint(info).Tag != ReparsePoint.TagType.None;
			Debug.Assert(!string.IsNullOrEmpty(info) && info.Length > 2 && info[1] == ':' && info[2] == '\\');

			// Open the file and get its handle
			var handle = Win32Native.CreateFile(info, FileAccess.Read, FileShare.None, 0, FileMode.Open,
				Win32Native.FILE_FLAG_OPEN_REPARSE_POINT | Win32Native.FILE_FLAG_BACKUP_SEMANTICS, IntPtr.Zero);
			if (handle.IsInvalid) return true;

			Win32Native.REPARSE_DATA_BUFFER buffer;
			// Make up the control code - see CTL_CODE on ntddk.h
			const uint controlCode = (Win32Native.FILE_DEVICE_FILE_SYSTEM << 16)
									 | (Win32Native.FILE_ANY_ACCESS << 14) | (Win32Native.FSCTL_GET_REPARSE_POINT << 2)
									 | Win32Native.METHOD_BUFFERED;
			uint bytesReturned;
			var result = true;
			if (Win32Native.DeviceIoControl(handle, controlCode, IntPtr.Zero, 0, out buffer,
				Win32Native.MAXIMUM_REPARSE_DATA_BUFFER_SIZE, out bytesReturned, IntPtr.Zero))
			{
				// Note that according to http://wesnerm.blogs.com/net_undocumented/2006/10/symbolic_links_.html
				// Symbolic links store relative paths, while junctions use absolute paths
				// however, they can in fact be either, and may or may not have a leading \.
				Debug.Assert(buffer.ReparseTag == Win32Native.IO_REPARSE_TAG_SYMLINK
							 || buffer.ReparseTag == Win32Native.IO_REPARSE_TAG_MOUNT_POINT,
					"Unrecognised reparse tag"); // We only recognise these two

				if (buffer.ReparseTag != Win32Native.IO_REPARSE_TAG_SYMLINK &&
					buffer.ReparseTag != Win32Native.IO_REPARSE_TAG_MOUNT_POINT)
					result = false;
			}
			else
				result = false;

			if (!handle.IsClosed)
				handle.Dispose();

			return result;
		}

		public static ulong Sum(this IEnumerable<ulong> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			ulong num1 = 0;
			foreach (var num2 in source)
				unchecked
				{
					num1 += num2;
				}

			return num1;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool FileSystemExists(string path)
		{
			return (Directory.Exists(path) || File.Exists(path));
		}
	}
}
