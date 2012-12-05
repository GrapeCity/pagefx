Option Strict Off
Module CharacterLiterals
	Function _Main() As Integer
        Dim c As Char
        c = "x"

        c = "X"

        Dim a As String = "X"c
        If a <> c Then
            System.Console.WriteLine("a is not same as c") : Return 1
        End If

        'the outcome should be "x"
        c = """x"""
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub


End Module
