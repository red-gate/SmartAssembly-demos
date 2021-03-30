Imports System
Imports System.Security
Imports System.Windows.Forms

Imports SmartAssembly.SmartExceptionsCore

Public Class UnhandledExceptionHandlerWithAdvancedUI
    Inherits UnhandledExceptionHandler

    Public Shared Function AttachApp() As Boolean
        Try
            UnhandledExceptionHandler.AttachExceptionHandler(New UnhandledExceptionHandlerWithAdvancedUI)
            Return True
        Catch exception1 As SecurityException
            Try
                Application.EnableVisualStyles()
                Dim form As New SecurityExceptionForm(New SecurityExceptionEventArgs(String.Format("{0} cannot initialize itself because some permissions are not granted." & ChrW(10) & ChrW(10) & "You probably try to launch {0} in a partial-trust situation. It's usually the case when the application is hosted on a network share." & ChrW(10) & ChrW(10) & "You need to run {0} in full-trust, or at least grant it the UnmanagedCode security permission." & ChrW(10) & ChrW(10) & "To grant this application the required permission, contact your system administrator, or use the Microsoft .NET Framework Configuration tool.", "{1fe9e38e-05cc-46a3-ae48-6cda8fb62056}"), False))
                form.ShowInTaskbar = True
                form.ShowDialog()
            Catch exception As exception
                MessageBox.Show(exception.ToString, String.Format("{0} Fatal Error", "{1fe9e38e-05cc-46a3-ae48-6cda8fb62056}"), MessageBoxButtons.OK, MessageBoxIcon.Hand)
            End Try
            Return False
        End Try
    End Function

    Protected Overrides Function GetUserID() As Guid
        Try
            Dim g As String = RegistryHelper.ReadHKLMRegistryString("AnonymousID")
            If (g.Length = 0) Then
                Dim newGuid As Guid = Guid.NewGuid
                RegistryHelper.SaveHKLMRegistryString("AnonymousID", newGuid.ToString("B"))
                If (RegistryHelper.ReadHKLMRegistryString("AnonymousID").Length > 0) Then
                    Return newGuid
                End If
                Return Guid.Empty
            End If
            Return New Guid(g)
        Catch
            Return Guid.Empty
        End Try
    End Function

    Protected Overrides Sub OnFatalException(ByVal e As FatalExceptionEventArgs)
        MessageBox.Show(e.FatalException.ToString, String.Format("{0} Fatal Error", "{1fe9e38e-05cc-46a3-ae48-6cda8fb62056}"), MessageBoxButtons.OK, MessageBoxIcon.Hand)
    End Sub

    Protected Overrides Sub OnReportException(ByVal e As ReportExceptionEventArgs)
        Dim form As ExceptionReportingForm = New ExceptionReportingForm(Me, e)
        form.ShowDialog()
    End Sub

    Protected Overrides Sub OnSecurityException(ByVal e As SecurityExceptionEventArgs)
        Dim form As SecurityExceptionForm = New SecurityExceptionForm(e)
        form.ShowDialog()
    End Sub

End Class

