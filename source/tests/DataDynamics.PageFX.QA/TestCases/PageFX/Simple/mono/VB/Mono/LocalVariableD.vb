'Author:
'   V. Sudharsan (vsudharsan@novell.com)
'
' (C) 2005 Novell, Inc.

Module M
    Function fun() As Integer
        Static Dim y As Integer = 10
        y = y + 1
        Return y
    End Function
    Function fun1() As Integer
        Return fun()
    End Function
	Function _Main() As Integer
        Dim x As Integer
        fun()
        x = fun1()
        If x <> 12 Then
            System.Console.WriteLine("Static declaration not implemented properly. Expected 12 but got " & x) : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
