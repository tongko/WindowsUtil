using Delimon.Win32.IO;
using SizeExplorer.Controls;
using SizeExplorer.Core;
using SizeExplorer.Model;
using SizeExplorer.Properties;
using SizeExplorer.UI.Resources;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Speech.Synthesis;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SizeExplorer.UI
{
	public partial class MainWnd : Form, IHandleThreadException
	{
		public MainWnd()
		{
			InitializeComponent();
			deviceView1.ItemMouseClick += ItemSelected;
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

			var speech = new SpeechSynthesizer();
			speech.SetOutputToDefaultAudioDevice();
			speech.SpeakAsync("Complete loading Size Explorer User Interface.");
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

			ulong fsoCount = 0;

			if (info is LogicalDrive)
			{
				if (SizeExplorerRuntime.SizeNodes.ContainsKey(info.Name))
				{
					directoryView1.SetState(true);
					var n = SizeExplorerRuntime.SizeNodes[info.Name];
					if (n == null) return;

					BindDirectoryView(n);
					directoryView1.SetState(false);
				}
				else
				{
					var fi = new DirectoryInfo(info.Name + "\\");
					var node = new SizeNode(fi);

					Task.Factory.StartNew(() =>
					{
						directoryView1.SetState(true);
						fsoCount = FileSizeHelper.Build(node);
					})
						.ContinueWith((task, o) =>
						{
							var n = o as ISizeNode;
							if (n == null) return;

							BindDirectoryView(n);
							SizeExplorerRuntime.SizeNodes.Add(info.Name, n);
							directoryView1.SetState(false);
						}, node)
						.ContinueWith((task, o) =>
						{
							var n = o as ISizeNode;
							if (n == null) return;

							directoryView1.SetState(true);
							var job = new CalculateJob(n, ReportProgress, fsoCount);
							job.StartJob();
							//FileSizeHelper.CalculateSize(n);
							directoryView1.SetState(false);
						}, node);
				}
			}
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

		protected override void OnClosing(CancelEventArgs e)
		{
			Settings.Default.Size = Size;
			Settings.Default.Location = Location;
			Settings.Default.Save();

			base.OnClosing(e);
		}

		private static void HandleThreadException(object sender, Exception ex)
		{
			MessageBox.Show("Error occurs " + ex, "Error in Thread", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
				item.SubItems[1].Text = CommonFunction.ConvertByte(node.Size);
				item.SubItems[2].Text = string.Format("{0:0.00}", node.Percentage);
				if (node.Percentage > 50)
				{
					item.Font = new Font(item.Font, FontStyle.Bold | FontStyle.Underline);
					item.ForeColor = Color.Red;
				}
				else if (node.Percentage > 30)
				{
					item.Font = new Font(item.Font, FontStyle.Bold | FontStyle.Underline);
					item.ForeColor = Color.LightBlue;
				}
				else if (node.Percentage > 20)
				{
					item.Font = new Font(item.Font, FontStyle.Bold | FontStyle.Underline);
					item.ForeColor = Color.GreenYellow;
				}
				else
				{
					item.Font = directoryView1.Font;
					item.ForeColor = Color.FromArgb(224, 224, 224);
				}
			}
		}

		private void BindDirectoryView(ISizeNode node)
		{
			if (directoryView1.InvokeRequired)
				directoryView1.Invoke(new Action<ISizeNode>(BindDirectoryView), new object[] { node });
			else
			{
				directoryView1.Clear();
				directoryView1.Bind(node);
			}
		}

		private void AddViewItem(ListViewItem dummy, ISizeNode node)
		{
			if (directoryView1.InvokeRequired)
				directoryView1.Invoke(new Action<ListViewItem, ISizeNode>(AddViewItem), new object[] { dummy, node });
			else
			{
				directoryView1.BindChild(node);
				directoryView1.Invalidate();
			}
		}

		private void ReportProgress(int progress, string description)
		{
			if (statusStrip1.InvokeRequired)
				statusStrip1.Invoke(new Action<int, string>(ReportProgress), new object[] { progress, description });
			else
			{
				tsProgress.Value = progress;
				statusLabel.Text = "Completed " + description;
			}
		}
	}
}
