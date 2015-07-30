using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace FolderSizeScanner.Core
{
	static class Geometry
	{
		private const float Epsilon = 0.000000001f;

		// Find the points nearest the upper left, upper right,
		// lower left, and lower right corners.
		private static void GetMinMaxCorners(IReadOnlyList<PointF> points, out PointF ul, out PointF ur, out PointF ll, out PointF lr)
		{
			// Start with the first point as the solution.
			ul = points[0];
			ur = ul;
			ll = ul;
			lr = ul;

			// Search the other points.
			foreach (var pt in points)
			{
				if (-pt.X - pt.Y > -ul.X - ul.Y)
					ul = pt;
				if (pt.X - pt.Y > ur.X - ur.Y)
					ur = pt;
				if (-pt.X + pt.Y > -ll.X + ll.Y)
					ll = pt;
				if (pt.X + pt.Y > lr.X + lr.Y)
					lr = pt;
			}
		}

		// Find a box that fits inside the MinMax quadrilateral.
		private static RectangleF GetMinMaxBox(IReadOnlyList<PointF> points)
		{
			// Find the MinMax quadrilateral.
			PointF ul, ur, ll, lr;
			GetMinMaxCorners(points, out ul, out ur, out ll, out lr);

			// Get the coordinates of a box that lies inside this quadrilateral.
			var xmin = ul.X;
			var ymin = ul.Y;

			var xmax = ur.X;
			if (ymin < ur.Y)
				ymin = ur.Y;

			if (xmax > lr.X)
				xmax = lr.X;
			var ymax = lr.Y;

			if (xmin < ll.X)
				xmin = ll.X;
			if (ymax > ll.Y)
				ymax = ll.Y;

			var result = new RectangleF(xmin, ymin, xmax - xmin, ymax - ymin);

			return result;
		}

		// Cull points out of the convex hull that lie inside the
		// trapezoid defined by the vertices with smallest and
		// largest X and Y coordinates.
		// Return the points that are not culled.
		private static List<PointF> HullCull(IReadOnlyList<PointF> points)
		{
			// Find a culling box.
			var cullingBox = GetMinMaxBox(points);

			// Cull the points.
			return
				points.Where(
					pt => pt.X <= cullingBox.Left || pt.X >= cullingBox.Right || pt.Y <= cullingBox.Top || pt.Y >= cullingBox.Bottom)
					.ToList();
		}

		// Return a number that gives the ordering of angles
		// WRST horizontal from the point (x1, y1) to (x2, y2).
		// In other words, AngleValue(x1, y1, x2, y2) is not
		// the angle, but if:
		//   Angle(x1, y1, x2, y2) > Angle(x1, y1, x2, y2)
		// then
		//   AngleValue(x1, y1, x2, y2) > AngleValue(x1, y1, x2, y2)
		// this angle is greater than the angle for another set
		// of points,) this number for
		//
		// This function is dy / (dy + dx).
		private static float AngleValue(float x1, float y1, float x2, float y2)
		{
			float t;

			var dx = x2 - x1;
			var ax = Math.Abs(dx);
			var dy = y2 - y1;
			var ay = Math.Abs(dy);

			if (Math.Abs(ay + ax) < Epsilon)
				// if (the two points are the same, return 360.
				t = 360f / 9f;
			else
				t = dy / (ax + ay);

			if (dx < 0)
				t = 2 - t;
			else if (dy < 0)
				t = 4 + t;

			return t * 90;
		}

		// Return the points that make up a polygon's convex hull.
		// This method leaves the points list unchanged.
		public static List<PointF> MakeConvexHull(List<PointF> points)
		{
			// Cull.
			points = HullCull(points);

			// Find the remaining point with the smallest Y value.
			// if (there's a tie, take the one with the smaller X value.
			PointF[] bestPt = { points[0] };
			foreach (var pt in points.Where(pt => (pt.Y < bestPt[0].Y) ||
												((Math.Abs(pt.Y - bestPt[0].Y) < Epsilon) && (pt.X < bestPt[0].X))))
				bestPt[0] = pt;

			// Move this point to the convex hull.
			var hull = new List<PointF> { bestPt[0] };
			points.Remove(bestPt[0]);

			// Start wrapping up the other points.
			float sweepAngle = 0;
			while (true)
			{
				// Find the point with smallest AngleValue
				// from the last point.
				var x = hull[hull.Count - 1].X;
				var y = hull[hull.Count - 1].Y;
				bestPt[0] = points[0];
				float bestAngle = 3600;

				// Search the rest of the points.
				foreach (var pt in points)
				{
					var testAngle = AngleValue(x, y, pt.X, pt.Y);
					if ((testAngle >= sweepAngle) && (bestAngle > testAngle))
					{
						bestAngle = testAngle;
						bestPt[0] = pt;
					}
				}

				// See if the first point is better.
				// If so, we are done.
				var firstAngle = AngleValue(x, y, hull[0].X, hull[0].Y);
				if ((firstAngle >= sweepAngle) && (bestAngle >= firstAngle))
					// The first point is better. We're done.
					break;

				// Add the best point to the convex hull.
				hull.Add(bestPt[0]);
				points.Remove(bestPt[0]);

				sweepAngle = bestAngle;

				// If all of the points are on the hull, we're done.
				if (points.Count == 0) break;
			}

			return hull;
		}
	}
}
