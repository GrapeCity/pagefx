Option Strict Off
Class B
    Private Function F()
    End Function
End Class

Class D
    Inherits B

    Shadows Function F()
    End Function
End Class

Module ShadowsC3
	Function _Main() As Integer
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
