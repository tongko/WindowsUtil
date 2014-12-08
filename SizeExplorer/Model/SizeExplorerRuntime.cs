using SizeExplorer.Controls;
using System.Collections.Generic;

namespace SizeExplorer.Model
{
	public static class SizeExplorerRuntime
	{
		private static readonly List<DiskSizeInfoCache> DiskSizeCache = new List<DiskSizeInfoCache>();
		private static readonly Dictionary<string, ISizeNode> Nodes = new Dictionary<string, ISizeNode>();

		public static List<DiskSizeInfoCache> DiskSizeInfo { get { return DiskSizeCache; } }

		public static Dictionary<string, ISizeNode> SizeNodes { get { return Nodes; } }
	}
}
