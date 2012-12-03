mkdir bin\debug
copy .\IVUI\ivui.dll bin\debug\ivui.dll
SET PFXHOME=C:\pfx\
SET PFXLIB="%PFXHOME%\framework"
SET FLEX="%PFXHOME%\flexsdk"
csc /nologo /nostdlib /noconfig /t:library /r:%PFXLIB%\mscorlib.dll /r:%PFXLIB%\System.dll /r:%PFXLIB%\System.Xml.dll /r:%PFXLIB%\System.Data.dll /r:%FLEX%\flex3.dll /r:%FLEX%\flex3.rpc.dll /r:bin\debug\ivui.dll /out:bin\Debug\IssueVision.dll /recurse:src\*.cs