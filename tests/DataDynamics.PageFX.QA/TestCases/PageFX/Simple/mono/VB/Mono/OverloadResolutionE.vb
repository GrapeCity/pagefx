'Author:
'   V. Sudharsan (vsudharsan@novell.com)
'
' (C) 2005 Novell, Inc.
Option Strict Off
Imports System

Class Base
End Class

Class Derived
    Inherits Base
End Class

Class Derived1
    Inherits Derived
End Class

Module Test
    Public i As Integer
    Function F(ByVal b As Base)
        i = 10
    End Function

    Function F(ByVal d As Object)
        i = 20
    End Function

	Function _Main() As Integer
        Dim b As Base = New Derived1()
        Dim o As Object = b

        F(b)
        If i <> 10 Then
            System.Console.WriteLine("#A1 Latebinding Not working") : Return 1
        End If
        F(o)
        If i <> 20 Then
            System.Console.WriteLine("#A2 Latebinding Not working") : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
