using System;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Abc;

namespace DataDynamics.PageFX.FlashLand.Core.SpecialTypes
{
    internal sealed class VectorInstance : IVectorType
    {
	    public VectorInstance(IType type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            var instance = type as IGenericInstance;
            if (instance == null)
                throw new ArgumentException(string.Format("Type {0} is not generic instance", type));

            if (instance.GenericArguments.Count != 1)
                throw new ArgumentException("Invalid vector instance");

            Type = instance;
            Param = instance.GenericArguments[0];
        }

	    public IGenericInstance Type { get; private set; }

	    public IType Param { get; private set; }

	    public AbcFile Abc { get; set; }

	    public AbcMultiname Name
        {
            get
            {
                if (_name != null) return _name;

                //TODO: Process builtin vector types (Vector$double, Vector$int, Vector$uint)

                var param = Abc.GetTypeName(Param, true);
                _name = Abc.DefineVectorTypeName(param);

                return _name;
            }
        }
        private AbcMultiname _name;

        public override string ToString()
        {
            return Type.ToString();
        }
    }
}