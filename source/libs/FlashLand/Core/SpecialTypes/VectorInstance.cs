using System;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Abc;

namespace DataDynamics.PageFX.FlashLand.Core.SpecialTypes
{
    internal sealed class VectorInstance : IVectorType
    {
	    public VectorInstance(AbcFile abc, IType type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            var instance = type as IGenericInstance;
            if (instance == null)
                throw new ArgumentException(string.Format("Type {0} is not generic instance", type));

            if (instance.GenericArguments.Count != 1)
                throw new ArgumentException("Invalid vector instance");

            Type = instance;
            Parameter = instance.GenericArguments[0];
		    Name = DefineName(abc);
        }

	    public IGenericInstance Type { get; private set; }

	    IType ITypeData.Type
	    {
			get { return Type; }
	    }

	    public IType Parameter { get; private set; }

	    public AbcMultiname Name { get; private set; }

	    private AbcMultiname DefineName(AbcFile abc)
		{
			//TODO: Process builtin vector types (Vector$double, Vector$int, Vector$uint)
			var param = abc.GetTypeName(Parameter, true);
			return abc.DefineVectorTypeName(param);
		}

	    public bool IsDefined(AbcFile abc)
	    {
		    return abc.IsDefined(Name);
	    }

	    public ITypeData Import(AbcFile abc)
	    {
		    return new VectorInstance(abc, Type);
	    }

	    public override string ToString()
        {
            return Type.ToString();
        }
    }
}