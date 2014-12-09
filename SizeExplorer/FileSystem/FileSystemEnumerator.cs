using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using SizeExplorer.Core;

namespace SizeExplorer.FileSystem
{
	[System.Security.SuppressUnmanagedCodeSecurity]
	public class FileSystemEnumerator : IEnumerator<Win32FsData>
	{
		private readonly string _path;
		private SafeFindHandle _findHandle;
		private bool _noMoreFiles;

		public FileSystemEnumerator(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
				throw new ArgumentNullException("path");

			_path = EnsureUnc(path);
			_noMoreFiles = false;
		}

		public Win32FsData Current { get; private set; }

		public bool MoveNext()
		{
			if (_noMoreFiles)
				return false;

			Win32Native.WIN32_FIND_DATA win32FindData;

			if (_findHandle == null)
			{
				_findHandle = Win32Native.FindFirstFile(_path, out win32FindData);
				if (_findHandle.IsInvalid)
					throw new Win32Exception(Marshal.GetLastWin32Error());

				Current = Win32FsData.Create(win32FindData);
				return true;
			}

			if (!Win32Native.FindNextFile(_findHandle, out win32FindData))
			{
				_noMoreFiles = true;
				return false;
			};

			Current = Win32FsData.Create(win32FindData);
			return true;
		}

		public void Reset()
		{
			if (_findHandle != null)
				_findHandle.Dispose();

			_findHandle = null;
			Current = null;
			_noMoreFiles = false;
		}

		private static string EnsureUnc(string path)
		{
			if (!Path.IsPathRooted(path))
				throw new InvalidPathFormatException("Must be absolute path.", path);

			if (!(path[0] == '\\' && path[1] == '\\'))
			{
				path = "\\\\?\\" + path;
			}

			if (path.EndsWith("\\"))
			{
				path = path + @"*.*";
			}
			else
			{
				path = path + @"\*.*";
			}

			return path;
		}

		#region Members of IEnumerator<Win32FsData>

		Win32FsData IEnumerator<Win32FsData>.Current
		{
			get { return Current; }
		}

		void System.IDisposable.Dispose()
		{
			if (_findHandle != null)
				_findHandle.Dispose();
		}

		object IEnumerator.Current
		{
			get { return Current; }
		}

		bool IEnumerator.MoveNext()
		{
			return MoveNext();
		}

		void IEnumerator.Reset()
		{
			throw new System.NotImplementedException();
		}

		#endregion
	}
}
