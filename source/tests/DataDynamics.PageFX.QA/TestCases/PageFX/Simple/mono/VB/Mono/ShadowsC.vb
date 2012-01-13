Option Strict Off
Interface ILeft
    Function F()
End Interface

Interface IRight
    Function F()
End Interface

Interface ILeftRight
    Inherits ILeft, IRight
    Shadows Function F()
End Interface


Module ShadowsC
	Function _Main() As Integer
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
