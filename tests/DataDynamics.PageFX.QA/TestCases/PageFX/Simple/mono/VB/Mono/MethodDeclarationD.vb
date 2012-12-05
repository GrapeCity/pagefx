'Author:
'   V. Sudharsan (vsudharsan@novell.com)
'
' (C) 2005 Novell, Inc.

Imports System

Module MethodDeclarationA
    Function A(ByRef i As Integer) As Integer
        i = 19
    End Function
	Function _Main() As Integer
        A(10)
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module

