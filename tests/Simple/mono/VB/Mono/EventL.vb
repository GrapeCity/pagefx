'Author:
'   V. Sudharsan (vsudharsan@novell.com)
'
' (C) 2005 Novell, Inc.

'This is similar to EventC9.vb. Just the accessor is changed

Class C
    Private Event E()
End Class

Class C1
    Inherits C
    Private Event E()
End Class

Module A
	Function _Main() As Integer
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
