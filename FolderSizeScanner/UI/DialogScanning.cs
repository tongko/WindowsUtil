using FolderSizeScanner.Core;
using System.Windows.Forms;

namespace FolderSizeScanner.UI
{
	partial class DialogScanning : Form
	{
		private readonly AnalyzeDriveJob _job;

		DialogScanning(AnalyzeDriveJob job)
		{
			_job = job;
			InitializeComponent();
		}

		protected override void OnShown(System.EventArgs e)
		{
			base.OnShown(e);

			progress.Style = ProgressBarStyle.Marquee;
		}
	}
}
