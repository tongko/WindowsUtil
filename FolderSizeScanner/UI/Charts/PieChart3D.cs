using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FolderSizeScanner.Core;

namespace FolderSizeScanner.UI.Charts
{
	/// <summary>
	///	Object representing a pie chart.
	/// </summary>
	class PieChart3D : IDisposable
	{
		private RectangleF _bounds;

		private float _initialAngle;
		private PieSlice[] _pieSlices;
		private readonly ArrayList _pieSlicesMapping = new ArrayList();
		private float[] _sliceRelativeDisplacements = { 0F };
		private float _sliceRelativeHeight;
		private decimal[] _values = { };

		/// <summary>
		///	Initializes an empty instance of <c>PieChart3D</c>.
		/// </summary>
		protected PieChart3D()
		{
			HighlightedIndex = -1;
			FitToBoundingRectangle = true;
			ShadowStyle = ShadowStyle.NoShadow;
			EdgeLineWidth = 1F;
			Colors = new[] {
				Color.Red,
				Color.Green,
				Color.Blue,
				Color.Yellow,
				Color.Purple,
				Color.Olive,
				Color.Navy,
				Color.Aqua,
				Color.Lime,
				Color.Maroon,
				Color.Teal,
				Color.Fuchsia
			};
			EdgeColorType = EdgeColorType.SystemColor;
			ForeColor = SystemColors.WindowText;
			Font = System.Windows.Forms.Control.DefaultFont;
		}

		/// <summary>
		///	Initializes an instance of a flat <c>PieChart3D</c> with 
		///	specified bounds, values to chart and relative thickness.
		/// </summary>
		/// <param name="boundingRect">
		///	Rectangle that bounds the chart.
		/// </param>
		/// <param name="values">
		///	An array of <c>decimal</c> values to chart.
		/// </param>
		public PieChart3D(RectangleF boundingRect, decimal[] values)
			: this()
		{
			_bounds = boundingRect;
			Values = values;
		}

		/// <summary>
		///	Initializes a new instance of <c>PieChart3D</c> with given bounds, 
		///	array of values and pie slice thickness.
		/// </summary>
		/// <param name="boundingRect">
		///	Bounding rectangle.
		/// </param>
		/// <param name="values">
		///	Array of values to initialize with.
		/// </param>
		/// <param name="sliceRelativeHeight"></param>
		public PieChart3D(RectangleF boundingRect, decimal[] values, float sliceRelativeHeight)
			: this(boundingRect, values)
		{
			_sliceRelativeHeight = sliceRelativeHeight;
		}

		/// <summary>
		///	Initializes a new instance of <c>PieChart3D</c> with given bounds,
		///	array of values and corresponding colors.
		/// </summary>
		/// <param name="boundingRectangle">
		///	Bounding rectangle.
		/// </param>
		/// <param name="values">
		///	Array of values to chart.
		/// </param>
		/// <param name="sliceColors">
		///	Colors used for rendering individual slices.
		/// </param>
		/// <param name="sliceRelativeHeight">
		///	Pie slice relative height.
		/// </param>
		/// <param name="texts">
		///	An array of strings that are displayed on corresponding slice.
		/// </param>
		public PieChart3D(RectangleF boundingRectangle, decimal[] values, Color[] sliceColors, float sliceRelativeHeight, string[] texts)
			: this(boundingRectangle, values, sliceRelativeHeight)
		{
			Colors = sliceColors;
			Texts = texts;
		}

		/// <summary>
		///	Initializes a new instance of <c>PieChart3D</c> with given bounds,
		///	array of values and relative pie slice height.
		/// </summary>
		/// <param name="boundingRect">
		///	Rectangle bounding the chart.
		/// </param>
		/// <param name="values">
		///	An array of <c>decimal</c> values to chart.
		/// </param>
		/// <param name="sliceRelativeHeight">
		///	Thickness of the slice to chart relative to the height of the
		///	bounding rectangle.
		/// </param>
		/// <param name="texts">
		///	An array of strings that are displayed on corresponding slice.
		/// </param>
		public PieChart3D(RectangleF boundingRect, decimal[] values, float sliceRelativeHeight, string[] texts)
			: this(boundingRect, values, sliceRelativeHeight)
		{
			Texts = texts;
		}

