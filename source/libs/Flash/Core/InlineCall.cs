using System;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Flash.Abc;
using DataDynamics.PageFX.Flash.Core.Inlining;
using DataDynamics.PageFX.Flash.IL;

namespace DataDynamics.PageFX.Flash.Core
{
	internal sealed class InlineCall : IMethodCall
	{
		public InlineCall(IMethod method, AbcMultiname targetType, AbcMultiname name, AbcCode inlineCode)
		{
			if (method == null)
				throw new ArgumentNullException("method");
			if (inlineCode == null)
				throw new ArgumentNullException("inlineCode");

			Method = method;
			TargetType = targetType;
			Name = name;
			InlineCode = inlineCode;
		}

		public IMethod Method { get; private set; }

		public AbcMultiname TargetType { get; private set; }

		public AbcMultiname Name { get; private set; }

		public AbcCode InlineCode { get; private set; }

		public static InlineCall Create(AbcFile abc, IMethod method, InlineMethodInfo info)
		{
			var code = new AbcCode(abc);
			var targetType = info.TargetType != null ? info.TargetType.Define(abc) : null;
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

			return new InlineCall(method, targetType, name, code);
		}
	}
}