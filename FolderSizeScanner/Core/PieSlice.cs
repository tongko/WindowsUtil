using System;
using System.Drawing;

namespace FolderSizeScanner.Core
{
	// Represents a pie slice.
	struct PieSlice : IComparable
	{
		public Brush TopBrush, SideBrush;
		public Pen TopPen;
		public float StartAngle, SweepAngle, ExplodeDistance;
		public float ZDistance
		{
			get
			{
				// Right half of the ellipse.
				if (StartAngle <= 90)
				{
					if (StartAngle + SweepAngle > 90)
					{
						// It spans the bottom of the
						// ellipse so should be last.
						return 181;
					}
					return 90 + StartAngle + SweepAngle;
				}

				// Left half of the ellipse.
				return 270 - StartAngle;
			}
		}

		#region IComparable Members

		// Compare by ZDistance.
		public int CompareTo(object obj)
		{
			var other = (PieSlice)obj;
			return ZDistance.CompareTo(other.ZDistance);
		}

		#endregion
	}
}