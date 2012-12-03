using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace DataDynamics.PageFX.Ecma335.GAC
{
    /// <summary>
    /// Wraps <see cref="IAssemblyName"/>.
    /// </summary>
    public class GacAssemblyName : IDisposable
    {
        public GacAssemblyName(IAssemblyName name)
        {
            _name = name;
        }

        private IAssemblyName _name;

        /// <summary>
        /// Gets the short name of assembly.
        /// </summary>
        public string Name
        {
            get { return AssemblyCache.GetName(_name); }
        }

        /// <summary>
        /// Gets the display name of assembly.
        /// </summary>
        public string DisplayName
        {
            get { return AssemblyCache.GetDisplayName(_name); }
        }

        /// <summary>
        /// Gets version of assembly.
        /// </summary>
        public Version Version
        {
            get { return AssemblyCache.GetVersion(_name); }
        }

        #region IDisposable Members
        public void Dispose()
        {
            if (_name != null)
            {
                Marshal.ReleaseComObject(_name);
                _name = null;
            }
        }
        #endregion
    }

    /// <summary>
    /// Wraps <see cref="IAssemblyEnum"/>.
    /// </summary>
    public class GacEnum : IEnumerable<GacAssemblyName>, IDisposable
    {
        private IAssemblyEnum _enum;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="assemblyName">filter</param>
        public GacEnum(string assemblyName)
        {
            IAssemblyName fusionName = null;
            if (assemblyName != null)
            {
                fusionName = AssemblyCache.CreateAssemblyName(assemblyName);
            }
            _enum = AssemblyCache.CreateGACEnum(fusionName);
        }

        ///<summary>
        ///Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        ///</summary>
        public void Dispose()
        {
            if (_enum != null)
            {
                Marshal.ReleaseComObject(_enum);
                _enum = null;
            }
        }

        #region IEnumerable<AssemblyName> Members
        public IEnumerator<GacAssemblyName> GetEnumerator()
        {
            IAssemblyName name;
            int hr = _enum.GetNextAssembly(IntPtr.Zero, out name, 0);
            AssemblyCache.ComCheck((uint)hr);
            if (name != null)
            {
                yield return new GacAssemblyName(name);
            }
        }
        #endregion

        #region IEnumerable Members
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}