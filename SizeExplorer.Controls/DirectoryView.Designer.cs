namespace SizeExplorer.Controls
{
	partial class DirectoryView
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DirectoryView));
			this.listView1 = new System.Windows.Forms.ListView();
			this._colFolder = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this._colSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this._colPercent = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.imageList2 = new System.Windows.Forms.ImageList(this.components);
			this.animCircle = new SizeExplorer.Controls.AnimatedCircle();
			this.SuspendLayout();
			// 
			// listView1
			// 
			this.listView1.BackColor = System.Drawing.SystemColors.Window;
			this.listView1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("listView1.BackgroundImage")));
			this.listView1.BackgroundImageTiled = true;
			this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._colFolder,
            this._colSize,
            this._colPercent});
			this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listView1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.listView1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.listView1.FullRowSelect = true;
			this.listView1.LargeImageList = this.imageList1;
			this.listView1.Location = new System.Drawing.Point(0, 0);
			this.listView1.MultiSelect = false;
			this.listView1.Name = "listView1";
			this.listView1.ShowGroups = false;
			this.listView1.ShowItemToolTips = true;
			this.listView1.Size = new System.Drawing.Size(488, 512);
			this.listView1.SmallImageList = this.imageList2;
			this.listView1.TabIndex = 0;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = System.Windows.Forms.View.Details;
			this.listView1.DoubleClick += new System.EventHandler(this.DoubleClicked);
			// 
			// _colFolder
			// 
			this._colFolder.Text = "Folder Name";
			this._colFolder.Width = 250;
			// 
			// _colSize
			// 
			this._colSize.Text = "Size";
			this._colSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this._colSize.Width = 120;
			// 
			// _colPercent
			// 
			this._colPercent.Text = "%";
			this._colPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// imageList1
			// 
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "Folder2.png");
			this.imageList1.Images.SetKeyName(1, "File.png");
			// 
			// imageList2
			// 
			this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
			this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList2.Images.SetKeyName(0, "Folder2.png");
			this.imageList2.Images.SetKeyName(1, "File.png");
			// 
			// animCircle
			// 
			this.animCircle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.animCircle.BackColor = System.Drawing.Color.Transparent;
			this.animCircle.DebugMode = false;
			this.animCircle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(183)))), ((int)(((byte)(229)))));
			this.animCircle.Location = new System.Drawing.Point(363, 389);
			this.animCircle.Name = "animCircle";
			this.animCircle.ShadowWidth = 0F;
			this.animCircle.Size = new System.Drawing.Size(88, 88);
			this.animCircle.TabIndex = 1;
			this.animCircle.Visible = false;
			// 
			// DirectoryView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.Controls.Add(this.animCircle);
			this.Controls.Add(this.listView1);
			this.Name = "DirectoryView";
			this.Size = new System.Drawing.Size(488, 512);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.ColumnHeader _colFolder;
		private System.Windows.Forms.ColumnHeader _colSize;
		private System.Windows.Forms.ColumnHeader _colPercent;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.ImageList imageList2;
		private AnimatedCircle animCircle;
	}
}
