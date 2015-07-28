using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FolderSizeScanner.Core
{
	class Fso
	{
		public static readonly List<DiskSizeInfo> DiskSizeInfoCache = new List<DiskSizeInfo>();

		/// <summary>
		/// Check if a file is hard link (Junction/Symbolic link/Mount Point).
		/// </summary>
		/// <param name="info">The file info instance of the specific file.</param>
		/// <returns>If a file is hard link, or error occurs, return result is true.</returns>
		public static bool IsHardLink(string info)
		{
			if (string.IsNullOrWhiteSpace(info))
				return true;

			Debug.Assert(!string.IsNullOrEmpty(info) && info.Length > 2 && info[1] == ':' && info[2] == '\\');

			// Open the file and get its handle
			var handle = Win32.CreateFile(info, FileAccess.Read, FileShare.None, 0, FileMode.Open,
				Win32.FILE_FLAG_OPEN_REPARSE_POINT | Win32.FILE_FLAG_BACKUP_SEMANTICS, IntPtr.Zero);
			if (handle.IsInvalid) return true;

			Win32.REPARSE_DATA_BUFFER buffer;
			// Make up the control code - see CTL_CODE on ntddk.h
			const uint controlCode = (Win32.FILE_DEVICE_FILE_SYSTEM << 16)
									 | (Win32.FILE_ANY_ACCESS << 14) | (Win32.FSCTL_GET_REPARSE_POINT << 2)
									 | Win32.METHOD_BUFFERED;
			uint bytesReturned;
			var result = true;
			if (Win32.DeviceIoControl(handle, controlCode, IntPtr.Zero, 0, out buffer,
				Win32.MAXIMUM_REPARSE_DATA_BUFFER_SIZE, out bytesReturned, IntPtr.Zero))
			{
				// Note that according to http://wesnerm.blogs.com/net_undocumented/2006/10/symbolic_links_.html
				// Symbolic links store relative paths, while junctions use absolute paths
				// however, they can in fact be either, and may or may not have a leading \.
				Debug.Assert(buffer.ReparseTag == Win32.IO_REPARSE_TAG_SYMLINK
							 || buffer.ReparseTag == Win32.IO_REPARSE_TAG_MOUNT_POINT,
					"Unrecognised reparse tag"); // We only recognise these two

				if (buffer.ReparseTag != Win32.IO_REPARSE_TAG_SYMLINK &&
					buffer.ReparseTag != Win32.IO_REPARSE_TAG_MOUNT_POINT)
					result = false;
			}
			else
				result = false;

			if (!handle.IsClosed)
				handle.Dispose();

			return result;
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

			var root = info.Directory.Root.FullName;
			if (!root.EndsWith("\\"))
				root = root + "\\";
			GetDiskSpaceInfo(root, out sectorsPerCluster, out bytesPerSector);

			var clusterSize = sectorsPerCluster * bytesPerSector;

			uint hosize;
			var losize = Win32.GetCompressedFileSize(info.FullName, out hosize);
			if (losize == 0)
			{
				var err = Marshal.GetLastWin32Error();
				if (err == 0)
					return 0;
			}

			var size = (ulong)hosize << 32 | losize;
			return ((size + clusterSize - 1) / clusterSize) * clusterSize;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool FileSystemExists(string path)
		{
			return (Directory.Exists(path) || File.Exists(path));
		}

		private static void GetDiskSpaceInfo(string root, out uint sectorsPerCluster, out uint bytesPerSector)
		{
			var info = DiskSizeInfoCache.FirstOrDefault(i => i.Root.Equals(root));
			if (info.Equals(DiskSizeInfo.Empty))
			{
				lock (((ICollection)DiskSizeInfoCache).SyncRoot)
				{
					info = DiskSizeInfoCache.FirstOrDefault(i => i.Root.Equals(root));
					if (info.Equals(DiskSizeInfo.Empty))
					{
						uint dummy;
						if (!Win32.GetDiskFreeSpace(root, out sectorsPerCluster, out bytesPerSector, out dummy, out dummy))
							throw new Win32Exception(Marshal.GetLastWin32Error());

						DiskSizeInfoCache.Add(new DiskSizeInfo(root, bytesPerSector, sectorsPerCluster));

						return;
					}
				}
			}

			sectorsPerCluster = info.SectorsPerCluster;
			bytesPerSector = info.BytesPerSector;
		}

	}
}
