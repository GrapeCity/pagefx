Option Strict Off
Imports System
Module LongLiteral
	Function _Main() As Integer
        Dim a As Long
        a = True
        If a <> -1 Then
            System.Console.WriteLine("#A1:LongLiteralA:Failed") : Return 1
        End If
        a = 1.23
        If a <> 1 Then
            System.Console.WriteLine("#A2:LongLiteralA:Failed") : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
