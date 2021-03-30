Module Program
    Sub Main(args As String())
        DoSomething()

        Console.WriteLine("Press any key to exit")
        Console.ReadKey()
    End Sub

    Private Sub DoSomething()
        ' The following will throw an exception.
        ' It will be handled by SmartAssembly And
        ' error report will be automatically sent.
        Console.WriteLine($"5 divided by 0 is: {Divide(5, 0)}")
    End Sub

    Private Function Divide(a As Integer, b As Integer) As Integer
        Return a / b
    End Function
End Module
