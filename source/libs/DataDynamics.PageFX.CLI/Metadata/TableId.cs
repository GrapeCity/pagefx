namespace DataDynamics.PageFX.CLI.Metadata
{
	/// <summary>
	/// Defines metadata table codes.
	/// </summary>
	public enum TableId
	{
		/// <summary>
		/// Identifies Assembly MDB Table (0, 0x00)
		/// </summary>
		Assembly = 0x20,
		/// <summary>
		/// Identifies AssemblyOS MDB Table (1, 0x01)
		/// </summary>
		AssemblyOS = 0x22,
		/// <summary>
		/// Identifies AssemblyProcessor MDB Table (2, 0x02)
		/// </summary>
		AssemblyProcessor = 0x21,
		/// <summary>
		/// Identifies AssemblyRef MDB Table (3, 0x03)
		/// </summary>
		AssemblyRef = 0x23,
		/// <summary>
		/// Identifies AssemblyRefOS MDB Table (4, 0x04)
		/// </summary>
		AssemblyRefOS = 0x25,
		/// <summary>
		/// Identifies AssemblyRefProcessor MDB Table (5, 0x05)
		/// </summary>
		AssemblyRefProcessor = 0x24,
		/// <summary>
		/// Identifies ClassLayout MDB Table (6, 0x06)
		/// </summary>
		ClassLayout = 0x0F,
		/// <summary>
		/// Identifies Constant MDB Table (7, 0x07)
		/// </summary>
		Constant = 0x0B,
		/// <summary>
		/// Identifies CustomAttribute MDB Table (8, 0x08)
		/// </summary>
		CustomAttribute = 0x0C,
		/// <summary>
		/// Identifies DeclSecurity MDB Table (9, 0x09)
		/// </summary>
		DeclSecurity = 0x0E,
		/// <summary>
		/// Identifies EventMap MDB Table (10, 0x0A)
		/// </summary>
		EventMap = 0x12,
		/// <summary>
		/// Identifies Event MDB Table (11, 0x0B)
		/// </summary>
		Event = 0x14,
		/// <summary>
		/// Identifies ExportedType MDB Table (12, 0x0C)
		/// </summary>
		ExportedType = 0x27,
		/// <summary>
		/// Identifies Field MDB Table (13, 0x0D)
		/// </summary>
		Field = 0x04,
		/// <summary>
		/// Identifies FieldLayout MDB Table (14, 0x0E)
		/// </summary>
		FieldLayout = 0x10,
		/// <summary>
		/// Identifies FieldMarshal MDB Table (15, 0x0F)
		/// </summary>
		FieldMarshal = 0x0D,
		/// <summary>
		/// Identifies FieldRVA MDB Table (16, 0x10)
		/// </summary>
		FieldRVA = 0x1D,
		/// <summary>
		/// Identifies File MDB Table (17, 0x11)
		/// </summary>
		File = 0x26,
		/// <summary>
		/// Identifies GenericParam MDB Table (18, 0x12)
		/// </summary>
		GenericParam = 0x2A,
		/// <summary>
		/// Identifies GenericParamConstraint MDB Table (19, 0x13)
		/// </summary>
		GenericParamConstraint = 0x2C,
		/// <summary>
		/// Identifies ImplMap MDB Table (20, 0x14)
		/// </summary>
		ImplMap = 0x1C,
		/// <summary>
		/// Identifies InterfaceImpl MDB Table (21, 0x15)
		/// </summary>
		InterfaceImpl = 0x09,
		/// <summary>
		/// Identifies ManifestResource MDB Table (22, 0x16)
		/// </summary>
		ManifestResource = 0x28,
		/// <summary>
		/// Identifies MemberRef MDB Table (23, 0x17)
		/// </summary>
		MemberRef = 0x0A,
		/// <summary>
		/// Identifies MethodDef MDB Table (24, 0x18)
		/// </summary>
		MethodDef = 0x06,
		/// <summary>
		/// Identifies MethodImpl MDB Table (25, 0x19)
		/// </summary>
		MethodImpl = 0x19,
		/// <summary>
		/// Identifies MethodSemantics MDB Table (26, 0x1A)
		/// </summary>
		MethodSemantics = 0x18,
		/// <summary>
		/// Identifies MethodSpec MDB Table (27, 0x1B)
		/// </summary>
		MethodSpec = 0x2B,
		/// <summary>
		/// Identifies Module MDB Table (28, 0x1C)
		/// </summary>
		Module = 0x00,
		/// <summary>
		/// Identifies ModuleRef MDB Table (29, 0x1D)
		/// </summary>
		ModuleRef = 0x1A,
		/// <summary>
		/// Identifies NestedClass MDB Table (30, 0x1E)
		/// </summary>
		NestedClass = 0x29,
		/// <summary>
		/// Identifies Param MDB Table (31, 0x1F)
		/// </summary>
		Param = 0x08,
		/// <summary>
		/// Identifies Property MDB Table (32, 0x20)
		/// </summary>
		Property = 0x17,
		/// <summary>
		/// Identifies PropertyMap MDB Table (33, 0x21)
		/// </summary>
		PropertyMap = 0x15,
		/// <summary>
		/// Identifies StandAloneSig MDB Table (34, 0x22)
		/// </summary>
		StandAloneSig = 0x11,
		/// <summary>
		/// Identifies TypeDef MDB Table (35, 0x23)
		/// </summary>
		TypeDef = 0x02,
		/// <summary>
		/// Identifies TypeRef MDB Table (36, 0x24)
		/// </summary>
		TypeRef = 0x01,
		/// <summary>
		/// Identifies TypeSpec MDB Table (37, 0x25)
		/// </summary>
		TypeSpec = 0x1B,
		/// <summary>
		/// Identifies FieldPtr MDB Table (38, 0x26)
		/// </summary>
		FieldPtr = 3,
		/// <summary>
		/// Identifies MethodPtr MDB Table (39, 0x27)
		/// </summary>
		MethodPtr = 5,
		/// <summary>
		/// Identifies ParamPtr MDB Table (40, 0x28)
		/// </summary>
		ParamPtr = 7,
		/// <summary>
		/// Identifies EventPtr MDB Table (41, 0x29)
		/// </summary>
		EventPtr = 19,
		/// <summary>
		/// Identifies PropertyPtr MDB Table (42, 0x2A)
		/// </summary>
		PropertyPtr = 22,
		/// <summary>
		/// Identifies EncodingLog MDB Table (43, 0x2B)
		/// </summary>
		EncodingLog = 30,
		/// <summary>
		/// Identifies EncodingMap MDB Table (44, 0x2C)
		/// </summary>
		EncodingMap = 31,
	}
}