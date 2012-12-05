'Author:
'   V. Sudharsan (vsudharsan@novell.com)
'
' (C) 2005 Novell, Inc.

Imports System

Class A
    ' Has implicit Constructor defined
End Class

Class AB
    Inherits A
    Sub New()
    End Sub
End Class

Module Test
	Function _Main() As Integer
        Dim a As AB = New AB()
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module

