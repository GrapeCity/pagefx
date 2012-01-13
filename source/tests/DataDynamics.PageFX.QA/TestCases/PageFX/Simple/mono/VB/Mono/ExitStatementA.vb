'Author:
'   V. Sudharsan (vsudharsan@novell.com)
'
' (C) 2005 Novell, Inc.

Module exitstmt
    Public i As Integer
    Function fun() As Object
        Exit Function
        i = i + 1
    End Function
    Function fun1() As Object
        i = i + 1
        Exit Function
    End Function
	Function _Main() As Integer
        fun()
        fun1()
        If i <> 1 Then
            System.Console.WriteLine("Exit statement not working properly ") : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
