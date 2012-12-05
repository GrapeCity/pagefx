'Author: Ritvik Mayank <mritvik@novell.com>
'Copyright (C) 2005 Novell, Inc (http://www.novell.com)
Option Strict Off
Imports System
Module ExpressionOperatorDecimal

	Function _Main() As Integer
        Dim a As Decimal = 55.5
        Dim b As Decimal = 66.6
        If a >= b Then
            System.Console.WriteLine("# >= operators: Failed") : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module

