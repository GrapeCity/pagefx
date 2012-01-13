'Author:
'   V. Sudharsan (vsudharsan@novell.com)
'
' (C) 2005 Novell, Inc.

Class A
    Sub fun()
    End Sub
End Class

Class AB
    Inherits A
    Shadows Sub fun()
    End Sub
End Class

Module ShadowE
	Function _Main() As Integer
        Dim a As AB = New AB()
        a.fun()
        CType(a, A).fun()
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
