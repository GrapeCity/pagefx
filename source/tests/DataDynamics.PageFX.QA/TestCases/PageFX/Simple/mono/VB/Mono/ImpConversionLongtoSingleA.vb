'Author:
'   V. Sudharsan (vsudharsan@novell.com)
'
' (C) 2005 Novell, Inc.

Module ImpConversionLongtoSingleA
	Function _Main() As Integer
        Dim a As Long = 124
        Dim b As Single = a
        If b <> 124 Then
            System.Console.WriteLine("Implicit Conversion of Long to Single has Failed. Expected 124 but got " & b) : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
