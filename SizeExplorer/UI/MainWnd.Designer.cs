
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
			this.imglIcon = new System.Windows.Forms.ImageList(this.components);
			this.deviceView1 = new SizeExplorer.Controls.DeviceView();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.SuspendLayout();
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
			this.splitContainer1.Size = new System.Drawing.Size(772, 652);
			this.splitContainer1.SplitterDistance = 320;
			this.splitContainer1.TabIndex = 0;
			// 
			// imglIcon
			// 
			this.imglIcon.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imglIcon.ImageStream")));
			this.imglIcon.TransparentColor = System.Drawing.Color.Transparent;
			this.imglIcon.Images.SetKeyName(0, "Folder.ico");
			this.imglIcon.Images.SetKeyName(1, "File.ico");
			// 
			// deviceView1
			// 
			this.deviceView1.AutoScroll = true;
			this.deviceView1.BackColor = System.Drawing.SystemColors.Window;
			this.deviceView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.deviceView1.IndentWidth = 0;
			this.deviceView1.Location = new System.Drawing.Point(0, 0);
			this.deviceView1.Margin = new System.Windows.Forms.Padding(5);
			this.deviceView1.Name = "deviceView1";
			this.deviceView1.SelectedIndex = 0;
			this.deviceView1.Size = new System.Drawing.Size(320, 652);
			this.deviceView1.TabIndex = 0;
			// 
			// MainWnd
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(772, 652);
			this.Controls.Add(this.splitContainer1);
			this.Name = "MainWnd";
			this.Text = "Size Explorer";
			this.splitContainer1.Panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.ImageList imglIcon;
		private Controls.DeviceView deviceView1;
	}
}