using System.IO;
using System.Linq;
using SizeExplorer.Model;

namespace SizeExplorer.Core
{
	public static class FileSizeHelper
	{
		public static void CalculateSize(SizeNode node)
		{
			var dirs = Directory.GetDirectories(node.Path).ToArray();
			for (var i = 0; i < dirs.Length; i++)
			{
				var dNode = new SizeNode(new DirectoryInfo(dirs[i]));
				CalculateSize(node);
				
			}
		}

		public static void AddChildNode(SizeNode parent, SizeNode child)
		{
			parent.Children.Add(child);
			parent.Size += child.Size;
		}
	}
}
