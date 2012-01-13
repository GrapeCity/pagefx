using System;
using Microsoft.VisualStudio.Debugger.Interop;
using System.Diagnostics;

namespace DataDynamics.PageFX.VisualStudio.Debugger
{
    // this class represents a module loaded in the debuggee process to the debugger. 
    class Module : IDebugModule3
    {
        public string Name;
        public string URL;
        public int Size;
        public ulong BaseAddress;
        public uint LoadOrder;
        public bool SymbolsLoaded;
        public string SymbolPath;

        #region IDebugModule2 Members

        // Gets the MODULE_INFO that describes this module.
        // This is how the debugger obtains most of the information about the module.
        int IDebugModule2.GetInfo(uint dwFields, MODULE_INFO[] infoArray)
        {
            try
            {
                var info = new MODULE_INFO();

                if ((dwFields & (uint)enum_MODULE_INFO_FIELDS.MIF_NAME) != 0)
                {
                    info.m_bstrName = System.IO.Path.GetFileName(Name);
                    info.dwValidFields |= (uint)enum_MODULE_INFO_FIELDS.MIF_NAME;
                }
                if ((dwFields & (uint)enum_MODULE_INFO_FIELDS.MIF_URL) != 0)
                {
                    info.m_bstrUrl = URL;
                    info.dwValidFields |= (uint)enum_MODULE_INFO_FIELDS.MIF_URL;
                }
                if ((dwFields & (uint)enum_MODULE_INFO_FIELDS.MIF_LOADADDRESS) != 0)
                {
                    info.m_addrLoadAddress = BaseAddress;
                    info.dwValidFields |= (uint)enum_MODULE_INFO_FIELDS.MIF_LOADADDRESS;
                }
                if ((dwFields & (uint)enum_MODULE_INFO_FIELDS.MIF_PREFFEREDADDRESS) != 0)
                {
                    // A debugger that actually supports showing the preferred base should crack the PE header and get 
                    // that field. This debugger does not do that, so assume the module loaded where it was suppose to.                   
                    info.m_addrPreferredLoadAddress = BaseAddress;
                    info.dwValidFields |= (uint)enum_MODULE_INFO_FIELDS.MIF_PREFFEREDADDRESS; ;
                }
                if ((dwFields & (uint)enum_MODULE_INFO_FIELDS.MIF_SIZE) != 0)
                {
                    info.m_dwSize = (uint)Size;
                    info.dwValidFields |= (uint)enum_MODULE_INFO_FIELDS.MIF_SIZE;
                }
                if ((dwFields & (uint)enum_MODULE_INFO_FIELDS.MIF_LOADORDER) != 0)
                {
                    info.m_dwLoadOrder = LoadOrder;
                    info.dwValidFields |= (uint)enum_MODULE_INFO_FIELDS.MIF_LOADORDER;
                }
                if ((dwFields & (uint)enum_MODULE_INFO_FIELDS.MIF_URLSYMBOLLOCATION) != 0)
                {
                    if (this.SymbolsLoaded)
                    {
                        info.m_bstrUrlSymbolLocation = SymbolPath;
                        info.dwValidFields |= (uint)enum_MODULE_INFO_FIELDS.MIF_URLSYMBOLLOCATION;
                    }
                }
                if ((dwFields & (uint)enum_MODULE_INFO_FIELDS.MIF_FLAGS) != 0)
                {
                    info.m_dwModuleFlags = 0;
                    if (SymbolsLoaded)
                    {
                        info.m_dwModuleFlags |= (uint)(enum_MODULE_FLAGS.MODULE_FLAG_SYMBOLS);
                    }
                    info.dwValidFields |= (uint)enum_MODULE_INFO_FIELDS.MIF_FLAGS;
                }
                
                
                infoArray[0] = info;

                return Const.S_OK;
            }
            //catch (ComponentException e)
            //{
            //    return e.HResult;
            //}
            catch (Exception e)
            {
                return Util.UnexpectedException(e);
            }
        }

        #endregion

        #region Deprecated interface methods
        // These methods are not called by the Visual Studio debugger, so they don't need to be implemented

        int IDebugModule2.ReloadSymbols_Deprecated(string urlToSymbols, out string debugMessage)
        {
            debugMessage = null;
            Debug.Fail("This function is not called by the debugger.");
            return Const.E_NOTIMPL;
        }

        int IDebugModule3.ReloadSymbols_Deprecated(string pszUrlToSymbols, out string pbstrDebugMessage)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IDebugModule3 Members

        // IDebugModule3 represents a module that supports alternate locations of symbols and JustMyCode states.
        // The sample does not support alternate symbol locations or JustMyCode, but it does not display symbol load information 

        // Gets the MODULE_INFO that describes this module.
        // This is how the debugger obtains most of the information about the module.
        int IDebugModule3.GetInfo(uint dwFields, MODULE_INFO[] pinfo)
        {
            return ((IDebugModule2)this).GetInfo(dwFields, pinfo);
        }

        // Returns a list of paths searched for symbols and the results of searching each path.
        int IDebugModule3.GetSymbolInfo(uint dwFields, MODULE_SYMBOL_SEARCH_INFO[] pinfo)
        {
            // This engine only supports loading symbols at the location specified in the binary's symbol path location in the PE file and
            // does so only for the primary exe of the debuggee.
            // Therefore, it only displays if the symbols were loaded or not. If symbols were loaded, that path is added.
            pinfo[0] = new MODULE_SYMBOL_SEARCH_INFO {dwValidFields = 1};

            if (SymbolsLoaded)
            {
                string symbolPath = "Symbols Loaded - " + SymbolPath;
                pinfo[0].bstrVerboseSearchInfo = symbolPath;
            }
            else
            {
                const string symbolsNotLoaded = "Symbols not loaded";
                pinfo[0].bstrVerboseSearchInfo = symbolsNotLoaded;
            }
            return Const.S_OK;
        }

        // Used to support the JustMyCode features of the debugger.
        // the sample debug engine does not support JustMyCode and therefore all modules
        // are considered "My Code"
        int IDebugModule3.IsUserCode(out int pfUser)
        {
            pfUser = 1;
            return Const.S_OK;           
        }

        // Loads and initializes symbols for the current module when the user explicitly asks for them to load.
        // The sample engine only supports loading symbols from the location pointed to by the PE file which will load
        // when the module is loaded.
        int IDebugModule3.LoadSymbols()
        {
            throw new NotImplementedException();
        }

        // Used to support the JustMyCode features of the debugger.
        // The sample engine does not support JustMyCode so this is not implemented
        int IDebugModule3.SetJustMyCodeState(int fIsUserCode)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}