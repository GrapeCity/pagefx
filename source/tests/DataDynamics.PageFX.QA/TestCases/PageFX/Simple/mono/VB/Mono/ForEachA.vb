Imports System

Module M
	Function _Main() As Integer
        Dim a As Integer() = {1, 2, 3}
        Dim b As Integer
        Dim c As Integer = 0
        For Each b In a
            c = c + b
            c = c + 1
        Next
        If c <> 9 Then
            System.Console.WriteLine("#A1: count is wrong") : Return 1
        End If
        c = 0
        For Each b In a
            c = c + b
            c = c + 1
            Dim d As Integer
            For Each d In a
                c = c + d
            Next
        Next
        If c <> 27 Then
            System.Console.WriteLine("#A2: count is wrong") : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module

