'Author:
'   V. Sudharsan (vsudharsan@novell.com)
'
' (C) 2005 Novell, Inc.
Option Strict Off

Module ImpConversionSingletoIntegerC
	Function _Main() As Integer
        Dim a As Single = 111
        Dim b As Integer = 111 + a
        If b <> 222 Then
            System.Console.WriteLine("Addition of Single & Integer not working. Expected 222 but got " & b) : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
