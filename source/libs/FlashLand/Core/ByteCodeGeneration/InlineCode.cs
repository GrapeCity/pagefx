using System;
using System.Collections.Generic;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Abc;
using DataDynamics.PageFX.FlashLand.Core.Inlining;
using DataDynamics.PageFX.FlashLand.Core.SpecialTypes;
using DataDynamics.PageFX.FlashLand.IL;

namespace DataDynamics.PageFX.FlashLand.Core.ByteCodeGeneration
{
    internal partial class AbcGenerator
    {
	    private static readonly Dictionary<string, InlineCodeProvider> Inlines =
		    new Dictionary<string, InlineCodeProvider>
			    {
				    {"System.Object", new SystemObjectInlines()},
				    {"System.String", new StringInlines()},
				    {"System.Diagnostics.Debugger", new DebuggerInlines()},
				    {"avm", new AvmInlines()},
			    };

		private static readonly Dictionary<string, InlineCodeProvider> NativeInlines =
			new Dictionary<string, InlineCodeProvider>
			    {
				    {"Object", new AvmObjectInlines()},
				    {"Class", new AvmClassInlines()},
				    {"Array", new AvmArrayInlines()},
				    {"XML", new AvmXmlInlines()},
				    {"XMLList", new AvmXmlInlines()},
			    };

	    private AbcCode DefineInlineCode(IMethod method, AbcInstance instance)
        {
            if (instance != null && instance.IsNative)
            {
				var mn = instance.Name;
				string name = mn.FullName;

				InlineCodeProvider provider;
				if (NativeInlines.TryGetValue(name, out provider))
				{
					return provider.GetImplementation(Abc, method);
				}

				if (name.StartsWith(AS3.Vector.FullName))
                {
                    return AvmVectorInlines.Get(Abc, method);
                }
            }

            var type = method.DeclaringType;
            if (type.Data is IVectorType)
                return AvmVectorInlines.Get(Abc, method);

            return GetInlineCode(type, method);
        }

	    private AbcCode GetInlineCode(IType type, IMethod method)
        {
			var info = method.GetInlineInfo();
			if (info != null)
			{
				return DefineInlineCode(method, info);
			}

		    InlineCodeProvider provider;
	        if (Inlines.TryGetValue(type.FullName, out provider))
	        {
		        return provider.GetImplementation(Abc, method);
	        }

		    return null;
        }

	    private AbcCode DefineInlineCode(IMethod method, InlineMethodInfo info)
	    {
		    var code = new AbcCode(Abc);
		    var name = info.Name.Define(Abc);

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

	internal static class InlineMethodExtensions
	{
		//TODO: PERF process method custom attributes only once (one iteration)
		//TODO: PERF cache inline info in custom attributes

		public static InlineMethodInfo GetInlineInfo(this IMethod method)
		{
			string name = null;
			string ns = null;

			var kns = KnownNamespace.Global;
			var kind = InlineKind.Function;

			foreach (var attr in method.CustomAttributes)
			{
				switch (attr.TypeName)
				{
					case Attrs.InlineFunction:
						name = attr.Arguments.Count == 0 ? method.Name : (string)attr.Arguments[0].Value;
						break;

					case Attrs.InlineProperty:
						name = attr.Arguments.Count == 0 ? method.Name : (string)attr.Arguments[0].Value;
						kind = InlineKind.Property;
						break;

					case Attrs.InlineOperator:
						name = attr.Arguments.Count == 0 ? method.Name : (string)attr.Arguments[0].Value;
						kind = InlineKind.Operator;
						break;

					case Attrs.AS3:
						kns = KnownNamespace.AS3;
						break;
				}
			}

			if (name == null)
				return null;

			var qname = ns != null ? new QName(name, ns) : new QName(name, kns);
			return new InlineMethodInfo(qname, kind);
		}
	}

	internal sealed class InlineMethodInfo
	{
		public readonly QName Name;
		public readonly InlineKind Kind;

		public InlineMethodInfo(QName name, InlineKind kind)
		{
			Name = name;
			Kind = kind;
		}

		public InstructionCode Op
		{
			get
			{
				switch (Name.Name)
				{
					case "+":
						return InstructionCode.Add;
					case "-":
						return InstructionCode.Subtract;
					case "/":
						return InstructionCode.Divide;
					case "*":
						return InstructionCode.Multiply;
					default:
						throw new InvalidOperationException();
				}
			}
		}
	}

	internal enum InlineKind
	{
		Function,
		Property,
		Operator
	}
}