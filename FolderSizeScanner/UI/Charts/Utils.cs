using System;
using System.Drawing;
using FolderSizeScanner.Core;

namespace FolderSizeScanner.UI.Charts
{
	/// <summary>
	///	Enumeration for different shadow styles
	/// </summary>
	public enum ShadowStyle
	{
		/// <summary>
		///	No shadow. Sides are drawn in the same color as the top od the 
		///	pie.
		/// </summary>
		NoShadow,
		/// <summary>
		///	Uniform shadow. Sides are drawn somewhat darker.
		/// </summary>
		UniformShadow,
		/// <summary>
		///	Gradual shadow is used to fully simulate 3-D shadow.
		/// </summary>
		GradualShadow
	}

	/// <summary>
	///	Enumeration for edge color types.
	/// </summary>
	public enum EdgeColorType
	{
		/// <summary>
		///	Edges are not drawn at all.
		/// </summary>
		NoEdge,
		/// <summary>
		///	System (window text) color is used to draw edges.
		/// </summary>
		SystemColor,
		/// <summary>
		///	Surface color is used for edges.
		/// </summary>
		SurfaceColor,
		/// <summary>
		///	A color that is little darker than surface color is used for
		///	edges.
		/// </summary>
		DarkerThanSurface,
		/// <summary>
		///	A color that is significantly darker than surface color is used 
		///	for edges.
		/// </summary>
		DarkerDarkerThanSurface,
		/// <summary>
		///	A color that is little lighter than surface color is used for
		///	edges.
		/// </summary>
		LighterThanSurface,
		/// <summary>
		///	A color that is significantly lighter than surface color is used 
		///	for edges.
		/// </summary>
		LighterLighterThanSurface,
		/// <summary>
		///	Contrast color is used for edges.
		/// </summary>
		Contrast,
		/// <summary>
		///	Enhanced contrast color is used for edges.
		/// </summary>
		EnhancedContrast,
		/// <summary>
		///	Black color is used for light surfaces and white for dark 
		///	surfaces.
		/// </summary>
		FullContrast
	}

	public static class ColorExtension
	{
		private const float BrightnessThreshold = 0.4F;

		/// <summary>
		///	Small brightness change factor.
		/// </summary>
		public const float BrightnessEnhancementFactor1 = 0.3F;
		/// <summary>
		///	Large brightness change factor.
		/// </summary>
		public const float BrightnessEnhancementFactor2 = 0.5F;

		/// <summary>
		///	Gets the actual color used for rendering.
		/// </summary>
		public static Color GetRenderingColor(this Color color, EdgeColorType edgeColorType)
		{
			if (color.IsEmpty)
				throw new ArgumentException("Invalid color. Empty color not supported.");

			if (edgeColorType == EdgeColorType.Contrast || edgeColorType == EdgeColorType.EnhancedContrast)
				edgeColorType = GetContrastColorType(color, edgeColorType);

			float correctionFactor = 0;

			switch (edgeColorType)
			{
				case EdgeColorType.SystemColor:
					return SystemColors.WindowText;
				case EdgeColorType.SurfaceColor:
					return color;
				case EdgeColorType.FullContrast:
					return GetFullContrastColor(color);
				case EdgeColorType.DarkerThanSurface:
					correctionFactor = -BrightnessEnhancementFactor1;
					break;
				case EdgeColorType.DarkerDarkerThanSurface:
					correctionFactor = -BrightnessEnhancementFactor2;
					break;
				case EdgeColorType.LighterThanSurface:
					correctionFactor = +BrightnessEnhancementFactor1;
					break;
				case EdgeColorType.LighterLighterThanSurface:
					correctionFactor = +BrightnessEnhancementFactor2;
					break;
				case EdgeColorType.NoEdge:
					return Color.Transparent;
			}

			return color.CreateColorWithCorrectedLightness(correctionFactor);
		}

