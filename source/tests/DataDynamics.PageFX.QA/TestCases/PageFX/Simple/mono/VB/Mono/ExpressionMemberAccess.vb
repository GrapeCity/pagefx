Option Strict Off
Imports System
Class C
    Public F As Integer = 10
End Class

Module Test
    Public Function ReturnC() As Object
        Console.WriteLine("Returning a new instance of C.")
        Return New C()
    End Function

	Function _Main() As Integer
        If Returnc().F <> 10 Then
            System.Console.WriteLine("Unexpected Behavior Returnc().F should be 10") : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
