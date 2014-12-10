using FolderSizeScanner.Core;
using System;

namespace FolderSizeScanner.Document
{
	interface IDocument
	{
		string ComputerName { get; set; }

		DateTime ResultAsOf { get; set; }

		long FsoCount { get; set; }

		ISizeNode[] Nodes { get; }

		ISizeNode FindNode(string path);
	}
}