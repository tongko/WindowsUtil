﻿namespace FolderSizeScanner.UI
{
	partial class DialogPickDrive
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
			this.label1 = new System.Windows.Forms.Label();
			this.btnScan = new System.Windows.Forms.Button();
			this.ttSelectDrive = new System.Windows.Forms.ToolTip(this.components);
			this.cmbDrive = new FolderSizeScanner.UI.CustomCombo();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(13, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(121, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "Select drive to scan:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnScan
			// 
			this.btnScan.Location = new System.Drawing.Point(240, 14);
			this.btnScan.Name = "btnScan";
			this.btnScan.Size = new System.Drawing.Size(75, 23);
			this.btnScan.TabIndex = 2;
			this.btnScan.Text = "&Start";
			this.btnScan.UseVisualStyleBackColor = true;
			this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
			// 
			// ttSelectDrive
			// 
			this.ttSelectDrive.ForeColor = System.Drawing.Color.Red;
			this.ttSelectDrive.IsBalloon = true;
			this.ttSelectDrive.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Error;
			this.ttSelectDrive.ToolTipTitle = "Choose a drive.";
			// 
			// cmbDrive
			// 
			this.cmbDrive.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
			this.cmbDrive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbDrive.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmbDrive.FormattingEnabled = true;
			this.cmbDrive.Location = new System.Drawing.Point(156, 13);
			this.cmbDrive.Name = "cmbDrive";
			this.cmbDrive.Size = new System.Drawing.Size(78, 24);
			this.cmbDrive.TabIndex = 1;
			this.cmbDrive.DropDown += new System.EventHandler(this.cmbDrive_DropDown);
			// 
			// DialogPickDrive
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(322, 57);
			this.Controls.Add(this.btnScan);
			this.Controls.Add(this.cmbDrive);
			this.Controls.Add(this.label1);
			this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DialogPickDrive";
			this.Text = "Choose a drive...";
			this.Load += new System.EventHandler(this.DialogPickDrive_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private CustomCombo cmbDrive;
		private System.Windows.Forms.Button btnScan;
		private System.Windows.Forms.ToolTip ttSelectDrive;
	}
}