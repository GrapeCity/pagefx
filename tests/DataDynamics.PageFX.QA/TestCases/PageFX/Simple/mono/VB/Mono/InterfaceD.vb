Option Strict Off
Interface I
    Function F1(ByVal i As Integer)
    Function F2(ByVal i As Integer)
End Interface

Class C
    Implements I

    Function F(ByVal i As Integer) Implements I.F1, I.F2
    End Function
End Class

Module InterfaceD
	Function _Main() As Integer
        Dim x As C = New C()
        x.F(10)
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
