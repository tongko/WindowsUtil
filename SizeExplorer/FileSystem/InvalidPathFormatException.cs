using System;

namespace SizeExplorer.FileSystem
{
	public class InvalidPathFormatException : Exception
	{
		public InvalidPathFormatException(string message, string path)
			: base(message)
		{
		}
	}
}
