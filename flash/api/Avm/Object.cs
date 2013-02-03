using System.Runtime.CompilerServices;
using PageFX;

namespace Avm
{
	public partial class Object
	{
		[InlineFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Object(string key, object value);

		[InlineFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Object(string key1, object value1,
		                     string key2, object value2);

		[InlineFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Object(string key1, object value1,
		                     string key2, object value2,
		                     string key3, object value3);

		[InlineFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Object(string key1, object value1,
		                     string key2, object value2,
		                     string key3, object value3,
		                     string key4, object value4);

		[InlineFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Object(string key1, object value1,
		                     string key2, object value2,
		                     string key3, object value3,
		                     string key4, object value4,
		                     string key5, object value5);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern object GetProperty(string name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern object GetProperty(string ns, string name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern object GetProperty(Namespace ns, string name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetProperty(string name, object value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetProperty(string ns, string name, object value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetProperty(Namespace ns, string name, object value);
	}
}