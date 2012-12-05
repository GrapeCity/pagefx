Imports System
Module Scope1
    Public Function S() As Integer
        Return 1
    End Function
End Module
Module Scope
    Function _Main() As Integer
        Dim a As Integer = S()
        If a <> 1 Then
            System.Console.WriteLine("ScopeA:Failed-public method should be visible in other modules too") : Return 1
        End If
    End Function
    Sub Main()
        _Main()
        System.Console.WriteLine("<%END%>")
    End Sub
End Module
