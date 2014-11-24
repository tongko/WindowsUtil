using SizeExplorer.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SizeExplorer.Model
{
	public class InfoAnalyzer
	{
		private readonly List<Task> _tasks = new List<Task>();

		public InfoAnalyzer(CompletedHandler completedCallback)
		{
			TaskCompletedCallback = completedCallback;
		}

		public CompletedHandler TaskCompletedCallback { get; set; }

		public void Analyze(object state)
		{
			var ias = state as InfoAnalyzerState;
			if (ias == null)
				throw new ArgumentException(@"Invalid state object. 'state' is not InfoAnalyzerState or not set to an object.",
					"state");

			if (!Directory.Exists(ias.Path))
				throw new DirectoryNotFoundException(string.Format("System cannot find the path '{0}'.", ias.Path));

			if (FileHelper.IsHardLink(ias.Path))
			{
				TaskCompletedCallback(null);
				return;
			}

			var node = new FileSystemNode { Depth = ias.Depth, Path = ias.Path, Parent = ias.Parent };
			if (node.Parent != null)
				node.Parent.Children.Add(node);

			var dirs = Directory.GetDirectories(node.Path).ToArray();
			//if (dirs.Length > 64)
			//{
			//	var k = 0;
			//	for (k = 0; k < dirs.Length / 64 + 1; k++)
			//	{
			//		_tasks.Clear();

			//		for (var i = 0; i < 64 * (k + 1) || i < dirs.Length; i++)
			//		{
			//			var s = new InfoAnalyzerState
			//			{
			//				Depth = node.Depth + 1,
			//				Path = dirs[i],
			//				HandleExceptionCallback = ias.HandleExceptionCallback
			//			};
			//			_tasks.Add(Task.Factory.StartNew(new InfoAnalyzer(TaskCompletedCallback).Analyze, s)
			//				.ContinueWith(ias.HandleExceptionCallback, TaskContinuationOptions.OnlyOnFaulted));
			//		}

			//		Task.WaitAll(_tasks.ToArray());
			//	}
			//}
			//else
			//{
			foreach (var s in dirs.Select(dir => new InfoAnalyzerState
			{
				Depth = node.Depth + 1,
				Path = dir,
				Parent = node,
				HandleExceptionCallback = ias.HandleExceptionCallback
			}))
			{
				_tasks.Add(Task.Factory.StartNew(new InfoAnalyzer(TaskCompletedCallback).Analyze, s)
					.ContinueWith(ias.HandleExceptionCallback, TaskContinuationOptions.OnlyOnFaulted));
			}

			var r = Task<long>.Factory.StartNew(CalculateFilesSize, Directory.GetFiles(node.Path));
			_tasks.Add(r);

			Task.WaitAll(_tasks.ToArray());

			node.SizeInByte += r.Result;
			//}

			TaskCompletedCallback(node);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static long CalculateFilesSize(object state)
		{
			var files = state as IEnumerable<string>;
			return files == null
				? 0
				: (from t in files where !FileHelper.IsHardLink(t) select FileHelper.GetFileSizeOnDisk(t)).Where(s => s >= 0).Sum();
		}
	}

	public class InfoAnalyzerState
	{
		public string Path { get; set; }

		public int Depth { get; set; }

		public FileSystemNode Parent { get; set; }

		public Action<Task> HandleExceptionCallback { get; set; }
	}
}
