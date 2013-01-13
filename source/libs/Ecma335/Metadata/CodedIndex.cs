using System;
using System.Text;

namespace DataDynamics.PageFX.Ecma335.Metadata
{
    /// <summary>
    /// Contains info about coded index.
    /// </summary>
    internal sealed class CodedIndex
    {
        public static readonly CodedIndex CustomAttributeType =
            new CodedIndex("CustomAttributeType", 3,
                              TableId.TypeRef, //Not used
                              TableId.TypeRef, //Not used
                              TableId.MethodDef,
                              TableId.MemberRef,
                              TableId.TypeDef); //Not used

        /// <summary>
        /// Codex index for elements that can has constant.
        /// </summary>
        public static readonly CodedIndex HasConstant =
            new CodedIndex("HasConstant", 2,
                              TableId.Field,
                              TableId.Param,
                              TableId.Property);

        //SPECNOTE:
        //[Note: HasCustomAttributes only has values for tables that are “externally visible”; that is, that correspond to items
        //in a user source program. For example, an attribute can be attached to a TypeDef table and a Field table, but not a
        //ClassLayout table. As a result, some table types are missing from the enum above.]

        public static readonly CodedIndex HasCustomAttribute =
            new CodedIndex("HasCustomAttribute", 5,
                              TableId.MethodDef,
                              TableId.Field,
                              TableId.TypeRef,
                              TableId.TypeDef,
                              TableId.Param,
                              TableId.InterfaceImpl,
                              TableId.MemberRef,
                              TableId.Module,
                              TableId.DeclSecurity,
                              TableId.Property,
                              TableId.Event,
                              TableId.StandAloneSig,
                              TableId.ModuleRef,
                              TableId.TypeSpec,
                              TableId.Assembly,
                              TableId.AssemblyRef,
                              TableId.File,
                              TableId.ExportedType,
                              TableId.ManifestResource,
                              TableId.GenericParam);

        public static readonly CodedIndex HasDeclSecurity =
            new CodedIndex("HasDeclSecurity", 2,
                              TableId.TypeDef,
                              TableId.MethodDef,
                              TableId.Assembly);

        public static readonly CodedIndex HasFieldMarshal =
            new CodedIndex("HasFieldMarshal", 1,
                              TableId.Field,
                              TableId.Param);

        public static readonly CodedIndex HasSemantics =
            new CodedIndex("HasSemantics", 1,
                              TableId.Event,
                              TableId.Property);

        public static readonly CodedIndex Implementation =
            new CodedIndex("Implementation", 2,
                              TableId.File,
                              TableId.AssemblyRef,
                              TableId.ExportedType);

        public static readonly CodedIndex MemberForwarded =
            new CodedIndex("MemberForwarded", 1,
                              TableId.Field,
                              TableId.MethodDef);

        public static readonly CodedIndex MemberRefParent =
            new CodedIndex("MemberRefParent", 3,
                              TableId.TypeDef,
                              TableId.TypeRef,
                              TableId.ModuleRef,
                              TableId.MethodDef,
                              TableId.TypeSpec);

        public static readonly CodedIndex MethodDefOrRef =
            new CodedIndex("MethodDefOrRef", 1,
                              TableId.MethodDef,
                              TableId.MemberRef);

        public static readonly CodedIndex ResolutionScope =
            new CodedIndex("ResolutionScope", 2,
                              TableId.Module,
                              TableId.ModuleRef,
                              TableId.AssemblyRef,
                              TableId.TypeRef);

        public static readonly CodedIndex TypeDefOrRef =
            new CodedIndex("TypeDefOrRef", 2,
                              TableId.TypeDef,
                              TableId.TypeRef,
                              TableId.TypeSpec);

        public static readonly CodedIndex TypeOrMethodDef =
            new CodedIndex("TypeOrMethodDef", 1,
                              TableId.TypeDef,
                              TableId.MethodDef);

        internal static readonly CodedIndex[] All =
            {
                CustomAttributeType,
                HasConstant,
                HasCustomAttribute,
                HasDeclSecurity,
                HasFieldMarshal,
                HasSemantics,
                Implementation,
                MemberForwarded,
                MemberRefParent,
                MethodDefOrRef,
                ResolutionScope,
                TypeDefOrRef,
                TypeOrMethodDef
            };

        static CodedIndex()
        {
            for (int i = 0; i < All.Length; i++)
            {
                All[i].Id = i;
            }
        }

	    internal CodedIndex(string name, int bits, params TableId[] tables)
        {
            Name = name;
            Bits = bits;
            Tables = tables;
        }

	    /// <summary>
	    /// Gets the name of this coded index.
	    /// </summary>
	    public string Name { get; private set; }

	    /// <summary>
	    /// Gets the id of this coded index.
	    /// </summary>
	    public int Id { get; private set; }

	    /// <summary>
	    /// Number of bits to encode table id.
	    /// </summary>
	    public int Bits { get; private set; }

	    public TableId[] Tables { get; private set; }

	    public override string ToString()
        {
            var s = new StringBuilder();
            s.AppendFormat("CodedIndex({0}", Name);
            int n = Tables.Length;
            for (int i = 0; i < n; ++i)
            {
                s.Append(", ");
                s.Append(Tables[i].ToString());
            }
            s.AppendFormat(", {0})", Bits);
            return s.ToString();
        }

        private Exception InvalidIndex(uint codedIndex)
        {
            return new BadMetadataException(string.Format("Invalid coded index {0} for {1}", codedIndex, this));
        }

        public SimpleIndex Decode(uint codedIndex)
        {
            int mask = 0xFF >> (8 - Bits);
            int tag = (int)(codedIndex & mask);
            if (tag < 0 || tag >= Tables.Length)
                throw InvalidIndex(codedIndex);
            uint index = codedIndex >> Bits;
            return new SimpleIndex(Tables[tag], (int)index);
        }
    }
}