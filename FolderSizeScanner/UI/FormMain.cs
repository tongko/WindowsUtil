using FolderSizeScanner.Core;
using FolderSizeScanner.Properties;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace FolderSizeScanner.UI
{
	public partial class FormMain : Form
	{
		public FormMain()
		{
			InitializeComponent();
		}

		private void LoadSettings()
		{
			//			SuspendLayout();

			Size = Settings.Default.WndSize;
			Location = Settings.Default.WndLocation;

			//			ResumeLayout(true);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Form.Load"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data. </param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			LoadSettings();
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Form.Closing"/> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs"/> that contains the event data. </param>
		protected override void OnClosing(CancelEventArgs e)
		{
			Settings.Default.WndLocation = Location;
			Settings.Default.WndSize = Size;
			Settings.Default.Save();
			base.OnClosing(e);
		}

		protected virtual void Open()
		{
			if (dlgOpen.ShowDialog(this) == DialogResult.OK)
			{
				Settings.Default.LastSavePath = dlgOpen.FileName;
			}
		}

		protected virtual void Save()
		{
			dlgSave.InitialDirectory = Settings.Default.LastSavePath;
			if (dlgSave.ShowDialog(this) == DialogResult.OK)
			{
				Settings.Default.LastSavePath = dlgSave.FileName;
			}
		}

		private void MenuItem_Select(object sender, EventArgs e)
		{
			var item = sender as MenuItem;
			if (item == null) return;

			switch (item.Name)
			{
				case "mnuFileNew":
					break;
				case "mnuFileOpen":
					Open();
					break;
				case "mnuFileSave":
					Save();
					break;
			}
		}

		private void newToolStripButton_Click(object sender, EventArgs e)
		{
			var job = new AnalyzeDriveJob("C:\\");
			var dlg = new DialogScanning(job);
			dlg.ShowDialog(this);
		}
	}
}
