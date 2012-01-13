'Author:
'   V. Sudharsan (vsudharsan@novell.com)
'
' (C) 2005 Novell, Inc.

Module ExpConversionDecimaltoIntA
	Function _Main() As Integer
        Dim i As Boolean = False
        Try
            Dim a As Integer
            Dim b As Decimal = 3000000000
            a = CInt(b)
        Catch e As System.Exception
            System.Console.WriteLine(" Arithmetic operation resulted in an overflow.")
            i = True
        End Try
        If i = False Then
            System.Console.WriteLine("Decimal to Integer Conversion is not working properly.")
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
