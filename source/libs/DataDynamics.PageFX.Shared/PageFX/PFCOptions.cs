using System.ComponentModel;

namespace DataDynamics.PageFX
{
    /// <summary>
    /// Contains pfc options
    /// </summary>
    public static class PFCOptions
    {
        const string Cat_CodeGeneration = "CODE GENERATION";
        const string Cat_Flex = "FLEX";
        const string Cat_SWF = "SWF GENERATION";
        const string Cat_HTML = "HTML WRAPPER";
        const string Cat_Tools = "TOOLS";

        #region Code Generation
        [Category(Cat_CodeGeneration)]
        [Description("Specify output format.")]
        [Format("[abc|swf|swc]")]
        public static readonly CLOption Format = new CLOption("format", "outformat");

        [Category(Cat_CodeGeneration)]
        [Description("Enable full reflection support.")]
        public static readonly CLOption ReflectionSupport = new CLOption("refl");

        [Category(Cat_CodeGeneration)]
        [Description("Specify the root namespace for all type declarations.")]
        public static readonly CLOption RootNamespace = new CLOption("rootnamespace", "rootns");

        [Category(Cat_CodeGeneration)]
        [Description("Emit debug information.")]
        [Format("[+|-]")]
        public static readonly CLOption Debug = new CLOption("debug");

        [Category(Cat_CodeGeneration)]
        [Description("Enable optimizations.")]
        [Format("[+|-]")]
        public static readonly CLOption Optimize = new CLOption("optimize", "o");

        [Category(Cat_CodeGeneration)]
        [Description("Specify password to connect to debug server.")]
        [Format("string")]
        public static readonly CLOption DebugPassword = new CLOption("debug-password");

        [Category(Cat_CodeGeneration)]
        [Description("Disable encoding of files in debug info")]
        public static readonly CLOption GoodDebugFiles = new CLOption("gdf", "good-debug-files");

        [Category(Cat_CodeGeneration)]
        [Description("Enable generation of debugger breaks before throw instructions")]
        [Format("[+|-]")]
        public static readonly CLOption ExceptionBreak = new CLOption("exception-break", "excbreak");
        #endregion

        #region Flex Options
        [Category(Cat_Flex)]
        [Description("Specify whether to add reference to MX (flex framework) library.")]
        public static readonly CLOption MX = new CLOption("mx");

        [Category(Cat_Flex)]
        [Description("Specify link to cross domain (signed) RSL. Properties: lib - path managed assembly (.dll) file; rsl - path to RSL (.swz) file; policyFile - path to policy file.")]
        [Format("<lib=path;(local|rslpath)=path;(rsluri|uri)=URI;[policyFile=path]>")]
        public static readonly CLOption CDRSL = new CLOption("cdrsl");

        [Category(Cat_Flex)]
        [Description("Specify link to unsigned RSL. Properties: lib - path managed assembly (.dll) file; rsl - path to RSL (.swz) file; policyFile - path to policy file.")]
        [Format("<lib=path;(local|rslpath)=path;(rsluri|uri)=URI;[policyFile=path]>")]
        public static readonly CLOption RSL = new CLOption("rsl");

        [Category(Cat_Flex)]
        [Description("Specify locales used to compile resource bundles.")]
        [Format("<en_US,ja_JP,...>")]
        public static readonly CLOption Locale = new CLOption("locale");

		[Category(Cat_Flex)]
		[Description("Specify style mixins to link to application.")]
		[Format("<path to swc>")]
		public static readonly CLOption StyleMixins = new CLOption("style-mixins");
        #endregion

        #region SWF Options
        [Category(Cat_SWF)]
        [Description("Specify full class name of root sprite.")]
        [Format("<full type name>")]
        public static readonly CLOption RootSprite =
            new CLOption("rootsprite", "root-sprite, mainsprite, main-sprite");

        [Category(Cat_SWF)]
        [Description("Specify flash runtime version. Default value is 9.")]
        [Format("number")]
        [DefaultValue(9)]
        public static readonly CLOption FlashVersion = new CLOption("fp", "flashversion;flash-version");

        [Category(Cat_SWF)]
        [Description("Enable or disables compressing of generated swiff file.")]
        [Format("[+|-]")]
        public static readonly CLOption Compressed = new CLOption("compressed");

        [Category(Cat_SWF)]
        [Description("Specify frame size.")]
        [Format("width[,height]")]
        public static readonly CLOption FrameSize = new CLOption("framesize", "frame-size");

        [Category(Cat_SWF)]
        [Description("Specify frame width.")]
        [Format("number")]
        public static readonly CLOption FrameWidth = new CLOption("framewidth", "frame-width");

        [Category(Cat_SWF)]
        [Description("Specify frame height.")]
        [Format("number")]
        public static readonly CLOption FrameHeight = new CLOption("frameheight", "frame-height");

        [Category(Cat_SWF)]
        [Description("Specify frame rate. Default value is 25.")]
        [Format("number")]
        [DefaultValue(25)]
        public static readonly CLOption FrameRate = new CLOption("framerate", "frame-rate");

        [Category(Cat_SWF)]
        [Description("Specify background color.")]
        [Format("[#rgb|#rgba]")]
        public static readonly CLOption BackgroundColor =
            new CLOption("bgcolor", "bg-color, bg_color, background-color, background_color");
        #endregion

        #region HTML Wrapper
        [Category(Cat_HTML)]
        [Description("Disable deployment of HTML wrapper for produce swf file")]
        public static readonly CLOption NoHtmlWrapper = new CLOption("nohtml", "notemplate");

        [Category(Cat_HTML)]
        [Description("Specify name of HTML template used to create HTML wrapper")]
        public static readonly CLOption HtmlTemplate = new CLOption("html-template", "html_template");

        [Category(Cat_HTML)]
        [Description("Specify title of HTML wrapper page")]
        public static readonly CLOption HtmlTitle = new CLOption("html-title");
        #endregion

        #region Tools
        [Category(Cat_Tools)]
        [Description("Convert .abc or .swc file to dll")]
        public static readonly CLOption Wrap = new CLOption("wrap");

        //[Category(Cat_Tools)]
        //[Description("Run NUnit test runner")]
        //public static readonly CLOption NUnit = new CLOption("nunit");
        #endregion

        static PFCOptions()
        {
            Debug.DotNetCompiler = true;
            Debug.PlusMinus = true;

            Optimize.DotNetCompiler = true;
            Optimize.PlusMinus = true;

            ExceptionBreak.PlusMinus = true;
            Compressed.PlusMinus = true;

            CLOption.Init(typeof(PFCOptions));
        }

        public static readonly CLOption[] All = new[]
            {
                //code generation
                Format,
                ReflectionSupport,
                RootNamespace,
                Debug,
                DebugPassword,
                GoodDebugFiles,

                //flex
                MX,
                CDRSL,
                RSL,
                Locale,
				StyleMixins,
                
                //swf
                RootSprite,
                FlashVersion,
                Compressed,
                FrameSize,
                FrameWidth,
                FrameHeight,
                FrameRate,
                BackgroundColor,              

                //html
                NoHtmlWrapper,
                HtmlTemplate,
                HtmlTitle,

                //tools
                Wrap,
                //NUnit
            };

        public static void Usage()
        {
            CommandLine.Usage(All, "");
        }

        public static bool Contains(string name)
        {
            return Algorithms.Contains(All, o => o.CheckName(name, true));
        }

        public static CLOption Find(string name)
        {
            return Algorithms.Find(All, o => o.CheckName(name, true));
        }
    }
}