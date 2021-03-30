using System;
using System.Drawing;
using System.Diagnostics;
using System.Collections;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;

using SmartAssembly.SmartExceptionsCore;

namespace SmartAssembly.SmartExceptionsWithAdvancedUI
{
	internal class ExceptionReportingForm : System.Windows.Forms.Form
	{
		private UnhandledExceptionHandler unhandledExceptionHandler;
		private ReportExceptionEventArgs reportExceptionEventArgs;
		private Thread workingThread;

		private System.Windows.Forms.CheckBox continueCheckBox;
		private System.Windows.Forms.Label pleaseTellTitle;
		private System.Windows.Forms.Button dontSendReport;
		private System.Windows.Forms.Button sendReport;
		private System.Windows.Forms.Label pleaseTellMessage;
		private System.Windows.Forms.Panel panelInformation;
		private System.Windows.Forms.Panel panelSending;
		private System.Windows.Forms.Button cancelSending;
		private SmartAssembly.SmartExceptionsCore.UI.WaitSendingReportControl waitSendingReport;
		private SmartAssembly.SmartExceptionsCore.UI.FeedbackControl preparingFeedback;
		private SmartAssembly.SmartExceptionsCore.UI.FeedbackControl connectingFeedback;
		private SmartAssembly.SmartExceptionsCore.UI.FeedbackControl transferingFeedback;
		private SmartAssembly.SmartExceptionsCore.UI.FeedbackControl completedFeedback;
		private System.Windows.Forms.Button ok;
		private System.Windows.Forms.Button retrySending;
		private SmartAssembly.SmartExceptionsCore.UI.HeaderControl headerControl1;
		private SmartAssembly.SmartExceptionsCore.UI.HeaderControl headerControl2;
		private System.Windows.Forms.Button debug;
		private System.Windows.Forms.Panel panelEmail;
		private System.Windows.Forms.Label label3;
		private SmartAssembly.SmartExceptionsCore.UI.HeaderControl headerControl3;
		private System.Windows.Forms.Button continueSendReport;
		private System.Windows.Forms.TextBox email;
		private System.Windows.Forms.Label labelEmail;
		private System.Windows.Forms.CheckBox sendAnonymously;
		private SmartAssembly.SmartExceptionsCore.UI.AutoHeightLabel errorMessage;
		private SmartAssembly.SmartExceptionsCore.UI.PoweredBy poweredBy;
		private System.Windows.Forms.Button saveAsFile;
        private Button saveReport;
	    private bool alreadyRetried;

	    public ExceptionReportingForm(UnhandledExceptionHandler unhandledExceptionHandler, ReportExceptionEventArgs reportExceptionEventArgs) : this()
		{
			int newHeight = Height;

			this.reportExceptionEventArgs = reportExceptionEventArgs;
			this.unhandledExceptionHandler = unhandledExceptionHandler;
			this.errorMessage.Text = reportExceptionEventArgs.Exception.Message;

			newHeight += (this.errorMessage.Height - FontHeight);

			if (!reportExceptionEventArgs.ShowContinueCheckbox)
			{
				this.continueCheckBox.Visible = false;
				newHeight -= (this.continueCheckBox.Height);
			}

			if (newHeight > Height) Height = newHeight;

			if (reportExceptionEventArgs.CanDebug)
			{
				unhandledExceptionHandler.DebuggerLaunched += new EventHandler(OnDebuggerLaunched);
				debug.Visible = true;
				poweredBy.Visible = false;
			}

			if (!reportExceptionEventArgs.CanSendReport)
			{
				sendReport.Enabled = false;
				if (dontSendReport.CanFocus) dontSendReport.Focus();
			}

			this.email.Text = RegistryHelper.ReadHKLMRegistryString("Email");

			unhandledExceptionHandler.SendingReportFeedback += new SendingReportFeedbackEventHandler(OnFeedback);
		}

