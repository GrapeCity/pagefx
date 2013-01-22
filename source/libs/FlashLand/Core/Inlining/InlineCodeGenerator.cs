using System;
using System.Collections.Generic;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Abc;
using DataDynamics.PageFX.FlashLand.Core.SpecialTypes;
using DataDynamics.PageFX.FlashLand.IL;

namespace DataDynamics.PageFX.FlashLand.Core.Inlining
{
    internal static class InlineCodeGenerator
    {
	    private static readonly Dictionary<string, InlineCodeProvider> Inlines =
		    new Dictionary<string, InlineCodeProvider>
			    {
				    {"System.Object", new SystemObjectInlines()},
				    {"System.String", new StringInlines()},
				    {"System.Diagnostics.Debugger", new DebuggerInlines()},
				    {"avm", new AvmInlines()},
					{"Native.NativeArray", new AvmArrayInlines()},
					{"Native.ByteArray", new FlashByteArrayInlines()},
			    };

		private static readonly Dictionary<string, InlineCodeProvider> AvmInlines =
			new Dictionary<string, InlineCodeProvider>
			    {
				    {"Object", new AvmObjectInlines()},
				    {"Class", new AvmClassInlines()},
				    {"Array", new AvmArrayInlines()},
				    {"XML", new AvmXmlInlines()},
				    {"XMLList", new AvmXmlInlines()},
			    };

		public static AbcCode Build(AbcFile abc, AbcInstance instance, IMethod method)
	    {
		    if (instance != null && instance.IsNative)
            {
				var mn = instance.Name;
				string name = mn.FullName;

				InlineCodeProvider provider;
				if (AvmInlines.TryGetValue(name, out provider))
				{
					return provider.GetImplementation(abc, method);
				}

				if (name.StartsWith(AS3.Vector.FullName))
                {
                    return AvmVectorInlines.Get(abc, method);
                }
            }

            var type = method.DeclaringType;
		    if (type.Data is IVectorType)
		    {
			    return AvmVectorInlines.Get(abc, method);
		    }

		    return GetImpl(abc, type, method);
        }

	    private static AbcCode GetImpl(AbcFile abc, IType type, IMethod method)
        {
			var info = method.GetInlineInfo();
			if (info != null)
			{
				return Call(abc, method, info);
			}

		    InlineCodeProvider provider;
	        if (Inlines.TryGetValue(type.FullName, out provider))
	        {
		        return provider.GetImplementation(abc, method);
	        }

		    return null;
        }

	    private static AbcCode Call(AbcFile abc, IMethod method, InlineMethodInfo info)
	    {
		    var code = new AbcCode(abc);
		    var name = info.Name.Define(abc);

		    switch (info.Kind)
		    {
			    case InlineKind.Property:
				    if (method.IsSetter())
				    {
					    code.SetProperty(name);
				    }
				    else
				    {
					    code.GetProperty(name);
					    code.Coerce(method.Type, true);
				    }
				    break;

			    case InlineKind.Operator:
				    {
					    int n = method.Parameters.Count;
					    if (n <= 1)
						    throw new InvalidOperationException();
					    var op = info.Op;
					    for (int i = 1; i < n; ++i)
					    {
						    code.Add(op);
					    }
					    code.Coerce(method.Type, true);
				    }
				    break;

			    default:
				    if (method.IsVoid())
				    {
					    code.CallVoid(name, method.Parameters.Count);
				    }
				    else
				    {
					    code.Call(name, method.Parameters.Count);
					    code.Coerce(method.Type, true);
				    }
				    break;
		    }

		    return code;
	    }
    }
}