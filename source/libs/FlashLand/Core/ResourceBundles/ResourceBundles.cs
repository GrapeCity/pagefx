using System;
using System.Collections.Generic;
using System.IO;
using DataDynamics.PageFX.Common.IO;
using Ionic.Zip;

namespace DataDynamics.PageFX.FlashLand.Core.ResourceBundles
{
	internal static class ResourceBundles
    {
        private static bool _loaded;
		private static bool _isFlex;

        private static void Load()
        {
            if (_loaded) return;
            _loaded = true;

            try
            {
                _isFlex = true;
                string dir = GlobalSettings.LocaleDirectory;
                if (Directory.Exists(dir))
                    LoadDirectory(dir);
            }
            finally
            {
                _isFlex = false;
            }
        }

        public static ResourceBundle Get(string locale, string name)
        {
            Load();

            var l = GetLocale(locale);
            if (l == null)
                //throw Errors.RBC.UnableToResolve.CreateException(name, locale);
                return null;

            var res = l.Find(name);
            if (res == null)
                //throw Errors.RBC.UnableToResolve.CreateException(name, locale);
                return null;

            return res;
        }

		private sealed class Locale
        {
			private readonly Dictionary<string, ResourceBundle> _rbcache = new Dictionary<string, ResourceBundle>();

			public ResourceBundle Find(string name)
			{
				ResourceBundle value;
				return _rbcache.TryGetValue(name, out value) ? value : null;
			}

			public void Add(ResourceBundle rb)
            {
                _rbcache.Add(rb.Name, rb);
            }
        }

		public static void CopyFlexLocales(IEnumerable<string> locales)
        {
            if (locales == null) return;
            foreach (var locale in locales)
                CopyFlexLocale(locale);
        }

        public static void CopyFlexLocale(string locale)
        {
            if (string.IsNullOrEmpty(locale)) return;
            if (_localeEnUs == null) return;
            if (Locales.ContainsKey(locale)) return;
            Locales.Add(locale, _localeEnUs);
        }

		private static readonly Dictionary<string, Locale> Locales = new Dictionary<string, Locale>(StringComparer.InvariantCultureIgnoreCase);
		private static Locale _localeEnUs;
        
        private static void LoadDirectory(string dir)
        {
            foreach (var locdir in Directory.GetDirectories(dir))
            {
                string locale = Path.GetFileName(locdir);

                foreach (var file in Directory.GetFiles(locdir, "*.swc"))
                    LoadZipFile(file, locale);

                foreach (var file in Directory.GetFiles(locdir, "*.zip"))
                    LoadZipFile(file, locale);

                foreach (var file in Directory.GetFiles(locdir, "*.properties"))
                    LoadPropertiesFile(file, locale);
            }
        }

        private static void LoadZipFile(string path, string locale)
        {
            try
            {
                var zip = new ZipFile(path);
                LoadZipFile(zip, locale);
            }
            catch (Exception e)
            {
            }
        }

        public static void LoadZipFile(ZipFile zip, string locale)
        {
            bool auto = string.IsNullOrEmpty(locale);
            foreach (ZipEntry entry in zip)
            {
                string path = entry.FileName;
                string name = Path.GetFileName(path);
                if (name != null && name.EndsWith(".properties", StringComparison.InvariantCultureIgnoreCase))
                {
                    name = Path.GetFileNameWithoutExtension(name);
                    var rbStream = entry.OpenReader().ToMemoryStream();
                    var lines = rbStream.GetResourceBundleLines();
                    string loc = locale;
                    if (auto)
                    {
                        string dir = Path.GetDirectoryName(path);
                        loc = Path.GetFileName(dir);
                        //TODO: check locale
                    }
                    var rb = new ResourceBundle
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

        private static void LoadPropertiesFile(string path, string locale)
        {
            try
            {
                string name = Path.GetFileNameWithoutExtension(path);
	            Register(new ResourceBundle
		            {
			            IsFlex = _isFlex,
			            Name = name,
			            Locale = locale,
			            SourcePath = path,
			            Content = File.ReadAllLines(path)
		            });
            }
            catch (Exception e)
            {
            }
        }

		private static Locale GetLocale(string locale)
		{
			Locale value;
			return Locales.TryGetValue(locale, out value) ? value : null;
		}

		private static void Register(ResourceBundle rb)
        {
            string name = rb.Locale;
            var locale = GetLocale(name);

            if (locale == null)
            {
                locale = new Locale();

                if (_isFlex && name.Equals(Const.Locales.en_US, StringComparison.InvariantCultureIgnoreCase))
                    _localeEnUs = locale;

                Locales.Add(name, locale);
            }

            locale.Add(rb);
        }
    }
}