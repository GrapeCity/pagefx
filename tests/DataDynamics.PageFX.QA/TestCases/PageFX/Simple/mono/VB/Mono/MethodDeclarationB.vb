'Author:
'   V. Sudharsan (vsudharsan@novell.com)
'
' (C) 2005 Novell, Inc.

Imports System

Module MethodDeclarationA
    Function A(ByRef i As Integer) As Integer
        i = 10
    End Function
    Function AB() As Integer
        Return 10
    End Function
	Function _Main() As Integer
        A(AB())
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module

