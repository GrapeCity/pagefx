using System.Runtime.CompilerServices;
using PageFX;

//Contains:
//1. low level inline code
public static class avm
{
    #region String API
	[InlineOperator("+")]
    [MethodImpl(MethodImplOptions.InternalCall)]
	public static extern string Concat(string s1, int value);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string ToString(sbyte value);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string ToString(byte value);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string ToString(short value);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string ToString(ushort value);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string ToString(int value);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string ToString(uint value);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string ToString(double value);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string ToString(float value);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string ToString(object value);
	#endregion

	[MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void Console_Write(string s);

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void exit(int exitCode);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void trace(string s);

	public static extern bool IsFlashPlayer
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        get;
    }

    /// <summary>
    /// Returns global package namespace
    /// </summary>
    internal static extern object GlobalPackage
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        get;
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern object Findpropstrict(object ns, string name);

    [MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern object Construct(object receiver, object ns, string name);

    #region GetProperty
    
    [MethodImpl(MethodImplOptions.InternalCall)]
	public static extern object GetProperty(object obj, object ns, string name);

    [MethodImpl(MethodImplOptions.InternalCall)]
	public static extern void SetProperty(object obj, object ns, string name, object value);

    #endregion

    [MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern object CreateInstance(object klass);

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern object get_m_value(object obj);

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void set_m_value(object obj, object value);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern object GetArrayElem(object obj, int index);
}