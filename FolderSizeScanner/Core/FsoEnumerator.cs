using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;

namespace FolderSizeScanner.Core
{
	class FsoEnumerator : IEnumerator<FindData>
	{
		private readonly string _path;
		private SafeFindHandle _findHandle;
		private bool _noMoreFiles;

		public FsoEnumerator(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
				throw new ArgumentNullException("path");

			_path = EnsureUnc(path);
			_noMoreFiles = false;
		}

		public FindData Current { get; private set; }

		public bool MoveNext()
		{
			if (_noMoreFiles)
				return false;

			Win32.WIN32_FIND_DATA win32FindData;

			if (_findHandle == null)
			{
				_findHandle = Win32.FindFirstFile(_path, out win32FindData);
				if (_findHandle.IsInvalid)
					throw new Win32Exception(Marshal.GetLastWin32Error());

				Current = FindData.Create(win32FindData);
				return true;
			}

			if (!Win32.FindNextFile(_findHandle, out win32FindData))
			{
				_noMoreFiles = true;
				return false;
			};

			Current = FindData.Create(win32FindData);
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

		#region Members of IEnumerator<FindData>

		FindData IEnumerator<FindData>.Current
		{
			get { return Current; }
		}

		void IDisposable.Dispose()
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
			Reset();
		}

		#endregion
	}
}
