using System;
using System.Runtime.CompilerServices;

namespace __AS3__.vec
{
	public partial class Vector_object : Avm.Object
	{
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
	}
}