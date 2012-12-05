Option Strict Off
Imports System

Class AA
    Inherits System.MarshalByRefObject
    Public Function fun()
    End Function
End Class


Class AAA
    Public Function fun(ByVal a As AA)
    End Function
End Class

Module Test
	Function _Main() As Integer
        Dim b As Object = New AA()
        Dim a As Object = New AAA()
        a.fun(b)
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module

