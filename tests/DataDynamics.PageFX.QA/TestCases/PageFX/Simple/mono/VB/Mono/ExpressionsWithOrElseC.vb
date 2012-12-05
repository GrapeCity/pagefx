'Author: Ritvik Mayank <mritvik@novell.com>
'Copyright (C) 2005 Novell, Inc (http://www.novell.com)
'this is when first term is True

Imports System
Imports Microsoft.VisualBasic
Module Test

    Function f1(ByVal i As Integer, ByVal j As Integer, ByVal k As Integer) As Boolean
        Dim E As Boolean = (k - i) > (j - i)
        Return E
    End Function

    Function f2(ByVal l As Integer, ByVal m As Integer) As Boolean
        Dim F As Boolean = l < m
        If l = 10 Then
            Throw New Exception("Second Term is not supposed to be evaluated as the First Term is True")
        End If
        Return F
    End Function

	Function _Main() As Integer
        Dim MyCheck As Boolean
        MyCheck = f1(10, 20, 30) OrElse f2(10, 20)
        If Mycheck = False Then
            Throw New Exception(" Unexpected Behavior of the Expression. OrElse should return True here ")
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
