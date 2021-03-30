using System;
using System.Drawing;
using System.Diagnostics;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Forms;

using SmartAssembly.SmartExceptionsCore;
using SmartAssembly.SmartExceptionsCore.UI;

namespace SmartAssembly.SmartExceptionsViaEmail
{
	[System.ComponentModel.DesignerCategory("Code")]
	internal class ExceptionReportingForm : Form
	{
		private UnhandledExceptionViaEmailHandler unhandledExceptionHandler = null;
		private ReportExceptionEventArgs reportExceptionEventArgs = null;
		private Thread workingThread = null;
		private CheckBox continueCheckBox = new CheckBox();
		private Label pleaseTellTitle = new Label();
		private Button dontSendReport = new Button();
		private Button sendReport = new Button();
		private Label pleaseTellMessage = new Label();
		private AutoHeightLabel errorMessage = new AutoHeightLabel();
		private Panel panelInformation = new Panel();
		private Panel panelSending = new Panel();
		private Button cancelSending = new Button();
		private WaitSendingReportControl waitSendingReport = new WaitSendingReportControl();
		private FeedbackControl preparingFeedback = new FeedbackControl(Localization.PreparingReport);
		private FeedbackControl transferingFeedback = new FeedbackControl(Localization.TransferringReport);
		private FeedbackControl completedFeedback = new FeedbackControl(Localization.CompletedThankYou);
		private Button ok = new Button();
		private Button retrySending = new Button();
		private HeaderControl headerControl1 = new HeaderControl(string.Format(Localization.AppHasEncounteredAProblem, UnhandledExceptionHandler.ApplicationName));
		private HeaderControl headerControl2 = new HeaderControl(string.Format(Localization.PleaseWaitWhileSending, UnhandledExceptionHandler.ApplicationName, UnhandledExceptionHandler.CompanyName));
		private PoweredBy powered = new PoweredBy();
		private Button debug = new Button();

