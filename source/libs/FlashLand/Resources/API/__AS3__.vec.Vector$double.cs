#region Custom Members
public static implicit operator Vector(Vector_double v)
{
    throw new NotImplementedException();
}

public extern double this[int index]
{
    [MethodImpl(MethodImplOptions.InternalCall)]
    get;

    [MethodImpl(MethodImplOptions.InternalCall)]
    set;
}
#endregion