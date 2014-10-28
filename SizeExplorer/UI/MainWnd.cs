using SizeExplorer.Core;
using System;
using System.Windows.Forms;

namespace SizeExplorer.UI
{
	public partial class MainWnd : Form
	{
		public MainWnd()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Form.Load"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data. </param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			deviceView1.Add("root", "Computer", "My Computer", 0);

			var indent = 1;
			var ddi = new DiskDriveInfo();
			ddi.PopulateInfo(null);

			var dd = ddi.DiskDrives[0];
			var dSize = FormatBytes(dd.Size);
			deviceView1.Add(dd.Name, dSize + " Hard Disk", dd.Name, indent);
			//foreach (var diskDrive in ddi.DiskDrives)
			//{
			//	var dSize = FormatBytes(diskDrive.Size);
			//	deviceView1.Add(diskDrive.Name, dSize + "Hard Disk", diskDrive.Name, indent);
			//}
		}

		private static string FormatBytes(ulong bytes)
		{
			string[] suffix = { "B", "KB", "MB", "GB", "TB" };
			int i;
			double dblSByte = bytes;
			for (i = 0; i < suffix.Length && bytes >= 1024; i++, bytes /= 1024)
				dblSByte = bytes / 1024.0;
			return String.Format("{0:0.#} {1}", dblSByte, suffix[i]);
		}
	}
}
