using Delimon.Win32.IO;
using SizeExplorer.Controls;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace SizeExplorer.Model
{
	[DebuggerDisplay("Name = {Name}")]
	public class SizeNode : ISizeNode
	{
		private static readonly object SyncObject = new object();
		private readonly string _fullPath;
		private readonly string _name;

		public SizeNode(FileSystemInfo info)
			: this(info.FullName, info.Name)
		{
		}

		public SizeNode(string fullPath, string name)
		{
			_fullPath = fullPath;
			_name = name;
			Size = 0;
			Children = new List<ISizeNode>();
		}

		public object SyncRoot
		{
			get { return SyncObject; }
		}

		public ListViewItem BindItem { get; set; }

		public UpdateViewItemCallback UpdateCallback { get; set; }

		public string Path
		{
			get { return _fullPath; }
		}

		public string Name
		{
			get { return _name; }
		}

		public bool IsFile { get; set; }

		public ISizeNode Parent { get; set; }

		public List<ISizeNode> Children { get; set; }

		public ulong Size { get; private set; }

		public double Percentage
		{
			get
			{
				if (Parent.Size == 0) return 0;
				return Size / (double)Parent.Size * 100.00;
			}
		}

		public void AddSize(ulong size)
		{
			lock (SyncObject)
			{
				Size += size;
				if (UpdateCallback != null)
					UpdateCallback(BindItem, this);
			}
		}

		public override int GetHashCode()
		{
			return _fullPath.GetHashCode();
		}

		protected virtual bool Equals(SizeNode node)
		{
			return Equals(_fullPath, node._fullPath);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != typeof(SizeNode)) return false;
			return Equals(obj as SizeNode);
		}
	}
}