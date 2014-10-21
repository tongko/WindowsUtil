using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SizeExplorer
{
	public sealed class TreeViewHelper
	{
		#region Singleton Implementation

		public static TreeViewHelper Instance
		{
			get { return GetInstance(); }
		}

		private static readonly object SyncRoot = new object();
		private static volatile TreeViewHelper _instance;

		private static TreeViewHelper GetInstance()
		{
			if (_instance != null) return _instance;

			lock (SyncRoot)
			{
				if (_instance == null)
					_instance = new TreeViewHelper();
			}

			return _instance;
		}

		#endregion

		public async Task InitTreeView(TreeView tv, Action initProc)
		{
			if (tv == null)
			{
				tv = new TreeView();
			}
		}
	}
}
