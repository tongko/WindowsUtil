using System.ComponentModel;
using System.Windows.Forms;

namespace FolderSizeScanner.Core
{
	static class ControlHelper
	{
		public static void DoInvoke(this ISynchronizeInvoke obj, MethodInvoker action)
		{
			if (obj.InvokeRequired)
				obj.Invoke(action, new object[0]);
			else
				action();
		}
	}
}
