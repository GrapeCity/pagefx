'============================================================================================
'Name:Manish Kumar Sinha 
'Email Address: manishkumarsinha@sify.com
'Test Case Name: ParamArray:
'APR-1.0.0: ParamArray can be used only on the last argument of argument list. it allows us to 'pass an arbitrary list. It allows us to pass an arbitrary number of argument to the procedure 
'=============================================================================================
Option Strict Off
Imports System
Module PA_1_0_0
    Function F(ByVal ParamArray args() As Integer)
        Dim a As Integer
        a = args.Length
        If a = 0 Then
            System.Console.WriteLine("#A1, Unexcepted behavoiur of PARAM ARRAY") : Return 1
        End If
    End Function
	Function _Main() As Integer
        Dim a As Integer() = {1, 2, 3}
        F(a)
        F(10, 20, 30, 40)
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
'=============================================================================================
