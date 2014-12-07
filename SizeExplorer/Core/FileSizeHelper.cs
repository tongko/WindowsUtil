using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using SizeExplorer.Controls;
using SizeExplorer.Model;

namespace SizeExplorer.Core
{
	public static class FileSizeHelper
	{
		public static void BuildTree(ISizeNode node, UpdateViewItemCallback callback)
		{
			var dirs = Directory.GetDirectories(node.Path);
			foreach (var dNode in from dir in dirs where !IsHardLink(dir) select new SizeNode(new DirectoryInfo(dir)))
			{
				node.UpdateCallback = callback;
				AddChildNode(node, dNode);
			}

			foreach (var child in node.Children)
			{
				BuildTree(child, callback);
			}
		}

		public static void CalculateSize(ISizeNode node)
		{
			var dirs = Directory.GetDirectories(node.Path).ToArray();
			Parallel.For(0, dirs.Length, (i, state) =>
			{
				var dir = dirs[i];
				if (IsHardLink(dir)) return;

				var dNode = new SizeNode(new DirectoryInfo(dir));
				CalculateSize(dNode);
				node.AddSize(dNode.Size);
			});

			var files = Directory.GetFiles(node.Path).ToArray();
			for (var i = 0; i < files.Length; i++)
			{
				var path = files[i];
				if (IsHardLink(path)) continue;

				var size = GetFileSizeOnDisk(path);
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
			uint dummy, sectorsPerCluster, bytesPerSector;

			if (string.IsNullOrWhiteSpace(path))
				return 0;	//	Param error.

			var info = new FileInfo(path);
			GetDiskSpaceInfo(info.Directory.Root.FullName, out sectorsPerCluster, out bytesPerSector);
			
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

		public static void GetDiskSpaceInfo(string root, out uint sectorsPerCluster, out uint bytesPerSector)
		{
			var info = SizeExplorerRuntime.DiskSizeInfo.FirstOrDefault(i => i.Root.Equals(root));
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
			}
			else
			{
				sectorsPerCluster = info.SectorsPerCluster;
				bytesPerSector = info.BytesPerSector;
			}
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
	}
}
