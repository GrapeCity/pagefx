using System;
using System.Runtime.CompilerServices;

namespace __AS3__.vec
{
	public partial class Vector_double : Avm.Object
	{
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
	}
}
