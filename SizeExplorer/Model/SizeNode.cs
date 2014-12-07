using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using SizeExplorer.Controls;

namespace SizeExplorer.Model
{
	public class SizeNode : ISizeNode
	{
		private static readonly object SyncObject = new object();
		private readonly FileSystemInfo _info;

		public SizeNode(FileSystemInfo info)
		{
			_info = info;
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
			get { return _info.FullName; }
		}

		public string Name
		{
			get { return _info.Name; }
		}

		public ISizeNode Parent { get; set; }

		public List<ISizeNode> Children { get; set; }

		public ulong Size { get; private set; }

		public double Percentage
		{
			get
			{
				if (Parent.Size == 0) return 0;
				return Size / (double) Parent.Size * 100.00;
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
			unchecked
			{
				int hashCode = (_info != null ? _info.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (Parent != null ? Parent.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (Children != null ? Children.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ Size.GetHashCode();
				return hashCode;
			}
		}

		protected virtual bool Equals(SizeNode node)
		{
			return Equals(_info, node._info);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != typeof (SizeNode)) return false;
			return Equals(obj as SizeNode);
		}
	}
}