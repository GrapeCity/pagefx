Option Strict Off
'Author:
'   V. Sudharsan (vsudharsan@novell.com)
'
' (C) 2005 Novell, Inc.

Class C
    Public Function fun(ByVal i As Integer, Optional ByVal a1 As Char = "c") As Integer
        If a1 = "c" And i = 2 Then
            Return 10
        End If
        Return 11
    End Function
End Class

Module M
	Function _Main() As Integer
        Dim o As C = New C()
        Dim a As Integer = o.fun(i:=2)
        If a <> 10 Then
            System.Console.WriteLine("#A1 - Binding not proper") : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
