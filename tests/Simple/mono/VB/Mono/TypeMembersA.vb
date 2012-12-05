Option Strict Off
'Author:
'   V. Sudharsan (vsudharsan@novell.com)
'
' (C) 2005 Novell, Inc.

Class C
    Public Function fun()
        Return 10
    End Function
    Private Function fun(ByVal a As Integer)
        Return 20
    End Function
End Class

Module M
	Function _Main() As Integer
        Dim o As Object = New C()
        If o.fun() <> 10 Then
            System.Console.WriteLine("#A1 - Binding not proper") : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
