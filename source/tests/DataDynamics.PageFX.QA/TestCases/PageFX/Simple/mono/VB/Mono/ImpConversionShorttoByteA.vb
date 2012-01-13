Option Strict Off
'Author:
'   V. Sudharsan (vsudharsan@novell.com)
'
' (C) 2005 Novell, Inc.

Module ImpConversionShorttoByteA
	Function _Main() As Integer
        Dim a As Byte
        Dim b As Short = 123
        a = b
        If a <> 123 Then
            System.Console.WriteLine("Byte to Short Conversion is not working properly. Expected 123 but got " & a) : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
