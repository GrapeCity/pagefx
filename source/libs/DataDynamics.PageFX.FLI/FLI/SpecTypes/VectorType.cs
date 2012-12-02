using System;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.CodeModel.TypeSystem;
using DataDynamics.PageFX.FLI.ABC;

namespace DataDynamics.PageFX.FLI
{
    internal class VectorType : IVectorType
    {
        public VectorType(IType type, IType param)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (param == null)
                throw new ArgumentNullException("param");

            _type = type;
            _param = param;

            type.Tag = this;
        }

        public AbcFile ABC
        {
            get { return _abc; }
            set { _abc = value; }
        }
        AbcFile _abc;

        public IType Type
        {
            get { return _type; }
        }
        readonly IType _type;

        public IType Param
        {
            get { return _param; }
        }
        readonly IType _param;

        public AbcMultiname Name
        {
            get 
            {
                if (_name == null)
                {
                    _abc.generator.DefineType(_param);
                    var paramType = _param.GetMultiname();
                    _name = _abc.DefineVectorTypeName(paramType);
                }
                return _name;
            }
        }
        AbcMultiname _name;

        public override string ToString()
        {
            return _type + ".<" + _param + ">";
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