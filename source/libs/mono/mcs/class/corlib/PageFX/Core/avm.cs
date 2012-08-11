using System.Runtime.CompilerServices;

//Contains:
//1. low level inline code
public static class avm
{
    #region String API
    public static extern Avm.String EmptyString
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        get;
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
	public static extern Avm.String Concat(Avm.String s1, int value);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Avm.String ToString(sbyte value);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Avm.String ToString(byte value);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Avm.String ToString(short value);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Avm.String ToString(ushort value);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Avm.String ToString(int value);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Avm.String ToString(uint value);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Avm.String ToString(double value);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Avm.String ToString(float value);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Avm.String ToString(object value);
    #endregion

    #region Array API
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Avm.Array NewArray();

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Avm.Array NewArray(int n);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Avm.Array CopyArray(Avm.Array arr);
    #endregion

    #region avm shell
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void Console_Write(Avm.String s);

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void exit(int exitCode);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void trace(Avm.String s);
    #endregion

    public static extern bool IsFlashPlayer
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        get;
    }

    /// <summary>
    /// Returns global package namespace
    /// </summary>
    public static extern Avm.Namespace GlobalPackage
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        get;
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern object Findpropstrict(Avm.Namespace ns, Avm.String name);

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern object Construct(object receiver, Avm.Namespace ns, Avm.String name);

    #region GetProperty
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern object GetProperty(Avm.Namespace ns, Avm.String name);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern object GetProperty(object obj, Avm.Namespace ns, Avm.String name);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetProperty(object obj, Avm.Namespace ns, Avm.String name, object value);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetProperty(Avm.Namespace ns, Avm.String name, object value);
    #endregion

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Avm.Object CreateInstance(Avm.Class klass);

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void AddEventListener(Avm.Object dispatcher, Avm.String eventName, Avm.Function f);

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void RemoveEventListener(Avm.Object dispatcher, Avm.String eventName, Avm.Function f);

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern object get_m_value(object obj);

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void set_m_value(object obj, object value);

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void ReturnValue(object obj);

    #region NewObject
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Avm.Object NewObject(string key, object value);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Avm.Object NewObject(string key1, object value1,
                                              string key2, object value2);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Avm.Object NewObject(string key1, object value1,
                                              string key2, object value2,
                                              string key3, object value3);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Avm.Object NewObject(string key1, object value1,
                                              string key2, object value2,
                                              string key3, object value3,
                                              string key4, object value4);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Avm.Object NewObject(string key1, object value1,
                                              string key2, object value2,
                                              string key3, object value3,
                                              string key4, object value4,
                                              string key5, object value5);
    #endregion

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool IsNull(object obj);

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool IsUndefined(object obj);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern object GetArrayElem(object obj, int index);
}

//Indicates that local variables with System.Object type should be declared as * (any) type
internal class AnyVarsAttribute : System.Attribute
{
}