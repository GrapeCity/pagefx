'Author:
'   V. Sudharsan (vsudharsan@novell.com)
'
' (C) 2005 Novell, Inc.

Module ExpConversionBytetoStringA
	Function _Main() As Integer
        Dim a As Byte = 123
        Dim b As String = a.toString()
        If b <> "123" Then
            System.Console.WriteLine("Conversion of Byte to String not working. Expected 123 but got " & b) : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
