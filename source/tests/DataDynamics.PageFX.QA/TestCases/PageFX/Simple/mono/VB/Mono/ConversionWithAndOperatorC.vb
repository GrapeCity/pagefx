'Author: Ritvik Mayank <mritvik@novell.com>
'Copyright (C) 2005 Novell, Inc (http://www.novell.com)
Option Strict Off
Imports System
Module ConversionAndOperator

	Function _Main() As Integer
        Dim A As Integer = 333
        Dim B As Long = 555.555
        Dim R As Boolean
        R = (B < A) And (A < B)
        If R = True Then
            System.Console.WriteLine("#Error With And Operator") : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
