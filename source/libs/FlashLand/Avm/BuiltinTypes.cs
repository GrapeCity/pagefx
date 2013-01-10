using System.Reflection;
using DataDynamics.PageFX.Common.Extensions;
using DataDynamics.PageFX.FlashLand.Abc;
using DataDynamics.PageFX.FlashLand.Core;
using DataDynamics.PageFX.FlashLand.IL;

namespace DataDynamics.PageFX.FlashLand.Avm
{
    #region enum AvmTypeCode
    public enum AvmTypeCode
    {
        [QName("void")]
        Void,

        Boolean,

		[QName("sbyte")]
        Int8,

		[QName("byte")]
        UInt8,

		[QName("short")]
        Int16,

		[QName("ushort")]
        UInt16,

		[QName("int")]
        Int32,

		[QName("uint")]
        UInt32,

		[QName("long")]
        Int64,

		[QName("ulong")]
        UInt64,

        Number,

		[QName("float")]
        Float,

		[QName("double")]
        Double,

		[QName("decimal")]
        Decimal,

        String,
        Object,
        Error,
        Array,
        Function,
        Class,
        Namespace,
        TypeError,
        XML,
        XMLList,
        QName,

		[QName(AS3.Vector.Namespace, AS3.Vector.Name)]
		Verctor
    }
    #endregion

    public sealed class BuiltinTypes
    {
        private readonly AbcFile _abc;
        private static readonly QName[] Names;

        static BuiltinTypes()
        {
            var fields = typeof(AvmTypeCode).GetFields(BindingFlags.Public | BindingFlags.Static);
            int n = fields.Length;
            Names = new QName[n];
            for (int i = 0; i < n; ++i)
            {
                var field = fields[i];
				var value = field.GetValue(null);
            	var index = (int)value;
                var attr = field.GetAttribute<QNameAttribute>(false);
            	Names[index] = attr != null ? attr.Value : new QName(field.Name, KnownNamespace.Global);
            }
        }

        private readonly AbcMultiname[] _types;

        public BuiltinTypes(AbcFile abc)
        {
            _abc = abc;
            _types = new AbcMultiname[Names.Length];
        }

        public AbcMultiname this[AvmTypeCode code]
        {
            get
            {
	            int i = (int)code;
				return _types[i] ?? (_types[i] = _abc.DefineName(Names[i]));
            }
        }

        public AbcMultiname Void
        {
            get { return this[AvmTypeCode.Void]; }
        }

        public AbcMultiname Boolean
        {
            get { return this[AvmTypeCode.Boolean]; }
        }

        public AbcMultiname RealBoolean
        {
            get
            {
                if (AvmConfig.BooleanAsInt)
                    return Int32;
                return Boolean;
            }
        }

        public AbcMultiname Int8
        {
            get { return this[AvmTypeCode.Int8]; }
        }

        public AbcMultiname UInt8
        {
            get { return this[AvmTypeCode.UInt8]; }
        }

        public AbcMultiname Int16
        {
            get { return this[AvmTypeCode.Int16]; }
        }

        public AbcMultiname UInt16
        {
            get { return this[AvmTypeCode.UInt16]; }
        }

        public AbcMultiname Int32
        {
            get { return this[AvmTypeCode.Int32]; }
        }

        public AbcMultiname UInt32
        {
            get { return this[AvmTypeCode.UInt32]; }
        }

        public AbcMultiname Int64
        {
            get { return this[AvmTypeCode.Int64]; }
        }

        public AbcMultiname UInt64
        {
            get { return this[AvmTypeCode.UInt64]; }
        }

        public AbcMultiname Number
        {
            get { return this[AvmTypeCode.Number]; }
        }

        public AbcMultiname Float
        {
            get { return this[AvmTypeCode.Float]; }
        }

        public AbcMultiname Double
        {
            get { return this[AvmTypeCode.Double]; }
        }

        public AbcMultiname Decimal
        {
            get { return this[AvmTypeCode.Decimal]; }
        }

        public AbcMultiname String
        {
            get { return this[AvmTypeCode.String]; }
        }

        public AbcMultiname Object
        {
            get { return this[AvmTypeCode.Object]; }
        }

        public AbcMultiname Error
        {
            get { return this[AvmTypeCode.Error]; }
        }

        public AbcMultiname TypeError
        {
            get { return this[AvmTypeCode.TypeError]; }
        }

        public AbcMultiname Array
        {
            get { return this[AvmTypeCode.Array]; }
        }

        public AbcMultiname Function
        {
            get { return this[AvmTypeCode.Function]; }
        }

        public AbcMultiname Class
        {
            get { return this[AvmTypeCode.Class]; }
        }

        public AbcMultiname Namespace
        {
            get { return this[AvmTypeCode.Namespace]; }
        }

        public AbcMultiname QName
        {
            get { return this[AvmTypeCode.QName]; }
        }

        public AbcMultiname XML
        {
            get { return this[AvmTypeCode.XML]; }
        }

        public AbcMultiname XMLList
        {
            get { return this[AvmTypeCode.XMLList]; }
        }

		public AbcMultiname Vector
		{
			get { return this[AvmTypeCode.Verctor]; }
		}

        public InstructionCode GetCoercionInstructionCode(AbcMultiname type)
        {
			if (ReferenceEquals(type, Int32))
				return InstructionCode.Coerce_i;
            if (ReferenceEquals(type, UInt32))
				return InstructionCode.Coerce_u;
            if (ReferenceEquals(type, String))
				return InstructionCode.Coerce_s;
            if (ReferenceEquals(type, Boolean))
				return InstructionCode.Coerce_b;
            if (ReferenceEquals(type, Object))
				return InstructionCode.Coerce_o;
            return InstructionCode.Coerce;
        }
    }
}