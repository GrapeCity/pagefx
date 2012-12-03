using System;
using System.Runtime.InteropServices;

namespace DataDynamics.PageFX.Ecma335.PDB.Interop
{
    [ComImport]
    [Guid(Guids.IID_IMetaDataDispenserEx)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [CoClass(typeof(CorMetaDataDispenser))]
    public interface IMetaDataDispenserEx
    {
        //[MethodImpl(MethodImplOptions.Unmanaged)]
        [PreserveSig]
        UInt32 DefineScope(
            [In, MarshalAs(UnmanagedType.Struct)] ref Guid rclsid,
            [In, MarshalAs(UnmanagedType.U4)]  uint dwCreateFlags,
            [In, MarshalAs(UnmanagedType.Struct)] ref Guid riid,
            out IntPtr ppIUnk
            );

        //[MethodImpl(MethodImplOptions.Unmanaged)]
        [PreserveSig]
        UInt32 OpenScope(
            [In, MarshalAs(UnmanagedType.LPWStr)]  string file,
            [In, MarshalAs(UnmanagedType.U4)]  uint dwOpenFlags,
            [In, MarshalAs(UnmanagedType.Struct)] ref Guid riid,
            out IntPtr ppIUnk
        );

        //[MethodImpl(MethodImplOptions.Unmanaged)]
        [PreserveSig]
        UInt32 OpenScopeOnMemory(
            [In]  IntPtr pData,
            [In, MarshalAs(UnmanagedType.U4)]  UInt32 cbData,
            [In, MarshalAs(UnmanagedType.U4)]  uint dwOpenFlags,
            [In, MarshalAs(UnmanagedType.Struct)] ref  Guid riid,
            out IntPtr ppIUnk
        );

        //[MethodImpl(MethodImplOptions.Unmanaged)]
        [PreserveSig]
        UInt32 GetOption(
            [In, MarshalAs(UnmanagedType.Struct)] ref  Guid optionId,
            [MarshalAs(UnmanagedType.Struct)] out ValueType pvalue //VARIANT
        );

        //[MethodImpl(MethodImplOptions.Unmanaged)]
        [PreserveSig]
        UInt32 SetOption(
            [In, MarshalAs(UnmanagedType.Struct)] ref  Guid optionId,
            [In, MarshalAs(UnmanagedType.Struct)] ValueType pvalue
        );
    }

    // Coclass for IMetaDataDispenserEx interface.  
    [ComImport]
    [Guid(Guids.CLSID_CorMetaDataDispenser)]
    public class CorMetaDataDispenser
    {
        //constructor of this class is runtime controlled, not CIL controlled 
    }
}
