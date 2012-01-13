using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace DataDynamics.PageFX.PDB.Interop
{
    static class ComHelper
    {
        [DllImport("ole32.dll")]
        [Obsolete("Use managed CreateCoClass method instead of pinvoke unmanaged method")]
        public static extern int CoCreateInstance([In] ref Guid rclsid,
                                                   [In, MarshalAs(UnmanagedType.IUnknown)] Object pUnkOuter,
                                                   [In] uint dwClsContext,
                                                   [In] ref Guid riid,
                                                   [Out, MarshalAs(UnmanagedType.Interface)] out Object ppv);


        public static object CreateCoClass(Guid clsid)
        {
            Type coClass = Type.GetTypeFromCLSID(clsid, true);
            return Activator.CreateInstance(coClass);
        }

        public static T GetInterfaceFromIUnknown<T>(IntPtr pIUnknown)
        {
            return (T)Marshal.GetTypedObjectForIUnknown(pIUnknown, typeof(T));
        }
    }
}
