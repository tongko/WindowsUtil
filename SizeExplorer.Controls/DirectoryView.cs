using System;
using System.Windows.Forms;

namespace SizeExplorer.Controls
{
	public delegate void UpdateViewItemCallback(ListViewItem item, ISizeNode node);

	public partial class DirectoryView : UserControl
	{
		private const int MinWidth = 430;
		private const int MinColFolderWidth = 260;

		public DirectoryView()
		{
			InitializeComponent();
		}

		public ISizeNode SizeNode { get; set; }

		public void Clear()
		{
			listView1.Items.Clear();
		}

		public void Bind(ISizeNode node)
		{
			var item = new ListViewItem {Text = ".."};
			item.SubItems.Add("");
			item.SubItems.Add("");
			item.ToolTipText = "Parent Directory";
			listView1.Items.Add(item);

			foreach (var child in node.Children)
			{
				item = new ListViewItem {Text = child.Name};
				item.SubItems.Add(CommonFunction.ConvertByte(child.Size));
				item.SubItems.Add(string.Format("{0:P}", child.Percentage));

				child.BindItem = item;
				listView1.Items.Add(item);
			}
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			if (Width < MinWidth)
				Width = MinWidth;

			if (_colFolder.Width < MinColFolderWidth)
				_colFolder.Width = MinColFolderWidth;

			base.OnSizeChanged(e);
		}

		private void DoubleClicked(object sender, EventArgs e)
		{
			if (listView1.SelectedItems.Count > 1) return;

			var item = listView1.SelectedItems[0];
			if (item.Text == "..")

		}

		private void BindNewObject()
		{
			
		}
	}
}
