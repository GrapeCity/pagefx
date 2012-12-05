Option Strict Off
Module ImpConversionofBooltoDoubleB
	Function _Main() As Integer
        Dim b As Boolean = True
        Dim a As Double = b
        If a <> -1 Then
            System.Console.WriteLine("Implicit Conversion of Bool(Tru):return 1 to Double has Failed. Expected -1 got " & a)
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
