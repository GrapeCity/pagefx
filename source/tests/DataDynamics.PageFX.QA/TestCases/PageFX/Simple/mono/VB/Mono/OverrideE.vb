Option Strict Off
Imports System

Class base
    Public Overridable Property Item(ByVal i As Integer) As Integer
        Get
            Return i
        End Get
        Set(ByVal value As Integer)
        End Set
    End Property
End Class

Class derive
    Inherits base
    Public Overrides Property Item(ByVal i As Integer) As Integer
        Get
            Return 2 * i
        End Get
        Set(ByVal value As Integer)
        End Set
    End Property
End Class

Module DefaultA
	Function _Main() As Integer
        Dim a As Object = New derive()
        Dim i As Integer
        i = a.Item(10)
        If i <> 20 Then
            System.Console.WriteLine("Default Not Working") : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
