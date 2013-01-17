using System.Runtime.CompilerServices;

namespace Native
{
	public class NativeArray
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern NativeArray();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern NativeArray(int n);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern NativeArray Copy();

		public extern uint length
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void push(object value);

		public extern object this[int index]
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetInt32(int index);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern NativeArray reverse();
	}
}