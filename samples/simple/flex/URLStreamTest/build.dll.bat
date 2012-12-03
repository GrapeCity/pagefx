mkdir bin\debug
SET PFXLIB="%PFXHOME%"\framework
SET FLEX="%PFXHOME%"\flexsdk
csc /nologo /nostdlib /noconfig /t:library /r:%PFXLIB%\corlib.dll /r:%PFXLIB%\System.dll /r:%FLEX%\flex3.dll /out:bin\Debug\URLStreamTest.dll /recurse:src\*.cs