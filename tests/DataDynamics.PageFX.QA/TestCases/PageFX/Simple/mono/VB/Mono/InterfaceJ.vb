'Author:
'   V. Sudharsan (vsudharsan@novell.com)
'
' (C) 2005 Novell, Inc.

'Tries to declare vaiables of a Interface using the Class which implements it...

Interface A
    Function fun() As Object
End Interface

Class C
    Implements A
    Function Cfun() As Object Implements A.fun
    End Function
End Class

Module InterfaceI
	Function _Main() As Integer
        Dim a As A = New C
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
