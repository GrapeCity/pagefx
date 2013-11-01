using System;
using System.Runtime.CompilerServices;

namespace Avm
{
	public partial class Class : Avm.Object
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Class Find(string ns, string name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Class Find(Namespace ns, string name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern object CreateInstance();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern object CreateInstance(object arg1);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern object CreateInstance(object arg1, object arg2);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern object CreateInstance(object arg1, object arg2, object arg3);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern object CreateInstance(object arg1, object arg2, object arg3, object arg4);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern object CreateInstance(object arg1, object arg2, object arg3, object arg4, object arg5);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern implicit operator Class(Type type);
	}
}