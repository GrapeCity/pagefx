using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.FLI.ABC;
using DataDynamics.PageFX.FLI.SWF;

namespace DataDynamics.PageFX.FLI.IL
{
    /// <summary>
    /// List of <see cref="Instruction"/>s.
    /// </summary>
    public class ILStream : InstructionList<Instruction>, ISupportXmlDump
    {
        #region Public Members
        public void Add(InstructionCode code)
        {
            Add(new Instruction(code));
        }

        public void Add(InstructionCode code, params object[] values)
        {
            Add(new Instruction(code, values));
        }

        public void Add(IInstruction i)
        {
            base.Add((Instruction)i);
        }

        public void Add(IEnumerable<IInstruction> set)
        {
            foreach (var instruction in set)
            {
                Add(instruction);
            }
        }
        #endregion

        #region IO
        public void Read(AbcMethodBody body, SwfReader reader)
        {
            while (reader.Position < reader.Length)
            {
                var i = Instruction.Read(this, body, reader);
                i.Index = Count;
                Add(i);
            }
        }

        public bool TranslateIndicesEnabled
        {
            get { return _translateIndicesEnabled; }
            set { _translateIndicesEnabled = value; }
        }
        bool _translateIndicesEnabled;
        bool _indicesTranslated;

        public void SetupOffsets()
        {
            int n = Count;
            int offset = 0;
            for (int i = 0; i < n; ++i)
            {
                var instruction = this[i];
                instruction.Index = i;
                instruction.Offset = offset;
                offset += instruction.Size;
            }
        }

        public void TranslateIndices()
        {
            if (!_translateIndicesEnabled) return;
            if (_indicesTranslated) return;
            _indicesTranslated = true;

            int n = Count;
            var brs = new List<Instruction>();
            int offset = 0;
            for (int i = 0; i < n; ++i)
            {
                var instr = this[i];
                instr.Index = i;
                instr.Offset = offset;
                if (instr.IsBranch || instr.IsSwitch)
                    brs.Add(instr);
                offset += instr.Size;
            }

            n = brs.Count;
            for (int i = 0; i < n; ++i)
                brs[i].TranslateIndices(this);
        }

        public void Write(SwfWriter writer)
        {
            Verify();
            TranslateIndices();
            
            int n = Count;
            for (int i = 0; i < n; ++i)
            {
                var instruction = this[i];
                instruction.Index = i;
                instruction.Write(writer);
            }
        }
        #endregion

        #region Dump
        public void DumpXml(XmlWriter writer)
        {
            writer.WriteStartElement("code");

            foreach (var instr in this)
            {
                string s = instr.ToString();
                s = ReplaceInvalidChars(s);
                writer.WriteComment("  " + s + "  ");
            }

            writer.WriteEndElement();
        }

        static string ReplaceInvalidChars(string s)
        {
            var sb = new StringBuilder();
            foreach (var c in s)
            {
                if (IsInvalidXmlChar(c))
                    sb.AppendFormat("&x{0:X4};", (int)c);
                else
                    sb.Append(c);
            }
            return sb.ToString();
        }

        static bool IsInvalidXmlChar(int c)
        {
            switch (c)
            {
                case 0x1B:
                    return true;
            }
            return false;
        }

        public void Dump(TextWriter writer, string tab)
        {
            foreach (var i in this)
                writer.WriteLine("{0}{1}", tab, i);
        }
        #endregion

        internal void ResolveBranchTargets()
        {
            int n = Count;
            for (int i = 0; i < n; ++i)
            {
                var instr = this[i];
                instr.Index = i;
                instr.ResolveBranchTargets();
            }
        }

        internal void MarkTargets()
        {
            int n = Count;
            for (int i = 0; i < n; ++i)
            {
                var instr = this[i];
                instr.Index = i;
                foreach (var target in instr.GetTargets())
                {
					if (target < 0 || target >= n)
					{
						//Debugger.Break();
						throw new InvalidOperationException("Bad branch target for instruction.");
					}
                    this[target].IsBranchTarget = true;
                }
            }
        }

        public void Verify()
        {
            foreach (var instruction in this)
                instruction.Verify();
        }

        //* (done) combine findpropstrict+getproperty into getlex when find's only use is get in same block.

        public void Optimize()
        {
            for (int i = 0; i < Count; ++i)
            {
                CombineGetlex(i);
            }
        }

        bool CombineGetlex(int i)
        {
            var instr = this[i];
            if (instr.Code != InstructionCode.Getproperty)
                return false;

            if (i - 1 < 0) return false;
            var prev = this[i - 1];
            if (prev.Code != InstructionCode.Findpropstrict)
                return false;

            var qn1 = instr.Operand as AbcMultiname;
            var qn2 = prev.Operand as AbcMultiname;
            if (qn1 != null && qn2 != null && qn1 == qn2)
            {
                RemoveAt(i);
                --i;
                RemoveAt(i);
                Insert(i, new Instruction(InstructionCode.Getlex, qn1));
                return true;
            }

            return false;
        }
    }
}