Option Strict Off
Imports System
Module IntegerLiteral
	Function _Main() As Integer
        'Assigning boolean to integer
        Dim a As Integer
        a = True
        If a <> -1 Then
            System.Console.WriteLine("IntegerLiteralA:Failed") : Return 1
        End If
        ' Assigning float to integer
        ' if option strict is off this 
        ' Test case should pass
        a = 1.23
        If a <> 1 Then
            System.Console.WriteLine("IntegerLiteralA:Failed") : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
