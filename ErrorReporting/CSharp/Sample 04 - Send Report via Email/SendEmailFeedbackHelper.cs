using System;
using System.Threading;

namespace SmartAssembly.SmartExceptionsViaEmail
{
	public enum SendingReportViaEmailStep
	{
		PreparingReport = 1,
		Transfering = 2,
		Finished = 3
	}

	public delegate void SendingReportViaEmailFeedbackEventHandler(object sender, SendingReportViaEmailFeedbackEventArgs e);

	public class SendingReportViaEmailFeedbackEventArgs : EventArgs
	{
		private SendingReportViaEmailStep step;
		private bool failed = false;
		private string errorMessage = string.Empty;

		public SendingReportViaEmailStep Step
		{
			get
			{
				return step;
			}
		}

		public bool Failed
		{
			get
			{
				return failed;
			}
		}

		public string ErrorMessage
		{
			get
			{
				return errorMessage;
			}
		}

		internal SendingReportViaEmailFeedbackEventArgs(SendingReportViaEmailStep step) : this(step, string.Empty)
		{
		}

		internal SendingReportViaEmailFeedbackEventArgs(SendingReportViaEmailStep step, string errorMessage)
		{
			this.step = step;
			this.failed = errorMessage != null && errorMessage.Length > 0;
			this.errorMessage = errorMessage;
		}
	}
}
