using System;
using System.Runtime.CompilerServices;

namespace __AS3__.vec
{
	public partial class Vector_int : Avm.Object
	{
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
	}
}