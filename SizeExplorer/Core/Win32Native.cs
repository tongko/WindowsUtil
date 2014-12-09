using System;
using System.IO;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace SizeExplorer.Core
{
	// ReSharper disable InconsistentNaming
	public static class Win32Native
	{
		#region Constants

		public const uint IO_REPARSE_TAG_MOUNT_POINT = 0xA0000003;		// Moiunt point or junction, see winnt.h
		public const uint IO_REPARSE_TAG_SYMLINK = 0xA000000C;			// SYMLINK or SYMLINKD (see http://wesnerm.blogs.com/net_undocumented/2006/10/index.html)
		public const uint SE_PRIVILEGE_ENABLED = 0x00000002;
		public const string SE_BACKUP_NAME = "SeBackupPrivilege";
		public const uint FILE_FLAG_BACKUP_SEMANTICS = 0x02000000;
		public const uint FILE_FLAG_OPEN_REPARSE_POINT = 0x00200000;
		public const uint FILE_DEVICE_FILE_SYSTEM = 9;
		public const uint FILE_ANY_ACCESS = 0;
		public const uint METHOD_BUFFERED = 0;
		public const int MAXIMUM_REPARSE_DATA_BUFFER_SIZE = 16 * 1024;
		public const uint TOKEN_ADJUST_PRIVILEGES = 0x0020;
		public const int FSCTL_GET_REPARSE_POINT = 42;

		public const int ERROR_NO_MORE_FILES = 18;

		public static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

		#endregion


		#region Structs

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

		[StructLayout(LayoutKind.Sequential)]
		public struct LUID
		{
			public uint LowPart;
			public int HighPart;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct LUID_AND_ATTRIBUTES
		{
			public LUID Luid;
			public uint Attributes;
		}

		public struct TOKEN_PRIVILEGES
		{
			public uint PrivilegeCount;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]		// !! think we only need one
			public LUID_AND_ATTRIBUTES[] Privileges;
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

		[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true, CharSet = CharSet.Auto)]
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

		[DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern SafeFileHandle CreateFile(
			string fileName,
			[MarshalAs(UnmanagedType.U4)] FileAccess fileAccess,
			[MarshalAs(UnmanagedType.U4)] FileShare fileShare,
			int securityAttributes,
			[MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
			uint flags,
			IntPtr template);

		[DllImport("advapi32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool OpenProcessToken(IntPtr ProcessHandle,
			uint DesiredAccess, out IntPtr TokenHandle);

		[DllImport("kernel32.dll")]
		public static extern IntPtr GetCurrentProcess();

		[DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool LookupPrivilegeValue(string lpSystemName, string lpName,
			out LUID lpLuid);

		[DllImport("advapi32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool AdjustTokenPrivileges(IntPtr TokenHandle,
			[MarshalAs(UnmanagedType.Bool)]bool DisableAllPrivileges,
			ref TOKEN_PRIVILEGES NewState,
			int BufferLength,
			//ref TOKEN_PRIVILEGES PreviousState,					!! for some reason this won't accept null
			IntPtr PreviousState,
			IntPtr ReturnLength);

		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool CloseHandle(IntPtr hObject);

		[DllImport("kernel32.dll", SetLastError = true, PreserveSig = true, CharSet = CharSet.Unicode)]
		public static extern uint GetCompressedFileSize(string lpFileName, out uint lpFileSizeHigh);

		[DllImport("kernel32.dll", SetLastError = true, PreserveSig = true, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetDiskFreeSpace(string lpRootPathName,
			out uint lpSectorsPerCluster, out uint lpBytesPerSector, out uint lpNumberOfFreeClusters,
			out uint lpTotalNumberOfClusters);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		public static extern SafeFindHandle FindFirstFile(string lpFileName, out WIN32_FIND_DATA lpFindFileData);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		public static extern bool FindNextFile(SafeFindHandle hFindFile, out WIN32_FIND_DATA lpFindFileData);

		#endregion
	}
}
