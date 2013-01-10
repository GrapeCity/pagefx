using System;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Abc;

namespace DataDynamics.PageFX.FlashLand.Core.SpecialTypes
{
    internal sealed class VectorType : IVectorType
    {
        public VectorType(IType type, IType param)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (param == null)
                throw new ArgumentNullException("param");

            Type = type;
            Param = param;
        }

	    public AbcFile ByteCode { get; set; }

	    public IType Type { get; private set; }

	    public IType Param { get; private set; }

	    public AbcMultiname Name
        {
            get 
            {
                if (_name == null)
                {
					ByteCode.Generator.TypeBuilder.Build(Param);
                    var paramType = Param.GetMultiname();
                    _name = ByteCode.DefineVectorTypeName(paramType);
                }
                return _name;
            }
        }
        private AbcMultiname _name;

        public override string ToString()
        {
            return Type + ".<" + Param + ">";
        }

        public static string GetVectorParam(IType type)
        {
            var attr = type.FindAttribute(Attrs.Vector);
            return GetVectorParam(attr);
        }

        public static string GetVectorParam(ICustomAttribute attr)
        {
            if (attr == null) return null;
            if (attr.Arguments.Count != 1) return null;
            return attr.Arguments[0].Value as string;
        }
    }
}