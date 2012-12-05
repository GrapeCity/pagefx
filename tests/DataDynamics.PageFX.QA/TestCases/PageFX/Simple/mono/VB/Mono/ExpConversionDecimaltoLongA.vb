'Author:
'   V. Sudharsan (vsudharsan@novell.com)
'
' (C) 2005 Novell, Inc.
Option Strict Off
Module ExpConversionDecimaltoLongA
	Function _Main() As Integer
        Dim a As Decimal = 123.501
        Dim b As Long = CLng(a)
        If b <> 124 Then
            System.Console.WriteLine("Explicit Conversion of Long to Single has Failed. Expected 123.5 but got " & b) : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
