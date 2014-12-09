using System;

namespace FolderSizeScanner.Core
{
	interface IDocument
	{
		DateTime ResultAsOf { get; set; }

		long FSOCount { get; set; }

		ISizeNode[] Nodes { get; }

		ISizeNode FindNode(string path);
	}
}