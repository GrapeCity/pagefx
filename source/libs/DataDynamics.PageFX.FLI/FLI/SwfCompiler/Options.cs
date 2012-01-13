using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using DataDynamics.PageFX.FLI.SWF;

namespace DataDynamics.PageFX.FLI
{
    class SwfCompilerOptions
    {
        public const string DefaultDebugPassword = "$1$22$D52fy3bya4.1fRX17bpC00";

        public SizeF FrameSize = SwfMovie.DefaultFrameSize;
        public float FrameRate = SwfMovie.DefaultFrameRate;
        public bool Compressed = true;
        public int FlashVersion = PfxConfig.Runtime.DefaultFlashVersion;
        public string RootSprite;
        public Color BackgroundColor = PfxConfig.SWF.DefaultBgColor;
        public bool Debug;
        public string DebugPassword = DefaultDebugPassword;
        public RSLList RSLList;
        public string HtmlTemplate;
        public bool NoHtmlWrapper;

        public bool ExceptionBreak;
        public bool IgnoreExceptionBreaks;

        public OutputFormat OutputFormat = OutputFormat.SWF;
        
        public SwfCompilerOptions()
        {
            RSLList = new RSLList();
            LoadDefaults();
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

            RSLList = RSLList.Parse(cl);

            _title = cl.GetOption(PFCOptions.HtmlTitle);
        }

        void LoadDefaults()
        {
            FlashVersion = PfxConfig.Runtime.FlashVersion;
            FrameSize = new SizeF(PfxConfig.SWF.Width, PfxConfig.SWF.Height);
            BackgroundColor = PfxConfig.SWF.BgColor;
            Compressed = PfxConfig.SWF.Compressed;
            Locales = new []{ Const.Locales.en_US };
        }

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
            if (!string.IsNullOrEmpty(s))
            {
                int v;
                if (int.TryParse(s.Trim(), out v))
                {
                    if (v <= 8 || v > 10)
                    {
                        //TODO: As Warning!
                        throw Errors.SWF.InvalidVersion.CreateException(v);
                    }
                    FlashVersion = v;
                }
            }
        }
        #endregion

        #region BgColor
        void SetBgColor(CommandLine cl)
        {
            string s = cl.GetOption(PFCOptions.BackgroundColor);
            ColorHelper.TryParse(s, ref BackgroundColor);
        }
        #endregion

        public string OutputPath { get; set; }

        #region Application
        public string Application
        {
            get
            {
                if (!string.IsNullOrEmpty(_app))
                    return _app;
                return Path.GetFileNameWithoutExtension(OutputPath);
            }
            set { _app = value; }
        }
        string _app;
        #endregion

        #region Title
        public string Title
        {
            get 
            {
                if (!string.IsNullOrEmpty(_title))
                    return _title;
                return Application;
            }
            set { _title = value; }
        }
        string _title;
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

            var arr = s.Split(comma, StringSplitOptions.RemoveEmptyEntries);
            if (arr.Length == 0) return;

            var list = new List<string>();
            for (int i = 0; i < arr.Length; ++i)
            {
                string locale = arr[i];
                if (!LocaleHelper.IsValid(locale))
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
    }
}