		private void InitializeComponent()
		{
			panelInformation.SuspendLayout();
			panelSending.SuspendLayout();
			SuspendLayout();

			// headerControl1
			headerControl1.IconState = IconState.Error;

			// headerControl2
			headerControl2.IconState = IconState.Error;

			// continueCheckBox 
			continueCheckBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			continueCheckBox.FlatStyle = FlatStyle.System;
			continueCheckBox.Location = new Point(22, 98);
			continueCheckBox.Size = new Size(Localization.ContinueCheckBoxWidth, 16);
			continueCheckBox.TabIndex = 13;
			continueCheckBox.Text = Localization.IgnoreThisError;
			continueCheckBox.CheckedChanged += new EventHandler(OnContinueCheckedChanged);

			// pleaseTellTitle
			pleaseTellTitle.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			pleaseTellTitle.FlatStyle = FlatStyle.System;
			pleaseTellTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			pleaseTellTitle.Location = new Point(20, 124);
			pleaseTellTitle.Size = new Size(381, 16);
			pleaseTellTitle.Text = string.Format(Localization.PleaseTellCompany, UnhandledExceptionHandler.CompanyName);

			// dontSendReport
			dontSendReport.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			dontSendReport.FlatStyle = FlatStyle.System;
			dontSendReport.Size = new Size(Localization.DontSendButtonWidth, 24);
			dontSendReport.Location = new Point(400 - dontSendReport.Width, 205);
			dontSendReport.TabIndex = 4;
			dontSendReport.Text = Localization.DontSendButton;
			dontSendReport.Click += new System.EventHandler(OnDontSendReportClick);

			// sendReport
			sendReport.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			sendReport.FlatStyle = FlatStyle.System;
			sendReport.Size = new Size(Localization.SendReportButtonWidth, 24);
			sendReport.Location = new Point(dontSendReport.Left - sendReport.Width - 6, 205);
			sendReport.TabIndex = 3;
			sendReport.Text = Localization.SendReportButton;
			sendReport.Click += new System.EventHandler(OnSendReportClick);

			// debug
			debug.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			debug.FlatStyle = FlatStyle.System;
			debug.Size = new Size(Localization.DebugButtonWidth, 24);
			debug.Location = new Point(sendReport.Left - debug.Width - 6, 205);
			debug.TabIndex = 14;
			debug.Text = Localization.DebugButton;
			debug.Visible = false;
			debug.Click += new System.EventHandler(OnDebugClick);

			// pleaseTellMessage
			pleaseTellMessage.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			pleaseTellMessage.FlatStyle = FlatStyle.System;
			pleaseTellMessage.Location = new Point(20, 140);
			pleaseTellMessage.Size = new Size(381, 55);
			pleaseTellMessage.Text = string.Format(Localization.CompanyInterestedInLearning, UnhandledExceptionHandler.CompanyName);

			// errorMessage
			errorMessage.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			errorMessage.Location = new Point(20, 69);
			errorMessage.Size = new Size(381, 13);

			cancelSending.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			cancelSending.FlatStyle = FlatStyle.System;
			cancelSending.Size = new Size(Localization.CancelButtonWidth, 24);
			cancelSending.Location = new Point(400 - cancelSending.Width, 205);
			cancelSending.TabIndex = 7;
			cancelSending.Text = Localization.CancelButton;
			cancelSending.Click += new System.EventHandler(OnCancelSending);

			// ok
			ok.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			ok.Enabled = false;
			ok.FlatStyle = FlatStyle.System;
			ok.Size = new Size(Localization.OKRetryButtonWidth, 24);
			ok.Location = new Point(cancelSending.Left - ok.Width - 6, 205);
			ok.TabIndex = 6;
			ok.Text = Localization.OKButton;
			ok.Click += new System.EventHandler(OnOK);

			// retrySending
			retrySending.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			retrySending.FlatStyle = FlatStyle.System;
			retrySending.Location = ok.Location;
			retrySending.Size = ok.Size;
			retrySending.TabIndex = 5;
			retrySending.Text = Localization.RetryButton;
			retrySending.Visible = false;
			retrySending.Click += new System.EventHandler(OnRetry);

			// waitSendingReport
			waitSendingReport.Location = new Point(87, 146);
			waitSendingReport.Visible = false;

			//feedback controls
			preparingFeedback.SetBounds(24, 72, 368, 16);
			transferingFeedback.SetBounds(24, 96, 368, 16);
			completedFeedback.SetBounds(24, 120, 368, 16);

			// Powered by {sa}
			powered.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			powered.SetBounds(6, 450, 120, 32);

			// panelInformation
			panelInformation.Controls.AddRange(new Control[]{debug, continueCheckBox, pleaseTellTitle, dontSendReport, sendReport, pleaseTellMessage, errorMessage, headerControl1});
			panelInformation.Size = new Size(413, 240);
			panelInformation.TabIndex = 0;

			// panelSending
			panelSending.Controls.AddRange(new Control[]{cancelSending, ok, retrySending, waitSendingReport, headerControl2, preparingFeedback, transferingFeedback, completedFeedback});
			panelSending.Size = new Size(413, 240);
			panelSending.TabIndex = 2;
			panelSending.Visible = false;

			// ExceptionReportingForm
			AutoScaleBaseSize = new Size(5, 13);
			ClientSize = new Size(434, 488);
			ControlBox = false;
			Controls.AddRange(new Control[]{powered, panelInformation, panelSending});
			FormBorderStyle = FormBorderStyle.FixedSingle;
			ShowInTaskbar = false;
			MinimizeBox = false;
			MaximizeBox = false;
			StartPosition = FormStartPosition.CenterScreen;
			Text = UnhandledExceptionHandler.ApplicationName;
			if (Text.Length == 0) Text = Localization.ErrorReporting;
			TopMost = true;
			panelInformation.ResumeLayout(false);
			panelSending.ResumeLayout(false);
			ResumeLayout(false);

			retrySending.BringToFront();
			Size = new Size(Localization.Width, Localization.Height);

			panelSending.Dock = DockStyle.Fill;
			panelInformation.Dock = DockStyle.Fill;
		}

		private void OnSendReportClick(object sender, System.EventArgs e)
		{
			try
			{
				panelInformation.Visible = false;
				panelSending.Visible = true;
				powered.Visible = true;
				if (reportExceptionEventArgs != null) StartWorkingThread(new ThreadStart(StartSendReport));
			}
			catch
			{
			}
		}

		private void StartWorkingThread(ThreadStart start)
		{
			workingThread = new Thread(start);
			workingThread.Start();
		}

		private void OnDontSendReportClick(object sender, System.EventArgs e)
		{
			Close();
		}

