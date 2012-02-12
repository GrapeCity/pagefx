using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;

#region pfc /help
/*
/format:[abc|swf|swc]       Specifies output format.
/mx                         Specifies whether to add reference to MX library.
/cdrsl:lib=path;(local|rslpath)=path;(rsluri|uri)=URI;[policyFile=path]
                            Specifies link to cross domain (signed) RSL.
Properties:
a)      lib - path managed assembly(.dll) file
b)      rsl - path to RSL (.swz) file
c)       policyFile - path to policy file
/rsl:lib=path;(local|rslpath)=path;(rsluri|uri)=URI;[policyFile=path]
                            Specifies link to unsigned RSL.
Properties:
a)      lib - path managed assembly (.dll) file
b)      rsl - path to RSL (.swz) file
c)       policyFile - path to policy file

/fp:number                  Specifies flash runtime version. Default value is 9.
/compressed:[+|-]           Enables or disables compressing of generated swiff file.
/locale:locale name         Specifies locales used to compile resource bundles.
/rootsprite:full type name  Specifies full class name of root sprite.
/framesize:width[,height]   Specifies frame size.
/framewidth:number          Specifies frame width.
/frameheight:number         Specifies frame height.
/framerate:number           Specifies frame rate. Default value is 25.
/bgcolor:[#rgb|#rgba]       Specifies background color.
/wrap                       Converts .abc or .swc file to dll
*/
#endregion

namespace DataDynamics.PageFX
{

    #region class PfxCompilerOptions
    public class PfxCompilerOptions
    {
        public PfxCompilerOptions()
        {
            format = "swf";
            Mx = false;
            Cdrsl = null;
            Rsl = null;
            Fp = null;
            compressed = null;
            Locale = null;
            RootSprite = null;
            FrameSize = null;
            FrameWidth = null;
            FrameHeight = null;
            FrameRate = null;
            BgColor = null;
            Wrap = false;
            Nologo = false;
            Reflection = false;
        }

        public bool Reflection { get; set; }
        public bool Nologo { get; set; }
        public string Format 
        { 
            get
            {
                return format;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                value = value.ToLower();
                switch(value)
                {
                    case "abc":
                    case "swf":
                    case "swc":
                        format = value;
                        break;
                    default:
                        throw new PfxCompilerOptionsException(value);
                }
            } 
        }
        private string format;
        public bool Mx { get; set; }
        public string Cdrsl { get; set; }
        public string Rsl { get; set; }
        public string Fp { get; set; }
        public string Compressed
        {
            get
            {
                return compressed;
            }
            set
            {
                if ((value == "+") || (value == "-"))
                {
                    compressed = value;
                }
                else
                {
                    throw new ArgumentException("/compressed:" + value);
                }
            }
        }
        private string compressed;

        public string Locale { get; set; }
        public string RootSprite { get; set; }
        public string FrameSize { get; set; }
        public string FrameWidth { get; set; }
        public string FrameHeight { get; set; }
        public string FrameRate { get; set; }
        public string BgColor { get; set; }
        public bool Wrap { get; set; }

        public string Input { get; set;}
        public string Output { get; set; }

        public override string ToString()
        {
            using (var writer = new StringWriter())
            {
                writer.Write(" /format:" + format);

                if (Nologo)
                    writer.Write(" /nologo");
                
                if (Mx)
                    writer.Write(" /mx");

                if (Reflection)
                    writer.Write(" /refl");

                if (!string.IsNullOrEmpty(Cdrsl))
                    writer.Write(" /cdrsl:" + Cdrsl);

                if (!string.IsNullOrEmpty(Rsl))
                    writer.Write(" /rsl:" + Rsl);
                
                if (!string.IsNullOrEmpty(Fp))
                    writer.Write(" /fp:" + Fp);

                if (!string.IsNullOrEmpty(compressed))
                    writer.Write(" /compressed:" + Compressed);

                if (!string.IsNullOrEmpty(Locale))
                    writer.Write(" /locale:" + Locale);
                
                if (!string.IsNullOrEmpty(RootSprite))
                    writer.Write(" /rootsprite:" + RootSprite);
                
                if (!string.IsNullOrEmpty(FrameSize))
                    writer.Write(" /framesize:" + FrameSize);

                if (!string.IsNullOrEmpty(FrameWidth))
                    writer.Write(" /framewidth:" + FrameWidth);
                
                if (!string.IsNullOrEmpty(FrameHeight))
                    writer.Write(" /frameheight:" + FrameHeight);
                
                if (!string.IsNullOrEmpty(FrameRate))
                    writer.Write(" /framerate:" + FrameRate);
                
                if (!string.IsNullOrEmpty(BgColor))
                    writer.Write(" /bgcolor:" + BgColor);
                
                if (Wrap)
                    writer.Write(" /wrap:");
                
                if (!string.IsNullOrEmpty(Output))
                    writer.Write(" /out:" + Output);
                
                if (!string.IsNullOrEmpty(Input))
                    writer.Write(" " + Input);

               //TextFormatter.WriteList(writer, _input, " ");
                return writer.ToString();
            }
        }

