'Author:
'   V. Sudharsan (vsudharsan@novell.com)
'
' (C) 2005 Novell, Inc.
Option Strict Off
Module Test
    Private b As Byte = 0
    Private i As Integer = 0
	Function _Main() As Integer
        b += 1
        b += i
        b += CByte(i)
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
