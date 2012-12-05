Option Strict Off
Module ImpConversionofDecimaltoBool
	Function _Main() As Integer
        Dim a As Decimal = 0
        Dim b As Boolean = a
        If b <> False Then
            System.Console.WriteLine("Implicit Conversion of Decimal to Bool(Fals):return 1 has Failed. Expected False, but got " & b)
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
