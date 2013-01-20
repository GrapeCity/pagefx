using System.Runtime.CompilerServices;
using PageFX;

namespace Native
{
	[Native]
	[QName("Array")]
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

		[QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
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

		[QName("reverse", "http://adobe.com/AS3/2006/builtin", "public")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern NativeArray reverse();
	}
}