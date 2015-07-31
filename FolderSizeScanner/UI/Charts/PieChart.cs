using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace FolderSizeScanner.UI.Charts
{
	/// <summary>
	/// Draws values in graphical pie chart form.
	/// </summary>
	public partial class PieChart : Panel
	{
		private PieChart3D _pieChart;
		private float _leftMargin;
		private float _topMargin;
		private float _rightMargin;
		private float _bottomMargin;
		private bool _fitChart;

		private decimal[] _values;
		private Color[] _colors;
		private float _sliceRelativeHeight;
		private float[] _relativeSliceDisplacements = { 0F };
		private ShadowStyle _shadowStyle = ShadowStyle.GradualShadow;
		private EdgeColorType _edgeColorType = EdgeColorType.SystemColor;
		private float _edgeLineWidth = 1F;
		private float _initialAngle;
		private int _highlightedIndex = -1;
		private readonly ToolTip _toolTip;
		private int _defaultToolTipAutoPopDelay;

		/// <summary>
		///	Initializes the <c>PieChartControl</c>.
		/// </summary>
		public PieChart()
		{
			Texts = null;
			ToolTips = null;
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.DoubleBuffer, true);
			SetStyle(ControlStyles.ResizeRedraw, true);
			_toolTip = new ToolTip();
			InitializeComponent();
		}

		/// <summary>
		///	Sets the left margin for the chart.
		/// </summary>
		public float LeftMargin
		{
			set
			{
				if (value < 0)
					throw new ArgumentOutOfRangeException("value");

				_leftMargin = value;
				Invalidate();
			}
		}

		/// <summary>
		///	Sets the right margin for the chart.
		/// </summary>
		public float RightMargin
		{
			set
			{
				if (value < 0)
					throw new ArgumentOutOfRangeException("value");

				_rightMargin = value;
				Invalidate();
			}
		}

		/// <summary>
		///	Sets the top margin for the chart.
		/// </summary>
		public float TopMargin
		{
			set
			{
				if (value < 0)
					throw new ArgumentOutOfRangeException("value");

				_topMargin = value;
				Invalidate();
			}
		}

		/// <summary>
		///	Sets the bottom margin for the chart.
		/// </summary>
		public float BottomMargin
		{
			set
			{
				if (value < 0)
					throw new ArgumentOutOfRangeException("value");

				_bottomMargin = value;
				Invalidate();
			}
		}

		/// <summary>
		///	Sets the indicator if chart should fit the bounding rectangle
		///	exactly.
		/// </summary>
		public bool FitChart
		{
			set
			{
				_fitChart = value;
				Invalidate();
			}
		}

		/// <summary>
		///	Sets values to be represented by the chart.
		/// </summary>
		public decimal[] Values
		{
			set
			{
				_values = value;
				Invalidate();
			}
		}

		/// <summary>
		///	Sets colors to be used for rendering pie slices.
		/// </summary>
		public Color[] Colors
		{
			set
			{
				_colors = value;
				Invalidate();
			}
		}

		/// <summary>
		///	Sets values for slice displacements.
		/// </summary>
		public float[] SliceRelativeDisplacements
		{
			set
			{
				_relativeSliceDisplacements = value;
				Invalidate();
			}
		}

		/// <summary>
		///	Gets or sets tooltip texts.
		/// </summary>
		public string[] ToolTips { get; set; }

		/// <summary>
		///	Sets texts appearing by each pie slice.
		/// </summary>
		public string[] Texts { private get; set; }

		/// <summary>
		///	Sets pie slice reative height.
		/// </summary>
		public float SliceRelativeHeight
		{
			set
			{
				_sliceRelativeHeight = value;
				Invalidate();
			}
		}

		/// <summary>
		///	Sets the shadow style.
		/// </summary>
		public ShadowStyle ShadowStyle
		{
			set
			{
				_shadowStyle = value;
				Invalidate();
			}
		}

		/// <summary>
		///  Sets the edge color type.
		/// </summary>
		public EdgeColorType EdgeColorType
		{
			set
			{
				_edgeColorType = value;
				Invalidate();
			}
		}

		/// <summary>
		///	Sets the edge lines width.
		/// </summary>
		public float EdgeLineWidth
		{
			set
			{
				_edgeLineWidth = value;
				Invalidate();
			}
		}

		/// <summary>
		///	Sets the initial angle from which pies are drawn.
		/// </summary>
		public float InitialAngle
		{
			set
			{
				_initialAngle = value;
				Invalidate();
			}
		}

		/// <summary>
		///	Handles <c>OnPaint</c> event.
		/// </summary>
		/// <param name="args">
		///	<c>PaintEventArgs</c> object.
		/// </param>
		protected override void OnPaint(PaintEventArgs args)
		{
			base.OnPaint(args);
			if (HasAnyValue)
			{
				DoDraw(args.Graphics);
			}
		}

		/// <summary>
		///	Sets values for the chart and draws them.
		/// </summary>
		/// <param name="graphics">
		///	Graphics object used for drawing.
		/// </param>
		protected void DoDraw(Graphics graphics)
		{
			if (_values == null || _values.Length <= 0) return;

			graphics.SmoothingMode = SmoothingMode.AntiAlias;
			var width = ClientSize.Width - _leftMargin - _rightMargin;
			var height = ClientSize.Height - _topMargin - _bottomMargin;
			// if the width or height if <=0 an exception would be thrown -> exit method..
			if (width <= 0 || height <= 0)
				return;

			if (_pieChart != null)
				_pieChart.Dispose();

			if (_colors != null && _colors.Length > 0)
				_pieChart = new PieChart3D(new RectangleF(_leftMargin, _topMargin, width, height), _values, _colors, _sliceRelativeHeight, Texts);
			else
				_pieChart = new PieChart3D(new RectangleF(_leftMargin, _topMargin, width, height), _values, _sliceRelativeHeight, Texts);

			_pieChart.FitToBoundingRectangle = _fitChart;
			_pieChart.InitialAngle = _initialAngle;
			_pieChart.SliceRelativeDisplacements = _relativeSliceDisplacements;
			_pieChart.EdgeColorType = _edgeColorType;
			_pieChart.EdgeLineWidth = _edgeLineWidth;
			_pieChart.ShadowStyle = _shadowStyle;
			_pieChart.HighlightedIndex = _highlightedIndex;
			_pieChart.Draw(graphics);
			_pieChart.Font = Font;
			_pieChart.ForeColor = ForeColor;
			_pieChart.PlaceTexts(graphics);
		}

		/// <summary>
		///	Handles <c>MouseEnter</c> event to activate the tooltip.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
			_defaultToolTipAutoPopDelay = _toolTip.AutoPopDelay;
			_toolTip.AutoPopDelay = short.MaxValue;
		}

		/// <summary>
		///	Handles <c>MouseLeave</c> event to disable tooltip.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			_toolTip.RemoveAll();
			_toolTip.AutoPopDelay = _defaultToolTipAutoPopDelay;
			_highlightedIndex = -1;
			Refresh();
		}

		/// <summary>
		///	Handles <c>MouseMove</c> event to display tooltip for the pie
		///	slice under pointer and to display slice in highlighted color.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (_pieChart == null || _values == null || _values.Length <= 0) return;

			var index = _pieChart.FindPieSliceUnderPoint(new PointF(e.X, e.Y));
			if (index != _highlightedIndex)
			{
				_highlightedIndex = index;
				Invalidate();
				//Refresh();
			}

			if (_highlightedIndex != -1)
			{
				if (ToolTips == null || ToolTips.Length <= _highlightedIndex || ToolTips[_highlightedIndex].Length == 0)
					_toolTip.SetToolTip(this, _values[_highlightedIndex].ToString("N"));
				else
					_toolTip.SetToolTip(this, ToolTips[_highlightedIndex]);
			}
			else
			{
				_toolTip.RemoveAll();
			}
		}

		/// <summary>
		///	Gets a flag indicating if at least one value is nonzero.
		/// </summary>
		private bool HasAnyValue
		{
			get
			{
				return _values != null && _values.Any(angle => angle != 0);
			}
		}

	}
}
