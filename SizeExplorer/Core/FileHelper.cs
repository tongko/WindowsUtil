using System.IO;
using System.Runtime.InteropServices;

namespace SizeExplorer.Core
{
	internal static class FileHelper
	{
		#region Method

		/// <summary>
		/// Calculate file size on disk.
		/// </summary>
		/// <param name="info">File info instance of the specific file</param>
		/// <returns>
		/// The file size on disk. 
		/// If return value is less than zero, error occurs: 
		/// -1 - Param error. 
		/// -2 - Win32 error.
		/// </returns>
		public static long GetFileSizeOnDisk(string path)
		{
			uint dummy, sectorsPerCluster, bytesPerSector;

			if (string.IsNullOrWhiteSpace(path))
				return -1;	//	Param error.

			var info = new FileInfo(path);
			var result = Win32Native.GetDiskFreeSpace(info.Directory.Root.FullName, out sectorsPerCluster, out bytesPerSector,
				out dummy, out dummy);
			if (result == 0)
				return -2;	//	Win32 error.

			var clusterSize = sectorsPerCluster * bytesPerSector;

			uint hosize;
			var losize = Win32Native.GetCompressedFileSize(info.FullName, out hosize);
			if (losize == 0)
			{
				var err = Marshal.GetLastWin32Error();
				if (err == 0)
					return -2;
			}

			var size = (long)hosize << 32 | losize;
			return ((size + clusterSize - 1) / clusterSize) * clusterSize;
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

		#endregion
	}
}
