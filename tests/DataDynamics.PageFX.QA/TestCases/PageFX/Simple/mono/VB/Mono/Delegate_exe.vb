REM CompilerOptions: /r:Delegate_dll.dll

Imports System
Imports NSDelegate

Class C1
    Dim x1 As New C()
    Function __f() As Object
        System.Console.WriteLine("__f called")
    End Function


    Public Function s() As Object
        x1.callSD(AddressOf Me.__f)
    End Function
End Class

Module M
	Function _Main() As Integer
        Dim x As New C1()
        x.s()
    End Function

	Sub Main()
		_Main()
		System.Console.WriteLine("<%END%>")
	End Sub

End Module
