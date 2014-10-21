using System.Collections.Generic;

namespace SizeExplorer.Model
{
	public class FileSystemNode
	{
		public FileSystemNode Parent { get; set; }

		public IList<FileSystemNode> Children { get; set; }


	}
}
