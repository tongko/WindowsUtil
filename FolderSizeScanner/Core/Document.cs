using System;

namespace FolderSizeScanner.Core
{
	[Serializable]
	class Document : IDocument
	{
		public Document()
		{
		}

		public Document(DateTime asOf, long fsoCount, ISizeNode[] nodes)
		{
			ResultAsOf = asOf;
			FSOCount = fsoCount;
			Nodes = nodes;
		}

		public DateTime ResultAsOf { get; set; }

		public long FSOCount { get; set; }

		public ISizeNode[] Nodes { get; private set; }

		public ISizeNode FindNode(string path)
		{

		}
	}
}
