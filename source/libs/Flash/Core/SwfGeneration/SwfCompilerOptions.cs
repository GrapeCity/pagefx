using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using DataDynamics.PageFX.Common.CompilerServices;
using DataDynamics.PageFX.Common.Graphics;
using DataDynamics.PageFX.Common.Utilities;
using DataDynamics.PageFX.Flash.Swf;

namespace DataDynamics.PageFX.Flash.Core.SwfGeneration
{
	//TODO: use reflection to parse options by metadata

    internal sealed class SwfCompilerOptions
    {
        public const string DefaultDebugPassword = "$1$22$D52fy3bya4.1fRX17bpC00";

		public SwfCompilerOptions() : this(PfxConfig.Default)
		{
		}

		public SwfCompilerOptions(PfxConfig config)
        {
        	FrameSize = SwfMovie.DefaultFrameSize;
        	FrameRate = SwfMovie.DefaultFrameRate;
        	Compressed = true;
        	FlashVersion = config.Runtime.DefaultFlashVersion;
        	BackgroundColor = config.Swf.DefaultBgColor;
			DebugPassword = DefaultDebugPassword;
			OutputFormat = OutputFormat.SWF;

        	RslList = new RslList();

	        FlashVersion = config.Runtime.FlashVersion;
	        FrameSize = new SizeF(config.Swf.Width, config.Swf.Height);
	        BackgroundColor = config.Swf.BgColor;
	        Compressed = config.Swf.Compressed;
	        Locales = new []{ Const.Locales.en_US };
        }

        public SwfCompilerOptions(CommandLine cl) : this()
        {
            SetFrameSize(cl);
            SetFrameRate(cl);
            SetFlashVersion(cl);
            SetBgColor(cl);

            Compressed = cl.GetBoolOption(true, PFCOptions.Compressed.Names);
            RootSprite = cl.GetOption(null, PFCOptions.RootSprite.Names);
            Debug = cl.GetBoolOption(true, PFCOptions.Debug.Names);
            DebugPassword = cl.GetOption(DefaultDebugPassword, PFCOptions.DebugPassword.Names);

            SetLocales(cl);

            SetRootNamespace(cl);

            HtmlTemplate = cl.GetOption(PFCOptions.HtmlTemplate);

            IgnoreExceptionBreaks = cl.IsMinus(PFCOptions.ExceptionBreak);
            if (!IgnoreExceptionBreaks)
                ExceptionBreak = cl.HasOption(PFCOptions.ExceptionBreak);

            if (cl.HasOption(PFCOptions.NoHtmlWrapper))
                NoHtmlWrapper = true;

            RslList = RslList.Parse(cl);

            Title = cl.GetOption(PFCOptions.HtmlTitle);

        	StyleMixins = cl.GetPath("", PFCOptions.StyleMixins.Names);
        }

	    public SizeF FrameSize { get; set; }

    	public float FrameRate { get; set; }

    	public bool Compressed { get; set; }

    	public float FlashVersion { get; set; }

		public string RootSprite { get; set; }

		public Color BackgroundColor { get; set; }

		public bool Debug { get; set; }

		public string DebugPassword { get; set; }

		public RslList RslList { get; private set; }

		public string HtmlTemplate { get; set; }

		public bool NoHtmlWrapper { get; set; }

		public bool ExceptionBreak { get; set; }

		public bool IgnoreExceptionBreaks { get; set; }

		public OutputFormat OutputFormat { get; set; }

        #region FrameSize
        void SetFrameSize(CommandLine cl)
        {
            string s = cl.GetOption(PFCOptions.FrameSize);
            if (string.IsNullOrEmpty(s))
            {
                string sw = cl.GetOption(null, "framewidth", "frame-width");
                string sh = cl.GetOption(null, "frameheight", "frame-height");
                if (!string.IsNullOrEmpty(sw) && !string.IsNullOrEmpty(sh))
                {
                    int w, h;
                    if (int.TryParse(sw.Trim(), out w) && w > 0
                        && int.TryParse(sh.Trim(), out h) && h > 0)
                        FrameSize = new SizeF(w, h);
                }
            }
            else
            {
                var c = s.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                int n = c.Length;
                if (n == 0)
                {
                    return;
                }
                if (n == 1)
                {
                    int w;
                    if (int.TryParse(c[0].Trim(), out w) && w > 0)
                        FrameSize = new SizeF(w, w);
                }
                else
                {
                    int w, h;
                    if (int.TryParse(c[0].Trim(), out w) && w > 0
                        && int.TryParse(c[1].Trim(), out h) && h > 0)
                        FrameSize = new SizeF(w, h);
                }
            }
        }
        #endregion

