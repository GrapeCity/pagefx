SET MYDOCSDIR=%USERPROFILE%\My Documents
SET PROJECTTEMPLATESDESTDIR=%MYDOCSDIR%\Visual Studio 2008\Templates\ProjectTemplates\Visual C#\PageFX\
SET ITEMTEMPLATESDESTDIR=%MYDOCSDIR%\Visual Studio 2008\Templates\ItemTemplates/Visual C#\PageFX\

SET PFXBINDIR=%PFXHOME%\bin
SET PFXTEMPLATESDIR=%PFXHOME%\Visual Studio\templates\ProjectTemplates

SET VSROOTKEY=SOFTWARE\Microsoft\VisualStudio\9.0


copy /y "%PFXTEMPLATESDIR%\*.*" "%PROJECTTEMPLATESDESTDIR%"

reg.exe add HKLM\%VSROOTKEY%\MSBuild\SafeImports /f /v PageFX /d "%PFXBINDIR%\PageFX.targets"