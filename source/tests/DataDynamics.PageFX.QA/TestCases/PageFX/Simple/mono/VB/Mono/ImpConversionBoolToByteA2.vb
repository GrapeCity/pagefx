Option Strict Off

Module ImpConversionofBooltoByte
	Function _Main() As Integer
        Dim b As Boolean = True
        Dim a As Byte = b
        If a <> 255 Then
            System.Console.WriteLine("Implicit Conversion of Bool(Tru):return 1 to Byte has Failed. Expected 255 got " & a)
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
