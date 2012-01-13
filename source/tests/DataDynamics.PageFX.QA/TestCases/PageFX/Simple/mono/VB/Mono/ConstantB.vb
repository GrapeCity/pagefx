Class A
    Public Const c As Integer = 10
End Class

Class B
    Inherits A

    Public Shadows Const c As Integer = 20
End Class

Module M
	Function _Main() As Integer
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module

