namespace FolderSizeScanner.UI
{
	partial class FolderList
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
			this.pcCap = new FolderSizeScanner.UI.Charts.PieChart();
			this.SuspendLayout();
			// 
			// pcCap
			// 
			this.pcCap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.pcCap.Location = new System.Drawing.Point(340, 40);
			this.pcCap.Name = "pcCap";
			this.pcCap.Size = new System.Drawing.Size(300, 120);
			this.pcCap.TabIndex = 0;
			this.pcCap.ToolTips = null;
			// 
			// FolderList
			// 
			this.BackColor = System.Drawing.SystemColors.Window;
			this.Controls.Add(this.pcCap);
			this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "FolderList";
			this.Size = new System.Drawing.Size(660, 533);
			this.ResumeLayout(false);

		}

		#endregion

		private Charts.PieChart pcCap;

	}
}
