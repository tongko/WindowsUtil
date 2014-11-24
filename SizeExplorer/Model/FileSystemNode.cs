using System;
using System.Collections.Generic;

namespace SizeExplorer.Model
{
	public class FileSystemNode
	{
		private static readonly object SyncObject = new Object();
		private long _size = 0;

		public FileSystemNode()
		{
			Children = new List<FileSystemNode>();
		}

		public FileSystemNode Parent { get; set; }

		public List<FileSystemNode> Children { get; set; }

		public int Depth { get; set; }

		public string Path { get; set; }

		public long SizeInByte
		{
			get { return _size; }
			set
			{
				lock (SyncObject)
				{
					_size = value;
				}
			}
		}

		public double LocalPercentage { get; set; }

		public double Percentage { get; set; }

		public CompletedHandler ProcessCompletedCallback { get; set; }
	}
}
