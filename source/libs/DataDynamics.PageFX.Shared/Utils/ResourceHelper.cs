using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DataDynamics
{
    public static class ResourceHelper
    {
        public static Stream GetStream(Assembly asm, string subname)
        {
        	return (from name in asm.GetManifestResourceNames()
					where name.Contains(subname)
					select asm.GetManifestResourceStream(name)).FirstOrDefault();
        }

    	public static Stream GetStream(Type type, string subname)
        {
            return GetStream(type.Assembly, subname);
        }

        public static string GetText(Assembly asm, string subname)
        {
            var rs = GetStream(asm, subname);
            return Stream2.ReadAllText(rs);
        }

        public static string GetText(Type type, string subname)
        {
            return GetText(type.Assembly, subname);
        }

        public static Image GetImage(Assembly asm, string subname)
        {
            var rs = GetStream(asm, subname);
            if (rs == null) return null;
            try
            {
                return Image.FromStream(rs);
            }
            catch
            {
                return null;
            }
        }

        public static Image GetImage(Type type, string subname)
        {
            return GetImage(type.Assembly, subname);
        }
    }
}