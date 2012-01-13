using System;
using Microsoft.VisualStudio.Debugger.Interop;

namespace DataDynamics.PageFX.VisualStudio.Debugger
{
    // And implementation of IDebugCodeContext2 and IDebugMemoryContext2. 
    // IDebugMemoryContext2 represents a position in the address space of the machine running the program being debugged.
    // IDebugCodeContext2 represents the starting position of a code instruction. 
    // For most run-time architectures today, a code context can be thought of as an address in a program's execution stream.
    class CodeContext : IDebugCodeContext2
    {
        readonly Engine m_engine;
        readonly uint m_address;
        IDebugDocumentContext2 m_documentContext;
        
        public CodeContext(Engine engine, uint address)
        {
            m_engine = engine;
            m_address = address;
        }

        public void SetDocumentContext(IDebugDocumentContext2 docContext)
        {
            m_documentContext = docContext;
        }

        #region IDebugMemoryContext2 Members

        // Adds a specified value to the current context's address to create a new context.
        public int Add(ulong dwCount, out IDebugMemoryContext2 newAddress)
        {
            newAddress = new CodeContext(m_engine, (uint)dwCount + m_address);
            return Const.S_OK;
        }

        // Compares the memory context to each context in the given array in the manner indicated by compare flags, 
        // returning an index of the first context that matches.
        public int Compare(uint uContextCompare, IDebugMemoryContext2[] compareToItems, uint compareToLength, out uint foundIndex)
        {
            foundIndex = uint.MaxValue;

            try
            {
                var contextCompare = (enum_CONTEXT_COMPARE)uContextCompare;

                for (uint c = 0; c < compareToLength; c++)
                {
                    var compareTo = compareToItems[c] as CodeContext;
                    if (compareTo == null)
                    {
                        continue;
                    }

                    if (!ReferenceEquals(m_engine, compareTo.m_engine))
                    {
                        continue;
                    }

                    bool result;

                    switch (contextCompare)
                    {
                        case enum_CONTEXT_COMPARE.CONTEXT_EQUAL:
                            result = (m_address == compareTo.m_address);
                            break;

                        case enum_CONTEXT_COMPARE.CONTEXT_LESS_THAN:
                            result = (m_address < compareTo.m_address);
                            break;

                        case enum_CONTEXT_COMPARE.CONTEXT_GREATER_THAN:
                            result = (m_address > compareTo.m_address);
                            break;

                        case enum_CONTEXT_COMPARE.CONTEXT_LESS_THAN_OR_EQUAL:
                            result = (m_address <= compareTo.m_address);
                            break;

                        case enum_CONTEXT_COMPARE.CONTEXT_GREATER_THAN_OR_EQUAL:
                            result = (m_address >= compareTo.m_address);
                            break;

                        // The sample debug engine doesn't understand scopes or functions
                        case enum_CONTEXT_COMPARE.CONTEXT_SAME_SCOPE:
                        case enum_CONTEXT_COMPARE.CONTEXT_SAME_FUNCTION:
                            result = (m_address == compareTo.m_address);
                            break;

                        case enum_CONTEXT_COMPARE.CONTEXT_SAME_MODULE:
                            result = (m_address == compareTo.m_address);
                            if (result == false)
                            {
                                //TODO:
                                //DebuggedModule module = m_engine.DebuggedProcess.ResolveAddress(m_address);

                                //if (module != null)
                                //{
                                //    result = (compareTo.m_address >= module.BaseAddress) &&
                                //        (compareTo.m_address < module.BaseAddress + module.Size);
                                //}
                            }
                            break;

                        case enum_CONTEXT_COMPARE.CONTEXT_SAME_PROCESS:
                            result = true;
                            break;

                        default:
                            // A new comparison was invented that we don't support
                            return Const.E_NOTIMPL;
                    }

                    if (result)
                    {
                        foundIndex = c;
                        return Const.S_OK;
                    }
                }

                return Const.S_FALSE;
            }
            catch (Exception e)
            {
                return Util.UnexpectedException(e);
            }
        }

        // Gets information that describes this context.
        public int GetInfo(uint dwFields, CONTEXT_INFO[] pinfo)
        {           
            try
            {
                pinfo[0].dwFields = 0;

                if ((dwFields & (uint)enum_CONTEXT_INFO_FIELDS.CIF_ADDRESS) != 0)
                {
                    pinfo[0].bstrAddress = m_address.ToString();
                    pinfo[0].dwFields |= (uint)enum_CONTEXT_INFO_FIELDS.CIF_ADDRESS;
                }

                // Fields not supported by the sample
                if ((dwFields & (uint)enum_CONTEXT_INFO_FIELDS.CIF_ADDRESSOFFSET) != 0){}
                if ((dwFields & (uint)enum_CONTEXT_INFO_FIELDS.CIF_ADDRESSABSOLUTE) != 0){}
                if ((dwFields & (uint)enum_CONTEXT_INFO_FIELDS.CIF_MODULEURL) != 0){}
                if ((dwFields & (uint)enum_CONTEXT_INFO_FIELDS.CIF_FUNCTION) != 0) {}
                if ((dwFields & (uint)enum_CONTEXT_INFO_FIELDS.CIF_FUNCTIONOFFSET) != 0) {}

                return Const.S_OK;
            }
            catch (Exception e)
            {
                return Util.UnexpectedException(e);
            }
        }

        // Gets the user-displayable name for this context
        // This is not supported by the sample engine.
        public int GetName(out string pbstrName)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        // Subtracts a specified value from the current context's address to create a new context.
        public int Subtract(ulong dwCount, out IDebugMemoryContext2 ppMemCxt)
        {
            ppMemCxt = new CodeContext(m_engine, (uint)dwCount - m_address);
            return Const.S_OK;
        }

        #endregion

        #region IDebugCodeContext2 Members

        // Gets the document context for this code-context
        public int GetDocumentContext(out IDebugDocumentContext2 ppSrcCxt)
        {
            ppSrcCxt = m_documentContext;
            return Const.S_OK;
        }

        // Gets the language information for this code context.
        public int GetLanguageInfo(ref string pbstrLanguage, ref Guid pguidLanguage)
        {
            if (m_documentContext != null)
            {
                m_documentContext.GetLanguageInfo(ref pbstrLanguage, ref pguidLanguage);
                return Const.S_OK;
            }
            return Const.S_FALSE;
        }
        #endregion
    }
}
