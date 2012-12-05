'Author:
'   V. Sudharsan (vsudharsan@novell.com)
'
' (C) 2005 Novell, Inc.

'To check if a variable without any type is been accepted or not
Option Strict Off

Imports System

Module MethodDeclarationA
    Function A(ByVal i)
    End Function
	Function _Main() As Integer
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module

