using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.Debugger.Interop;

namespace DataDynamics.PageFX.VisualStudio.Debugger
{
    // Represents a logical stack frame on the thread stack. 
    // Also implements the IDebugExpressionContext interface, which allows expression evaluation and watch windows.
    class StackFrame : IDebugStackFrame2, IDebugExpressionContext2
    {
        public string File;
        public string Function;
        public int Line;

        public StackFrame(Engine engine)
        {
            m_engine = engine;
        }

        public Engine Engine
        {
            get { return m_engine; }
        }
        readonly Engine m_engine;

        public int Index { get; set; }

        // An array of this frame's parameters
        public List<Property> Args
        {
            get { return m_args; }
        }
        readonly List<Property> m_args = new List<Property>();

        // An array of this frame's locals
        public List<Property> Locals
        {
            get { return m_locals; }
        }
        readonly List<Property> m_locals = new List<Property>();

        public IEnumerable<Property> Properties
        {
            get
            {
                foreach (var prop in m_args)
                    yield return prop;
                foreach (var prop in m_locals)
                    yield return prop;
            }
        }

        public DebugThread Thread
        {
            get
            {
                return m_engine == null ? null : m_engine.Thread;
            }
        }

        string GetModuleName()
        {
            var m = Thread.m_module;
            if (m != null)
                return m.Name;
            return m_engine.ModuleName;
        }

        #region Non-interface methods
        string FormatFunctionName(enum_FRAMEINFO_FLAGS flags)
        {
            string func = "";

            if ((flags & enum_FRAMEINFO_FLAGS.FIF_FUNCNAME_MODULE) != 0)
            {
                func = GetModuleName() + "!";
            }

            func += Function;

            if ((flags & enum_FRAMEINFO_FLAGS.FIF_FUNCNAME_ARGS) != 0)
            {
                func += "(";
                bool comma = false;
                int n = m_args.Count;
                for (int i = 0; i < n; i++)
                {
                    var p = m_args[i];
                    if (p.IsThis) continue;

                    if (comma) func += ", ";

                    if ((flags & enum_FRAMEINFO_FLAGS.FIF_FUNCNAME_ARGS_TYPES) != 0
                        && !string.IsNullOrEmpty(p.Type))
                    {
                        func += p.Type + " ";
                    }

                    if ((flags & enum_FRAMEINFO_FLAGS.FIF_FUNCNAME_ARGS_NAMES) != 0)
                    {
                        func += p.Name;
                    }

                    if ((flags & enum_FRAMEINFO_FLAGS.FIF_FUNCNAME_ARGS_VALUES) != 0
                        && !string.IsNullOrEmpty(p.Value))
                    {
                        func += "=";
                        if (p.IsObject) func += "{";
                        func += p.Value;
                        if (p.IsObject) func += "}";
                    }

                    comma = true;
                }
                func += ")";
            }

            if ((flags & enum_FRAMEINFO_FLAGS.FIF_FUNCNAME_LINES) != 0)
            {
                func += " Line:" + Line;
            }

            return func;
        }

        // Construct a FRAMEINFO for this stack frame with the requested information.
        public void SetFrameInfo(uint dwFieldSpec, out FRAMEINFO frameInfo)
        {
            frameInfo = new FRAMEINFO();

            //TODO:
            var module = Thread.m_module;

            var flags = (enum_FRAMEINFO_FLAGS)dwFieldSpec;

            // The debugger is asking for the formatted name of the function which is displayed in the callstack window.
            // There are several optional parts to this name including the module, argument types and values, and line numbers.
            // The optional information is requested by setting flags in the dwFieldSpec parameter.
            if ((flags & enum_FRAMEINFO_FLAGS.FIF_FUNCNAME) != 0)
            {
                // If there is source information, construct a string that contains the module name, function name, and optionally argument names and values.
                frameInfo.m_bstrFuncName = FormatFunctionName(flags);
                frameInfo.m_dwValidFields |= (uint)enum_FRAMEINFO_FLAGS.FIF_FUNCNAME;
            }

            // The debugger is requesting the name of the module for this stack frame.
            if ((dwFieldSpec & (uint)enum_FRAMEINFO_FLAGS.FIF_MODULE) != 0)
            {
                frameInfo.m_bstrModule = GetModuleName();
                frameInfo.m_dwValidFields |= (uint)enum_FRAMEINFO_FLAGS.FIF_MODULE;
            }

            // The debugger is requesting the range of memory addresses for this frame.
            // For the sample engine, this is the contents of the frame pointer.
            if ((dwFieldSpec & (uint)enum_FRAMEINFO_FLAGS.FIF_STACKRANGE) != 0)
            {
                frameInfo.m_addrMin = Thread.ebp;
                frameInfo.m_addrMax = Thread.ebp;
                frameInfo.m_dwValidFields |= (uint)enum_FRAMEINFO_FLAGS.FIF_STACKRANGE;
            }

            // The debugger is requesting the IDebugStackFrame2 value for this frame info.
            if ((dwFieldSpec & (uint)enum_FRAMEINFO_FLAGS.FIF_FRAME) != 0)
            {
                frameInfo.m_pFrame = this;
                frameInfo.m_dwValidFields |= (uint)enum_FRAMEINFO_FLAGS.FIF_FRAME;
            }

            // Does this stack frame of symbols loaded?
            if ((dwFieldSpec & (uint)enum_FRAMEINFO_FLAGS.FIF_DEBUGINFO) != 0)
            {
                frameInfo.m_fHasDebugInfo = 1;
                frameInfo.m_dwValidFields |= (uint)enum_FRAMEINFO_FLAGS.FIF_DEBUGINFO;
            }

            // Is this frame stale?
            if ((dwFieldSpec & (uint)enum_FRAMEINFO_FLAGS.FIF_STALECODE) != 0)
            {
                frameInfo.m_fStaleCode = 0;
                frameInfo.m_dwValidFields |= (uint)enum_FRAMEINFO_FLAGS.FIF_STALECODE;
            }

            // The debugger would like a pointer to the IDebugModule2 that contains this stack frame.
            if ((dwFieldSpec & (uint)enum_FRAMEINFO_FLAGS.FIF_DEBUG_MODULEP) != 0)
            {
                if (module != null)
                {
                    frameInfo.m_pModule = module;
                    frameInfo.m_dwValidFields |= (uint)enum_FRAMEINFO_FLAGS.FIF_DEBUG_MODULEP;
                }
            }
        }

