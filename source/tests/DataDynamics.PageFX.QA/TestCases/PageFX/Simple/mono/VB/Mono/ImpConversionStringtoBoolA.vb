'Author:
'   V. Sudharsan (vsudharsan@novell.com)
'
' (C) 2005 Novell, Inc.
Option Strict Off

Module ImpConversionStringtoBooleanA
	Function _Main() As Integer
        Dim a As Boolean
        Dim b As String = "True"
        a = b
        If a <> True Then
            System.Console.WriteLine("Conversion of String to Boolean not working. Expected True but got " & a) : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
