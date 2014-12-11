using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FolderSizeScanner.Core
{
	class AnalyzeDriveJob
	{
		private readonly DirectoryInfo _driveInfo;

		public AnalyzeDriveJob(string drive)
		{
			if (string.IsNullOrWhiteSpace(drive))
				throw new ArgumentNullException("drive");

			drive = drive.EndsWith("\\") ? drive : drive + "\\";
			_driveInfo = new DirectoryInfo(drive);
		}

		public event EventHandler JobStart;

		public event EventHandler<ulong> JobCompleted;

		public Task<ISizeNode> StartAsync()
		{
			var token = new CancellationToken();
			var task = Task<ISizeNode>.Factory.StartNew(() =>
			{
				OnJobStart();
				var node = new SizeNode(_driveInfo.FullName);
				var fsoCount = Build(node);

				OnJobCompleted(fsoCount);
				return node;
			}, token);

			return task;
		}

		protected virtual void OnJobCompleted(ulong fsoCount)
		{
			var handler = JobCompleted;
			if (handler != null)
				handler(this, fsoCount);
		}

		protected virtual void OnJobStart()
		{
			var handler = JobStart;
			if (handler != null)
				handler(this, EventArgs.Empty);
		}

		private static ulong Build(ISizeNode node)
		{
			ulong fsoCount = 1;
			var enumerators = new Stack<FsoEnumerator>();
			var nodes = new Stack<ISizeNode>();
			enumerators.Push(new FsoEnumerator(node.FullPath));
			nodes.Push(node);

			while (enumerators.Count > 0)
			{
				var e = enumerators.Pop();
				var n = nodes.Pop();

				while ((e.MoveNext()))
				{
					var dir = e.Current;
					if (dir.Name == "." || dir.Name == "..") continue;

					var fp = Path.Combine(n.FullPath, dir.Name);
					if (FsoHelper.IsHardLink(fp))
					{
						continue;
					}

					var newNode = new SizeNode(fp);
					if (!dir.IsDirectory)
						newNode.SetIsFile();
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