        // Construct an instance of IEnumDebugPropertyInfo2 for the combined locals and parameters.
        void CreateProperties(out uint n, out IEnumDebugPropertyInfo2 enumObject,
            bool args, bool vars)
        {
            n = 0;

            int argc = 0;
            if (vars)
                n += (uint)m_locals.Count;

            if (args)
            {
                argc = m_args.Count;
                n += (uint)argc;
            }

            var propInfo = new DEBUG_PROPERTY_INFO[n];

            if (m_args != null)
            {
                for (int i = 0; i < argc; i++)
                {
                    var property = m_args[i];
                    propInfo[i] = property.CreateDebugPropertyInfo((uint)DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_STANDARD);
                }
            }

            if (vars)
            {
                for (int i = 0; i < m_locals.Count; i++)
                {
                    var property = m_locals[i];
                    propInfo[argc + i] = property.CreateDebugPropertyInfo((uint)DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_STANDARD);
                }
            }

            enumObject = new PropertyInfoEnum(propInfo);
        }
        #endregion

        #region IDebugStackFrame2 Members

        // Creates an enumerator for properties associated with the stack frame, such as local variables.
        // The sample engine only supports returning locals and parameters. Other possible values include
        // class fields (this pointer), registers, exceptions...
        int IDebugStackFrame2.EnumProperties(uint dwFields, uint nRadix, ref Guid guidFilter, uint dwTimeout, out uint elementsReturned, out IEnumDebugPropertyInfo2 enumObject)
        {
            int hr;

            elementsReturned = 0;
            enumObject = null;

            try
            {
                if (guidFilter == Guids.FilterLocalsPlusArgs ||
                        guidFilter == Guids.FilterAllLocalsPlusArgs ||
                        guidFilter == Guids.FilterAllLocals)
                {
                    CreateProperties(out elementsReturned, out enumObject, true, true);
                    hr = Const.S_OK;
                }
                else if (guidFilter == Guids.FilterLocals)
                {
                    CreateProperties(out elementsReturned, out enumObject, false, true);
                    hr = Const.S_OK;
                }
                else if (guidFilter == Guids.FilterArgs)
                {
                    CreateProperties(out elementsReturned, out enumObject, true, false);
                    hr = Const.S_OK;
                }
                else
                {
                    hr = Const.E_NOTIMPL;
                }
            }
            catch (Exception e)
            {
                return Util.UnexpectedException(e);
            }

            return hr;
        }

        // Gets the code context for this stack frame. The code context represents the current instruction pointer in this stack frame.
        int IDebugStackFrame2.GetCodeContext(out IDebugCodeContext2 memoryAddress)
        {
            memoryAddress = null;

            try
            {
                memoryAddress = new CodeContext(m_engine, Thread.eip);
                return Const.S_OK;
            }
            catch (Exception e)
            {
                return Util.UnexpectedException(e);
            }
        }

        Property m_root;

        // Gets a description of the properties of a stack frame.
        // Calling the IDebugProperty2::EnumChildren method with appropriate filters can retrieve the local variables, method parameters, registers, and "this" 
        // pointer associated with the stack frame. The debugger calls EnumProperties to obtain these values in the sample.
        int IDebugStackFrame2.GetDebugProperty(out IDebugProperty2 property)
        {
            if (m_root == null)
            {
                m_root = new Property("") { Frame = this };
                m_root.AddChildren(Properties);
            }
            property = m_root;
            return Const.S_OK;
        }

