Option Strict Off
Interface ILeft
    Function F()
End Interface

Interface IRight
    Function F()
End Interface

Interface ILeftRight
    Inherits ILeft, IRight
End Interface

Class LeftRight
    Implements ILeftRight

    Function LeftF() Implements ILeft.F
    End Function

    Function RightF() Implements IRight.F
    End Function
End Class

Module InterfaceB
	Function _Main() As Integer
        Dim lr As New LeftRight()
        lr.LeftF()
        lr.RightF()
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
