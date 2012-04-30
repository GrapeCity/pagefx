using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;

namespace DataDynamics.PageFX
{
    public class PfxConfig
    {
        #region data
        static SimpleConfig Config
        {
            get
            {
                Load();
                return _config;
            }
        }

        static string GetConfigPath()
        {
            string exe = GlobalSettings.PfcPath;
            return Path.Combine(Path.GetDirectoryName(exe), "pfc.config");
        }

        static void Load()
        {
            if (_config != null) return;
            string path = GetConfigPath();
            _config = File.Exists(path) ? new SimpleConfig(path) : new SimpleConfig();
        }

        internal static void Save()
        {
            Config.Save(GetConfigPath());
        }

        static SimpleConfig _config;
        #endregion

        #region Keys
        static class Keys
        {
            public const string flash_player_path = "flash-player.path";
            public const string compiler_exception_break = "compiler.exception-break";
            public const string runtime_version = "runtime.version";
            public const string swf_compressed = "swf.compressed";
            public const string swf_width = "swf.width";
            public const string swf_height = "swf.height";
            public const string swf_bgcolor = "swf.bgcolor";
            public const string html_template = "html.template";
            public const string flex_locale = "flex.locale";
        }
        #endregion

        #region flash-player section
        static readonly Regex _repath = new Regex("%(?<env>.*)%", RegexOptions.Compiled);

        static string GetPath(string key)
        {
            string path = Config[key, null];
            if (!string.IsNullOrEmpty(path))
            {
                path = _repath.Replace(path,
                    m =>
                    {
                        var env = m.Groups["env"].Value;
                        if (!string.IsNullOrEmpty(env))
                            return GlobalSettings.GetVar(env);
                        return env;
                    });
                path = path.Replace(@"\\", @"\");
                return path;
            }
            return "";
        }

        public static class FlashPlayer
        {
            public static string Path
            {
                get { return GetPath(Keys.flash_player_path); }
                internal set { Config[Keys.flash_player_path] = value; }
            }
        }
        #endregion

        #region compiler section
        public static class Compiler
        {
            public static bool ExceptionBreak
            {
                get
                {
                    return Config.GetBool(Keys.compiler_exception_break, true);
                }
                internal set 
                {
                    Config[Keys.compiler_exception_break] = value ? "true" : "false";
                }
            }
        }
        #endregion

        #region runtime section
        public static class Runtime
        {
            public const int DefaultFlashVersion = 9;

            public static int FlashVersion
            {
                get
                {
                    return Config.GetInt32(
                        Keys.runtime_version,
                        DefaultFlashVersion,
                        v => v >= 9 && v <= 10);
                }
                internal set 
                {
                    Config[Keys.runtime_version] = value.ToString();
                }
            }
        }
        #endregion

        #region swf section
        public static class SWF
        {
            public static bool Compressed
            {
                get { return Config.GetBool(Keys.swf_compressed, true); }
                internal set { Config[Keys.swf_compressed] = value ? "true" : "false"; }
            }

            public const int DefaultWidth = 800;

            public static int Width
            {
                get { return Config.GetPositiveInt32(Keys.swf_width, DefaultWidth); }
                internal set { Config[Keys.swf_width] = value.ToString(); }
            }

            public const int DefaultHeight = 800;

            public static int Height
            {
                get { return Config.GetPositiveInt32(Keys.swf_height, DefaultHeight); }
                internal set { Config[Keys.swf_height] = value.ToString(); }
            }

            public static Color DefaultBgColor
            {
                get { return Color.FromArgb(0x86, 0x96, 0xA7); }
            }

            public static Color BgColor
            {
                get
                {
                    return Config.GetColor(Keys.swf_bgcolor, DefaultBgColor);
                }
                internal set 
                {
                    Config[Keys.swf_bgcolor] = Hex.ToString(value);
                }
            }
        }
        #endregion

        #region flex section
        public static class Flex
        {
            public static string DefaultLocale
            {
                get { return "en_US"; }
            }

            public static string Locale
            {
                get
                {
                    return Config[Keys.flex_locale, DefaultLocale];
                }
                internal set
                {
                    Config[Keys.flex_locale] = value;
                }
            }
        }
        #endregion

        #region html section
        public static class HTML
        {
            public static string DefaultTemplate
            {
                get { return "swfobject"; }
            }

            public static string Template
            {
                get 
                {
                    return Config[Keys.html_template, DefaultTemplate];
                }
                internal set 
                {
                    Config[Keys.html_template] = value;
                }
            }
        }
        #endregion
    }
}