		protected RectangleF Bounds { get { return _bounds; } set { _bounds = value; } }

		/// <summary>
		///	Sets values to be displayed on the chart.
		/// </summary>
		public decimal[] Values
		{
			get { return _values; }
			set
			{
				if (value == null || value.Length == 0)
					throw new ArgumentNullException("value");
				_values = value;
			}
		}

		/// <summary>
		///	Sets colors used for individual pie slices.
		/// </summary>
		public Color[] Colors { get; set; }

		/// <summary>
		///	Sets text displayed by slices.
		/// </summary>
		public string[] Texts { get; set; }

		/// <summary>
		///	Gets or sets the font of the text displayed by the control.
		/// </summary>
		public Font Font { get; set; }

		/// <summary>
		///	Gets or sets the foreground color of the control used to draw text.
		/// </summary>
		public Color ForeColor { get; set; }

		/// <summary>
		///	Sets slice edge color mode. If set to <c>PenColor</c> (default),
		///	then value set by <c>EdgeColor</c> property is used.
		/// </summary>
		public EdgeColorType EdgeColorType { get; set; }

		/// <summary>
		///	Sets slice edge line width. If not set, default value is 1.
		/// </summary>
		public float EdgeLineWidth { get; set; }

		/// <summary>
		///	Sets slice height, relative to the top ellipse semi-axis. Must be
		///	less than or equal to 0.5.
		/// </summary>
		public float SliceRelativeHeight
		{
			get { return _sliceRelativeHeight; }
			set
			{
				if (value > 0.5F)
					throw new ArgumentOutOfRangeException("value");

				_sliceRelativeHeight = value;
			}
		}

		/// <summary>
		///	Sets the slice displacement relative to the ellipse semi-axis.
		///	Must be less than 1.
		/// </summary>
		public float SliceRelativeDisplacement
		{
			get { return _sliceRelativeDisplacements[0]; }
			set
			{
				if (!IsDisplacementValid(value))
					throw new ArgumentOutOfRangeException("value");

				_sliceRelativeDisplacements = new[] { value };
			}
		}

		/// <summary>
		///	Sets the slice displacement relative to the ellipse semi-axis.
		///	Must be less than 1.
		/// </summary>
		public float[] SliceRelativeDisplacements
		{
			get { return _sliceRelativeDisplacements; }
			set
			{
				if (!AreDisplacementsValid(value))
					throw new ArgumentOutOfRangeException("value");

				_sliceRelativeDisplacements = value;
			}
		}

		/// <summary>
		///	Gets or sets the size of the entire pie chart.
		/// </summary>
		public SizeF ChartSize
		{
			get { return _bounds.Size; }
			set
			{
				_bounds.Size = value;
			}
		}

		/// <summary>
		///	Gets or sets the width of the bounding rectangle.
		/// </summary>
		public float Width
		{
			get { return _bounds.Width; }
			set { _bounds.Width = value; }
		}

		/// <summary>
		///	Gets or sets the height of the bounding rectangle.
		/// </summary>
		public float Height
		{
			get { return _bounds.Height; }
			set { _bounds.Height = value; }
		}

		/// <summary>
		///	Gets the y-coordinate of the bounding rectangle top edge.
		/// </summary>
		public float Top
		{
			get { return _bounds.X; }
			set { _bounds.X = value; }
		}

		/// <summary>
		///	Gets the y-coordinate of the bounding rectangle bottom edge.
		/// </summary>
		public float Bottom
		{
			get { return _bounds.Bottom; }
		}

		/// <summary>
		///	Gets the x-coordinate of the bounding rectangle left edge.
		/// </summary>
		public float Left
		{
			get { return _bounds.Left; }
		}

