using System.Collections.Generic;

namespace SizeExplorer.Model
{
	public static class SizeExplorerRuntime
	{
		private static readonly List<DiskSizeInfoCache> DiskSizeCache = new List<DiskSizeInfoCache>(); 
		public static List<DiskSizeInfoCache> DiskSizeInfo { get { return DiskSizeCache; } }
	}
}