        private static void WriteSignOption(TextWriter writer, string name, bool value)
        {
            writer.Write("/{0}{1} ", name, value ? "+" : "-");
        }

        private static void WriteList(TextWriter writer, IEnumerable<string> arr, string prefix, bool path)
        {
            if (arr == null) return;
            foreach (var s in arr)
            {
                writer.Write(prefix);
                if (path && s.IndexOf(' ') >= 0)
                    writer.Write("\"" + s + "\"");
                else
                    writer.Write(s);
                writer.Write(" ");
            }
        }
    }
    #endregion
    
    #region PfxCompilerOptionsException 
    public sealed class PfxCompilerOptionsException : Exception
    {
        public PfxCompilerOptionsException()
        {
        }

        public PfxCompilerOptionsException(string message)
            : base(message)
        {
        }

        public PfxCompilerOptionsException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    #endregion

    #region class PfxCompiler
    public static class PfxCompiler
    {
        private static string GetFileName()
        {
            return "pfc.exe";
        }

        public static FrameworkVersion FrameworkVersion = FrameworkVersion.NET_3_5;

        /// <summary>
        /// Gets path to .NET Framework root folder.
        /// </summary>
        /// <returns></returns>
        public static string GetFrameworkRoot()
        {
            return FrameworkInfo.GetRoot(FrameworkVersion);
        }

        /// <summary>
        /// Gets path to compiler.
        /// </summary>
        /// <returns></returns>
        public static string GetPath()
        {
            return GlobalSettings.BinDirectory;
        }

        private static void ParseLocation(string loc, out string file, out int line, out int col)
        {
            line = 0;
            col = 0;

            int i = loc.LastIndexOf(')');
            if (i < 0)
            {
                file = loc;
                return;
            }

            int i2 = loc.LastIndexOf('(', i);

            if (i2 >= 0 && i2 < i)
            {
                file = loc.Substring(0, i2).Trim();
                string path = loc.Substring(i2 + 1, i - i2 - 1).Trim();
                int i3 = path.IndexOf(',');
                if (i3 >= 0)
                {
                    line = int.Parse(path.Substring(0, i3).Trim());
                    col = int.Parse(path.Substring(i3 + 1).Trim());
                }
                else
                {
                    line = int.Parse(path.Trim());
                }
            }
            else
            {
                file = loc;
            }
        }

        private static bool Is(string s, int i, string value)
        {
            if (i + value.Length > s.Length)
                return false;
            for (int j = 0; j < value.Length; ++i, ++j)
            {
                if (char.ToLower(s[i]) != value[j])
                    return false;
            }
            return true;
        }

        private static string substr(string s, int start, int end)
        {
            return s.Substring(start, end - start + 1);
        }

        private static readonly string[] ErrorLevels =
            {
                "error",
                "warning",
                "warn",
            };

        private static int FindError(string s, ref string level, ref string errorNumber)
        {
            int n = s.Length;
            for (int i = 0; i < n; ++i)
            {
                foreach (string el in ErrorLevels)
                {
                	if (Is(s, i, el))
                	{
                		int si = i + el.Length + 1;
                		int colon = s.IndexOf(':', si);
                		if (colon >= 0)
                		{
                			level = el;
                			errorNumber = substr(s, si, colon - 1).Trim();
                			return i;
                		}
                	}
                }
            }
            return -1;
        }

        public static CompilerError ParseError(string s)
        {
            if (string.IsNullOrEmpty(s))
                return null;

            string file = null;
            int line = 0, col = 0;
            string level = "warning";
            string errorNumber = "CS0000";
            string errorText;

            int i = FindError(s, ref level, ref errorNumber);
            if (i >= 0)
            {
                string loc = s.Substring(0, i).Trim().TrimEnd(':');
                ParseLocation(loc, out file, out line, out col);
                errorText = s.Substring(i + level.Length + errorNumber.Length + 3).Trim();
            }
            else
            {
                errorText = s;
            }

            var err = new CompilerError(file, line, col, errorNumber, errorText);
            if (level.StartsWith("warn"))
                err.IsWarning = true;

            return err;
        }

        public static CompilerErrorCollection ParseOutput(string output)
        {
            using (var reader = new StringReader(output))
            {
                var errors = new CompilerErrorCollection();
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var err = ParseError(line);
                    if (err != null)
                        errors.Add(err);
                }
                return errors;
            }
        }

        public static bool HasErrors(string output)
        {
            return ParseOutput(output).HasErrors;
        }

        public static string Run(PfxCompilerOptions options, bool redirect)
        {
            string cmd = Path.Combine(GetPath(), "pfc.exe");
            string args = options.ToString();
            int exitCode;
            return CommandPromt.Run(cmd, args, out exitCode, false, redirect);
        }

        public static string Run(PfxCompilerOptions options)
        {
            return Run(options, true);
        }

        public static CompilerErrorCollection RunErr(PfxCompilerOptions options)
        {
            string cout = Run(options, true);
            var errors = ParseOutput(cout);
            if (errors.HasErrors)
                Console.WriteLine(cout);
            return errors;
        }
    }
    #endregion
}