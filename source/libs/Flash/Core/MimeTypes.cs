using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataDynamics.PageFX.Flash.Core
{
    internal static class MimeTypes
    {
        public static class Text
        {
            public const string Plain = "text/plain";
            public const string Css = "text/css";
            public const string Html = "text/html";
            public const string Xml = "text/xml";
            public const string Mxml = "text/mxml";
        }

        public static class Image
        {
            public const string Jpeg = "image/jpeg";
            public const string Jpg = "image/jpg";
            public const string Png = "image/png";
            public const string Gif = "image/gif";
            public const string Svg = "image/svg";
            public const string SvgXml = "image/svg-xml";
            public const string Tiff = "image/tiff";
            public const string Bmp = "image/bmp";
            public const string Xjg = "image/x-jg";
            public const string Dwg = "image/x-dwg";
            public const string Ico = "image/x-icon";
            public const string Pbm = "image/x-portable-bitmap";
        }

        public static class Audio
        {
            public const string Mpeg = "audio/mpeg";
            public const string Mp3 = "audio/mpeg";
            public const string Mp4 = "audio/mp4";
            public const string Aif = "audio/aif";
            public const string Au = "audio/x-au";
        }

        public static class Video
        {
            public const string Mp4 = "video/mp4";
            public const string Asf = "video/x-ms-asf";
            public const string Asx = "video/x-ms-asf";
            public const string Avi = "video/avi";
            public const string Avs = "video/avs-video";
            public const string Dv = "video/x-dv";
            public const string Dl = "video/dl";
        }

        public static class Application
        {
            public const string Swf = "application/x-shockwave-flash";

            public const string OctetStream = "application/octet-stream";

            public const string XFont = "application/x-font";
            public const string Ttf = "application/x-font-truetype";
            public const string Otf = "application/x-font-opentype";

            public const string Abc = "application/x-actionscript-bytecode";

            public const string Book = "application/book";

            public const string BZip = "application/x-bzip";
            public const string BZip2 = "application/x-bzip2";

            public const string GZip = "application/x-gzip";

            public const string Bsh = "application/x-bsh";

            public const string Cdf = "application/x-cdf";

            public const string Java = "application/java";

            public const string MSWord = "application/msword";

            public const string Dvi = "application/x-dvi";

            public const string AutoCAD = "application/acad";
        }

        /// <summary>
        /// Detects mime-time by source name using its extension.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string AutoDetect(string source)
        {
        	string ext = Path.GetExtension(source);
        	ext = string.IsNullOrEmpty(ext) ? null : (ext[0] == '.' ? ext.Substring(1).ToLower() : ext.ToLower());
            if (string.IsNullOrEmpty(ext)) return null;
            switch (ext)
            {
                #region text
                case "txt":
                case "c":
                case "h":
                case "cc":
                case "c++":
                case "cpp":
                case "cxx":
                case "conf":
                    return Text.Plain;

                case "css":
                    return Text.Css;

                case "htm":
                case "html":
                case "htmls":
                case "htx":
                    return Text.Html;
                #endregion

                #region image
                case "jpg":
                case "jpeg":
                    return Image.Jpeg;
                case "png":
                    return Image.Png;
                case "bmp":
                    return Image.Bmp;
                case "svg":
                case "svgz":
                    return Image.Svg;
                case "gif":
                    return Image.Gif;
                case "tiff":
                case "tif":
                    return Image.Tiff;
                case "art":
                    return Image.Xjg;
                case "dwg":
                case "dxf":
                    return Image.Dwg;
                case "ico":
                    return Image.Ico;
                case "pbm":
                    return Image.Pbm;
                #endregion

                #region audio
                case "f4a": //Audio for Adobe Flash Player
                case "f4b": //Audio Book for Adobe Flash Player
                    return Audio.Mp4;
                case "aif":
                case "aifc":
                    return Audio.Aif;
                case "au":
                    return Audio.Au;
                #endregion

                #region video
                case "f4v": //Video for Adobe Flash Player
                case "f4p": //Protected Media for Adobe Flash Player
                    return Video.Mp4;
                case "asf":
                    return Video.Asf;
                case "asx":
                    return Video.Asx;
                case "avi":
                    return Video.Avi;
                case "avs":
                    return Video.Avs;
                case "dl":
                    return Video.Dl;
                case "dif":
                case "dv":
                    return Video.Dv;
                #endregion

                #region application
                case "swf":
                    return Application.Swf;
                case "bin":
                    return Application.OctetStream;
                case "boo":
                case "book":
                    return Application.Book;
                case "bz":
                    return Application.BZip;
                case "boz":
                case "bz2":
                    return Application.BZip2;
                case "bsh":
                    return Application.Bsh;
                case "cdf":
                    return Application.Cdf;
                case "class":
                    return Application.Java;
                case "doc":
                case "dot":
                    return Application.MSWord;
                case "dvi":
                    return Application.Dvi;
                //case "dwg":
                //    return Application.AutoCAD;
                case "gz":
                case "gzip":
                    return Application.GZip;
                #endregion
            }
            return Application.OctetStream;
        }

        public static void Check(string type)
        {
            if (string.IsNullOrEmpty(type))
                throw new NotSupportedException("Empty mime-type is not supported");
        }

    	private static readonly HashSet<string> BitmapTypes = new HashSet<string>(
    		new[]
    			{
    				Image.Png,
    				Image.Gif,
    				Image.Bmp,
    				Image.Ico
    			}, StringComparer.InvariantCultureIgnoreCase);

    	private static readonly HashSet<string> JpegTypes = new HashSet<string>(
    		new[]
    			{
    				Image.Jpeg,
    				Image.Jpg
    			}, StringComparer.InvariantCultureIgnoreCase);

    	private static readonly HashSet<string> SupportedTypes = new HashSet<string>(
    		BitmapTypes.Concat(JpegTypes),
    		StringComparer.InvariantCultureIgnoreCase);
        
        public static bool IsBitmap(string type)
        {
			return BitmapTypes.Contains(type);
        }

        public static bool IsJpeg(string type)
        {
            return JpegTypes.Contains(type);
        }

        public static bool IsSupported(string type)
        {
			return SupportedTypes.Contains(type);
        }
    }
}