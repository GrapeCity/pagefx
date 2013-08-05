using System;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Flash.Abc;

namespace DataDynamics.PageFX.Flash.Core.SpecialTypes
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

	    public IType Type { get; private set; }

	    public IType Parameter { get; private set; }

	    public AbcMultiname Name { get; private set; }

	    public bool IsDefined(AbcFile abc)
	    {
		    return abc.IsDefined(Name);
	    }

	    public ITypeData Import(AbcFile abc)
	    {
			var name = Name ?? DefineName(abc);
		    return new VectorType(Type, Parameter) {Name = name};
	    }

	    private AbcMultiname DefineName(AbcFile abc)
		{
			abc.Generator.TypeBuilder.Build(Parameter);
			var paramType = Parameter.GetMultiname();
			return abc.DefineVectorTypeName(paramType);
		}

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