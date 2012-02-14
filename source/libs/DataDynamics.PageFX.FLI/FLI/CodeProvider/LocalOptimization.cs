//#define DUMP_PATOPT

using System;
using System.Collections.Generic;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.FLI.ABC;
using DataDynamics.PageFX.FLI.IL;

namespace DataDynamics.PageFX.FLI
{
	using PatternAction = System.Func<AbcFile, IInstruction[], int, IInstruction[]>;
	using PatternPredicate = System.Func<IInstruction[], int, Instruction, bool>;

    internal partial class AvmCodeProvider
    {
        class Pattern
        {
            public Pattern(string name,
                PatternAction action,
                params PatternPredicate[] predicates)
            {
#if DUMP_PATOPT
                Name = name;
#endif
                _action = action;
                _predicates = predicates;
            }

#if DUMP_PATOPT
            public readonly string Name;
            public int Counter;
#endif
            readonly PatternAction _action;
            readonly PatternPredicate[] _predicates;

            public int Length
            {
                get { return _predicates.Length; }
            }

            public IInstruction[] Replace(AbcFile abc, IInstruction[] code, int index)
            {
                int n = code.Length;
                int pn = Length;
                if (index + pn > n) return null;
                for (int i = index, j = 0; j < pn; ++i, ++j)
                {
                    var instr = (Instruction)code[i];
                    if (!_predicates[j](code, index, instr))
                        return null;
                }
#if DUMP_PATOPT
                ++Counter;
                if (_dumper == null)
                    _dumper = new PatternDumper();
#endif
                return _action(abc, code, index);
            }

#if DUMP_PATOPT
            static PatternDumper _dumper;
#endif
        }

        static bool Pat0(Instruction instr)
        {
            if (instr.IsGetLocal)
                return true;
            switch (instr.Code)
            {
                case InstructionCode.Pushnull:
                case InstructionCode.Pushundefined:
                    return true;
            }
            return false;
        }

        static bool PatSwap(Instruction instr)
        {
            if (instr.IsGetLocal) return true;
            if (instr.IsPushConst) return true;
            switch (instr.Code)
            {
                case InstructionCode.Getlex:
                    return true;
            }
            return false;
        }

        static bool IgnoreBoolCast(Instruction instr)
        {
            switch (instr.Code)
            {
                case InstructionCode.Convert_b:
                case InstructionCode.Coerce_b:
                case InstructionCode.Convert_i:
                case InstructionCode.Coerce_i:
                case InstructionCode.Convert_u:
                case InstructionCode.Coerce_u:
                    return true;
            }
            return false;
        }

        static bool IfTrueOrFalse(Instruction instr)
        {
            return instr.Code == InstructionCode.Iftrue || instr.Code == InstructionCode.Iffalse;
        }

        static bool IsOneOnStack(Instruction instr)
        {
            if (instr.Code == InstructionCode.Pushbyte)
            {
                var v = instr.Operands[0].ToInt32();
                if (v == 1)
                    return true;
            }
            else if (instr.Code == InstructionCode.Pushint)
            {
                var c = instr.Operands[0].Value as AbcConst<int>;
                if ((c != null) && (c.Value == 1))
                    return true;
            }
            else if (instr.Code == InstructionCode.Pushuint)
            {
                var c = instr.Operands[0].Value as AbcConst<uint>;
                if ((c != null) && (c.Value == 1))
                    return true;
            }
            else if (instr.Code == InstructionCode.Pushdouble)
            {
                var c = instr.Operands[0].Value as AbcConst<double>;
                if ((c != null) && (c.Value == 1))
                    return true;
            }
            return false;
        }

        static bool IsLoadThis(Instruction instr)
        {
            if (instr.Code == InstructionCode.Getlocal0)
                return true;
            if (instr.Code == InstructionCode.Getlocal)
                return (int)instr.Operand == 0;
            return false;
        }

