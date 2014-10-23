using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace SizeExplorer.Core
{
	internal class ReparsePoint
	{
		public enum TagType
		{
			None = 0,
			MountPoint = 1,
			SymbolicLink = 2,
			JunctionPoint = 3,
			Error = 4
		}

		private readonly string _normalisedTarget;
		private readonly string _actualTarget;
		private readonly TagType _tag;

		/// <summary>
		/// Takes a full path to a reparse point and finds the target.
		/// </summary>
		/// <param name="path">Full path of the reparse point</param>
		public ReparsePoint(string path)
		{
			var success = false;
			Debug.Assert(!string.IsNullOrEmpty(path) && path.Length > 2 && path[1] == ':' && path[2] == '\\');
			_normalisedTarget = "";
			_tag = TagType.None;

			// Apparently we need to have backup privileges
			IntPtr token;
			var tokenPrivileges = new Win32Native.TOKEN_PRIVILEGES { Privileges = new Win32Native.LUID_AND_ATTRIBUTES[1] };
			if (Win32Native.OpenProcessToken(Win32Native.GetCurrentProcess(), Win32Native.TOKEN_ADJUST_PRIVILEGES,
				out token))
			{
				if (Win32Native.LookupPrivilegeValue(null, Win32Native.SE_BACKUP_NAME,
					out tokenPrivileges.Privileges[0].Luid))			// null for local system
				{
					tokenPrivileges.PrivilegeCount = 1;
					tokenPrivileges.Privileges[0].Attributes = Win32Native.SE_PRIVILEGE_ENABLED;
					success = Win32Native.AdjustTokenPrivileges(token, false, ref tokenPrivileges, Marshal.SizeOf(tokenPrivileges),
						IntPtr.Zero, IntPtr.Zero);
				}

				Win32Native.CloseHandle(token);
			}

			if (!success) return;

			// Open the file and get its handle
			var handle = Win32Native.CreateFile(path, FileAccess.Read, FileShare.None, 0, FileMode.Open,
				Win32Native.FILE_FLAG_OPEN_REPARSE_POINT | Win32Native.FILE_FLAG_BACKUP_SEMANTICS, IntPtr.Zero);
			if (handle.ToInt32() < 0) return;

			Win32Native.REPARSE_DATA_BUFFER buffer;
			// Make up the control code - see CTL_CODE on ntddk.h
			const uint controlCode = (Win32Native.FILE_DEVICE_FILE_SYSTEM << 16)
									 | (Win32Native.FILE_ANY_ACCESS << 14) | (Win32Native.FSCTL_GET_REPARSE_POINT << 2)
									 | Win32Native.METHOD_BUFFERED;
			uint bytesReturned;

			if (Win32Native.DeviceIoControl(handle, controlCode, IntPtr.Zero, 0, out buffer,
				Win32Native.MAXIMUM_REPARSE_DATA_BUFFER_SIZE, out bytesReturned, IntPtr.Zero))
			{
				var subsString = "";
				var printString = "";

				// Note that according to http://wesnerm.blogs.com/net_undocumented/2006/10/symbolic_links_.html
				// Symbolic links store relative paths, while junctions use absolute paths
				// however, they can in fact be either, and may or may not have a leading \.
				Debug.Assert(buffer.ReparseTag == Win32Native.IO_REPARSE_TAG_SYMLINK
							 || buffer.ReparseTag == Win32Native.IO_REPARSE_TAG_MOUNT_POINT,
					"Unrecognised reparse tag");						// We only recognise these two

				switch (buffer.ReparseTag)
				{
					case Win32Native.IO_REPARSE_TAG_SYMLINK:
						subsString = new string(buffer.ReparseTarget, (buffer.SubsNameOffset / 2 + 2), buffer.SubsNameLength / 2);
						printString = new string(buffer.ReparseTarget, (buffer.PrintNameOffset / 2 + 2), buffer.PrintNameLength / 2);
						_tag = TagType.SymbolicLink;
						break;
					case Win32Native.IO_REPARSE_TAG_MOUNT_POINT:
						subsString = new string(buffer.ReparseTarget, buffer.SubsNameOffset / 2, buffer.SubsNameLength / 2);
						printString = new string(buffer.ReparseTarget, buffer.PrintNameOffset / 2, buffer.PrintNameLength / 2);
						_tag = subsString.StartsWith(@"\??\Volume") ? TagType.MountPoint : TagType.JunctionPoint;
						break;
				}

				Debug.Assert(!(string.IsNullOrEmpty(subsString) && string.IsNullOrEmpty(printString)), "Failed to retrieve parse point");

				// the printstring should give us what we want
				if (!string.IsNullOrEmpty(printString))
					_normalisedTarget = printString;
				else
				{
					// if not we can use the substring with a bit of tweaking
					_normalisedTarget = subsString;

					Debug.Assert(_normalisedTarget.Length > 2, "Target string too short");
					Debug.Assert(
						(_normalisedTarget.StartsWith(@"\??\") && (_normalisedTarget[5] == ':' || _normalisedTarget.StartsWith(@"\??\Volume")) ||
						 (!_normalisedTarget.StartsWith(@"\??\") && _normalisedTarget[1] != ':')),
						"Malformed subsString");
					// Junction points must be absolute
					Debug.Assert(
						buffer.ReparseTag == Win32Native.IO_REPARSE_TAG_SYMLINK
						|| _normalisedTarget.StartsWith(@"\??\Volume")
						|| _normalisedTarget[1] == ':',
						"Relative junction point");

					if (_normalisedTarget.StartsWith(@"\??\"))
						_normalisedTarget = _normalisedTarget.Substring(4);
				}

				_actualTarget = _normalisedTarget;

				// Symlinks can be relative.
				if (buffer.ReparseTag == Win32Native.IO_REPARSE_TAG_SYMLINK
					&& (_normalisedTarget.Length < 2 || _normalisedTarget[1] != ':'))
				{
					// it's relative, we need to tack it onto the path
					if (_normalisedTarget[0] == '\\')
						_normalisedTarget = _normalisedTarget.Substring(1);

					if (path.EndsWith(@"\"))
						path = path.Substring(0, path.Length - 1);

					// Need to take the symlink name off the path
					_normalisedTarget = path.Substring(0, path.LastIndexOf('\\')) + @"\" + _normalisedTarget;
					// Note that if the symlink target path contains any ..s these are not normalised but returned as is.
				}

				// Remove any final slash for consistency
				if (_normalisedTarget.EndsWith("\\"))
					_normalisedTarget = _normalisedTarget.Substring(0, _normalisedTarget.Length - 1);
			}

			Win32Native.CloseHandle(handle);
		}

		/// <summary>
		/// This returns the normalised target, ie. if the actual target is relative it has been made absolute
		/// Note that it is not fully normalised in that .s and ..s may still be included.
		/// </summary>
		/// <returns>The normalised path</returns>
		public override string ToString()
		{
			return _normalisedTarget;
		}

		/// <summary>
		/// Gets the actual target string, before normalising
		/// </summary>
		public string Target
		{
			get
			{
				return _actualTarget;
			}
		}

		/// <summary>
		/// Gets the tag
		/// </summary>
		public TagType Tag
		{
			get
			{
				return _tag;
			}
		}
	}
}
