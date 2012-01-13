using System;
using System.Runtime.InteropServices;
using ProgramDebugDatabase;

namespace DataDynamics.PageFX.PDB.Interop
{
    using HRESULT = UInt32;

    [Guid(Guids.IID_IMetaDataDispenser)]
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Obsolete("IMetaDataDispenser now is obsolete. Use IMetaDataDispenserEx")]
    public interface IMetaDataDispenser
    {
        //[MethodImpl(MethodImplOptions.Unmanaged)]
        [PreserveSig]
        UInt32 DefineScope(
            [In, MarshalAs(UnmanagedType.Struct)] ref Guid rclsid,
            [In, MarshalAs(UnmanagedType.U4), ComAliasName("DWORD")]  COR_OPEN_FLAGS dwCreateFlags,
            [In, MarshalAs(UnmanagedType.Struct)] ref Guid riid,
            [ComAliasName("IUnknown**")] out IntPtr ppIUnk
            );

        //[MethodImpl(MethodImplOptions.Unmanaged)]
        [PreserveSig]
        UInt32 OpenScope(
            [In, MarshalAs(UnmanagedType.LPWStr), ComAliasName("LPCWSTR")]  string file,
            [In, MarshalAs(UnmanagedType.U4), ComAliasName("DWORD")]  COR_OPEN_FLAGS dwOpenFlags,
            [In, MarshalAs(UnmanagedType.Struct)] ref Guid riid,
            [ComAliasName("IUnknown**")] out IntPtr ppIUnk
        );

        //[MethodImpl(MethodImplOptions.Unmanaged)]
        [PreserveSig]
        UInt32 OpenScopeOnMemory(
            [In, ComAliasName("LPCVOID")]  IntPtr pData,
            [In, MarshalAs(UnmanagedType.U4), ComAliasName("ULONG")]  UInt32 cbData,
            [In, MarshalAs(UnmanagedType.U4), ComAliasName("DWORD")]  COR_OPEN_FLAGS dwOpenFlags,
            [In, MarshalAs(UnmanagedType.Struct)] ref  Guid riid,
            [ComAliasName("IUnknown**")] out IntPtr ppIUnk
        );
    }
}
