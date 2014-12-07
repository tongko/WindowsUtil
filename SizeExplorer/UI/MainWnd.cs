using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SizeExplorer.Controls;
using SizeExplorer.Core;
using SizeExplorer.Model;
using SizeExplorer.Properties;
using SizeExplorer.UI.Resources;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SizeExplorer.UI
{
	public partial class MainWnd : Form, IHandleThreadException
	{
		private bool _waiting;

		public MainWnd()
		{
			InitializeComponent();
			deviceView1.ItemMouseClick += ItemSelected;
			_waiting = false;
			ThreadExceptionHandlerCallback = HandleThreadException;
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Form.Load"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data. </param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			SuspendLayout();

			Size = Settings.Default.Size;
			Location = Settings.Default.Location;

			deviceView1.SuspendLayout();

			deviceView1.Clear();
			deviceView1.Add("root", Environment.MachineName, Environment.OSVersion.ToString(), 0,
				GetResourceImage("Computer.png"), null);

			const int indent = 1;
			var ddi = new DiskDriveInfo();
			ddi.PopulateInfo(null);

			foreach (var diskDrive in ddi.DiskDrives)
			{
				var dSize = FormatBytes(diskDrive.Size);
				deviceView1.Add(diskDrive.Name, dSize + " Hard Disk", diskDrive.Name, indent, GetResourceImage("Device.png"),
					diskDrive);
				foreach (var partition in diskDrive.Partitions)
				{
					deviceView1.Add(partition.Name, "Partition " + partition.Index,
						partition.DeviceId, indent + 1, GetResourceImage("Partition.png"), partition);
					foreach (var drive in partition.Drives)
					{
						deviceView1.Add(drive.Name, drive.Name.Substring(0, 1) + " Drive",
							FormatBytes(drive.Size) + " Logical Drive", indent + 2, GetResourceImage("Folder.png"), drive);
					}
				}
			}

			deviceView1.ResumeLayout(true);
			ResumeLayout(true);
		}

		protected Image GetResourceImage(string name)
		{
			Image bmp;
			using (var s = EmbededResources.GetResourceStream(name))
			{
				bmp = Image.FromStream(s);
			}

			return bmp;
		}

		private void ItemSelected(object sender, ItemMouseClickEventArgs e)
		{
			if (e.Item == null || e.Item.UserData == null) return;
			var info = e.Item.UserData as InfoBase;
			if (info == null || info.Properties == null) return;

			if (info is LogicalDrive)
			{
				var fi = new DirectoryInfo(info.Name + "\\");
				var node = new SizeNode(fi);

				Task.Factory.StartNew(() => FileSizeHelper.BuildTree(node, UpdateViewItem))
					.ContinueWith((task, o) =>
					{
						var n = o as ISizeNode;
						if (n == null) return;

						BindDirectoryView(n);
					}, node);
			}

			_waiting = true;
//			animCircle.Visible = true;
//			animCircle.Start();
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

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Form.Activated"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data. </param>
		protected override void OnActivated(EventArgs e)
		{
			base.OnActivated(e);
			if (_waiting)
			{
//				animCircle.Visible = true;
//				animCircle.Start();
			}
//			else
//				animCircle.Visible = false;
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			Settings.Default.Size = Size;
			Settings.Default.Location = Location;
			Settings.Default.Save();

			base.OnClosing(e);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Form.Deactivate"/> event.
		/// </summary>
		/// <param name="e">The <see cref="T:System.EventArgs"/> that contains the event data. </param>
		protected override void OnDeactivate(EventArgs e)
		{
			base.OnDeactivate(e);
//			animCircle.Stop();
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Form.ResizeBegin"/> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.EventArgs"/> that contains the event data. </param>
		protected override void OnResizeBegin(EventArgs e)
		{
			base.OnResizeBegin(e);
//			animCircle.Stop();
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Form.ResizeEnd"/> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.EventArgs"/> that contains the event data. </param>
		protected override void OnResizeEnd(EventArgs e)
		{
			base.OnResizeEnd(e);
//			animCircle.Start();
		}

		private void PanelSizeChanged(object sender, EventArgs e)
		{
//			animCircle.Left = panel1.Width / 2 - animCircle.Width / 2;
//			animCircle.Top = panel1.Height / 2 - animCircle.Height / 2;
		}

		private void HandleThreadException(object sender, Exception ex)
		{
			MessageBox.Show("Error occurs" + ex, "Error in Thread", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}

		public ThreadExceptionHandler ThreadExceptionHandlerCallback { get; set; }


		public void UpdateViewItem(ListViewItem item, ISizeNode node)
		{
			if (item == null) return;

			if (directoryView1.InvokeRequired)
			{
				var d = new UpdateViewItemCallback(UpdateViewItem);
				directoryView1.Invoke(d, new object[] { item, node });
			}
			else
			{
				item.SubItems["_colSize"].Text = CommonFunction.ConvertByte(node.Size);
				item.SubItems["_colPercent"].Text = string.Format("{0:P}", node.Percentage);
			}
		}

		private void BindDirectoryView(ISizeNode node)
		{
			if (directoryView1.InvokeRequired)
				directoryView1.Invoke(new Action<ISizeNode>(BindDirectoryView), new object[] {node});
			else
			{
				directoryView1.Clear();
				directoryView1.Bind(node);
			}
		}
	}
}
