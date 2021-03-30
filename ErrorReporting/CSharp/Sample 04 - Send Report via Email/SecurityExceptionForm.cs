using System;
using System.Drawing;
using System.Diagnostics;
using System.Collections;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;

using SmartAssembly.SmartExceptionsCore;
using SmartAssembly.SmartExceptionsCore.UI;

namespace SmartAssembly.SmartExceptionsViaEmail
{
	[System.ComponentModel.DesignerCategory("Code")]
	internal class SecurityExceptionForm : Form
	{
		private SecurityExceptionEventArgs securityExceptionEventArgs = null;
		private PoweredBy powered = new PoweredBy();
		private Button continueButton = new Button();
		private Button quitButton = new Button();
		private HeaderControl headerControl = new HeaderControl(string.Format(Localization.SecurityExceptionHeader, UnhandledExceptionHandler.ApplicationName));
		private AutoHeightLabel errorMessage = new AutoHeightLabel();

		private void InitializeComponent()
		{
			SuspendLayout();

			// quitButton
			quitButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			quitButton.FlatStyle = FlatStyle.System;
			quitButton.Size = new Size(Localization.QuitButtonWidth, 24);
			quitButton.Location = new Point(408 - quitButton.Width, 188);
			quitButton.TabIndex = 0;
			quitButton.Text = Localization.QuitButton;
			quitButton.Click += new EventHandler(OnQuitClick);

			// continueButton
			continueButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			continueButton.FlatStyle = FlatStyle.System;
			continueButton.Size = new Size(Localization.ContinueButtonWidth, 24);
			continueButton.Location = new Point(quitButton.Left - continueButton.Width - 6, 188);
			continueButton.TabIndex = 1;
			continueButton.Text = Localization.ContinueButton;
			continueButton.Click += new EventHandler(OnContinueClick);

			// powered
			powered.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			powered.SetBounds(6, 186, 120, 32);

			// headerControl
			headerControl.IconState = IconState.Warning;

			// errorMessage
			errorMessage.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			errorMessage.Location = new Point(20, 72);
			errorMessage.Size = new Size(382, 13);

			// SecurityExceptionForm
			AutoScaleBaseSize = new Size(5, 13);
			ClientSize = new Size(418, 224);
			ControlBox = false;
			Controls.AddRange(new Control[]{powered, continueButton, quitButton, headerControl, errorMessage});
			FormBorderStyle = FormBorderStyle.FixedSingle;
			MaximizeBox = false;
			MinimizeBox = false;
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterScreen;

			ResumeLayout(false);
		}

		private void OnContinueClick(object sender, System.EventArgs e)
		{
			securityExceptionEventArgs.TryToContinue = true;
			Close();
		}

		private void OnQuitClick(object sender, System.EventArgs e)
		{
			securityExceptionEventArgs.TryToContinue = false;
			Close();
		}

		public SecurityExceptionForm(SecurityExceptionEventArgs securityExceptionEventArgs)
		{
			InitializeComponent();

			Icon = Win32.GetApplicationIcon();
			Text = UnhandledExceptionHandler.ApplicationName;
			if (Text.Length == 0) Text = Localization.SecurityException;

			this.securityExceptionEventArgs = securityExceptionEventArgs;
			if (!securityExceptionEventArgs.CanContinue) continueButton.Visible = false;

			if (securityExceptionEventArgs.SecurityMessage.Length > 0)
			{
				errorMessage.Text = securityExceptionEventArgs.SecurityMessage;
			}
			else
			{
				StringBuilder sb = new StringBuilder();
				sb.Append(string.Format(Localization.SecurityExceptionMessage, UnhandledExceptionHandler.ApplicationName));
				if (securityExceptionEventArgs.CanContinue) sb.Append(Localization.ContinueOrQuit);
				sb.Append(securityExceptionEventArgs.SecurityException.Message);
				errorMessage.Text = sb.ToString();
			}

			int newClientHeigth = errorMessage.Bottom + 60;
			if (newClientHeigth > ClientSize.Height) ClientSize = new Size(ClientSize.Width, newClientHeigth);
		}
	}
}
