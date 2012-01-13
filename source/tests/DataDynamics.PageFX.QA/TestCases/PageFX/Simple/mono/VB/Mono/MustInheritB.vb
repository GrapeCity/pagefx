Option Strict Off
'Author:
'   V. Sudharsan (vsudharsan@novell.com)
'
' (C) 2005 Novell, Inc.

MustInherit Class A
    MustOverride Function fun(ByVal a As Integer)
End Class

MustInherit Class AB
    Inherits A
    MustOverride Function fun1(ByVal a As String)
End Class

Class ABC
    Inherits AB
    Overrides Function fun(ByVal a As Integer)
    End Function
    Overrides Function fun1(ByVal a As String)
    End Function
End Class

Module MustInheritF
	Function _Main() As Integer
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module

