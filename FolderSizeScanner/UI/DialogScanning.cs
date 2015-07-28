using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FolderSizeScanner.Core;

namespace FolderSizeScanner.UI
{
	partial class DialogScanning : Form
	{
		private CancellationTokenSource _cts;
		private readonly AnalyzeDriveJob _job;
		private Task<ISizeNode> _sizeNode;

		public DialogScanning(AnalyzeDriveJob job)
		{
			_job = job;
			_job.JobStart += _job_JobStart;
			_job.JobCompleted += _job_JobCompleted;
			_job.JobCanceled += _job_JobCanceled;
			_job.ReportProgress += ReportProgress;
			_job.JobFaulty += _job_JobFaulty;

			InitializeComponent();
		}

		public ISizeNode SizeNode
		{
			get { return _sizeNode.Result; }
		}

		void _job_JobFaulty(object sender, Exception e)
		{
			this.DoInvoke(() =>
			{
				var exception = e as AggregateException;
				if (exception != null)
				{
					exception.Handle(x =>
					{
						MessageBox.Show(this, string.Format("Error occurs while running job.\r\n\r\n{0}", x),
							@"Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return true;
					});
				}
				else
				{
					MessageBox.Show(this, string.Format("Error occurs while running job.\r\n\r\n{0}", e),
						@"Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			});
		}

		void _job_JobCanceled(object sender, EventArgs e)
		{
			btnOk.DoInvoke(() =>
			{
				MessageBox.Show(this, @"Analyze job cancel by user.", @"Job Cancel", MessageBoxButtons.OK,
					MessageBoxIcon.Exclamation);

				progress.Style = ProgressBarStyle.Continuous;
				animCircle.Stop();
				animCircle.Visible = false;
				btnOk.Text = @"Close";
				btnOk.DialogResult = DialogResult.OK;
			});
		}

		private void _job_JobStart(object sender, EventArgs e)
		{
			btnOk.DoInvoke(() =>
			{
				animCircle.Start();
				btnOk.Text = @"Run in background";
			});
		}

		private void _job_JobCompleted(object sender, ulong e)
		{
			btnOk.DoInvoke(() =>
			{
				animCircle.Stop();
				progress.Style = ProgressBarStyle.Continuous;
				progress.Value = 0;
				btnCancel.Enabled = false;
				btnOk.Text = @"Close";
				btnOk.DialogResult = DialogResult.OK;
			});
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			progress.Style = ProgressBarStyle.Marquee;
			_cts = new CancellationTokenSource();
			_sizeNode = _job.StartAsync(_cts.Token);
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			if (_cts != null)
				_cts.Cancel();
		}

		private void ReportProgress(string action, string message, int value)
		{
			progress.DoInvoke(() =>
			{
				if (value > 0)
				{
					lblAction.Text = action;
					lblItem.Text = message;
					progress.Value = value;
				}
				else
				{
					lblAction.Text = action;
					lblItem.Text = message;
				}
			});
		}
	}
}
