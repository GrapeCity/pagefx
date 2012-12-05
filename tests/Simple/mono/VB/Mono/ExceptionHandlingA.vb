Option Strict Off
Imports System

Module ExceptionHandlingA

	Function _Main() As Integer

        ' Finally block is executed regardless of how execution 
        ' leaves the Try statement

        ' Case 1: through the end of Try block
        Dim i As Integer = 0
        Try
            i = i + 1
        Finally
            i = i + 2
        End Try

        If i <> 3 Then
            System.Console.WriteLine("#EHA1 - Finally block not executed") : Return 1
        End If

        ' Case 2: through the end of Catch block
        Try
            i = i / 0
        Catch e As Exception
            i = i * 2
        Finally
            i = i * 3 / 2
        End Try

        If i <> 9 Then
            System.Console.WriteLine("#EHA2 - Finally block not executed") : Return 1
        End If

        ' Case 3: through an Exit Try statement
        Try
            i = i / 9 * 2
            Exit Try
        Catch e As Exception
            Console.WriteLine(e.Message)
        Finally
            i = i / 2
        End Try

        If i <> 1 Then
            System.Console.WriteLine("#EHA3 - Finally block not executed") : Return 1
        End If

        ' Case 4: through a GoTo statement
        Try
            i = i - 1
            GoTo label
        Catch e As Exception
            Console.WriteLine(e.Message)
        Finally
            i = i + 1
        End Try
label:
        If i <> 1 Then
            System.Console.WriteLine("#EHA4 - Finally block not executed") : Return 1
        End If

        ' Case 5: by not handling a thrown exception
        Try
            Try
                i = 5
                Throw New Exception("EE") : Return 1
            Finally
                i = i * 5
            End Try
        Catch e As Exception
            i = i * 2
        End Try

        If i <> 50 Then
            System.Console.WriteLine("#EHA5 - Finally block not exceuted") : Return 1
        End If

    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub


End Module
