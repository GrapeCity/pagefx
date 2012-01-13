Option Strict Off
'Author:
'   V. Sudharsan (vsudharsan@novell.com)
'
' (C) 2005 Novell, Inc.

Module ImpConversionofLongtoIntA
	Function _Main() As Integer
        Dim a As Long = 123
        Dim b As Integer = a
        If b <> 123 Then
            System.Console.WriteLine("Implicit Conversion of Long to Int has Failed") : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
