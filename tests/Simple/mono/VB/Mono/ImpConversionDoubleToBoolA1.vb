Option Strict Off
Module ImpConversionofDoubletoBool
	Function _Main() As Integer
        Dim a As Double = 0
        Dim b As Boolean = a
        If b <> False Then
            System.Console.WriteLine("Implicit Conversion of Double to Bool(Fals):return 1 has Failed. Expected False, but got " & b)
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
