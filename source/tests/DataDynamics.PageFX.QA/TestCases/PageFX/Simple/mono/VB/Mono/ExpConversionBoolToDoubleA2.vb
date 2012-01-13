Module ExpConversionofBoolToDouble
	Function _Main() As Integer
        Dim a As Boolean = True
        Dim b As Double = CDbl(a)
        If b <> -1 Then
            System.Console.WriteLine("Explicit Conversion of Bool(Tru):return 1 to Double has Failed. Expected -1, but got " & b) : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