        // Gets the document context for this stack frame. The debugger will call this when the current stack frame is changed
        // and will use it to open the correct source document for this stack frame.
        int IDebugStackFrame2.GetDocumentContext(out IDebugDocumentContext2 docContext)
        {
            docContext = null;
            try
            {
                // Assume all lines begin and end at the beginning of the line.
                var begTp = new TEXT_POSITION { dwColumn = 0, dwLine = (uint)(Line - 1) };
                var endTp = new TEXT_POSITION { dwColumn = 0, dwLine = (uint)(Line - 1) };
                docContext = new DocumentContext(File, begTp, endTp, null);
                return Const.S_OK;
            }
            catch (Exception e)
            {
                return Util.UnexpectedException(e);
            }
        }

        // Gets an evaluation context for expression evaluation within the current context of a stack frame and thread.
        // Generally, an expression evaluation context can be thought of as a scope for performing expression evaluation. 
        // Call the IDebugExpressionContext2::ParseText method to parse an expression and then call the resulting IDebugExpression2::EvaluateSync 
        // or IDebugExpression2::EvaluateAsync methods to evaluate the parsed expression.
        int IDebugStackFrame2.GetExpressionContext(out IDebugExpressionContext2 ppExprCxt)
        {
            ppExprCxt = this;
            return Const.S_OK;
        }

        // Gets a description of the stack frame.
        int IDebugStackFrame2.GetInfo(uint dwFieldSpec, uint nRadix, FRAMEINFO[] pFrameInfo)
        {
            try
            {
                SetFrameInfo(dwFieldSpec, out pFrameInfo[0]);
                return Const.S_OK;
            }
            catch (Exception e)
            {
                return Util.UnexpectedException(e);
            }
        }

        // Gets the language associated with this stack frame. 
        // In this sample, all the supported stack frames are C++
        int IDebugStackFrame2.GetLanguageInfo(ref string pbstrLanguage, ref Guid pguidLanguage)
        {
            pbstrLanguage = Const.LangCharp;
            pguidLanguage = Guids.LanguageCSharp;
            return Const.S_OK;
        }

        // Gets the name of the stack frame.
        // The name of a stack frame is typically the name of the method being executed.
        int IDebugStackFrame2.GetName(out string name)
        {
            name = null;

            try
            {
                name = Util.GetAddressDescription(null, Thread.eip);

                return Const.S_OK;
            }
            catch (Exception e)
            {
                return Util.UnexpectedException(e);
            }
        }

        // Gets a machine-dependent representation of the range of physical addresses associated with a stack frame.
        int IDebugStackFrame2.GetPhysicalStackRange(out ulong addrMin, out ulong addrMax)
        {
            addrMin = Thread.ebp;
            addrMax = Thread.ebp;
            return Const.S_OK;
        }

        // Gets the thread associated with a stack frame.
        int IDebugStackFrame2.GetThread(out IDebugThread2 thread)
        {
            thread = Thread;
            return Const.S_OK;
        }

        #endregion

        #region IDebugExpressionContext2 Members

        // Retrieves the name of the evaluation context. 
        // The name is the description of this evaluation context. It is typically something that can be parsed by an expression evaluator 
        // that refers to this exact evaluation context. For example, in C++ the name is as follows: 
        // "{ function-name, source-file-name, module-file-name }"
        int IDebugExpressionContext2.GetName(out string pbstrName)
        {
            pbstrName = "{}";
            return Const.S_OK;
        }

        readonly Hashtable _propcache = new Hashtable();

        public void Register(Property prop)
        {
            if (prop.IsIgnored) return;
            string key = prop.FullName;
            if (_propcache.ContainsKey(key)) return;
            _propcache[key] = prop;
        }

        public void Unregister(Property prop)
        {
            string key = prop.FullName;
            if (_propcache.ContainsKey(key))
                _propcache.Remove(key);
        }

        string[] PropCache
        {
            get 
            {
                int n = _propcache.Count;
                var arr = new string[n];
                int i = 0;
                foreach (DictionaryEntry e in _propcache)
                {
                    //arr[i++] = e.Key + " = " + e.Value;
                    arr[i++] = e.Value.ToString();
                }
                return arr;
            }
        }

        // Parses a text-based expression for evaluation.
        public int ParseText(string code, uint dwFlags, uint nRadix,
            out IDebugExpression2 ppExpr, out string pbstrError, out uint pichError)
        {
            ppExpr = null;
            pbstrError = "";
            pichError = 0;

            try
            {
                var prop = EvalExpression(code);
                if (prop == null)
                {
                    pbstrError = "Invalid Expression";
                    pichError = (uint)pbstrError.Length;
                    return Const.S_FALSE;
                }
                ppExpr = new Expression(prop);
                return Const.S_OK;
            }
            catch (Exception e)
            {
                return Util.UnexpectedException(e);
            }
        }

        public Property EvalExpression(string exp)
        {
            var prop = _propcache[exp] as Property;
            if (prop != null)
            {
                prop.Eval();
                return prop;
            }
            prop = m_engine.EvalExpression(this, exp);
            if (prop == null) return null;
            prop.IsEvaluated = true;
            Register(prop);
            prop.Eval();
            return prop;
        }
        #endregion
    }
}