		/// <summary>
		///	Gets the x-coordinate of the bounding rectangle right edge.
		/// </summary>
		public float Right
		{
			get { return _bounds.Right; }
		}

		/// <summary>
		///	Gets or sets the x-coordinate of the upper-left corner of the 
		///	bounding rectangle.
		/// </summary>
		public float X
		{
			get { return _bounds.X; }
			set { _bounds.X = value; }
		}

		/// <summary>
		///	Gets or sets the y-coordinate of the upper-left corner of the 
		///	bounding rectangle.
		/// </summary>
		public float Y
		{
			get { return _bounds.Y; }
			set { _bounds.Y = value; }
		}

		/// <summary>
		///	Sets the shadowing style used.
		/// </summary>
		public ShadowStyle ShadowStyle { get; set; }

		/// <summary>
		///	Sets the flag that controls if chart is fit to bounding rectangle 
		///	exactly.
		/// </summary>
		public bool FitToBoundingRectangle { get; set; }

		/// <summary>
		///	Sets the initial angle from which pies are placed.
		/// </summary>
		public float InitialAngle
		{
			get { return _initialAngle; }
			set
			{
				_initialAngle = value % 360;
				if (_initialAngle < 0)
					_initialAngle += 360;
			}
		}

		/// <summary>
		///	Sets the index of the highlighted pie.
		/// </summary>
		public int HighlightedIndex { get; set; }

		/// <summary>
		///	Finds the largest displacement.
		/// </summary>
		protected float LargestDisplacement
		{
			get
			{
				var value = 0F;
				for (var i = 0; i < _sliceRelativeDisplacements.Length && i < _values.Length; ++i)
				{
					if (_sliceRelativeDisplacements[i] > value)
						value = _sliceRelativeDisplacements[i];
				}

				return value;
			}
		}

		/// <summary>
		///	Gets the top ellipse size.
		/// </summary>
		protected SizeF TopEllipseSize
		{
			get
			{
				var factor = 1 + LargestDisplacement;
				var widthTopEllipse = _bounds.Width / factor;
				var heightTopEllipse = _bounds.Height / factor * (1 - _sliceRelativeHeight);

				return new SizeF(widthTopEllipse, heightTopEllipse);
			}
		}

		/// <summary>
		///	Gets the ellipse defined by largest displacement.
		/// </summary>
		protected SizeF LargestDisplacementEllipseSize
		{
			get
			{
				var factor = LargestDisplacement;
				var widthDisplacementEllipse = TopEllipseSize.Width * factor;
				var heightDisplacementEllipse = TopEllipseSize.Height * factor;

				return new SizeF(widthDisplacementEllipse, heightDisplacementEllipse);
			}
		}

		/// <summary>
		///	Calculates the pie height.
		/// </summary>
		protected float PieHeight
		{
			get
			{
				return _bounds.Height / (1 + LargestDisplacement) * _sliceRelativeHeight;
			}
		}

		/// <summary>
		///	Implementation of <c>IDisposable</c> interface.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		///	Disposes of all pie slices.
		/// </summary>
		protected virtual void Dispose(bool disposing)
		{
			if (!disposing) return;

			foreach (var slice in _pieSlices)
			{
				slice.Dispose();
			}
		}

		/// <summary>
		///	Draws the chart.
		/// </summary>
		/// <param name="graphics">
		///	<c>Graphics</c> object used for drawing.
		/// </param>
		public void Draw(Graphics graphics)
		{
			if (_values == null || _values.Length == 0)
				return;

			InitializePieSlices();
			if (FitToBoundingRectangle)
			{
				var newBoundingRectangle = GetFittingRectangle();
				ReadjustSlices(newBoundingRectangle);
			}

			DrawBottoms(graphics);
			if (_sliceRelativeHeight > 0F)
				DrawSliceSides(graphics);

			DrawTops(graphics);
		}

