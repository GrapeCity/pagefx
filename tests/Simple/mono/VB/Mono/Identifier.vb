Imports System

Module Identifier
    Function _abc() As Object
    End Function

    Function xys2() As Object
    End Function

    '' escaped identifier
    Sub [sub]()
    End Sub

    Dim [function] As Int16

	Function _Main() As Integer
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
