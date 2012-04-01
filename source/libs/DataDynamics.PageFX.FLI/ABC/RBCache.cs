using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DataDynamics.Compression.Zip;

namespace DataDynamics.PageFX.FLI.ABC
{
	internal static class ResourceBundleExtensions
    {
        public static string GetResourceBundleName(this AbcMetaEntry e)
        {
            if (e.Items.Count <= 0)
                return null;
            var val = e.Items[0].Value;
            if (val == null) return null;
            string s = val.Value;
            return string.IsNullOrEmpty(s) ? null : s;
        }

        public static bool IsResourceBundleComment(this string line)
        {
            if (string.IsNullOrEmpty(line)) return false;
            return line[0] == '#' || line[0] == '!';
        }

        public static string[] GetResourceBundleLines(this Stream stream)
        {
            using (var reader = new StreamReader(stream))
                return reader.GetResourceBundleLines();
        }

        public static string[] GetResourceBundleLines(this TextReader reader)
        {
        	return reader.ReadLines(true, line => line.Length != 0 && !line.IsResourceBundleComment());
        }
    }

	#region class RBCache
    class RB
    {
        public string Name;
        public string Locale;
        public bool IsFlex;
        public bool IsZipped;
        public string SourcePath;
        public string ZipEntry;
        public string[] Content;
    }

    static class RBCache
    {
        static bool _loaded;
        static bool _isFlex;

        static void Load()
        {
            if (_loaded) return;
            _loaded = true;

            try
            {
                _isFlex = true;
                string dir = GlobalSettings.LocaleDirectory;
                if (Directory.Exists(dir))
                    LoadFromDir(dir);
            }
            finally
            {
                _isFlex = false;
            }
        }

        public static RB Get(string locale, string name)
        {
            Load();

            var l = Cache[locale] as Locale;
            if (l == null)
                //throw Errors.RBC.UnableToResolve.CreateException(name, locale);
                return null;

            var res = l[name];
            if (res == null)
                //throw Errors.RBC.UnableToResolve.CreateException(name, locale);
                return null;

            return res;
        }

        #region Locale
        class Locale
        {
            readonly Hashtable _rbcache = new Hashtable();

            public RB this[string name]
            {
                get { return _rbcache[name] as RB; }
            }

            public void Add(RB rb)
            {
                _rbcache[rb.Name] = rb;
            }
        }
        #endregion

        public static void CopyFlexLocales(IEnumerable<string> locales)
        {
            if (locales == null) return;
            foreach (var locale in locales)
                CopyFlexLocale(locale);
        }

        public static void CopyFlexLocale(string locale)
        {
            if (string.IsNullOrEmpty(locale)) return;
            if (Locale_en_US == null) return;
            if (Cache.Contains(locale)) return;
            Cache[locale] = Locale_en_US;
        }

        static readonly Hashtable Cache = new Hashtable();
        static Locale Locale_en_US;
        
        static void LoadFromDir(string dir)
        {
            foreach (var locdir in Directory.GetDirectories(dir))
            {
                string locale = Path.GetFileName(locdir);

                foreach (var file in Directory.GetFiles(locdir, "*.swc"))
                    LoadFromZipFile(file, locale);

                foreach (var file in Directory.GetFiles(locdir, "*.zip"))
                    LoadFromZipFile(file, locale);

                foreach (var file in Directory.GetFiles(locdir, "*.properties"))
                    LoadFromPropertiesFile(file, locale);
            }
        }

        static void LoadFromZipFile(string path, string locale)
        {
            try
            {
                var zip = new ZipFile(path);
                LoadFromZip(zip, locale);
            }
            catch (Exception e)
            {
            }
        }

        public static void LoadFromZip(ZipFile zip, string locale)
        {
            bool auto = string.IsNullOrEmpty(locale);
            foreach (ZipEntry entry in zip)
            {
                string path = entry.Name;
                string name = Path.GetFileName(path);
                if (name != null && name.EndsWith(".properties", StringComparison.InvariantCultureIgnoreCase))
                {
                    name = Path.GetFileNameWithoutExtension(name);
                    var rbStream = entry.Data.ToMemoryStream();
                    var lines = rbStream.GetResourceBundleLines();
                    string loc = locale;
                    if (auto)
                    {
                        string dir = Path.GetDirectoryName(path);
                        loc = Path.GetFileName(dir);
                        //TODO: check locale
                    }
                    var rb = new RB
                                 {
                                     IsFlex = _isFlex,
                                     IsZipped = true,
                                     Name = name,
                                     Locale = loc,
                                     Content = lines,
                                     SourcePath = zip.Name,
                                     ZipEntry = path
                                 };
                    Register(rb);
                }
            }
        }

        static void LoadFromPropertiesFile(string path, string locale)
        {
            try
            {
                string name = Path.GetFileNameWithoutExtension(path);
                var rb = new RB
                             {
                                 IsFlex = _isFlex,
                                 Name = name,
                                 Locale = locale,
                                 SourcePath = path,
                                 Content = File.ReadAllLines(path)
                             };
                Register(rb);
            }
            catch (Exception e)
            {
            }
        }

        static void Register(RB rb)
        {
            string locale = rb.Locale;
            var l = Cache[locale] as Locale;

            if (l == null)
            {
                l = new Locale();

                if (_isFlex && locale == Const.Locales.en_US)
                    Locale_en_US = l;

                Cache[locale] = l;
            }

            l.Add(rb);
        }
    }
    #endregion
}