		/// <summary>
		///	Draws strings by individual slices. Position of the text is 
		///	calculated by overridable <c>GetTextPosition</c> method of the
		///	<c>PieSlice</c> type.
		/// </summary>
		/// <param name="graphics">
		///	<c>Graphics</c> object.
		/// </param>
		public virtual void PlaceTexts(Graphics graphics)
		{
			var drawFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

			using (Brush fontBrush = new SolidBrush(ForeColor))
			{
				foreach (var slice in _pieSlices)
				{
					if (string.IsNullOrWhiteSpace(slice.Text)) continue;

					var point = slice.GetTextPosition();
					graphics.DrawString(slice.Text, Font, fontBrush, point, drawFormat);
				}
			}
		}

		/// <summary>
		///	Searches the chart to find the index of the pie slice which 
		///	contains point given. Search order goes in the direction opposite
		///	to drawing order.
		/// </summary>
		/// <param name="point">
		///	<c>PointF</c> point for which pie slice is searched for.
		/// </param>
		/// <returns>
		///	Index of the corresponding pie slice, or -1 if none is found.
		/// </returns>
		public int FindPieSliceUnderPoint(PointF point)
		{
			// first check tops
			for (var i = 0; i < _pieSlices.Length; ++i)
			{
				var slice = _pieSlices[i];
				if (slice.PieSliceContainsPoint(point))
					return (int)_pieSlicesMapping[i];
			}

			// split the backmost (at 270 degrees) pie slice
			var pieSlicesList = new ArrayList(_pieSlices);
			var splitSlices = _pieSlices[0].Split(270F);
			if (splitSlices.Length > 1)
			{
				pieSlicesList[0] = splitSlices[1];
				if (splitSlices[0].SweepAngle > 0F)
					pieSlicesList.Add(splitSlices[0]);
			}

			var pieSlices = (PieSlice[])pieSlicesList.ToArray(typeof(PieSlice));
			var indexFound = -1;

			// if not found yet, then check for periferies
			var incrementIndex = 0;
			var decrementIndex = pieSlices.Length - 1;
			while (incrementIndex <= decrementIndex)
			{
				var sliceLeft = pieSlices[decrementIndex];
				var angle1 = 270 - sliceLeft.StartAngle;
				var sliceRight = pieSlices[incrementIndex];
				var angle2 = (sliceRight.EndAngle + 90) % 360;

				if (angle2 < 0)
					throw new InvalidOperationException("angle2 less than 0");

				if (angle2 < angle1)
				{
					if (sliceRight.PeripheryContainsPoint(point))
						indexFound = incrementIndex;
					++incrementIndex;
				}
				else
				{
					if (sliceLeft.PeripheryContainsPoint(point))
						indexFound = decrementIndex;
					--decrementIndex;
				}
			}

			// check for start/stop sides, starting from the foremost
			if (indexFound < 0)
			{
				var foremostPieIndex = GetForemostPieSlice(pieSlices);
				// check for start sides from the foremost slice to the left 
				// side
				var i = foremostPieIndex;
				while (i < pieSlices.Length)
				{
					var sliceLeft = pieSlices[i];
					if (sliceLeft.StartSideContainsPoint(point))
					{
						indexFound = i;
						break;
					}
					++i;
				}

				// if not found yet, check end sides from the foremost to the right
				// side
				if (indexFound < 0)
				{
					i = foremostPieIndex;
					while (i >= 0)
					{
						var sliceLeft = pieSlices[i];
						if (sliceLeft.EndSideContainsPoint(point))
						{
							indexFound = i;
							break;
						}
						--i;
					}
				}
			}

			// finally search for bottom sides
			if (indexFound < 0)
			{
				for (var i = 0; i < _pieSlices.Length; ++i)
				{
					var slice = _pieSlices[i];
					if (slice.BottomSurfaceSectionContainsPoint(point))
						return (int)_pieSlicesMapping[i];
				}
			}

			if (indexFound > -1)
			{
				indexFound %= _pieSlicesMapping.Count;
				return (int)_pieSlicesMapping[indexFound];
			}

			return -1;
		}

