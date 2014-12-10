using System;
using System.IO;

namespace FolderSizeScanner.Core
{
	/// <summary>
	/// Contains information about a file returned by the 
	/// <see cref="FileSystemEnumerator"/> class.
	/// </summary>
	[Serializable]
	sealed class FindData
	{
		private FindData()
		{
		}

		/// <summary>
		/// Get file attributes of target file system object.
		/// </summary>
		public FileAttributes Attributes { get; private set; }

		/// <summary>
		/// Get a value indicating target file system object is a file.
		/// </summary>
		public bool IsDirectory { get { return (Attributes & FileAttributes.Directory) == FileAttributes.Directory; } }

		/// <summary>
		/// Get a value representing the name of target file system object.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Get a value representing the size in byte of target file system object. This value is zero if target object is directory.
		/// </summary>
		public ulong Size { get; private set; }

		/// <summary>
		/// Create a new instance of <see cref="FindData" /> using specified WIN32_FIND_DATA structure.
		/// </summary>
		/// <param name="data">A WIN32_FIND_DATA structure representing target file system object.</param>
		/// <returns>New instance of Win32FsData.</returns>
		public static FindData Create(Win32.WIN32_FIND_DATA data)
		{
			if (string.IsNullOrWhiteSpace(data.cFileName)) return null;

			var w = new FindData
			{
				Attributes = ((FileAttributes)data.dwFileAttributes),
				Name = data.cFileName,
				Size = (data.nFileSizeHigh * (ulong)0xfffffffeL) + data.nFileSizeLow
			};

			return w;
		}
	}
}
