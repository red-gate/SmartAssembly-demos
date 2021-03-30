using System;
using System.Drawing;
using System.Diagnostics;
using System.Collections;
using System.ComponentModel;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;

using SmartAssembly.SmartExceptionsCore;

namespace SmartAssembly.SmartExceptionsWithSecuredProxy
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
		private SmartAssembly.SmartExceptionsCore.UI.HeaderControl headerControl3;
		private System.Windows.Forms.Button continueSendReport;
		private SmartAssembly.SmartExceptionsCore.UI.AutoHeightLabel errorMessage;
		private SmartAssembly.SmartExceptionsCore.UI.PoweredBy poweredBy;
		private System.Windows.Forms.Button saveAsFile;
		private System.Windows.Forms.Button saveAsFile3;
		private System.Windows.Forms.Label labelProxy;
		private System.Windows.Forms.TextBox userName;
		private System.Windows.Forms.Label labelPassword;
		private System.Windows.Forms.TextBox password;
		private System.Windows.Forms.Button saveAsFile2;
		private System.Windows.Forms.CheckBox doNotUseProxy;
		private System.Windows.Forms.Panel panelProxy;
		private System.Windows.Forms.Label labelUserName;
		private System.Windows.Forms.TextBox proxy;

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

			switch (reportExceptionEventArgs.Exception.HelpLink)
			{
				//We don't ask for sending or not, we go directly to the email information
				//Used by SmartAssembly if the developer clicks the "Send Report" button from the Main Application
				case "{send}":
					reportExceptionEventArgs.TryToContinue = true;
					panelInformation.Visible = false;
					panelProxy.Visible = true;
					saveAsFile2.Visible = true;
					break;
			}

			this.proxy.Text = RegistryHelper.ReadHKLMRegistryString("Proxy");
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
			
			this.panelProxy.Location = Point.Empty;
			this.panelProxy.Dock = DockStyle.Fill;

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
			this.waitSendingReport = new SmartAssembly.SmartExceptionsCore.UI.WaitSendingReportControl();
			this.headerControl2 = new SmartAssembly.SmartExceptionsCore.UI.HeaderControl();
			this.preparingFeedback = new SmartAssembly.SmartExceptionsCore.UI.FeedbackControl();
			this.connectingFeedback = new SmartAssembly.SmartExceptionsCore.UI.FeedbackControl();
			this.transferingFeedback = new SmartAssembly.SmartExceptionsCore.UI.FeedbackControl();
			this.completedFeedback = new SmartAssembly.SmartExceptionsCore.UI.FeedbackControl();
			this.retrySending = new System.Windows.Forms.Button();
			this.saveAsFile3 = new System.Windows.Forms.Button();
			this.panelProxy = new System.Windows.Forms.Panel();
			this.labelUserName = new System.Windows.Forms.Label();
			this.labelPassword = new System.Windows.Forms.Label();
			this.password = new System.Windows.Forms.TextBox();
			this.userName = new System.Windows.Forms.TextBox();
			this.saveAsFile2 = new System.Windows.Forms.Button();
			this.labelProxy = new System.Windows.Forms.Label();
			this.proxy = new System.Windows.Forms.TextBox();
			this.doNotUseProxy = new System.Windows.Forms.CheckBox();
			this.headerControl3 = new SmartAssembly.SmartExceptionsCore.UI.HeaderControl();
			this.continueSendReport = new System.Windows.Forms.Button();
			this.poweredBy = new SmartAssembly.SmartExceptionsCore.UI.PoweredBy();
			this.panelInformation.SuspendLayout();
			this.panelSending.SuspendLayout();
			this.panelProxy.SuspendLayout();
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
			this.debug.TabIndex = 14;
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
			this.continueCheckBox.TabIndex = 13;
			this.continueCheckBox.Text = "Ignore this error and attempt to &continue.";
			this.continueCheckBox.CheckedChanged += new System.EventHandler(this.continueCheckBox_CheckedChanged);
			// 
			// pleaseTellTitle
			// 
			this.pleaseTellTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.pleaseTellTitle.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.pleaseTellTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
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
			this.dontSendReport.TabIndex = 4;
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
			this.sendReport.TabIndex = 3;
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
			this.headerControl1.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(36)), ((System.Byte)(96)), ((System.Byte)(179)));
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
			this.saveAsFile.TabIndex = 15;
			this.saveAsFile.Text = "Save as &File";
			this.saveAsFile.Click += new System.EventHandler(this.saveAsFile_Click);
			// 
			// panelSending
			// 
			this.panelSending.Controls.Add(this.cancelSending);
			this.panelSending.Controls.Add(this.ok);
			this.panelSending.Controls.Add(this.waitSendingReport);
			this.panelSending.Controls.Add(this.headerControl2);
			this.panelSending.Controls.Add(this.preparingFeedback);
			this.panelSending.Controls.Add(this.connectingFeedback);
			this.panelSending.Controls.Add(this.transferingFeedback);
			this.panelSending.Controls.Add(this.completedFeedback);
			this.panelSending.Controls.Add(this.retrySending);
			this.panelSending.Controls.Add(this.saveAsFile3);
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
			this.cancelSending.TabIndex = 8;
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
			this.ok.TabIndex = 7;
			this.ok.Text = "&OK";
			this.ok.Click += new System.EventHandler(this.ok_Click);
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
			this.headerControl2.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(36)), ((System.Byte)(96)), ((System.Byte)(179)));
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
			// retrySending
			// 
			this.retrySending.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.retrySending.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.retrySending.Location = new System.Drawing.Point(56, 197);
			this.retrySending.Name = "retrySending";
			this.retrySending.Size = new System.Drawing.Size(80, 24);
			this.retrySending.TabIndex = 6;
			this.retrySending.Text = "&Retry";
			this.retrySending.Visible = false;
			this.retrySending.Click += new System.EventHandler(this.retrySending_Click);
			// 
			// saveAsFile3
			// 
			this.saveAsFile3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.saveAsFile3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.saveAsFile3.Location = new System.Drawing.Point(144, 197);
			this.saveAsFile3.Name = "saveAsFile3";
			this.saveAsFile3.Size = new System.Drawing.Size(80, 24);
			this.saveAsFile3.TabIndex = 5;
			this.saveAsFile3.Text = "Save as &File";
			this.saveAsFile3.Visible = false;
			this.saveAsFile3.Click += new System.EventHandler(this.saveAsFile_Click);
			// 
			// panelProxy
			// 
			this.panelProxy.Controls.Add(this.labelUserName);
			this.panelProxy.Controls.Add(this.labelPassword);
			this.panelProxy.Controls.Add(this.password);
			this.panelProxy.Controls.Add(this.userName);
			this.panelProxy.Controls.Add(this.saveAsFile2);
			this.panelProxy.Controls.Add(this.labelProxy);
			this.panelProxy.Controls.Add(this.proxy);
			this.panelProxy.Controls.Add(this.doNotUseProxy);
			this.panelProxy.Controls.Add(this.headerControl3);
			this.panelProxy.Controls.Add(this.continueSendReport);
			this.panelProxy.Location = new System.Drawing.Point(11, 512);
			this.panelProxy.Name = "panelProxy";
			this.panelProxy.Size = new System.Drawing.Size(413, 232);
			this.panelProxy.TabIndex = 4;
			this.panelProxy.Visible = false;
			// 
			// labelUserName
			// 
			this.labelUserName.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.labelUserName.Location = new System.Drawing.Point(20, 115);
			this.labelUserName.Name = "labelUserName";
			this.labelUserName.Size = new System.Drawing.Size(84, 16);
			this.labelUserName.TabIndex = 11;
			this.labelUserName.Text = "&User Name:";
			// 
			// labelPassword
			// 
			this.labelPassword.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.labelPassword.Location = new System.Drawing.Point(20, 147);
			this.labelPassword.Name = "labelPassword";
			this.labelPassword.Size = new System.Drawing.Size(100, 16);
			this.labelPassword.TabIndex = 13;
			this.labelPassword.Text = "P&assword:";
			// 
			// password
			// 
			this.password.Location = new System.Drawing.Point(120, 144);
			this.password.Name = "password";
			this.password.PasswordChar = '*';
			this.password.Size = new System.Drawing.Size(256, 20);
			this.password.TabIndex = 14;
			this.password.Text = "";
			// 
			// userName
			// 
			this.userName.Location = new System.Drawing.Point(120, 112);
			this.userName.Name = "userName";
			this.userName.Size = new System.Drawing.Size(256, 20);
			this.userName.TabIndex = 12;
			this.userName.Text = "";
			// 
			// saveAsFile2
			// 
			this.saveAsFile2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.saveAsFile2.BackColor = System.Drawing.SystemColors.Window;
			this.saveAsFile2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.saveAsFile2.Location = new System.Drawing.Point(209, 197);
			this.saveAsFile2.Name = "saveAsFile2";
			this.saveAsFile2.Size = new System.Drawing.Size(80, 24);
			this.saveAsFile2.TabIndex = 16;
			this.saveAsFile2.Text = "Save as &File";
			this.saveAsFile2.Click += new System.EventHandler(this.saveAsFile_Click);
			// 
			// labelProxy
			// 
			this.labelProxy.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.labelProxy.Location = new System.Drawing.Point(20, 83);
			this.labelProxy.Name = "labelProxy";
			this.labelProxy.Size = new System.Drawing.Size(100, 16);
			this.labelProxy.TabIndex = 9;
			this.labelProxy.Text = "&Proxy address:";
			// 
			// proxy
			// 
			this.proxy.Location = new System.Drawing.Point(120, 80);
			this.proxy.Name = "proxy";
			this.proxy.Size = new System.Drawing.Size(256, 20);
			this.proxy.TabIndex = 10;
			this.proxy.Text = "";
			this.proxy.TextChanged += new System.EventHandler(this.email_TextChanged);
			// 
			// doNotUseProxy
			// 
			this.doNotUseProxy.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.doNotUseProxy.Location = new System.Drawing.Point(120, 168);
			this.doNotUseProxy.Name = "doNotUseProxy";
			this.doNotUseProxy.Size = new System.Drawing.Size(136, 24);
			this.doNotUseProxy.TabIndex = 15;
			this.doNotUseProxy.Text = "I do &not use a proxy.";
			this.doNotUseProxy.CheckedChanged += new System.EventHandler(this.sendAnonymously_CheckedChanged);
			// 
			// headerControl3
			// 
			this.headerControl3.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(36)), ((System.Byte)(96)), ((System.Byte)(179)));
			this.headerControl3.Dock = System.Windows.Forms.DockStyle.Top;
			this.headerControl3.ForeColor = System.Drawing.Color.White;
			this.headerControl3.IconState = SmartAssembly.SmartExceptionsCore.UI.IconState.Error;
			this.headerControl3.Image = null;
			this.headerControl3.Location = new System.Drawing.Point(0, 0);
			this.headerControl3.Name = "headerControl3";
			this.headerControl3.Size = new System.Drawing.Size(413, 58);
			this.headerControl3.TabIndex = 3;
			this.headerControl3.TabStop = false;
			this.headerControl3.Text = "Do you use a corporate proxy to connect to the Internet?";
			// 
			// continueSendReport
			// 
			this.continueSendReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.continueSendReport.Enabled = false;
			this.continueSendReport.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.continueSendReport.Location = new System.Drawing.Point(295, 197);
			this.continueSendReport.Name = "continueSendReport";
			this.continueSendReport.Size = new System.Drawing.Size(105, 24);
			this.continueSendReport.TabIndex = 17;
			this.continueSendReport.Text = "&Send Error Report";
			this.continueSendReport.Click += new System.EventHandler(this.continueSendReport_Click);
			// 
			// poweredBy
			// 
			this.poweredBy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.poweredBy.Cursor = System.Windows.Forms.Cursors.Hand;
			this.poweredBy.Location = new System.Drawing.Point(6, 730);
			this.poweredBy.Name = "poweredBy";
			this.poweredBy.Size = new System.Drawing.Size(120, 32);
			this.poweredBy.TabIndex = 5;
			this.poweredBy.TabStop = false;
			// 
			// ExceptionReportingForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.SystemColors.Window;
			this.ClientSize = new System.Drawing.Size(434, 768);
			this.ControlBox = false;
			this.Controls.Add(this.poweredBy);
			this.Controls.Add(this.panelProxy);
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
			this.panelProxy.ResumeLayout(false);
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
				this.panelProxy.Visible = false;
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
			this.panelProxy.Visible = true;
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
			switch (e.Step)
			{
				case SendingReportStep.PreparingReport:
					if (e.Failed)
					{
						preparingFeedback.Stop(e.ErrorMessage);
						retrySending.Visible = true;
						saveAsFile3.Visible = true;
						retrySending.Focus();
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
						retrySending.Visible = true;
						saveAsFile3.Visible = true;
						retrySending.Focus();
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
						retrySending.Visible = true;
						saveAsFile3.Visible = true;
						retrySending.Focus();
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
			retrySending.Visible = false;
			this.panelSending.Visible = false;
			preparingFeedback.Init();
			connectingFeedback.Init();
			transferingFeedback.Init();
			this.panelProxy.Visible = true;
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
			if (!doNotUseProxy.Checked && reportExceptionEventArgs != null)
			{
				RegistryHelper.SaveHKLMRegistryString("Proxy", proxy.Text);
			}

			if (doNotUseProxy.Checked)
			{
				unhandledExceptionHandler.SetProxy(null);
			}
			else
			{
				WebProxy webProxy = new WebProxy(proxy.Text);
				if (userName.Text.Length > 0) webProxy.Credentials = new NetworkCredential(userName.Text, password.Text);
				unhandledExceptionHandler.SetProxy(webProxy);
			}

			SendReport();
		}

		private void email_TextChanged(object sender, System.EventArgs e)
		{
			continueSendReport.Enabled = (proxy.Text.Length > 0 || doNotUseProxy.Checked);
		}

		private void sendAnonymously_CheckedChanged(object sender, System.EventArgs e)
		{
			proxy.Enabled = !doNotUseProxy.Checked;
			userName.Enabled = !doNotUseProxy.Checked;
			password.Enabled = !doNotUseProxy.Checked;
			continueSendReport.Enabled = (proxy.Text.Length > 0 || doNotUseProxy.Checked);
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
					MessageBox.Show("Please send the exception report to Red Gate Support at support@red-gate.com", UnhandledExceptionHandler.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
					Close();
				}
				else
				{
					MessageBox.Show("Failed to save the report.", UnhandledExceptionHandler.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}
	}
}
