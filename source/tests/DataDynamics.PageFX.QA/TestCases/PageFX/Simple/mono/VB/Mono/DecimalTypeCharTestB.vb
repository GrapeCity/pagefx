Imports System
Module DecimalTypeCharTest
	Function _Main() As Integer
        Dim m As Decimal
        m = f(20.2D)
        If m <> 20.2D Then
            System.Console.WriteLine("DecimalTypeCharTest: failed") : Return 1
        End If
        Exit Function
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub


    Function f@(ByVal param@)
        f@ = param
    End Function
End Module
