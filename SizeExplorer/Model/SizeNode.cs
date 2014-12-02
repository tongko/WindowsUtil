using System.Collections.Generic;
using System.IO;

namespace SizeExplorer.Model
{
	public class SizeNode
	{
		private readonly FileSystemInfo _info;

		public SizeNode(FileSystemInfo info)
		{
			_info = info;
		}

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

		public ulong Size { get; set; }

		public virtual bool Equals(SizeNode node)
		{
			if (node == null) return false;
			return _info.Equals(node._info) && Parent.Equals()
		}

		public override bool Equals(object obj)
		{
			
		}
	}
}
