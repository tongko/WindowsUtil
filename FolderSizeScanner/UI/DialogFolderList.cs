using System.IO;
using System.Windows.Forms;
using FolderSizeScanner.Core;

namespace FolderSizeScanner.UI
{
	public partial class DialogFolderList : Form
	{
		public DialogFolderList(DriveInfo drive, ISizeNode sizeNode)
		{
			InitializeComponent();
			folderList1.Drive = drive;
			folderList1.SizeNode = sizeNode;
		}
	}
}
