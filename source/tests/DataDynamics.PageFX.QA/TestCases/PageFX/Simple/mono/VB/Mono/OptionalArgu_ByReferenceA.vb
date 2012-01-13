Option Strict Off
'=============================================================================================
'Name:Manish Kumar Sinha 
'Email Address: manishkumarsinha@sify.com
'Test Case Name: Argument passing by Optional Keyword:
'O.P-1.0.1: An Optional parameter must specify a constant expression to be used a replacement
'		value if no argument is specified.
'=============================================================================================

Imports System
Module OP1_0_1
    Function F(ByVal telephoneNo As Long, Optional ByRef code As Integer = 80)
        If (code = 80) Then
            System.Console.WriteLine("#A1, Unexcepted behaviour in string of OP1_0_1") : Return 1
        End If
    End Function

	Function _Main() As Integer
        Dim telephoneNo As Long = 9886066432
        Dim code As Integer = 81
        F(telephoneNo, code)
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub


End Module
