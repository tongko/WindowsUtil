using System.Collections;
using System.Collections.Generic;

namespace SizeExplorer.FileSystem
{
	public class FileSystemEnumerable : IEnumerable<Win32FsData>
	{
		private readonly string _path;

		public FileSystemEnumerable(string path)
		{
			_path = path;
		}

		protected FileSystemEnumerator GetEnumerator()
		{
			return new FileSystemEnumerator(_path);
		}

		#region Members of IEnumerable<Win32FsData>

		IEnumerator<Win32FsData> IEnumerable<Win32FsData>.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion
	}
}
