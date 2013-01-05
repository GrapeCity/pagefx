using System;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Ecma335.Metadata
{
	/// <summary>
	/// Contains MDB Table Schemas
	/// </summary>
	internal static class Schema
	{
		#region CreateTable
		internal static MetadataTable CreateTable(TableId id)
		{
			switch (id)
			{
				case TableId.Assembly: return new MetadataTable(id, Assembly.Columns);
				case TableId.AssemblyOS: return new MetadataTable(id, AssemblyOS.Columns);
				case TableId.AssemblyProcessor: return new MetadataTable(id, AssemblyProcessor.Columns);
				case TableId.AssemblyRef: return new MetadataTable(id, AssemblyRef.Columns);
				case TableId.AssemblyRefOS: return new MetadataTable(id, AssemblyRefOS.Columns);
				case TableId.AssemblyRefProcessor: return new MetadataTable(id, AssemblyRefProcessor.Columns);
				case TableId.ClassLayout: return new MetadataTable(id, ClassLayout.Columns);
				case TableId.Constant: return new MetadataTable(id, Constant.Columns);
				case TableId.CustomAttribute: return new MetadataTable(id, CustomAttribute.Columns);
				case TableId.DeclSecurity: return new MetadataTable(id, DeclSecurity.Columns);
				case TableId.EventMap: return new MetadataTable(id, EventMap.Columns);
				case TableId.Event: return new MetadataTable(id, Event.Columns);
				case TableId.ExportedType: return new MetadataTable(id, ExportedType.Columns);
				case TableId.Field: return new MetadataTable(id, Field.Columns);
				case TableId.FieldLayout: return new MetadataTable(id, FieldLayout.Columns);
				case TableId.FieldMarshal: return new MetadataTable(id, FieldMarshal.Columns);
				case TableId.FieldRVA: return new MetadataTable(id, FieldRVA.Columns);
				case TableId.File: return new MetadataTable(id, File.Columns);
				case TableId.GenericParam: return new MetadataTable(id, GenericParam.Columns);
				case TableId.GenericParamConstraint: return new MetadataTable(id, GenericParamConstraint.Columns);
				case TableId.ImplMap: return new MetadataTable(id, ImplMap.Columns);
				case TableId.InterfaceImpl: return new MetadataTable(id, InterfaceImpl.Columns);
				case TableId.ManifestResource: return new MetadataTable(id, ManifestResource.Columns);
				case TableId.MemberRef: return new MetadataTable(id, MemberRef.Columns);
				case TableId.MethodDef: return new MetadataTable(id, MethodDef.Columns);
				case TableId.MethodImpl: return new MetadataTable(id, MethodImpl.Columns);
				case TableId.MethodSemantics: return new MetadataTable(id, MethodSemantics.Columns);
				case TableId.MethodSpec: return new MetadataTable(id, MethodSpec.Columns);
				case TableId.Module: return new MetadataTable(id, Module.Columns);
				case TableId.ModuleRef: return new MetadataTable(id, ModuleRef.Columns);
				case TableId.NestedClass: return new MetadataTable(id, NestedClass.Columns);
				case TableId.Param: return new MetadataTable(id, Param.Columns);
				case TableId.Property: return new MetadataTable(id, Property.Columns);
				case TableId.PropertyMap: return new MetadataTable(id, PropertyMap.Columns);
				case TableId.StandAloneSig: return new MetadataTable(id, StandAloneSig.Columns);
				case TableId.TypeDef: return new MetadataTable(id, TypeDef.Columns);
				case TableId.TypeRef: return new MetadataTable(id, TypeRef.Columns);
				case TableId.TypeSpec: return new MetadataTable(id, TypeSpec.Columns);
				case TableId.FieldPtr: return new MetadataTable(id, FieldPtr.Columns);
				case TableId.MethodPtr: return new MetadataTable(id, MethodPtr.Columns);
				case TableId.ParamPtr: return new MetadataTable(id, ParamPtr.Columns);
				case TableId.EventPtr: return new MetadataTable(id, EventPtr.Columns);
				case TableId.PropertyPtr: return new MetadataTable(id, PropertyPtr.Columns);
				case TableId.EncodingLog: return new MetadataTable(id, EncodingLog.Columns);
				case TableId.EncodingMap: return new MetadataTable(id, EncodingMap.Columns);
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
			public static readonly MetadataColumn HashAlgId = new MetadataColumn(0, "HashAlgId", ColumnType.Int32, typeof(HashAlgorithmId), "");

			/// <summary>
			/// 1. MajorVersion : Int16
			/// </summary>
			public static readonly MetadataColumn MajorVersion = new MetadataColumn(1, "MajorVersion", ColumnType.Int16, "");

			/// <summary>
			/// 2. MinorVersion : Int16
			/// </summary>
			public static readonly MetadataColumn MinorVersion = new MetadataColumn(2, "MinorVersion", ColumnType.Int16, "");

			/// <summary>
			/// 3. BuildNumber : Int16
			/// </summary>
			public static readonly MetadataColumn BuildNumber = new MetadataColumn(3, "BuildNumber", ColumnType.Int16, "");

			/// <summary>
			/// 4. RevisionNumber : Int16
			/// </summary>
			public static readonly MetadataColumn RevisionNumber = new MetadataColumn(4, "RevisionNumber", ColumnType.Int16, "");

			/// <summary>
			/// 5. Flags : AssemblyFlags
			/// </summary>
			public static readonly MetadataColumn Flags = new MetadataColumn(5, "Flags", ColumnType.Int32, typeof(AssemblyFlags), "");

			/// <summary>
			/// 6. PublicKey : BlobIndex
			/// </summary>
			public static readonly MetadataColumn PublicKey = new MetadataColumn(6, "PublicKey", ColumnType.BlobIndex, "");

			/// <summary>
			/// 7. Name : StringIndex
			/// </summary>
			public static readonly MetadataColumn Name = new MetadataColumn(7, "Name", ColumnType.StringIndex, "");

			/// <summary>
			/// 8. Culture : StringIndex
			/// </summary>
			public static readonly MetadataColumn Culture = new MetadataColumn(8, "Culture", ColumnType.StringIndex, "");

			/// <summary>
			/// Assembly Columns
			/// </summary>
			internal static readonly MetadataColumn[] Columns = {HashAlgId, MajorVersion, MinorVersion, BuildNumber, RevisionNumber, Flags, PublicKey, Name, Culture};
		}
		/// <summary>
		/// 22.3 AssemblyOS : 0x22
		/// </summary>
		public static class AssemblyOS
		{
			/// <summary>
			/// 0. OSPlatformId : Int32
			/// </summary>
			public static readonly MetadataColumn OSPlatformId = new MetadataColumn(0, "OSPlatformId", ColumnType.Int32, "");

			/// <summary>
			/// 1. OSMajorVersion : Int32
			/// </summary>
			public static readonly MetadataColumn OSMajorVersion = new MetadataColumn(1, "OSMajorVersion", ColumnType.Int32, "");

			/// <summary>
			/// 2. OSMinorVersion : Int32
			/// </summary>
			public static readonly MetadataColumn OSMinorVersion = new MetadataColumn(2, "OSMinorVersion", ColumnType.Int32, "");

			/// <summary>
			/// AssemblyOS Columns
			/// </summary>
			internal static readonly MetadataColumn[] Columns = {OSPlatformId, OSMajorVersion, OSMinorVersion};
		}
		/// <summary>
		/// 22.4 AssemblyProcessor : 0x21
		/// </summary>
		public static class AssemblyProcessor
		{
			/// <summary>
			/// 0. Processor : Int32
			/// </summary>
			public static readonly MetadataColumn Processor = new MetadataColumn(0, "Processor", ColumnType.Int32, "");

			/// <summary>
			/// AssemblyProcessor Columns
			/// </summary>
			internal static readonly MetadataColumn[] Columns = {Processor};
		}
		/// <summary>
		/// 22.5 AssemblyRef : 0x23
		/// </summary>
		public static class AssemblyRef
		{
			/// <summary>
			/// 0. MajorVersion : Int16
			/// </summary>
			public static readonly MetadataColumn MajorVersion = new MetadataColumn(0, "MajorVersion", ColumnType.Int16, "");

			/// <summary>
			/// 1. MinorVersion : Int16
			/// </summary>
			public static readonly MetadataColumn MinorVersion = new MetadataColumn(1, "MinorVersion", ColumnType.Int16, "");

			/// <summary>
			/// 2. BuildNumber : Int16
			/// </summary>
			public static readonly MetadataColumn BuildNumber = new MetadataColumn(2, "BuildNumber", ColumnType.Int16, "");

			/// <summary>
			/// 3. RevisionNumber : Int16
			/// </summary>
			public static readonly MetadataColumn RevisionNumber = new MetadataColumn(3, "RevisionNumber", ColumnType.Int16, "");

			/// <summary>
			/// 4. Flags : AssemblyFlags
			/// </summary>
			public static readonly MetadataColumn Flags = new MetadataColumn(4, "Flags", ColumnType.Int32, typeof(AssemblyFlags), "");

			/// <summary>
			/// 5. PublicKeyOrToken : BlobIndex
			/// </summary>
			public static readonly MetadataColumn PublicKeyOrToken = new MetadataColumn(5, "PublicKeyOrToken", ColumnType.BlobIndex, "");

			/// <summary>
			/// 6. Name : StringIndex
			/// </summary>
			public static readonly MetadataColumn Name = new MetadataColumn(6, "Name", ColumnType.StringIndex, "");

			/// <summary>
			/// 7. Culture : StringIndex
			/// </summary>
			public static readonly MetadataColumn Culture = new MetadataColumn(7, "Culture", ColumnType.StringIndex, "");

			/// <summary>
			/// 8. HashValue : BlobIndex
			/// </summary>
			public static readonly MetadataColumn HashValue = new MetadataColumn(8, "HashValue", ColumnType.BlobIndex, "");

			/// <summary>
			/// AssemblyRef Columns
			/// </summary>
			internal static readonly MetadataColumn[] Columns = {MajorVersion, MinorVersion, BuildNumber, RevisionNumber, Flags, PublicKeyOrToken, Name, Culture, HashValue};
		}
		/// <summary>
		/// 22.6 AssemblyRefOS : 0x25
		/// </summary>
		public static class AssemblyRefOS
		{
			/// <summary>
			/// 0. OSPlatformId : Int32
			/// </summary>
			public static readonly MetadataColumn OSPlatformId = new MetadataColumn(0, "OSPlatformId", ColumnType.Int32, "");

			/// <summary>
			/// 1. OSMajorVersion : Int32
			/// </summary>
			public static readonly MetadataColumn OSMajorVersion = new MetadataColumn(1, "OSMajorVersion", ColumnType.Int32, "");

			/// <summary>
			/// 2. OSMinorVersion : Int32
			/// </summary>
			public static readonly MetadataColumn OSMinorVersion = new MetadataColumn(2, "OSMinorVersion", ColumnType.Int32, "");

			/// <summary>
			/// 3. AssemblyRef : AssemblyRef
			/// </summary>
			public static readonly MetadataColumn AssemblyRef = new MetadataColumn(3, "AssemblyRef", TableId.AssemblyRef, "");

			/// <summary>
			/// AssemblyRefOS Columns
			/// </summary>
			internal static readonly MetadataColumn[] Columns = {OSPlatformId, OSMajorVersion, OSMinorVersion, AssemblyRef};
		}
		/// <summary>
		/// 22.7 AssemblyRefProcessor : 0x24
		/// </summary>
		public static class AssemblyRefProcessor
		{
			/// <summary>
			/// 0. Processor : Int32
			/// </summary>
			public static readonly MetadataColumn Processor = new MetadataColumn(0, "Processor", ColumnType.Int32, "");

			/// <summary>
			/// 1. AssemblyRef : AssemblyRef
			/// </summary>
			public static readonly MetadataColumn AssemblyRef = new MetadataColumn(1, "AssemblyRef", TableId.AssemblyRef, "");

			/// <summary>
			/// AssemblyRefProcessor Columns
			/// </summary>
			internal static readonly MetadataColumn[] Columns = {Processor, AssemblyRef};
		}
		/// <summary>
		/// 22.8 The ClassLayout table is used to define how the fields of a class or value type shall be laid out by the CLI.
		/// </summary>
		public static class ClassLayout
		{
			/// <summary>
			/// 0. PackingSize : Int16
			/// </summary>
			public static readonly MetadataColumn PackingSize = new MetadataColumn(0, "PackingSize", ColumnType.Int16, "");

			/// <summary>
			/// 1. ClassSize : Int32
			/// </summary>
			public static readonly MetadataColumn ClassSize = new MetadataColumn(1, "ClassSize", ColumnType.Int32, "");

			/// <summary>
			/// 2. Parent : TypeDef
			/// </summary>
			public static readonly MetadataColumn Parent = new MetadataColumn(2, "Parent", TableId.TypeDef, "");

			/// <summary>
			/// ClassLayout Columns
			/// </summary>
			internal static readonly MetadataColumn[] Columns = {PackingSize, ClassSize, Parent};
		}
		/// <summary>
		/// 22.9 The Constant table is used to store compile-time, constant values for fields, parameters, and properties.
		/// </summary>
		public static class Constant
		{
			/// <summary>
			/// 0. Type : Int16
			/// </summary>
			public static readonly MetadataColumn Type = new MetadataColumn(0, "Type", ColumnType.Int16, "");

			/// <summary>
			/// 1. Parent : HasConstant
			/// </summary>
			public static readonly MetadataColumn Parent = new MetadataColumn(1, "Parent", CodedIndex.HasConstant, "");

			/// <summary>
			/// 2. Value : BlobIndex
			/// </summary>
			public static readonly MetadataColumn Value = new MetadataColumn(2, "Value", ColumnType.BlobIndex, "");

			/// <summary>
			/// Constant Columns
			/// </summary>
			internal static readonly MetadataColumn[] Columns = {Type, Parent, Value};
		}
		/// <summary>
		/// 22.10 CustomAttribute : 0x0C
		/// </summary>
		public static class CustomAttribute
		{
			/// <summary>
			/// 0. Parent : HasCustomAttribute
			/// </summary>
			public static readonly MetadataColumn Parent = new MetadataColumn(0, "Parent", CodedIndex.HasCustomAttribute, "");

			/// <summary>
			/// 1. Type : CustomAttributeType
			/// </summary>
			public static readonly MetadataColumn Type = new MetadataColumn(1, "Type", CodedIndex.CustomAttributeType, "");

			/// <summary>
			/// 2. Value : BlobIndex
			/// </summary>
			public static readonly MetadataColumn Value = new MetadataColumn(2, "Value", ColumnType.BlobIndex, "");

			/// <summary>
			/// CustomAttribute Columns
			/// </summary>
			internal static readonly MetadataColumn[] Columns = {Parent, Type, Value};
		}
		/// <summary>
		/// 22.11 DeclSecurity : 0x0E
		/// </summary>
		public static class DeclSecurity
		{
			/// <summary>
			/// 0. Action : Int16
			/// </summary>
			public static readonly MetadataColumn Action = new MetadataColumn(0, "Action", ColumnType.Int16, "");

			/// <summary>
			/// 1. Parent : HasDeclSecurity
			/// </summary>
			public static readonly MetadataColumn Parent = new MetadataColumn(1, "Parent", CodedIndex.HasDeclSecurity, "");

			/// <summary>
			/// 2. PermissionSet : BlobIndex
			/// </summary>
			public static readonly MetadataColumn PermissionSet = new MetadataColumn(2, "PermissionSet", ColumnType.BlobIndex, "");

			/// <summary>
			/// DeclSecurity Columns
			/// </summary>
			internal static readonly MetadataColumn[] Columns = {Action, Parent, PermissionSet};
		}
		/// <summary>
		/// 22.12 EventMap : 0x12
		/// </summary>
		public static class EventMap
		{
			/// <summary>
			/// 0. Parent : TypeDef
			/// </summary>
			public static readonly MetadataColumn Parent = new MetadataColumn(0, "Parent", TableId.TypeDef, "");

			/// <summary>
			/// 1. EventList : Event
			/// </summary>
			public static readonly MetadataColumn EventList = new MetadataColumn(1, "EventList", TableId.Event, "");

			/// <summary>
			/// EventMap Columns
			/// </summary>
			internal static readonly MetadataColumn[] Columns = {Parent, EventList};
		}
		/// <summary>
		/// 22.13 Event : 0x14
		/// </summary>
		public static class Event
		{
			/// <summary>
			/// 0. EventFlags : EventAttributes
			/// </summary>
			public static readonly MetadataColumn EventFlags = new MetadataColumn(0, "EventFlags", ColumnType.Int16, typeof(EventAttributes), "");

			/// <summary>
			/// 1. Name : StringIndex
			/// </summary>
			public static readonly MetadataColumn Name = new MetadataColumn(1, "Name", ColumnType.StringIndex, "");

			/// <summary>
			/// 2. EventType : TypeDefOrRef
			/// </summary>
			public static readonly MetadataColumn EventType = new MetadataColumn(2, "EventType", CodedIndex.TypeDefOrRef, "");

			/// <summary>
			/// Event Columns
			/// </summary>
			internal static readonly MetadataColumn[] Columns = {EventFlags, Name, EventType};
		}
		/// <summary>
		/// 22.14 ExportedType : 0x27
		/// </summary>
		public static class ExportedType
		{
			/// <summary>
			/// 0. Flags : TypeAttributes
			/// </summary>
			public static readonly MetadataColumn Flags = new MetadataColumn(0, "Flags", ColumnType.Int32, typeof(TypeAttributes), "");

			/// <summary>
			/// 1. TypeDefId : Int32
			/// </summary>
			public static readonly MetadataColumn TypeDefId = new MetadataColumn(1, "TypeDefId", ColumnType.Int32, "");

			/// <summary>
			/// 2. TypeName : StringIndex
			/// </summary>
			public static readonly MetadataColumn TypeName = new MetadataColumn(2, "TypeName", ColumnType.StringIndex, "");

			/// <summary>
			/// 3. TypeNamespace : StringIndex
			/// </summary>
			public static readonly MetadataColumn TypeNamespace = new MetadataColumn(3, "TypeNamespace", ColumnType.StringIndex, "");

			/// <summary>
			/// 4. Implementation : Implementation
			/// </summary>
			public static readonly MetadataColumn Implementation = new MetadataColumn(4, "Implementation", CodedIndex.Implementation, "");

			/// <summary>
			/// ExportedType Columns
			/// </summary>
			internal static readonly MetadataColumn[] Columns = {Flags, TypeDefId, TypeName, TypeNamespace, Implementation};
		}
		/// <summary>
		/// 22.15 Field : 0x04
		/// </summary>
		public static class Field
		{
			/// <summary>
			/// 0. Flags : FieldAttributes
			/// </summary>
			public static readonly MetadataColumn Flags = new MetadataColumn(0, "Flags", ColumnType.Int16, typeof(FieldAttributes), "");

			/// <summary>
			/// 1. Name : StringIndex
			/// </summary>
			public static readonly MetadataColumn Name = new MetadataColumn(1, "Name", ColumnType.StringIndex, "");

			/// <summary>
			/// 2. Signature : BlobIndex
			/// </summary>
			public static readonly MetadataColumn Signature = new MetadataColumn(2, "Signature", ColumnType.BlobIndex, "");

			/// <summary>
			/// Field Columns
			/// </summary>
			internal static readonly MetadataColumn[] Columns = {Flags, Name, Signature};
		}
		/// <summary>
		/// 22.16 FieldLayout : 0x10
		/// </summary>
		public static class FieldLayout
		{
			/// <summary>
			/// 0. Offset : Int32
			/// </summary>
			public static readonly MetadataColumn Offset = new MetadataColumn(0, "Offset", ColumnType.Int32, "");

			/// <summary>
			/// 1. Field : Field
			/// </summary>
			public static readonly MetadataColumn Field = new MetadataColumn(1, "Field", TableId.Field, "");

			/// <summary>
			/// FieldLayout Columns
			/// </summary>
			internal static readonly MetadataColumn[] Columns = {Offset, Field};
		}
		/// <summary>
		/// 22.17 FieldMarshal : 0x0D
		/// </summary>
		public static class FieldMarshal
		{
			/// <summary>
			/// 0. Parent : HasFieldMarshal
			/// </summary>
			public static readonly MetadataColumn Parent = new MetadataColumn(0, "Parent", CodedIndex.HasFieldMarshal, "");

			/// <summary>
			/// 1. NativeType : BlobIndex
			/// </summary>
			public static readonly MetadataColumn NativeType = new MetadataColumn(1, "NativeType", ColumnType.BlobIndex, "");

			/// <summary>
			/// FieldMarshal Columns
			/// </summary>
			internal static readonly MetadataColumn[] Columns = {Parent, NativeType};
		}
		/// <summary>
		/// 22.18 FieldRVA : 0x1D
		/// </summary>
		public static class FieldRVA
		{
			/// <summary>
			/// 0. RVA : Int32
			/// </summary>
			public static readonly MetadataColumn RVA = new MetadataColumn(0, "RVA", ColumnType.Int32, "");

			/// <summary>
			/// 1. Field : Field
			/// </summary>
			public static readonly MetadataColumn Field = new MetadataColumn(1, "Field", TableId.Field, "");

			/// <summary>
			/// FieldRVA Columns
			/// </summary>
			internal static readonly MetadataColumn[] Columns = {RVA, Field};
		}
		/// <summary>
		/// 22.19 File : 0x26
		/// </summary>
		public static class File
		{
			/// <summary>
			/// 0. Flags : FileFlags
			/// </summary>
			public static readonly MetadataColumn Flags = new MetadataColumn(0, "Flags", ColumnType.Int32, typeof(FileFlags), "");

			/// <summary>
			/// 1. Name : StringIndex
			/// </summary>
			public static readonly MetadataColumn Name = new MetadataColumn(1, "Name", ColumnType.StringIndex, "");

			/// <summary>
			/// 2. HashValue : BlobIndex
			/// </summary>
			public static readonly MetadataColumn HashValue = new MetadataColumn(2, "HashValue", ColumnType.BlobIndex, "");

			/// <summary>
			/// File Columns
			/// </summary>
			internal static readonly MetadataColumn[] Columns = {Flags, Name, HashValue};
		}
		/// <summary>
		/// 22.20 GenericParam : 0x2A
		/// </summary>
		public static class GenericParam
		{
			/// <summary>
			/// 0. Number : Int16
			/// </summary>
			public static readonly MetadataColumn Number = new MetadataColumn(0, "Number", ColumnType.Int16, "");

			/// <summary>
			/// 1. Flags : GenericParamAttributes
			/// </summary>
			public static readonly MetadataColumn Flags = new MetadataColumn(1, "Flags", ColumnType.Int16, typeof(GenericParamAttributes), "");

			/// <summary>
			/// 2. Owner : TypeOrMethodDef
			/// </summary>
			public static readonly MetadataColumn Owner = new MetadataColumn(2, "Owner", CodedIndex.TypeOrMethodDef, "");

			/// <summary>
			/// 3. Name : StringIndex
			/// </summary>
			public static readonly MetadataColumn Name = new MetadataColumn(3, "Name", ColumnType.StringIndex, "");

			/// <summary>
			/// GenericParam Columns
			/// </summary>
			internal static readonly MetadataColumn[] Columns = {Number, Flags, Owner, Name};
		}
		/// <summary>
		/// 22.21 GenericParamConstraint : 0x2C
		/// </summary>
		public static class GenericParamConstraint
		{
			/// <summary>
			/// 0. Owner : GenericParam
			/// </summary>
			public static readonly MetadataColumn Owner = new MetadataColumn(0, "Owner", TableId.GenericParam, "");

			/// <summary>
			/// 1. Constraint : TypeDefOrRef
			/// </summary>
			public static readonly MetadataColumn Constraint = new MetadataColumn(1, "Constraint", CodedIndex.TypeDefOrRef, "");

			/// <summary>
			/// GenericParamConstraint Columns
			/// </summary>
			internal static readonly MetadataColumn[] Columns = {Owner, Constraint};
		}
		/// <summary>
		/// 22.22 ImplMap : 0x1C
		/// </summary>
		public static class ImplMap
		{
			/// <summary>
			/// 0. MappingFlags : PInvokeAttributes
			/// </summary>
			public static readonly MetadataColumn MappingFlags = new MetadataColumn(0, "MappingFlags", ColumnType.Int16, typeof(PInvokeAttributes), "");

			/// <summary>
			/// 1. MemberForwarded : MemberForwarded
			/// </summary>
			public static readonly MetadataColumn MemberForwarded = new MetadataColumn(1, "MemberForwarded", CodedIndex.MemberForwarded, "");

			/// <summary>
			/// 2. ImportName : StringIndex
			/// </summary>
			public static readonly MetadataColumn ImportName = new MetadataColumn(2, "ImportName", ColumnType.StringIndex, "");

			/// <summary>
			/// 3. ImportScope : ModuleRef
			/// </summary>
			public static readonly MetadataColumn ImportScope = new MetadataColumn(3, "ImportScope", TableId.ModuleRef, "");

			/// <summary>
			/// ImplMap Columns
			/// </summary>
			internal static readonly MetadataColumn[] Columns = {MappingFlags, MemberForwarded, ImportName, ImportScope};
		}
		/// <summary>
		/// 22.23 InterfaceImpl : 0x09
		/// </summary>
		public static class InterfaceImpl
		{
			/// <summary>
			/// 0. Class : TypeDef
			/// </summary>
			public static readonly MetadataColumn Class = new MetadataColumn(0, "Class", TableId.TypeDef, "");

			/// <summary>
			/// 1. Interface : TypeDefOrRef
			/// </summary>
			public static readonly MetadataColumn Interface = new MetadataColumn(1, "Interface", CodedIndex.TypeDefOrRef, "");

			/// <summary>
			/// InterfaceImpl Columns
			/// </summary>
			internal static readonly MetadataColumn[] Columns = {Class, Interface};
		}
		/// <summary>
		/// 22.24 ManifestResource : 0x28
		/// </summary>
		public static class ManifestResource
		{
			/// <summary>
			/// 0. Offset : Int32
			/// </summary>
			public static readonly MetadataColumn Offset = new MetadataColumn(0, "Offset", ColumnType.Int32, "");

			/// <summary>
			/// 1. Flags : ManifestResourceAttributes
			/// </summary>
			public static readonly MetadataColumn Flags = new MetadataColumn(1, "Flags", ColumnType.Int32, typeof(ManifestResourceAttributes), "");

			/// <summary>
			/// 2. Name : StringIndex
			/// </summary>
			public static readonly MetadataColumn Name = new MetadataColumn(2, "Name", ColumnType.StringIndex, "");

			/// <summary>
			/// 3. Implementation : Implementation
			/// </summary>
			public static readonly MetadataColumn Implementation = new MetadataColumn(3, "Implementation", CodedIndex.Implementation, "");

			/// <summary>
			/// ManifestResource Columns
			/// </summary>
			internal static readonly MetadataColumn[] Columns = {Offset, Flags, Name, Implementation};
		}
		/// <summary>
		/// 22.25 The MemberRef table combines two sorts of references, to Methods and to Fields of a class, known as MethodRef and FieldRef, respectively.
		/// </summary>
		public static class MemberRef
		{
			/// <summary>
			/// 0. Class : MemberRefParent
			/// </summary>
			public static readonly MetadataColumn Class = new MetadataColumn(0, "Class", CodedIndex.MemberRefParent, "");

			/// <summary>
			/// 1. Name : StringIndex
			/// </summary>
			public static readonly MetadataColumn Name = new MetadataColumn(1, "Name", ColumnType.StringIndex, "");

			/// <summary>
			/// 2. Signature : BlobIndex
			/// </summary>
			public static readonly MetadataColumn Signature = new MetadataColumn(2, "Signature", ColumnType.BlobIndex, "");

			/// <summary>
			/// MemberRef Columns
			/// </summary>
			internal static readonly MetadataColumn[] Columns = {Class, Name, Signature};
		}
		/// <summary>
		/// 22.26 MethodDef : 0x06
		/// </summary>
		public static class MethodDef
		{
			/// <summary>
			/// 0. RVA : Int32
			/// </summary>
			public static readonly MetadataColumn RVA = new MetadataColumn(0, "RVA", ColumnType.Int32, "");

			/// <summary>
			/// 1. ImplFlags : MethodImplAttributes
			/// </summary>
			public static readonly MetadataColumn ImplFlags = new MetadataColumn(1, "ImplFlags", ColumnType.Int16, typeof(MethodImplAttributes), "");

			/// <summary>
			/// 2. Flags : MethodAttributes
			/// </summary>
			public static readonly MetadataColumn Flags = new MetadataColumn(2, "Flags", ColumnType.Int16, typeof(MethodAttributes), "");

			/// <summary>
			/// 3. Name : StringIndex
			/// </summary>
			public static readonly MetadataColumn Name = new MetadataColumn(3, "Name", ColumnType.StringIndex, "");

			/// <summary>
			/// 4. Signature : BlobIndex
			/// </summary>
			public static readonly MetadataColumn Signature = new MetadataColumn(4, "Signature", ColumnType.BlobIndex, "");

			/// <summary>
			/// 5. ParamList : Param
			/// </summary>
			public static readonly MetadataColumn ParamList = new MetadataColumn(5, "ParamList", TableId.Param, "");

			/// <summary>
			/// MethodDef Columns
			/// </summary>
			internal static readonly MetadataColumn[] Columns = {RVA, ImplFlags, Flags, Name, Signature, ParamList};
		}
		/// <summary>
		/// 22.27 MethodImpl tables let a compiler override the default inheritance rules provided by the CLI.
		/// </summary>
		public static class MethodImpl
		{
			/// <summary>
			/// 0. Class : TypeDef
			/// </summary>
			public static readonly MetadataColumn Class = new MetadataColumn(0, "Class", TableId.TypeDef, "");

			/// <summary>
			/// 1. MethodBody : MethodDefOrRef
			/// </summary>
			public static readonly MetadataColumn MethodBody = new MetadataColumn(1, "MethodBody", CodedIndex.MethodDefOrRef, "");

			/// <summary>
			/// 2. MethodDeclaration : MethodDefOrRef
			/// </summary>
			public static readonly MetadataColumn MethodDeclaration = new MetadataColumn(2, "MethodDeclaration", CodedIndex.MethodDefOrRef, "");

			/// <summary>
			/// MethodImpl Columns
			/// </summary>
			internal static readonly MetadataColumn[] Columns = {Class, MethodBody, MethodDeclaration};
		}
		/// <summary>
		/// 22.28 MethodSemantics : 0x18
		/// </summary>
		public static class MethodSemantics
		{
			/// <summary>
			/// 0. Semantics : MethodSemanticsAttributes
			/// </summary>
			public static readonly MetadataColumn Semantics = new MetadataColumn(0, "Semantics", ColumnType.Int16, typeof(MethodSemanticsAttributes), "");

			/// <summary>
			/// 1. Method : MethodDef
			/// </summary>
			public static readonly MetadataColumn Method = new MetadataColumn(1, "Method", TableId.MethodDef, "");

			/// <summary>
			/// 2. Association : HasSemantics
			/// </summary>
			public static readonly MetadataColumn Association = new MetadataColumn(2, "Association", CodedIndex.HasSemantics, "");

			/// <summary>
			/// MethodSemantics Columns
			/// </summary>
			internal static readonly MetadataColumn[] Columns = {Semantics, Method, Association};
		}
		/// <summary>
		/// 22.29 MethodSpec : 0x2B
		/// </summary>
		public static class MethodSpec
		{
			/// <summary>
			/// 0. Method : MethodDefOrRef
			/// </summary>
			public static readonly MetadataColumn Method = new MetadataColumn(0, "Method", CodedIndex.MethodDefOrRef, "");

			/// <summary>
			/// 1. Instantiation : BlobIndex
			/// </summary>
			public static readonly MetadataColumn Instantiation = new MetadataColumn(1, "Instantiation", ColumnType.BlobIndex, "");

			/// <summary>
			/// MethodSpec Columns
			/// </summary>
			internal static readonly MetadataColumn[] Columns = {Method, Instantiation};
		}
		/// <summary>
		/// 22.30 Module : 0x00
		/// </summary>
		public static class Module
		{
			/// <summary>
			/// 0. Generation : Int16
			/// </summary>
			public static readonly MetadataColumn Generation = new MetadataColumn(0, "Generation", ColumnType.Int16, "");

			/// <summary>
			/// 1. Name : StringIndex
			/// </summary>
			public static readonly MetadataColumn Name = new MetadataColumn(1, "Name", ColumnType.StringIndex, "");

			/// <summary>
			/// 2. Mvid : GuidIndex
			/// </summary>
			public static readonly MetadataColumn Mvid = new MetadataColumn(2, "Mvid", ColumnType.GuidIndex, "");

			/// <summary>
			/// 3. EncId : GuidIndex
			/// </summary>
			public static readonly MetadataColumn EncId = new MetadataColumn(3, "EncId", ColumnType.GuidIndex, "");

			/// <summary>
			/// 4. EncBaseId : GuidIndex
			/// </summary>
			public static readonly MetadataColumn EncBaseId = new MetadataColumn(4, "EncBaseId", ColumnType.GuidIndex, "");

			/// <summary>
			/// Module Columns
			/// </summary>
			internal static readonly MetadataColumn[] Columns = {Generation, Name, Mvid, EncId, EncBaseId};
		}
		/// <summary>
		/// 22.31 ModuleRef : 0x1A
		/// </summary>
		public static class ModuleRef
		{
			/// <summary>
			/// 0. Name : StringIndex
			/// </summary>
			public static readonly MetadataColumn Name = new MetadataColumn(0, "Name", ColumnType.StringIndex, "");

			/// <summary>
			/// ModuleRef Columns
			/// </summary>
			internal static readonly MetadataColumn[] Columns = {Name};
		}
		/// <summary>
		/// 22.32 NestedClass : 0x29
		/// </summary>
		public static class NestedClass
		{
			/// <summary>
			/// 0. Class : TypeDef
			/// </summary>
			public static readonly MetadataColumn Class = new MetadataColumn(0, "Class", TableId.TypeDef, "");

			/// <summary>
			/// 1. EnclosingClass : TypeDef
			/// </summary>
			public static readonly MetadataColumn EnclosingClass = new MetadataColumn(1, "EnclosingClass", TableId.TypeDef, "");

			/// <summary>
			/// NestedClass Columns
			/// </summary>
			internal static readonly MetadataColumn[] Columns = {Class, EnclosingClass};
		}
		/// <summary>
		/// 22.33 Param : 0x08
		/// </summary>
		public static class Param
		{
			/// <summary>
			/// 0. Flags : ParamAttributes
			/// </summary>
			public static readonly MetadataColumn Flags = new MetadataColumn(0, "Flags", ColumnType.Int16, typeof(ParamAttributes), "");

			/// <summary>
			/// 1. Sequence : Int16
			/// </summary>
			public static readonly MetadataColumn Sequence = new MetadataColumn(1, "Sequence", ColumnType.Int16, "");

			/// <summary>
			/// 2. Name : StringIndex
			/// </summary>
			public static readonly MetadataColumn Name = new MetadataColumn(2, "Name", ColumnType.StringIndex, "");

			/// <summary>
			/// Param Columns
			/// </summary>
			internal static readonly MetadataColumn[] Columns = {Flags, Sequence, Name};
		}
		/// <summary>
		/// 22.34 Property : 0x17
		/// </summary>
		public static class Property
		{
			/// <summary>
			/// 0. Flags : PropertyAttributes
			/// </summary>
			public static readonly MetadataColumn Flags = new MetadataColumn(0, "Flags", ColumnType.Int16, typeof(PropertyAttributes), "");

			/// <summary>
			/// 1. Name : StringIndex
			/// </summary>
			public static readonly MetadataColumn Name = new MetadataColumn(1, "Name", ColumnType.StringIndex, "");

			/// <summary>
			/// 2. Type : BlobIndex
			/// </summary>
			public static readonly MetadataColumn Type = new MetadataColumn(2, "Type", ColumnType.BlobIndex, "");

			/// <summary>
			/// Property Columns
			/// </summary>
			internal static readonly MetadataColumn[] Columns = {Flags, Name, Type};
		}
		/// <summary>
		/// 22.35 PropertyMap : 0x15
		/// </summary>
		public static class PropertyMap
		{
			/// <summary>
			/// 0. Parent : TypeDef
			/// </summary>
			public static readonly MetadataColumn Parent = new MetadataColumn(0, "Parent", TableId.TypeDef, "");

			/// <summary>
			/// 1. PropertyList : Property
			/// </summary>
			public static readonly MetadataColumn PropertyList = new MetadataColumn(1, "PropertyList", TableId.Property, "");

			/// <summary>
			/// PropertyMap Columns
			/// </summary>
			internal static readonly MetadataColumn[] Columns = {Parent, PropertyList};
		}
		/// <summary>
		/// 22.36 StandAloneSig : 0x11
		/// </summary>
		public static class StandAloneSig
		{
			/// <summary>
			/// 0. Signature : BlobIndex
			/// </summary>
			public static readonly MetadataColumn Signature = new MetadataColumn(0, "Signature", ColumnType.BlobIndex, "");

			/// <summary>
			/// StandAloneSig Columns
			/// </summary>
			internal static readonly MetadataColumn[] Columns = {Signature};
		}
		/// <summary>
		/// 22.37 TypeDef : 0x02
		/// </summary>
		public static class TypeDef
		{
			/// <summary>
			/// 0. Flags : TypeAttributes
			/// </summary>
			public static readonly MetadataColumn Flags = new MetadataColumn(0, "Flags", ColumnType.Int32, typeof(TypeAttributes), "");

			/// <summary>
			/// 1. TypeName : StringIndex
			/// </summary>
			public static readonly MetadataColumn TypeName = new MetadataColumn(1, "TypeName", ColumnType.StringIndex, "");

			/// <summary>
			/// 2. TypeNamespace : StringIndex
			/// </summary>
			public static readonly MetadataColumn TypeNamespace = new MetadataColumn(2, "TypeNamespace", ColumnType.StringIndex, "");

			/// <summary>
			/// 3. Extends : TypeDefOrRef
			/// </summary>
			public static readonly MetadataColumn Extends = new MetadataColumn(3, "Extends", CodedIndex.TypeDefOrRef, "");

			/// <summary>
			/// 4. FieldList : Field
			/// </summary>
			public static readonly MetadataColumn FieldList = new MetadataColumn(4, "FieldList", TableId.Field, "");

			/// <summary>
			/// 5. MethodList : MethodDef
			/// </summary>
			public static readonly MetadataColumn MethodList = new MetadataColumn(5, "MethodList", TableId.MethodDef, "");

			/// <summary>
			/// TypeDef Columns
			/// </summary>
			internal static readonly MetadataColumn[] Columns = {Flags, TypeName, TypeNamespace, Extends, FieldList, MethodList};
		}
		/// <summary>
		/// 22.38 TypeRef : 0x01
		/// </summary>
		public static class TypeRef
		{
			/// <summary>
			/// 0. ResolutionScope : ResolutionScope
			/// </summary>
			public static readonly MetadataColumn ResolutionScope = new MetadataColumn(0, "ResolutionScope", CodedIndex.ResolutionScope, "");

			/// <summary>
			/// 1. TypeName : StringIndex
			/// </summary>
			public static readonly MetadataColumn TypeName = new MetadataColumn(1, "TypeName", ColumnType.StringIndex, "");

			/// <summary>
			/// 2. TypeNamespace : StringIndex
			/// </summary>
			public static readonly MetadataColumn TypeNamespace = new MetadataColumn(2, "TypeNamespace", ColumnType.StringIndex, "");

			/// <summary>
			/// TypeRef Columns
			/// </summary>
			internal static readonly MetadataColumn[] Columns = {ResolutionScope, TypeName, TypeNamespace};
		}
		/// <summary>
		/// 22.39 TypeSpec : 0x1B
		/// </summary>
		public static class TypeSpec
		{
			/// <summary>
			/// 0. Signature : BlobIndex
			/// </summary>
			public static readonly MetadataColumn Signature = new MetadataColumn(0, "Signature", ColumnType.BlobIndex, "");

			/// <summary>
			/// TypeSpec Columns
			/// </summary>
			internal static readonly MetadataColumn[] Columns = {Signature};
		}
		/// <summary>
		/// FieldPtr : 3
		/// </summary>
		public static class FieldPtr
		{
			/// <summary>
			/// 0. Field : Field
			/// </summary>
			public static readonly MetadataColumn Field = new MetadataColumn(0, "Field", TableId.Field, "");

			/// <summary>
			/// FieldPtr Columns
			/// </summary>
			internal static readonly MetadataColumn[] Columns = {Field};
		}
		/// <summary>
		/// MethodPtr : 5
		/// </summary>
		public static class MethodPtr
		{
			/// <summary>
			/// 0. Method : MethodDef
			/// </summary>
			public static readonly MetadataColumn Method = new MetadataColumn(0, "Method", TableId.MethodDef, "");

			/// <summary>
			/// MethodPtr Columns
			/// </summary>
			internal static readonly MetadataColumn[] Columns = {Method};
		}
		/// <summary>
		/// ParamPtr : 7
		/// </summary>
		public static class ParamPtr
		{
			/// <summary>
			/// 0. Param : Param
			/// </summary>
			public static readonly MetadataColumn Param = new MetadataColumn(0, "Param", TableId.Param, "");

			/// <summary>
			/// ParamPtr Columns
			/// </summary>
			internal static readonly MetadataColumn[] Columns = {Param};
		}
		/// <summary>
		/// EventPtr : 19
		/// </summary>
		public static class EventPtr
		{
			/// <summary>
			/// 0. Event : Event
			/// </summary>
			public static readonly MetadataColumn Event = new MetadataColumn(0, "Event", TableId.Event, "");

			/// <summary>
			/// EventPtr Columns
			/// </summary>
			internal static readonly MetadataColumn[] Columns = {Event};
		}
		/// <summary>
		/// PropertyPtr : 22
		/// </summary>
		public static class PropertyPtr
		{
			/// <summary>
			/// 0. Property : Property
			/// </summary>
			public static readonly MetadataColumn Property = new MetadataColumn(0, "Property", TableId.Property, "");

			/// <summary>
			/// PropertyPtr Columns
			/// </summary>
			internal static readonly MetadataColumn[] Columns = {Property};
		}
		/// <summary>
		/// EncodingLog : 30
		/// </summary>
		public static class EncodingLog
		{
			/// <summary>
			/// 0. Token : Int32
			/// </summary>
			public static readonly MetadataColumn Token = new MetadataColumn(0, "Token", ColumnType.Int32, "");

			/// <summary>
			/// 1. FuncCode : Int32
			/// </summary>
			public static readonly MetadataColumn FuncCode = new MetadataColumn(1, "FuncCode", ColumnType.Int32, "");

			/// <summary>
			/// EncodingLog Columns
			/// </summary>
			internal static readonly MetadataColumn[] Columns = {Token, FuncCode};
		}
		/// <summary>
		/// EncodingMap : 31
		/// </summary>
		public static class EncodingMap
		{
			/// <summary>
			/// 0. Token : Int32
			/// </summary>
			public static readonly MetadataColumn Token = new MetadataColumn(0, "Token", ColumnType.Int32, "");

			/// <summary>
			/// EncodingMap Columns
			/// </summary>
			internal static readonly MetadataColumn[] Columns = {Token};
		}
		#endregion
	}
}
