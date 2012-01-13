Imports System

Module IntegerLiteralTest
	Function _Main() As Integer
        Dim i As Integer
        Dim l As Long
        Dim s As Short

        l = 20L
        s = 20S
        i = 20I
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