		/// <summary>
		///	Return the index of the foremost pie slice i.e. the one crossing
		///	90 degrees boundary.
		/// </summary>
		/// <param name="pieSlices">
		///	Array of <c>PieSlice</c> objects to examine.
		/// </param>
		/// <returns>
		///	Index of the foremost pie slice.
		/// </returns>
		private static int GetForemostPieSlice(IList<PieSlice> pieSlices)
		{
			if (pieSlices == null || pieSlices.Count == 0)
				throw new ArgumentNullException("pieSlices");

			for (var i = 0; i < pieSlices.Count; ++i)
			{
				var pieSlice = pieSlices[i];
				if (((pieSlice.StartAngle <= 90) && ((pieSlice.StartAngle + pieSlice.SweepAngle) >= 90)) ||
					((pieSlice.StartAngle + pieSlice.SweepAngle > 360) && ((pieSlice.StartAngle) <= 450) &&
					(pieSlice.StartAngle + pieSlice.SweepAngle) >= 450))
				{
					return i;
				}
			}

			throw new InvalidOperationException("Foremost pie slice not found");
		}

		/// <summary>
		///	Finds the smallest rectangle int which chart fits entirely.
		/// </summary>
		/// <returns>
		///	<c>RectangleF</c> into which all member slices fit.
		/// </returns>
		protected RectangleF GetFittingRectangle()
		{
			var boundingRectangle = _pieSlices[0].GetFittingRectangle();
			for (var i = 1; i < _pieSlices.Length; ++i)
			{
				boundingRectangle = RectangleF.Union(boundingRectangle, _pieSlices[i].GetFittingRectangle());
			}

			return boundingRectangle;
		}

		/// <summary>
		///	Readjusts each slice for new bounding rectangle. 
		/// </summary>
		/// <param name="newBoundingRect">
		///	<c>RectangleF</c> representing new boundary.
		/// </param>
		protected void ReadjustSlices(RectangleF newBoundingRect)
		{
			var xResizeFactor = _bounds.Width / newBoundingRect.Width;
			var yResizeFactor = _bounds.Height / newBoundingRect.Height;
			var xOffset = newBoundingRect.X - _bounds.X;
			var yOffset = newBoundingRect.Y - _bounds.Y;

			foreach (var slice in _pieSlices)
			{
				var x = slice.BoundingRectangle.X - xOffset;
				var y = slice.BoundingRectangle.Y - yOffset;
				var width = slice.BoundingRectangle.Width * xResizeFactor;
				var height = slice.BoundingRectangle.Height * yResizeFactor;
				var sliceHeight = slice.SliceHeight * yResizeFactor;

				slice.Readjust(x, y, width, height, sliceHeight);
			}
		}

