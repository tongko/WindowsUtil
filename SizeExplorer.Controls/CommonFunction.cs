using System;

namespace SizeExplorer.Controls
{
	public static class CommonFunction
	{
		public static string ConvertByte(ulong size)
		{
			string[] sizes = { "B", "KB", "MB", "GB" };
			double len = size;
			int order = 0;
			while (len >= 1024 && order + 1 < sizes.Length)
			{
				order++;
				len = len / 1024;
			}

			// Adjust the format string to your preferences. For example "{0:0.#}{1}" would
			// show a single decimal place, and no space.
			return String.Format("{0:0.00} {1}", len, sizes[order]);
		}
	}
}
