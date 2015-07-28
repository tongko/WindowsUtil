using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FolderSizeScanner.Core
{
	public delegate void ReportProgressCallback(string action, string message, int progress);

	class AnalyzeDriveJob
	{
		private const string ActionText = "Analyzing";
		private readonly DirectoryInfo _driveInfo;
		private readonly Stopwatch _stopwatch;

		public AnalyzeDriveJob(string drive)
		{
			if (string.IsNullOrWhiteSpace(drive))
				throw new ArgumentNullException("drive");

			drive = drive.EndsWith("\\") ? drive : drive + "\\";
			_driveInfo = new DirectoryInfo(drive);
			_stopwatch = new Stopwatch();
		}

		public event EventHandler JobStart;

		public event EventHandler JobCanceled;

		public event EventHandler<ulong> JobCompleted;

		public event EventHandler<Exception> JobFaulty;

		public ReportProgressCallback ReportProgress { get; set; }

		public TimeSpan Ellapsed { get { return _stopwatch.Elapsed; } }

		public Task<ISizeNode> StartAsync(CancellationToken cToken)
		{
			var task = Task.Run<ISizeNode>(() =>
			{
				try
				{
					_stopwatch.Start();
					OnJobStart();
					Thread.Sleep(3000);
					var node = new SizeNode(_driveInfo.FullName);
					var fsoCount = Build(node, cToken);

					OnJobCompleted(fsoCount);
					_stopwatch.Stop();
					return node;
				}
				catch (Exception ex)
				{
					if (!(ex is OperationCanceledException))
						OnJobFaulty(ex);

					throw;
				}
			}, cToken);

			return task;
		}

		protected void DoReport(string action, string message)
		{
			if (ReportProgress != null)
				ReportProgress(action, message, 0);
		}

		protected virtual void OnJobFaulty(Exception ex)
		{
			var handler = JobFaulty;
			if (handler != null)
				handler(this, ex);
		}

		protected virtual void OnJobCanceled()
		{
			var handler = JobCanceled;
			if (handler == null) return;

			DoReport("Job canceled by user.", string.Empty);
			handler(this, EventArgs.Empty);
		}

		protected virtual void OnJobCompleted(ulong fsoCount)
		{
			DoReport(ActionText, "Job completed.");
			var handler = JobCompleted;
			if (handler != null)
				handler(this, fsoCount);
		}

		protected virtual void OnJobStart()
		{
			DoReport(ActionText, "Job started...");
			var handler = JobStart;
			if (handler != null)
				handler(this, EventArgs.Empty);
		}

		private ulong Build(ISizeNode node, CancellationToken token)
		{
			const string encounterText = "Scanning ";
			ulong fsoCount = 1;
			var enumerators = new Stack<FsoEnumerator>();
			var nodes = new Stack<ISizeNode>();
			enumerators.Push(new FsoEnumerator(node.FullPath));
			nodes.Push(node);

			while (enumerators.Count > 0)
			{
				if (token.IsCancellationRequested)
				{
					OnJobCanceled();
					token.ThrowIfCancellationRequested();
				}

				var e = enumerators.Pop();
				var n = nodes.Pop();

				while ((e.MoveNext()))
				{
					var dir = e.Current;
					if (dir.Name == "." || dir.Name == "..") continue;

					var fp = Path.Combine(n.FullPath, dir.Name);
					if (Fso.IsHardLink(fp))
					{
						continue;
					}

					DoReport(encounterText, fp);
					var newNode = new SizeNode(fp);
					if (!dir.IsDirectory)
					{
						newNode.SetIsFile();
						newNode.AddSizeOnDisk(dir.Size);
					}
					n.AddChildNode(newNode);

					if (!dir.IsDirectory) continue;

					fsoCount++;
					enumerators.Push(e);
					nodes.Push(n);
					enumerators.Push(new FsoEnumerator(newNode.FullPath));
					nodes.Push(newNode);
					break;
				}
			}

			return fsoCount;
		}
	}
}
