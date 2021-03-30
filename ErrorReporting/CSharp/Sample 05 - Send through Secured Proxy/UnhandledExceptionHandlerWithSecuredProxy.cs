using System;
using System.Security;
using System.Windows.Forms;
using SmartAssembly.SmartExceptionsCore;

namespace SmartAssembly.SmartExceptionsWithSecuredProxy
{
	public class UnhandledExceptionHandlerWithSecuredProxy : UnhandledExceptionHandler
	{
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
			MessageBox.Show(e.FatalException.ToString(), string.Format("{0} Fatal Error", ApplicationName), MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		public static bool AttachApp()
		{
			try
			{
				AttachExceptionHandler(new UnhandledExceptionHandlerWithSecuredProxy());
				return true;
			}
			catch (SecurityException)
			{
				try
				{
					Application.EnableVisualStyles();
					string securityMessage = string.Format("{0} cannot initialize itself because some permissions are not granted.\n\nYou probably try to launch {0} in a partial-trust situation. It's usually the case when the application is hosted on a network share.\n\nYou need to run {0} in full-trust, or at least grant it the UnmanagedCode security permission.\n\nTo grant this application the required permission, contact your system administrator, or use the Microsoft .NET Framework Configuration tool.", UnhandledExceptionHandler.ApplicationName);
					SecurityExceptionForm form = new SecurityExceptionForm(new SecurityExceptionEventArgs(securityMessage, false));
					form.ShowInTaskbar = true;
					form.ShowDialog();
				}
				catch (Exception exception)
				{
					MessageBox.Show(exception.ToString(), string.Format("{0} Fatal Error", UnhandledExceptionHandler.ApplicationName), MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				return false;
			}
		}
	}
}
