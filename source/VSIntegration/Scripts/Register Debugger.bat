SET ADDINFILE=DataDynamics.PageFX.VisualStudio.Addin.addin
%PFXHOME%\bin\pfc.exe /--rv /PFXHOME:envp(PFXHOME) /out:DebugEngine.reg DebugEngine.txt  
%PFXHOME%\bin\pfc.exe /--rv /PFXHOME:env(PFXHOME) /out:%ADDINFILE% VSAddin.xml

rem todo: depend on os version My Documents or Documents
SET MYDOCSDIR=%USERPROFILE%\Documents
SET ADDINSDIR=%MYDOCSDIR%\Visual Studio 2008\Addins
COPY /y "%ADDINFILE%" "%ADDINSDIR%"

DebugEngine.reg

del DebugEngine.reg
del "%ADDINFILE%"