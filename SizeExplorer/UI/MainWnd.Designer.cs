
namespace SizeExplorer.UI
{
	partial class MainWnd
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWnd));
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.deviceView1 = new SizeExplorer.Controls.DeviceView();
			this.panel1 = new System.Windows.Forms.Panel();
			this.directoryView1 = new SizeExplorer.Controls.DirectoryView();
			this.imglIcon = new System.Windows.Forms.ImageList(this.components);
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.deviceView1);
			this.splitContainer1.Panel1MinSize = 250;
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.panel1);
			this.splitContainer1.Size = new System.Drawing.Size(772, 624);
			this.splitContainer1.SplitterDistance = 320;
			this.splitContainer1.TabIndex = 0;
			// 
			// deviceView1
			// 
			this.deviceView1.AutoScroll = true;
			this.deviceView1.AutoScrollMargin = new System.Drawing.Size(0, 10);
			this.deviceView1.BackColor = System.Drawing.SystemColors.Window;
			this.deviceView1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("deviceView1.BackgroundImage")));
			this.deviceView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.deviceView1.IndentWidth = 0;
			this.deviceView1.Location = new System.Drawing.Point(0, 0);
			this.deviceView1.Margin = new System.Windows.Forms.Padding(5);
			this.deviceView1.Name = "deviceView1";
			this.deviceView1.Padding = new System.Windows.Forms.Padding(0, 0, 50, 0);
			this.deviceView1.SelectedIndex = 0;
			this.deviceView1.Size = new System.Drawing.Size(320, 624);
			this.deviceView1.TabIndex = 0;
			// 
			// panel1
			// 
			this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
			this.panel1.Controls.Add(this.directoryView1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(448, 624);
			this.panel1.TabIndex = 0;
			// 
			// directoryView1
			// 
			this.directoryView1.BackColor = System.Drawing.Color.White;
			this.directoryView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.directoryView1.Location = new System.Drawing.Point(0, 0);
			this.directoryView1.Name = "directoryView1";
			this.directoryView1.Size = new System.Drawing.Size(448, 624);
			this.directoryView1.TabIndex = 0;
			// 
			// imglIcon
			// 
			this.imglIcon.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imglIcon.ImageStream")));
			this.imglIcon.TransparentColor = System.Drawing.Color.Transparent;
			this.imglIcon.Images.SetKeyName(0, "Folder.ico");
			this.imglIcon.Images.SetKeyName(1, "File.ico");
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
			this.statusStrip1.Location = new System.Drawing.Point(0, 624);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(772, 22);
			this.statusStrip1.TabIndex = 1;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// statusLabel
			// 
			this.statusLabel.Name = "statusLabel";
			this.statusLabel.Size = new System.Drawing.Size(757, 17);
			this.statusLabel.Spring = true;
			this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// MainWnd
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(772, 646);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.statusStrip1);
			this.Name = "MainWnd";
			this.Text = "Size Explorer";
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.ImageList imglIcon;
		private Controls.DeviceView deviceView1;
		private System.Windows.Forms.Panel panel1;
		private Controls.DirectoryView directoryView1;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel statusLabel;
	}
}