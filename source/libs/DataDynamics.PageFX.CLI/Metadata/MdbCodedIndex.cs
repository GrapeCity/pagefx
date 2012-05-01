using System;
using System.Text;

namespace DataDynamics.PageFX.CLI.Metadata
{
    /// <summary>
    /// Contains info about coded index.
    /// </summary>
    public class MdbCodedIndex
    {
        public static readonly MdbCodedIndex CustomAttributeType =
            new MdbCodedIndex("CustomAttributeType", 3,
                              MdbTableId.TypeRef, //Not used
                              MdbTableId.TypeRef, //Not used
                              MdbTableId.MethodDef,
                              MdbTableId.MemberRef,
                              MdbTableId.TypeDef); //Not used

        /// <summary>
        /// Codex index for elements that can has constant.
        /// </summary>
        public static readonly MdbCodedIndex HasConstant =
            new MdbCodedIndex("HasConstant", 2,
                              MdbTableId.Field,
                              MdbTableId.Param,
                              MdbTableId.Property);

        //SPECNOTE:
        //[Note: HasCustomAttributes only has values for tables that are “externally visible”; that is, that correspond to items
        //in a user source program. For example, an attribute can be attached to a TypeDef table and a Field table, but not a
        //ClassLayout table. As a result, some table types are missing from the enum above.]

        public static readonly MdbCodedIndex HasCustomAttribute =
            new MdbCodedIndex("HasCustomAttribute", 5,
                              MdbTableId.MethodDef,
                              MdbTableId.Field,
                              MdbTableId.TypeRef,
                              MdbTableId.TypeDef,
                              MdbTableId.Param,
                              MdbTableId.InterfaceImpl,
                              MdbTableId.MemberRef,
                              MdbTableId.Module,
                              MdbTableId.DeclSecurity,
                              MdbTableId.Property,
                              MdbTableId.Event,
                              MdbTableId.StandAloneSig,
                              MdbTableId.ModuleRef,
                              MdbTableId.TypeSpec,
                              MdbTableId.Assembly,
                              MdbTableId.AssemblyRef,
                              MdbTableId.File,
                              MdbTableId.ExportedType,
                              MdbTableId.ManifestResource,
                              MdbTableId.GenericParam);

        public static readonly MdbCodedIndex HasDeclSecurity =
            new MdbCodedIndex("HasDeclSecurity", 2,
                              MdbTableId.TypeDef,
                              MdbTableId.MethodDef,
                              MdbTableId.Assembly);

        public static readonly MdbCodedIndex HasFieldMarshal =
            new MdbCodedIndex("HasFieldMarshal", 1,
                              MdbTableId.Field,
                              MdbTableId.Param);

        public static readonly MdbCodedIndex HasSemantics =
            new MdbCodedIndex("HasSemantics", 1,
                              MdbTableId.Event,
                              MdbTableId.Property);

        public static readonly MdbCodedIndex Implementation =
            new MdbCodedIndex("Implementation", 2,
                              MdbTableId.File,
                              MdbTableId.AssemblyRef,
                              MdbTableId.ExportedType);

        public static readonly MdbCodedIndex MemberForwarded =
            new MdbCodedIndex("MemberForwarded", 1,
                              MdbTableId.Field,
                              MdbTableId.MethodDef);

        public static readonly MdbCodedIndex MemberRefParent =
            new MdbCodedIndex("MemberRefParent", 3,
                              MdbTableId.TypeDef,
                              MdbTableId.TypeRef,
                              MdbTableId.ModuleRef,
                              MdbTableId.MethodDef,
                              MdbTableId.TypeSpec);

        public static readonly MdbCodedIndex MethodDefOrRef =
            new MdbCodedIndex("MethodDefOrRef", 1,
                              MdbTableId.MethodDef,
                              MdbTableId.MemberRef);

        public static readonly MdbCodedIndex ResolutionScope =
            new MdbCodedIndex("ResolutionScope", 2,
                              MdbTableId.Module,
                              MdbTableId.ModuleRef,
                              MdbTableId.AssemblyRef,
                              MdbTableId.TypeRef);

        public static readonly MdbCodedIndex TypeDefOrRef =
            new MdbCodedIndex("TypeDefOrRef", 2,
                              MdbTableId.TypeDef,
                              MdbTableId.TypeRef,
                              MdbTableId.TypeSpec);

        public static readonly MdbCodedIndex TypeOrMethodDef =
            new MdbCodedIndex("TypeOrMethodDef", 1,
                              MdbTableId.TypeDef,
                              MdbTableId.MethodDef);

        internal static readonly MdbCodedIndex[] All =
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

        static MdbCodedIndex()
        {
            for (int i = 0; i < All.Length; i++)
            {
                All[i]._id = i;
            }
        }

        private readonly int _bits;
        private int _id;
        private readonly string _name;
        private readonly MdbTableId[] _tables;

        internal MdbCodedIndex(string name, int bits, params MdbTableId[] tables)
        {
            _name = name;
            _bits = bits;
            _tables = tables;
        }

        /// <summary>
        /// Gets the name of this coded index.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Gets the id of this coded index.
        /// </summary>
        public int ID
        {
            get { return _id; }
        }

        /// <summary>
        /// Number of bits to encode table id.
        /// </summary>
        public int Bits
        {
            get { return _bits; }
        }

        public MdbTableId[] Tables
        {
            get { return _tables; }
        }

        public override string ToString()
        {
            var s = new StringBuilder();
            s.AppendFormat("CodedIndex({0}", _name);
            int n = _tables.Length;
            for (int i = 0; i < n; ++i)
            {
                s.Append(", ");
                s.Append(_tables[i].ToString());
            }
            s.AppendFormat(", {0})", Bits);
            return s.ToString();
        }

        private Exception InvalidIndex(uint codedIndex)
        {
            return new BadMetadataException(string.Format("Invalid coded index {0} for {1}", codedIndex, this));
        }

        public MdbIndex Decode(uint codedIndex)
        {
            int mask = 0xFF >> (8 - Bits);
            int tag = (int)(codedIndex & mask);
            if (tag < 0 || tag >= Tables.Length)
                throw InvalidIndex(codedIndex);
            uint index = codedIndex >> Bits;
            return new MdbIndex(Tables[tag], (int)index);
        }
    }
}