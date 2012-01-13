Option Strict Off
' Same function having both Overrides and Implements simultanously

Interface IBase
    Function F(ByVal i As Integer)
End Interface

MustInherit Class Base
    MustOverride Function F(ByVal i As Integer)
End Class


Class D
    Inherits Base
    Implements IBase

    Overrides Function F(ByVal i As Integer) Implements IBase.F
    End Function
End Class

Module InheritanceJ
	Function _Main() As Integer
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
