'Author: Ritvik Mayank <mritvik@novell.com>
'Copyright (C) 2005 Novell, Inc (http://www.novell.com)
'CharacterLiteral ::= DoubleQuoteCharacter StringCharacter DoubleQuoteCharacter C
Option Strict Off
Imports System
Imports Microsoft.VisualBasic
Module ExpressionLiteralsChar
	Function _Main() As Integer
        Dim A As Char = """"c
        Dim B = Chr(34)
        If B <> A Then
            Throw New Exception("  Unexpected Result for the Expression :: A should be equal to  ")
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
