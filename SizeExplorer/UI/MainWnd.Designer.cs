﻿namespace SizeExplorer.UI
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
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.tvFolder = new System.Windows.Forms.TreeView();
			this.lvDetails = new System.Windows.Forms.ListView();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
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
			this.splitContainer1.Panel1.Controls.Add(this.tvFolder);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.lvDetails);
			this.splitContainer1.Size = new System.Drawing.Size(772, 652);
			this.splitContainer1.SplitterDistance = 257;
			this.splitContainer1.TabIndex = 0;
			// 
			// tvFolder
			// 
			this.tvFolder.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvFolder.Location = new System.Drawing.Point(0, 0);
			this.tvFolder.Name = "tvFolder";
			this.tvFolder.Size = new System.Drawing.Size(257, 652);
			this.tvFolder.TabIndex = 0;
			// 
			// lvDetails
			// 
			this.lvDetails.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvDetails.Location = new System.Drawing.Point(0, 0);
			this.lvDetails.Name = "lvDetails";
			this.lvDetails.Size = new System.Drawing.Size(511, 652);
			this.lvDetails.TabIndex = 0;
			this.lvDetails.UseCompatibleStateImageBehavior = false;
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
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.TreeView tvFolder;
		private System.Windows.Forms.ListView lvDetails;
	}
}