'Author:
'   V. Sudharsan (vsudharsan@novell.com)
'
' (C) 2005 Novell, Inc.
Option Strict Off

Module Test
	Function _Main() As Integer
        Dim i
        For i = 9 To 0 Step 1
        Next
        If i <> 9 Then
            System.Console.WriteLine("For loop not working. Expected 9 but got " & i) : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
