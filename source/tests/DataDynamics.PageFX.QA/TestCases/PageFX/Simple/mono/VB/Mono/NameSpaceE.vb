'Author:
'   V. Sudharsan (vsudharsan@novell.com)
'
' (C) 2005 Novell, Inc.

'VB.Net Accepts recognizes even keywords as qualifiers as long as they follow a period as given below. 

Namespace A.Default.If
    Class C
    End Class
End Namespace

Namespace B.Class.Integer
    Class D
    End Class
End Namespace

Module NamespaceA
	Function _Main() As Integer
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module

