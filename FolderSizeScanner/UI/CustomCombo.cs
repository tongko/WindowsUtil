using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FolderSizeScanner.UI
{
	public partial class CustomCombo : ComboBox
	{
		public CustomCombo()
		{
			InitializeComponent();
		}

		protected override void OnMeasureItem(MeasureItemEventArgs e)
		{
			if (e.Index < 0) return;

			var g = e.Graphics;

			var vertScrollBarWidth =
				(Items.Count > MaxDropDownItems)
					? SystemInformation.VerticalScrollBarWidth
					: 0;
			if (e.Index >= 0)
			{
				var newWidth = (int)g.MeasureString(GetItemString(GetItem(e.Index)), Font).Width + vertScrollBarWidth;
				if (DropDownWidth < newWidth)
				{
					DropDownWidth = newWidth;
					e.ItemWidth = newWidth;
				}
			}
		}

		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			if (e.Index < 0) return;

			base.OnDrawItem(e);
			var g = e.Graphics;

			var size = 0f;

			//	draw item background
			e.DrawBackground();

			var s = GetItemString(GetItem(e.Index));
			var b = new SolidBrush(e.ForeColor);
			e.Graphics.DrawString(s, Font, b, e.Bounds);
			e.DrawFocusRectangle();
		}

		protected virtual DriveInfo GetItem(int index)
		{
			if (index < 0) return null;

			var item = Items[index] as DriveInfo;
			if (item == null)
				throw new InvalidCastException(string.Format("Item at index {0} is not DriveInfo class.", index));

			return item;
		}

		private string GetItemString(DriveInfo di)
		{
			string type;
			switch (di.DriveType)
			{
				case DriveType.Removable:
					type = "Removable media";
					break;
				case DriveType.Fixed:
					type = "Fixed drive";
					break;
				case DriveType.Ram:
					type = "RAM Disk";
					break;
				default:
					type = "Not supported";
					break;
			}

			return di.Name.Substring(0, 1) + type.PadLeft(20);
		}
	}
}
