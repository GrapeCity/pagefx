Imports System

<AttributeUsage(AttributeTargets.All, AllowMultiple:=True)> _
Public Class AuthorAttribute
    Inherits Attribute
    Public Name As String
    Public No As Integer
    Public Sub New(ByVal Name As String)
        Me.Name = Name
    End Sub
    Public Sub New(ByVal Name As Integer)
        Me.Name = Name.toString()
    End Sub
End Class

<AttributeUsage(AttributeTargets.All)> _
Public Class PublicationAttribute
    Inherits Attribute
    Public Name As Object
    Public Sub New(ByVal Name As String)
        Me.Name = Name
    End Sub
End Class


<Publication("Tata McGraw"), AuthorAttribute("Robin Cook", _
 No:=47), Author(10)> _
Public Class C1
    <Author("John"), Publication("Tata McGraw")> _
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
