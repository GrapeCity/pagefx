'Author: Ritvik Mayank <mritvik@novell.com>
'Copyright (C) 2005 Novell, Inc (http://www.novell.com)

Imports System

Module ExpressionParenthesized
	Function _Main() As Integer
        Dim A, B, C, D, E, F, G, H As Double
        A = 3.0
        B = 6.0
        C = 4.0
        D = 2.0
        E = 1.0
        F = A + B - C / D * E
        H = (A + B) - ((C / D) * E)
        If F <> H Then
            System.Console.WriteLine("Unexpected value for Expression. F should be Equal to H  ") : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module

