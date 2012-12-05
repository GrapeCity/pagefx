'Author:
'   V. Sudharsan (vsudharsan@novell.com)
'
' (C) 2005 Novell, Inc.
Option Strict Off

'Note That fun need not be present in base class to get shadowed
Class A
    Function fun1()
    End Function
End Class

Class AB
    Inherits A
    Shadows Function fun()
    End Function
End Class

Module ShadowE
	Function _Main() As Integer
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
