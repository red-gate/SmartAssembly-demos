using System;
using System.Reflection;
using System.Security;
using System.Windows.Forms;

using SmartAssembly.SmartExceptionsCore;

namespace SmartAssembly.SmartExceptionsWithUI
{
	public class UnhandledExceptionHandlerWithUI : UnhandledExceptionHandler
	{
		protected override void OnSecurityException(SecurityExceptionEventArgs e)
		{
            using (SecurityExceptionForm form = new SecurityExceptionForm(e))
            {
                form.ShowDialog();
            }
		}

		protected override void OnReportException(ReportExceptionEventArgs e)
		{
            using (ExceptionReportingForm form = new ExceptionReportingForm(this, e))
            {
                form.ShowDialog();
            }
		}

		protected override void OnFatalException(FatalExceptionEventArgs e)
		{
			MessageBox.Show(e.FatalException.ToString(), string.Format(Localization.FatalError, ApplicationName), MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		public static bool AttachApp()
		{
			try
			{
				AttachExceptionHandler(new UnhandledExceptionHandlerWithUI());
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

					string securityMessage = string.Format(Localization.CannotAttachApp, ApplicationName);
					SecurityExceptionForm form = new SecurityExceptionForm(new SecurityExceptionEventArgs(securityMessage, false));
					form.ShowInTaskbar = true;
					form.ShowDialog();
				}
				catch (Exception exception)
				{
					MessageBox.Show(exception.ToString(), string.Format(Localization.FatalError, ApplicationName), MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				return false;
			}
		}
	}
}
