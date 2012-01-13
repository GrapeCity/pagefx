'Author: Ritvik Mayank <mritvik@novell.com>
'Copyright (C) 2005 Novell, Inc (http://www.novell.com)

Imports System
Module TypeValRef

    Public Class ValueReference
        Public Age As Short
    End Class

    Structure MyStruct
        Public Age As Short
    End Structure

	Function _Main() As Integer
        Dim objRef1 As ValueReference
        Dim objRef2 As ValueReference

        Dim objValue1 As MyStruct
        Dim objValue2 As MyStruct

        objRef1 = New ValueReference()
        objRef1.Age = 20
        objRef2 = objRef1
        objRef2.Age = 30

        If ((objRef1.Age <> objRef2.Age) Or (objRef1.Age <> 30)) Then
            Throw New Exception("Unexpected behavior objRef1.Age and objRef2.Age should return the same value Expected 30 but got = " & objRef2.Age)
        End If

        objValue1 = New MyStruct()
        objValue1.Age = 20
        objValue2 = objValue1
        objValue2.Age = 30
        If (objValue1.Age <> 20) Or (objValue2.Age <> 30) Then
            Throw New Exception("Unexpected behavior. Expected objValue1.Age = 20 and objValue2.Age = 30 but got  objValue1.Age = " & objValue1.Age & " objValue2.Age = " & objValue2.Age)
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
