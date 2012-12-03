using System;
using System.IO;
using Microsoft.Win32;

namespace DataDynamics
{
    public enum FrameworkVersion
    {
        NET_1_0,
		NET_1_1,
		NET_BETA2,
		NET_2_0,
		NET_3_0,
		NET_3_5,
    }

    public static class FrameworkInfo
    {
        public static string InstallRoot
        {
            get
            {
                const string regPath = @"Software\Microsoft\.NetFramework";
                var key = Registry.LocalMachine.OpenSubKey(regPath, false);
                return key != null ? key.GetValue("InstallRoot").ToString() : "";
            }
        }

        private const string root_1_0 = "v1.0.3705";
        private const string root_1_1 = "v1.1.4322";
        private const string root_beta2 = "v2.0.50215";
        private const string root_2_0 = "v2.0.50727";
        private const string root_3_0 = "v3.0";
        private const string root_3_5 = "v3.5";

        public static string Root_1_0
        {
            get { return Path.Combine(InstallRoot, root_1_0); }
        }

        public static string Root_1_1
        {
            get { return Path.Combine(InstallRoot, root_1_1); }
        }

        public static string Root_Beta2
        {
            get { return Path.Combine(InstallRoot, root_beta2); }
        }

        public static string Root_2_0
        {
            get { return Path.Combine(InstallRoot, root_2_0); }
        }

        public static string Root_3_0
        {
            get { return Path.Combine(InstallRoot, root_3_0); }
        }

        public static string Root_3_5
        {
            get { return Path.Combine(InstallRoot, root_3_5); }
        }

        public static string GetRoot(FrameworkVersion v)
        {
            switch (v)
            {
                case FrameworkVersion.NET_1_0:
                    return Root_1_0;
                case FrameworkVersion.NET_1_1:
                    return Root_1_1;
                case FrameworkVersion.NET_BETA2:
                    return Root_Beta2;
                case FrameworkVersion.NET_2_0:
                    return Root_2_0;
                case FrameworkVersion.NET_3_0:
                    return Root_3_0;
                case FrameworkVersion.NET_3_5:
                    return Root_3_5;
                default:
                    throw new ArgumentOutOfRangeException("v");
            }
        }
    }
}