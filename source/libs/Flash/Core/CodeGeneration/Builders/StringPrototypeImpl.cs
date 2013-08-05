using System;
using DataDynamics.PageFX.Common.Services;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Flash.Abc;
using DataDynamics.PageFX.Flash.Avm;
using DataDynamics.PageFX.Flash.IL;

namespace DataDynamics.PageFX.Flash.Core.CodeGeneration.Builders
{
	/// <summary>
	/// Provides implementaions of System.String interfaces for AVM String.prototype.
	/// </summary>
    internal sealed class StringPrototypeImpl
    {
	    private readonly AbcGenerator _generator;

	    public StringPrototypeImpl(AbcGenerator generator)
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

	    public void Implement(IMethod method)
        {
		    if (method == null)
				throw new ArgumentNullException("method");

		    var iface = method.DeclaringType;
            if (!iface.IsInterface) return;

            if (iface.Name.StartsWith("IEnumerable"))
            {
                GetEnumeratorImpl(method);
                return;
            }

            if (iface.Name.StartsWith("IComparable"))
            {
                CompareToImpl(method);
                return;
            }

            if (iface.Name.StartsWith("IEquatable"))
            {
                EqualsImpl(method);
                return;
            }

            switch (iface.Name)
            {
                case "IConvertible":
                    ConvertImpl(method);
                    return;

                case "ICloneable":
                    CloneImpl(method);
                    return;
            }
        }

	    //NOTE: Also used to implement IEnumerable<char>.GetEnumerator
	    private void GetEnumeratorImpl(IMethod method)
	    {
		    Impl(method,
		         code =>
			         {
				         var implType = _generator.Corlib.GetType(CorlibTypeId.CharEnumerator);
				         var ctor = implType.FindConstructor(1);
				         code.NewObject(ctor, () => code.LoadThis());
				         code.ReturnValue();
			         });
	    }

	    private void ConvertImpl(IMethod method)
        {
            var m = method.AbcMethod();
            if (m == null) return;

            string name = method.Name;

            if (name == "ToString")
            {
	            var impl = Abc.DefineMethod(
		            Sig.@global(AvmTypeCode.String),
		            code => code.ReturnThis());

	            _generator.NewApi.SetProtoFunction(AvmTypeCode.String, m.TraitName, impl);
				return;
            }

            if (name == "GetTypeCode")
            {
	            Impl(method,
	                 code =>
		                 {
			                 code.PushInt(18);
			                 code.ReturnValue();
		                 });
                return;
            }

		    Impl(method,
		         code =>
			         {
				         var convertInstance = _generator.Corlib.Convert.Instance;
				         var convertMethod = FindConvertImpl(method);
						 var impl = _generator.MethodBuilder.BuildAbcMethod(convertMethod);

				         code.Getlex(convertInstance);
				         code.GetLocals(0, method.Parameters.Count);
				         code.Call(impl);
				         code.ReturnValue();
			         });
        }

	    private IMethod FindConvertImpl(IMethod method)
	    {
		    var type = _generator.Corlib.Convert.Type;

		    IMethod convertMethod;
		    if (method.Name == "ToType")
		    {
			    convertMethod = type.Methods.Find(method.Name, 3);
		    }
		    else
		    {
			    int paramCount = method.Parameters.Count;
			    convertMethod = type.FindMethod(method.Name, paramCount + 1,
			                                    args => args[0].Type.Is(SystemTypeCode.String));
		    }

		    if (convertMethod == null)
			    throw new InvalidOperationException("Bad corlib");

		    return convertMethod;
	    }

	    private void CloneImpl(IMethod method)
	    {
		    Impl(method,
		         code =>
			         {
				         code.LoadThis();
				         code.ReturnValue();
			         });
	    }

	    //NOTE: Also used to implement IComparable<String>
        private void CompareToImpl(IMethod method)
        {
	        Impl(method,
	             code =>
		             {
			             var stringType = SystemTypes.String;
			             var objectType = SystemTypes.Object;
			             var cmp = stringType.Methods.Find("CompareTo", objectType);

			             if (cmp == null)
				             throw new InvalidOperationException("String has no CompareTo(object) method");

						 var impl = _generator.MethodBuilder.BuildAbcMethod(cmp);

			             code.Getlex(impl);
			             code.LoadThis();
			             code.GetLocal(1);
			             code.Call(impl);
			             code.ReturnValue();
		             });
        }

	    private void EqualsImpl(IMethod method)
	    {
		    Impl(method,
		         code =>
			         {
				         code.LoadThis();
				         code.GetLocal(1);
				         code.Add(InstructionCode.Equals);
				         code.FixBool();
				         code.ReturnValue();
			         });
	    }

	    private void Impl(IMethod method, AbcCoder coder)
		{
			var abcMethod = method.AbcMethod();
			if (abcMethod == null) return;
			_generator.NewApi.SetProtoFunction(AvmTypeCode.String, abcMethod, coder);
		}
    }
}