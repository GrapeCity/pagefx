#region Custom Members
public static implicit operator Vector(Vector_int v)
{
    throw new NotImplementedException();
}

public extern int this[int index]
{
    [MethodImpl(MethodImplOptions.InternalCall)]
    get;

    [MethodImpl(MethodImplOptions.InternalCall)]
    set;
}
#endregion