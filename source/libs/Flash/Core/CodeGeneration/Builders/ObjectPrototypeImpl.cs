using System.Collections;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Flash.Abc;
using DataDynamics.PageFX.Flash.Avm;
using DataDynamics.PageFX.Flash.IL;

namespace DataDynamics.PageFX.Flash.Core.CodeGeneration.Builders
{
    //NOTE: We can not extend AVM global Object class in MX context,
    //because of empty object becomes non empty after registration of functions commented below,
    //and errors can occur in "for each in" loop for empty object.
    //This error was found in MX ModuleManager.getAssociatedFactory method.

    //NOTE: We should extend Object prototype using setPropertyIsEnumerable(prop, false) to prevent
    //a dynamic property being used in loop operations

    internal sealed class ObjectPrototypeImpl
    {
	    private readonly AbcGenerator _generator;

	    public ObjectPrototypeImpl(AbcGenerator generator)
		{
			_generator = generator;
		}

	    private AbcFile Abc
	    {
			get { return _generator.Abc; }
	    }

	    private SystemTypes SystemTypes
	    {
			get { return _generator.SystemTypes; }
	    }

	    #region Prototypes
        readonly Hashtable _prototypes = new Hashtable();

        static string GetKey(AvmTypeCode type, string name)
        {
            return (int)type + "." + name;
        }

        void RegisterPrototype(AvmTypeCode type, string name, AbcCoder coder)
        {
            string key = GetKey(type, name);
            _prototypes[key] = new object[] { coder, null };
        }
        #endregion

        #region DefinePrototype
        void DefinePrototype(AvmTypeCode type, AbcMethod sig)
        {
            var srcmethod = sig.Method;
            if (srcmethod == null) return;

            string key = GetKey(type, srcmethod.Name);
            var val = _prototypes[key] as object[];
            if (val == null) return;

            var m = val[1] as AbcMethod;
            if (m != null) return;

            var coder = val[0] as AbcCoder;
	        m = Abc.DefineMethod(Sig.@from(sig), coder);

	        _generator.NewApi.SetProtoFunction(type, sig.TraitName, m);

            val[0] = m;
        }
        #endregion

        public void Init()
        {
            #region Object.Equals
            RegisterPrototype(
                            AvmTypeCode.Object,
                            Const.Object.MethodEquals,
                            code =>
                                {
                                    const int obj = 1;
                                    code.GetLocal(obj);
                                    var ifNull = code.IfNull();

                                    code.GetLocal(obj);
                                    code.LoadThis();
                                    code.Add(InstructionCode.Equals);
                                    code.FixBool();
                                    code.ReturnValue();

                                    ifNull.BranchTarget = code.Label();
                                    code.PushBool(false);
                                    code.ReturnValue();
                                });
            #endregion

            #region Object.ToString
            RegisterPrototype(
                AvmTypeCode.Object,
                Const.Object.MethodToString,
                code =>
                    {
                        code.LoadThis();
                        code.CallGlobal("toString", 0);
                        code.ReturnValue();
                    });
            #endregion

            #region Object.GetHashCode
            RegisterPrototype(
                AvmTypeCode.Object,
                Const.Object.MethodGetHashCode,
                code =>
                    {
                        var hc = HashCodeImpl.CalcHashCode(_generator);
                        code.Getlex(hc);
                        code.LoadThis();
                        code.Call(hc);
                        code.ReturnValue();
                    });
            #endregion

            #region Object.GetType
            //TODO: use flash.utils.getQualifiedClassName
            RegisterPrototype(
                AvmTypeCode.Object,
                Const.Object.MethodGetType,
                code => code.ReturnNull());
            #endregion

            #region String.ToString
            RegisterPrototype(
                AvmTypeCode.String,
                Const.Object.MethodToString,
                code => code.ReturnThis());
            #endregion

            #region String.GetHashCode
            RegisterPrototype(
                AvmTypeCode.String,
                Const.Object.MethodGetHashCode,
                code =>
                    {

						var m = _generator.MethodBuilder.BuildAbcMethod(SystemTypes.String, "CalcHashCode", 1);

                        code.Getlex(m);
                        code.LoadThis();
                        code.Call(m);
                        code.ReturnValue();
                    });
            #endregion

            #region String.GetType
            RegisterPrototype(
                AvmTypeCode.String,
                Const.Object.MethodGetType,
                code =>
                    {
                        _generator.NewApi.SetProtoFunction(
                            AvmTypeCode.String,
                            Const.Object.MethodGetTypeId,
                            DefineString_GetTypeId());

                        code.ReturnTypeOf(SystemTypes.String);
                    });
            #endregion
        }

	    public void Implement(IMethod method, AbcMethod abcMethod)
        {
            if (method == null) return;
            if (method.IsStatic) return;
            if (method.IsConstructor) return;
            var declType = method.DeclaringType;
            if (!declType.Is(SystemTypeCode.Object)) return;

            DefinePrototype(AvmTypeCode.Object, abcMethod);
            DefinePrototype(AvmTypeCode.String, abcMethod);
        }

	    private AbcMethod DefineString_GetTypeId()
	    {
		    return Abc.DefineMethod(
			    Sig.@global(AvmTypeCode.Int32),
			    code =>
				    {
					    code.PushTypeId(SystemTypes.String);
					    code.ReturnValue();
				    });
	    }
    }
}