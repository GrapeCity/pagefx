'Author:
'   V. Sudharsan (vsudharsan@novell.com)
'
' (C) 2005 Novell, Inc.

MustInherit Class A
    MustInherit Class B
    End Class
End Class

Class C
    Inherits A
End Class

Module InheritanceN
	Function _Main() As Integer
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
