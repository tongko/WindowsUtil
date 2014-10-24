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

			LoadTreeView();

			tvFolder.Update();
		}

		private void LoadTreeView()
		{
			var ddi = new DiskDriveInfo();
			ddi.PopulateInfo(null);

			var root = tvFolder.Nodes.Add(ddi.Index.ToString(), ddi.Name);
			foreach (var diskDrive in ddi.DiskDrives)
			{
				root.Nodes.Add(diskDrive.Index.ToString(), diskDrive.Name);
			}

			tvFolder.Update();
		}
	}
}
