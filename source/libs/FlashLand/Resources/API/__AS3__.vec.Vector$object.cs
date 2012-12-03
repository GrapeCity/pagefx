#region Custom Members
public static implicit operator Vector(Vector_object v)
{
    throw new NotImplementedException();
}

public extern object this[int index]
{
    [MethodImpl(MethodImplOptions.InternalCall)]
    get;

    [MethodImpl(MethodImplOptions.InternalCall)]
    set;
}
#endregion