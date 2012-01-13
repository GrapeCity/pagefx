using System;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.FLI.ABC;

namespace DataDynamics.PageFX.FLI
{
    internal class VectorInstance : IVectorType
    {
        private readonly IGenericInstance _type;
        private readonly IType _param;

        public VectorInstance(IType type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            var instance = type as IGenericInstance;
            if (instance == null)
                throw new ArgumentException(string.Format("Type {0} is not generic instance", type));
            if (instance.GenericArguments.Count != 1)
                throw new ArgumentException("Invalid vector instance");

            _type = instance;
            _param = instance.GenericArguments[0];
            type.Tag = this;
        }

        public IGenericInstance Type
        {
            get { return _type; }
        }

        public IType Param
        {
            get { return _param; }
        }

        public AbcFile ABC
        {
            get { return _abc; }
            set { _abc = value; }
        }
        AbcFile _abc;

        public AbcMultiname Name
        {
            get
            {
                if (_name != null) return _name;

                //TODO: Process builtin vector types (Vector$double, Vector$int, Vector$uint)

                var param = _abc.GetTypeName(_param, true);
                _name = _abc.DefineVectorTypeName(param);

                return _name;
            }
        }
        AbcMultiname _name;

        public override string ToString()
        {
            return _type.ToString();
        }
    }
}