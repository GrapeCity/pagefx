Imports SYSTEM
Module M
    Private i As Integer

    Public Property p() As Integer
        Get
            Return i
        End Get

        Set(ByVal value As Integer)
            i = Value
        End Set

    End Property

	Function _Main() As Integer
        p = 10
        If p <> 10 Then
            System.Console.WriteLine("#A1 Property Not Working") : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub


End Module
