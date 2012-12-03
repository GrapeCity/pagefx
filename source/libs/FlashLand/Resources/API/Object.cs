#region Custom Members
[MethodImpl(MethodImplOptions.InternalCall)]
public extern object GetProperty(string name);

[MethodImpl(MethodImplOptions.InternalCall)]
public extern object GetProperty(string ns, string name);

[MethodImpl(MethodImplOptions.InternalCall)]
public extern object GetProperty(Namespace ns, string name);

[MethodImpl(MethodImplOptions.InternalCall)]
public extern void SetProperty(string name, object value);

[MethodImpl(MethodImplOptions.InternalCall)]
public extern void SetProperty(string ns, string name, object value);

[MethodImpl(MethodImplOptions.InternalCall)]
public extern void SetProperty(Namespace ns, string name, object value);
#endregion