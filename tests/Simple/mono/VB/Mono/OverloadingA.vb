Option Strict Off
'Testing overloading in different classes
Class B
    Function F()
    End Function

    Function F(ByVal i As Integer)
    End Function

    Function F1()
    End Function

    Function F1(ByVal i As Integer)
    End Function
End Class

Class D
    Inherits B

    Overloads Function F()
    End Function
End Class

Module OverloadingA
	Function _Main() As Integer
        Dim x As Object = New D()

        x.F()
        x.F(10)
        x.F1(20)

        Dim x1 As Object = New B()
        x1.F(20)
        x1.F1()
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub


End Module
