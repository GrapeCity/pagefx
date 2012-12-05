'Author:
'   V. Sudharsan (vsudharsan@novell.com)
'
' (C) 2005 Novell, Inc.
Option Strict Off

Module ImpConversionSingletoDecimalA
	Function _Main() As Integer
        Dim a As Decimal
        Dim b As Single = -4.94065645841247E-100
        a = b
        If a <> -0 Then
            System.Console.WriteLine("Single to Decimal Conversion is not working properly. Expected 0 but got " & a) : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
