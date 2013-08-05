using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Flash.Abc
{
	/// <summary>
	/// Enumerates constant types supported by AVM+
	/// </summary>
	public enum AbcConstKind : byte
	{
		/// <summary>
		/// undefined
		/// </summary>
		Undefined = 0x00,

		/// <summary>
		/// UTF8 encoded string
		/// </summary>
		String = 0x01,

		/// <summary>
		/// Decimal constant
		/// </summary>
		Decimal = 0x02, // the number 2 was unused in previous version

		/// <summary>
		/// signed integer
		/// </summary>
		Int = 0x03,

		/// <summary>
		/// unsigned integer
		/// </summary>
		UInt = 0x04,

		/// <summary>
		/// non-shared namespace
		/// </summary>
		PrivateNamespace = 0x05,

		/// <summary>
		/// double
		/// </summary>
		Double = 0x06,

		/// <summary>
		/// 
		/// </summary>
		QName = 0x07,

		PublicNamespace = 0x08,

		Multiname = 0x09, // o.name, ct nsset, ct name

		False = 0x0A,

		True = 0x0B,

		Null = 0x0C,

		QNameA = 0x0D, // o.@ns::name, ct ns, ct attr-name
		MultinameA = 0x0E, // o.@name, ct attr-name
		RTQName = 0x0F, // o.ns::name, rt ns, ct name
		RTQNameA = 0x10, // o.@ns::name, rt ns, ct attr-name
		RTQNameL = 0x11, // o.ns::[name], rt ns, rt name
		RTQNameLA = 0x12, // o.@ns::[name], rt ns, rt attr-name

		NamespaceSet = 0x15,
		PackageNamespace = 0x16,
		InternalNamespace = 0x17,
		ProtectedNamespace = 0x18,
		ExplicitNamespace = 0x19,
		StaticProtectedNamespace = 0x1A,
		MultinameL = 0x1B,
		MultinameLA = 0x1C,
		TypeName = 0x1D,    // used for parametrized types 
	}

	internal static class AbcConstKindEnum
	{
		/// <summary>
		/// Determines whether specified constant is namespace.
		/// </summary>
		public static bool IsNamespace(this AbcConstKind value)
		{
			switch (value)
			{
				case AbcConstKind.ExplicitNamespace:
				case AbcConstKind.InternalNamespace:
				case AbcConstKind.PackageNamespace:
				case AbcConstKind.PrivateNamespace:
				case AbcConstKind.ProtectedNamespace:
				case AbcConstKind.PublicNamespace:
				case AbcConstKind.StaticProtectedNamespace:
					return true;
			}
			return false;
		}

		/// <summary>
		/// Gets visibility of specified constant type.
		/// </summary>
		public static Visibility Visibility(this AbcConstKind value)
		{
			switch (value)
			{
				case AbcConstKind.PrivateNamespace:
					return Common.TypeSystem.Visibility.Private;
				case AbcConstKind.PublicNamespace:
					return Common.TypeSystem.Visibility.Public;
				case AbcConstKind.PackageNamespace:
					return Common.TypeSystem.Visibility.Public;
				case AbcConstKind.InternalNamespace:
					return Common.TypeSystem.Visibility.Internal;
				case AbcConstKind.ProtectedNamespace:
					return Common.TypeSystem.Visibility.Protected;
				case AbcConstKind.StaticProtectedNamespace:
					return Common.TypeSystem.Visibility.Protected;
				default:
					return Common.TypeSystem.Visibility.Private;
			}
		}
	}
}