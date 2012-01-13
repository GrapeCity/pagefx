Option Strict Off
Imports System
Module M
	Function _Main() As Integer
        Dim a As Boolean = True
        Dim b As Boolean = False
        Dim c As Boolean
        c = a + b
        If a <> True Then
            System.Console.WriteLine("BoolLiteralB:Failed") : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
