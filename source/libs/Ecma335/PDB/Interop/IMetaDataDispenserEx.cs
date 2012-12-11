using System;
using System.Runtime.InteropServices;

namespace DataDynamics.PageFX.Ecma335.Pdb.Interop
{
	[ComImport]
    [Guid("31BCFCE2-DAFB-11D2-9F81-00C04F79A0A3")]
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
    [Guid("E5CB7A31-7512-11d2-89CE-0080C792E5D8")]
    public class CorMetaDataDispenser
    {
        //constructor of this class is runtime controlled, not CIL controlled 
    }
}