        #region FrameRate
        private void SetFrameRate(CommandLine cl)
        {
            string s = cl.GetOption(PFCOptions.FrameRate);
            if (!string.IsNullOrEmpty(s))
            {
                float v;
                if (float.TryParse(s, out v) && v > 0)
                    FrameRate = v;
            }
        }
        #endregion

        #region FlashVersion
        void SetFlashVersion(CommandLine cl)
        {
            string s = cl.GetOption(PFCOptions.FlashVersion);
	        if (string.IsNullOrEmpty(s)) return;

	        float v;
	        if (!float.TryParse(s.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out v) && v <= 8)
	        {
				//TODO: As Warning!
				throw Errors.SWF.InvalidVersion.CreateException(v);
	        }

			FlashVersion = v;
        }
        #endregion

        #region BgColor

		void SetBgColor(CommandLine cl)
		{
			string s = cl.GetOption(PFCOptions.BackgroundColor);
			var backgroundColor = BackgroundColor;
			if (s.TryParseColor(ref backgroundColor))
			{
				BackgroundColor = backgroundColor;

			}
		}

    	#endregion

        public string OutputPath { get; set; }

        #region Application
        public string Application
        {
            get
            {
            	return !string.IsNullOrEmpty(_application)
            	       	? _application
            	       	: Path.GetFileNameWithoutExtension(OutputPath);
            }
        	set { _application = value; }
        }
        private string _application;
        #endregion

        #region Title
        public string Title
        {
            get
            {
            	return !string.IsNullOrEmpty(_title)
            	       	? _title
            	       	: Application;
            }
        	set { _title = value; }
        }
        private string _title;
        #endregion

        #region RootNamespace
        void SetRootNamespace(CommandLine cl)
        {
            string s = cl.GetOption(PFCOptions.RootNamespace);
            if (string.IsNullOrEmpty(s))
            {
                _hasrootns = cl.HasOption(PFCOptions.RootNamespace);
            }
            else
            {
                _hasrootns = true;
                _rootns = s;
            }
        }

        public string RootNamespace
        {
            get 
            {
                if (_rootns == null)
                {
                    if (_hasrootns)
                    {
                        string path = OutputPath;
                        if (!string.IsNullOrEmpty(path))
                            _rootns = Path.GetFileNameWithoutExtension(path);
                    }
                }
                return _rootns ?? "";
            }
            set { _rootns = value; }
        }
        string _rootns;
        bool _hasrootns;
        #endregion

        #region Locales
        internal static string[] DefaultLocales = new[] { Const.Locales.en_US };

        static readonly char[] comma = { ',', ';' };

        void SetLocales(CommandLine cl)
        {
            string s = cl.GetOption(PFCOptions.Locale);
            if (string.IsNullOrEmpty(s)) return;

            var locales = s.Split(comma, StringSplitOptions.RemoveEmptyEntries);
            if (locales.Length == 0) return;

            var list = new List<string>();
            foreach (string locale in locales)
            {
            	if (!locale.IsValidLocale())
            	{
            		CompilerReport.Add(Warnings.InvalidLocale, locale);
            		continue;
            	}
            	list.Add(locale);
            }

            _locales = list.ToArray();
        }

        public string[] Locales
        {
            get 
            {
                if (_locales == null || _locales.Length == 0)
                    return DefaultLocales;
                return _locales;
            }
            set 
            {
                _locales = value;
            }
        }

    	string[] _locales;
        #endregion

		/// <summary>
		/// Specifies path to SWC file with style mixins.
		/// </summary>
		public string StyleMixins { get; set; }
    }
}