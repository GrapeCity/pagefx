Option Strict Off
Imports System
Module SingleTypeCharTest
	Function _Main() As Integer
        Dim m As Integer
        m = f(20)
        If m <> 20 Then
            System.Console.WriteLine("IntegerTypeChar: failed") : Return 1
        End If
        Exit Function
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub


    Function f!(ByVal param!)
        f = param
    End Function
End Module
