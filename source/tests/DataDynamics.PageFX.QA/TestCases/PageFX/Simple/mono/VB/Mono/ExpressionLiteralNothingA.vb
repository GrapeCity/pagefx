'Author: Ritvik Mayank <mritvik@novell.com>
'Copyright (C) 2005 Novell, Inc (http://www.novell.com)
'Nothing keyword represents the default value of any data type
Option Strict Off
Imports System
Imports Microsoft.VisualBasic

Module ExpressionLiteralsChar
	Function _Main() As Integer
        Dim L As Long, S As String, B As Boolean, O As Object, D As Date
        B = Nothing
        If B <> False Then
            Throw New Exception("Unexpected Behavior of Nothing. As B should be assigned Flase")
        End If
        S = Nothing
        If S <> Nothing Then
            Throw New Exception("Unexpected Behavior of Nothing. As S should be assigned Nothing")
        End If
        D = Nothing
        If D <> #12:00:00 AM# Then
            System.Console.WriteLine("Unexpected Behavior of Nothing. D not set to default value") : Return 1
        End If
        L = Nothing
        If L <> 0 Then
            Throw New Exception("Unexpected Behavior of Nothing. As L should be assigned 0 ")
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module

