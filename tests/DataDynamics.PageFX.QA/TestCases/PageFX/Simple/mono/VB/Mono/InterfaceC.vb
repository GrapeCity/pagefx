Option Strict Off

Interface I
    Function F()
End Interface


Interface I1
    Function S()
End Interface


MustInherit Class C1
    Implements I

    Function F() Implements I.F
    End Function
End Class

MustInherit Class C2
    Implements I

    MustOverride Function F() Implements I.F
End Class


MustInherit Class C3
    Implements I1

    MustOverride Function S() Implements I1.S
End Class


Class DC1
    Inherits C1
End Class

Class DC2
    Inherits C2

    Overrides Function F()
    End Function
End Class


Class DC3
    Inherits C3

    Overrides Function S()
    End Function
End Class


Module InterfaceC
	Function _Main() As Integer
        Dim x As DC1 = New DC1()
        x.F()

        Dim y As DC2 = New DC2()
        y.F()


        Dim z As DC3 = New DC3()
        z.S()
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module

