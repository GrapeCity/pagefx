'Author:
'   V. Sudharsan (vsudharsan@novell.com)
'
' (C) 2005 Novell, Inc.

Module ExpConversionBytetoSingleA
	Function _Main() As Integer
        Dim a As Byte = 123
        Dim b As Single
        b = CSng(a)
        If b <> 123 Then
            System.Console.WriteLine("Byte to Single Conversion is not working properly. Expected 123 but got " & b) : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
