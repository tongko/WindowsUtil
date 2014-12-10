using FolderSizeScanner.Core;
using System;

namespace FolderSizeScanner.Document
{
	[Serializable]
	class Document : IDocument
	{
		public Document()
		{
		}

		public Document(string computer, DateTime asOf, long fsoCount, ISizeNode[] nodes)
		{
			ComputerName = computer;
			ResultAsOf = asOf;
			FsoCount = fsoCount;
			Nodes = nodes;
		}

		public string ComputerName { get; set; }

		public DateTime ResultAsOf { get; set; }

		public long FsoCount { get; set; }

		public ISizeNode[] Nodes { get; private set; }

		public ISizeNode FindNode(string path)
		{
			return null;
		}
	}
}
