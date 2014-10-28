using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SizeExplorer.Controls
{
	public partial class DeviceViewItem : UserControl
	{
		public event EventHandler<LeftIndentedEventArgs> ImageLeftIndented;
		public event EventHandler<SelectedEventArgs> ItemSelected;

		private Rectangle _titleBounds;
		private Rectangle _descBounds;
		private Rectangle _imageBounds;
		private Image _image;
		private int _imgLeftIndentation;
		private List<DeviceViewItem> _items = new List<DeviceViewItem>();
		private bool _highlighted;
		private string _desc;

		public DeviceViewItem()
		{
			SetStyle(ControlStyles.SupportsTransparentBackColor
				| ControlStyles.AllPaintingInWmPaint
				| ControlStyles.DoubleBuffer
				| ControlStyles.FixedHeight
				| ControlStyles.ResizeRedraw
				| ControlStyles.OptimizedDoubleBuffer
				//				| ControlStyles.Selectable
				| ControlStyles.UserPaint, true);
			InitializeComponent();
			_imageBounds = new Rectangle(new Point(3, 3), new Size(64, 64));
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

		public string Title { get; set; }

		public string Description { get; set; }

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

			_imageBounds.X = Margin.Left + indentation;
			_imageBounds.Y = Margin.Top;
			_titleBounds.X = _descBounds.X = _imageBounds.Left + _imageBounds.Width;
			_titleBounds.Height = _descBounds.Height = 30;
			_titleBounds.Y = _imageBounds.Y + 2;
			_descBounds.Y = _titleBounds.Y + _titleBounds.Height;
			_titleBounds.Width = _descBounds.Width = Width - Margin.Left - Margin.Right - _imageBounds.Width;

			var ico = GetResourceIconBySize("SizeExplorer.Controls.Resources." +
				(indentation == 0 ? "Computer.ico" : "Device.ico"), _imageBounds.Size);
			if (ico != null)
				_image = Bitmap.FromHicon(ico.Handle);
		}

		protected virtual void OnItemSelected(SelectedEventArgs e)
		{
			if (ItemSelected != null)
				ItemSelected(this, e);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseClick"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.Windows.Forms.MouseEventArgs"/> that contains the event data. </param>
		protected override void OnMouseClick(MouseEventArgs e)
		{
			Selected = e.Button == MouseButtons.Left;

			base.OnMouseClick(e);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseEnter"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data. </param>
		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
			_highlighted = true;
			Invalidate(ClientRectangle);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseLeave"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data. </param>
		protected override void OnMouseLeave(EventArgs e)
		{
			if (ClientRectangle.Contains(PointToClient(MousePosition)))
				return;

			base.OnMouseLeave(e);
			_highlighted = false;
			Invalidate(ClientRectangle);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.GotFocus"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data. </param>
		protected override void OnGotFocus(EventArgs e)
		{
			Selected = true;
			base.OnGotFocus(e);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.LostFocus"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data. </param>
		protected override void OnLostFocus(EventArgs e)
		{
			Selected = false;
			base.OnLostFocus(e);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Paint"/> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"/> that contains the event data. </param>
		protected override void OnPaint(PaintEventArgs e)
		{
			//base.OnPaint(e);
			var g = e.Graphics;

			if (_highlighted && Selected)
			{
				var cs = new ColorScheme
				{
					BorderColor = Color.FromArgb(125, 162, 206),
					CornerColor = Color.FromArgb(174, 197, 224),
					GradientStart = Color.FromArgb(220, 235, 252),
					GradientEnd = Color.FromArgb(193, 219, 252)
				};
				PaintHighlighted(g, cs);
			}
			else if (Selected)
			{
				var cs = new ColorScheme
				{
					BorderColor = Color.FromArgb(217, 217, 217),
					CornerColor = Color.FromArgb(239, 239, 239),
					GradientStart = Color.FromArgb(248, 248, 248),
					GradientEnd = Color.FromArgb(229, 229, 229)
				};
				PaintHighlighted(g, cs);
			}
			else if (_highlighted)
			{
				var cs = new ColorScheme
				{
					BorderColor = Color.FromArgb(184, 214, 251),
					CornerColor = Color.FromArgb(214, 231, 252),
					GradientStart = Color.FromArgb(250, 251, 253),
					GradientEnd = Color.FromArgb(235, 243, 253)
				};
				PaintHighlighted(g, cs);
			}
			else
				EraseBackground(g);

			g.DrawImage(_image, _imageBounds, 0, 0, _image.Width, _image.Height, GraphicsUnit.Pixel);

			var sf = new StringFormat
			{
				Alignment = StringAlignment.Near,
				LineAlignment = StringAlignment.Center,
				Trimming = StringTrimming.EllipsisCharacter
			};

			var br = new SolidBrush(ForeColor);
			var tFont = new Font("Segoe UI", 11.25f, FontStyle.Bold);
			g.DrawString(Title, tFont, br, _titleBounds, sf);
			tFont = new Font("Trebuchet MS", 9.75f, FontStyle.Regular);
			g.DrawString(Description, tFont, br, _descBounds, sf);
		}

		private void EraseBackground(Graphics g)
		{
			var b = new SolidBrush(BackColor);
			g.FillRectangle(b, ClientRectangle);
		}

		private void PaintHighlighted(Graphics g, ColorScheme scheme)
		{
			var cp = new Pen(scheme.CornerColor);
			var p = new Pen(scheme.BorderColor);
			var cr = ClientRectangle;

			var l = cr.Left;
			var t = cr.Top;
			var r = cr.Right - 1;
			var b = cr.Bottom - 1;

			var pt1 = new Point(l + 2, t);
			var pt2 = new Point(r - 2, t);
			g.DrawLine(p, pt1, pt2);
			pt1 = new Point(r, t + 2);
			pt2 = new Point(r, b - 2);
			g.DrawLine(p, pt1, pt2);
			pt1 = new Point(r - 2, b);
			pt2 = new Point(l + 2, b);
			g.DrawLine(p, pt1, pt2);
			pt1 = new Point(l, b - 2);
			pt2 = new Point(l, t + 2);
			g.DrawLine(p, pt1, pt2);

			// Left Top Corner
			pt1 = new Point(l, t + 1); // {0, 1}
			pt2 = new Point(l + 1, t + 1); // {1, 1}
			g.DrawLine(cp, pt1, pt2);
			pt1 = new Point(l + 1, t); // {1, 0}
			g.DrawLine(cp, pt2, pt1);

			// Right Top Corner
			pt1 = new Point(r - 1, t);
			pt2 = new Point(r - 1, t + 1);
			g.DrawLine(cp, pt1, pt2);
			pt1 = new Point(r, t + 1);
			g.DrawLine(cp, pt2, pt1);

			// Right Bottom Corner
			pt1 = new Point(r, b - 1);
			pt2 = new Point(r - 1, b - 1);
			g.DrawLine(cp, pt1, pt2);
			pt1 = new Point(r - 1, b);
			g.DrawLine(cp, pt2, pt1);

			// Left Bottom Corner
			pt1 = new Point(l + 1, b);
			pt2 = new Point(l + 1, b - 1);
			g.DrawLine(cp, pt1, pt2);
			pt1 = new Point(l, b - 1);
			g.DrawLine(cp, pt2, pt1);

			var bounds = new Rectangle(ClientRectangle.X + 2, ClientRectangle.Y + 2, ClientRectangle.Width - 4,
				ClientRectangle.Height - 4);
			var gb = new LinearGradientBrush(bounds, scheme.GradientStart, scheme.GradientEnd, LinearGradientMode.Vertical);
			g.FillRectangle(gb, bounds);
			gb.Dispose();
		}

		protected Icon GetResourceIconBySize(string name, Size size)
		{
			Icon icon;
			using (var s = EmbededResources.GetResourceStream(name))
			{
				icon = new Icon(s, size);
			}

			return icon;
		}

		struct ColorScheme
		{
			public Color BorderColor;
			public Color CornerColor;
			public Color GradientStart;
			public Color GradientEnd;
		}
	}

	public class LeftIndentedEventArgs : EventArgs
	{
		public int Indentation { get; set; }
	}

	public class SelectedEventArgs : EventArgs
	{
		public int SelectedIndex { get; set; }
	}
}
