Option Strict Off
Module ImpConversionofBooltoShort
	Function _Main() As Integer
        Dim b As Boolean = True
        Dim a As Short = b
        If a <> -1 Then
            System.Console.WriteLine("Implicit Conversion of Bool(Tru):return 1 to Short has Failed. Expected -1 got " & a)
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
