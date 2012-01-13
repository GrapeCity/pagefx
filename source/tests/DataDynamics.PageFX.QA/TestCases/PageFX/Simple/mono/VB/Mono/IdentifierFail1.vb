Module IdentifierFail1
    ' not a valid identifier as per spec
    ' but accepted by .net 1.0 
    Function __() As Object
    End Function

	Function _Main() As Integer
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
