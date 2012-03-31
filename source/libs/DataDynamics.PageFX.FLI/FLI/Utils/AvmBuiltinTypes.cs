using System.Reflection;
using DataDynamics.PageFX.FLI.ABC;
using DataDynamics.PageFX.FLI.IL;

namespace DataDynamics.PageFX.FLI
{
    #region enum AvmTypeCode
    public enum AvmTypeCode
    {
        [Name("void")]
        Void,

        Boolean,

        [Name("sbyte")]
        Int8,

        [Name("byte")]
        UInt8,

        [Name("short")]
        Int16,

        [Name("ushort")]
        UInt16,

        [Name("int")]
        Int32,

        [Name("uint")]
        UInt32,

        [Name("long")]
        Int64,

        [Name("ulong")]
        UInt64,

        Number,

        [Name("float")]
        Float,

        [Name("double")]
        Double,

        [Name("decimal")]
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
        QName
    }
    #endregion

    public class AvmBuiltinTypes
    {
        readonly AbcFile _abc;

        static readonly string[] Names;

        static AvmBuiltinTypes()
        {
            var fields = typeof(AvmTypeCode).GetFields(BindingFlags.Public | BindingFlags.Static);
            int n = fields.Length;
            Names = new string[n];
            for (int i = 0; i < n; ++i)
            {
                var field = fields[i];
				var value = field.GetValue(null);
            	var index = (int)value;
                var attr = field.GetAttribute<NameAttribute>(false);
            	Names[index] = attr != null ? attr.Name : field.Name;
            }
        }

        readonly AbcMultiname[] _types;

        public AvmBuiltinTypes(AbcFile abc)
        {
            _abc = abc;
            _types = new AbcMultiname[Names.Length];
        }

        public AbcMultiname this[AvmTypeCode code]
        {
            get
            {
                //int i = (int)code;
                //return _abc.DefineGlobalQName(Names[i]);

                int i = (int)code;
                var mn = _types[i];
                if (mn == null)
                {
                    mn = _abc.DefineGlobalQName(Names[i]);
                    _types[i] = mn;
                }
                return mn;
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

        public InstructionCode GetCoercionInstructionCode(AbcMultiname type)
        {
            if (type == Int32) return InstructionCode.Coerce_i;
            if (type == UInt32) return InstructionCode.Coerce_u;
            if (type == String) return InstructionCode.Coerce_s;
            if (type == Boolean) return InstructionCode.Coerce_b;
            if (type == Object) return InstructionCode.Coerce_o;
            return InstructionCode.Coerce;
        }
    }
}