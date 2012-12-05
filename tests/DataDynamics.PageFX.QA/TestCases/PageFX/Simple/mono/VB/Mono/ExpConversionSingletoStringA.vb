'Author:
'   V. Sudharsan (vsudharsan@novell.com)
'
' (C) 2005 Novell, Inc.

Imports System.Threading
Imports System.Globalization

Module ExpConversionSingletoStringA
	Function _Main() As Integer
        Thread.CurrentThread.CurrentCulture = New CultureInfo("en-US")

        Dim a As Single = 123.052
        Dim b As String = a.toString()
        If b <> "123.052" Then
            System.Console.WriteLine("Conversion of Single to String not working. Expected 123.052 but got " & b) : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
