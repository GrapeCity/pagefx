Option Strict Off
Imports System
Module DecimalLiteral
	Function _Main() As Integer
        Dim a As Decimal = True
        If a <> -1 Then
            System.Console.WriteLine("DecimalLiteralB:Failed") : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
