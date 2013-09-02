using System;
using System.Runtime.CompilerServices;

namespace __AS3__.vec
{
	public partial class Vector_uint : Avm.Object
	{
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
	}
}