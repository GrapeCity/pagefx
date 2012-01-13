using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using EnvDTE;

namespace DataDynamics.PageFX
{
    static class Utils
    {
        public static T GetAttribute<T>(ICustomAttributeProvider provider)
            where T : Attribute
        {
            object[] attributes = provider.GetCustomAttributes(typeof(T), true);
            if (attributes.Length == 0) return null;
            return attributes[0] as T;
        }

        public static Stream GetResourceStream(Assembly asm, string resSubName)
        {
            foreach (var rn in asm.GetManifestResourceNames())
            {
                if (rn.Contains(resSubName))
                    return asm.GetManifestResourceStream(rn);
            }
            return null;
        }

        public static void ErrorBox(string message)
        {
            MessageBox.Show(message, "PageFX - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ErrorBox(string format, params object[] args)
        {
            ErrorBox(string.Format(format, args));
        }

        public static string SelectPath(params string[] pathes)
        {
            foreach (var path in pathes)
            {
                if (File.Exists(path))
                    return path;
            }
            return null;
        }

        //static string StrOutputGroups(Project project)
        //{
        //    string s = "";
        //    var config = project.ConfigurationManager.ActiveConfiguration;
        //    var groups = config.OutputGroups;
        //    for (int i = 1; i < groups.Count; ++i)
        //    {
        //        var g = groups.Item(i);
        //        s += g.CanonicalName;
        //        s += "\r\n";
        //    }
        //    return s;
        //}

        //static string SafeGetValue(Property p)
        //{
        //    try
        //    {
        //        var v = p.Value;
        //        if (v != null)
        //            return v.ToString();
        //    }
        //    catch (Exception exc)
        //    {
        //    }
        //    return "";
        //}

        //static string StrProperties(Properties properties)
        //{
        //    if (properties == null) return "";
        //    string s = "";
        //    for (int i = 0; i < properties.Count; ++i)
        //    {
        //        var p = properties.Item(i + 1);
        //        s += p.Name;
        //        s += ": ";
        //        s += SafeGetValue(p);
        //        s += "\r\n";
        //    }
        //    return s;
        //}

        public static string GetProjectOutputPath(Project project)
        {
            //string s = StrOutputGroups(project);
            var config = project.ConfigurationManager.ActiveConfiguration;

            //string configProps = StrProperties(config.Properties);
            //string projProps = StrProperties(project.Properties);

            var outputPath = (string)config.Properties.Item("OutputPath").Value;
            if (!Path.IsPathRooted(outputPath))
            {
                string fullName = project.FullName;
                string dir = Path.GetDirectoryName(fullName);
                outputPath = Path.Combine(dir, outputPath);
            }

            string asmName = (string)project.Properties.Item("AssemblyName").Value;
            return Path.Combine(outputPath, asmName + ".dll");
        }

        public static string QName(string prefix, string name)
        {
            if (string.IsNullOrEmpty(prefix))
                return name;
            if (string.IsNullOrEmpty(name))
                return prefix;
            return prefix + "." + name;
        }
    }
}