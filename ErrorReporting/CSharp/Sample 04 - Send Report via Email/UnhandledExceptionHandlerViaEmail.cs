using System;
using System.IO;
using System.Net.Mail;
using System.Reflection;
using System.Security;
using System.Threading;
using System.Windows.Forms;
using SmartAssembly.SmartExceptionsCore;

namespace SmartAssembly.SmartExceptionsViaEmail
{
	public class UnhandledExceptionViaEmailHandler : UnhandledExceptionHandler
	{
		public event SendingReportViaEmailFeedbackEventHandler SendingReportViaEmailFeedback;

		private void SendEmail(string fileName)
        {
            // TODO: Update SMTP details here
            MailMessage email = new MailMessage("bugreport@your_smtp_server.com", "your_email");
            email.Subject = "Exception report";
            SmtpClient client = new SmtpClient("mail.your_smtp_server.com");
            //***********************/

            email.Attachments.Add(new Attachment(fileName));
            client.Send(email);
		}

		public bool SendReportViaEmail(ReportExceptionEventArgs reportExceptionEventArgs)
		{
			try
			{
				if (SendingReportViaEmailFeedback != null) SendingReportViaEmailFeedback(this, new SendingReportViaEmailFeedbackEventArgs(SendingReportViaEmailStep.PreparingReport));

				// WARNING: To open the encrypted report with SA, the file must have a .saencryptedreport extension.
				string tempFileName = Path.GetTempPath() + Guid.NewGuid().ToString("N") + ".saencryptedreport";

				if (reportExceptionEventArgs.SaveEncryptedReport(tempFileName))
				{
					if (SendingReportViaEmailFeedback != null) SendingReportViaEmailFeedback(this, new SendingReportViaEmailFeedbackEventArgs(SendingReportViaEmailStep.Transfering));

					bool transferingFailed = false;

					try
					{
						SendEmail(tempFileName);
					}
					catch(Exception exception)
					{
						if (SendingReportViaEmailFeedback != null) SendingReportViaEmailFeedback(this, new SendingReportViaEmailFeedbackEventArgs(SendingReportViaEmailStep.Transfering, exception.Message));
						transferingFailed = true;
					}

					try
					{
						File.Delete(tempFileName);
					}
					catch
					{
					}

					if (transferingFailed)
					{
						return false;
					}
					else
					{
						if (SendingReportViaEmailFeedback != null) SendingReportViaEmailFeedback(this, new SendingReportViaEmailFeedbackEventArgs(SendingReportViaEmailStep.Finished));
						return true;
					}
				}
				else
				{
					if (SendingReportViaEmailFeedback != null) SendingReportViaEmailFeedback(this, new SendingReportViaEmailFeedbackEventArgs(SendingReportViaEmailStep.PreparingReport, "Cannot create temp file."));
					return false;
				}
			}
			catch (ThreadAbortException)
			{
				return false;
			}
			catch (Exception fatalException)
			{
				MessageBox.Show(fatalException.ToString(), string.Format(Localization.FatalError, ApplicationName), MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
		}

		protected override void OnSecurityException(SecurityExceptionEventArgs e)
		{
			SecurityExceptionForm form = new SecurityExceptionForm(e);
			form.ShowDialog();
		}

		protected override void OnReportException(ReportExceptionEventArgs e)
		{
			ExceptionReportingForm form = new ExceptionReportingForm(this, e);
			form.ShowDialog();
		}

		protected override void OnFatalException(FatalExceptionEventArgs e)
		{
			MessageBox.Show(e.FatalException.ToString(), string.Format(Localization.FatalError, ApplicationName), MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		public static bool AttachApp()
		{
			try
			{
				AttachExceptionHandler(new UnhandledExceptionViaEmailHandler());
				return true;
			}
			catch (SecurityException)
			{
				try
				{
					try
					{
						typeof(Application).InvokeMember("EnableVisualStyles", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null , null, null);
					}
					catch (MissingMethodException)
					{
					}

					string securityMessage = string.Format(Localization.CannotAttachApp, UnhandledExceptionHandler.ApplicationName);
					SecurityExceptionForm form = new SecurityExceptionForm(new SecurityExceptionEventArgs(securityMessage, false));
					form.ShowInTaskbar = true;
					form.ShowDialog();
				}
				catch (Exception exception)
				{
					MessageBox.Show(exception.ToString(), string.Format(Localization.FatalError, UnhandledExceptionHandler.ApplicationName), MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				return false;
			}
		}
	}
}
