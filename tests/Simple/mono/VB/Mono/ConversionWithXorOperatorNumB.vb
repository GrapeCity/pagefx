'Author: Ritvik Mayank <mritvik@novell.com>
'Copyright (C) 2005 Novell, Inc (http://www.novell.com)
Option Strict Off
Imports System
Module ConversionXorOperator

	Function _Main() As Integer
        Dim A As Integer = 1
        Dim B As Integer = 3
        Dim R As Boolean
        R = A Xor B     '01 XOr 11 
        If R = False Then
            System.Console.WriteLine("#Error With Xor Operator") : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
