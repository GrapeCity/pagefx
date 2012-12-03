#region Custom Members
public static implicit operator Vector(Vector_uint v)
{
    throw new NotImplementedException();
}

public extern uint this[int index]
{
    [MethodImpl(MethodImplOptions.InternalCall)]
    get;

    [MethodImpl(MethodImplOptions.InternalCall)]
    set;
}
#endregion