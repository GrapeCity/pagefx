using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Abc;

namespace DataDynamics.PageFX.FlashLand.Core.Inlining
{
	internal static class InlineMethodExtensions
	{
		//TODO: PERF process method custom attributes only once (one iteration)
		//TODO: PERF cache inline info in custom attributes

		public static InlineMethodInfo GetInlineInfo(this IMethod method)
		{
			QName qname = null;
			string name = null;
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
						name = attr.Arguments.Count == 0 ? method.Association.Name : (string)attr.Arguments[0].Value;
						kind = InlineKind.Property;
						break;

					case Attrs.InlineOperator:
						name = attr.Arguments.Count == 0 ? method.Name : (string)attr.Arguments[0].Value;
						kind = InlineKind.Operator;
						break;

					case Attrs.AS3:
						kns = KnownNamespace.AS3;
						break;

					case Attrs.QName:
						qname = QName.FromAttribute(attr);
						break;
				}
			}

			if (name == null && qname == null)
			{
				return null;
			}

			if (qname == null)
			{
				qname = new QName(name, kns);
			}

			return new InlineMethodInfo(qname, kind);
		}
	}
}