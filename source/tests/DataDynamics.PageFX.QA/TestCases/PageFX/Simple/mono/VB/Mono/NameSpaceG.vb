Option Strict Off
'Author:
'   V. Sudharsan (vsudharsan@novell.com)
'
' (C) 2005 Novell, Inc.

'Checking For unqualifed name getting accessed

Imports A

Namespace A
    Public Module B
        Function C()
            Return 10
        End Function
    End Module
End Namespace

Module NamespaceA
	Function _Main() As Integer
        If C() <> 10 Then
            System.Console.WriteLine("Not Working") : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module

