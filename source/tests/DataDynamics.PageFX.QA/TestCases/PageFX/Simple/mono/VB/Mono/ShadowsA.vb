Option Strict Off
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

    Shadows Function F1(ByVal i As Integer)
    End Function
End Class

Module ShadowA
	Function _Main() As Integer
        Dim x As D = New D()

        x.F()
        x.F(10)
        x.F1(20)
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub


End Module
