using System.Runtime.CompilerServices;

namespace Native
{
	public class Function
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern object call(object target);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern object call(object target, object arg1);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern object apply(object target);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern object apply(object target, NativeArray args);
	}
}
