'Author:
'   V. Sudharsan (vsudharsan@novell.com)
'
' (C) 2005 Novell, Inc.
Option Strict Off

Module ImpConversionDecimaltoInt
    Function fun(ByVal i As Integer)
        If i <> 10 Then
            System.Console.WriteLine("Implicit Conversion of Decimal to Int not working. Expected 10 but got " & i) : Return 1
        End If
    End Function
	Function _Main() As Integer
        Dim i As Decimal = 10
        fun(i)

    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
