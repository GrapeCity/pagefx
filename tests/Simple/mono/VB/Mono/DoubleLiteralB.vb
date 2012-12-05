Option Strict Off
Imports System
Module DoubleLiteral
	Function _Main() As Integer
        Dim a As Double = True
        If a <> -1 Then
            System.Console.WriteLine("DoubleLiteralB:Failed") : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
