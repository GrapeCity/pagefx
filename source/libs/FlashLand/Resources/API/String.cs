#region Custom Members
public extern char this[int index]
{
    [PageFX.ABC]
    [PageFX.QName("charCodeAt", "http://adobe.com/AS3/2006/builtin", "public")]
    [MethodImpl(MethodImplOptions.InternalCall)]
    get;
}

//To optimze CIL code
[PageFX.ABC]
[PageFX.QName("fromCharCode", "http://adobe.com/AS3/2006/builtin", "public")]
[MethodImpl(MethodImplOptions.InternalCall)]
public extern static String fromCharCode(char ch);

[PageFX.ABC]
[PageFX.QName("fromCharCode", "http://adobe.com/AS3/2006/builtin", "public")]
[MethodImpl(MethodImplOptions.InternalCall)]
public extern static String fromCharCode(uint ch);
#endregion