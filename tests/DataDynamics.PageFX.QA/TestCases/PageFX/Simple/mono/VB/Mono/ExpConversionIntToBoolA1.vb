Module ExpConversionInttoBool
	Function _Main() As Integer
        Dim a As Integer = -4
        Dim b As Boolean
        b = CBool(a)
        If b <> True Then
            System.Console.WriteLine("Int to Boolean Conversion is not working properly. Expected True but got " & b) : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
