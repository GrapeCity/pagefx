'Author: Ritvik Mayank <mritvik@novell.com>
'Copyright (C) 2005 Novell, Inc (http://www.novell.com)
Option Strict Off
Imports System
Module ConversionAndOperator

	Function _Main() As Integer
        Dim A As Integer = 3
        Dim B As Boolean = True
        Dim R As Boolean
        R = A And B     '0000 And -1 = 1111 
        If R = False Then
            System.Console.WriteLine("#Error With And Operator") : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
