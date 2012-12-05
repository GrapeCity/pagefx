'Author:
'   V. Sudharsan (vsudharsan@novell.com)
'
' (C) 2005 Novell, Inc.
Option Strict Off

Module M
	Function _Main() As Integer
        Dim count As Integer
        Dim ErrMsg As String = ""
        For i As Long = 1 To 10.49 Step 0.51
            count = count + 1
        Next i
        If count <> 10 Then
            ErrMsg = "#A1 For not working"
        End If

        count = 0
        For i As Byte = 1 To 10.49 Step 0.51
            count = count + 1
        Next i
        If count <> 10 Then
            ErrMsg = ErrMsg & vbCrLf & "#A2 For not working"
        End If

        count = 0
        For i As Short = 1 To 10.49 Step 0.51
            count = count + 1
        Next i
        If count <> 10 Then
            ErrMsg = ErrMsg & vbCrLf & "#A3 For not working"
        End If
        If (ErrMsg <> "") Then
            Throw New System.Exception(ErrMsg)
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
