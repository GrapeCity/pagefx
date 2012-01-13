@rem generate *.wxs
tallow -nologo -d bin       > bin.tmp
tallow -nologo -d flexsdk   > flexsdk.tmp
tallow -nologo -d framework > framework.tmp
tallow -nologo -d samples   > samples.tmp
tallow -nologo -d tests     > test.tmp
tallow -nologo -d "Visual Studio" > vs.tmp

@rem copy project

@rem replace guids
rv /noprefix /file:binfile /TARGETDIR:PFXDIR /PUT-GUID-HERE:newguid()  /component:bincomp /out:bin.wxt bin.tmp
rv /noprefix /TARGETDIR:PFXDIR /PUT-GUID-HERE:newguid() /file:fsdkfile /component:fsdkcomp /out:flexsdk.wxt flexsdk.tmp
rv /noprefix /TARGETDIR:PFXDIR /PUT-GUID-HERE:newguid() /file:fwfile /component:fwcomp /out:framework.wxt framework.tmp
rv /noprefix /TARGETDIR:PFXDIR /PUT-GUID-HERE:newguid() /file:smplfile /component:smplcomp /out:samples.wxt samples.tmp
rv /noprefix /TARGETDIR:PFXDIR /PUT-GUID-HERE:newguid() /file:testfile /component:testcomp /out:test.wxt test.tmp
rv /noprefix /TARGETDIR:PFXDIR /PUT-GUID-HERE:newguid() /file:vsfile /component:vscomp /out:vs.wxt vs.tmp

@rem pfx-wix
pfx-wix bin.wxt flexsdk.wxt framework.wxt samples.wxt test.wxt vs.wxt

@rem rename
@ren bin.wxt.2.xml bin.wxs
@ren flexsdk.wxt.2.xml flexsdk.wxs
@ren framework.wxt.2.xml framework.wxs
@ren samples.wxt.2.xml samples.wxs
@ren test.wxt.2.xml test.wxs
@ren vs.wxt.2.xml vs.wxs


@rem build msi
candle product.wxs bin.wxs flexsdk.wxs framework.wxs samples.wxs test.wxs vs.wxs
light product.wixobj bin.wixobj flexsdk.wixobj framework.wixobj samples.wixobj test.wixobj vs.wixobj -out %1

@rem CLEAN SECTION
del *.tmp
del *.xml
del *.wixobj
del *.wxt
del *.wxi
del *.wxs