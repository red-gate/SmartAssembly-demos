Imports System
Imports System.Drawing
Imports System.Diagnostics
Imports System.Collections
Imports System.ComponentModel
Imports System.Threading
Imports System.Windows.Forms
Imports Microsoft.Win32

Imports SmartAssembly.SmartExceptionsCore
Imports SmartAssembly.SmartExceptionsCore.UI


Friend Class ExceptionReportingForm
    Inherits Form

    Public Sub New()
        Me.InitializeComponent()
        MyBase.Size = New Size(&H1A3, &H108)
        MyBase.MinimizeBox = False
        MyBase.MaximizeBox = False
        Me.panelInformation.Location = Point.Empty
        Me.panelInformation.Dock = DockStyle.Fill
        Me.retrySending.Location = Me.ok.Location
        Me.retrySending.Size = Me.ok.Size
        Me.retrySending.BringToFront()
        Me.panelSending.Location = Point.Empty
        Me.panelSending.Dock = DockStyle.Fill
        Me.Text = Me.GetConvertedString(Me.Text)
        Me.panelEmail.Location = Point.Empty
        Me.panelEmail.Dock = DockStyle.Fill
        Dim control As control
        For Each control In MyBase.Controls
            control.Text = Me.GetConvertedString(control.Text)
            Dim control2 As control
            For Each control2 In control.Controls
                control2.Text = Me.GetConvertedString(control2.Text)
            Next
        Next
    End Sub

    Public Sub New(ByVal unhandledExceptionHandler As unhandledExceptionHandler, ByVal reportExceptionEventArgs As reportExceptionEventArgs)
        Me.New()
        Dim height As Integer = MyBase.Height
        Me.reportExceptionEventArgs = reportExceptionEventArgs
        Me.unhandledExceptionHandler = unhandledExceptionHandler
        Me.errorMessage.Text = reportExceptionEventArgs.Exception.Message
        height = (height + (Me.errorMessage.Height - MyBase.FontHeight))
        If Not reportExceptionEventArgs.ShowContinueCheckbox Then
            Me.continueCheckBox.Visible = False
            height = (height - Me.continueCheckBox.Height)
        End If
        If (height > MyBase.Height) Then
            MyBase.Height = height
        End If
        If reportExceptionEventArgs.CanDebug Then
            AddHandler unhandledExceptionHandler.DebuggerLaunched, New EventHandler(AddressOf Me.OnDebuggerLaunched)
            Me.debug.Visible = True
            Me.poweredBy.Visible = False
        End If
        If Not reportExceptionEventArgs.CanSendReport Then
            Me.sendReportButton.Enabled = False
            If Me.dontSendReport.CanFocus Then
                Me.dontSendReport.Focus()
            End If
        End If
        Me.email.Text = RegistryHelper.ReadHKLMRegistryString("Email")
        AddHandler unhandledExceptionHandler.SendingReportFeedback, New SendingReportFeedbackEventHandler(AddressOf Me.OnFeedback)
    End Sub

    Private Sub cancelSending_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            If (Not Me.workingThread Is Nothing) Then
                Me.workingThread.Abort()
            End If
        Catch
        End Try
        MyBase.Close()
    End Sub

    Private Sub continueCheckBox_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Me.reportExceptionEventArgs.TryToContinue = Me.continueCheckBox.Checked
    End Sub

    Private Sub continueSendReport_Click(ByVal sender As Object, ByVal e As EventArgs)
        If (Not Me.sendAnonymously.Checked AndAlso (Not Me.reportExceptionEventArgs Is Nothing)) Then
            Me.reportExceptionEventArgs.AddCustomProperty("Email", Me.email.Text)
            RegistryHelper.SaveHKLMRegistryString("Email", Me.email.Text)
        End If
        Me.SendReport()
    End Sub

    Private Sub debug_Click(ByVal sender As Object, ByVal e As EventArgs)
        If (Not Me.reportExceptionEventArgs Is Nothing) Then
            Me.StartWorkingThread(New ThreadStart(AddressOf Me.reportExceptionEventArgs.LaunchDebugger))
        End If
    End Sub

    Private Sub DebuggerLaunched(ByVal sender As Object, ByVal e As EventArgs)
        MyBase.Close()
    End Sub

    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If (disposing AndAlso (Not Me.components Is Nothing)) Then
            Me.components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private Sub dontSendReport_Click(ByVal sender As Object, ByVal e As EventArgs)
        MyBase.Close()
    End Sub

    Private Sub email_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Me.continueSendReport.Enabled = ((Me.email.Text.Length > 0) OrElse Me.sendAnonymously.Checked)
    End Sub

    Private Sub Feedback(ByVal sender As Object, ByVal e As SendingReportFeedbackEventArgs)
        Select Case e.Step
            Case SendingReportStep.PreparingReport
                If Not e.Failed Then
                    Me.preparingFeedback.Start()
                    Return
                End If
                Me.preparingFeedback.Stop(e.ErrorMessage)
                Me.retrySending.Visible = True
                Me.retrySending.Focus()
                Return
            Case SendingReportStep.ConnectingToServer
                If Not e.Failed Then
                    Me.preparingFeedback.Stop()
                    Me.connectingFeedback.Start()
                    Return
                End If
                Me.connectingFeedback.Stop(e.ErrorMessage)
                Me.retrySending.Visible = True
                Me.retrySending.Focus()
                Return
            Case SendingReportStep.Transfering
                If Not e.Failed Then
                    Me.connectingFeedback.Stop()
                    Me.transferingFeedback.Start()
                    Me.waitSendingReport.Visible = True
                    Return
                End If
                Me.waitSendingReport.Visible = False
                Me.transferingFeedback.Stop(e.ErrorMessage)
                Me.retrySending.Visible = True
                Me.retrySending.Focus()
                Return
            Case SendingReportStep.Finished
                Me.waitSendingReport.Visible = False
                Me.transferingFeedback.Stop()
                Me.completedFeedback.Stop()
                Me.ok.Enabled = True
                Me.ok.Focus()
                Me.cancelSending.Enabled = False
                Return
        End Select
    End Sub

    Private Function GetConvertedString(ByVal s As String) As String
        s = s.Replace("%AppName%", "{1fe9e38e-05cc-46a3-ae48-6cda8fb62056}")
        s = s.Replace("%CompanyName%", "{395edd3b-130e-4160-bb08-6931086cea46}")
        Return s
    End Function

    Private Sub InitializeComponent()
        Me.panelInformation = New Panel
        Me.debug = New Button
        Me.continueCheckBox = New CheckBox
        Me.pleaseTellTitle = New Label
        Me.dontSendReport = New Button
        Me.sendReportButton = New Button
        Me.pleaseTellMessage = New Label
        Me.headerControl1 = New HeaderControl
        Me.errorMessage = New AutoHeightLabel
        Me.saveAsFile = New Button
        Me.panelSending = New Panel
        Me.cancelSending = New Button
        Me.ok = New Button
        Me.retrySending = New Button
        Me.waitSendingReport = New WaitSendingReportControl
        Me.headerControl2 = New HeaderControl
        Me.preparingFeedback = New FeedbackControl
        Me.connectingFeedback = New FeedbackControl
        Me.transferingFeedback = New FeedbackControl
        Me.completedFeedback = New FeedbackControl
        Me.panelEmail = New Panel
        Me.labelEmail = New Label
        Me.sendAnonymously = New CheckBox
        Me.email = New TextBox
        Me.headerControl3 = New HeaderControl
        Me.label3 = New Label
        Me.continueSendReport = New Button
        Me.poweredBy = New poweredBy
        Me.panelInformation.SuspendLayout()
        Me.panelSending.SuspendLayout()
        Me.panelEmail.SuspendLayout()
        MyBase.SuspendLayout()
        Me.panelInformation.Controls.Add(Me.debug)
        Me.panelInformation.Controls.Add(Me.continueCheckBox)
        Me.panelInformation.Controls.Add(Me.pleaseTellTitle)
        Me.panelInformation.Controls.Add(Me.dontSendReport)
        Me.panelInformation.Controls.Add(Me.sendReportButton)
        Me.panelInformation.Controls.Add(Me.pleaseTellMessage)
        Me.panelInformation.Controls.Add(Me.headerControl1)
        Me.panelInformation.Controls.Add(Me.errorMessage)
        Me.panelInformation.Controls.Add(Me.saveAsFile)
        Me.panelInformation.Location = New Point(8, 8)
        Me.panelInformation.Name = "panelInformation"
        Me.panelInformation.Size = New Size(&H19D, 240)
        Me.panelInformation.TabIndex = 0
        Me.debug.Anchor = (AnchorStyles.Right Or AnchorStyles.Bottom)
        Me.debug.FlatStyle = FlatStyle.System
        Me.debug.Location = New Point(&H42, &HCD)
        Me.debug.Name = "debug"
        Me.debug.Size = New Size(&H40, &H18)
        Me.debug.TabIndex = 13
        Me.debug.Text = "Debug"
        Me.debug.Visible = False
        AddHandler Me.debug.Click, New EventHandler(AddressOf Me.debug_Click)
        Me.continueCheckBox.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
        Me.continueCheckBox.FlatStyle = FlatStyle.System
        Me.continueCheckBox.Location = New Point(&H16, &H63)
        Me.continueCheckBox.Name = "continueCheckBox"
        Me.continueCheckBox.Size = New Size(&HE2, &H10)
        Me.continueCheckBox.TabIndex = 14
        Me.continueCheckBox.Text = "Ignore this error and attempt to &continue."
        AddHandler Me.continueCheckBox.CheckedChanged, New EventHandler(AddressOf Me.continueCheckBox_CheckedChanged)
        Me.pleaseTellTitle.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or AnchorStyles.Bottom))
        Me.pleaseTellTitle.FlatStyle = FlatStyle.System
        Me.pleaseTellTitle.Font = New Font("Microsoft Sans Serif", 8.25!, FontStyle.Bold, GraphicsUnit.Point, 0)
        Me.pleaseTellTitle.Location = New Point(20, &H7C)
        Me.pleaseTellTitle.Name = "pleaseTellTitle"
        Me.pleaseTellTitle.Size = New Size(&H17D, &H10)
        Me.pleaseTellTitle.TabIndex = 11
        Me.pleaseTellTitle.Text = "Please tell %CompanyName% about this problem."
        Me.dontSendReport.Anchor = (AnchorStyles.Right Or AnchorStyles.Bottom)
        Me.dontSendReport.FlatStyle = FlatStyle.System
        Me.dontSendReport.Location = New Point(&H145, &HCD)
        Me.dontSendReport.Name = "dontSendReport"
        Me.dontSendReport.Size = New Size(&H4B, &H18)
        Me.dontSendReport.TabIndex = 6
        Me.dontSendReport.Text = "&Don't Send"
        AddHandler Me.dontSendReport.Click, New EventHandler(AddressOf Me.dontSendReport_Click)
        Me.sendReportButton.Anchor = (AnchorStyles.Right Or AnchorStyles.Bottom)
        Me.sendReportButton.FlatStyle = FlatStyle.System
        Me.sendReportButton.Location = New Point(&HD6, &HCD)
        Me.sendReportButton.Name = "sendReportButton"
        Me.sendReportButton.Size = New Size(&H69, &H18)
        Me.sendReportButton.TabIndex = 9
        Me.sendReportButton.Text = "&Send Error Report"
        AddHandler Me.sendReportButton.Click, New EventHandler(AddressOf Me.sendReport_Click)
        Me.pleaseTellMessage.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or AnchorStyles.Bottom))
        Me.pleaseTellMessage.FlatStyle = FlatStyle.System
        Me.pleaseTellMessage.Location = New Point(20, 140)
        Me.pleaseTellMessage.Name = "pleaseTellMessage"
        Me.pleaseTellMessage.Size = New Size(&H17D, &H37)
        Me.pleaseTellMessage.TabIndex = 12
        Me.pleaseTellMessage.Text = "To help improve the software you use, %CompanyName% is interested in learning more about this error. We have created a report about the error for you to send to us."
        Me.headerControl1.BackColor = Color.FromArgb(240, &HEE, &HE1)
        Me.headerControl1.Dock = DockStyle.Top
        Me.headerControl1.IconState = IconState.Error
        Me.headerControl1.Image = Nothing
        Me.headerControl1.Location = New Point(0, 0)
        Me.headerControl1.Name = "headerControl1"
        Me.headerControl1.Size = New Size(&H19D, &H3A)
        Me.headerControl1.TabIndex = 3
        Me.headerControl1.TabStop = False
        Me.headerControl1.Text = "%AppName% has encountered a problem." & ChrW(10) & "We are sorry for the inconvenience."
        Me.errorMessage.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or AnchorStyles.Top))
        Me.errorMessage.FlatStyle = FlatStyle.System
        Me.errorMessage.Location = New Point(20, &H45)
        Me.errorMessage.Name = "errorMessage"
        Me.errorMessage.Size = New Size(&H17D, 13)
        Me.errorMessage.TabIndex = 10
        Me.errorMessage.Text = "errorMessage"
        Me.errorMessage.UseMnemonic = False
        Me.saveAsFile.Anchor = (AnchorStyles.Right Or AnchorStyles.Bottom)
        Me.saveAsFile.FlatStyle = FlatStyle.System
        Me.saveAsFile.Location = New Point(&H88, &HCD)
        Me.saveAsFile.Name = "saveAsFile"
        Me.saveAsFile.Size = New Size(&H48, &H18)
        Me.saveAsFile.TabIndex = 11
        Me.saveAsFile.Text = "Save as &File"
        AddHandler Me.saveAsFile.Click, New EventHandler(AddressOf Me.saveAsFile_Click)
        Me.panelSending.Controls.Add(Me.cancelSending)
        Me.panelSending.Controls.Add(Me.ok)
        Me.panelSending.Controls.Add(Me.retrySending)
        Me.panelSending.Controls.Add(Me.waitSendingReport)
        Me.panelSending.Controls.Add(Me.headerControl2)
        Me.panelSending.Controls.Add(Me.preparingFeedback)
        Me.panelSending.Controls.Add(Me.connectingFeedback)
        Me.panelSending.Controls.Add(Me.transferingFeedback)
        Me.panelSending.Controls.Add(Me.completedFeedback)
        Me.panelSending.Location = New Point(8, &H108)
        Me.panelSending.Name = "panelSending"
        Me.panelSending.Size = New Size(&H19D, &HE8)
        Me.panelSending.TabIndex = 2
        Me.panelSending.Visible = False
        Me.cancelSending.Anchor = (AnchorStyles.Right Or AnchorStyles.Bottom)
        Me.cancelSending.FlatStyle = FlatStyle.System
        Me.cancelSending.Location = New Point(320, &HC5)
        Me.cancelSending.Name = "cancelSending"
        Me.cancelSending.Size = New Size(80, &H18)
        Me.cancelSending.TabIndex = 10
        Me.cancelSending.Text = "&Cancel"
        AddHandler Me.cancelSending.Click, New EventHandler(AddressOf Me.cancelSending_Click)
        Me.ok.Anchor = (AnchorStyles.Right Or AnchorStyles.Bottom)
        Me.ok.Enabled = False
        Me.ok.FlatStyle = FlatStyle.System
        Me.ok.Location = New Point(&HE8, &HC5)
        Me.ok.Name = "ok"
        Me.ok.Size = New Size(80, &H18)
        Me.ok.TabIndex = &H16
        Me.ok.Text = "&OK"
        AddHandler Me.ok.Click, New EventHandler(AddressOf Me.ok_Click)
        Me.retrySending.Anchor = (AnchorStyles.Right Or AnchorStyles.Bottom)
        Me.retrySending.FlatStyle = FlatStyle.System
        Me.retrySending.Location = New Point(&H90, &HC5)
        Me.retrySending.Name = "retrySending"
        Me.retrySending.Size = New Size(80, &H18)
        Me.retrySending.TabIndex = &H17
        Me.retrySending.Text = "&Retry"
        Me.retrySending.Visible = False
        AddHandler Me.retrySending.Click, New EventHandler(AddressOf Me.retrySending_Click)
        Me.waitSendingReport.Location = New Point(&H57, &H91)
        Me.waitSendingReport.Name = "waitSendingReport"
        Me.waitSendingReport.TabIndex = 11
        Me.waitSendingReport.TabStop = False
        Me.waitSendingReport.Visible = False
        Me.headerControl2.BackColor = Color.FromArgb(240, &HEE, &HE1)
        Me.headerControl2.Dock = DockStyle.Top
        Me.headerControl2.IconState = IconState.Error
        Me.headerControl2.Image = Nothing
        Me.headerControl2.Location = New Point(0, 0)
        Me.headerControl2.Name = "headerControl2"
        Me.headerControl2.Size = New Size(&H19D, &H3A)
        Me.headerControl2.TabIndex = &H18
        Me.headerControl2.TabStop = False
        Me.headerControl2.Text = "Please wait while %AppName% is sending the report to %CompanyName% through the Internet."
        Me.preparingFeedback.Location = New Point(&H18, &H48)
        Me.preparingFeedback.Name = "preparingFeedback"
        Me.preparingFeedback.Size = New Size(&H170, &H10)
        Me.preparingFeedback.TabIndex = &H12
        Me.preparingFeedback.TabStop = False
        Me.preparingFeedback.Text = "Preparing the error report."
        Me.connectingFeedback.Location = New Point(&H18, &H60)
        Me.connectingFeedback.Name = "connectingFeedback"
        Me.connectingFeedback.Size = New Size(&H170, &H10)
        Me.connectingFeedback.TabIndex = &H13
        Me.connectingFeedback.TabStop = False
        Me.connectingFeedback.Text = "Connecting to server."
        Me.transferingFeedback.Location = New Point(&H18, 120)
        Me.transferingFeedback.Name = "transferingFeedback"
        Me.transferingFeedback.Size = New Size(&H170, &H10)
        Me.transferingFeedback.TabIndex = 20
        Me.transferingFeedback.TabStop = False
        Me.transferingFeedback.Text = "Transferring report."
        Me.completedFeedback.Location = New Point(&H18, &H90)
        Me.completedFeedback.Name = "completedFeedback"
        Me.completedFeedback.Size = New Size(&H170, &H10)
        Me.completedFeedback.TabIndex = &H15
        Me.completedFeedback.TabStop = False
        Me.completedFeedback.Text = "Error reporting completed. Thank you."
        Me.panelEmail.Controls.Add(Me.labelEmail)
        Me.panelEmail.Controls.Add(Me.sendAnonymously)
        Me.panelEmail.Controls.Add(Me.email)
        Me.panelEmail.Controls.Add(Me.headerControl3)
        Me.panelEmail.Controls.Add(Me.label3)
        Me.panelEmail.Controls.Add(Me.continueSendReport)
        Me.panelEmail.Location = New Point(11, &H200)
        Me.panelEmail.Name = "panelEmail"
        Me.panelEmail.Size = New Size(&H19D, &HE8)
        Me.panelEmail.TabIndex = 4
        Me.panelEmail.Visible = False
        Me.labelEmail.FlatStyle = FlatStyle.System
        Me.labelEmail.Location = New Point(20, &H83)
        Me.labelEmail.Name = "labelEmail"
        Me.labelEmail.Size = New Size(100, &H10)
        Me.labelEmail.TabIndex = 9
        Me.labelEmail.Text = "&Email address:"
        Me.sendAnonymously.FlatStyle = FlatStyle.System
        Me.sendAnonymously.Location = New Point(120, 160)
        Me.sendAnonymously.Name = "sendAnonymously"
        Me.sendAnonymously.Size = New Size(&HD8, &H10)
        Me.sendAnonymously.TabIndex = 11
        Me.sendAnonymously.Text = "I prefer to send this report &anonymously."
        AddHandler Me.sendAnonymously.CheckedChanged, New EventHandler(AddressOf Me.sendAnonymously_CheckedChanged)
        Me.email.Location = New Point(120, &H80)
        Me.email.Name = "email"
        Me.email.Size = New Size(&H100, 20)
        Me.email.TabIndex = 10
        Me.email.Text = ""
        AddHandler Me.email.TextChanged, New EventHandler(AddressOf Me.email_TextChanged)
        Me.headerControl3.BackColor = Color.FromArgb(240, &HEE, &HE1)
        Me.headerControl3.Dock = DockStyle.Top
        Me.headerControl3.IconState = IconState.Error
        Me.headerControl3.Image = Nothing
        Me.headerControl3.Location = New Point(0, 0)
        Me.headerControl3.Name = "headerControl3"
        Me.headerControl3.Size = New Size(&H19D, &H3A)
        Me.headerControl3.TabIndex = 3
        Me.headerControl3.TabStop = False
        Me.headerControl3.Text = "Do you want to be contacted by %CompanyName% regarding this problem?"
        Me.label3.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or AnchorStyles.Top))
        Me.label3.FlatStyle = FlatStyle.System
        Me.label3.Location = New Point(20, &H45)
        Me.label3.Name = "label3"
        Me.label3.Size = New Size(&H17D, &H2B)
        Me.label3.TabIndex = 10
        Me.label3.Text = "If you want to be contacted by %CompanyName% regarding this error, please provide your e-mail address. This information will not be used for any other purpose."
        Me.continueSendReport.Anchor = (AnchorStyles.Right Or AnchorStyles.Bottom)
        Me.continueSendReport.Enabled = False
        Me.continueSendReport.FlatStyle = FlatStyle.System
        Me.continueSendReport.Location = New Point(&H127, &HC5)
        Me.continueSendReport.Name = "continueSendReport"
        Me.continueSendReport.Size = New Size(&H69, &H18)
        Me.continueSendReport.TabIndex = 12
        Me.continueSendReport.Text = "&Send Error Report"
        AddHandler Me.continueSendReport.Click, New EventHandler(AddressOf Me.continueSendReport_Click)
        Me.poweredBy.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
        Me.poweredBy.Cursor = Cursors.Hand
        Me.poweredBy.Location = New Point(6, 730)
        Me.poweredBy.Name = "poweredBy"
        Me.poweredBy.Size = New Size(120, &H20)
        Me.poweredBy.TabIndex = 5
        Me.poweredBy.TabStop = False
        Me.poweredBy.Text = "poweredBy1"
        Me.AutoScaleBaseSize = New Size(5, 13)
        Me.BackColor = SystemColors.Window
        MyBase.ClientSize = New Size(&H1B2, &H300)
        MyBase.ControlBox = False
        MyBase.Controls.Add(Me.poweredBy)
        MyBase.Controls.Add(Me.panelEmail)
        MyBase.Controls.Add(Me.panelInformation)
        MyBase.Controls.Add(Me.panelSending)
        MyBase.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        MyBase.Name = "ExceptionReportingForm"
        MyBase.ShowInTaskbar = False
        MyBase.StartPosition = FormStartPosition.CenterScreen
        Me.Text = "%AppName%"
        MyBase.TopMost = True
        Me.panelInformation.ResumeLayout(False)
        Me.panelSending.ResumeLayout(False)
        Me.panelEmail.ResumeLayout(False)
        MyBase.ResumeLayout(False)
    End Sub

    Private Sub ok_Click(ByVal sender As Object, ByVal e As EventArgs)
        MyBase.Close()
    End Sub

    Protected Overrides Sub OnClosing(ByVal e As CancelEventArgs)
        If ((Not Me.workingThread Is Nothing) AndAlso Me.workingThread.IsAlive) Then
            Me.workingThread.Abort()
        End If
        MyBase.OnClosing(e)
    End Sub

    Private Sub OnDebuggerLaunched(ByVal sender As Object, ByVal e As EventArgs)
        Try
            MyBase.Invoke(New EventHandler(AddressOf Me.DebuggerLaunched), New Object() {sender, e})
        Catch exception1 As InvalidOperationException
        End Try
    End Sub

    Private Sub OnFeedback(ByVal sender As Object, ByVal e As SendingReportFeedbackEventArgs)
        Try
            MyBase.Invoke(New SendingReportFeedbackEventHandler(AddressOf Me.Feedback), New Object() {sender, e})
        Catch exception1 As InvalidOperationException
        End Try
    End Sub

    Private Sub retrySending_Click(ByVal sender As Object, ByVal e As EventArgs)
        Me.retrySending.Visible = False
        Me.preparingFeedback.Init()
        Me.connectingFeedback.Init()
        Me.transferingFeedback.Init()
        If (Not Me.reportExceptionEventArgs Is Nothing) Then
            Me.StartWorkingThread(New ThreadStart(AddressOf Me.StartSendReport))
        End If
    End Sub

    Private Sub saveAsFile_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim dialog As New SaveFileDialog
        dialog.Title = "Save an Exception Report"
        ' WARNING: To open the encrypted report with SA, the file must have a .saencryptedreport extension.
        dialog.DefaultExt = "saencryptedreport"
        dialog.Filter = "SmartAssembly Encrypted Exception Report|*.saencryptedreport"
        If (dialog.ShowDialog(Me) <>  Windows.Forms.DialogResult.Cancel) Then
            If Me.reportExceptionEventArgs.SaveEncryptedReport(dialog.FileName) Then
                MessageBox.Show(String.Format("Please send the Exception Report to {0} Support Team.", "{395edd3b-130e-4160-bb08-6931086cea46}"), "{1fe9e38e-05cc-46a3-ae48-6cda8fb62056}", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
                MyBase.Close()
            Else
                MessageBox.Show("Failed to save the report.", "{1fe9e38e-05cc-46a3-ae48-6cda8fb62056}", MessageBoxButtons.OK, MessageBoxIcon.Hand)
            End If
        End If
    End Sub

    Private Sub sendAnonymously_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Me.email.Enabled = Not Me.sendAnonymously.Checked
        Me.continueSendReport.Enabled = ((Me.email.Text.Length > 0) OrElse Me.sendAnonymously.Checked)
    End Sub

    Public Sub SendReport()
        Try
            Me.panelEmail.Visible = False
            Me.panelSending.Visible = True
            If (Not Me.reportExceptionEventArgs Is Nothing) Then
                Me.StartWorkingThread(New ThreadStart(AddressOf Me.StartSendReport))
            End If
        Catch
        End Try
    End Sub

    Private Sub sendReport_Click(ByVal sender As Object, ByVal e As EventArgs)
        Me.panelInformation.Visible = False
        Me.panelEmail.Visible = True
    End Sub

    Private Sub StartSendReport()
        Me.reportExceptionEventArgs.SendReport()
    End Sub

    Private Sub StartWorkingThread(ByVal start As ThreadStart)
        Me.workingThread = New Thread(start)
        Me.workingThread.Start()
    End Sub


    Private cancelSending As Button
    Private completedFeedback As FeedbackControl
    Private components As IContainer
    Private connectingFeedback As FeedbackControl
    Private continueCheckBox As CheckBox
    Private continueSendReport As Button
    Private debug As Button
    Private dontSendReport As Button
    Private email As TextBox
    Private errorMessage As AutoHeightLabel
    Private headerControl1 As HeaderControl
    Private headerControl2 As HeaderControl
    Private headerControl3 As HeaderControl
    Private label3 As Label
    Private labelEmail As Label
    Private ok As Button
    Private panelEmail As Panel
    Private panelInformation As Panel
    Private panelSending As Panel
    Private pleaseTellMessage As Label
    Private pleaseTellTitle As Label
    Private poweredBy As poweredBy
    Private preparingFeedback As FeedbackControl
    Private reportExceptionEventArgs As reportExceptionEventArgs
    Private retrySending As Button
    Private saveAsFile As Button
    Private sendAnonymously As CheckBox
    Private sendReportButton As Button
    Private transferingFeedback As FeedbackControl
    Private unhandledExceptionHandler As unhandledExceptionHandler
    Private waitSendingReport As WaitSendingReportControl
    Private workingThread As Thread
End Class
