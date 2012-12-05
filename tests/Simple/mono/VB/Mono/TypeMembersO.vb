'Author:
'   V. Sudharsan (vsudharsan@novell.com)
'
' (C) 2005 Novell, Inc.
Option Strict Off

Imports System

Class base
    Default Public Property Item(ByVal i As Integer) As Integer
        Get
            Return i
        End Get
        Set(ByVal value As Integer)
        End Set
    End Property
End Class

Module DefaultA
	Function _Main() As Integer
        Dim a As Object = New base()
        Dim i As Integer
        i = a(10)
        If i <> 10 Then
            System.Console.WriteLine("Binding Not Working") : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
