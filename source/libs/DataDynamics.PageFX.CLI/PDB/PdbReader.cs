using System;
using System.Diagnostics.SymbolStore;

namespace DataDynamics.PageFX.PDB
{
    internal class PdbReader : IDisposable
    {
        public SymBinder Binder;
        public ISymbolReader SymReader;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Free other state (managed objects).
                SymReader = null;
                Binder = null;
            }
            // Free your own state (unmanaged objects).
        }

        ~PdbReader()
        {
            Dispose(false);
        }
    }
}