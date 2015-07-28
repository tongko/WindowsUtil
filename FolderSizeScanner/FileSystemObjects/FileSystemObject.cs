using System;
using System.Collections.Generic;
using System.IO;
using FolderSizeScanner.Core;

namespace FolderSizeScanner.FileSystemObjects
{
	// ReSharper disable PossibleInvalidOperationException
	class FileSystemObject
	{
		private bool? _isHardLink;
		private bool? _isDirectory;
		private ulong? _size;
		private ulong? _sizeOnDisk;

		public FileSystemObject(string fullName)
		{
			if (string.IsNullOrWhiteSpace(fullName))
				throw new ArgumentNullException("fullName");
			if (!File.Exists(fullName))
				throw new ArgumentException(@"Invalid path specified.", "fullName",
					new FileNotFoundException("System cannot find the file specified.", fullName));

			FullName = fullName;
			ChildObjects = new List<FileSystemObject>();
		}

		public int ChildCount { get { return ChildObjects.Count; } }

		public List<FileSystemObject> ChildObjects { get; private set; }

		public string FullName { get; private set; }

		public bool IsDirectory
		{
			get
			{
				if (!_isDirectory.HasValue)
				{
					var attr = File.GetAttributes(FullName);
					_isDirectory = (attr & FileAttributes.Directory) == FileAttributes.Directory;
				}

				return _isDirectory.Value;
			}
		}

		public bool IsHardLink
		{
			get
			{
				if (!_isHardLink.HasValue)
					_isHardLink = Fso.IsHardLink(FullName);

				return _isHardLink.Value;
			}
		}

		public ulong Size
		{
			get
			{
				if (!_size.HasValue)
				{
					if (IsDirectory)
					{
						foreach (var fileSystemObject in ChildObjects)
							_size += fileSystemObject.Size;
					}
					else if (IsHardLink)
						return 0;
					else
						_size = (ulong)new FileInfo(FullName).Length;
				}

				return _size.Value;
			}
		}

		public ulong SizeOnDisk {
			get
			{
				if (!_sizeOnDisk.HasValue)
				{
					if (IsDirectory)
					{
						foreach (var fileSystemObject in ChildObjects)
							_sizeOnDisk += fileSystemObject.SizeOnDisk;
					}
					else if (IsHardLink)
						return 0;
					else
						_sizeOnDisk = Fso.GetFileSizeOnDisk(FullName);
				}

				return _sizeOnDisk.Value;
			}
		}
	}
}
// ReSharper restore PossibleInvalidOperationException