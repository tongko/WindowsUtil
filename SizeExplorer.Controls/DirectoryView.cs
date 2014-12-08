using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SizeExplorer.Controls
{
	public delegate void UpdateViewItemCallback(ListViewItem item, ISizeNode node);

	public partial class DirectoryView : UserControl
	{
		private const int MinWidth = 430;
		private const int MinColFolderWidth = 260;
		private readonly Dictionary<string, ISizeNode> _map;
		//private AnimatedCircle animCircle;

		public DirectoryView()
		{
			InitializeComponent();

			_map = new Dictionary<string, ISizeNode>();
		}

		public ISizeNode SizeNode { get; private set; }

		public void Clear()
		{
			listView1.Items.Clear();
		}

		public void Bind(ISizeNode node)
		{
			_map.Clear();
			listView1.Items.Clear();

			SizeNode = node;
			var item = new ListViewItem { Text = "..", ImageIndex = 0, ToolTipText = "Parent Directory" };
			item.SubItems.Add("");
			item.SubItems.Add("");
			listView1.Items.Add(item);
			_map.Add(item.Text, node.Parent);

			foreach (var child in node.Children)
			{
				item = new ListViewItem { Text = child.Name, ImageIndex = child.IsFile ? 1 : 0, ToolTipText = child.Path };
				item.SubItems.Add(CommonFunction.ConvertByte(child.Size));
				item.SubItems.Add(string.Format("{0:0.00}", child.Percentage));

				child.BindItem = item;
				listView1.Items.Add(item);
				_map.Add(item.Text, child);
			}
		}

		public void BindChild(ISizeNode node)
		{
			if (_map.ContainsKey(node.Name)) return;

			var item = new ListViewItem { Text = node.Name, ImageIndex = node.IsFile ? 1 : 0, ToolTipText = node.Path };
			item.SubItems.Add(CommonFunction.ConvertByte(node.Size));
			item.SubItems.Add(string.Format("{0:0.00}", node.Percentage));

			node.BindItem = item;
			listView1.Items.Add(item);
			_map.Add(item.Text, node);
		}

		public void SetState(bool busy)
		{
			if (animCircle.InvokeRequired)
			{
				animCircle.Invoke(new Action<bool>(SetState), busy);
			}
			else
			{
				if (busy)
				{
					animCircle.Visible = true;
					animCircle.Start();
				}
				else
				{
					animCircle.Stop();
					animCircle.Visible = false;
				}
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
			ISizeNode node;

			if (!_map.TryGetValue(item.Text, out node) || node == null) return;
			Bind(node);
		}
	}
}
