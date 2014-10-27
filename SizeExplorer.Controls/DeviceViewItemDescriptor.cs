using System;
using System.Windows.Forms;

namespace SizeExplorer.Controls
{
	public partial class DeviceViewItemDescriptor : UserControl
	{
		public event EventHandler TitleChanged;
		public event EventHandler<TextChangeEventArgs> TitleChanging;
		public event EventHandler DescriptionChanged;
		public event EventHandler<TextChangeEventArgs> DescriptionChanging;

		public DeviceViewItemDescriptor()
		{
			InitializeComponent();
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
	}

	public class TextChangeEventArgs : EventArgs
	{
		public string NewText { get; set; }
	}
}