        static bool IsGetlexPrimitive(Instruction instr)
        {
            if (instr.Code != InstructionCode.Getlex) return false;
            var mn = instr.Operand as AbcMultiname;
            if (mn == null) return false;
            if (mn.Kind != AbcConstKind.QName) return false;
            if (mn.NamespaceString != "System") return false;
            switch (mn.NameString)
            {
                case "Int32":
                case "UInt32":
                case "Double":
                case "Single":
                case "Int16":
                case "UInt16":
                case "Byte":
                case "SByte":
                case "Boolean":
                case "Char":
                    return true;
            }
            return false;
        }

        static bool IsCallProperty(Instruction instr, string ns, string name)
        {
            if (instr.Code != InstructionCode.Callproperty) return false;
            var mn = instr.Operand as AbcMultiname;
            if (mn == null) return false;
            if (mn.Kind != AbcConstKind.QName) return false;
            if (mn.NamespaceString != ns) return false;
            return mn.NameString == name;
        }

        static bool IsUnboxCall(Instruction instr)
        {
            return IsCallProperty(instr, Const.Namespaces.PFX, Const.Boxing.MethodUnbox);
        }

        private static readonly Pattern[] Patterns =
            new[]
                {
                    new Pattern(
                        "i, Pop, i",
                        (abc, code, index) => new[] {code[index]},
                        (code, index, i) => Pat0(i),
                        (code, index, i) => i.Code == InstructionCode.Pop,
                        (code, index, i) => Equals(i, code[index])
                        ),

                    new Pattern(
                        "IgnoreBoolCast, IfTrueOrFalse",
                        (abc, code, index) => new[] {code[index + 1]},
                        (code, index, i) => IgnoreBoolCast(i),
                        (code, index, i) => IfTrueOrFalse(i)
                        ),

                    #region Ifeq, Ifstricteq
                    new Pattern(
                        "Ifeq1",
                        (abc, code, index) => new IInstruction[] {new Instruction(InstructionCode.Ifeq)},
                        (code, index, i) => i.Code == InstructionCode.Equals,
                        (code, index, i) => i.Code == InstructionCode.Iftrue
                        ),

                    new Pattern(
                        "Ifeq2",
                        (abc, code, index) => new IInstruction[] {new Instruction(InstructionCode.Ifeq)},
                        (code, index, i) => i.Code == InstructionCode.Equals,
                        (code, index, i) => IgnoreBoolCast(i),
                        (code, index, i) => i.Code == InstructionCode.Iftrue
                        ),

                    new Pattern(
                        "Ifeq3",
                        (abc, code, index) => new IInstruction[] {new Instruction(InstructionCode.Ifeq)},
                        (code, index, i) => i.Code == InstructionCode.Equals,
                        (code, index, i) => i.Code == InstructionCode.Not,
                        (code, index, i) => i.Code == InstructionCode.Iffalse
                        ),

                    new Pattern(
                        "Ifeq4",
                        (abc, code, index) => new IInstruction[] {new Instruction(InstructionCode.Ifeq)},
                        (code, index, i) => i.Code == InstructionCode.Equals,
                        (code, index, i) => i.Code == InstructionCode.Not,
                        (code, index, i) => IgnoreBoolCast(i),
                        (code, index, i) => i.Code == InstructionCode.Iffalse
                        ),

                    new Pattern(
                        "Ifstricteq1",
                        (abc, code, index) => new IInstruction[] {new Instruction(InstructionCode.Ifstricteq)},
                        (code, index, i) => i.Code == InstructionCode.Strictequals,
                        (code, index, i) => i.Code == InstructionCode.Iftrue
                        ),

                    new Pattern(
                        "Ifstricteq2",
                        (abc, code, index) => new IInstruction[] {new Instruction(InstructionCode.Ifstricteq)},
                        (code, index, i) => i.Code == InstructionCode.Strictequals,
                        (code, index, i) => IgnoreBoolCast(i),
                        (code, index, i) => i.Code == InstructionCode.Iftrue
                        ),

                    new Pattern(
                        "Ifstricteq3",
                        (abc, code, index) => new IInstruction[] {new Instruction(InstructionCode.Ifstricteq)},
                        (code, index, i) => i.Code == InstructionCode.Strictequals,
                        (code, index, i) => i.Code == InstructionCode.Not,
                        (code, index, i) => i.Code == InstructionCode.Iffalse
                        ),

                    new Pattern(
                        "Ifstricteq4",
                        (abc, code, index) => new IInstruction[] {new Instruction(InstructionCode.Ifstricteq)},
                        (code, index, i) => i.Code == InstructionCode.Strictequals,
                        (code, index, i) => i.Code == InstructionCode.Not,
                        (code, index, i) => IgnoreBoolCast(i),
                        (code, index, i) => i.Code == InstructionCode.Iffalse
                        ),

                    #endregion

                    #region Ifne, Ifstrictne
                    new Pattern(
                        "Ifne1",
                        (abc, code, index) => new IInstruction[] {new Instruction(InstructionCode.Ifne)},
                        (code, index, i) => i.Code == InstructionCode.Equals,
                        (code, index, i) => i.Code == InstructionCode.Iffalse
                        ),

                    new Pattern(
                        "Ifne2",
                        (abc, code, index) => new IInstruction[] {new Instruction(InstructionCode.Ifne)},
                        (code, index, i) => i.Code == InstructionCode.Equals,
                        (code, index, i) => IgnoreBoolCast(i),
                        (code, index, i) => i.Code == InstructionCode.Iffalse
                        ),

                    new Pattern(
                        "Ifne3",
                        (abc, code, index) => new IInstruction[] {new Instruction(InstructionCode.Ifne)},
                        (code, index, i) => i.Code == InstructionCode.Equals,
                        (code, index, i) => i.Code == InstructionCode.Not,
                        (code, index, i) => i.Code == InstructionCode.Iftrue
                        ),

                    new Pattern(
                        "Ifne4",
                        (abc, code, index) => new IInstruction[] {new Instruction(InstructionCode.Ifne)},
                        (code, index, i) => i.Code == InstructionCode.Equals,
                        (code, index, i) => i.Code == InstructionCode.Not,
                        (code, index, i) => IgnoreBoolCast(i),
                        (code, index, i) => i.Code == InstructionCode.Iftrue
                        ),

                    new Pattern(
                        "Ifstrictne1",
                        (abc, code, index) => new IInstruction[] {new Instruction(InstructionCode.Ifstrictne)},
                        (code, index, i) => i.Code == InstructionCode.Strictequals,
                        (code, index, i) => i.Code == InstructionCode.Iffalse
                        ),

                    new Pattern(
                        "Ifstrictne2",
                        (abc, code, index) => new IInstruction[] {new Instruction(InstructionCode.Ifstrictne)},
                        (code, index, i) => i.Code == InstructionCode.Strictequals,
                        (code, index, i) => IgnoreBoolCast(i),
                        (code, index, i) => i.Code == InstructionCode.Iffalse
                        ),

                    new Pattern(
                        "Ifstrictne3",
                        (abc, code, index) => new IInstruction[] {new Instruction(InstructionCode.Ifstrictne)},
                        (code, index, i) => i.Code == InstructionCode.Strictequals,
                        (code, index, i) => i.Code == InstructionCode.Not,
                        (code, index, i) => i.Code == InstructionCode.Iftrue
                        ),

                    new Pattern(
                        "Ifstrictne4",
                        (abc, code, index) => new IInstruction[] {new Instruction(InstructionCode.Ifstrictne)},
                        (code, index, i) => i.Code == InstructionCode.Strictequals,
                        (code, index, i) => i.Code == InstructionCode.Not,
                        (code, index, i) => IgnoreBoolCast(i),
                        (code, index, i) => i.Code == InstructionCode.Iftrue
                        ),

                    #endregion

                    #region Inclocal
                    new Pattern(
                        "Inclocal_i_Dup",
                        (abc, code, index) =>
                        new[]
                            {
                                code[index],
                                new Instruction(InstructionCode.Inclocal_i, ((Instruction)code[index]).LocalIndex)
                            },
                        (code, index, i) => i.IsGetLocal,
                        (code, index, i) => i.Code == InstructionCode.Dup,
                        (code, index, i) => IsOneOnStack(i),
                        (code, index, i) => i.Code == InstructionCode.Add_i,
                        (code, index, i) => i.IsSetLocal && i.LocalIndex == ((Instruction)code[index]).LocalIndex
                    ),
                    
                    new Pattern(
                        "Inclocal_i",
                        (abc, code, index) =>
                        new IInstruction[]
                            {
                                new Instruction(InstructionCode.Inclocal_i, ((Instruction)code[index]).LocalIndex)
                            },
                        (code, index, i) => i.IsGetLocal,
                        (code, index, i) => IsOneOnStack(i),
                        (code, index, i) => i.Code == InstructionCode.Add_i,
                        (code, index, i) => i.IsSetLocal && i.LocalIndex == ((Instruction)code[index]).LocalIndex
                    ),

                    new Pattern(
                        "Inclocal_Dup",
                        (abc, code, index) =>
                        new[]
                            {
                                code[index],
                                new Instruction(InstructionCode.Inclocal, ((Instruction)code[index]).LocalIndex)
                            },
                        (code, index, i) => i.IsGetLocal,
                        (code, index, i) => i.Code == InstructionCode.Dup,
                        (code, index, i) => IsOneOnStack(i),
                        (code, index, i) => i.Code == InstructionCode.Add,
                        (code, index, i) => i.IsSetLocal && i.LocalIndex == ((Instruction)code[index]).LocalIndex
                    ),

                    new Pattern(
                        "Inclocal",
                        (abc, code, index) =>
                        new IInstruction[]
                            {
                                new Instruction(InstructionCode.Inclocal, ((Instruction)code[index]).LocalIndex)
                            },
                        (code, index, i) => i.IsGetLocal,
                        (code, index, i) => IsOneOnStack(i),
                        (code, index, i) => i.Code == InstructionCode.Add,
                        (code, index, i) => i.IsSetLocal && i.LocalIndex == ((Instruction)code[index]).LocalIndex
                    ),
                    #endregion
                    
                    #region Declocal
                    new Pattern(
                        "Declocal_i_Dup",
                        (abc, code, index) =>
                        new[]
                            {
                                code[index],
                                new Instruction(InstructionCode.Declocal_i, ((Instruction)code[index]).LocalIndex)
                            },
                        (code, index, i) => i.IsGetLocal,
                        (code, index, i) => i.Code == InstructionCode.Dup,
                        (code, index, i) => IsOneOnStack(i),
                        (code, index, i) => i.Code == InstructionCode.Subtract_i,
                        (code, index, i) => i.IsSetLocal && i.LocalIndex == ((Instruction)code[index]).LocalIndex
                    ),
                    
                    new Pattern(
                        "Declocal_i",
                        (abc, code, index) =>
                        new IInstruction[]
                            {
                                new Instruction(InstructionCode.Declocal_i, ((Instruction)code[index]).LocalIndex)
                            },
                        (code, index, i) => i.IsGetLocal,
                        (code, index, i) => IsOneOnStack(i),
                        (code, index, i) => i.Code == InstructionCode.Subtract_i,
                        (code, index, i) => i.IsSetLocal && i.LocalIndex == ((Instruction)code[index]).LocalIndex
                    ),

                    new Pattern(
                        "Declocal_Dup",
                        (abc, code, index) =>
                        new[]
                            {
                                code[index],
                                new Instruction(InstructionCode.Declocal, ((Instruction)code[index]).LocalIndex)
                            },
                        (code, index, i) => i.IsGetLocal,
                        (code, index, i) => i.Code == InstructionCode.Dup,
                        (code, index, i) => IsOneOnStack(i),
                        (code, index, i) => i.Code == InstructionCode.Subtract,
                        (code, index, i) => i.IsSetLocal && i.LocalIndex == ((Instruction)code[index]).LocalIndex
                    ),

                    new Pattern(
                        "Declocal",
                        (abc, code, index) =>
                        new IInstruction[]
                            {
                                new Instruction(InstructionCode.Declocal, ((Instruction)code[index]).LocalIndex)
                            },
                        (code, index, i) => i.IsGetLocal,
                        (code, index, i) => IsOneOnStack(i),
                        (code, index, i) => i.Code == InstructionCode.Subtract,
                        (code, index, i) => i.IsSetLocal && i.LocalIndex == ((Instruction)code[index]).LocalIndex
                    ),
                    #endregion
                    
                    new Pattern("UnboxPrimitiveThis.Swap",
                                (abc, code, index) => new IInstruction[]
                                                          {
                                                              new Instruction(InstructionCode.Getlocal0),
                                                              new Instruction(InstructionCode.Getproperty,
                                                                              abc.DefineGlobalQName(Const.Boxing.Value))
                                                          },
                                (code, index, i) => IsLoadThis(i),
                                (code, index, i) => IsGetlexPrimitive(i),
                                (code, index, i) => i.Code == InstructionCode.Swap,
                                (code, index, i) => IsUnboxCall(i)
                        ),

                    new Pattern("UnboxPrimitiveThis",
                                (abc, code, index) => new IInstruction[]
                                                          {
                                                              new Instruction(InstructionCode.Getlocal0),
                                                              new Instruction(InstructionCode.Getproperty,
                                                                              abc.DefineGlobalQName(Const.Boxing.Value))
                                                          },
                                (code, index, i) => IsGetlexPrimitive(i),
                                (code, index, i) => IsLoadThis(i),
                                (code, index, i) => IsUnboxCall(i)
                        ),

                    new Pattern(
                        "Swap",
                        (abc, code, index) => new[] {code[index + 1], code[index]},
                        (code, index, i) => PatSwap(i),
                        (code, index, i) => PatSwap(i),
                        (code, index, i) => i.Code == InstructionCode.Swap
                        ),

                    new Pattern(
                        "Increment_i",
                        (abc, code, index) => new IInstruction[] {new Instruction(InstructionCode.Increment_i)},
                        (code, index, i) => IsOneOnStack(i),
                        (code, index, i) => i.Code == InstructionCode.Add_i
                        ),

                    new Pattern(
                        "Increment",
                        (abc, code, index) => new IInstruction[] {new Instruction(InstructionCode.Increment)},
                        (code, index, i) => IsOneOnStack(i),
                        (code, index, i) => i.Code == InstructionCode.Add
                        ),

                    new Pattern(
                        "Decrement_i",
                        (abc, code, index) => new IInstruction[] {new Instruction(InstructionCode.Decrement_i)},
                        (code, index, i) => IsOneOnStack(i),
                        (code, index, i) => i.Code == InstructionCode.Subtract_i
                        ),

                    new Pattern(
                        "Decrement",
                        (abc, code, index) => new IInstruction[] {new Instruction(InstructionCode.Decrement)},
                        (code, index, i) => IsOneOnStack(i),
                        (code, index, i) => i.Code == InstructionCode.Subtract
                        )
            };

