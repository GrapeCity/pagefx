'Author:
'   V. Sudharsan (vsudharsan@novell.com)
'
' (C) 2005 Novell, Inc.

Module ImpConversionChartoStringA
	Function _Main() As Integer
        Dim a As Char = "T"c
        Dim b As String = a
        If b <> "T" Then
            System.Console.WriteLine("Conversion of Char to String not working. Expected T but got " & b) : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
