using SizeExplorer.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SizeExplorer.Model
{
	public delegate void CompletedHandler(FileSystemNode node);

	public delegate void ExceptionHandler(Task task);

	public class FileSystemInfoX
	{
		private static readonly object SyncObject = new object();
		private readonly IHandleThreadException _parent;

		public FileSystemInfoX(IHandleThreadException parent)
		{
			_parent = parent;
			Drives = new List<FileSystemNode>();
		}

		public DateTime UpdatedOn { get; set; }

		public List<FileSystemNode> Drives { get; set; }

		public CompletedHandler ProcessCompleteCallback { get; set; }

		public void BeginAnalyze(string drive)
		{
			if (string.IsNullOrWhiteSpace(drive))
				throw new ArgumentNullException("drive");

			var dir = new DirectoryInfo(drive);
			var ias = new InfoAnalyzerState
			{
				Depth = 0,
				Path = dir.Root.FullName,
				Parent = null,
				HandleExceptionCallback = ExceptionHandler
			};
			var task = new Task(new InfoAnalyzer(TaskCompleted).Analyze, ias);
			task.ContinueWith(ExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
			task.Start();
		}

		public void TaskCompleted(FileSystemNode node)
		{
			if (node == null || node.Parent != null) return;

			Drives.Add(node);
			if (ProcessCompleteCallback != null)
				ProcessCompleteCallback(node);
		}

		private void ExceptionHandler(Task task)
		{
			lock (SyncObject)
			{
				if (_parent != null)
					_parent.ThreadExceptionHandlerCallback(this, task.Exception);
			}
		}
	}
}
