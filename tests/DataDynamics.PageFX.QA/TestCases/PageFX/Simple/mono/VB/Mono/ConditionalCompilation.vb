Imports System
Module ConditionalCompilation
	Function _Main() As Integer
        Try
            'Using syntatically wrong statements inside a #If block that does not satisfy condition
#If False Then
				Console.WriteLine("Hello)
#End If
        Catch e As Exception
            Console.WriteLine(e.Message)
        End Try
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module



