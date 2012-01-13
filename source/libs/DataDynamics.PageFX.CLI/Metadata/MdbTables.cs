//
// WARNING: Automatically generated file. DO NOT EDIT
//

using System;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.Metadata
{
	#region enum MdbTableId
	/// <summary>
	/// MDB Table Identifiers
	/// </summary>
	public enum MdbTableId
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
	#endregion

	#region enum MdbColumnId
	/// <summary>
	/// MDB Columns
	/// </summary>
	public enum MdbColumnId
	{
		/// <summary>
		/// Index of HashAlgId column in Assembly table
		/// </summary>
		Assembly_HashAlgId = 0,
		/// <summary>
		/// Index of MajorVersion column in Assembly table
		/// </summary>
		Assembly_MajorVersion = 1,
		/// <summary>
		/// Index of MinorVersion column in Assembly table
		/// </summary>
		Assembly_MinorVersion = 2,
		/// <summary>
		/// Index of BuildNumber column in Assembly table
		/// </summary>
		Assembly_BuildNumber = 3,
		/// <summary>
		/// Index of RevisionNumber column in Assembly table
		/// </summary>
		Assembly_RevisionNumber = 4,
		/// <summary>
		/// Index of Flags column in Assembly table
		/// </summary>
		Assembly_Flags = 5,
		/// <summary>
		/// Index of PublicKey column in Assembly table
		/// </summary>
		Assembly_PublicKey = 6,
		/// <summary>
		/// Index of Name column in Assembly table
		/// </summary>
		Assembly_Name = 7,
		/// <summary>
		/// Index of Culture column in Assembly table
		/// </summary>
		Assembly_Culture = 8,
		/// <summary>
		/// Index of OSPlatformId column in AssemblyOS table
		/// </summary>
		AssemblyOS_OSPlatformId = 0,
		/// <summary>
		/// Index of OSMajorVersion column in AssemblyOS table
		/// </summary>
		AssemblyOS_OSMajorVersion = 1,
		/// <summary>
		/// Index of OSMinorVersion column in AssemblyOS table
		/// </summary>
		AssemblyOS_OSMinorVersion = 2,
		/// <summary>
		/// Index of Processor column in AssemblyProcessor table
		/// </summary>
		AssemblyProcessor_Processor = 0,
		/// <summary>
		/// Index of MajorVersion column in AssemblyRef table
		/// </summary>
		AssemblyRef_MajorVersion = 0,
		/// <summary>
		/// Index of MinorVersion column in AssemblyRef table
		/// </summary>
		AssemblyRef_MinorVersion = 1,
		/// <summary>
		/// Index of BuildNumber column in AssemblyRef table
		/// </summary>
		AssemblyRef_BuildNumber = 2,
		/// <summary>
		/// Index of RevisionNumber column in AssemblyRef table
		/// </summary>
		AssemblyRef_RevisionNumber = 3,
		/// <summary>
		/// Index of Flags column in AssemblyRef table
		/// </summary>
		AssemblyRef_Flags = 4,
		/// <summary>
		/// Index of PublicKeyOrToken column in AssemblyRef table
		/// </summary>
		AssemblyRef_PublicKeyOrToken = 5,
		/// <summary>
		/// Index of Name column in AssemblyRef table
		/// </summary>
		AssemblyRef_Name = 6,
		/// <summary>
		/// Index of Culture column in AssemblyRef table
		/// </summary>
		AssemblyRef_Culture = 7,
		/// <summary>
		/// Index of HashValue column in AssemblyRef table
		/// </summary>
		AssemblyRef_HashValue = 8,
		/// <summary>
		/// Index of OSPlatformId column in AssemblyRefOS table
		/// </summary>
		AssemblyRefOS_OSPlatformId = 0,
		/// <summary>
		/// Index of OSMajorVersion column in AssemblyRefOS table
		/// </summary>
		AssemblyRefOS_OSMajorVersion = 1,
		/// <summary>
		/// Index of OSMinorVersion column in AssemblyRefOS table
		/// </summary>
		AssemblyRefOS_OSMinorVersion = 2,
		/// <summary>
		/// Index of AssemblyRef column in AssemblyRefOS table
		/// </summary>
		AssemblyRefOS_AssemblyRef = 3,
		/// <summary>
		/// Index of Processor column in AssemblyRefProcessor table
		/// </summary>
		AssemblyRefProcessor_Processor = 0,
		/// <summary>
		/// Index of AssemblyRef column in AssemblyRefProcessor table
		/// </summary>
		AssemblyRefProcessor_AssemblyRef = 1,
		/// <summary>
		/// Index of PackingSize column in ClassLayout table
		/// </summary>
		ClassLayout_PackingSize = 0,
		/// <summary>
		/// Index of ClassSize column in ClassLayout table
		/// </summary>
		ClassLayout_ClassSize = 1,
		/// <summary>
		/// Index of Parent column in ClassLayout table
		/// </summary>
		ClassLayout_Parent = 2,
		/// <summary>
		/// Index of Type column in Constant table
		/// </summary>
		Constant_Type = 0,
		/// <summary>
		/// Index of Parent column in Constant table
		/// </summary>
		Constant_Parent = 1,
		/// <summary>
		/// Index of Value column in Constant table
		/// </summary>
		Constant_Value = 2,
		/// <summary>
		/// Index of Parent column in CustomAttribute table
		/// </summary>
		CustomAttribute_Parent = 0,
		/// <summary>
		/// Index of Type column in CustomAttribute table
		/// </summary>
		CustomAttribute_Type = 1,
		/// <summary>
		/// Index of Value column in CustomAttribute table
		/// </summary>
		CustomAttribute_Value = 2,
		/// <summary>
		/// Index of Action column in DeclSecurity table
		/// </summary>
		DeclSecurity_Action = 0,
		/// <summary>
		/// Index of Parent column in DeclSecurity table
		/// </summary>
		DeclSecurity_Parent = 1,
		/// <summary>
		/// Index of PermissionSet column in DeclSecurity table
		/// </summary>
		DeclSecurity_PermissionSet = 2,
		/// <summary>
		/// Index of Parent column in EventMap table
		/// </summary>
		EventMap_Parent = 0,
		/// <summary>
		/// Index of EventList column in EventMap table
		/// </summary>
		EventMap_EventList = 1,
		/// <summary>
		/// Index of EventFlags column in Event table
		/// </summary>
		Event_EventFlags = 0,
		/// <summary>
		/// Index of Name column in Event table
		/// </summary>
		Event_Name = 1,
		/// <summary>
		/// Index of EventType column in Event table
		/// </summary>
		Event_EventType = 2,
		/// <summary>
		/// Index of Flags column in ExportedType table
		/// </summary>
		ExportedType_Flags = 0,
		/// <summary>
		/// Index of TypeDefId column in ExportedType table
		/// </summary>
		ExportedType_TypeDefId = 1,
		/// <summary>
		/// Index of TypeName column in ExportedType table
		/// </summary>
		ExportedType_TypeName = 2,
		/// <summary>
		/// Index of TypeNamespace column in ExportedType table
		/// </summary>
		ExportedType_TypeNamespace = 3,
		/// <summary>
		/// Index of Implementation column in ExportedType table
		/// </summary>
		ExportedType_Implementation = 4,
		/// <summary>
		/// Index of Flags column in Field table
		/// </summary>
		Field_Flags = 0,
		/// <summary>
		/// Index of Name column in Field table
		/// </summary>
		Field_Name = 1,
		/// <summary>
		/// Index of Signature column in Field table
		/// </summary>
		Field_Signature = 2,
		/// <summary>
		/// Index of Offset column in FieldLayout table
		/// </summary>
		FieldLayout_Offset = 0,
		/// <summary>
		/// Index of Field column in FieldLayout table
		/// </summary>
		FieldLayout_Field = 1,
		/// <summary>
		/// Index of Parent column in FieldMarshal table
		/// </summary>
		FieldMarshal_Parent = 0,
		/// <summary>
		/// Index of NativeType column in FieldMarshal table
		/// </summary>
		FieldMarshal_NativeType = 1,
		/// <summary>
		/// Index of RVA column in FieldRVA table
		/// </summary>
		FieldRVA_RVA = 0,
		/// <summary>
		/// Index of Field column in FieldRVA table
		/// </summary>
		FieldRVA_Field = 1,
		/// <summary>
		/// Index of Flags column in File table
		/// </summary>
		File_Flags = 0,
		/// <summary>
		/// Index of Name column in File table
		/// </summary>
		File_Name = 1,
		/// <summary>
		/// Index of HashValue column in File table
		/// </summary>
		File_HashValue = 2,
		/// <summary>
		/// Index of Number column in GenericParam table
		/// </summary>
		GenericParam_Number = 0,
		/// <summary>
		/// Index of Flags column in GenericParam table
		/// </summary>
		GenericParam_Flags = 1,
		/// <summary>
		/// Index of Owner column in GenericParam table
		/// </summary>
		GenericParam_Owner = 2,
		/// <summary>
		/// Index of Name column in GenericParam table
		/// </summary>
		GenericParam_Name = 3,
		/// <summary>
		/// Index of Owner column in GenericParamConstraint table
		/// </summary>
		GenericParamConstraint_Owner = 0,
		/// <summary>
		/// Index of Constraint column in GenericParamConstraint table
		/// </summary>
		GenericParamConstraint_Constraint = 1,
		/// <summary>
		/// Index of MappingFlags column in ImplMap table
		/// </summary>
		ImplMap_MappingFlags = 0,
		/// <summary>
		/// Index of MemberForwarded column in ImplMap table
		/// </summary>
		ImplMap_MemberForwarded = 1,
		/// <summary>
		/// Index of ImportName column in ImplMap table
		/// </summary>
		ImplMap_ImportName = 2,
		/// <summary>
		/// Index of ImportScope column in ImplMap table
		/// </summary>
		ImplMap_ImportScope = 3,
		/// <summary>
		/// Index of Class column in InterfaceImpl table
		/// </summary>
		InterfaceImpl_Class = 0,
		/// <summary>
		/// Index of Interface column in InterfaceImpl table
		/// </summary>
		InterfaceImpl_Interface = 1,
		/// <summary>
		/// Index of Offset column in ManifestResource table
		/// </summary>
		ManifestResource_Offset = 0,
		/// <summary>
		/// Index of Flags column in ManifestResource table
		/// </summary>
		ManifestResource_Flags = 1,
		/// <summary>
		/// Index of Name column in ManifestResource table
		/// </summary>
		ManifestResource_Name = 2,
		/// <summary>
		/// Index of Implementation column in ManifestResource table
		/// </summary>
		ManifestResource_Implementation = 3,
		/// <summary>
		/// Index of Class column in MemberRef table
		/// </summary>
		MemberRef_Class = 0,
		/// <summary>
		/// Index of Name column in MemberRef table
		/// </summary>
		MemberRef_Name = 1,
		/// <summary>
		/// Index of Signature column in MemberRef table
		/// </summary>
		MemberRef_Signature = 2,
		/// <summary>
		/// Index of RVA column in MethodDef table
		/// </summary>
		MethodDef_RVA = 0,
		/// <summary>
		/// Index of ImplFlags column in MethodDef table
		/// </summary>
		MethodDef_ImplFlags = 1,
		/// <summary>
		/// Index of Flags column in MethodDef table
		/// </summary>
		MethodDef_Flags = 2,
		/// <summary>
		/// Index of Name column in MethodDef table
		/// </summary>
		MethodDef_Name = 3,
		/// <summary>
		/// Index of Signature column in MethodDef table
		/// </summary>
		MethodDef_Signature = 4,
		/// <summary>
		/// Index of ParamList column in MethodDef table
		/// </summary>
		MethodDef_ParamList = 5,
		/// <summary>
		/// Index of Class column in MethodImpl table
		/// </summary>
		MethodImpl_Class = 0,
		/// <summary>
		/// Index of MethodBody column in MethodImpl table
		/// </summary>
		MethodImpl_MethodBody = 1,
		/// <summary>
		/// Index of MethodDeclaration column in MethodImpl table
		/// </summary>
		MethodImpl_MethodDeclaration = 2,
		/// <summary>
		/// Index of Semantics column in MethodSemantics table
		/// </summary>
		MethodSemantics_Semantics = 0,
		/// <summary>
		/// Index of Method column in MethodSemantics table
		/// </summary>
		MethodSemantics_Method = 1,
		/// <summary>
		/// Index of Association column in MethodSemantics table
		/// </summary>
		MethodSemantics_Association = 2,
		/// <summary>
		/// Index of Method column in MethodSpec table
		/// </summary>
		MethodSpec_Method = 0,
		/// <summary>
		/// Index of Instantiation column in MethodSpec table
		/// </summary>
		MethodSpec_Instantiation = 1,
		/// <summary>
		/// Index of Generation column in Module table
		/// </summary>
		Module_Generation = 0,
		/// <summary>
		/// Index of Name column in Module table
		/// </summary>
		Module_Name = 1,
		/// <summary>
		/// Index of Mvid column in Module table
		/// </summary>
		Module_Mvid = 2,
		/// <summary>
		/// Index of EncId column in Module table
		/// </summary>
		Module_EncId = 3,
		/// <summary>
		/// Index of EncBaseId column in Module table
		/// </summary>
		Module_EncBaseId = 4,
		/// <summary>
		/// Index of Name column in ModuleRef table
		/// </summary>
		ModuleRef_Name = 0,
		/// <summary>
		/// Index of Class column in NestedClass table
		/// </summary>
		NestedClass_Class = 0,
		/// <summary>
		/// Index of EnclosingClass column in NestedClass table
		/// </summary>
		NestedClass_EnclosingClass = 1,
		/// <summary>
		/// Index of Flags column in Param table
		/// </summary>
		Param_Flags = 0,
		/// <summary>
		/// Index of Sequence column in Param table
		/// </summary>
		Param_Sequence = 1,
		/// <summary>
		/// Index of Name column in Param table
		/// </summary>
		Param_Name = 2,
		/// <summary>
		/// Index of Flags column in Property table
		/// </summary>
		Property_Flags = 0,
		/// <summary>
		/// Index of Name column in Property table
		/// </summary>
		Property_Name = 1,
		/// <summary>
		/// Index of Type column in Property table
		/// </summary>
		Property_Type = 2,
		/// <summary>
		/// Index of Parent column in PropertyMap table
		/// </summary>
		PropertyMap_Parent = 0,
		/// <summary>
		/// Index of PropertyList column in PropertyMap table
		/// </summary>
		PropertyMap_PropertyList = 1,
		/// <summary>
		/// Index of Signature column in StandAloneSig table
		/// </summary>
		StandAloneSig_Signature = 0,
		/// <summary>
		/// Index of Flags column in TypeDef table
		/// </summary>
		TypeDef_Flags = 0,
		/// <summary>
		/// Index of TypeName column in TypeDef table
		/// </summary>
		TypeDef_TypeName = 1,
		/// <summary>
		/// Index of TypeNamespace column in TypeDef table
		/// </summary>
		TypeDef_TypeNamespace = 2,
		/// <summary>
		/// Index of Extends column in TypeDef table
		/// </summary>
		TypeDef_Extends = 3,
		/// <summary>
		/// Index of FieldList column in TypeDef table
		/// </summary>
		TypeDef_FieldList = 4,
		/// <summary>
		/// Index of MethodList column in TypeDef table
		/// </summary>
		TypeDef_MethodList = 5,
		/// <summary>
		/// Index of ResolutionScope column in TypeRef table
		/// </summary>
		TypeRef_ResolutionScope = 0,
		/// <summary>
		/// Index of TypeName column in TypeRef table
		/// </summary>
		TypeRef_TypeName = 1,
		/// <summary>
		/// Index of TypeNamespace column in TypeRef table
		/// </summary>
		TypeRef_TypeNamespace = 2,
		/// <summary>
		/// Index of Signature column in TypeSpec table
		/// </summary>
		TypeSpec_Signature = 0,
		/// <summary>
		/// Index of Field column in FieldPtr table
		/// </summary>
		FieldPtr_Field = 0,
		/// <summary>
		/// Index of Method column in MethodPtr table
		/// </summary>
		MethodPtr_Method = 0,
		/// <summary>
		/// Index of Param column in ParamPtr table
		/// </summary>
		ParamPtr_Param = 0,
		/// <summary>
		/// Index of Event column in EventPtr table
		/// </summary>
		EventPtr_Event = 0,
		/// <summary>
		/// Index of Property column in PropertyPtr table
		/// </summary>
		PropertyPtr_Property = 0,
		/// <summary>
		/// Index of Token column in EncodingLog table
		/// </summary>
		EncodingLog_Token = 0,
		/// <summary>
		/// Index of FuncCode column in EncodingLog table
		/// </summary>
		EncodingLog_FuncCode = 1,
		/// <summary>
		/// Index of Token column in EncodingMap table
		/// </summary>
		EncodingMap_Token = 0,
	}
	#endregion

	/// <summary>
	/// Contains MDB Table Schemas
	/// </summary>
	public static class MDB
	{
		#region CreateTable
		internal static MdbTable CreateTable(MdbTableId id)
		{
			switch (id)
			{
				case MdbTableId.Assembly: return new MdbTable(id, Assembly.Columns);
				case MdbTableId.AssemblyOS: return new MdbTable(id, AssemblyOS.Columns);
				case MdbTableId.AssemblyProcessor: return new MdbTable(id, AssemblyProcessor.Columns);
				case MdbTableId.AssemblyRef: return new MdbTable(id, AssemblyRef.Columns);
				case MdbTableId.AssemblyRefOS: return new MdbTable(id, AssemblyRefOS.Columns);
				case MdbTableId.AssemblyRefProcessor: return new MdbTable(id, AssemblyRefProcessor.Columns);
				case MdbTableId.ClassLayout: return new MdbTable(id, ClassLayout.Columns);
				case MdbTableId.Constant: return new MdbTable(id, Constant.Columns);
				case MdbTableId.CustomAttribute: return new MdbTable(id, CustomAttribute.Columns);
				case MdbTableId.DeclSecurity: return new MdbTable(id, DeclSecurity.Columns);
				case MdbTableId.EventMap: return new MdbTable(id, EventMap.Columns);
				case MdbTableId.Event: return new MdbTable(id, Event.Columns);
				case MdbTableId.ExportedType: return new MdbTable(id, ExportedType.Columns);
				case MdbTableId.Field: return new MdbTable(id, Field.Columns);
				case MdbTableId.FieldLayout: return new MdbTable(id, FieldLayout.Columns);
				case MdbTableId.FieldMarshal: return new MdbTable(id, FieldMarshal.Columns);
				case MdbTableId.FieldRVA: return new MdbTable(id, FieldRVA.Columns);
				case MdbTableId.File: return new MdbTable(id, File.Columns);
				case MdbTableId.GenericParam: return new MdbTable(id, GenericParam.Columns);
				case MdbTableId.GenericParamConstraint: return new MdbTable(id, GenericParamConstraint.Columns);
				case MdbTableId.ImplMap: return new MdbTable(id, ImplMap.Columns);
				case MdbTableId.InterfaceImpl: return new MdbTable(id, InterfaceImpl.Columns);
				case MdbTableId.ManifestResource: return new MdbTable(id, ManifestResource.Columns);
				case MdbTableId.MemberRef: return new MdbTable(id, MemberRef.Columns);
				case MdbTableId.MethodDef: return new MdbTable(id, MethodDef.Columns);
				case MdbTableId.MethodImpl: return new MdbTable(id, MethodImpl.Columns);
				case MdbTableId.MethodSemantics: return new MdbTable(id, MethodSemantics.Columns);
				case MdbTableId.MethodSpec: return new MdbTable(id, MethodSpec.Columns);
				case MdbTableId.Module: return new MdbTable(id, Module.Columns);
				case MdbTableId.ModuleRef: return new MdbTable(id, ModuleRef.Columns);
				case MdbTableId.NestedClass: return new MdbTable(id, NestedClass.Columns);
				case MdbTableId.Param: return new MdbTable(id, Param.Columns);
				case MdbTableId.Property: return new MdbTable(id, Property.Columns);
				case MdbTableId.PropertyMap: return new MdbTable(id, PropertyMap.Columns);
				case MdbTableId.StandAloneSig: return new MdbTable(id, StandAloneSig.Columns);
				case MdbTableId.TypeDef: return new MdbTable(id, TypeDef.Columns);
				case MdbTableId.TypeRef: return new MdbTable(id, TypeRef.Columns);
				case MdbTableId.TypeSpec: return new MdbTable(id, TypeSpec.Columns);
				case MdbTableId.FieldPtr: return new MdbTable(id, FieldPtr.Columns);
				case MdbTableId.MethodPtr: return new MdbTable(id, MethodPtr.Columns);
				case MdbTableId.ParamPtr: return new MdbTable(id, ParamPtr.Columns);
				case MdbTableId.EventPtr: return new MdbTable(id, EventPtr.Columns);
				case MdbTableId.PropertyPtr: return new MdbTable(id, PropertyPtr.Columns);
				case MdbTableId.EncodingLog: return new MdbTable(id, EncodingLog.Columns);
				case MdbTableId.EncodingMap: return new MdbTable(id, EncodingMap.Columns);
				default: throw new ArgumentOutOfRangeException("id");
			}
		}
		#endregion

		#region Columns
		/// <summary>
		/// 22.2 Assembly : 0x20
		/// </summary>
		public static class Assembly
		{
			/// <summary>
			/// 0. HashAlgId : HashAlgorithmId
			/// </summary>
			public static readonly MdbColumn HashAlgId = new MdbColumn(0, "HashAlgId", MdbColumnType.Int32, typeof(HashAlgorithmId), "");

			/// <summary>
			/// 1. MajorVersion : Int16
			/// </summary>
			public static readonly MdbColumn MajorVersion = new MdbColumn(1, "MajorVersion", MdbColumnType.Int16, "");

			/// <summary>
			/// 2. MinorVersion : Int16
			/// </summary>
			public static readonly MdbColumn MinorVersion = new MdbColumn(2, "MinorVersion", MdbColumnType.Int16, "");

			/// <summary>
			/// 3. BuildNumber : Int16
			/// </summary>
			public static readonly MdbColumn BuildNumber = new MdbColumn(3, "BuildNumber", MdbColumnType.Int16, "");

			/// <summary>
			/// 4. RevisionNumber : Int16
			/// </summary>
			public static readonly MdbColumn RevisionNumber = new MdbColumn(4, "RevisionNumber", MdbColumnType.Int16, "");

			/// <summary>
			/// 5. Flags : AssemblyFlags
			/// </summary>
			public static readonly MdbColumn Flags = new MdbColumn(5, "Flags", MdbColumnType.Int32, typeof(AssemblyFlags), "");

			/// <summary>
			/// 6. PublicKey : BlobIndex
			/// </summary>
			public static readonly MdbColumn PublicKey = new MdbColumn(6, "PublicKey", MdbColumnType.BlobIndex, "");

			/// <summary>
			/// 7. Name : StringIndex
			/// </summary>
			public static readonly MdbColumn Name = new MdbColumn(7, "Name", MdbColumnType.StringIndex, "");

			/// <summary>
			/// 8. Culture : StringIndex
			/// </summary>
			public static readonly MdbColumn Culture = new MdbColumn(8, "Culture", MdbColumnType.StringIndex, "");

			/// <summary>
			/// Assembly Columns
			/// </summary>
			internal static readonly MdbColumn[] Columns = {HashAlgId, MajorVersion, MinorVersion, BuildNumber, RevisionNumber, Flags, PublicKey, Name, Culture};
		}
		/// <summary>
		/// 22.3 AssemblyOS : 0x22
		/// </summary>
		public static class AssemblyOS
		{
			/// <summary>
			/// 0. OSPlatformId : Int32
			/// </summary>
			public static readonly MdbColumn OSPlatformId = new MdbColumn(0, "OSPlatformId", MdbColumnType.Int32, "");

			/// <summary>
			/// 1. OSMajorVersion : Int32
			/// </summary>
			public static readonly MdbColumn OSMajorVersion = new MdbColumn(1, "OSMajorVersion", MdbColumnType.Int32, "");

			/// <summary>
			/// 2. OSMinorVersion : Int32
			/// </summary>
			public static readonly MdbColumn OSMinorVersion = new MdbColumn(2, "OSMinorVersion", MdbColumnType.Int32, "");

			/// <summary>
			/// AssemblyOS Columns
			/// </summary>
			internal static readonly MdbColumn[] Columns = {OSPlatformId, OSMajorVersion, OSMinorVersion};
		}
		/// <summary>
		/// 22.4 AssemblyProcessor : 0x21
		/// </summary>
		public static class AssemblyProcessor
		{
			/// <summary>
			/// 0. Processor : Int32
			/// </summary>
			public static readonly MdbColumn Processor = new MdbColumn(0, "Processor", MdbColumnType.Int32, "");

			/// <summary>
			/// AssemblyProcessor Columns
			/// </summary>
			internal static readonly MdbColumn[] Columns = {Processor};
		}
		/// <summary>
		/// 22.5 AssemblyRef : 0x23
		/// </summary>
		public static class AssemblyRef
		{
			/// <summary>
			/// 0. MajorVersion : Int16
			/// </summary>
			public static readonly MdbColumn MajorVersion = new MdbColumn(0, "MajorVersion", MdbColumnType.Int16, "");

			/// <summary>
			/// 1. MinorVersion : Int16
			/// </summary>
			public static readonly MdbColumn MinorVersion = new MdbColumn(1, "MinorVersion", MdbColumnType.Int16, "");

			/// <summary>
			/// 2. BuildNumber : Int16
			/// </summary>
			public static readonly MdbColumn BuildNumber = new MdbColumn(2, "BuildNumber", MdbColumnType.Int16, "");

			/// <summary>
			/// 3. RevisionNumber : Int16
			/// </summary>
			public static readonly MdbColumn RevisionNumber = new MdbColumn(3, "RevisionNumber", MdbColumnType.Int16, "");

			/// <summary>
			/// 4. Flags : AssemblyFlags
			/// </summary>
			public static readonly MdbColumn Flags = new MdbColumn(4, "Flags", MdbColumnType.Int32, typeof(AssemblyFlags), "");

			/// <summary>
			/// 5. PublicKeyOrToken : BlobIndex
			/// </summary>
			public static readonly MdbColumn PublicKeyOrToken = new MdbColumn(5, "PublicKeyOrToken", MdbColumnType.BlobIndex, "");

			/// <summary>
			/// 6. Name : StringIndex
			/// </summary>
			public static readonly MdbColumn Name = new MdbColumn(6, "Name", MdbColumnType.StringIndex, "");

			/// <summary>
			/// 7. Culture : StringIndex
			/// </summary>
			public static readonly MdbColumn Culture = new MdbColumn(7, "Culture", MdbColumnType.StringIndex, "");

			/// <summary>
			/// 8. HashValue : BlobIndex
			/// </summary>
			public static readonly MdbColumn HashValue = new MdbColumn(8, "HashValue", MdbColumnType.BlobIndex, "");

			/// <summary>
			/// AssemblyRef Columns
			/// </summary>
			internal static readonly MdbColumn[] Columns = {MajorVersion, MinorVersion, BuildNumber, RevisionNumber, Flags, PublicKeyOrToken, Name, Culture, HashValue};
		}
		/// <summary>
		/// 22.6 AssemblyRefOS : 0x25
		/// </summary>
		public static class AssemblyRefOS
		{
			/// <summary>
			/// 0. OSPlatformId : Int32
			/// </summary>
			public static readonly MdbColumn OSPlatformId = new MdbColumn(0, "OSPlatformId", MdbColumnType.Int32, "");

			/// <summary>
			/// 1. OSMajorVersion : Int32
			/// </summary>
			public static readonly MdbColumn OSMajorVersion = new MdbColumn(1, "OSMajorVersion", MdbColumnType.Int32, "");

			/// <summary>
			/// 2. OSMinorVersion : Int32
			/// </summary>
			public static readonly MdbColumn OSMinorVersion = new MdbColumn(2, "OSMinorVersion", MdbColumnType.Int32, "");

			/// <summary>
			/// 3. AssemblyRef : AssemblyRef
			/// </summary>
			public static readonly MdbColumn AssemblyRef = new MdbColumn(3, "AssemblyRef", MdbTableId.AssemblyRef, "");

			/// <summary>
			/// AssemblyRefOS Columns
			/// </summary>
			internal static readonly MdbColumn[] Columns = {OSPlatformId, OSMajorVersion, OSMinorVersion, AssemblyRef};
		}
		/// <summary>
		/// 22.7 AssemblyRefProcessor : 0x24
		/// </summary>
		public static class AssemblyRefProcessor
		{
			/// <summary>
			/// 0. Processor : Int32
			/// </summary>
			public static readonly MdbColumn Processor = new MdbColumn(0, "Processor", MdbColumnType.Int32, "");

			/// <summary>
			/// 1. AssemblyRef : AssemblyRef
			/// </summary>
			public static readonly MdbColumn AssemblyRef = new MdbColumn(1, "AssemblyRef", MdbTableId.AssemblyRef, "");

			/// <summary>
			/// AssemblyRefProcessor Columns
			/// </summary>
			internal static readonly MdbColumn[] Columns = {Processor, AssemblyRef};
		}
		/// <summary>
		/// 22.8 The ClassLayout table is used to define how the fields of a class or value type shall be laid out by the CLI.
		/// </summary>
		public static class ClassLayout
		{
			/// <summary>
			/// 0. PackingSize : Int16
			/// </summary>
			public static readonly MdbColumn PackingSize = new MdbColumn(0, "PackingSize", MdbColumnType.Int16, "");

			/// <summary>
			/// 1. ClassSize : Int32
			/// </summary>
			public static readonly MdbColumn ClassSize = new MdbColumn(1, "ClassSize", MdbColumnType.Int32, "");

			/// <summary>
			/// 2. Parent : TypeDef
			/// </summary>
			public static readonly MdbColumn Parent = new MdbColumn(2, "Parent", MdbTableId.TypeDef, "");

			/// <summary>
			/// ClassLayout Columns
			/// </summary>
			internal static readonly MdbColumn[] Columns = {PackingSize, ClassSize, Parent};
		}
		/// <summary>
		/// 22.9 The Constant table is used to store compile-time, constant values for fields, parameters, and properties.
		/// </summary>
		public static class Constant
		{
			/// <summary>
			/// 0. Type : Int16
			/// </summary>
			public static readonly MdbColumn Type = new MdbColumn(0, "Type", MdbColumnType.Int16, "");

			/// <summary>
			/// 1. Parent : HasConstant
			/// </summary>
			public static readonly MdbColumn Parent = new MdbColumn(1, "Parent", MdbCodedIndex.HasConstant, "");

			/// <summary>
			/// 2. Value : BlobIndex
			/// </summary>
			public static readonly MdbColumn Value = new MdbColumn(2, "Value", MdbColumnType.BlobIndex, "");

			/// <summary>
			/// Constant Columns
			/// </summary>
			internal static readonly MdbColumn[] Columns = {Type, Parent, Value};
		}
		/// <summary>
		/// 22.10 CustomAttribute : 0x0C
		/// </summary>
		public static class CustomAttribute
		{
			/// <summary>
			/// 0. Parent : HasCustomAttribute
			/// </summary>
			public static readonly MdbColumn Parent = new MdbColumn(0, "Parent", MdbCodedIndex.HasCustomAttribute, "");

			/// <summary>
			/// 1. Type : CustomAttributeType
			/// </summary>
			public static readonly MdbColumn Type = new MdbColumn(1, "Type", MdbCodedIndex.CustomAttributeType, "");

			/// <summary>
			/// 2. Value : BlobIndex
			/// </summary>
			public static readonly MdbColumn Value = new MdbColumn(2, "Value", MdbColumnType.BlobIndex, "");

			/// <summary>
			/// CustomAttribute Columns
			/// </summary>
			internal static readonly MdbColumn[] Columns = {Parent, Type, Value};
		}
		/// <summary>
		/// 22.11 DeclSecurity : 0x0E
		/// </summary>
		public static class DeclSecurity
		{
			/// <summary>
			/// 0. Action : Int16
			/// </summary>
			public static readonly MdbColumn Action = new MdbColumn(0, "Action", MdbColumnType.Int16, "");

			/// <summary>
			/// 1. Parent : HasDeclSecurity
			/// </summary>
			public static readonly MdbColumn Parent = new MdbColumn(1, "Parent", MdbCodedIndex.HasDeclSecurity, "");

			/// <summary>
			/// 2. PermissionSet : BlobIndex
			/// </summary>
			public static readonly MdbColumn PermissionSet = new MdbColumn(2, "PermissionSet", MdbColumnType.BlobIndex, "");

			/// <summary>
			/// DeclSecurity Columns
			/// </summary>
			internal static readonly MdbColumn[] Columns = {Action, Parent, PermissionSet};
		}
		/// <summary>
		/// 22.12 EventMap : 0x12
		/// </summary>
		public static class EventMap
		{
			/// <summary>
			/// 0. Parent : TypeDef
			/// </summary>
			public static readonly MdbColumn Parent = new MdbColumn(0, "Parent", MdbTableId.TypeDef, "");

			/// <summary>
			/// 1. EventList : Event
			/// </summary>
			public static readonly MdbColumn EventList = new MdbColumn(1, "EventList", MdbTableId.Event, "");

			/// <summary>
			/// EventMap Columns
			/// </summary>
			internal static readonly MdbColumn[] Columns = {Parent, EventList};
		}
		/// <summary>
		/// 22.13 Event : 0x14
		/// </summary>
		public static class Event
		{
			/// <summary>
			/// 0. EventFlags : EventAttributes
			/// </summary>
			public static readonly MdbColumn EventFlags = new MdbColumn(0, "EventFlags", MdbColumnType.Int16, typeof(EventAttributes), "");

			/// <summary>
			/// 1. Name : StringIndex
			/// </summary>
			public static readonly MdbColumn Name = new MdbColumn(1, "Name", MdbColumnType.StringIndex, "");

			/// <summary>
			/// 2. EventType : TypeDefOrRef
			/// </summary>
			public static readonly MdbColumn EventType = new MdbColumn(2, "EventType", MdbCodedIndex.TypeDefOrRef, "");

			/// <summary>
			/// Event Columns
			/// </summary>
			internal static readonly MdbColumn[] Columns = {EventFlags, Name, EventType};
		}
		/// <summary>
		/// 22.14 ExportedType : 0x27
		/// </summary>
		public static class ExportedType
		{
			/// <summary>
			/// 0. Flags : TypeAttributes
			/// </summary>
			public static readonly MdbColumn Flags = new MdbColumn(0, "Flags", MdbColumnType.Int32, typeof(TypeAttributes), "");

			/// <summary>
			/// 1. TypeDefId : Int32
			/// </summary>
			public static readonly MdbColumn TypeDefId = new MdbColumn(1, "TypeDefId", MdbColumnType.Int32, "");

			/// <summary>
			/// 2. TypeName : StringIndex
			/// </summary>
			public static readonly MdbColumn TypeName = new MdbColumn(2, "TypeName", MdbColumnType.StringIndex, "");

			/// <summary>
			/// 3. TypeNamespace : StringIndex
			/// </summary>
			public static readonly MdbColumn TypeNamespace = new MdbColumn(3, "TypeNamespace", MdbColumnType.StringIndex, "");

			/// <summary>
			/// 4. Implementation : Implementation
			/// </summary>
			public static readonly MdbColumn Implementation = new MdbColumn(4, "Implementation", MdbCodedIndex.Implementation, "");

			/// <summary>
			/// ExportedType Columns
			/// </summary>
			internal static readonly MdbColumn[] Columns = {Flags, TypeDefId, TypeName, TypeNamespace, Implementation};
		}
		/// <summary>
		/// 22.15 Field : 0x04
		/// </summary>
		public static class Field
		{
			/// <summary>
			/// 0. Flags : FieldAttributes
			/// </summary>
			public static readonly MdbColumn Flags = new MdbColumn(0, "Flags", MdbColumnType.Int16, typeof(FieldAttributes), "");

			/// <summary>
			/// 1. Name : StringIndex
			/// </summary>
			public static readonly MdbColumn Name = new MdbColumn(1, "Name", MdbColumnType.StringIndex, "");

			/// <summary>
			/// 2. Signature : BlobIndex
			/// </summary>
			public static readonly MdbColumn Signature = new MdbColumn(2, "Signature", MdbColumnType.BlobIndex, "");

			/// <summary>
			/// Field Columns
			/// </summary>
			internal static readonly MdbColumn[] Columns = {Flags, Name, Signature};
		}
		/// <summary>
		/// 22.16 FieldLayout : 0x10
		/// </summary>
		public static class FieldLayout
		{
			/// <summary>
			/// 0. Offset : Int32
			/// </summary>
			public static readonly MdbColumn Offset = new MdbColumn(0, "Offset", MdbColumnType.Int32, "");

			/// <summary>
			/// 1. Field : Field
			/// </summary>
			public static readonly MdbColumn Field = new MdbColumn(1, "Field", MdbTableId.Field, "");

			/// <summary>
			/// FieldLayout Columns
			/// </summary>
			internal static readonly MdbColumn[] Columns = {Offset, Field};
		}
		/// <summary>
		/// 22.17 FieldMarshal : 0x0D
		/// </summary>
		public static class FieldMarshal
		{
			/// <summary>
			/// 0. Parent : HasFieldMarshal
			/// </summary>
			public static readonly MdbColumn Parent = new MdbColumn(0, "Parent", MdbCodedIndex.HasFieldMarshal, "");

			/// <summary>
			/// 1. NativeType : BlobIndex
			/// </summary>
			public static readonly MdbColumn NativeType = new MdbColumn(1, "NativeType", MdbColumnType.BlobIndex, "");

			/// <summary>
			/// FieldMarshal Columns
			/// </summary>
			internal static readonly MdbColumn[] Columns = {Parent, NativeType};
		}
		/// <summary>
		/// 22.18 FieldRVA : 0x1D
		/// </summary>
		public static class FieldRVA
		{
			/// <summary>
			/// 0. RVA : Int32
			/// </summary>
			public static readonly MdbColumn RVA = new MdbColumn(0, "RVA", MdbColumnType.Int32, "");

			/// <summary>
			/// 1. Field : Field
			/// </summary>
			public static readonly MdbColumn Field = new MdbColumn(1, "Field", MdbTableId.Field, "");

			/// <summary>
			/// FieldRVA Columns
			/// </summary>
			internal static readonly MdbColumn[] Columns = {RVA, Field};
		}
		/// <summary>
		/// 22.19 File : 0x26
		/// </summary>
		public static class File
		{
			/// <summary>
			/// 0. Flags : FileFlags
			/// </summary>
			public static readonly MdbColumn Flags = new MdbColumn(0, "Flags", MdbColumnType.Int32, typeof(FileFlags), "");

			/// <summary>
			/// 1. Name : StringIndex
			/// </summary>
			public static readonly MdbColumn Name = new MdbColumn(1, "Name", MdbColumnType.StringIndex, "");

			/// <summary>
			/// 2. HashValue : BlobIndex
			/// </summary>
			public static readonly MdbColumn HashValue = new MdbColumn(2, "HashValue", MdbColumnType.BlobIndex, "");

			/// <summary>
			/// File Columns
			/// </summary>
			internal static readonly MdbColumn[] Columns = {Flags, Name, HashValue};
		}
		/// <summary>
		/// 22.20 GenericParam : 0x2A
		/// </summary>
		public static class GenericParam
		{
			/// <summary>
			/// 0. Number : Int16
			/// </summary>
			public static readonly MdbColumn Number = new MdbColumn(0, "Number", MdbColumnType.Int16, "");

			/// <summary>
			/// 1. Flags : GenericParamAttributes
			/// </summary>
			public static readonly MdbColumn Flags = new MdbColumn(1, "Flags", MdbColumnType.Int16, typeof(GenericParamAttributes), "");

			/// <summary>
			/// 2. Owner : TypeOrMethodDef
			/// </summary>
			public static readonly MdbColumn Owner = new MdbColumn(2, "Owner", MdbCodedIndex.TypeOrMethodDef, "");

			/// <summary>
			/// 3. Name : StringIndex
			/// </summary>
			public static readonly MdbColumn Name = new MdbColumn(3, "Name", MdbColumnType.StringIndex, "");

			/// <summary>
			/// GenericParam Columns
			/// </summary>
			internal static readonly MdbColumn[] Columns = {Number, Flags, Owner, Name};
		}
		/// <summary>
		/// 22.21 GenericParamConstraint : 0x2C
		/// </summary>
		public static class GenericParamConstraint
		{
			/// <summary>
			/// 0. Owner : GenericParam
			/// </summary>
			public static readonly MdbColumn Owner = new MdbColumn(0, "Owner", MdbTableId.GenericParam, "");

			/// <summary>
			/// 1. Constraint : TypeDefOrRef
			/// </summary>
			public static readonly MdbColumn Constraint = new MdbColumn(1, "Constraint", MdbCodedIndex.TypeDefOrRef, "");

			/// <summary>
			/// GenericParamConstraint Columns
			/// </summary>
			internal static readonly MdbColumn[] Columns = {Owner, Constraint};
		}
		/// <summary>
		/// 22.22 ImplMap : 0x1C
		/// </summary>
		public static class ImplMap
		{
			/// <summary>
			/// 0. MappingFlags : PInvokeAttributes
			/// </summary>
			public static readonly MdbColumn MappingFlags = new MdbColumn(0, "MappingFlags", MdbColumnType.Int16, typeof(PInvokeAttributes), "");

			/// <summary>
			/// 1. MemberForwarded : MemberForwarded
			/// </summary>
			public static readonly MdbColumn MemberForwarded = new MdbColumn(1, "MemberForwarded", MdbCodedIndex.MemberForwarded, "");

			/// <summary>
			/// 2. ImportName : StringIndex
			/// </summary>
			public static readonly MdbColumn ImportName = new MdbColumn(2, "ImportName", MdbColumnType.StringIndex, "");

			/// <summary>
			/// 3. ImportScope : ModuleRef
			/// </summary>
			public static readonly MdbColumn ImportScope = new MdbColumn(3, "ImportScope", MdbTableId.ModuleRef, "");

			/// <summary>
			/// ImplMap Columns
			/// </summary>
			internal static readonly MdbColumn[] Columns = {MappingFlags, MemberForwarded, ImportName, ImportScope};
		}
		/// <summary>
		/// 22.23 InterfaceImpl : 0x09
		/// </summary>
		public static class InterfaceImpl
		{
			/// <summary>
			/// 0. Class : TypeDef
			/// </summary>
			public static readonly MdbColumn Class = new MdbColumn(0, "Class", MdbTableId.TypeDef, "");

			/// <summary>
			/// 1. Interface : TypeDefOrRef
			/// </summary>
			public static readonly MdbColumn Interface = new MdbColumn(1, "Interface", MdbCodedIndex.TypeDefOrRef, "");

			/// <summary>
			/// InterfaceImpl Columns
			/// </summary>
			internal static readonly MdbColumn[] Columns = {Class, Interface};
		}
		/// <summary>
		/// 22.24 ManifestResource : 0x28
		/// </summary>
		public static class ManifestResource
		{
			/// <summary>
			/// 0. Offset : Int32
			/// </summary>
			public static readonly MdbColumn Offset = new MdbColumn(0, "Offset", MdbColumnType.Int32, "");

			/// <summary>
			/// 1. Flags : ManifestResourceAttributes
			/// </summary>
			public static readonly MdbColumn Flags = new MdbColumn(1, "Flags", MdbColumnType.Int32, typeof(ManifestResourceAttributes), "");

			/// <summary>
			/// 2. Name : StringIndex
			/// </summary>
			public static readonly MdbColumn Name = new MdbColumn(2, "Name", MdbColumnType.StringIndex, "");

			/// <summary>
			/// 3. Implementation : Implementation
			/// </summary>
			public static readonly MdbColumn Implementation = new MdbColumn(3, "Implementation", MdbCodedIndex.Implementation, "");

			/// <summary>
			/// ManifestResource Columns
			/// </summary>
			internal static readonly MdbColumn[] Columns = {Offset, Flags, Name, Implementation};
		}
		/// <summary>
		/// 22.25 The MemberRef table combines two sorts of references, to Methods and to Fields of a class, known as MethodRef and FieldRef, respectively.
		/// </summary>
		public static class MemberRef
		{
			/// <summary>
			/// 0. Class : MemberRefParent
			/// </summary>
			public static readonly MdbColumn Class = new MdbColumn(0, "Class", MdbCodedIndex.MemberRefParent, "");

			/// <summary>
			/// 1. Name : StringIndex
			/// </summary>
			public static readonly MdbColumn Name = new MdbColumn(1, "Name", MdbColumnType.StringIndex, "");

			/// <summary>
			/// 2. Signature : BlobIndex
			/// </summary>
			public static readonly MdbColumn Signature = new MdbColumn(2, "Signature", MdbColumnType.BlobIndex, "");

			/// <summary>
			/// MemberRef Columns
			/// </summary>
			internal static readonly MdbColumn[] Columns = {Class, Name, Signature};
		}
		/// <summary>
		/// 22.26 MethodDef : 0x06
		/// </summary>
		public static class MethodDef
		{
			/// <summary>
			/// 0. RVA : Int32
			/// </summary>
			public static readonly MdbColumn RVA = new MdbColumn(0, "RVA", MdbColumnType.Int32, "");

			/// <summary>
			/// 1. ImplFlags : MethodImplAttributes
			/// </summary>
			public static readonly MdbColumn ImplFlags = new MdbColumn(1, "ImplFlags", MdbColumnType.Int16, typeof(MethodImplAttributes), "");

			/// <summary>
			/// 2. Flags : MethodAttributes
			/// </summary>
			public static readonly MdbColumn Flags = new MdbColumn(2, "Flags", MdbColumnType.Int16, typeof(MethodAttributes), "");

			/// <summary>
			/// 3. Name : StringIndex
			/// </summary>
			public static readonly MdbColumn Name = new MdbColumn(3, "Name", MdbColumnType.StringIndex, "");

			/// <summary>
			/// 4. Signature : BlobIndex
			/// </summary>
			public static readonly MdbColumn Signature = new MdbColumn(4, "Signature", MdbColumnType.BlobIndex, "");

			/// <summary>
			/// 5. ParamList : Param
			/// </summary>
			public static readonly MdbColumn ParamList = new MdbColumn(5, "ParamList", MdbTableId.Param, "");

			/// <summary>
			/// MethodDef Columns
			/// </summary>
			internal static readonly MdbColumn[] Columns = {RVA, ImplFlags, Flags, Name, Signature, ParamList};
		}
		/// <summary>
		/// 22.27 MethodImpl tables let a compiler override the default inheritance rules provided by the CLI.
		/// </summary>
		public static class MethodImpl
		{
			/// <summary>
			/// 0. Class : TypeDef
			/// </summary>
			public static readonly MdbColumn Class = new MdbColumn(0, "Class", MdbTableId.TypeDef, "");

			/// <summary>
			/// 1. MethodBody : MethodDefOrRef
			/// </summary>
			public static readonly MdbColumn MethodBody = new MdbColumn(1, "MethodBody", MdbCodedIndex.MethodDefOrRef, "");

			/// <summary>
			/// 2. MethodDeclaration : MethodDefOrRef
			/// </summary>
			public static readonly MdbColumn MethodDeclaration = new MdbColumn(2, "MethodDeclaration", MdbCodedIndex.MethodDefOrRef, "");

			/// <summary>
			/// MethodImpl Columns
			/// </summary>
			internal static readonly MdbColumn[] Columns = {Class, MethodBody, MethodDeclaration};
		}
		/// <summary>
		/// 22.28 MethodSemantics : 0x18
		/// </summary>
		public static class MethodSemantics
		{
			/// <summary>
			/// 0. Semantics : MethodSemanticsAttributes
			/// </summary>
			public static readonly MdbColumn Semantics = new MdbColumn(0, "Semantics", MdbColumnType.Int16, typeof(MethodSemanticsAttributes), "");

			/// <summary>
			/// 1. Method : MethodDef
			/// </summary>
			public static readonly MdbColumn Method = new MdbColumn(1, "Method", MdbTableId.MethodDef, "");

			/// <summary>
			/// 2. Association : HasSemantics
			/// </summary>
			public static readonly MdbColumn Association = new MdbColumn(2, "Association", MdbCodedIndex.HasSemantics, "");

			/// <summary>
			/// MethodSemantics Columns
			/// </summary>
			internal static readonly MdbColumn[] Columns = {Semantics, Method, Association};
		}
		/// <summary>
		/// 22.29 MethodSpec : 0x2B
		/// </summary>
		public static class MethodSpec
		{
			/// <summary>
			/// 0. Method : MethodDefOrRef
			/// </summary>
			public static readonly MdbColumn Method = new MdbColumn(0, "Method", MdbCodedIndex.MethodDefOrRef, "");

			/// <summary>
			/// 1. Instantiation : BlobIndex
			/// </summary>
			public static readonly MdbColumn Instantiation = new MdbColumn(1, "Instantiation", MdbColumnType.BlobIndex, "");

			/// <summary>
			/// MethodSpec Columns
			/// </summary>
			internal static readonly MdbColumn[] Columns = {Method, Instantiation};
		}
		/// <summary>
		/// 22.30 Module : 0x00
		/// </summary>
		public static class Module
		{
			/// <summary>
			/// 0. Generation : Int16
			/// </summary>
			public static readonly MdbColumn Generation = new MdbColumn(0, "Generation", MdbColumnType.Int16, "");

			/// <summary>
			/// 1. Name : StringIndex
			/// </summary>
			public static readonly MdbColumn Name = new MdbColumn(1, "Name", MdbColumnType.StringIndex, "");

			/// <summary>
			/// 2. Mvid : GuidIndex
			/// </summary>
			public static readonly MdbColumn Mvid = new MdbColumn(2, "Mvid", MdbColumnType.GuidIndex, "");

			/// <summary>
			/// 3. EncId : GuidIndex
			/// </summary>
			public static readonly MdbColumn EncId = new MdbColumn(3, "EncId", MdbColumnType.GuidIndex, "");

			/// <summary>
			/// 4. EncBaseId : GuidIndex
			/// </summary>
			public static readonly MdbColumn EncBaseId = new MdbColumn(4, "EncBaseId", MdbColumnType.GuidIndex, "");

			/// <summary>
			/// Module Columns
			/// </summary>
			internal static readonly MdbColumn[] Columns = {Generation, Name, Mvid, EncId, EncBaseId};
		}
		/// <summary>
		/// 22.31 ModuleRef : 0x1A
		/// </summary>
		public static class ModuleRef
		{
			/// <summary>
			/// 0. Name : StringIndex
			/// </summary>
			public static readonly MdbColumn Name = new MdbColumn(0, "Name", MdbColumnType.StringIndex, "");

			/// <summary>
			/// ModuleRef Columns
			/// </summary>
			internal static readonly MdbColumn[] Columns = {Name};
		}
		/// <summary>
		/// 22.32 NestedClass : 0x29
		/// </summary>
		public static class NestedClass
		{
			/// <summary>
			/// 0. Class : TypeDef
			/// </summary>
			public static readonly MdbColumn Class = new MdbColumn(0, "Class", MdbTableId.TypeDef, "");

			/// <summary>
			/// 1. EnclosingClass : TypeDef
			/// </summary>
			public static readonly MdbColumn EnclosingClass = new MdbColumn(1, "EnclosingClass", MdbTableId.TypeDef, "");

			/// <summary>
			/// NestedClass Columns
			/// </summary>
			internal static readonly MdbColumn[] Columns = {Class, EnclosingClass};
		}
		/// <summary>
		/// 22.33 Param : 0x08
		/// </summary>
		public static class Param
		{
			/// <summary>
			/// 0. Flags : ParamAttributes
			/// </summary>
			public static readonly MdbColumn Flags = new MdbColumn(0, "Flags", MdbColumnType.Int16, typeof(ParamAttributes), "");

			/// <summary>
			/// 1. Sequence : Int16
			/// </summary>
			public static readonly MdbColumn Sequence = new MdbColumn(1, "Sequence", MdbColumnType.Int16, "");

			/// <summary>
			/// 2. Name : StringIndex
			/// </summary>
			public static readonly MdbColumn Name = new MdbColumn(2, "Name", MdbColumnType.StringIndex, "");

			/// <summary>
			/// Param Columns
			/// </summary>
			internal static readonly MdbColumn[] Columns = {Flags, Sequence, Name};
		}
		/// <summary>
		/// 22.34 Property : 0x17
		/// </summary>
		public static class Property
		{
			/// <summary>
			/// 0. Flags : PropertyAttributes
			/// </summary>
			public static readonly MdbColumn Flags = new MdbColumn(0, "Flags", MdbColumnType.Int16, typeof(PropertyAttributes), "");

			/// <summary>
			/// 1. Name : StringIndex
			/// </summary>
			public static readonly MdbColumn Name = new MdbColumn(1, "Name", MdbColumnType.StringIndex, "");

			/// <summary>
			/// 2. Type : BlobIndex
			/// </summary>
			public static readonly MdbColumn Type = new MdbColumn(2, "Type", MdbColumnType.BlobIndex, "");

			/// <summary>
			/// Property Columns
			/// </summary>
			internal static readonly MdbColumn[] Columns = {Flags, Name, Type};
		}
		/// <summary>
		/// 22.35 PropertyMap : 0x15
		/// </summary>
		public static class PropertyMap
		{
			/// <summary>
			/// 0. Parent : TypeDef
			/// </summary>
			public static readonly MdbColumn Parent = new MdbColumn(0, "Parent", MdbTableId.TypeDef, "");

			/// <summary>
			/// 1. PropertyList : Property
			/// </summary>
			public static readonly MdbColumn PropertyList = new MdbColumn(1, "PropertyList", MdbTableId.Property, "");

			/// <summary>
			/// PropertyMap Columns
			/// </summary>
			internal static readonly MdbColumn[] Columns = {Parent, PropertyList};
		}
		/// <summary>
		/// 22.36 StandAloneSig : 0x11
		/// </summary>
		public static class StandAloneSig
		{
			/// <summary>
			/// 0. Signature : BlobIndex
			/// </summary>
			public static readonly MdbColumn Signature = new MdbColumn(0, "Signature", MdbColumnType.BlobIndex, "");

			/// <summary>
			/// StandAloneSig Columns
			/// </summary>
			internal static readonly MdbColumn[] Columns = {Signature};
		}
		/// <summary>
		/// 22.37 TypeDef : 0x02
		/// </summary>
		public static class TypeDef
		{
			/// <summary>
			/// 0. Flags : TypeAttributes
			/// </summary>
			public static readonly MdbColumn Flags = new MdbColumn(0, "Flags", MdbColumnType.Int32, typeof(TypeAttributes), "");

			/// <summary>
			/// 1. TypeName : StringIndex
			/// </summary>
			public static readonly MdbColumn TypeName = new MdbColumn(1, "TypeName", MdbColumnType.StringIndex, "");

			/// <summary>
			/// 2. TypeNamespace : StringIndex
			/// </summary>
			public static readonly MdbColumn TypeNamespace = new MdbColumn(2, "TypeNamespace", MdbColumnType.StringIndex, "");

			/// <summary>
			/// 3. Extends : TypeDefOrRef
			/// </summary>
			public static readonly MdbColumn Extends = new MdbColumn(3, "Extends", MdbCodedIndex.TypeDefOrRef, "");

			/// <summary>
			/// 4. FieldList : Field
			/// </summary>
			public static readonly MdbColumn FieldList = new MdbColumn(4, "FieldList", MdbTableId.Field, "");

			/// <summary>
			/// 5. MethodList : MethodDef
			/// </summary>
			public static readonly MdbColumn MethodList = new MdbColumn(5, "MethodList", MdbTableId.MethodDef, "");

			/// <summary>
			/// TypeDef Columns
			/// </summary>
			internal static readonly MdbColumn[] Columns = {Flags, TypeName, TypeNamespace, Extends, FieldList, MethodList};
		}
		/// <summary>
		/// 22.38 TypeRef : 0x01
		/// </summary>
		public static class TypeRef
		{
			/// <summary>
			/// 0. ResolutionScope : ResolutionScope
			/// </summary>
			public static readonly MdbColumn ResolutionScope = new MdbColumn(0, "ResolutionScope", MdbCodedIndex.ResolutionScope, "");

			/// <summary>
			/// 1. TypeName : StringIndex
			/// </summary>
			public static readonly MdbColumn TypeName = new MdbColumn(1, "TypeName", MdbColumnType.StringIndex, "");

			/// <summary>
			/// 2. TypeNamespace : StringIndex
			/// </summary>
			public static readonly MdbColumn TypeNamespace = new MdbColumn(2, "TypeNamespace", MdbColumnType.StringIndex, "");

			/// <summary>
			/// TypeRef Columns
			/// </summary>
			internal static readonly MdbColumn[] Columns = {ResolutionScope, TypeName, TypeNamespace};
		}
		/// <summary>
		/// 22.39 TypeSpec : 0x1B
		/// </summary>
		public static class TypeSpec
		{
			/// <summary>
			/// 0. Signature : BlobIndex
			/// </summary>
			public static readonly MdbColumn Signature = new MdbColumn(0, "Signature", MdbColumnType.BlobIndex, "");

			/// <summary>
			/// TypeSpec Columns
			/// </summary>
			internal static readonly MdbColumn[] Columns = {Signature};
		}
		/// <summary>
		/// FieldPtr : 3
		/// </summary>
		public static class FieldPtr
		{
			/// <summary>
			/// 0. Field : Field
			/// </summary>
			public static readonly MdbColumn Field = new MdbColumn(0, "Field", MdbTableId.Field, "");

			/// <summary>
			/// FieldPtr Columns
			/// </summary>
			internal static readonly MdbColumn[] Columns = {Field};
		}
		/// <summary>
		/// MethodPtr : 5
		/// </summary>
		public static class MethodPtr
		{
			/// <summary>
			/// 0. Method : MethodDef
			/// </summary>
			public static readonly MdbColumn Method = new MdbColumn(0, "Method", MdbTableId.MethodDef, "");

			/// <summary>
			/// MethodPtr Columns
			/// </summary>
			internal static readonly MdbColumn[] Columns = {Method};
		}
		/// <summary>
		/// ParamPtr : 7
		/// </summary>
		public static class ParamPtr
		{
			/// <summary>
			/// 0. Param : Param
			/// </summary>
			public static readonly MdbColumn Param = new MdbColumn(0, "Param", MdbTableId.Param, "");

			/// <summary>
			/// ParamPtr Columns
			/// </summary>
			internal static readonly MdbColumn[] Columns = {Param};
		}
		/// <summary>
		/// EventPtr : 19
		/// </summary>
		public static class EventPtr
		{
			/// <summary>
			/// 0. Event : Event
			/// </summary>
			public static readonly MdbColumn Event = new MdbColumn(0, "Event", MdbTableId.Event, "");

			/// <summary>
			/// EventPtr Columns
			/// </summary>
			internal static readonly MdbColumn[] Columns = {Event};
		}
		/// <summary>
		/// PropertyPtr : 22
		/// </summary>
		public static class PropertyPtr
		{
			/// <summary>
			/// 0. Property : Property
			/// </summary>
			public static readonly MdbColumn Property = new MdbColumn(0, "Property", MdbTableId.Property, "");

			/// <summary>
			/// PropertyPtr Columns
			/// </summary>
			internal static readonly MdbColumn[] Columns = {Property};
		}
		/// <summary>
		/// EncodingLog : 30
		/// </summary>
		public static class EncodingLog
		{
			/// <summary>
			/// 0. Token : Int32
			/// </summary>
			public static readonly MdbColumn Token = new MdbColumn(0, "Token", MdbColumnType.Int32, "");

			/// <summary>
			/// 1. FuncCode : Int32
			/// </summary>
			public static readonly MdbColumn FuncCode = new MdbColumn(1, "FuncCode", MdbColumnType.Int32, "");

			/// <summary>
			/// EncodingLog Columns
			/// </summary>
			internal static readonly MdbColumn[] Columns = {Token, FuncCode};
		}
		/// <summary>
		/// EncodingMap : 31
		/// </summary>
		public static class EncodingMap
		{
			/// <summary>
			/// 0. Token : Int32
			/// </summary>
			public static readonly MdbColumn Token = new MdbColumn(0, "Token", MdbColumnType.Int32, "");

			/// <summary>
			/// EncodingMap Columns
			/// </summary>
			internal static readonly MdbColumn[] Columns = {Token};
		}
		#endregion
	}
}
