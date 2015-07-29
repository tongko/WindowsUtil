using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using FolderSizeScanner.Core;

namespace FolderSizeScanner.UI
{
	public partial class DialogPickDrive : Form
	{
		private Dictionary<string, DriveInfo> _driveInfosCache;

		public DialogPickDrive()
		{
			InitializeComponent();
			_driveInfosCache = new Dictionary<string, DriveInfo>();
		}

		private void DialogPickDrive_Load(object sender, EventArgs e)
		{
			var di = DriveInfo.GetDrives();
			foreach (var info in di)
			{
				cmbDrive.Items.Add(info);
			}
		}

		private void btnScan_Click(object sender, EventArgs e)
		{
			if (cmbDrive.SelectedIndex < 0)
			{
				ttSelectDrive.Show(string.Empty, cmbDrive, 0);
				ttSelectDrive.Show("Select a drive from the drop down before continue.", cmbDrive);
				return;
			}
			var di = cmbDrive.GetItem(cmbDrive.SelectedIndex);
			var job = new AnalyzeDriveJob(di);
			var dlg = new DialogScanning(job);
			dlg.ShowDialog(this);
		}

		private void cmbDrive_DropDown(object sender, EventArgs e)
		{
			ttSelectDrive.Hide(cmbDrive);
		}
	}
}
