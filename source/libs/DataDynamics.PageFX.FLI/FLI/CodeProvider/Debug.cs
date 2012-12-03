﻿using DataDynamics.PageFX.Common;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FLI.IL;

namespace DataDynamics.PageFX.FLI
{
    internal partial class AvmCodeProvider
    {
        static string FormatDebugFile(string file)
        {
            if (GlobalSettings.EncodeDebugFile)
            {
                return DebugUtils.Encode(file);
            }
            return file;
        }

        public IInstruction[] DebuggerBreak()
        {
            var code = new AbcCode(_abc);
            code.DebuggerBreak();
            return code.ToArray();
        }

        public IInstruction[] DebugFile(string file)
        {
            _hasDebugFile = true;
            var code = new AbcCode(_abc);

            //Operand of debugfile instruction is string
            //Also we should look at format of this string. See how ASC formats this string.

            string s = FormatDebugFile(file);
            code.DebugFile(s);
            return code.ToArray();
        }

        public int DebugFirstLine { get; set; }

        public IInstruction[] DebugLine(int line)
        {
            var code = new AbcCode(_abc);
            code.DebugLine(line);
            return code.ToArray();
        }

        bool IsEmitDebugInfo
        {
            get { return GlobalSettings.EmitDebugInfo && _hasDebugFile; }
        }
        bool _hasDebugFile;

        void EmitLocalsDebugInfo(AbcCode code)
        {
            if (!IsEmitDebugInfo) return;

            const int regShift = -1;

            //if (HasActivationVar)
            //{
            //    code.DebugLocalInfo(0, ThisTraitName + "$0", 0);
            //}

            if (HasPseudoThis)
            {
                code.DebugLocalInfo(0, "pfx$pseudo_this", 0);
            }

            int n = _method.Parameters.Count;
            for (int i = 0; i < n; ++i)
            {
                IParameter p = _method.Parameters[i];
                int slot = GetArgIndex(p);
                code.DebugLocalInfo(slot + regShift, p.Name, 0);
            }

            if (HasActivationVar)
            {
                code.DebugLocalInfo(_activationVar + regShift, ThisTraitName + "$0", 0);
            }

            if (HasLocalVariables)
            {
                n = VarCount;
                for (int i = 0; i < n; ++i)
                {
                    var v = GetVar(i);
                    if (v.IsAddressed) continue;
                    int slot = GetVarIndex(i);
                    code.DebugLocalInfo(slot + regShift, v.Name, 0);
                }
            }
        }
    }
}
