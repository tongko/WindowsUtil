using System;
using System.Windows.Forms;

namespace SizeExplorer.Controls
{
	public partial class DeviceView : UserControl
	{
		private DeviceViewItem _item;

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
			CustomInit();
		}

		protected void CustomInit()
		{
			Item = new DeviceViewItem
			{
				Anchor = AnchorStyles.Left | AnchorStyles.Right,
				Title = "Computer",
				Description = "My desktop computer.",
				LeftIndent = 0,
				Left = Margin.Left,
				Top = 0//Margin.Top
			};
			Controls.Add(Item);
		}

		public DeviceViewItem Item
		{
			get { return _item; }
			set
			{
				_item = value;
				Controls.Add(_item);
			}
		}

		public int IndentWidth { get; set; }

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
	}
}
