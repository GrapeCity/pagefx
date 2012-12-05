'Author: Ritvik Mayank <mritvik@novell.com>
'Copyright (C) 2005 Novell Inc. (http://www.novell.com)
'Both Simple and SimpleAttribute is defined

Imports System

<AttributeUsage(AttributeTargets.Class)> _
Public Class SimpleAttribute
    Inherits System.Attribute

    Public Sub New(ByVal A As String)
        Me.A = A
    End Sub

    Public B As String
    Private A As String

    Public ReadOnly Property A1() As String
        Get
            Return A
        End Get
    End Property
End Class

<SimpleAttribute("string")> _
Public Class Class2
End Class


<Simple("hello")> _
Public Class Class1
    Shared Function Main() As Integer
        Dim type As Type = GetType(Class1)
        Dim arr() As Object = _
                      type.GetCustomAttributes(GetType(simpleAttribute), True)
    End Function
End Class