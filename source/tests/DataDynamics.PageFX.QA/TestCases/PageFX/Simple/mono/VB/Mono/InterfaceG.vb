Option Strict Off
Imports System

Interface I
    Function F1()
    Function F2()
End Interface

Class C1
    Implements I
    Public Function F() Implements I.F1, I.F2
    End Function
End Class

Module InterfaceG
	Function _Main() As Integer
        Dim C As New C1()
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
