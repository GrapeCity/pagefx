Option Strict Off
'Author:
'   V. Sudharsan (vsudharsan@novell.com)
'
' (C) 2005 Novell, Inc.

Module ImpConversionLongtoStringC
	Function _Main() As Integer
        Dim a As Long = 123
        Dim b As String = a + "123"
        If b <> "246" Then
            System.Console.WriteLine("Concat of Long & String not working. Expected 246 but got " & b) : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module


