using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FolderSizeScanner.UI
{
	public partial class CustomCombo : ComboBox
	{
		private int _maxItemWidth;

		public CustomCombo()
		{
			InitializeComponent();
		}

		protected override void OnDropDown(EventArgs e)
		{
			var g = CreateGraphics();
			var vertScrollBarWidth =
				(Items.Count > MaxDropDownItems)
					? SystemInformation.VerticalScrollBarWidth
					: 0;

			for (int i = 0; i < Items.Count; i++)
			{
				var newWidth = (int)g.MeasureString(GetItemString(GetItem(i)), Font).Width + vertScrollBarWidth;
				if (DropDownWidth < newWidth)
					DropDownWidth = newWidth;
			}

			base.OnDropDown(e);
		}

		protected override void OnMeasureItem(MeasureItemEventArgs e)
		{
			if (e.Index < 0) return;

			if (e.ItemWidth < DropDownWidth)
				e.ItemWidth = DropDownWidth;
		}

		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			if (e.Index < 0) return;
	
			var g = e.Graphics;
			var b = new SolidBrush(e.ForeColor);
			var di = GetItem(e.Index);

			//	draw item background
			e.DrawBackground();

			if ((e.State & DrawItemState.ComboBoxEdit) == DrawItemState.ComboBoxEdit)
			{
				g.DrawString(di.Name.Substring(0, 1), Font, b, e.Bounds);
				e.DrawFocusRectangle();
				return;
			}

			var bb = new SolidBrush(Color.Gray);
			var height = e.Bounds.Height - 4;
			var rect = new Rectangle(e.Bounds.Left + 2, e.Bounds.Top + 2, height, height);
			g.FillRectangle(bb, rect);

			var width = e.Bounds.Left + 4F + height;
			var newRect = new RectangleF(width, e.Bounds.Top + 2, e.Bounds.Width - width - 4, height); 

			var sf = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center };
			e.Graphics.DrawString(di.Name.Substring(0, 1), Font, b, newRect, sf);

			sf.Alignment = StringAlignment.Far;
			e.Graphics.DrawString(GetNameByType(di.DriveType), Font, b, newRect, sf);

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

		protected override void OnSelectedItemChanged(EventArgs e)
		{
			base.OnSelectedItemChanged(e);
			var di = GetItem(SelectedIndex);
			Text = di.Name.Substring(0, 1);
		}

		private string GetItemString(DriveInfo di)
		{
			string type = GetNameByType(di.DriveType);

			return di.Name.Substring(0, 1) + type.PadLeft(30);
		}

		private string GetNameByType(DriveType dt)
		{
			string type;
			switch (dt)
			{
				case DriveType.Removable:
					type = "Removable media";
					break;
				case DriveType.Fixed:
					type = "Fixed disk drive";
					break;
				case DriveType.Ram:
					type = "RAM Disk";
					break;
				default:
					type = "Not supported";
					break;
			}

			return type;
		}
	}
}
