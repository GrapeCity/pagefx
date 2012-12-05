'Author: Ritvik Mayank <mritvik@novell.com>
'Copyright (C) 2005 Novell, Inc (http://www.novell.com)
Option Strict Off
Imports System
Module EnumToInt

    Private Enum LongEnum As Long
        MyEnumLong1 = 125.5678
        MyEnumLong2 = 567.125
    End Enum

	Function _Main() As Integer
        Dim i As Integer
        Dim j As Integer = 125
        i = CInt(LongEnum.MyEnumLong1)
        If LongEnum.MyEnumLong1 < j Then
            System.Console.WriteLine("#Can not Convert Explicitly from Enum to Integer") : Return 1
        End If
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