        /// <summary>
        /// Performs local optimization.
        /// </summary>
        /// <param name="code">instruction set to optimize</param>
        /// <returns>optimized instruction set</returns>
        public IInstruction[] OptimizeBasicBlock(IInstruction[] code)
        {
            if (code == null)
                throw new ArgumentNullException("code");

            var list = new List<IInstruction>();
            int n = code.Length;
            bool noopt = true;
            for (int index = 0; index < n; ++index)
            {
                bool add = true;
                for (int j = 0; j < Patterns.Length; ++j)
                {
                    var p = Patterns[j];
                    var r = p.Replace(_abc, code, index);
                    if (r != null)
                    {
                        list.AddRange(r);
                        index += p.Length - 1;
                        add = false;
                        noopt = false;
                        break;
                    }
                }
                if (add)
                    list.Add(code[index]);
            }
            if (noopt) return code;
            return list.ToArray();
        }

#if DUMP_PATOPT
        class PatternDumper : IDisposable
        {
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            bool _dump = true;

            void Dispose(bool disposing)
            {
                if (_dump)
                {
                    _dump = false;
                    using (var writer = new StreamWriter(@"c:\patopt.txt"))
                    {
                        foreach (var pattern in _patterns)
                        {
                            writer.WriteLine("{0}: {1}", pattern.Name, pattern.Counter);
                        }
                    }
                }
            }

            ~PatternDumper()
            {
                Dispose(false);
            }
        }
#endif
    }
}