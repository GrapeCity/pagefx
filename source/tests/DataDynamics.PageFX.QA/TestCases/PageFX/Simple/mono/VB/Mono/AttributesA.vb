Imports System
'Ommiting AttributeUsage attribute

Public Class AuthorAttribute
    Inherits Attribute
    Public Name As Object
    Public Sub New(ByVal Name As String)
        Me.Name = Name
    End Sub
    Public ReadOnly Property NameP() As String
        Get
            Return CStr(Name)
        End Get
    End Property
End Class


<Author("Robin Cook")> _
Public Class C1
    <Author("John")> _
    Public Function S1() As Object
    End Function

End Class

Module Test
	Function _Main() As Integer

    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
