using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace DataDynamics.PageFX.CLI.GAC
{
    public class AssemblyCache
    {
        #region Fusion.dll Functions
        /// <summary>
        /// The key entry point for reading the assembly cache.
        /// </summary>
        /// <param name="ppAsmCache">Pointer to return IAssemblyCache</param>
        /// <param name="dwReserved">must be 0</param>
        [DllImport("fusion.dll", SetLastError = true, PreserveSig = false)]
        private static extern void CreateAssemblyCache(out IAssemblyCache ppAsmCache, uint dwReserved);

        /// <summary>
        /// To obtain an instance of the CreateAssemblyEnum API, call the CreateAssemblyNameObject API.
        /// </summary>
        /// <param name="pEnum">Pointer to a memory location that contains the IAssemblyEnum pointer.</param>
        /// <param name="pUnkReserved">Must be null.</param>
        /// <param name="pName">An assembly name that is used to filter the enumeration. Can be null to enumerate all assemblies in the GAC.</param>
        /// <param name="dwFlags">Exactly one bit from the ASM_CACHE_FLAGS enumeration.</param>
        /// <param name="pvReserved">Must be NULL.</param>
        [DllImport("fusion.dll", SetLastError = true, PreserveSig = false)]
        private static extern void CreateAssemblyEnum(out IAssemblyEnum pEnum, IntPtr pUnkReserved, IAssemblyName pName,
                                                      ASM_CACHE_FLAGS dwFlags, IntPtr pvReserved);

        /// <summary>
        /// An instance of IAssemblyName is obtained by calling the CreateAssemblyNameObject API.
        /// </summary>
        /// <param name="ppAssemblyNameObj">Pointer to a memory location that receives the IAssemblyName pointer that is created.</param>
        /// <param name="szAssemblyName">A string representation of the assembly name or of a full assembly reference that is 
        /// determined by dwFlags. The string representation can be null.</param>
        /// <param name="dwFlags">Zero or more of the bits that are defined in the CREATE_ASM_NAME_OBJ_FLAGS enumeration.</param>
        /// <param name="pvReserved"> Must be null.</param>
        [DllImport("fusion.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        private static extern void CreateAssemblyNameObject(out IAssemblyName ppAssemblyNameObj, string szAssemblyName,
                                                            uint dwFlags, IntPtr pvReserved);

        /// <summary>
        /// To obtain an instance of the CreateInstallReferenceEnum API, call the CreateInstallReferenceEnum API.
        /// </summary>
        /// <param name="ppRefEnum">A pointer to a memory location that receives the IInstallReferenceEnum pointer.</param>
        /// <param name="pName">The assembly name for which the references are enumerated.</param>
        /// <param name="dwFlags"> Must be zero.</param>
        /// <param name="pvReserved">Must be null.</param>
        [DllImport("fusion.dll", SetLastError = true, PreserveSig = false)]
        private static extern void CreateInstallReferenceEnum(out IInstallReferenceEnum ppRefEnum, IAssemblyName pName,
                                                              uint dwFlags, IntPtr pvReserved);

        /// <summary>
        /// The GetCachePath API returns the storage location of the GAC. 
        /// </summary>
        /// <param name="dwCacheFlags">Exactly one of the bits defined in the ASM_CACHE_FLAGS enumeration.</param>
        /// <param name="pwzCachePath">Pointer to a buffer that is to receive the path of the GAC as a Unicode string.</param>
        /// <param name="pcchPath">Length of the pwszCachePath buffer, in Unicode characters.</param>
        [DllImport("fusion.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        private static extern void GetCachePath(ASM_CACHE_FLAGS dwCacheFlags,
                                                [MarshalAs(UnmanagedType.LPWStr)] StringBuilder pwzCachePath,
                                                ref uint pcchPath);
        #endregion

        public static class HResults
        {
            public const int S_OK = 0;
            public const uint E_INSUFFICIENT_BUFFER = 0x8007007a;
        }

        /// <summary>
        /// Use this method as a start for the GAC API
        /// </summary>
        /// <returns>IAssemblyCache COM interface</returns>
        public static IAssemblyCache CreateAssemblyCache()
        {
            IAssemblyCache ac;
            CreateAssemblyCache(out ac, 0);
            return ac;
        }

        public static IAssemblyName CreateAssemblyName(string name)
        {
            IAssemblyName an;
            CreateAssemblyNameObject(out an, name, 2, IntPtr.Zero);
            return an;
        }

        public static IAssemblyEnum CreateGACEnum()
        {
            IAssemblyEnum ae;
            CreateAssemblyEnum(out ae, IntPtr.Zero, null, ASM_CACHE_FLAGS.ASM_CACHE_GAC, IntPtr.Zero);
            return ae;
        }

        public static IAssemblyEnum CreateGACEnum(IAssemblyName name)
        {
            IAssemblyEnum ae;
            CreateAssemblyEnum(out ae, IntPtr.Zero, name, ASM_CACHE_FLAGS.ASM_CACHE_GAC, IntPtr.Zero);
            return ae;
        }

        public static string DownloadPath
        {
            get
            {
                uint bufferSize = 0xff;
                var buffer = new StringBuilder((int)bufferSize);
                GetCachePath(ASM_CACHE_FLAGS.ASM_CACHE_DOWNLOAD, buffer, ref bufferSize);
                return buffer.ToString();
            }
        }

        public static string GacPath
        {
            get
            {
                uint bufferSize = 0xff;
                var buffer = new StringBuilder((int)bufferSize);
                GetCachePath(ASM_CACHE_FLAGS.ASM_CACHE_GAC, buffer, ref bufferSize);
                return buffer.ToString();
            }
        }

        public static CultureInfo GetCulture(IAssemblyName name)
        {
            uint bufferSize = 0xff;
            var buffer = Marshal.AllocHGlobal((int)bufferSize);
            name.GetProperty(ASM_NAME.CULTURE, buffer, ref bufferSize);
            string result = Marshal.PtrToStringAuto(buffer);
            Marshal.FreeHGlobal(buffer);
            return new CultureInfo(result);
        }

        public static string GetDisplayName(IAssemblyName name, ASM_DISPLAY_FLAGS flags)
        {
            uint len = 0;
            int hr = name.GetDisplayName(null, ref len, flags);
            if ((uint)hr == E_INSUFFICIENT_BUFFER && len > 0)
            {
                var buf = new StringBuilder((int)len);
                hr = name.GetDisplayName(buf, ref len, flags);
                ComCheck((uint)hr);
                return buf.ToString();
            }
            else
            {
                return "";
            }
        }

        public static string GetDisplayName(IAssemblyName name)
        {
            return GetDisplayName(name, ASM_DISPLAY_FLAGS.ALL);
        }

        public static string GetName(IAssemblyName name)
        {
            uint bufferSize = 0xff;
            var buffer = new StringBuilder((int)bufferSize);
            name.GetName(ref bufferSize, buffer);
            return buffer.ToString();
        }

        /// <summary>
        /// Get the next assembly name in the current enumerator or fail
        /// </summary>
        /// <param name="enumerator"></param>
        /// <param name="name"></param>
        /// <returns>0 if the enumeration is not at its end</returns>
        public static int GetNextAssembly(IAssemblyEnum enumerator, out IAssemblyName name)
        {
            return enumerator.GetNextAssembly(IntPtr.Zero, out name, 0);
        }

        private const uint E_INSUFFICIENT_BUFFER = 0x8007007a;

        public static void ComCheck(uint errorCode)
        {
            if ((errorCode & 0x80000000) != 0)
                Marshal.ThrowExceptionForHR((int)errorCode);
        }

        public static byte[] GetPropertyData(IAssemblyName name, ASM_NAME id)
        {
            uint len = 0;
            int hr = name.GetProperty(id, IntPtr.Zero, ref len);
            if ((uint)hr == E_INSUFFICIENT_BUFFER && len > 0)
            {
                var buf = Marshal.AllocCoTaskMem((int)len);
                try
                {
                    hr = name.GetProperty(id, buf, ref len);
                    ComCheck((uint)hr);
                    var data = new byte[len];
                    Marshal.Copy(buf, data, 0, (int)len);
                    return data;
                }
                finally
                {
                    Marshal.FreeCoTaskMem(buf);
                }
            }
            else
            {
                return new byte[0];
            }
        }

        public static string GetPropertyString(IAssemblyName name, ASM_NAME id)
        {
            uint len = 0;
            int hr = name.GetProperty(id, IntPtr.Zero, ref len);
            if ((uint)hr == E_INSUFFICIENT_BUFFER && len > 0)
            {
                var buf = Marshal.AllocCoTaskMem((int)len);
                try
                {
                    hr = name.GetProperty(id, buf, ref len);
                    ComCheck((uint)hr);
                    string str = Marshal.PtrToStringAuto(buf);
                    return str;
                }
                finally
                {
                    Marshal.FreeCoTaskMem(buf);
                }
            }
            else
            {
                return "";
            }
        }

        public static byte[] GetPublicKey(IAssemblyName name)
        {
            return GetPropertyData(name, ASM_NAME.PUBLIC_KEY);
        }

        public static byte[] GetPublicKeyToken(IAssemblyName name)
        {
            return GetPropertyData(name, ASM_NAME.PUBLIC_KEY_TOKEN);
        }

        public static Version GetVersion(IAssemblyName name)
        {
            //uint major;
            //uint minor;
            //name.GetVersion(out major, out minor);
            //return new Version((int)(major >> 0x10), ((int)major) & 0xffff, (int)(minor >> 0x10), ((int)minor) & 0xffff);
            var data = GetPropertyData(name, ASM_NAME.MAJOR_VERSION);
            int major = data[1] << 8 | (data[0]);
            data = GetPropertyData(name, ASM_NAME.MINOR_VERSION);
            int minor = data[1] << 8 | (data[0]);
            data = GetPropertyData(name, ASM_NAME.REVISION_NUMBER);
            int revision = data[1] << 8 | (data[0]);
            data = GetPropertyData(name, ASM_NAME.BUILD_NUMBER);
            int build = data[1] << 8 | (data[0]);
            return new Version(major, minor, build, revision);
        }

        public static string GetZapPath()
        {
            uint bufferSize = 0xff;
            var buffer = new StringBuilder((int)bufferSize);
            GetCachePath(ASM_CACHE_FLAGS.ASM_CACHE_ZAP, buffer, ref bufferSize);
            return buffer.ToString();
        }

        /// <summary>
        /// GUID value for element guidScheme in the struct FUSION_INSTALL_REFERENCE
        /// 
        /// </summary>
        public static Guid FUSION_REFCOUNT_FILEPATH_GUID
        {
            get { return new Guid("b02f9d65-fb77-4f7a-afa5-b391309f11c9"); }
        }

        /// <summary>
        /// GUID value for element guidScheme in the struct FUSION_INSTALL_REFERENCE
        /// 
        /// </summary>
        public static Guid FUSION_REFCOUNT_MSI_GUID
        {
            get { return new Guid("25df0fc1-7f97-4070-add7-4b13bbfd7cb8"); }
        }

        /// <summary>
        /// GUID value for element guidScheme in the struct FUSION_INSTALL_REFERENCE
        /// 
        /// </summary>
        public static Guid FUSION_REFCOUNT_OPAQUE_STRING_GUID
        {
            get { return new Guid("2ec93463-b0c3-45e1-8364-327e96aea856"); }
        }

        /// <summary>
        /// GUID value for element guidScheme in the struct FUSION_INSTALL_REFERENCE
        /// The assembly is referenced by an application that has been installed by using Windows Installer. 
        /// The szIdentifier field is set to MSI, and szNonCannonicalData is set to Windows Installer. 
        /// This scheme must only be used by Windows Installer itself.
        /// </summary>
        public static Guid FUSION_REFCOUNT_UNINSTALL_SUBKEY_GUID
        {
            get { return new Guid("8cedc215-ac4b-488b-93c0-a50a49cb2fb8"); }
        }
    }
}