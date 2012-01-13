'Author:
'   V. Sudharsan (vsudharsan@novell.com)
'
' (C) 2005 Novell, Inc.

Module ExpConversionShorttoLongA
	Function _Main() As Integer
        Dim a As Short = 123
        Dim b As Long
        b = CLng(a)
        If b <> 123 Then
            System.Console.WriteLine("Short to Long Conversion is not working properly. Expected 123 but got " & b) : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
