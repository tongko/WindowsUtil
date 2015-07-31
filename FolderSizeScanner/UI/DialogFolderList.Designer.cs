namespace FolderSizeScanner.UI
{
	partial class DialogFolderList
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
			this.folderList1 = new FolderSizeScanner.UI.FolderList();
			this.SuspendLayout();
			// 
			// folderList1
			// 
			this.folderList1.BackColor = System.Drawing.SystemColors.Window;
			this.folderList1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.folderList1.Drive = null;
			this.folderList1.Location = new System.Drawing.Point(0, 0);
			this.folderList1.Name = "folderList1";
			this.folderList1.Size = new System.Drawing.Size(740, 704);
			this.folderList1.SizeNode = null;
			this.folderList1.TabIndex = 0;
			// 
			// DialogFolderList
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(740, 704);
			this.Controls.Add(this.folderList1);
			this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DialogFolderList";
			this.Text = "DialogFolderList";
			this.ResumeLayout(false);

		}

		#endregion

		private FolderList folderList1;
	}
}