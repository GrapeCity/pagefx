using System;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Abc;

namespace DataDynamics.PageFX.FlashLand.Core.SpecialTypes
{
    internal sealed class VectorType : IVectorType
    {
        public VectorType(IType type, IType parameter)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (parameter == null)
                throw new ArgumentNullException("parameter");

            Type = type;
            Parameter = parameter;
        }

	    public AbcFile Abc { get; set; }

	    public IType Type { get; private set; }

	    public IType Parameter { get; private set; }

	    public AbcMultiname Name
        {
            get 
            {
                if (_name == null)
                {
					Abc.Generator.TypeBuilder.Build(Parameter);
                    var paramType = Parameter.GetMultiname();
                    _name = Abc.DefineVectorTypeName(paramType);
                }
                return _name;
            }
        }
        private AbcMultiname _name;

        public override string ToString()
        {
            return Type + ".<" + Parameter + ">";
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