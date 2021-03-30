Imports Microsoft.Win32

Friend Class RegistryHelper

    Public Shared Function ReadHKLMRegistryString(ByVal name As String) As String
        Try
            Dim key As RegistryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\MyCompany\MyProduct")
            If (key Is Nothing) Then
                Return String.Empty
            End If
            Dim text As String = CStr(key.GetValue(name, String.Empty))
            key.Close()
            Return [text]
        Catch
            Return String.Empty
        End Try
    End Function

    Public Shared Sub SaveHKLMRegistryString(ByVal name As String, ByVal value As String)
        Try
            Dim key As RegistryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\MyCompany\MyProduct", True)
            If (key Is Nothing) Then
                key = Registry.LocalMachine.CreateSubKey("SOFTWARE\MyCompany\MyProduct")
            End If
            key.SetValue(name, value)
            key.Close()
        Catch
        End Try
    End Sub

    Private Const REGISTRY_ROOT As String = "SOFTWARE\MyCompany\MyProduct"
End Class


