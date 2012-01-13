Option Strict Off
Module ImpConversionofInttoBool
	Function _Main() As Integer
        Dim a As Integer = 0
        Dim b As Boolean = a
        If b <> False Then
            System.Console.WriteLine("Implicit Conversion of Int to Bool(Fals):return 1 has Failed. Expected False, but got " & b) : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
