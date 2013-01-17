using System.ComponentModel;
using System.Linq;
using DataDynamics.PageFX.Common.Utilities;

namespace DataDynamics.PageFX.Common.CompilerServices
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
        public static readonly CliOption Format = new CliOption("format", "outformat");

        [Category(Cat_CodeGeneration)]
        [Description("Enable full reflection support.")]
        public static readonly CliOption ReflectionSupport = new CliOption("refl");

        [Category(Cat_CodeGeneration)]
        [Description("Specify the root namespace for all type declarations.")]
        public static readonly CliOption RootNamespace = new CliOption("rootnamespace", "rootns");

        [Category(Cat_CodeGeneration)]
        [Description("Emit debug information.")]
        [Format("[+|-]")]
        public static readonly CliOption Debug = new CliOption("debug");

        [Category(Cat_CodeGeneration)]
        [Description("Enable optimizations.")]
        [Format("[+|-]")]
        public static readonly CliOption Optimize = new CliOption("optimize", "o");

        [Category(Cat_CodeGeneration)]
        [Description("Specify password to connect to debug server.")]
        [Format("string")]
        public static readonly CliOption DebugPassword = new CliOption("debug-password");

        [Category(Cat_CodeGeneration)]
        [Description("Disable encoding of files in debug info")]
        public static readonly CliOption GoodDebugFiles = new CliOption("gdf", "good-debug-files");

        [Category(Cat_CodeGeneration)]
        [Description("Enable generation of debugger breaks before throw instructions")]
        [Format("[+|-]")]
        public static readonly CliOption ExceptionBreak = new CliOption("exception-break", "excbreak");
        #endregion

        #region Flex Options
        [Category(Cat_Flex)]
        [Description("Specify whether to add reference to MX (flex framework) library.")]
        public static readonly CliOption MX = new CliOption("mx");

        [Category(Cat_Flex)]
        [Description("Specify link to cross domain (signed) RSL. Properties: lib - path managed assembly (.dll) file; rsl - path to RSL (.swz) file; policyFile - path to policy file.")]
        [Format("<lib=path;(local|rslpath)=path;(rsluri|uri)=URI;[policyFile=path]>")]
        public static readonly CliOption CDRSL = new CliOption("cdrsl");

        [Category(Cat_Flex)]
        [Description("Specify link to unsigned RSL. Properties: lib - path managed assembly (.dll) file; rsl - path to RSL (.swz) file; policyFile - path to policy file.")]
        [Format("<lib=path;(local|rslpath)=path;(rsluri|uri)=URI;[policyFile=path]>")]
        public static readonly CliOption RSL = new CliOption("rsl");

        [Category(Cat_Flex)]
        [Description("Specify locales used to compile resource bundles.")]
        [Format("<en_US,ja_JP,...>")]
        public static readonly CliOption Locale = new CliOption("locale");

		[Category(Cat_Flex)]
		[Description("Specify style mixins to link to application.")]
		[Format("<path to swc>")]
		public static readonly CliOption StyleMixins = new CliOption("style-mixins");
        #endregion

        #region SWF Options
        [Category(Cat_SWF)]
        [Description("Specify full class name of root sprite.")]
        [Format("<full type name>")]
        public static readonly CliOption RootSprite =
            new CliOption("rootsprite", "root-sprite, mainsprite, main-sprite");

        [Category(Cat_SWF)]
        [Description("Specify flash runtime version. Default value is 9.")]
        [Format("number")]
        [DefaultValue(9)]
        public static readonly CliOption FlashVersion = new CliOption("fp", "flashversion;flash-version");

        [Category(Cat_SWF)]
        [Description("Enable or disables compressing of generated swiff file.")]
        [Format("[+|-]")]
        public static readonly CliOption Compressed = new CliOption("compressed");

        [Category(Cat_SWF)]
        [Description("Specify frame size.")]
        [Format("width[,height]")]
        public static readonly CliOption FrameSize = new CliOption("framesize", "frame-size");

        [Category(Cat_SWF)]
        [Description("Specify frame width.")]
        [Format("number")]
        public static readonly CliOption FrameWidth = new CliOption("framewidth", "frame-width");

        [Category(Cat_SWF)]
        [Description("Specify frame height.")]
        [Format("number")]
        public static readonly CliOption FrameHeight = new CliOption("frameheight", "frame-height");

        [Category(Cat_SWF)]
        [Description("Specify frame rate. Default value is 25.")]
        [Format("number")]
        [DefaultValue(25)]
        public static readonly CliOption FrameRate = new CliOption("framerate", "frame-rate");

        [Category(Cat_SWF)]
        [Description("Specify background color.")]
        [Format("[#rgb|#rgba]")]
        public static readonly CliOption BackgroundColor =
            new CliOption("bgcolor", "bg-color, bg_color, background-color, background_color");
        #endregion

        #region HTML Wrapper
        [Category(Cat_HTML)]
        [Description("Disable deployment of HTML wrapper for produce swf file")]
        public static readonly CliOption NoHtmlWrapper = new CliOption("nohtml", "notemplate");

        [Category(Cat_HTML)]
        [Description("Specify name of HTML template used to create HTML wrapper")]
        public static readonly CliOption HtmlTemplate = new CliOption("html-template", "html_template");

        [Category(Cat_HTML)]
        [Description("Specify title of HTML wrapper page")]
        public static readonly CliOption HtmlTitle = new CliOption("html-title");
        #endregion

        #region Tools
        [Category(Cat_Tools)]
        [Description("Convert .abc or .swc file to dll")]
        public static readonly CliOption Wrap = new CliOption("wrap");

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

            CliOption.Init(typeof(PFCOptions));
        }

        public static readonly CliOption[] All = new[]
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
                Wrap
                //NUnit
            };

        public static void Usage()
        {
            CommandLine.Usage(All, "");
        }

        public static bool Contains(string name)
        {
            return All.Any(o => o.CheckName(name, true));
        }

        public static CliOption Find(string name)
        {
            return All.FirstOrDefault(o => o.CheckName(name, true));
        }
    }
}