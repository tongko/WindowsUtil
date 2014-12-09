using System.Collections.Generic;
using System.Windows.Forms;

namespace SizeExplorer.Controls
{
	public interface ISizeNode
	{
		#region Properties

		ListViewItem BindItem { get; set; }

		List<ISizeNode> Children { get; set; }

		bool IsFile { get; set; }

		string Name { get; }

		ISizeNode Parent { get; set; }

		string Path { get; }

		double Percentage { get; }

		ulong Size { get; }

		object SyncRoot { get; }

		UpdateViewItemCallback UpdateCallback { get; set; }

		#endregion


		#region Public Methods

		void AddSize(ulong size);

		#endregion
	}
}