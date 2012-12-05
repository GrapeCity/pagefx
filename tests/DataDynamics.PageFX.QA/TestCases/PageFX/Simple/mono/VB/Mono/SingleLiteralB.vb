Option Strict Off
Imports System
Module SingleLiteral
	Function _Main() As Integer
        Dim a As Single = True
        If a <> -1 Then
            System.Console.WriteLine("SingleLiteralB:Failed") : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
