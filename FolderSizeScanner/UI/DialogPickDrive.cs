using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

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

		}
	}
}