		public ExceptionReportingForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			this.Size = new Size(419, 264);
			this.MinimizeBox = false;
			this.MaximizeBox = false;
			this.panelInformation.Location = Point.Empty;
			this.panelInformation.Dock = DockStyle.Fill;

			this.retrySending.Location = ok.Location;
			this.retrySending.Size = ok.Size;
			this.retrySending.BringToFront();

			this.panelSending.Location = Point.Empty;
			this.panelSending.Dock = DockStyle.Fill;
			this.Text = GetConvertedString(this.Text);
			
			this.panelEmail.Location = Point.Empty;
			this.panelEmail.Dock = DockStyle.Fill;

			foreach(Control control in this.Controls)
			{
				control.Text = GetConvertedString(control.Text);
				foreach(Control subControl in control.Controls)
				{
					subControl.Text = GetConvertedString(subControl.Text);
				}
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.panelInformation = new System.Windows.Forms.Panel();
            this.debug = new System.Windows.Forms.Button();
            this.continueCheckBox = new System.Windows.Forms.CheckBox();
            this.pleaseTellTitle = new System.Windows.Forms.Label();
            this.dontSendReport = new System.Windows.Forms.Button();
            this.sendReport = new System.Windows.Forms.Button();
            this.pleaseTellMessage = new System.Windows.Forms.Label();
            this.headerControl1 = new SmartAssembly.SmartExceptionsCore.UI.HeaderControl();
            this.errorMessage = new SmartAssembly.SmartExceptionsCore.UI.AutoHeightLabel();
            this.saveAsFile = new System.Windows.Forms.Button();
            this.panelSending = new System.Windows.Forms.Panel();
            this.cancelSending = new System.Windows.Forms.Button();
            this.ok = new System.Windows.Forms.Button();
            this.retrySending = new System.Windows.Forms.Button();
            this.waitSendingReport = new SmartAssembly.SmartExceptionsCore.UI.WaitSendingReportControl();
            this.headerControl2 = new SmartAssembly.SmartExceptionsCore.UI.HeaderControl();
            this.preparingFeedback = new SmartAssembly.SmartExceptionsCore.UI.FeedbackControl();
            this.connectingFeedback = new SmartAssembly.SmartExceptionsCore.UI.FeedbackControl();
            this.transferingFeedback = new SmartAssembly.SmartExceptionsCore.UI.FeedbackControl();
            this.completedFeedback = new SmartAssembly.SmartExceptionsCore.UI.FeedbackControl();
            this.panelEmail = new System.Windows.Forms.Panel();
            this.labelEmail = new System.Windows.Forms.Label();
            this.sendAnonymously = new System.Windows.Forms.CheckBox();
            this.email = new System.Windows.Forms.TextBox();
            this.headerControl3 = new SmartAssembly.SmartExceptionsCore.UI.HeaderControl();
            this.label3 = new System.Windows.Forms.Label();
            this.continueSendReport = new System.Windows.Forms.Button();
            this.poweredBy = new SmartAssembly.SmartExceptionsCore.UI.PoweredBy();
            this.saveReport = new System.Windows.Forms.Button();
            this.panelInformation.SuspendLayout();
            this.panelSending.SuspendLayout();
            this.panelEmail.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelInformation
            // 
            this.panelInformation.Controls.Add(this.debug);
            this.panelInformation.Controls.Add(this.continueCheckBox);
            this.panelInformation.Controls.Add(this.pleaseTellTitle);
            this.panelInformation.Controls.Add(this.dontSendReport);
            this.panelInformation.Controls.Add(this.sendReport);
            this.panelInformation.Controls.Add(this.pleaseTellMessage);
            this.panelInformation.Controls.Add(this.headerControl1);
            this.panelInformation.Controls.Add(this.errorMessage);
            this.panelInformation.Controls.Add(this.saveAsFile);
            this.panelInformation.Location = new System.Drawing.Point(8, 8);
            this.panelInformation.Name = "panelInformation";
            this.panelInformation.Size = new System.Drawing.Size(413, 240);
            this.panelInformation.TabIndex = 0;
            // 
            // debug
            // 
            this.debug.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.debug.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.debug.Location = new System.Drawing.Point(66, 205);
            this.debug.Name = "debug";
            this.debug.Size = new System.Drawing.Size(64, 24);
            this.debug.TabIndex = 13;
            this.debug.Text = "Debug";
            this.debug.Visible = false;
            this.debug.Click += new System.EventHandler(this.debug_Click);
            // 
            // continueCheckBox
            // 
            this.continueCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.continueCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.continueCheckBox.Location = new System.Drawing.Point(22, 99);
            this.continueCheckBox.Name = "continueCheckBox";
            this.continueCheckBox.Size = new System.Drawing.Size(226, 16);
            this.continueCheckBox.TabIndex = 14;
            this.continueCheckBox.Text = "Ignore this error and attempt to &continue.";
            this.continueCheckBox.CheckedChanged += new System.EventHandler(this.continueCheckBox_CheckedChanged);
            // 
            // pleaseTellTitle
            // 
            this.pleaseTellTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pleaseTellTitle.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.pleaseTellTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pleaseTellTitle.Location = new System.Drawing.Point(20, 124);
            this.pleaseTellTitle.Name = "pleaseTellTitle";
            this.pleaseTellTitle.Size = new System.Drawing.Size(381, 16);
            this.pleaseTellTitle.TabIndex = 11;
            this.pleaseTellTitle.Text = "Please tell %CompanyName% about this problem.";
            // 
            // dontSendReport
            // 
            this.dontSendReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.dontSendReport.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.dontSendReport.Location = new System.Drawing.Point(325, 205);
            this.dontSendReport.Name = "dontSendReport";
            this.dontSendReport.Size = new System.Drawing.Size(75, 24);
            this.dontSendReport.TabIndex = 6;
            this.dontSendReport.Text = "&Don\'t Send";
            this.dontSendReport.Click += new System.EventHandler(this.dontSendReport_Click);
            // 
            // sendReport
            // 
            this.sendReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sendReport.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.sendReport.Location = new System.Drawing.Point(214, 205);
            this.sendReport.Name = "sendReport";
            this.sendReport.Size = new System.Drawing.Size(105, 24);
            this.sendReport.TabIndex = 9;
            this.sendReport.Text = "&Send Error Report";
            this.sendReport.Click += new System.EventHandler(this.sendReport_Click);
            // 
            // pleaseTellMessage
            // 
            this.pleaseTellMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pleaseTellMessage.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.pleaseTellMessage.Location = new System.Drawing.Point(20, 140);
            this.pleaseTellMessage.Name = "pleaseTellMessage";
            this.pleaseTellMessage.Size = new System.Drawing.Size(381, 55);
            this.pleaseTellMessage.TabIndex = 12;
            this.pleaseTellMessage.Text = "To help improve the software you use, %CompanyName% is interested in learning mor" +
    "e about this error. We have created a report about the error for you to send to " +
    "us.";
            // 
            // headerControl1
            // 
            this.headerControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(96)))), ((int)(((byte)(179)))));
            this.headerControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerControl1.ForeColor = System.Drawing.Color.White;
            this.headerControl1.IconState = SmartAssembly.SmartExceptionsCore.UI.IconState.Error;
            this.headerControl1.Image = null;
            this.headerControl1.Location = new System.Drawing.Point(0, 0);
            this.headerControl1.Name = "headerControl1";
            this.headerControl1.Size = new System.Drawing.Size(413, 58);
            this.headerControl1.TabIndex = 3;
            this.headerControl1.TabStop = false;
            this.headerControl1.Text = "%AppName% has encountered a problem.\nWe are sorry for the inconvenience.";
            // 
            // errorMessage
            // 
            this.errorMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.errorMessage.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.errorMessage.Location = new System.Drawing.Point(20, 69);
            this.errorMessage.Name = "errorMessage";
            this.errorMessage.Size = new System.Drawing.Size(381, 13);
            this.errorMessage.TabIndex = 10;
            this.errorMessage.Text = "errorMessage";
            this.errorMessage.UseMnemonic = false;
            // 
            // saveAsFile
            // 
            this.saveAsFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveAsFile.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.saveAsFile.Location = new System.Drawing.Point(136, 205);
            this.saveAsFile.Name = "saveAsFile";
            this.saveAsFile.Size = new System.Drawing.Size(72, 24);
            this.saveAsFile.TabIndex = 11;
            this.saveAsFile.Text = "Save as &File";
            this.saveAsFile.Click += new System.EventHandler(this.saveAsFile_Click);
            // 
            // panelSending
            // 
            this.panelSending.Controls.Add(this.saveReport);
            this.panelSending.Controls.Add(this.cancelSending);
            this.panelSending.Controls.Add(this.ok);
            this.panelSending.Controls.Add(this.retrySending);
            this.panelSending.Controls.Add(this.waitSendingReport);
            this.panelSending.Controls.Add(this.headerControl2);
            this.panelSending.Controls.Add(this.preparingFeedback);
            this.panelSending.Controls.Add(this.connectingFeedback);
            this.panelSending.Controls.Add(this.transferingFeedback);
            this.panelSending.Controls.Add(this.completedFeedback);
            this.panelSending.Location = new System.Drawing.Point(8, 264);
            this.panelSending.Name = "panelSending";
            this.panelSending.Size = new System.Drawing.Size(413, 232);
            this.panelSending.TabIndex = 2;
            this.panelSending.Visible = false;
            // 
            // cancelSending
            // 
            this.cancelSending.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelSending.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cancelSending.Location = new System.Drawing.Point(320, 197);
            this.cancelSending.Name = "cancelSending";
            this.cancelSending.Size = new System.Drawing.Size(80, 24);
            this.cancelSending.TabIndex = 10;
            this.cancelSending.Text = "&Cancel";
            this.cancelSending.Click += new System.EventHandler(this.cancelSending_Click);
            // 
            // ok
            // 
            this.ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok.Enabled = false;
            this.ok.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ok.Location = new System.Drawing.Point(232, 197);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(80, 24);
            this.ok.TabIndex = 22;
            this.ok.Text = "&OK";
            this.ok.Click += new System.EventHandler(this.ok_Click);
            // 
            // retrySending
            // 
            this.retrySending.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.retrySending.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.retrySending.Location = new System.Drawing.Point(144, 197);
            this.retrySending.Name = "retrySending";
            this.retrySending.Size = new System.Drawing.Size(80, 24);
            this.retrySending.TabIndex = 23;
            this.retrySending.Text = "&Retry";
            this.retrySending.Visible = false;
            this.retrySending.Click += new System.EventHandler(this.retrySending_Click);
            // 
            // waitSendingReport
            // 
            this.waitSendingReport.Location = new System.Drawing.Point(87, 145);
            this.waitSendingReport.Name = "waitSendingReport";
            this.waitSendingReport.Size = new System.Drawing.Size(250, 42);
            this.waitSendingReport.TabIndex = 11;
            this.waitSendingReport.TabStop = false;
            this.waitSendingReport.Visible = false;
            // 
            // headerControl2
            // 
            this.headerControl2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(96)))), ((int)(((byte)(179)))));
            this.headerControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerControl2.ForeColor = System.Drawing.Color.White;
            this.headerControl2.IconState = SmartAssembly.SmartExceptionsCore.UI.IconState.Error;
            this.headerControl2.Image = null;
            this.headerControl2.Location = new System.Drawing.Point(0, 0);
            this.headerControl2.Name = "headerControl2";
            this.headerControl2.Size = new System.Drawing.Size(413, 58);
            this.headerControl2.TabIndex = 24;
            this.headerControl2.TabStop = false;
            this.headerControl2.Text = "Please wait while %AppName% is sending the report to %CompanyName% through the In" +
    "ternet.";
            // 
            // preparingFeedback
            // 
            this.preparingFeedback.Location = new System.Drawing.Point(24, 72);
            this.preparingFeedback.Name = "preparingFeedback";
            this.preparingFeedback.Size = new System.Drawing.Size(368, 16);
            this.preparingFeedback.TabIndex = 18;
            this.preparingFeedback.TabStop = false;
            this.preparingFeedback.Text = "Preparing the error report.";
            // 
            // connectingFeedback
            // 
            this.connectingFeedback.Location = new System.Drawing.Point(24, 96);
            this.connectingFeedback.Name = "connectingFeedback";
            this.connectingFeedback.Size = new System.Drawing.Size(368, 16);
            this.connectingFeedback.TabIndex = 19;
            this.connectingFeedback.TabStop = false;
            this.connectingFeedback.Text = "Connecting to server.";
            // 
            // transferingFeedback
            // 
            this.transferingFeedback.Location = new System.Drawing.Point(24, 120);
            this.transferingFeedback.Name = "transferingFeedback";
            this.transferingFeedback.Size = new System.Drawing.Size(368, 16);
            this.transferingFeedback.TabIndex = 20;
            this.transferingFeedback.TabStop = false;
            this.transferingFeedback.Text = "Transferring report.";
            // 
            // completedFeedback
            // 
            this.completedFeedback.Location = new System.Drawing.Point(24, 144);
            this.completedFeedback.Name = "completedFeedback";
            this.completedFeedback.Size = new System.Drawing.Size(368, 16);
            this.completedFeedback.TabIndex = 21;
            this.completedFeedback.TabStop = false;
            this.completedFeedback.Text = "Error reporting completed. Thank you.";
            // 
            // panelEmail
            // 
            this.panelEmail.Controls.Add(this.labelEmail);
            this.panelEmail.Controls.Add(this.sendAnonymously);
            this.panelEmail.Controls.Add(this.email);
            this.panelEmail.Controls.Add(this.headerControl3);
            this.panelEmail.Controls.Add(this.label3);
            this.panelEmail.Controls.Add(this.continueSendReport);
            this.panelEmail.Location = new System.Drawing.Point(11, 512);
            this.panelEmail.Name = "panelEmail";
            this.panelEmail.Size = new System.Drawing.Size(413, 232);
            this.panelEmail.TabIndex = 4;
            this.panelEmail.Visible = false;
            // 
            // labelEmail
            // 
            this.labelEmail.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.labelEmail.Location = new System.Drawing.Point(20, 131);
            this.labelEmail.Name = "labelEmail";
            this.labelEmail.Size = new System.Drawing.Size(100, 16);
            this.labelEmail.TabIndex = 9;
            this.labelEmail.Text = "&Email address:";
            // 
            // sendAnonymously
            // 
            this.sendAnonymously.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.sendAnonymously.Location = new System.Drawing.Point(120, 160);
            this.sendAnonymously.Name = "sendAnonymously";
            this.sendAnonymously.Size = new System.Drawing.Size(232, 16);
            this.sendAnonymously.TabIndex = 11;
            this.sendAnonymously.Text = "I prefer to send this report &anonymously.";
            this.sendAnonymously.CheckedChanged += new System.EventHandler(this.sendAnonymously_CheckedChanged);
            // 
            // email
            // 
            this.email.Location = new System.Drawing.Point(120, 128);
            this.email.Name = "email";
            this.email.Size = new System.Drawing.Size(256, 20);
            this.email.TabIndex = 10;
            this.email.TextChanged += new System.EventHandler(this.email_TextChanged);
            // 
            // headerControl3
            // 
            this.headerControl3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(96)))), ((int)(((byte)(179)))));
            this.headerControl3.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerControl3.ForeColor = System.Drawing.Color.White;
            this.headerControl3.IconState = SmartAssembly.SmartExceptionsCore.UI.IconState.Error;
            this.headerControl3.Image = null;
            this.headerControl3.Location = new System.Drawing.Point(0, 0);
            this.headerControl3.Name = "headerControl3";
            this.headerControl3.Size = new System.Drawing.Size(413, 58);
            this.headerControl3.TabIndex = 3;
            this.headerControl3.TabStop = false;
            this.headerControl3.Text = "Do you want to be contacted by %CompanyName% regarding this problem?";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label3.Location = new System.Drawing.Point(20, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(381, 43);
            this.label3.TabIndex = 10;
            this.label3.Text = "If you want to be contacted by %CompanyName% regarding this error, please provide" +
    " your e-mail address. This information will not be used for any other purpose.";
            // 
            // continueSendReport
            // 
            this.continueSendReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.continueSendReport.Enabled = false;
            this.continueSendReport.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.continueSendReport.Location = new System.Drawing.Point(295, 197);
            this.continueSendReport.Name = "continueSendReport";
            this.continueSendReport.Size = new System.Drawing.Size(105, 24);
            this.continueSendReport.TabIndex = 12;
            this.continueSendReport.Text = "&Send Error Report";
            this.continueSendReport.Click += new System.EventHandler(this.continueSendReport_Click);
            // 
            // poweredBy
            // 
            this.poweredBy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.poweredBy.Cursor = System.Windows.Forms.Cursors.Hand;
            this.poweredBy.Location = new System.Drawing.Point(6, 730);
            this.poweredBy.Name = "poweredBy";
            this.poweredBy.Size = new System.Drawing.Size(112, 32);
            this.poweredBy.TabIndex = 5;
            this.poweredBy.TabStop = false;
            this.poweredBy.Text = "poweredBy1";
            // 
            // saveReport
            // 
            this.saveReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveReport.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.saveReport.Location = new System.Drawing.Point(146, 197);
            this.saveReport.Name = "saveReport";
            this.saveReport.Size = new System.Drawing.Size(80, 24);
            this.saveReport.TabIndex = 25;
            this.saveReport.Text = "&Save Report";
            this.saveReport.Visible = false;
            this.saveReport.Click += new System.EventHandler(this.saveReport_Click);
            // 
            // ExceptionReportingForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(434, 768);
            this.ControlBox = false;
            this.Controls.Add(this.poweredBy);
            this.Controls.Add(this.panelEmail);
            this.Controls.Add(this.panelInformation);
            this.Controls.Add(this.panelSending);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ExceptionReportingForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "%AppName%";
            this.TopMost = true;
            this.panelInformation.ResumeLayout(false);
            this.panelSending.ResumeLayout(false);
            this.panelEmail.ResumeLayout(false);
            this.panelEmail.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		private string GetConvertedString(string s)
		{
			s = s.Replace("%AppName%", UnhandledExceptionHandler.ApplicationName);
			s = s.Replace("%CompanyName%", UnhandledExceptionHandler.CompanyName);
			return s;
		}

		public void SendReport()
		{
			try
			{
				this.panelEmail.Visible = false;
				this.panelSending.Visible = true;
				if (reportExceptionEventArgs != null) StartWorkingThread(new ThreadStart(StartSendReport));
			}
			catch
			{
			}
		}

		private void sendReport_Click(object sender, System.EventArgs e)
		{
			this.panelInformation.Visible = false;
			this.panelEmail.Visible = true;
		}

		private void StartWorkingThread(ThreadStart start)
		{
			workingThread = new Thread(start);
			workingThread.Start();
		}

		private void dontSendReport_Click(object sender, System.EventArgs e)
		{
			Close();
		}

		private void cancelSending_Click(object sender, System.EventArgs e)
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

		private void ok_Click(object sender, System.EventArgs e)
		{
			Close();
		}

		private void continueCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			reportExceptionEventArgs.TryToContinue = this.continueCheckBox.Checked;
		}

		private void OnFeedback(object sender, SendingReportFeedbackEventArgs e)
		{
			try
			{
				Invoke(new SendingReportFeedbackEventHandler(Feedback), new object[]{sender, e});
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

        private void Feedback(object sender, SendingReportFeedbackEventArgs e)
        {
            Button failureButton = alreadyRetried && Thread.CurrentThread.GetApartmentState() == ApartmentState.STA ? saveReport : retrySending;

            switch (e.Step)
            {
                case SendingReportStep.PreparingReport:
                    if (e.Failed)
                    {
                        preparingFeedback.Stop(e.ErrorMessage);
                        failureButton.Visible = true;
                        failureButton.Focus();
                    }
                    else
                    {
                        preparingFeedback.Start();
                    }
                    break;

                case SendingReportStep.ConnectingToServer:
                    if (e.Failed)
                    {
                        connectingFeedback.Stop(e.ErrorMessage);
                        failureButton.Visible = true;
                        failureButton.Focus();
                    }
                    else
                    {
                        preparingFeedback.Stop();
                        connectingFeedback.Start();
                    }
                    break;

                case SendingReportStep.Transfering:
                    if (e.Failed)
                    {
                        waitSendingReport.Visible = false;
                        transferingFeedback.Stop(e.ErrorMessage);
                        failureButton.Visible = true;
                        failureButton.Focus();
                    }
                    else
                    {
                        connectingFeedback.Stop();
                        transferingFeedback.Start();
                        waitSendingReport.Visible = true;
                    }
                    break;

                case SendingReportStep.Finished:
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

		private void retrySending_Click(object sender, System.EventArgs e)
		{
		    alreadyRetried = true;
			retrySending.Visible = false;
			preparingFeedback.Init();
			connectingFeedback.Init();
			transferingFeedback.Init();
			if (reportExceptionEventArgs != null) StartWorkingThread(new ThreadStart(StartSendReport));
		}

		private void StartSendReport()
		{
			reportExceptionEventArgs.SendReport();
		}

		private void debug_Click(object sender, System.EventArgs e)
		{
			if (reportExceptionEventArgs != null) StartWorkingThread(new ThreadStart(reportExceptionEventArgs.LaunchDebugger));
		}

		private void continueSendReport_Click(object sender, System.EventArgs e)
		{
			if (!sendAnonymously.Checked && reportExceptionEventArgs != null)
			{
				reportExceptionEventArgs.AddCustomProperty("Email", email.Text);
				RegistryHelper.SaveHKLMRegistryString("Email", email.Text);
			}
			SendReport();
		}

		private void email_TextChanged(object sender, System.EventArgs e)
		{
			continueSendReport.Enabled = (email.Text.Length > 0 || sendAnonymously.Checked);
		}

		private void sendAnonymously_CheckedChanged(object sender, System.EventArgs e)
		{
			email.Enabled = !sendAnonymously.Checked;
			continueSendReport.Enabled = (email.Text.Length > 0 || sendAnonymously.Checked);
		}

		private void saveAsFile_Click(object sender, System.EventArgs e)
		{
			SaveFileDialog saveReportDialog = new SaveFileDialog();
			saveReportDialog.Title = "Save an Exception Report";
			// WARNING: To open the encrypted report with SA, the file must have a .saencryptedreport extension.
			saveReportDialog.DefaultExt = "saencryptedreport";
			saveReportDialog.Filter = "SmartAssembly Encrypted Exception Report|*.saencryptedreport";

			if (saveReportDialog.ShowDialog(this) != DialogResult.Cancel)
			{
				if (reportExceptionEventArgs.SaveEncryptedReport(saveReportDialog.FileName))
				{
					MessageBox.Show(string.Format("Please send the Exception Report to {0} Support Team.", UnhandledExceptionHandler.CompanyName), UnhandledExceptionHandler.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
					Close();
				}
				else
				{
					MessageBox.Show("Failed to save the report.", UnhandledExceptionHandler.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

        private void saveReport_Click(object sender, EventArgs e)
        {
            saveAsFile_Click(sender, e);
        }
	}
}
