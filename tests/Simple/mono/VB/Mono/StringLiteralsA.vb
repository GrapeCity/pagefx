Option Strict Off
Imports System
Module StringLiteral
	Function _Main() As Integer
        Dim a As String = "Hello"
        Dim b As String = " World "
        Dim c As String = 47
        Dim d As String = a + b + c
        If d <> "Hello World 47" Then
            System.Console.WriteLine("StringLiteralA:Failed-String concatenation does not work right") : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
