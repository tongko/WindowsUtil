using System.Collections.Generic;
using System.IO;

namespace SizeExplorer.Model
{
	public class SizeNode
	{
		private static readonly object SyncObject = new object();
		private readonly FileSystemInfo _info;

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = (_info != null ? _info.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (Parent != null ? Parent.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (Children != null ? Children.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ Size.GetHashCode();
				return hashCode;
			}
		}

		public SizeNode(FileSystemInfo info)
		{
			_info = info;
			Size = 0;
		}

		public object SyncRoot { get { return SyncObject; } }

		public string Path
		{
			get { return _info.FullName; }
		}

		public string Name
		{
			get { return _info.Name; }
		}

		public SizeNode Parent { get; set; }

		public List<SizeNode> Children { get; set; }

		public ulong Size { get; private set; }

		public void AddSize(ulong size)
		{
			lock (SyncObject)
			{
				Size += size;
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
			if (obj.GetType() != typeof(SizeNode)) return false;
			return Equals(obj as SizeNode);
		}


	}
}
