'Author: Ritvik Mayank <mritvik@novell.com>
'Copyright (C) 2005 Novell, Inc (http://www.novell.com)

Imports System
Module ExpressionOperator1

	Function _Main() As Integer
        Dim a As Integer = 6
        Dim b As Double = 5.7
        If a < b Then
            System.Console.WriteLine("#Exception") : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
