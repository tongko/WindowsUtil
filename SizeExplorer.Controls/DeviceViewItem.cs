using SizeExplorer.Controls.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SizeExplorer.Controls
{
	public partial class DeviceViewItem : UserControl
	{
		public event EventHandler<LeftIndentedEventArgs> ImageLeftIndented;

		private int _imgLeftIndentation;
		private List<DeviceViewItem> _items = new List<DeviceViewItem>();

		public DeviceViewItem()
		{
			InitializeComponent();
			IndentImage(_imgLeftIndentation);
		}

		public int LeftIndent
		{
			get { return _imgLeftIndentation; }
			set
			{
				if (value < 0) value = 0;

				if (_imgLeftIndentation == value) return;

				_imgLeftIndentation = value;
				IndentImage(_imgLeftIndentation);
				OnImageLeftIndented(value);
			}
		}

		public string Title
		{
			get { return lblDesc.Title; }
			set { lblDesc.Title = value; }
		}

		public string Description
		{
			get { return lblDesc.Description; }
			set { lblDesc.Description = value; }
		}

		public IList<DeviceViewItem> Items
		{
			get { return _items; }
			protected set { _items = new List<DeviceViewItem>(value); }
		}

		public bool Selected { get; set; }

		public void AddItem(DeviceViewItem item)
		{
			item.Parent = Parent;
		}

		public void UpdateLocation()
		{
			Location = new Point(Parent.Margin.Left, Parent.Margin.Top);
			//Size = new Size(Parent.ClientSize.Width - (Parent.Margin.Right * 2), Size.Height);
		}

		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data. </param>
		protected override void OnParentChanged(EventArgs e)
		{
			base.OnParentChanged(e);

			foreach (var item in Items)
				item.Parent = Parent;
		}

		protected virtual void OnImageLeftIndented(int indentation)
		{
			if (ImageLeftIndented != null)
				ImageLeftIndented(this, new LeftIndentedEventArgs { Indentation = indentation });
		}

		private void IndentImage(int indentation)
		{
			if (indentation < 0) indentation = 0;

			pbIcon.Left = Margin.Left + indentation;
			lblDesc.Left = pbIcon.Left + pbIcon.Width;
			lblDesc.Width = Width - Margin.Right - lblDesc.Left;

			pbIcon.Image = Bitmap.FromHicon(indentation == 0 ? Resources.Computer.Handle : Resources.Device.Handle);
		}
	}

	public class LeftIndentedEventArgs : EventArgs
	{
		public int Indentation { get; set; }
	}
}
