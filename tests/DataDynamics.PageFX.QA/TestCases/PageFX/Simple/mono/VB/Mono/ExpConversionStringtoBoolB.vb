'Author:
'   V. Sudharsan (vsudharsan@novell.com)
'
' (C) 2005 Novell, Inc.

Module ExpConversionStringtoBooleanA
	Function _Main() As Integer
        Dim a As Boolean
        Dim b As String = "False"
        a = CBool(b)
        If a <> False Then
            System.Console.WriteLine("Conversion of String to Boolean not working. Expected False but got " & a) : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
