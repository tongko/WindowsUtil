using SizeExplorer.UI;
using System;
using System.Windows.Forms;

namespace SizeExplorer
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainWnd());
		}
	}
}
