Option Strict Off
Module ImpConversionofBooltoLong
	Function _Main() As Integer
        Dim b As Boolean = False
        Dim a As Long = b
        If a <> 0 Then
            System.Console.WriteLine("Implicit Conversion of Bool(Fals):return 1 to Long has Failed. Expected 0, but got " & a)
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
