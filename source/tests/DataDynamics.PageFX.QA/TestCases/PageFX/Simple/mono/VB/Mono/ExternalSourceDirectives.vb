Imports System
Module ExternalSourceDirective
	Function _Main() As Integer
		#ExternalSource("/home/main.aspx",30)
        Console.WriteLine("In main.aspx")
		#End ExternalSource
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub



    Function A() As Object
		#ExternalSource("/home/a.aspx",23)
        Console.WriteLine("In a.aspx")
		#End ExternalSource		
    End Function
End Module
