using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using DataDynamics.PageFX.Common.Utilities;

namespace DataDynamics.PageFX.Common.CompilerServices
{
    public sealed class PfxConfig
    {
	    private static PfxConfig _default;

		//TODO: reduce usage of PfxConfig.Default

	    public static PfxConfig Default
	    {
		    get
		    {
			    if (_default == null)
			    {
				    var config = new PfxConfig();
				    config.Load();
				    _default = config;
			    }
			    return _default;
		    }
	    }

	    private SimpleConfig _config = new SimpleConfig();
        
        private static string GetDefaultPath()
        {
            string exe = GlobalSettings.PfcPath;
            return Path.Combine(Path.GetDirectoryName(exe), "pfc.config");
        }

		public void Load(string path)
		{
			_config = File.Exists(path) ? new SimpleConfig(path) : new SimpleConfig();

			// reset sections
			_flashPlayer = null;
			_compiler = null;
			_runtime = null;
			_swf = null;
			_flex = null;
			_html = null;
		}

        public void Load()
        {
			Load(GetDefaultPath());
        }

        public void Save()
        {
            _config.Save(GetDefaultPath());
        }

	    private sealed class Section
		{
			private readonly SimpleConfig _config;
			private readonly string _key;

			public Section(SimpleConfig config, string key)
			{
				_config = config;
				_key = key;
			}

			public T Get<T>(string name, T defval, Predicate<T> validator)
			{
				var key = _key + "." + name;
				return _config.Get(key, defval, validator);
			}

			public T Get<T>(string name, T defval)
			{
				var key = _key + "." + name;
				return _config.Get(key, defval);
			}

		    public void Set<T>(string name, T value)
			{
				var key = _key + "." + name;
				_config.Set(key, value);
			}

		    private static readonly Regex _repath = new Regex("%(?<env>.*)%", RegexOptions.Compiled);

			public string GetPath(string name)
			{
				var key = _key + "." + name;
				string path = _config[key, null];
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
		}

	    #region flash-player section
        
	    public FlashPlayerSection FlashPlayer
	    {
			get { return _flashPlayer ?? (_flashPlayer = new FlashPlayerSection(_config)); }
	    }
		private FlashPlayerSection _flashPlayer;

        public sealed class FlashPlayerSection
        {
	        private readonly Section _section;

			internal FlashPlayerSection(SimpleConfig config)
			{
				_section = new Section(config, "flash-player");
			}

            public string Path
            {
                get { return _section.GetPath("path"); }
                internal set { _section.Set("path", value); }
            }
        }

        #endregion

        #region compiler section

	    public CompilerSection Compiler
	    {
		    get { return _compiler ?? (_compiler = new CompilerSection(_config)); }
	    }
		private CompilerSection _compiler;

        public sealed class CompilerSection
        {
	        private readonly Section _section;

			internal CompilerSection(SimpleConfig config)
			{
				_section = new Section(config, "compiler");
			}

	        public bool ExceptionBreak
            {
                get { return _section.Get("exception-break", true); }
                set { _section.Set("exception-break", value); }
            }
        }

        #endregion

        #region runtime section

	    public RuntimeSection Runtime
	    {
			get { return _runtime ?? (_runtime = new RuntimeSection(_config)); }
	    }
		private RuntimeSection _runtime;

        public sealed class RuntimeSection
        {
	        private readonly Section _section;

	        public RuntimeSection(SimpleConfig config)
	        {
		        _section = new Section(config, "runtime");
	        }

	        public float DefaultFlashVersion = 9;

            public float FlashVersion
            {
                get
                {
                    return _section.Get(
                        "version",
                        DefaultFlashVersion,
                        v => v >= 9 && v <= 11.5);
                }
                set { _section.Set("version", value); }
            }
        }

        #endregion

        #region swf section

	    public SwfSection Swf
	    {
			get { return _swf ?? (_swf = new SwfSection(_config)); }
	    }
		private SwfSection _swf;

        public sealed class SwfSection
        {
	        private readonly Section _section;

	        public SwfSection(SimpleConfig config)
	        {
		        _section = new Section(config, "swf");
	        }

	        public bool Compressed
            {
                get { return _section.Get("compressed", true); }
                internal set { _section.Set("compressed", value); }
            }

            public int DefaultWidth = 800;

            public int Width
            {
                get { return _section.Get("width", DefaultWidth, v => v > 0); }
                internal set { _section.Set("width", value); }
            }

            public int DefaultHeight = 800;

            public int Height
            {
                get { return _section.Get("height", DefaultHeight, v => v > 0); }
                internal set { _section.Set("height", value); }
            }

            public Color DefaultBgColor
            {
                get { return Color.FromArgb(0x86, 0x96, 0xA7); }
            }

            public Color BgColor
            {
                get { return _section.Get("bgcolor", DefaultBgColor); }
                internal set { _section.Set("bgcolor", value); }
            }
        }

        #endregion

        #region flex section

	    public FlexSection Flex
	    {
			get { return _flex ?? (_flex = new FlexSection(_config)); }
	    }
		private FlexSection _flex;

        public sealed class FlexSection
        {
	        private readonly Section _section;

	        public FlexSection(SimpleConfig config)
	        {
		        _section = new Section(config, "flex");
	        }

	        public string DefaultLocale
            {
                get { return "en_US"; }
            }

            public string Locale
            {
                get { return _section.Get("locale", DefaultLocale); }
                set { _section.Set("locale", value); }
            }
        }

        #endregion

        #region html section

	    public HtmlSection Html
	    {
			get { return _html ?? (_html = new HtmlSection(_config)); }
	    }
		private HtmlSection _html;

        public sealed class HtmlSection
        {
	        private readonly Section _section;

	        public HtmlSection(SimpleConfig config)
	        {
		        _section = new Section(config, "html");
	        }

	        public string DefaultTemplate
            {
                get { return "swfobject"; }
            }

            public string Template
            {
                get { return _section.Get("template", DefaultTemplate); }
                set { _section.Set("template", value); }
            }
        }

        #endregion
    }
}