using System.Runtime.InteropServices;

namespace DataDynamics.PageFX.CLI.GAC
{
    /// <summary>
    /// Undocumented. Probably only for internal use.
    /// <see cref="M:System.GAC.IAssemblyCache.CreateAssemblyCacheItem(System.UInt32,System.IntPtr,System.GAC.IAssemblyCacheItem@,System.String)" />
    /// </summary>
    [ComImport, Guid("9E3AAEB4-D1CD-11D2-BAB9-00C04F8ECEAE"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IAssemblyCacheItem
    {
        /// <summary>
        /// Undocumented.
        /// </summary>
        /// <param name="dwFlags"></param>
        /// <param name="pszStreamName"></param>
        /// <param name="dwFormat"></param>
        /// <param name="dwFormatFlags"></param>
        /// <param name="ppIStream"></param>
        /// <param name="puliMaxSize"></param>
        void CreateStream(uint dwFlags, [MarshalAs(UnmanagedType.LPWStr)] string pszStreamName, uint dwFormat, uint dwFormatFlags, out UCOMIStream ppIStream, ref long puliMaxSize);
        /// <summary>
        /// Undocumented.
        /// </summary>
        /// <param name="dwFlags"></param>
        /// <param name="pulDisposition"></param>
        void Commit(uint dwFlags, out long pulDisposition);
        /// <summary>
        /// Undocumented.
        /// </summary>
        void AbortItem();
    }
}

