copy *.dll ..\dll
SET FXDIR=c:\pfx\flexsdk
md %FXDIR%
copy *.dll %FXDIR%
del *.dll;*.pdb;*.swc.abc;*.swcdep