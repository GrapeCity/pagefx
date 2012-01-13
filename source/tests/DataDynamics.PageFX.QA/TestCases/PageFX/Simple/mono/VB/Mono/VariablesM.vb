'Author:
'   V. Sudharsan (vsudharsan@novell.com)
'
' (C) 2005 Novell, Inc.
Option Strict Off

Imports System

Class AA
    Inherits System.MarshalByRefObject
    Public Function fun()
    End Function
End Class


Class AAA
    Public Function fun(ByVal a As AA)
    End Function
End Class

Module Test
	Function _Main() As Integer
        Dim b As AA = New AA()
        Dim a As AAA = New AAA()
        a.fun(b)
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module

