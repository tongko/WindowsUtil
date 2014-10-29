using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace SizeExplorer.Controls
{
	public partial class DeviceViewItem : UserControl
	{
		private Rectangle _descBounds;
		private bool _highlighted;
		private Rectangle _imageBounds;
		private int _imgLeftIndentation;
		private bool _selected;
		private Rectangle _titleBounds;

		#region Events

		public event EventHandler<LeftIndentedEventArgs> ImageLeftIndented;

		#endregion

		#region Properties

		public string Description { get; set; }
		public DeviceView DeviceView { get; set; }
		public object UserData { get; set; }
		public Image Image { get; set; }
		internal int Index { get; set; }

		public int LeftIndent
		{
			get { return _imgLeftIndentation; }
			set
			{
				if (value < 0) value = 0;

				_imgLeftIndentation = value;
				UpdateBounds(_imgLeftIndentation);
				OnImageLeftIndented(value);
			}
		}

		public bool Selected
		{
			get { return _selected; }
			set
			{
				_selected = value;
				Invalidate();
			}
		}

		public string Title { get; set; }

		#endregion

		#region Constructors

		public DeviceViewItem()
		{
			SetStyle(
				//ControlStyles.SupportsTransparentBackColor
				ControlStyles.AllPaintingInWmPaint
				| ControlStyles.DoubleBuffer
				| ControlStyles.FixedHeight
				| ControlStyles.ResizeRedraw
				| ControlStyles.OptimizedDoubleBuffer
				//				| ControlStyles.Selectable
				| ControlStyles.UserPaint, true);
			InitializeComponent();
			_imageBounds = new Rectangle(new Point(3, 3), new Size(64, 64));
			UpdateBounds(_imgLeftIndentation);
		}

		#endregion

		#region Override Methods

		/// <summary>
		///     Raises the <see cref="E:System.Windows.Forms.Control.MouseClick" /> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data. </param>
		protected override void OnMouseClick(MouseEventArgs e)
		{
			Selected = e.Button == MouseButtons.Left;
			if (Selected)
				DeviceView.UpdateSelected(this);
			base.OnMouseClick(e);
		}

		/// <summary>
		///     Raises the <see cref="E:System.Windows.Forms.Control.MouseEnter" /> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
			SetHighlight(true);
		}

		/// <summary>
		///     Raises the <see cref="E:System.Windows.Forms.Control.MouseLeave" /> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			SetHighlight(false);
		}

		/// <summary>
		///     Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data. </param>
		protected override void OnPaint(PaintEventArgs e)
		{
			//base.OnPaint(e);
			Graphics g = e.Graphics;

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

			g.InterpolationMode = InterpolationMode.HighQualityBicubic;
			g.DrawImage(Image, _imageBounds);

			var sf = new StringFormat
			{
				Alignment = StringAlignment.Near,
				LineAlignment = StringAlignment.Center,
				Trimming = StringTrimming.EllipsisCharacter,
				FormatFlags = StringFormatFlags.NoWrap
			};

			var br = new SolidBrush(ForeColor);
			var tFont = new Font("Segoe UI", 11.25f, FontStyle.Bold);
			g.DrawString(Title, tFont, br, _titleBounds, sf);
			tFont = new Font("Trebuchet MS", 9.75f, FontStyle.Regular);
			g.DrawString(Description, tFont, br, _descBounds, sf);
		}

		/// <summary>
		///     Raises the <see cref="E:System.Windows.Forms.Control.SizeChanged" /> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			UpdateBounds(LeftIndent);
			Invalidate();
		}

		#endregion

		#region Virtual Methods

		protected virtual void OnImageLeftIndented(int indentation)
		{
			if (ImageLeftIndented != null)
				ImageLeftIndented(this, new LeftIndentedEventArgs { Indentation = indentation });
		}

		#endregion

		#region Public Methods

		public void AddItem(DeviceViewItem item)
		{
			item.Parent = Parent;
		}

		public void UpdateLocation(DeviceViewItem previousItem)
		{
			Location = previousItem == null
				? new Point(Parent.Margin.Left, Parent.Margin.Top)
				: new Point(Parent.Margin.Left, previousItem.Top + previousItem.Height);
			Size = new Size(Parent.ClientSize.Width - Parent.Margin.Right - Parent.Margin.Left, Size.Height);
		}

		#endregion

		#region Protected Methods

		protected Image GetResourceImage(string name)
		{
			Image bmp;
			using (Stream s = EmbededResources.GetResourceStream(name))
			{
				bmp = Image.FromStream(s);
			}

			return bmp;
		}

		#endregion

		#region Private Methods

		private void EraseBackground(Graphics g)
		{
			var b = new SolidBrush(BackColor);
			g.FillRectangle(b, ClientRectangle);
		}

		private void PaintHighlighted(Graphics g, ColorScheme scheme)
		{
			var cp = new Pen(scheme.CornerColor);
			var p = new Pen(scheme.BorderColor);
			Rectangle cr = ClientRectangle;

			int l = cr.Left;
			int t = cr.Top;
			int r = cr.Right - 1;
			int b = cr.Bottom - 1;

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

			var bounds = new Rectangle(cr.X + 2, cr.Y + 2, cr.Width - 4, cr.Height - 4);
			var gb = new LinearGradientBrush(bounds, scheme.GradientStart, scheme.GradientEnd, LinearGradientMode.Vertical);
			g.FillRectangle(gb, bounds);
			gb.Dispose();
		}

		private void UpdateBounds(int indentation)
		{
			const int labelHeight = 20;

			if (indentation < 0) indentation = 0;

			_imageBounds.X = Margin.Left + indentation;
			_imageBounds.Y = (Height - _imageBounds.Height) / 2 + Margin.Top;

			_titleBounds.X = _descBounds.X = _imageBounds.Left + _imageBounds.Width + 10;
			_titleBounds.Height = _descBounds.Height = labelHeight;
			_titleBounds.Y = (Height - 40) / 2 + _imageBounds.Y;
			_descBounds.Y = _titleBounds.Y + labelHeight;
			_titleBounds.Width = _descBounds.Width = Width - Margin.Left
													 - Margin.Right - _imageBounds.Width - LeftIndent - 10;
		}

		#endregion

		internal void SetHighlight(bool highlight)
		{
			_highlighted = highlight;
			Invalidate();
		}

		#region Nested type: ColorScheme

		private struct ColorScheme
		{
			public Color BorderColor;
			public Color CornerColor;
			public Color GradientEnd;
			public Color GradientStart;
		}

		#endregion
	}

	public class LeftIndentedEventArgs : EventArgs
	{
		#region Properties

		public int Indentation { get; set; }

		#endregion
	}

	public class SelectedEventArgs : EventArgs
	{
		#region Properties

		public int SelectedIndex { get; set; }

		#endregion
	}
}