		/// <summary>
		///	Initializes pies.
		/// </summary>
		/// Creates a list of pies, starting with the pie that is crossing the 
		/// 270 degrees boundary, i.e. "backmost" pie that always has to be 
		/// drawn first to ensure correct surface overlapping.
		protected virtual void InitializePieSlices()
		{
			// calculates the sum of values required to evaluate sweep angles 
			// for individual pies
			var sum = _values.Sum(itemValue => (double)itemValue);

			// some values and indices that will be used in the loop
			var topEllipeSize = TopEllipseSize;
			var largestDisplacementEllipseSize = LargestDisplacementEllipseSize;
			var maxDisplacementIndex = _sliceRelativeDisplacements.Length - 1;
			var largestDisplacement = LargestDisplacement;
			var listPieSlices = new ArrayList();
			var colorIndex = 0;
			var backPieIndex = -1;
			var displacementIndex = 0;
			var startAngle = (double)_initialAngle;

			_pieSlicesMapping.Clear();
			for (var i = 0; i < _values.Length; ++i)
			{
				var itemValue = _values[i];
				var sweepAngle = (double)itemValue / sum * 360;

				// displacement from the center of the ellipse
				var xDisplacement = _sliceRelativeDisplacements[displacementIndex];
				var yDisplacement = _sliceRelativeDisplacements[displacementIndex];
				if (xDisplacement > 0F)
				{
					if (largestDisplacement <= 0F)
						throw new InvalidOperationException("Largest displacement cannot be less than or equal 0.");

					var pieDisplacement = GetSliceDisplacement((float)(startAngle + sweepAngle / 2),
						_sliceRelativeDisplacements[displacementIndex]);
					xDisplacement = pieDisplacement.Width;
					yDisplacement = pieDisplacement.Height;
				}

				var slice = i == HighlightedIndex
					? CreatePieSliceHighlighted(_bounds.X + largestDisplacementEllipseSize.Width / 2 + xDisplacement,
						_bounds.Y + largestDisplacementEllipseSize.Height / 2 + yDisplacement, topEllipeSize.Width,
						topEllipeSize.Height, PieHeight, (float)(startAngle % 360), (float)(sweepAngle), Colors[colorIndex],
						ShadowStyle, EdgeColorType, EdgeLineWidth)
					: CreatePieSlice(_bounds.X + largestDisplacementEllipseSize.Width / 2 + xDisplacement,
						_bounds.Y + largestDisplacementEllipseSize.Height / 2 + yDisplacement, topEllipeSize.Width,
						topEllipeSize.Height, PieHeight, (float)(startAngle % 360), (float)(sweepAngle), Colors[colorIndex],
						ShadowStyle, EdgeColorType, EdgeLineWidth);

				slice.Text = Texts[i];
				// the backmost pie is inserted to the front of the list for correct drawing
				if (backPieIndex > -1 || (startAngle <= 270 && startAngle + sweepAngle > 270) ||
					(startAngle >= 270 && startAngle + sweepAngle > 630))
				{
					++backPieIndex;
					listPieSlices.Insert(backPieIndex, slice);
					_pieSlicesMapping.Insert(backPieIndex, i);
				}
				else
				{
					listPieSlices.Add(slice);
					_pieSlicesMapping.Add(i);
				}

				// increment displacementIndex only if there are more displacements available
				if (displacementIndex < maxDisplacementIndex)
					++displacementIndex;
				++colorIndex;

				// if all colors have been exhausted, reset color index
				if (colorIndex >= Colors.Length)
					colorIndex = 0;

				// prepare for the next pie slice
				startAngle += sweepAngle;
				if (startAngle > 360)
					startAngle -= 360;
			}

			_pieSlices = (PieSlice[])listPieSlices.ToArray(typeof(PieSlice));
		}

		/// <summary>
		///	Creates a <c>PieSlice</c> object.
		/// </summary>
		/// <param name="boundingRectLeft">
		///	x-coordinate of the upper-left corner of the rectangle that is 
		///	used to draw the top surface of the slice.
		/// </param>
		/// <param name="boundingRectTop">
		///	y-coordinate of the upper-left corner of the rectangle that is 
		///	used to draw the top surface of the slice.
		/// </param>
		/// <param name="boundingRectWidth">
		///	Width of the rectangle that is used to draw the top surface of 
		///	the slice.
		/// </param>
		/// <param name="boundingRectHeight">
		///	Height of the rectangle that is used to draw the top surface of 
		///	the slice.
		/// </param>
		/// <param name="sliceHeight">
		///	Slice height.
		/// </param>
		/// <param name="startAngle">
		///	Starting angle.
		/// </param>
		/// <param name="sweepAngle">
		///	Sweep angle.
		/// </param>
		/// <param name="color">
		///	Color used for slice rendering.
		/// </param>
		/// <param name="shadowStyle">
		///	Shadow style used for slice rendering.
		/// </param>
		/// <param name="edgeColorType">
		///	Edge lines color type.
		/// </param>
		/// <param name="edgeLineWidth">
		///	Edge lines width.
		/// </param>
		/// <returns>
		///	<c>PieSlice</c> object with given values.
		/// </returns>
		protected virtual PieSlice CreatePieSlice(float boundingRectLeft, float boundingRectTop, float boundingRectWidth,
			float boundingRectHeight, float sliceHeight, float startAngle, float sweepAngle, Color color, ShadowStyle shadowStyle,
			EdgeColorType edgeColorType, float edgeLineWidth)
		{
			return new PieSlice(boundingRectLeft, boundingRectTop, boundingRectWidth, boundingRectHeight, sliceHeight,
				startAngle % 360, sweepAngle, color, shadowStyle, edgeColorType, edgeLineWidth);
		}

