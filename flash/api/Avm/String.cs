using System;
using System.Runtime.CompilerServices;

namespace Avm
{
	public partial class String : Avm.Object
	{
		public extern char this[int index]
		{
			[PageFX.ABC]
			[PageFX.QName("charCodeAt", "http://adobe.com/AS3/2006/builtin", "public")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		//To optimze CIL code
		[PageFX.ABC]
		[PageFX.QName("fromCharCode", "http://adobe.com/AS3/2006/builtin", "public")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern static String fromCharCode(char ch);

		[PageFX.ABC]
		[PageFX.QName("fromCharCode", "http://adobe.com/AS3/2006/builtin", "public")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern static String fromCharCode(uint ch);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern implicit operator System.String(Avm.String s);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern implicit operator Avm.String(System.String s);
	}
}