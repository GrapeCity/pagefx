'Author:
'   V. Sudharsan (vsudharsan@novell.com)
'
' (C) 2005 Novell, Inc.
Option Strict Off

Module ImpConversionStringtoIntD
	Function _Main() As Integer
        Dim a As String = "12"
        Dim b As Integer = a + 123
        If b <> 135 Then
            System.Console.WriteLine("Concat of String & Int not working. Expected  135 but got " & b) : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module

