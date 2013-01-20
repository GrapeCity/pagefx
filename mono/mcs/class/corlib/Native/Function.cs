using System.Runtime.CompilerServices;
using PageFX;

namespace Native
{
	[Native]
	[QName("Function")]
	public class Function
	{
		[QName("call", "http://adobe.com/AS3/2006/builtin", "public")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern object call(object target);

		[QName("call", "http://adobe.com/AS3/2006/builtin", "public")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern object call(object target, object arg1);

		[QName("apply", "http://adobe.com/AS3/2006/builtin", "public")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern object apply(object target);

		[QName("apply", "http://adobe.com/AS3/2006/builtin", "public")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern object apply(object target, NativeArray args);
	}
}
