using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.CodeModel.TypeSystem;

namespace DataDynamics.PageFX.CLI.IL
{
    public static class CIL
    {
        #region StackBehaviour
        internal static int GetPopCount(IMethod method, Instruction instruction)
        {
            var c = instruction.Code;
            if (c == InstructionCode.Ret)
            {
                if (method.IsVoid())
                    return 0;
                return 1;
            }
            //if (c == InstructionCode.Stfld)
            //    return 2;
            //if (c == InstructionCode.Stsfld)
            //    return 1;

            switch (instruction.OpCode.StackBehaviourPop)
            {
                case StackBehaviour.Pop0:
                    return 0;

                case StackBehaviour.Pop1:
                case StackBehaviour.Popi:
                case StackBehaviour.Popref:
                    return 1;

                case StackBehaviour.Pop1_pop1:
                case StackBehaviour.Popi_pop1:
                case StackBehaviour.Popi_popi:
                case StackBehaviour.Popi_popi8:
                case StackBehaviour.Popi_popr4:
                case StackBehaviour.Popi_popr8:
                case StackBehaviour.Popref_pop1:
                case StackBehaviour.Popref_popi:
                    return 2;

                case StackBehaviour.Popi_popi_popi:
                case StackBehaviour.Popref_popi_popi:
                case StackBehaviour.Popref_popi_popi8:
                case StackBehaviour.Popref_popi_popr4:
                case StackBehaviour.Popref_popi_popr8:
                case StackBehaviour.Popref_popi_popref:
                case StackBehaviour.Popref_popi_pop1:
                    return 3;

                case StackBehaviour.Varpop:
                    {
                        //TODO:
                        return -1;
                    }

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        internal static int GetPushCount(Instruction instruction)
        {
            //InstructionCode c = instruction.Code;
            
            switch (instruction.OpCode.StackBehaviourPush)
            {
                case StackBehaviour.Push0:
                    return 0;
                case StackBehaviour.Push1:
                case StackBehaviour.Pushi:
                case StackBehaviour.Pushi8:
                case StackBehaviour.Pushr4:
                case StackBehaviour.Pushr8:
                case StackBehaviour.Pushref:
                    return 1;
                case StackBehaviour.Push1_push1:
                    return 2;
                
                case StackBehaviour.Varpush:
                    {
                        //TODO:
                        return -1;
                    }

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        #endregion

        #region OpCodes
        public const ushort MultiBytePrefix = 0xFE;

        public static OpCode? GetShortOpCode(byte code)
        {
            LoadAllOpCodes();
            var i = _short[code];
            if (i != null)
                return i.OpCode;
            return null;
        }

        public static OpCode? GetLongOpCode(byte code)
        {
            LoadAllOpCodes();
            var i = _long[code];
            if (i != null)
                return i.OpCode;
            return null;
        }

        public static OpCode[] GetOpCodes()
        {
            LoadAllOpCodes();
            return _all.ToArray();
        }

        public static int InstructionCount
        {
            get
            {
                LoadAllOpCodes();
                return _all.Count;
            }
        }

        private sealed class I
        {
            public readonly OpCode OpCode;
            public bool IsUsed;

            public I(OpCode c)
            {
                OpCode = c;
            }

            public override string ToString()
            {
                return string.Format("{0,15} [{1}, 0x{1:X2}]", OpCode, (ushort)OpCode.Value);
            }
        }
        static I[] _short;
        static I[] _long;
        static List<OpCode> _all;

        static void LoadAllOpCodes()
        {
            if (_all != null) return;
            _all = new List<OpCode>(226);
            _short = new I[256];
            _long = new I[256];
            const BindingFlags bf = BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField;
            var fields = typeof(OpCodes).GetFields(bf);
            foreach (var fi in fields)
            {
                var c = (OpCode)fi.GetValue(null);
                if (c.Size == 1)
                {
                    _short[c.Value] = new I(c);
                }
                else
                {
                    int i = (ushort)c.Value & 0xFF;
                    _long[i] = new I(c);
                }
                _all.Add(c);
            }
        }
        #endregion

        #region GetEquivalent
        public static InstructionCode? GetEquivalent(InstructionCode code)
        {
            switch (code)
            {
                case InstructionCode.Beq:
                    return InstructionCode.Beq_S;
                case InstructionCode.Beq_S:
                    return InstructionCode.Beq;
                case InstructionCode.Bge:
                    return InstructionCode.Bge_S;
                case InstructionCode.Bge_S:
                    return InstructionCode.Bge;
                case InstructionCode.Bge_Un:
                    return InstructionCode.Bge_Un_S;
                case InstructionCode.Bge_Un_S:
                    return InstructionCode.Bge_Un;
                case InstructionCode.Bgt:
                    return InstructionCode.Bgt_S;
                case InstructionCode.Bgt_S:
                    return InstructionCode.Bgt;
                case InstructionCode.Bgt_Un:
                    return InstructionCode.Bgt_Un_S;
                case InstructionCode.Bgt_Un_S:
                    return InstructionCode.Bgt_Un;
                case InstructionCode.Ble:
                    return InstructionCode.Ble_S;
                case InstructionCode.Ble_S:
                    return InstructionCode.Ble;
                case InstructionCode.Ble_Un:
                    return InstructionCode.Ble_Un_S;
                case InstructionCode.Ble_Un_S:
                    return InstructionCode.Ble_Un;
                case InstructionCode.Blt:
                    return InstructionCode.Blt_S;
                case InstructionCode.Blt_S:
                    return InstructionCode.Blt;
                case InstructionCode.Blt_Un:
                    return InstructionCode.Blt_Un_S;
                case InstructionCode.Blt_Un_S:
                    return InstructionCode.Blt_Un;
                case InstructionCode.Bne_Un:
                    return InstructionCode.Bne_Un_S;
                case InstructionCode.Bne_Un_S:
                    return InstructionCode.Bne_Un;
                case InstructionCode.Br:
                    return InstructionCode.Br_S;
                case InstructionCode.Br_S:
                    return InstructionCode.Br;
                case InstructionCode.Brfalse:
                    return InstructionCode.Brfalse_S;
                case InstructionCode.Brfalse_S:
                    return InstructionCode.Brfalse;
                case InstructionCode.Brtrue:
                    return InstructionCode.Brtrue_S;
                case InstructionCode.Brtrue_S:
                    return InstructionCode.Brtrue;
                case InstructionCode.Leave:
                    return InstructionCode.Leave_S;
                case InstructionCode.Leave_S:
                    return InstructionCode.Leave;
                case InstructionCode.Ldarg:
                    return InstructionCode.Ldarga_S;
                case InstructionCode.Ldarg_S:
                    return InstructionCode.Ldarg;
                case InstructionCode.Ldarga:
                    return InstructionCode.Ldarga_S;
                case InstructionCode.Ldarga_S:
                    return InstructionCode.Ldarga;
                case InstructionCode.Ldloc:
                    return InstructionCode.Ldloc_S;
                case InstructionCode.Ldloc_S:
                    return InstructionCode.Ldloc;
                case InstructionCode.Ldloca:
                    return InstructionCode.Ldloca_S;
                case InstructionCode.Ldloca_S:
                    return InstructionCode.Ldloca;
                case InstructionCode.Starg:
                    return InstructionCode.Starg_S;
                case InstructionCode.Starg_S:
                    return InstructionCode.Starg;
                case InstructionCode.Stloc:
                    return InstructionCode.Stloc_S;
                case InstructionCode.Stloc_S:
                    return InstructionCode.Stloc;
            }
            return null;
        }
        #endregion

        #region Coverage
        public static float Coverage
        {
            get { return _coveredCount / (float)InstructionCount; }
        }
        static int _coveredCount;

        public static void ResetCoverage()
        {
            LoadAllOpCodes();
            foreach (var i in _short)
                if (i != null)
                    i.IsUsed = false;
            foreach (var i in _long)
                if (i != null)
                    i.IsUsed = false;
            _coveredCount = 0;
        }

        static void UpdateCoverageCore(ushort code)
        {
            int i = code & 0xFF;
            if (code >> 8 == MultiBytePrefix)
            {
                if (!_long[i].IsUsed)
                {
                    _long[i].IsUsed = true;
                    ++_coveredCount;
                }
            }
            else
            {
                if (!_short[i].IsUsed)
                {
                    _short[i].IsUsed = true;
                    ++_coveredCount;
                }
            }
        }

        internal static void UpdateCoverage(InstructionCode code)
        {
            LoadAllOpCodes();
            UpdateCoverageCore((ushort)code);
            var eq = GetEquivalent(code);
            if (eq != null)
            {
                UpdateCoverageCore((ushort)eq.Value);
            }
        }

        internal static void UpdateCoverage(IEnumerable<Instruction> list)
        {
            foreach (var instruction in list)
            {
                UpdateCoverage(instruction.Code);
            }
        }

        static IEnumerable<I> GetAllInstructions()
        {
            return _short.Concat(_long);
        }

        public static void DumpCoverage(TextWriter writer)
        {
            writer.WriteLine("CIL Instruction Set Coverage: {0}% ({1} from {2})",
                             Coverage * 100, _coveredCount, InstructionCount);

            writer.WriteLine("--------------------------------------------------");
            writer.WriteLine("Not covered instructions:");
            writer.WriteLine("--------------------------------------------------");
            foreach (var i in GetAllInstructions())
                if (i != null && !i.IsUsed)
                    writer.WriteLine(i);

            writer.WriteLine();
            writer.WriteLine("--------------------------------------------------");
            writer.WriteLine("Covered instructions:");
            writer.WriteLine("--------------------------------------------------");
            foreach (var i in GetAllInstructions())
                if (i != null && i.IsUsed)
                    writer.WriteLine(i);
        }

        public static void DumpCoverage(string path)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            using (var writer = new StreamWriter(path))
                DumpCoverage(writer);
        }
        #endregion
    }
}