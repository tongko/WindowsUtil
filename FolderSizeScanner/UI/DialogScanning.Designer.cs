namespace FolderSizeScanner.UI
{
	partial class DialogScanning
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
			this.lblItem = new System.Windows.Forms.Label();
			this.progress = new System.Windows.Forms.ProgressBar();
			this.lblAction = new System.Windows.Forms.Label();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.animCircle = new FolderSizeScanner.UI.AnimatedCircle();
			this.SuspendLayout();
			// 
			// lblItem
			// 
			this.lblItem.AutoEllipsis = true;
			this.lblItem.Location = new System.Drawing.Point(98, 53);
			this.lblItem.Name = "lblItem";
			this.lblItem.Size = new System.Drawing.Size(491, 20);
			this.lblItem.TabIndex = 0;
			this.lblItem.Text = "lblItem";
			// 
			// progress
			// 
			this.progress.Location = new System.Drawing.Point(26, 112);
			this.progress.MarqueeAnimationSpeed = 50;
			this.progress.Name = "progress";
			this.progress.Size = new System.Drawing.Size(563, 23);
			this.progress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.progress.TabIndex = 1;
			// 
			// lblAction
			// 
			this.lblAction.AutoSize = true;
			this.lblAction.Location = new System.Drawing.Point(98, 27);
			this.lblAction.Name = "lblAction";
			this.lblAction.Size = new System.Drawing.Size(58, 17);
			this.lblAction.TabIndex = 2;
			this.lblAction.Text = "lblAction";
			// 
			// btnOk
			// 
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(371, 156);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(137, 26);
			this.btnOk.TabIndex = 3;
			this.btnOk.Text = "Run in background";
			this.btnOk.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(514, 156);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 26);
			this.btnCancel.TabIndex = 4;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// animCircle
			// 
			this.animCircle.BackColor = System.Drawing.Color.Transparent;
			this.animCircle.DebugMode = false;
			this.animCircle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(183)))), ((int)(((byte)(229)))));
			this.animCircle.Location = new System.Drawing.Point(12, 13);
			this.animCircle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.animCircle.Name = "animCircle";
			this.animCircle.ShadowWidth = 0F;
			this.animCircle.Size = new System.Drawing.Size(80, 80);
			this.animCircle.TabIndex = 5;
			// 
			// DialogScanning
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(615, 194);
			this.Controls.Add(this.animCircle);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.lblAction);
			this.Controls.Add(this.progress);
			this.Controls.Add(this.lblItem);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DialogScanning";
			this.Text = "Scanning";
			this.TopMost = true;
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblItem;
		private System.Windows.Forms.ProgressBar progress;
		private System.Windows.Forms.Label lblAction;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private AnimatedCircle animCircle;
	}
}