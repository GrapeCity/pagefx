'Author:
'   V. Sudharsan (vsudharsan@novell.com)
'
' (C) 2005 Novell, Inc.
Option Strict Off

Class C
    Public Function fun(ByVal i As Integer, ByVal ParamArray a() As Short)
        Return 10
    End Function
    Public Function fun(ByVal ParamArray a() As Long)
        Return 20
    End Function
End Class

Module M
	Function _Main() As Integer
        Dim o As Object = New C()
        Dim a As Integer = o.fun(1, 2, 3)
        If a <> 20 Then
            System.Console.WriteLine("#A1 - Binding not proper") : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
