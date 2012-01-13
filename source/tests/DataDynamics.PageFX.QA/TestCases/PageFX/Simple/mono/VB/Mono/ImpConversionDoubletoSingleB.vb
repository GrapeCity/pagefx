'Author:
'   V. Sudharsan (vsudharsan@novell.com)
'
' (C) 2005 Novell, Inc.
Option Strict Off
Module ImpConversionDoubletoSingleB
	Function _Main() As Integer
        Dim a As Single
        Dim b As Double = 4.94065645841247E+300
        a = b
        If a <> Single.PositiveInfinity Then
            System.Console.WriteLine("Double to Single Conversion is not working properly. Expected Infinity but got " & a) : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
