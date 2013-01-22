using System.Runtime.CompilerServices;
using PageFX;

namespace Native
{
	[Native]
	[QName("Function")]
	public class Function
	{
		[AS3]
		[InlineFunction("call")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern object call(object target);

		[AS3]
		[InlineFunction("call")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern object call(object target, object arg1);

		[AS3]
		[InlineFunction("apply")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern object apply(object target);

		[AS3]
		[InlineFunction("apply")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern object apply(object target, NativeArray args);
	}
}
