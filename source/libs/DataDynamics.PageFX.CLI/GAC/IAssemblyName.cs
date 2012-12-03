using System;
using System.Runtime.InteropServices;
using System.Text;

namespace DataDynamics.PageFX.Ecma335.GAC
{
    /// <summary>
    /// The IAssemblyName interface represents an assembly name. An assembly name includes a predetermined set of name-value pairs. 
    /// The assembly name is described in detail in the .NET Framework SDK.
    /// </summary>
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("CD193BC0-B4BC-11d2-9833-00C04FC31D2E")]
    public interface IAssemblyName
    {
        /// <summary>
        /// The IAssemblyName::SetProperty method adds a name-value pair to the assembly name, or, if a name-value pair 
        /// with the same name already exists, modifies or deletes the value of a name-value pair.
        /// </summary>
        /// <param name="PropertyId">The ID that represents the name part of the name-value pair that is to be 
        /// added or to be modified. Valid property IDs are defined in the ASM_NAME enumeration.</param>
        /// <param name="pvProperty">A pointer to a buffer that contains the value of the property.</param>
        /// <param name="cbProperty">The length of the pvProperty buffer in bytes. If cbProperty is zero, the name-value pair 
        /// is removed from the assembly name.</param>
        /// <returns></returns>
        [PreserveSig]
        int SetProperty(ASM_NAME PropertyId, IntPtr pvProperty, uint cbProperty);

        /// <summary>
        /// The IAssemblyName::GetProperty method retrieves the value of a name-value pair in the assembly name that specifies the name.
        /// </summary>
        /// <param name="PropertyId">The ID that represents the name of the name-value pair whose value is to be retrieved.
        /// Specified property IDs are defined in the ASM_NAME enumeration.</param>
        /// <param name="pvProperty">A pointer to a buffer that is to contain the value of the property.</param>
        /// <param name="pcbProperty">The length of the pvProperty buffer, in bytes.</param>
        /// <returns></returns>
        [PreserveSig]
        int GetProperty(ASM_NAME PropertyId, IntPtr pvProperty, ref uint pcbProperty);

        /// <summary>
        /// The IAssemblyName::Finalize method freezes an assembly name. Additional calls to IAssemblyName::SetProperty are 
        /// unsuccessful after this method has been called.
        /// </summary>
        /// <returns></returns>
        [PreserveSig]
        int Finalize();

        [PreserveSig]
        int GetDisplayName([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder szDisplayName, ref uint pccDisplayName,
                           ASM_DISPLAY_FLAGS dwDisplayFlags);

        /// <summary>
        /// Undocumented
        /// </summary>
        /// <param name="refIID"></param>
        /// <param name="pUnkSink"></param>
        /// <param name="pUnkContext"></param>
        /// <param name="szCodeBase"></param>
        /// <param name="llFlags"></param>
        /// <param name="pvReserved"></param>
        /// <param name="cbReserved"></param>
        /// <param name="ppv"></param>
        /// <returns></returns>
        [PreserveSig]
        int BindToObject(ref Guid refIID, [MarshalAs(UnmanagedType.IUnknown)] object pUnkSink,
                         [MarshalAs(UnmanagedType.IUnknown)] object pUnkContext,
                         [MarshalAs(UnmanagedType.LPWStr)] string szCodeBase, long llFlags, IntPtr pvReserved,
                         uint cbReserved, out IntPtr ppv);

        /// <summary>
        /// The IAssemblyName::GetName method returns the name part of the assembly name.
        /// </summary>
        /// <param name="lpcwBuffer">Size of the pwszName buffer (on input). Length of the name (on return).</param>
        /// <param name="pwzName">Pointer to the buffer that is to contain the name part of the assembly name.</param>
        /// <returns></returns>
        /// <remarks>http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpguide/html/cpcondefaultmarshalingforstrings.asp</remarks>
        [PreserveSig]
        int GetName(ref uint lpcwBuffer, [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pwzName);

        /// <summary>
        /// The IAssemblyName::GetVersion method returns the version part of the assembly name.
        /// </summary>
        /// <param name="pdwVersionHi">Pointer to a DWORD that contains the upper 32 bits of the version number.</param>
        /// <param name="pdwVersionLow">Pointer to a DWORD that contain the lower 32 bits of the version number.</param>
        /// <returns></returns>
        [PreserveSig]
        int GetVersion(out uint pdwVersionHi, out uint pdwVersionLow);

        /// <summary>
        /// The IAssemblyName::IsEqual method compares the assembly name to another assembly names.
        /// </summary>
        /// <param name="pName">The assembly name to compare to.</param>
        /// <param name="dwCmpFlags">Indicates which part of the assembly name to use in the comparison. 
        /// Values are one or more of the bits defined in the ASM_CMP_FLAGS enumeration.</param>
        /// <returns></returns>
        [PreserveSig]
        int IsEqual(IAssemblyName pName, ASM_CMP_FLAGS dwCmpFlags);

        /// <summary>
        /// The IAssemblyName::Clone method creates a copy of an assembly name. 
        /// </summary>
        /// <param name="pName"></param>
        /// <returns></returns>
        [PreserveSig]
        int Clone(out IAssemblyName pName);
    }
}