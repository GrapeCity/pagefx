'Author:
'   V. Sudharsan (vsudharsan@novell.com)
'
' (C) 2005 Novell, Inc.
Option Strict Off

Module ImpConversionDecimaltoDoubleA
	Function _Main() As Integer
        Dim a As Decimal = 123.5
        Dim b As Double = a
        If b <> 123.5 Then
            System.Console.WriteLine("Implicit Conversion of Long to Double has Failed. Expected 123.5 but got " & b) : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
