Imports System
Imports Microsoft.VisualBasic

Module VariableC
    Dim a() As Integer = {1, 2, 3, 4, 5}

	Function _Main() As Integer
        Dim c As Integer

        c = UBound(a, 1)
        c = LBound(a, 1)
        'c = UBound(a)
        'c = LBound(a)

    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
