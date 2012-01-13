'Author: Ritvik Mayank <mritvik@novell.com>
'Copyright (C) 2005 Novell, Inc (http://www.novell.com)

Imports System
Module ConversionXorOperator

	Function _Main() As Integer
        Dim A As Integer = 333
        Dim B As Double = 555.555
        Dim R As Boolean
        R = (B < A) Xor (A > B)
        If R = True Then
            System.Console.WriteLine("#Error With Xor Operator") : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
