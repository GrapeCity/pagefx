'Author:
'   V. Sudharsan (vsudharsan@novell.com)
'
' (C) 2005 Novell, Inc.

Module ImpConversionSingletoDoubleA
	Function _Main() As Integer
        Dim a As Single = 123.5
        Dim b As Double
        b = a
        If b <> 123.5 Then
            System.Console.WriteLine("Single to Double Conversion is not working properly. Expected 123.5 but got " & b) : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
