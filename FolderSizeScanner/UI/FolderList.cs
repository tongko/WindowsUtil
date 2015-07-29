using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using FolderSizeScanner.Core;

namespace FolderSizeScanner.UI
{
	public partial class FolderList : Control
	{
		private DriveInfo _drive;
		private ISizeNode _sizeNode;

		public event EventHandler DriveChanged = delegate { };
		public event EventHandler SizeNodeChanged = delegate { };

		public FolderList()
		{
			InitializeComponent();
		}

		public ISizeNode SizeNode
		{
			get { return _sizeNode; }
			set
			{
				_sizeNode = value;
				OnSizeNodeChanged(EventArgs.Empty);
			}
		}

		public DriveInfo Drive
		{
			get { return _drive; }
			set
			{
				_drive = value;
				OnDriveChanged(EventArgs.Empty);
			}
		}

		protected virtual void OnDriveChanged(EventArgs e)
		{
			DriveChanged(this, e);
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			var g = pe.Graphics;

			g.FillRectangle(Brushes.White, Bounds);

			if (Drive != null)
			{

			}
		}

		protected virtual void OnSizeNodeChanged(EventArgs e)
		{
			SizeNodeChanged(this, e);
		}

	}
}
