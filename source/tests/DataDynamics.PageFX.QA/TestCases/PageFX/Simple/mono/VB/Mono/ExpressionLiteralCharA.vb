'Author: Ritvik Mayank <mritvik@novell.com>
'Copyright (C) 2005 Novell, Inc (http://www.novell.com)
'CharacterLiteral ::= DoubleQuoteCharacter StringCharacter DoubleQuoteCharacter C
Option Strict Off
Imports System
Imports Microsoft.VisualBasic
Module ExpressionLiteralsChar
	Function _Main() As Integer
        Dim A As Char = "a"c
        Dim B = Chr(97)
        If B <> A Then
            Throw New Exception(" Unexpected Result for the Expression A should be eual to a")
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
