using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SizeExplorer.Controls
{
	public partial class DeviceView : UserControl
	{
		private List<DeviceViewItem> _items;
		private int _selectedIndex;

		public DeviceView()
		{
			SetStyle(//ControlStyles.AllPaintingInWmPaint
					 ControlStyles.CacheText
					 | ControlStyles.ResizeRedraw
					 | ControlStyles.SupportsTransparentBackColor
					 | ControlStyles.Selectable
				//| ControlStyles.UserPaint
				, true);
			InitializeComponent();
			//CustomInit();
		}

		public void Add(string key, string title, string description, int indentLevel)
		{
			Add(key, new DeviceViewItem { Title = title, Description = description, LeftIndent = indentLevel * 20 });
		}

		public void Add(string key, DeviceViewItem item)
		{
			while (Controls.ContainsKey(key))
			{
				key += "X";
			}

			item.Name = key;
			item.Anchor = item.LeftIndent == 0 ? AnchorStyles.Left | AnchorStyles.Right : AnchorStyles.Left;
			item.Left = Margin.Left;
			item.Width = Width - Margin.Right - Margin.Left;
			if (Controls.Count > 0)
			{
				var dvi = Controls[Controls.Count - 1] as DeviceViewItem;
				item.Top = dvi.Top + dvi.Height - 1;
			}
			else
			{
				item.Top = Margin.Top;
			}
			item.MouseClick += ItemSelected;

			Controls.Add(item);
			item.Invalidate();
		}

		public int IndentWidth { get; set; }

		public int SelectedIndex
		{
			get { return _selectedIndex; }
			set
			{
				if (_selectedIndex != value)
				{
					var dvi = Controls[_selectedIndex] as DeviceViewItem;
					if (dvi != null)
					{
						dvi.Selected = false;
						dvi.Invalidate();
					}
					_selectedIndex = value;
					dvi = Controls[_selectedIndex] as DeviceViewItem;
					if (dvi != null)
					{
						dvi.Selected = true;
						dvi.Invalidate();
					}
				}
			}
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.SizeChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data. </param>
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);

			foreach (var control in Controls)
			{
				var c = control as DeviceViewItem;
				if (c != null)
					c.UpdateLocation();
			}
		}

		private void ItemSelected(object sender, MouseEventArgs me)
		{

		}
	}
}
