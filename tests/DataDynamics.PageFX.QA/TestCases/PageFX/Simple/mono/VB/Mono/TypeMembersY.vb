' Author:
'   Maverson Eduardo Schulze Rosa (maverson@gmail.com)
'
' GrupoTIC - UFPR - Federal University of Paraná
Option Strict Off

Imports System

Module M
    Class C
        Public b As Byte
    End Class

	Function _Main() As Integer
        Dim o As Object = New C
        o.b = 0

        If o.b <> 0 Then
            System.Console.WriteLine("LateBinding Not Working") : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
