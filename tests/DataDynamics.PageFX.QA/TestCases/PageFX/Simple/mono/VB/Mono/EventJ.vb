'Author:
'   V. Sudharsan (vsudharsan@novell.com)
'
' (C) 2005 Novell, Inc.

Imports System

Class C
    Delegate Sub EH()
    Public Event E As EH

    Public Function S() As Object
        RaiseEvent E()
    End Function
End Class

Class C1
    Dim x As C = New C()

    Function setHandler() As Object
        ' AddHandler x.E, AddressOf xh - Note: AddHandler Commented
    End Function

    Function unsetHandler() As Object
        RemoveHandler x.E, AddressOf xh
    End Function

    Function call_S() As Object
        x.S()
    End Function

    Sub xh()
        Console.WriteLine("event called")
    End Sub
End Class

Module M
	Function _Main() As Integer
        Dim y As New C1
        y.setHandler()
        y.call_S()
        y.unsetHandler()
        y.call_S()
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub


End Module
