using System.Reflection.Emit;
using DataDynamics.PageFX.Common;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.CLI.IL
{
	internal static class InstructionExtensions
	{
		public static bool IsBranchOrSwitch(this IInstruction i)
		{
			return i.IsBranch || i.IsSwitch;
		}

		public static bool IsCall(this Instruction i)
		{
			if (i == null) return false;
			return i.FlowControl == FlowControl.Call;
		}

		/// <summary>
		/// Determines whether given instruction performs call of non static method
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public static bool HasReceiver(this Instruction i)
		{
			if (i.Code == InstructionCode.Newobj)
				return false;
			var m = i.Method;
			if (m == null)
				return false;
			return !m.IsStatic;
		}

		public static bool HasReturnValue(this Instruction i)
		{
			if (i.Code == InstructionCode.Newobj)
				return true;

			var m = i.Method;
			return m != null && !m.IsVoid();
		}

		public static bool IsEndOfBasicBlock(this IInstruction instr)
		{
			return instr.IsBranch || instr.IsSwitch || instr.IsReturn || instr.IsThrow;
		}
	}
}