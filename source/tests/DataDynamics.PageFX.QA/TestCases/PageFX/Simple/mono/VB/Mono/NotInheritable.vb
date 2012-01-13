NotInheritable Class C1
End Class

Module Module1
	Function _Main() As Integer
        Dim x As New C1()
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
