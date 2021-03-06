using System;
using System.IO;
using DataDynamics.PageFX.Common.Extensions;
using DataDynamics.PageFX.Common.Graphics;
using DataDynamics.PageFX.Flash.Swc;

namespace DataDynamics.PageFX.Flash.Swf
{
    public static class SwfDumpService
    {
        /// <summary>
        /// True to dump images embedded to swiff file.
        /// </summary>
        public static bool DumpImages;

        /// <summary>
        /// True to dump shapes embedded to swiff file.
        /// </summary>
        public static bool DumpShapes;

        /// <summary>
        /// True to dump display list tags.
        /// </summary>
        public static bool DumpDisplayListTags;

        public static bool DumpRefs = true;

        public static bool SwfOnly;

        public static bool Verbose;

        private static string GetAbcDumpDir(string path)
        {
            string dir = Path.GetDirectoryName(path);
            dir = Path.Combine(dir, Path.GetFileName(path) + ".abcfiles");
            return dir;
        }

        public static void DumpSwf(string path)
        {
            var swf = new SwfMovie(path);
            swf.LinkAssets();
            DumpSwf(swf, path);
        }

        private static void DumpSwf(SwfMovie swf, string path)
        {
            swf.Dump(path + ".xml");
            if (!SwfOnly)
            {
                string dir = GetAbcDumpDir(path);
                foreach (var abc in swf.GetAbcFiles())
                {
                    Directory.CreateDirectory(dir);
                    //abc.DumpDirectory(dir);
                    string name = abc.Name.Replace('/', '.').Replace('\\', '.');
                    abc.DumpXml(Path.Combine(dir, name.ReplaceInvalidFileNameChars() + ".xml"));
                }

                if (DumpImages)
                {
                    SaveImages(swf, path + ".images");
                }
            }
        }

        public static void SaveImages(SwfMovie swf, string dir)
        {
            foreach (var tag in swf.Tags)
            {
                var c = tag as ISwfImageCharacter;
                if (c != null)
                {
                    var img = c.Image;
                    if (img != null)
                    {
                        Directory.CreateDirectory(dir);

                        string name = "";
                        name += c.CharacterId;
                        if (img.IsIndexed())
                            name += "_indexed";
                        if (!string.IsNullOrEmpty(c.Name))
                        {
                            name += "_";
                            name += c.Name;
                        }
                        
                        try
                        {
                            ImageExtensions.Save(img, Path.Combine(dir, name + ".png"));
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Unable to save image. Exception:\n{0}", e);
                        }
                    }
                }
            }
        }

        public static void DumpSwc(string path)
        {
            var lib = path.ExtractSwfLibrary();
            var swf = new SwfMovie(lib);
            swf.LinkAssets();
            DumpSwf(swf, path);
        }
    }
}