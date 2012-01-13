'=============================================================================================
'Name:Manish Kumar Sinha 
'Email Address: manishkumarsinha@sify.com
'Test Case Name: Argument passing by Optional Keyword:
'O.P-1.0.0: An Optional parameter must specify a constant expression to be used a replacement
'		value if no argument is specified.
'=============================================================================================

Imports System
Module OP1_0_0
    Function F(ByVal telephoneNo As Long, Optional ByVal code As Integer = 80, Optional ByVal code1 As Integer = 91) As Boolean
        If (code <> 80 And code1 <> 91) Then
            Return False
        Else
            Return True
        End If
    End Function

	Function _Main() As Integer
        Dim telephoneNo As Long = 9886066432
        Dim status As Boolean
        status = F(telephoneNo)
        If (status = False) Then
            System.Console.WriteLine("#A1, Unexcepted behaviour in string of OP1_0_1") : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub


End Module
