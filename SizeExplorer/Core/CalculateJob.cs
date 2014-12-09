using System;
using SizeExplorer.Controls;

namespace SizeExplorer.Core
{
	public class CalculateJob
	{
		private readonly ulong _fsoCount;
		private ulong _progress = 0;

		public CalculateJob(ISizeNode node, Action<int, string> reportProgress, ulong objectCount)
		{
			_fsoCount = objectCount;
			Node = node;
			ReportProgressAction = reportProgress;
		}

		public ISizeNode Node { get; private set; }

		public Action<int, string> ReportProgressAction { get; private set; }

		public void StartJob()
		{
			FileSizeHelper.CalculateSize(Node, NodeDoneCallback);
		}

		private void NodeDoneCallback(ISizeNode node)
		{
			_progress++;
			var p = ((int)(_progress / _fsoCount)) * 100;
			ReportProgressAction(p, node.Path);
		}
	}
}
