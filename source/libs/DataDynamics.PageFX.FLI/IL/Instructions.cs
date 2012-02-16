using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;

namespace DataDynamics.PageFX.FLI.IL
{
    public static class Instructions
    {
        static List<Instruction> _list;

        static Instruction ParseInstruction(string s)
        {
            var row = s.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            var i = new Instruction
                        {
                            Name = row[1].Trim('\"')
                        };

            string scode = row[2];
            if (scode.StartsWith("0x"))
                scode = scode.Substring(2).Trim();
            i.Code = (InstructionCode)int.Parse(scode, NumberStyles.HexNumber);

            if (row[4].Trim() != "-1")
                i.IsUsed = true;

            i.FrameUse = int.Parse(row[5]);
            i.FrameSet = int.Parse(row[6]);
            i.StackPop = int.Parse(row[7]);
            i.StackPush = int.Parse(row[8]);
            i.CanThrow = int.Parse(row[9]) != 0;

            return i;
        }

        static Instruction Find(string name)
        {
            return _list.Find(i => i.Name == name);
        }

        static OperandType ParseOperandType(string s)
        {
            switch (s)
            {
                case "u8": return OperandType.U8;
                case "u16": return OperandType.U16;
                case "u24": return OperandType.U24;
                case "u32": return OperandType.U32;
                case "s24": return OperandType.S24;
                case "u30": return OperandType.U30;
                case "s30": return OperandType.S30;
                case "const_int": return OperandType.ConstInt;
                case "const_uint": return OperandType.ConstUInt;
                case "const_double": return OperandType.ConstDouble;
                case "const_str":
                case "const_string":
                    return OperandType.ConstString;
                case "mname":
                case "multiname":
                    return OperandType.ConstMultiname;
                case "ns": return OperandType.ConstNamespace;
                case "method": return OperandType.MethodIndex;
                case "class": return OperandType.ClassIndex;
                case "offset": return OperandType.BranchTarget;
                case "offsets": return OperandType.BranchTargets;
                case "exc":
                case "exception":
                    return OperandType.ExceptionIndex;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        static Operand CreateOperand(XmlElement opElem)
        {
            var op = new Operand
                         {
                             Name = opElem.GetAttribute("name"),
                             Description = opElem.GetAttribute("desc")
                         };
            string type = opElem.GetAttribute("type");
            if (string.IsNullOrEmpty(type))
            {
                if (op.Name == "arg_count")
                {
                    op.Type = OperandType.U30;
                    if (string.IsNullOrEmpty(op.Description))
                        op.Description = "the number of arguments present on the stack";
                }
                else
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                op.Type = ParseOperandType(type);
            }
            return op;
        }

        static void Load()
        {
            _list = new List<Instruction>();

            var rs = ResourceHelper.GetStream(typeof(Instructions), "opcodes.txt");
            using (var reader = new StreamReader(rs))
            {
                bool deprecated = false;
                while (true)
                {
                    string line = reader.ReadLine();
                    if (line == null) break;
                    line = line.Trim();
                    if (string.IsNullOrEmpty(line)) continue;
                    if (line.StartsWith("//"))
                    {
                        deprecated = false;
                        if (line.Substring(2).Trim().StartsWith("deprecated"))
                        {
                            deprecated = true;
                        }
                        continue;
                    }

                    var i = ParseInstruction(line);
                    i.IsDeprecated = deprecated;
                    _list.Add(i);

                    if (deprecated)
                        deprecated = false;
                }
            }

            rs = ResourceHelper.GetStream(typeof(Instructions), "il.xml");
            var doc = new XmlDocument();
            doc.Load(rs);

            foreach (XmlElement catElem in doc.DocumentElement.GetElementsByTagName("cat"))
            {
                string catName = catElem.GetAttribute("name");
                foreach (XmlElement insElem in catElem.GetElementsByTagName("i"))
                {
                    string name = insElem.GetAttribute("name");
                    var instruction = Find(name);
                    if (instruction != null)
                    {
                        instruction.Category = catName;
                        instruction.Description = insElem.GetAttribute("desc");
                        var ops = insElem.GetElementsByTagName("op");
                        if (ops.Count > 0)
                        {
                            instruction.Operands = new Operand[ops.Count];
                            for (int i = 0; i < ops.Count; ++i)
                            {
                                var opElem = (XmlElement)ops[i];
                                instruction.Operands[i] = CreateOperand(opElem);
                            }
                        }
                    }
                }
            }
        }

        public static Instruction GetInstruction(InstructionCode code)
        {
			if (_list == null)
			{
				Load();
				Debug.Assert(_list != null);
			}
        	return _list[(int)code].Clone();
        }

        public static Instruction[] GetInstructions()
        {
			if (_list == null)
			{
				Load();
				Debug.Assert(_list != null);
			}

        	//clone instructions to avoid modifications
            int n = _list.Count;
            var res = new Instruction[n];
            for (int i = 0; i < n; ++i)
                res[i] = _list[i].Clone();

            return res;
        }

        public static Instruction[] GetUsedInstructions()
        {
            var all = GetInstructions();
        	return all.Where(i => i.IsUsed).ToArray();
        }
    }
}