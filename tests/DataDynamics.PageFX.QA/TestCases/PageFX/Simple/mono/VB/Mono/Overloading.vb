Option Strict Off
Class c
    Function f()
    End Function

    Function f(ByVal i As Integer)
    End Function

    Function f(ByVal s As String)
    End Function

    Function f(ByVal i1 As Integer, ByVal i2 As Integer)
    End Function
End Class


Module Overloading
    Function s()
    End Function

	Function _Main() As Integer
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub


End Module
