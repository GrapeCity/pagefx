'Author: Ritvik Mayank <mritvik@novell.com>
'Copyright (C) 2005 Novell, Inc (http://www.novell.com)
Option Strict Off
Imports System
Module ConversionOrOperator

	Function _Main() As Integer
        Dim A As Integer = 1
        Dim B As Integer = 3
        Dim R As Boolean
        R = A Or B  '01 And 11 
        If R = False Then
            System.Console.WriteLine("#Error With Or Operator") : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
