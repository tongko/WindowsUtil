using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace FolderSizeScanner.Core
{
	// ReSharper disable InconsistentNaming
	static class Win32
	{
		#region Constants

		public const uint IO_REPARSE_TAG_MOUNT_POINT = 0xA0000003;		// Moiunt point or junction, see winnt.h
		public const uint IO_REPARSE_TAG_SYMLINK = 0xA000000C;			// SYMLINK or SYMLINKD (see http://wesnerm.blogs.com/net_undocumented/2006/10/index.html)
		public const uint FILE_FLAG_BACKUP_SEMANTICS = 0x02000000;
		public const uint FILE_FLAG_OPEN_REPARSE_POINT = 0x00200000;
		public const uint FILE_DEVICE_FILE_SYSTEM = 9;
		public const uint FILE_ANY_ACCESS = 0;
		public const uint METHOD_BUFFERED = 0;
		public const int MAXIMUM_REPARSE_DATA_BUFFER_SIZE = 16 * 1024;
		public const int FSCTL_GET_REPARSE_POINT = 42;

		#endregion


		#region Structure

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct REPARSE_DATA_BUFFER
		{
			public uint ReparseTag;
			public short ReparseDataLength;
			public short Reserved;
			public short SubsNameOffset;
			public short SubsNameLength;
			public short PrintNameOffset;
			public short PrintNameLength;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = MAXIMUM_REPARSE_DATA_BUFFER_SIZE)]
			public char[] ReparseTarget;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public struct WIN32_FIND_DATA
		{
			public uint dwFileAttributes;
			public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;
			public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;
			public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;
			public uint nFileSizeHigh;
			public uint nFileSizeLow;
			public uint dwReserved0;
			public uint dwReserved1;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
			public string cFileName;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
			public string cAlternateFileName;
		}

		#endregion


		#region Dll Imports

		[DllImport("kernel32.dll", SetLastError = true, PreserveSig = true, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool DeviceIoControl(
			SafeFileHandle hDevice,
			uint dwIoControlCode,
			IntPtr lpInBuffer,
			uint nInBufferSize,
			//IntPtr lpOutBuffer, 
			out REPARSE_DATA_BUFFER outBuffer,
			uint nOutBufferSize,
			out uint lpBytesReturned,
			IntPtr lpOverlapped);

		[DllImport("Kernel32.dll", SetLastError = true, PreserveSig = true, CharSet = CharSet.Unicode)]
		public static extern SafeFileHandle CreateFile(
			string fileName,
			[MarshalAs(UnmanagedType.U4)] FileAccess fileAccess,
			[MarshalAs(UnmanagedType.U4)] FileShare fileShare,
			int securityAttributes,
			[MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
			uint flags,
			IntPtr template);

		[DllImport("kernel32.dll", SetLastError = true, PreserveSig = true, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool CloseHandle(IntPtr hObject);

		[DllImport("kernel32.dll", SetLastError = true, PreserveSig = true, CharSet = CharSet.Unicode)]
		public static extern uint GetCompressedFileSize(
			string lpFileName,
			out uint lpFileSizeHigh);

		[DllImport("kernel32.dll", SetLastError = true, PreserveSig = true, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetDiskFreeSpace(
			string lpRootPathName,
			out uint lpSectorsPerCluster, out uint lpBytesPerSector, out uint lpNumberOfFreeClusters,
			out uint lpTotalNumberOfClusters);

		[DllImport("kernel32.dll", SetLastError = true, PreserveSig = true, CharSet = CharSet.Unicode)]
		public static extern SafeFindHandle FindFirstFile(
			string lpFileName,
			out WIN32_FIND_DATA lpFindFileData);

		[DllImport("kernel32.dll", SetLastError = true, PreserveSig = true, CharSet = CharSet.Unicode)]
		public static extern bool FindNextFile(
			SafeFindHandle hFindFile,
			out WIN32_FIND_DATA lpFindFileData);

		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("kernel32.dll", SetLastError = true, PreserveSig = true, CharSet = CharSet.Unicode)]
		public static extern bool FindClose(IntPtr hFindFile);

		#endregion
	}
	// ReSharper restore InconsistentNaming
}
