using System;
using System.Runtime.CompilerServices;

namespace Avm
{
	public partial class XMLList
	{
		public extern XML this[int index]
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern XMLList this[Avm.String name]
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern XMLList this[Avm.Namespace ns, Avm.String name]
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern XMLList this[Avm.String ns, Avm.String name]
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
	}
}