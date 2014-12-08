using Delimon.Win32.IO;
using SizeExplorer.Controls;
using SizeExplorer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace SizeExplorer.Core
{
	public static class FileSizeHelper
	{
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

			return new ReparsePoint(info).Tag != ReparsePoint.TagType.None;
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
