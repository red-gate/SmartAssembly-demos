Imports System
Imports System.Drawing
Imports System.Diagnostics
Imports System.Collections
Imports System.ComponentModel
Imports System.Text
Imports System.Windows.Forms
Imports Microsoft.Win32

Imports SmartAssembly.SmartExceptionsCore
Imports SmartAssembly.SmartExceptionsCore.UI

    Public Class SecurityExceptionForm
        Inherits Form

        Public Sub New()
            Me.securityExceptionEventArgs = Nothing
            Me.InitializeComponent()
            MyBase.Icon = Win32.GetApplicationIcon
            Me.Text = Me.GetConvertedString(Me.Text)
            If (Me.Text.Length = 0) Then
                Me.Text = "Security Exception"
            End If
            Dim control As Control
            For Each control In MyBase.Controls
                control.Text = Me.GetConvertedString(control.Text)
                Dim control2 As Control
                For Each control2 In control.Controls
                    control2.Text = Me.GetConvertedString(control2.Text)
                Next
            Next
        End Sub

        Public Sub New(ByVal securityExceptionEventArgs As SecurityExceptionEventArgs)
            Me.New()
            If (Not securityExceptionEventArgs Is Nothing) Then
                If Not securityExceptionEventArgs.CanContinue Then
                    Me.continueButton.Visible = False
                End If
                Me.securityExceptionEventArgs = securityExceptionEventArgs
                If (securityExceptionEventArgs.SecurityMessage.Length > 0) Then
                    Me.errorMessage.Text = securityExceptionEventArgs.SecurityMessage
                Else
                    Dim builder As New StringBuilder
                    builder.Append("%AppName% attempted to perform an operation not allowed by the security policy. To grant this application the required permission, contact your system administrator, or use the Microsoft .NET Framework Configuration tool." & ChrW(10) & ChrW(10))
                    If securityExceptionEventArgs.CanContinue Then
                        builder.Append("If you click Continue, the application will ignore this error and attempt to continue. If you click Quit, the application will close immediately." & ChrW(10) & ChrW(10))
                    End If
                    builder.Append(securityExceptionEventArgs.SecurityException.Message)
                    Me.errorMessage.Text = Me.GetConvertedString(builder.ToString)
                End If
                Dim height As Integer = (Me.errorMessage.Bottom + 60)
                If (height > MyBase.ClientSize.Height) Then
                    MyBase.ClientSize = New Size(MyBase.ClientSize.Width, height)
                End If
            End If
        End Sub

        Private Sub continueButton_Click(ByVal sender As Object, ByVal e As EventArgs)
            If (Not Me.securityExceptionEventArgs Is Nothing) Then
                Me.securityExceptionEventArgs.TryToContinue = True
            End If
            MyBase.Close()
        End Sub

        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing AndAlso (Not Me.components Is Nothing)) Then
                Me.components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Function GetConvertedString(ByVal s As String) As String
            s = s.Replace("%AppName%", "{1fe9e38e-05cc-46a3-ae48-6cda8fb62056}")
            s = s.Replace("%CompanyName%", "{395edd3b-130e-4160-bb08-6931086cea46}")
            Return s
        End Function

        Private Sub InitializeComponent()
            Me.quitButton = New Button
            Me.continueButton = New Button
            Me.headerControl1 = New HeaderControl
            Me.errorMessage = New AutoHeightLabel
            Me.poweredBy = New PoweredBy
            MyBase.SuspendLayout()
            Me.quitButton.Anchor = (AnchorStyles.Right Or AnchorStyles.Bottom)
            Me.quitButton.FlatStyle = FlatStyle.System
            Me.quitButton.Location = New Point(&H134, &HBC)
            Me.quitButton.Name = "quitButton"
            Me.quitButton.Size = New Size(100, &H18)
            Me.quitButton.TabIndex = 0
            Me.quitButton.Text = "&Quit"
            AddHandler Me.quitButton.Click, New EventHandler(AddressOf Me.quitButton_Click)
            Me.continueButton.Anchor = (AnchorStyles.Right Or AnchorStyles.Bottom)
            Me.continueButton.FlatStyle = FlatStyle.System
            Me.continueButton.Location = New Point(&HCA, &HBC)
            Me.continueButton.Name = "continueButton"
            Me.continueButton.Size = New Size(100, &H18)
            Me.continueButton.TabIndex = 1
            Me.continueButton.Text = "&Continue"
            AddHandler Me.continueButton.Click, New EventHandler(AddressOf Me.continueButton_Click)
            Me.headerControl1.BackColor = Color.FromArgb(240, &HEE, &HE1)
            Me.headerControl1.Dock = DockStyle.Top
            Me.headerControl1.IconState = IconState.Warning
            Me.headerControl1.Image = Nothing
            Me.headerControl1.Location = New Point(0, 0)
            Me.headerControl1.Name = "headerControl1"
            Me.headerControl1.Size = New Size(&H1A2, &H3A)
            Me.headerControl1.TabIndex = 7
            Me.headerControl1.TabStop = False
            Me.headerControl1.Text = "%AppName% attempted to perform an operation not allowed by the security policy."
            Me.errorMessage.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or AnchorStyles.Top))
            Me.errorMessage.FlatStyle = FlatStyle.System
            Me.errorMessage.Location = New Point(20, &H48)
            Me.errorMessage.Name = "errorMessage"
            Me.errorMessage.Size = New Size(&H17E, 13)
            Me.errorMessage.TabIndex = 14
            Me.errorMessage.Text = "errorMessage"
            Me.errorMessage.UseMnemonic = False
            Me.poweredBy.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
            Me.poweredBy.Cursor = Cursors.Hand
            Me.poweredBy.Location = New Point(6, &HBA)
            Me.poweredBy.Name = "poweredBy"
            Me.poweredBy.Size = New Size(120, &H20)
            Me.poweredBy.TabIndex = 15
            Me.poweredBy.TabStop = False
            Me.poweredBy.Text = "poweredBy1"
            Me.AutoScaleBaseSize = New Size(5, 13)
            Me.BackColor = SystemColors.Window
            MyBase.ClientSize = New Size(&H1A2, &HE0)
            MyBase.ControlBox = False
            MyBase.Controls.Add(Me.continueButton)
            MyBase.Controls.Add(Me.quitButton)
            MyBase.Controls.Add(Me.headerControl1)
            MyBase.Controls.Add(Me.errorMessage)
            MyBase.Controls.Add(Me.poweredBy)
            MyBase.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
            MyBase.MaximizeBox = False
            MyBase.MinimizeBox = False
            MyBase.Name = "SecurityExceptionForm"
            MyBase.ShowInTaskbar = False
            MyBase.StartPosition = FormStartPosition.CenterScreen
            Me.Text = "%AppName%"
            MyBase.ResumeLayout(False)
        End Sub

        Private Sub quitButton_Click(ByVal sender As Object, ByVal e As EventArgs)
            If (Not Me.securityExceptionEventArgs Is Nothing) Then
                Me.securityExceptionEventArgs.TryToContinue = False
            End If
            MyBase.Close()
        End Sub

        Private components As IContainer
        Private continueButton As Button
        Private errorMessage As AutoHeightLabel
        Private headerControl1 As HeaderControl
        Private poweredBy As poweredBy
        Private quitButton As Button
        Private securityExceptionEventArgs As securityExceptionEventArgs
End Class
