'Author:
'   V. Sudharsan (vsudharsan@novell.com)
'
' (C) 2005 Novell, Inc.

'Expected to call single 
Option Strict Off

Module ImpConversionDecimaltoSingle
    Function fun(ByVal i As Single)
        Return 1
    End Function
    Function fun(ByVal i As Double)
        Return 2
    End Function
	Function _Main() As Integer
        Dim j As Integer
        Dim i As Decimal = 10
        j = fun(i)
        If j <> 1 Then
            System.Console.WriteLine("Implicit Conversion not working. Expected 1 but got " & j) : Return 1
        End If

    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
