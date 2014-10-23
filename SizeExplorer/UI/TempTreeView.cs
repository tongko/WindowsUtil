using System.Drawing;
using System.Windows.Forms;

namespace SizeExplorer.UI
{
	public class TempTreeView : TreeView
	{
		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.TreeView.DrawNode"/> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DrawTreeNodeEventArgs"/> that contains the event data. </param>
		protected override void OnDrawNode(DrawTreeNodeEventArgs e)
		{
			base.OnDrawNode(e);

			e.DrawDefault = false;
			var g = e.Graphics;
			g.FillRectangle(Brushes.Aqua, e.Bounds);


		}
	}
}
