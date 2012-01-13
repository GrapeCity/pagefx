Option Strict Off
Interface IBase
    Function F(ByVal i As Integer)
End Interface

Interface ILeft
    Inherits IBase
End Interface

Interface IRight
    Inherits IBase
End Interface

Interface IDerived
    Inherits ILeft, IRight
End Interface

Class D
    Implements IDerived

    Function F(ByVal i As Integer) Implements IDerived.F
    End Function
End Class

Module InheritanceD
	Function _Main() As Integer
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
