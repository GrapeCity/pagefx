'Author: Ritvik Mayank <mritvik@novell.com>
'Copyright (C) 2005 Novell, Inc (http://www.novell.com)
'Method pointer reclassified.
Option Strict Off

Imports System
Delegate Function A(ByVal c As Integer)
Module Test
    Function B(ByVal c As Integer)
        If c <> 100 Then
            Throw New Exception("Unexpected Behavior C should be equal to 100 but got c =" & c)
        End If
    End Function
	Function _Main() As Integer
        Dim delg As A
        delg = New A(AddressOf B)
        delg.Invoke(100)
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
