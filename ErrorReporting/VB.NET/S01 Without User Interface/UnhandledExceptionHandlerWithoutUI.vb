Imports SmartAssembly.SmartExceptionsCore

Public Class UnhandledExceptionHandlerWithoutUI
    Inherits UnhandledExceptionHandler

    Public Shared Function AttachApp() As Boolean
        UnhandledExceptionHandler.AttachExceptionHandler(New UnhandledExceptionHandlerWithoutUI)
        Return True
    End Function

    Protected Overrides Sub OnFatalException(ByVal e As FatalExceptionEventArgs)
        Throw e.FatalException
    End Sub

    Protected Overrides Sub OnReportException(ByVal e As ReportExceptionEventArgs)
        Dim i As Integer
        For i = 0 To 2
            If e.SendReport Then
                Exit For
            End If
        Next i
        e.TryToContinue = True
    End Sub

    Protected Overrides Sub OnSecurityException(ByVal e As SecurityExceptionEventArgs)
        e.ReportException = True
    End Sub

End Class

