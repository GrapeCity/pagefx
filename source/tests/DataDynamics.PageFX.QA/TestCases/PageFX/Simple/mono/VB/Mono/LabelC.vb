' Positive Test
' Test labels in functions
Imports System

Module labelC


    Function Abs(ByVal x As Integer) As Integer

        If x >= 0 Then
            GoTo x
        End If

        x = -x

x:
y:
z:
        Return x

    End Function


	Function _Main() As Integer
        Dim x As Integer, y As Integer
        x = Abs(-1)
        y = Abs(1)

        If x <> 1 Then
            System.Console.WriteLine("#Lbl1") : Return 1
        End If
        If y <> 1 Then
            System.Console.WriteLine("#Lbl2") : Return 1
        End If


    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub



End Module
