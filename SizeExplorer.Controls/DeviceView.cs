using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SizeExplorer.Controls
{
	public partial class DeviceView : UserControl
	{
		private int _selectedIndex;

		#region Events

		public event EventHandler<ItemMouseClickEventArgs> ItemMouseClick;

		#endregion

		#region Properties

		public override Rectangle DisplayRectangle
		{
			get { return ClientRectangle; }
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

		#endregion

		#region Constructors

		public DeviceView()
		{
			SetStyle(ControlStyles.CacheText
					 | ControlStyles.ContainerControl
					 | ControlStyles.ResizeRedraw
					 | ControlStyles.SupportsTransparentBackColor
					 | ControlStyles.Selectable
				, true);
			InitializeComponent();
		}

		#endregion

		#region Override Methods

		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		protected override void OnResize(EventArgs e)
		{
			if (Size.Width <= 350) return;
			base.OnResize(e);
		}

		/// <summary>
		///     Raises the <see cref="E:System.Windows.Forms.Control.SizeChanged" /> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);

			DeviceViewItem previous = null;
			foreach (var c in Controls.OfType<DeviceViewItem>())
			{
				c.UpdateLocation(previous);
				previous = c;
			}
		}

		#endregion

		#region Virtual Methods

		protected virtual void OnItemMouseClick(DeviceViewItem item)
		{
			if (ItemMouseClick != null)
				ItemMouseClick(this, new ItemMouseClickEventArgs(item));
		}

		#endregion

		#region Public Methods

		public void Add(string key, string title, string description, int indentLevel, Image itemImage,
			object userData)
		{
			Add(key, new DeviceViewItem
			{
				UserData = userData,
				Title = title,
				Description = description,
				LeftIndent = indentLevel * 40,
				Image = itemImage
			});
		}

		public void Add(string key, DeviceViewItem item)
		{
			while (Controls.ContainsKey(key))
			{
				key += "X";
			}

			item.Name = key;
			item.Left = Margin.Left;
			item.Width = Width - Margin.Right - Margin.Left;
			if (Controls.Count > 0)
			{
				var i = Controls.Count - 1;
				var dvi = Controls[i] as DeviceViewItem;
				while (dvi == null)
				{
					if (i == 0)
						break;
					dvi = Controls[--i] as DeviceViewItem;
				}

				item.Top = dvi == null ? 0 : dvi.Top + dvi.Height;
			}
			else
			{
				item.Top = Margin.Top;
			}

			item.Index = Controls.Count;
			item.DeviceView = this;

			Controls.Add(item);
		}

		public void Clear()
		{
			foreach (var control in Controls)
			{
				if (control is DeviceViewItem)
					Controls.Remove(control as DeviceViewItem);
			}
		}

		#endregion

		internal void UpdateSelected(DeviceViewItem item)
		{
			OnItemMouseClick(item);

			for (int i = 0; i < Controls.Count; i++)
			{
				var itm = Controls[i] as DeviceViewItem;
				if (itm == null || item == itm || !itm.Selected) continue;

				itm.Selected = false;
				break;
			}
		}
	}

	public class ItemMouseClickEventArgs
	{
		#region Properties

		public DeviceViewItem Item { get; set; }

		#endregion

		#region Constructors

		public ItemMouseClickEventArgs(DeviceViewItem item)
		{
			Item = item;
		}

		#endregion
	}
}