using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.IO
{
    public class PathResolver
    {
        internal List<string> _dirs = new List<string>();

    	public bool UseCurrentDirectory { get; set; }

    	private static bool CheckDirectory(string dir)
        {
            if (string.IsNullOrEmpty(dir))
            {
                //log.Warning(ErrorCodes.Path_NullOrEmpty, "Search directory is null or empty");
                return false;
            }
            if (!Path.IsPathRooted(dir))
            {
                //log.Warning(ErrorCodes.Path_NotRooted, "Specified directory {0} is not rooted", dir);
                return false;
            }
            if (!Directory.Exists(dir))
            {
                //log.Warning(ErrorCodes.Path_DirNotExist, "Specified directory {0} does not exist", dir);
                return false;
            }
            return true;
        }

        public void AddSearchDirectory(string dir)
        {
            if (!CheckDirectory(dir))
                return;

            if (_dirs.Any(s => string.Compare(s, dir, StringComparison.CurrentCultureIgnoreCase) == 0))
            {
            	return;
            }

            _dirs.Add(dir);
        }

        public void AddSearchDirectories(string dirs)
        {
            if (string.IsNullOrEmpty(dirs)) return;
            foreach (var dir in  dirs.Split(';', ','))
                AddSearchDirectory(dir);
        }

        public void AddSearchDirectoryRecursively(string dir)
        {
            if (CheckDirectory(dir))
            {
                AddSearchDirectory(dir);
                foreach (var subdir in Directory.GetDirectories(dir))
                    AddSearchDirectoryRecursively(subdir);
            }
        }

        private static string GetEnvironmentVariable(string name)
        {
            string val = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
            if (!string.IsNullOrEmpty(val))
                return val;

            val = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.User);
            if (!string.IsNullOrEmpty(val))
                return val;

            val = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Machine);
            return val;
        }

        public void AddEnvironmentVariable(string name)
        {
            string path = GetEnvironmentVariable(name);
            if (!string.IsNullOrEmpty(path))
                AddSearchDirectories(path);
        }

        public string Resolve(string path)
        {
            if (string.IsNullOrEmpty(path))
                return null;

            //Now this one is either a short file name or full path name
            if (Path.IsPathRooted(path))
            {
                if (File.Exists(path))
                    return path;
                return null;
            }

            //Check current directory
            if (UseCurrentDirectory)
            {
                string fullpath = Path.Combine(Directory.GetCurrentDirectory(), path);
                if (File.Exists(fullpath))
                    return fullpath;
            }

            //Check search locations
            int n = _dirs.Count;
            for (int i = 0; i < n; ++i)
            {
                string fullpath = Path.Combine(_dirs[i], path);
                if (File.Exists(fullpath))
                    return fullpath;
            }
            return null;
        }

        public override string ToString()
        {
            var s = new StringBuilder();
            foreach (var dir in _dirs)
                s.AppendLine(dir);
            return s.ToString();
        }
    }
}