		/// <summary>
		///	Creates highlighted <c>PieSlice</c> object.
		/// </summary>
		/// <param name="boundingRectLeft">
		///	x-coordinate of the upper-left corner of the rectangle that is 
		///	used to draw the top surface of the slice.
		/// </param>
		/// <param name="boundingRectTop">
		///	y-coordinate of the upper-left corner of the rectangle that is 
		///	used to draw the top surface of the slice.
		/// </param>
		/// <param name="boundingRectWidth">
		///	Width of the rectangle that is used to draw the top surface of 
		///	the slice.
		/// </param>
		/// <param name="boundingRectHeight">
		///	Height of the rectangle that is used to draw the top surface of 
		///	the slice.
		/// </param>
		/// <param name="sliceHeight">
		///	Slice height.
		/// </param>
		/// <param name="startAngle">
		///	Starting angle.
		/// </param>
		/// <param name="sweepAngle">
		///	Sweep angle.
		/// </param>
		/// <param name="color">
		///	Color used for slice rendering.
		/// </param>
		/// <param name="shadowStyle">
		///	Shadow style used for slice rendering.
		/// </param>
		/// <param name="edgeColorType">
		///	Edge lines color type.
		/// </param>
		/// <param name="edgeLineWidth">
		///	Edge lines width.
		/// </param>
		/// <returns>
		///	<c>PieSlice</c> object with given values.
		/// </returns>
		protected virtual PieSlice CreatePieSliceHighlighted(float boundingRectLeft, float boundingRectTop, float boundingRectWidth, float boundingRectHeight, float sliceHeight, float startAngle, float sweepAngle, Color color, ShadowStyle shadowStyle, EdgeColorType edgeColorType, float edgeLineWidth)
		{
			var highLightedColor = color.CreateColorWithCorrectedLightness(ColorExtension.BrightnessEnhancementFactor1);

			return new PieSlice(boundingRectLeft, boundingRectTop, boundingRectWidth, boundingRectHeight, sliceHeight,
				startAngle % 360, sweepAngle, highLightedColor, shadowStyle, edgeColorType, edgeLineWidth);
		}

		/// <summary>
		///	Calculates the displacement for given angle.
		/// </summary>
		/// <param name="angle">
		///	Angle (in degrees).
		/// </param>
		/// <param name="displacementFactor">
		///	Displacement factor.
		/// </param>
		/// <returns>
		///	<c>SizeF</c> representing displacement.
		/// </returns>
		protected SizeF GetSliceDisplacement(float angle, float displacementFactor)
		{
			if (displacementFactor <= 0F || displacementFactor > 1F)
				throw new ArgumentOutOfRangeException("displacementFactor");

			if (displacementFactor.AreEquals(0F))
				return SizeF.Empty;

			var xDisplacement = (float)(TopEllipseSize.Width * displacementFactor / 2 * Math.Cos(angle * Math.PI / 180));
			var yDisplacement = (float)(TopEllipseSize.Height * displacementFactor / 2 * Math.Sin(angle * Math.PI / 180));

			return new SizeF(xDisplacement, yDisplacement);
		}

