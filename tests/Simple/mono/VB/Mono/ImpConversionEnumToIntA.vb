'Author: Ritvik Mayank <mritvik@novell.com>
'Copyright (C) 2005 Novell, Inc (http://www.novell.com)

Imports System
Module EnumToInt

    Enum Days
        Sunday
        Monday
        Tuesday
        Wednesday
        Thursday
        Friday
        Saturday
    End Enum

	Function _Main() As Integer
        Dim i As Integer = 4
        If Days.Thursday <> i Then
            Throw New System.Exception("#Can not Convert from Enum to Integer")
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
