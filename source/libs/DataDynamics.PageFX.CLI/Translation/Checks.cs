using System;
using System.Linq;
using DataDynamics.PageFX.CLI.IL;
using DataDynamics.PageFX.CLI.Translation.ControlFlow;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.CodeModel.TypeSystem;

namespace DataDynamics.PageFX.CLI.Translation
{
	internal static class Checks
	{
		public static void ItShouldBeArray(this EvalItem item)
		{
			var type = item.Type;
			if (type.TypeKind != TypeKind.Array)
				throw new InvalidOperationException();
		}

		public static void ItShouldBeNonPointer(this EvalItem item)
		{
			if (item.IsPointer)
				throw new InvalidOperationException();
		}

		public static void CheckValidCast(IType source, IType target)
		{
			if (IsInvalidCast(source, target))
			{
				throw new InvalidOperationException();
			}
		}

		public static bool IsInvalidCast(IType source, IType target)
		{
			if (source.IsNumeric())
			{
				if (!IsNumericOrEnumOrObject(target))
					return true;
			}
			else if (target.IsNumeric())
			{
				if (!IsNumericOrEnumOrObject(source))
					return true;
			}
			return false;
		}

		public static bool IsNumericOrEnumOrObject(IType type)
		{
			if (type == null) return false;
			return type.IsNumeric()
			       || type.IsEnum
			       || type.Is(SystemTypeCode.Object)
			       || type.Is(SystemTypeCode.ValueType);
		}

		public static void CheckStackBalance(Node bb)
		{
			int nbefore = bb.StackBefore.Count;
			if (bb.InEdges.Select(e => e.From).Any(from => @from.IsTranslated && nbefore != @from.Stack.Count))
			{
				throw new ILTranslatorException();
			}

			int nafter = bb.Stack.Count;
			if (bb.InEdges.Select(e => e.To).Any(to => to.IsTranslated && nafter != to.Stack.Count))
			{
				throw new ILTranslatorException();
			}
		}

		public const int MaxGenericNesting = 100;

		private static void CalcGenericNesting(IGenericInstance instance, ref int depth)
		{
			foreach (var arg in instance.GenericArguments.OfType<IGenericInstance>())
			{
				++depth;
				CalcGenericNesting(arg, ref depth);
			}
		}

		public static bool IsGenericNestingExceeds(TranslationContext context)
		{
			var gi = context.Method.DeclaringType as IGenericInstance;
			if (gi == null) return false;
			int depth = 1;
			CalcGenericNesting(gi, ref depth);
			return depth > MaxGenericNesting;
		}
	}
}
