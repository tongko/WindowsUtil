using System;
using System.Drawing;
using System.Windows.Forms;

namespace SizeExplorer.Controls
{
	public partial class DeviceViewItemDescriptor : UserControl
	{
		public event EventHandler TitleChanged;
		public event EventHandler<TextChangeEventArgs> TitleChanging;
		public event EventHandler DescriptionChanged;
		public event EventHandler<TextChangeEventArgs> DescriptionChanging;

		private Color _backColor;

		public DeviceViewItemDescriptor()
		{
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			InitializeComponent();
			_backColor = BackColor;
		}

		public string Title
		{
			get { return lblTitle.Text; }
			set
			{
				OnTitleChanging(ref value);
				lblTitle.Text = value;
				OnTitleChanged();
			}
		}

		public string Description
		{
			get { return lblDesc.Text; }
			set
			{
				OnDescriptionChanging(ref value);
				lblDesc.Text = value;
				OnDescriptionChanged();
			}
		}

		public Color HighlightColor { get; set; }

		public void SetHighlighted(bool highlighted)
		{
			BackColor = highlighted ? HighlightColor : _backColor;
			lblTitle.BackColor = BackColor;
			lblDesc.BackColor = BackColor;
			Invalidate();
		}

		protected virtual void OnTitleChanging(ref string newTitle)
		{
			if (TitleChanging != null)
				TitleChanging(this, new TextChangeEventArgs { NewText = newTitle });
		}

		protected virtual void OnTitleChanged()
		{
			if (TitleChanged != null)
				TitleChanged(this, EventArgs.Empty);
		}

		protected virtual void OnDescriptionChanging(ref string newDescription)
		{
			if (DescriptionChanging != null)
				DescriptionChanging(this, new TextChangeEventArgs { NewText = newDescription });
		}

		protected virtual void OnDescriptionChanged()
		{
			if (DescriptionChanged != null)
				DescriptionChanged(this, EventArgs.Empty);
		}

		//private void ChildMosueEnter(object sender, EventArgs e)
		//{
		//	OnMouseEnter(e);
		//}

		//private void ChildMouseLeave(object sender, EventArgs e)
		//{
		//	OnMouseLeave(e);
		//}
	}

	public class TextChangeEventArgs : EventArgs
	{
		public string NewText { get; set; }
	}
}
