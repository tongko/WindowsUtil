using System;

namespace FolderSizeScanner.Core
{
	class InvalidPathFormatException : Exception
	{
		public InvalidPathFormatException(string message, string path)
			: base(message)
		{
			Path = path;
		}

		public string Path { get; set; }
	}
}
