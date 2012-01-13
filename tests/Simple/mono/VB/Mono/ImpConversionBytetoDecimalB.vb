'Author:
'   V. Sudharsan (vsudharsan@novell.com)
'
' (C) 2005 Novell, Inc.
Option Strict Off

Module ImpConversionBytetoDecimalC
	Function _Main() As Integer
        Dim a As Byte = 111
        Dim b As Decimal = 111.9 + a
        If b <> 222.9 Then
            System.Console.WriteLine("Addition of Byte & Decimal not working. Expected 222.9 but got " & b) : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module


