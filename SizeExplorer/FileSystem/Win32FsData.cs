using Delimon.Win32.IO;
using System;
using SizeExplorer.Core;

namespace SizeExplorer.FileSystem
{
	/// <summary>
	/// Contains information about a file returned by the 
	/// <see cref="FileSystemEnumerator"/> class.
	/// </summary>
	[Serializable]
	public sealed class Win32FsData
	{
		private Win32FsData()
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
		/// Create a new instance of <see cref="Win32FsData" /> using specified WIN32_FIND_DATA structure.
		/// </summary>
		/// <param name="data">A WIN32_FIND_DATA structure representing target file system object.</param>
		/// <returns>New instance of Win32FsData.</returns>
		public static Win32FsData Create(Win32Native.WIN32_FIND_DATA data)
		{
			if (string.IsNullOrWhiteSpace(data.cFileName)) return null;

			var w = new Win32FsData
			{
				Attributes = ((FileAttributes)data.dwFileAttributes),
				Name = data.cFileName,
				Size = (data.nFileSizeHigh * (ulong)0xfffffffeL) + data.nFileSizeLow
			};

			return w;
		}
	}
}
