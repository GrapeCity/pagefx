'Author:
'   V. Sudharsan (vsudharsan@novell.com)
'
' (C) 2005 Novell, Inc.

'Simple Check if Interfaces within interfaces are allowed
Interface A
    Interface AB
        Function fun() As Object
    End Interface
End Interface

Class C
    Implements A.AB
    Function Cfun() As Object Implements A.AB.fun
    End Function
End Class

Module InterfaceI
	Function _Main() As Integer
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module

