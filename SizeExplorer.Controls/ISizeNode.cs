using System.Collections.Generic;
using System.Windows.Forms;

namespace SizeExplorer.Controls
{
	public interface ISizeNode
	{
		string Path { get; }

		string Name { get; }

		ISizeNode Parent { get; set; }

		List<ISizeNode> Children { get; set; }

		ulong Size { get; }

		double Percentage { get; }

		object SyncRoot { get; }

		void AddSize(ulong size);

		ListViewItem BindItem { get; set; }

		UpdateViewItemCallback UpdateCallback { get; set; }
	}
}