		/// <summary>
		///	Draws outer peripheries of all slices.
		/// </summary>
		/// <param name="graphics">
		///	<c>Graphics</c> used for drawing.
		/// </param>
		protected void DrawSliceSides(Graphics graphics)
		{
			var pieSlicesList = new ArrayList(_pieSlices);
			PieSlice ps;

			// if the first pie slice (crossing 270 i.e. back) is crossing 90 
			// (front) axis too, we have to split it
			if ((_pieSlices[0].StartAngle > 90) && (_pieSlices[0].StartAngle <= 270) &&
				(_pieSlices[0].StartAngle + _pieSlices[0].SweepAngle > 450))
			{
				ps = (PieSlice)pieSlicesList[0];

				// this one is split at 0 deg to avoid line of split to be
				// visible on the periphery
				var splitSlices = ps.Split(0F);
				pieSlicesList[0] = splitSlices[0];
				if (splitSlices[1].SweepAngle > 0F)
					pieSlicesList.Insert(1, splitSlices[1]);
			}
			else if ((_pieSlices[0].StartAngle > 270 && _pieSlices[0].StartAngle + _pieSlices[0].SweepAngle > 450) ||
				(_pieSlices[0].StartAngle < 90 && _pieSlices[0].StartAngle + _pieSlices[0].SweepAngle > 270))
			{
				ps = (PieSlice)pieSlicesList[0];
				// this one is split at 180 deg to avoid line of split to be
				// visible on the periphery
				var splitSlices = ps.Split(180F);
				pieSlicesList[0] = splitSlices[1];
				if (splitSlices[1].SweepAngle > 0F)
					pieSlicesList.Add(splitSlices[0]);
			}

			// first draw the backmost pie slice
			ps = (PieSlice)pieSlicesList[0];
			ps.DrawSides(graphics);

			// draw pie slices from the backmost to forward
			var incrementIndex = 1;
			var decrementIndex = pieSlicesList.Count - 1;
			while (incrementIndex < decrementIndex)
			{
				var sliceLeft = (PieSlice)pieSlicesList[decrementIndex];
				var angle1 = sliceLeft.StartAngle - 90;
				if (angle1 > 180 || angle1 < 0)
					angle1 = 0;

				var sliceRight = (PieSlice)pieSlicesList[incrementIndex];
				var angle2 = (450 - sliceRight.EndAngle) % 360;
				if (angle2 > 180 || angle2 < 0)
					angle2 = 0;

				if (angle1 < 0)
					throw new InvalidOperationException("angle1 cannot be less than 0");
				if (angle2 < 0)
					throw new InvalidOperationException("angle2 cannot be less than 0");

				if (angle2 >= angle1)
				{
					sliceRight.DrawSides(graphics);
					++incrementIndex;
				}
				else if (angle2 < angle1)
				{
					sliceLeft.DrawSides(graphics);
					--decrementIndex;
				}
			}

			ps = (PieSlice)pieSlicesList[decrementIndex];
			ps.DrawSides(graphics);
		}

		/// <summary>
		///	Draws bottom sides of all pie slices.
		/// </summary>
		/// <param name="graphics">
		///	<c>Graphics</c> used for drawing.
		/// </param>
		protected void DrawBottoms(Graphics graphics)
		{
			foreach (var slice in _pieSlices)
			{
				slice.DrawBottom(graphics);
			}
		}

		/// <summary>
		///	Draws top sides of all pie slices.
		/// </summary>
		/// <param name="graphics">
		///	<c>Graphics</c> used for drawing.
		/// </param>
		protected void DrawTops(Graphics graphics)
		{
			foreach (var slice in _pieSlices)
			{
				slice.DrawTop(graphics);
			}
		}

		/// <summary>
		///	Helper function used in assertions. Checks the validity of 
		///	slice displacements.
		/// </summary>
		/// <param name="displacements">
		///	Array of displacements to check.
		/// </param>
		/// <returns>
		///	<c>true</c> if all displacements have a valid value; otherwise 
		///	<c>false</c>.
		/// </returns>
		private static bool AreDisplacementsValid(IEnumerable<float> displacements)
		{
			return displacements.All(IsDisplacementValid);
		}

		/// <summary>
		///	Helper function used in assertions. Checks the validity of 
		///	a slice displacement.
		/// </summary>
		/// <param name="value">
		///	Displacement value to check.
		/// </param>
		/// <returns>
		///	<c>true</c> if displacement has a valid value; otherwise 
		///	<c>false</c>.
		/// </returns>
		private static bool IsDisplacementValid(float value)
		{
			return (value >= 0F && value <= 1F);
		}
	}
}
