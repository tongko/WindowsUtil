using System;

namespace FolderSizeScanner.Document
{
	class DocumentFormatException : Exception
	{
		public DocumentFormatException(string message)
			: base(message)
		{
		}
	}
}