		/// <summary>
		///	Creates color with corrected lightness.
		/// </summary>
		/// <param name="color">
		///	Color to correct.
		/// </param>
		/// <param name="correctionFactor">
		///	Correction factor, with a value between -1 and 1. Negative values
		///	create darker color, positive values lighter color. Zero value
		///	returns the current color.
		/// </param>
		/// <returns>
		///	Corrected <c>Color</c> structure.
		/// </returns>
		public static Color CreateColorWithCorrectedLightness(this Color color, float correctionFactor)
		{
			if (correctionFactor < -1 || correctionFactor > 1)
				throw new ArgumentOutOfRangeException("correctionFactor");

			if (correctionFactor.AreEquals(0F))
				return color;

			float red = color.R;
			float green = color.G;
			float blue = color.B;

			if (correctionFactor < 0)
			{
				correctionFactor = 1 + correctionFactor;
				red *= correctionFactor;
				green *= correctionFactor;
				blue *= correctionFactor;
			}
			else
			{
				red = (255 - red) * correctionFactor + red;
				green = (255 - green) * correctionFactor + green;
				blue = (255 - blue) * correctionFactor + blue;
			}

			return Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
		}

		private static EdgeColorType GetContrastColorType(Color color, EdgeColorType colorType)
		{
			if (colorType != EdgeColorType.Contrast && colorType != EdgeColorType.EnhancedContrast)
				throw new ArgumentOutOfRangeException("colorType");


			if (color.GetBrightness() > BrightnessThreshold)
			{
				return colorType == EdgeColorType.Contrast ? EdgeColorType.DarkerThanSurface : EdgeColorType.DarkerDarkerThanSurface;
			}

			return colorType == EdgeColorType.Contrast ? EdgeColorType.LighterThanSurface : EdgeColorType.LighterLighterThanSurface;
		}

		private static Color GetFullContrastColor(Color color)
		{
			return color.GetBrightness() > BrightnessThreshold ? Color.Black : Color.White;
		}
	}

	static class RectangleExtension
	{
		/// <summary>
		///	Checks if point is contained within <c>RectangleF</c> structure 
		///	and extends rectangle bounds if neccessary to include the point.
		/// </summary>
		/// <param name="rect">
		///	Reference to <c>RectangleF</c> to check.
		/// </param>
		/// <param name="pointToInclude">
		///	<c>PontF</c> object to include.
		/// </param>
		public static void IncludePoint(this RectangleF rect, PointF pointToInclude)
		{
			rect.IncludePointX(pointToInclude.X);
			rect.IncludePointY(pointToInclude.Y);
		}

		/// <summary>
		///	Checks if x-coordinate is contained within the <c>RectangleF</c> 
		///	structure and extends rectangle bounds if neccessary to include 
		///	the point.
		/// </summary>
		/// <param name="rect">
		///	<c>RectangleF</c> to check.
		/// </param>
		/// <param name="xToInclude">
		///	x-coordinate to include.
		/// </param>
		public static void IncludePointX(this RectangleF rect, float xToInclude)
		{
			if (xToInclude < rect.X)
			{
				rect.Width = rect.Right - xToInclude;
				rect.X = xToInclude;
			}
			else if (xToInclude > rect.Right)
				rect.Width = xToInclude - rect.X;
		}

		/// <summary>
		///	Checks if y-coordinate is contained within the <c>RectangleF</c> 
		///	structure and extends rectangle bounds if neccessary to include 
		///	the point.
		/// </summary>
		/// <param name="rect">
		///	<c>RectangleF</c> to check.
		/// </param>
		/// <param name="yToInclude">
		///	y-coordinate to include.
		/// </param>
		public static void IncludePointY(this RectangleF rect, float yToInclude)
		{
			if (yToInclude < rect.Y)
			{
				rect.Height = rect.Bottom - yToInclude;
				rect.Y = yToInclude;
			}
			else if (yToInclude > rect.Bottom)
				rect.Height = yToInclude - rect.Y;
		}
	}

}