		private void OnCancelSending(object sender, System.EventArgs e)
		{
			try
			{
				if (workingThread != null) workingThread.Abort();
			}
			catch
			{
			}
			Close();
		}

		private void OnOK(object sender, System.EventArgs e)
		{
			Close();
		}

		private void OnContinueCheckedChanged(object sender, System.EventArgs e)
		{
			reportExceptionEventArgs.TryToContinue = continueCheckBox.Checked;
		}

		private void OnFeedback(object sender, SendingReportViaEmailFeedbackEventArgs e)
		{
			try
			{
				Invoke(new SendingReportViaEmailFeedbackEventHandler(Feedback), new object[]{sender, e});
			}
			catch (InvalidOperationException)
			{
			}
		}

		private void OnDebuggerLaunched(object sender, EventArgs e)
		{
			try
			{
				Invoke(new EventHandler(DebuggerLaunched), new object[]{sender, e});
			}
			catch (InvalidOperationException)
			{
			}
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			if (workingThread != null && workingThread.IsAlive)
			{
				workingThread.Abort();
			}
			base.OnClosing(e);
		}

		private void Feedback(object sender, SendingReportViaEmailFeedbackEventArgs e)
		{
			switch (e.Step)
			{
				case SendingReportViaEmailStep.PreparingReport:
					if (e.Failed)
					{
						preparingFeedback.Stop(e.ErrorMessage);
						retrySending.Visible = true;
						retrySending.Focus();
					}
					else
					{
						preparingFeedback.Start();
					}
					break;

				case SendingReportViaEmailStep.Transfering:
					if (e.Failed)
					{
						waitSendingReport.Visible = false;
						transferingFeedback.Stop(e.ErrorMessage);
						retrySending.Visible = true;
						retrySending.Focus();
					}
					else
					{
						preparingFeedback.Stop();
						transferingFeedback.Start();
						waitSendingReport.Visible = true;
					}
					break;

				case SendingReportViaEmailStep.Finished:
					waitSendingReport.Visible = false;
					transferingFeedback.Stop();
					completedFeedback.Stop();
					ok.Enabled = true;
					ok.Focus();
					cancelSending.Enabled = false;
					break;
			}
		}

		private void DebuggerLaunched(object sender, EventArgs e)
		{
			Close();
		}

		private void OnRetry(object sender, System.EventArgs e)
		{
			retrySending.Visible = false;
			preparingFeedback.Init();
			transferingFeedback.Init();
			if (reportExceptionEventArgs != null) StartWorkingThread(new ThreadStart(StartSendReport));
		}

		private void StartSendReport()
		{
			unhandledExceptionHandler.SendReportViaEmail(reportExceptionEventArgs);
		}

		private void OnDebugClick(object sender, System.EventArgs e)
		{
			if (reportExceptionEventArgs != null) StartWorkingThread(new ThreadStart(reportExceptionEventArgs.LaunchDebugger));
		}

		public ExceptionReportingForm(UnhandledExceptionViaEmailHandler unhandledExceptionHandler, ReportExceptionEventArgs reportExceptionEventArgs)
		{
			InitializeComponent();

			int newHeight = Height;

			this.reportExceptionEventArgs = reportExceptionEventArgs;
			this.unhandledExceptionHandler = unhandledExceptionHandler;
			errorMessage.Text = reportExceptionEventArgs.Exception.Message;

			newHeight += (errorMessage.Height - FontHeight);

			if (!reportExceptionEventArgs.ShowContinueCheckbox)
			{
				continueCheckBox.Visible = false;
				newHeight -= (continueCheckBox.Height);
			}

			if (newHeight > Height) Height = newHeight;

			if (reportExceptionEventArgs.CanDebug)
			{
				unhandledExceptionHandler.DebuggerLaunched += new EventHandler(OnDebuggerLaunched);
				debug.Visible = true;
				if (debug.Left < powered.Right) powered.Visible = false;
			}

			if (!reportExceptionEventArgs.CanSendReport)
			{
				sendReport.Enabled = false;
				if (dontSendReport.CanFocus) dontSendReport.Focus();
			}

			unhandledExceptionHandler.SendingReportViaEmailFeedback += new SendingReportViaEmailFeedbackEventHandler(OnFeedback);
		}

	}
}
