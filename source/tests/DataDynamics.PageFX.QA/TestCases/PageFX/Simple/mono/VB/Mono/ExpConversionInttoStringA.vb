'Author:
'   V. Sudharsan (vsudharsan@novell.com)
'
' (C) 2005 Novell, Inc.

Module ExpConversionIntegertoStringA
	Function _Main() As Integer
        Dim a As Integer = 1234
        Dim b As String = a.toString()
        If b <> "1234" Then
            System.Console.WriteLine("Conversion of Integer to String not working. Expected 1234 but got " & b) : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
