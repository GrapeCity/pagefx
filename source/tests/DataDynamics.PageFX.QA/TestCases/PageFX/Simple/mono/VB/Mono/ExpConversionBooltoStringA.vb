'Author:
'   V. Sudharsan (vsudharsan@novell.com)
'
' (C) 2005 Novell, Inc.

Module ExpConversionBooleantoStringA
	Function _Main() As Integer
        Dim a As Boolean = True
        Dim b As String = a.toString()
        If b <> "True" Then
            System.Console.WriteLine("Conversion of Boolean to String not working. Expected True but got " & b) : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
