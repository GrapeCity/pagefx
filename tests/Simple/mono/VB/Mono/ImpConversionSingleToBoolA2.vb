Option Strict Off
Module ImpConversionofSingletoBool
	Function _Main() As Integer
        Dim a As Single = 123
        Dim b As Boolean = a
        If b <> True Then
            System.Console.WriteLine("Implicit Conversion of Single to Bool(Tru):return 1 has Failed. Expected True got " + b)
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
