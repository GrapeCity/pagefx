using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DataDynamics
{
    public static class PathHelper
    {
        private static bool IsRunningOnWindows
        {
            get 
            {
                return Path.PathSeparator == '\\';
            }
        }

        private static bool IgnoreCase
        {
            get
            {
                return IsRunningOnWindows;
            }
        }

        public static int Compare(string path1, string path2)
        {
            return string.Compare(path1, path2, IgnoreCase);
        }

        public static string TrimExtension(string name)
        {
            int i = name.LastIndexOf('.');
            if (i >= 0)
                return name.Substring(0, i);
            return name;
        }

        public static string ReplaceInvalidChars(IEnumerable<char> s,
            IEnumerable<char> invalidChars,
            Converter<char, char> replacer)
        {
            var list = new List<char>();
            foreach (char c in s)
            {
                if (invalidChars.Contains(c))
                {
                    char rc = replacer(c);
                    if (rc != 0)
                        list.Add(rc);
                }
                else
                {
                    list.Add(c);
                }
            }
            return new string(list.ToArray());
        }

        public static string ReplaceInvalidChars(string s, IEnumerable<char> invalidChars, char newChar)
        {
            return ReplaceInvalidChars(s, invalidChars, c => newChar);
        }

        public static string ReplaceInvalidPathChars(string s)
        {
            return ReplaceInvalidChars(s, Path.GetInvalidPathChars(), '_');
        }

        public static string ReplaceInvalidFileNameChars(string s)
        {
            return ReplaceInvalidChars(s, Path.GetInvalidFileNameChars(), '_');
        }

        /// <summary>
        /// Finds relative path.
        /// </summary>
        /// <param name="root">The root directory.</param>
        /// <param name="path">The full path to file.</param>
        /// <param name="ignoreCase">A flag indicating case-sensitive or insensitive comparison. (true indicates a case-insensitive comparison.)</param>
        /// <returns>relative path.</returns>
        public static string FindRelativePath(string root, string path, bool ignoreCase)
        {
            var r = root.Split(Path.DirectorySeparatorChar);
            var f = path.Split(Path.DirectorySeparatorChar);

            var relpath = new StringBuilder();

            int rn = r.Length;
            int fn = f.Length;
            int i = 0;
            for (; i < rn && i < fn - 1; ++i)
            {
                if (string.Compare(f[i], r[i], ignoreCase) != 0)
                    break;
            }

            for (int j = i; j < rn; ++j)
            {
                relpath.Append("..");
                relpath.Append(Path.DirectorySeparatorChar);
            }

            bool slash = false;
            for (; i < fn; ++i)
            {
                if (slash) relpath.Append(Path.DirectorySeparatorChar);
                relpath.Append(f[i]);
                slash = true;
            }

            return relpath.ToString();
        }

        /// <summary>
        /// Converts an absolute path to one relative to the given base directory path
        /// </summary>
        /// <param name="basePath">The base directory path</param>
        /// <param name="absolutePath">An absolute path</param>
        /// <returns>A path to the given absolute path, relative to the base path</returns>
        public static string AbsoluteToRelativePath(string basePath, string absolutePath)
        {
            char[] separators = {
                                    Path.DirectorySeparatorChar,
                                    Path.AltDirectorySeparatorChar,
                                    Path.VolumeSeparatorChar
                                };

            //split the paths into their component parts
            var basePathParts = basePath.Split(separators);
            var absPathParts = absolutePath.Split(separators);
            int indx = 0;

            //work out how much they have in common
            int minLength = Math.Min(basePathParts.Length, absPathParts.Length);
            for (; indx < minLength; ++indx)
            {
                if (String.Compare(basePathParts[indx], absPathParts[indx], true) != 0)
                    break;
            }

            //if they have nothing in common, just return the absolute path
            if (indx == 0)
            {
                return absolutePath;
            }


            //start constructing the relative path
            string relPath = "";

            if (indx == basePathParts.Length)
            {
                // the entire base path is in the abs path
                // so the rel path starts with "./"
                relPath += "." + Path.DirectorySeparatorChar;
            }
            else
            {
                //step up from the base to the common root 
                for (int i = indx; i < basePathParts.Length; ++i)
                {
                    relPath += ".." + Path.DirectorySeparatorChar;
                }
            }
            //add the path from the common root to the absPath
            relPath += String.Join(Path.DirectorySeparatorChar.ToString(),
                                   absPathParts, indx, absPathParts.Length - indx);

            return relPath;
        }


        /// <summary>
        /// Converts a given base and relative path to an absolute path
        /// </summary>
        /// <param name="basePath">The base directory path</param>
        /// <param name="relativePath">A path to the base directory path</param>
        /// <returns>An absolute path</returns>
        public static string RelativeToAbsolutePath(string basePath, string relativePath)
        {
            //if the relativePath isn't... 
            if (Path.IsPathRooted(relativePath))
            {
                return relativePath;
            }

            //split the paths into their component parts
            var basePathParts = basePath.Split('\\', '/');
            var relPathParts = relativePath.Split('\\', '/');

            //determine how many we must go up from the base path
            int indx = 0;
            for (; indx < relPathParts.Length; ++indx)
            {
                if (!relPathParts[indx].Equals(".."))
                {
                    break;
                }
            }

            //if the rel path contains no ".." it is below the base
            //therefor just concatonate the rel path to the base
            if (indx == 0)
            {
                int offset = 0;
                //ingnore the first part, if it is a rooting "."
                if (relPathParts[0] == ".") offset = 1;

                return basePath + Path.DirectorySeparatorChar +
                       String.Join(Path.DirectorySeparatorChar.ToString(),
                                   relPathParts, offset,
                                   relPathParts.Length - offset);
            }

            string absPath = String.Join(Path.DirectorySeparatorChar.ToString(), basePathParts, 0,
                                         Math.Max(0, basePathParts.Length - indx));

            absPath += Path.DirectorySeparatorChar + String.Join(Path.DirectorySeparatorChar.ToString(),
                                                                 relPathParts, indx,
                                                                 relPathParts.Length - indx);

            return absPath;
        }

        public static string ToFullPath(string path)
        {
            var parts = new List<string>(path.Split('/', '\\'));
            for (int i = 0; i < parts.Count; )
            {
                string part = parts[i];
                if (part == "..")
                {
                    parts.RemoveAt(i);
                    parts.RemoveAt(i - 1);
                }
                else if (part == ".")
                {
                    parts.RemoveAt(i);
                }
                else
                {
                    ++i;
                }
            }
            var sb = new StringBuilder();
            for (int i = 0; i < parts.Count; ++i)
            {
                if (i > 0) sb.Append('/');
                sb.Append(parts[i]);
            }
            return sb.ToString();
        }
    }
}