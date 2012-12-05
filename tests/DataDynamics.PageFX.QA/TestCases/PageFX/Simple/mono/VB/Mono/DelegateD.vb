'Author:
'   V. Sudharsan (vsudharsan@novell.com)
'
' (C) 2005 Novell, Inc.

Imports System

MustInherit Class aa
    Delegate Function SD(ByVal i As Integer) As Integer
    MustOverride Function f(ByVal i As Integer) As Integer
End Class

Class ab
    Inherits aa
    Delegate Function SD(ByVal i As Integer) As Integer
    Overrides Function f(ByVal i As Integer) As Integer
    End Function
End Class

Module M
	Function _Main() As Integer
        Dim a As ab = New ab()
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
