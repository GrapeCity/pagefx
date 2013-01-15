using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Core.IL;
using DataDynamics.PageFX.Core.Translation.Values;

namespace DataDynamics.PageFX.Core.Translation
{
	internal static class PointerOperations
	{
		/// <summary>
		/// Reads value at given mock field ptr
		/// </summary>
		/// <param name="code"></param>
		/// <param name="ptr"></param>
		public static Code LoadFieldPtr(this Code code, MockFieldPtr ptr)
		{
			if (ptr.IsInstance)
			{
				return code.LoadTempVar(ptr.obj)
				           .LoadField(ptr.field)
				           .KillTempVar(ptr.obj);
			}
			return code.LoadField(ptr.field);
		}

		/// <summary>
		/// Writes value at given mock field ptr. The value must be onto the stack.
		/// </summary>
		/// <param name="code"></param>
		/// <param name="ptr"></param>
		public static Code StoreFieldPtr(this Code code, MockFieldPtr ptr)
		{
			if (ptr.IsInstance)
			{
				return code.LoadTempVar(ptr.obj)
				           .Swap()
				           .StoreField(ptr.field)
				           .KillTempVar(ptr.obj);
			}
			return code.StoreField(ptr.field);
		}

		/// <summary>
		/// Reads value at given mock elem ptr.
		/// </summary>
		/// <param name="code"></param>
		/// <param name="ptr"></param>
		public static Code LoadElemPtr(this Code code, MockElemPtr ptr)
		{
			return code.LoadTempVar(ptr.arr)
			           .LoadTempVar(ptr.index)
			           .GetArrayElem(ptr.elemType)
			           .KillTempVar(ptr.arr)
			           .KillTempVar(ptr.index);
		}

		/// <summary>
		/// Writes value at given mock elem ptr. The value must be onto the stack.
		/// </summary>
		/// <param name="code"></param>
		/// <param name="ptr"></param>
		public static Code StoreElemPtr(this Code code, MockElemPtr ptr)
		{
			return code.LoadTempVar(ptr.arr)
			           .Swap()
			           .LoadTempVar(ptr.index)
			           .Swap()
			           .SetArrayElem(ptr.elemType)
			           .KillTempVar(ptr.arr)
			           .KillTempVar(ptr.index);
		}

		public static Code LoadPtr(this Code code, IValue value)
		{
			if (!value.IsPointer) return code;

			if (value.IsMockPointer)
			{
				switch (value.Kind)
				{
					case ValueKind.MockThisPtr:
						return code.LoadThis();

					case ValueKind.MockArgPtr:
						return code.LoadArgument(((MockArgPtr)value).arg);

					case ValueKind.MockVarPtr:
						return code.LoadVariable(((MockVarPtr)value).var);

					case ValueKind.MockFieldPtr:
						return code.LoadFieldPtr((MockFieldPtr)value);

					case ValueKind.MockElemPtr:
						return code.LoadElemPtr((MockElemPtr)value);

					default:
						throw new ILTranslatorException();
				}
			}

			return code.LoadIndirect(value.Type);
		}

		public static Code StorePtr(this Code code, IValue addr, IType valueType)
		{
			if (addr.IsMockPointer)
			{
				switch (addr.Kind)
				{
					case ValueKind.MockThisPtr:
						return code.StoreThis();

					case ValueKind.MockArgPtr:
						return code.StoreArgument(((MockArgPtr)addr).arg);

					case ValueKind.MockVarPtr:
						return code.StoreVariable(((MockVarPtr)addr).var);

					case ValueKind.MockFieldPtr:
						return code.StoreFieldPtr((MockFieldPtr)addr);

					case ValueKind.MockElemPtr:
						return code.StoreElemPtr((MockElemPtr)addr);

					default:
						throw new ILTranslatorException();
				}
			}
			return code.StoreIndirect(valueType);
		}
	}
}
