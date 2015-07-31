using System;

namespace FolderSizeScanner.Core
{
	static class Constants
	{
		public const float Epsilon = 0.0000001F;

		public static bool AreEquals(this float float1, float float2)
		{
			return Math.Abs(float1 - float2) < Epsilon;
		}
	}
}
