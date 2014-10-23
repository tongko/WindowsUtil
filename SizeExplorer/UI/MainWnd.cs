using SizeExplorer.Model;
using System;
using System.Globalization;
using System.Management;
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
			var root = tvFolder.Nodes.Add("root", "My Computer");

			var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");

			foreach (var media in searcher.Get())
			{
				var wm = Win32PhysicalMedia.FromMedia(media);
				var node = root.Nodes.Add(wm.SerialNumber, wm.Tag);
				node.Nodes.Add(wm.Caption);
				node.Nodes.Add(wm.Description);
				node.Nodes.Add(wm.InstallDate.ToString("yyyy-MM-dd"));
				node.Nodes.Add(wm.Name);
				node.Nodes.Add(wm.Status);
				node.Nodes.Add(wm.CreationClassName);
				node.Nodes.Add(wm.Manufacturer);
				node.Nodes.Add(wm.Model);
				node.Nodes.Add(wm.SKU);
				node.Nodes.Add(wm.SerialNumber);
				node.Nodes.Add(wm.Tag);
				node.Nodes.Add(wm.Version);
				node.Nodes.Add(wm.PartNumber);
				node.Nodes.Add(wm.OtherIdentifyingInfo);
				node.Nodes.Add(wm.PoweredOn.ToString());
				node.Nodes.Add(wm.Removable.ToString());
				node.Nodes.Add(wm.Replaceable.ToString());
				node.Nodes.Add(wm.HotSwappable.ToString());
				node.Nodes.Add(wm.Capacity.ToString(CultureInfo.InvariantCulture));
				node.Nodes.Add(wm.MediaType.ToString(CultureInfo.InvariantCulture));
				node.Nodes.Add(wm.MediaDescription);
				node.Nodes.Add(wm.WriteProtectOn.ToString());
				node.Nodes.Add(wm.CleanerMedia.ToString());
			}

			tvFolder.Update();
		}
	}
}
