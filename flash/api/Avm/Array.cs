using System;
using System.Runtime.CompilerServices;

namespace Avm
{
	public partial class Array : Avm.Object
	{
		[PageFX.ABC]
		[PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern virtual uint push(int arg);

		[PageFX.ABC]
		[PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern virtual uint push(uint arg);

		[PageFX.ABC]
		[PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern virtual uint push(Avm.String arg);

		public extern int Length
		{
			[PageFX.InlineProperty("length")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern object this[int index]
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;

			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		//Methods below can be used to Minimize CIL by avoiding unnecessary casting operations
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool GetBool(int index);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetInt32(int index);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern uint GetUInt32(int index);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern double GetDouble(int index);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Avm.String GetString(int index);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern char GetChar(int index);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetValue(int index, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetValue(int index, char value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetValue(int index, byte value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetValue(int index, sbyte value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetValue(int index, short value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetValue(int index, ushort value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetValue(int index, int value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetValue(int index, uint value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetValue(int index, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetValue(int index, double value);
	}
}