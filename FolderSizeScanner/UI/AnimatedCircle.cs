using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace FolderSizeScanner.UI
{
	public partial class AnimatedCircle : UserControl
	{
		private int _roundTrip;
		private RectangleF _rcCircle;
		private RectangleF _rcShadowInner;
		private RectangleF _rcShadowOuter;
		private RectangleF _rcInner;
		private RectangleF _rcCircleInner;
		private GraphicsPath _innerPath;
		private PointF _center;
		private Bitmap _bitmap;
		private float _maxShadow;
		private float _increament;
		private bool _init;
		private bool _reverse;
		private int _outterOpaque;
		private float _factor;
		private float _innerStart;
		private readonly float _innerSweep;

		public AnimatedCircle()
		{
			_init = false;
			DebugMode = false;
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.DoubleBuffer
				| ControlStyles.UserPaint, true);
			Style = CircleStyle.Symmetry;
			_bitmap = new Bitmap(Width, Height);
			StartAngle = 0f;
			SweepAngle = 75f;
			_roundTrip = 0;
			_reverse = false;
			_outterOpaque = 0;
			_innerPath = new GraphicsPath();
			_innerStart = 0f;
			_innerSweep = 45f;

			InitializeComponent();
		}

		public float StartAngle { get; private set; }

		public float SweepAngle { get; private set; }

		public float ShadowWidth { get; set; }

		public bool DebugMode { get; set; }

		public CircleStyle Style { get; private set; }

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);

			CalculateBounds();
			_init = true;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			if (!_init) return;

			if (DesignMode)
			{
				var pen = new Pen(ForeColor, 2.5f);
				e.Graphics.DrawRectangle(pen, ClientRectangle);
			}
			else
			{
				var g = Graphics.FromImage(_bitmap);
				g.Clear(Color.Transparent);
				g.SmoothingMode = SmoothingMode.HighQuality;
				var r = (Width / 2f) / 2f;

				_rcShadowOuter = new RectangleF(
					_center.X - r - ShadowWidth - 1,
					_center.Y - r - ShadowWidth - 1,
					(r + 1 + ShadowWidth) * 2,
					(r + 1 + ShadowWidth) * 2);

				DrawShadow(g);
				DrawCircle(g);
				g.Dispose();

				e.Graphics.DrawImage(_bitmap, ClientRectangle, ClientRectangle, GraphicsUnit.Pixel);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void DrawShadow(Graphics g)
		{
			using (var gp = new GraphicsPath())
			{
				gp.AddEllipse(_rcShadowOuter);
				gp.AddEllipse(_rcShadowInner);

				using (var brush = new PathGradientBrush(gp))
				{
					brush.WrapMode = WrapMode.Clamp;
					var blend = new ColorBlend(4)
					{
						Colors = new[]
					    {
						    Color.Transparent,
							Color.FromArgb(40, ForeColor),
						    Color.FromArgb(90, ForeColor),
							Color.FromArgb(140, ForeColor),
							Color.FromArgb(190, ForeColor),
						    Color.FromArgb(230, ForeColor),
						    Color.FromArgb(230, ForeColor)
					    },
						Positions = new[] { 0f, .13f, .22f, .31f, .4f, .499f, 1f }
					};
					brush.InterpolationColors = blend;

					g.FillPath(brush, gp);
				}

				using (var brush = new PathGradientBrush(_innerPath))
				{
					brush.WrapMode = WrapMode.Clamp;
					var blend = new ColorBlend(3)
					{
						Colors = new[]
						{
							Color.Transparent,
							Color.FromArgb(115, ForeColor),
							Color.FromArgb(230, ForeColor)
						},
						Positions = new[] { 0f, .25f, 1f }
					};
					brush.InterpolationColors = blend;

					g.FillPath(brush, _innerPath);
				}
			}

			if (DebugMode)
			{
				var pen = new Pen(Color.Red, 1);
				g.DrawEllipse(pen, _rcCircle);
				pen = new Pen(Color.Green, 1);
				g.DrawEllipse(pen, _rcShadowInner);
				pen = new Pen(Color.Blue, 1);
				g.DrawEllipse(pen, _rcShadowOuter);
				pen = new Pen(Color.White, 1);
				g.DrawRectangle(pen, Bounds.Left, Bounds.Top, Bounds.Width, Bounds.Height);
				g.DrawEllipse(pen, _rcInner);
				g.DrawEllipse(pen, _rcCircleInner);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void DrawCircle(Graphics g)
		{
			var pen = new Pen(Color.FromArgb(_outterOpaque, ForeColor), 5);
			switch (Style)
			{
				case CircleStyle.Full:
					g.DrawEllipse(pen, _rcCircle);
					break;
				case CircleStyle.Semi:
					g.DrawArc(pen, _rcCircle, StartAngle, 180f);	// Ignore SweepAngle
					break;
				case CircleStyle.Quater:
					g.DrawArc(pen, _rcCircle, StartAngle, 90f);		// Ignore SwwepAngle
					break;
				case CircleStyle.Symmetry:
					g.DrawArc(pen, _rcCircle, StartAngle, SweepAngle);
					g.DrawArc(pen, _rcCircle, StartAngle + 180, SweepAngle);

					pen = new Pen(Color.FromArgb(230, ForeColor), 4);
					var rc = new RectangleF(_rcCircleInner.Location, _rcCircleInner.Size);
					rc.Inflate(-3, -3);
					g.DrawArc(pen, rc, _innerStart, _innerSweep);
					g.DrawArc(pen, rc, _innerStart + 180, _innerSweep);
					break;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Start()
		{
			timer1.Enabled = true;
			//_timer.Change(0, _interval);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Stop()
		{
			timer1.Enabled = false;
			//_timer.Change(Timeout.Infinite, Timeout.Infinite);
		}

		private void TimerTick(object sender, EventArgs e)
		{
			Stop();

			//	Beginning of loop
			if (_roundTrip == 0)
			{
				_outterOpaque = 0;
				_factor = 1;
				StartAngle = 0;
			}
			else
			{
				_outterOpaque += 20;
				if (_outterOpaque > 255)
					_outterOpaque = 255;
				_factor += 0.4f;
			}

			StartAngle -= 3.6f * _factor;

			if (_reverse)
			{
				ShadowWidth -= _increament * 3.6f;
				_roundTrip = 0;

				_reverse = !(ShadowWidth <= _increament + 3);
			}
			else
			{
				ShadowWidth += _increament;
				_roundTrip++;

				_reverse = (ShadowWidth >= _maxShadow);
			}

			_innerStart += 3.6f;
			if (_innerStart > 360f)
				_innerStart = 0f;

			Invalidate();

			//if (doSleep)
			//	timer2.Enabled = true;
			//else
			Start();
		}

		private void CalculateBounds()
		{
			SuspendLayout();

			_bitmap = new Bitmap(Width, Height);
			var half = Width / 2f;
			var r = half * 70 / 100 - 4;
			_center = new PointF(half, half);
			_rcCircle = new RectangleF(_center.X - r, _center.Y - r, r * 2, r * 2);
			_rcShadowInner = new RectangleF(_center.X - r - 4, _center.Y - r - 4, (r + 4) * 2, (r + 4) * 2);

			_rcInner = new RectangleF(_center.X - r + 1, _center.Y - r + 1, (r - 1f) * 2, (r - 1f) * 2);
			_rcCircleInner = new RectangleF(_center.X - r + 8, _center.Y - r + 8, (r - 8f) * 2, (r - 8f) * 2);

			_innerPath.Reset();
			_innerPath.AddEllipse(_rcInner);
			_innerPath.AddEllipse(_rcCircleInner);
			_bitmap = new Bitmap(Convert.ToInt32(Width), Convert.ToInt32(Height));
			_maxShadow = half / 2;
			_increament = (_maxShadow / 50) * 2f;

			ResumeLayout(true);
		}
	}

	public enum CircleStyle
	{
		Full,
		Semi,
		Quater,
		Symmetry
	}
}
