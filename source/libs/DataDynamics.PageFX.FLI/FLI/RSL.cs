using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DataDynamics.PageFX.FLI.SWC;

namespace DataDynamics.PageFX.FLI
{
    #region RSLItem
    /// <summary>
    /// Contains info about RSL
    /// </summary>
    public class RSLItem
    {
        #region Properties
        /// <summary>
        /// Gets or sets path to RSL.
        /// </summary>
        public string LocalPath
        {
            get { return _localPath; }
            set { _localPath = value; }
        }
        private string _localPath;

        /// <summary>
        /// Gets or sets URI to RSL.
        /// </summary>
        public string URI
        {
            get { return _uri; }
            set { _uri = value; }
        }
        private string _uri;

        /// <summary>
        /// Gets or sets path to managed assembly (wrapper).
        /// </summary>
        public string Library
        {
            get { return _lib; }
            set { _lib = value; }
        }
        private string _lib;

        /// <summary>
        /// Gets or sets path to policyFile.
        /// </summary>
        public string PolicyFile
        {
            get { return _policyFile; }
            set { _policyFile = value; }
        }
        private string _policyFile;

        public string[] PolicyFiles
        {
            get
            {
                if (_policyFiles == null)
                {
                    if (string.IsNullOrEmpty(_policyFile))
                        _policyFiles = new[] { "" };
                    else
                        _policyFiles = new[] { _policyFile };
                }
                return _policyFiles;
            }
        }
        private string[] _policyFiles;

        public bool IsCrossDomain { get; set; }

        public string HashType
        {
            get
            {
                if (string.IsNullOrEmpty(_hashType))
                    return HashHelper.TypeSHA256;
                return _hashType;
            }
            set { _hashType = value; }
        }
        private string _hashType = HashHelper.TypeSHA256;

        public bool IsSigned
        {
            get
            {
                if (IsCrossDomain)
                {
                    //NOTE: Currently we assume that signed RSL file has .swz extension.
                    if (LocalPath != null 
                        && LocalPath.EndsWith(".swz", StringComparison.InvariantCultureIgnoreCase))
                        return true;
                }
                return false;
            }
        }

        public SwcFile SWC { get; set; }
        #endregion

        #region Parse
        private static readonly char[] Semicolon = { ';' };
        private static readonly char[] EqualitySign = { '=' };

        private static string GetExt(string path)
        {
            if (string.IsNullOrEmpty(path)) return "";
            string ext = Path.GetExtension(path);
            if (string.IsNullOrEmpty(ext)) return "";
            if (ext[0] == '.')
                ext = ext.Substring(1);
            return ext.ToLower();
        }

        public static RSLItem Parse(string s, bool crossDomain)
        {
            var rsl = new RSLItem();
            rsl.IsCrossDomain = crossDomain;
            var pairs = s.Split(Semicolon, StringSplitOptions.RemoveEmptyEntries);
            if (pairs.Length <= 0)
                throw new FormatException("Unable to parse RSL info");

            bool mx = false;
            foreach (var pair in pairs)
            {
                if (string.Compare(pair, "mx", true) == 0)
                {
                    rsl.Library = GlobalSettings.GetMxLibraryPath();
                    mx = true;
                }
                else
                {
                    var kv = pair.Split(EqualitySign, StringSplitOptions.RemoveEmptyEntries);
                    if (kv.Length <= 0)
                        throw new FormatException("Bad format of RSL string");

                    if (kv.Length == 1)
                    {
                        string path = kv[0];
                        string ext = GetExt(path);
                        switch (ext)
                        {
                            case "dll":
                                rsl.Library = path;
                                break;

                            case "swf":
                            case "swz":
                                rsl.LocalPath = path;
                                break;

                                //???
                            case "xml":
                                rsl.PolicyFile = path;
                                break;
                        }
                    }
                    else
                    {
                        rsl.SetValue(kv[0], kv[1]);
                    }
                }
            }

            if (mx)
            {
                rsl.IsCrossDomain = true;
                if (string.IsNullOrEmpty(rsl.LocalPath))
                    rsl.LocalPath = GetMxRSL();
            }

            rsl.Resolve();

            return rsl;
        }

        private static string GetMxRSL()
        {
            string path = FindFrameworkRSL(Environment.CurrentDirectory);
            if (!string.IsNullOrEmpty(path))
                return path;

            return FindFrameworkRSL(GlobalSettings.RslsDirectory);
        }

        private static string FindFrameworkRSL(string dir)
        {
            if (!Directory.Exists(dir))
                return "";

            //TODO: Handle files.Length > 1
            var files = Directory.GetFiles(dir, "framework_*.swz", SearchOption.TopDirectoryOnly);
            if (files != null && files.Length == 1)
                return files[0];

            files = Directory.GetFiles(dir, "framework_*.swf", SearchOption.TopDirectoryOnly);
            if (files != null && files.Length == 1)
                return files[0];

            return "";
        }

        public bool SetValue(string key, string value)
        {
            if (string.Compare(key, "lib", true) == 0
                || string.Compare(key, "library", true) == 0)
            {
                _lib = value;
                return true;
            }

            if (string.Compare(key, "local", true) == 0
                || string.Compare(key, "localpath", true) == 0
                || string.Compare(key, "local-path", true) == 0
                || string.Compare(key, "local_path", true) == 0
                || string.Compare(key, "rsl", true) == 0
                || string.Compare(key, "rslpath", true) == 0
                || string.Compare(key, "rsl-path", true) == 0
                || string.Compare(key, "rsl_path", true) == 0)
            {
                _localPath = value;
                return true;
            }

            if (string.Compare(key, "policyfile", true) == 0
                || string.Compare(key, "policy-file", true) == 0
                || string.Compare(key, "policy_file", true) == 0)
            {
                _policyFile = value;
                return true;
            }

            if (string.Compare(key, "hashtype", true) == 0
                || string.Compare(key, "hash-type", true) == 0
                || string.Compare(key, "hash_type", true) == 0
                || string.Compare(key, "hashalg", true) == 0
                || string.Compare(key, "hash-alg", true) == 0
                || string.Compare(key, "hash_alg", true) == 0
                || string.Compare(key, "hashalgorithm", true) == 0
                || string.Compare(key, "hash-algoritm", true) == 0
                || string.Compare(key, "hash_algoritm", true) == 0)
            {
                _hashType = value;
                return true;
            }

            if (string.Compare(key, "uri", true) == 0
                || string.Compare(key, "rsluri", true) == 0
                || string.Compare(key, "rsl-uri", true) == 0
                || string.Compare(key, "rsl_uri", true) == 0)
            {
                _uri = value;
                return true;
            }

            return true;
        }
        #endregion

        #region Private Members
        private void Resolve()
        {
            AutoComplete();
            ResolveLibPath();
            ResolveLocalPath();

            if (string.IsNullOrEmpty(_uri))
                _uri = Path.GetFileName(_localPath);

            _policyFile = ResolvePath("Policy", _policyFile, false, true);
        }

        private void ResolveLibPath()
        {
            _lib = ResolvePath("Library", _lib, false, false);
        }

        private void ResolveLocalPath()
        {
            _localPath = ResolvePath("RSL", _localPath, false, false);
        }

        private void AutoComplete()
        {
            if (string.IsNullOrEmpty(_lib))
            {
                CheckEmpty(_localPath, "RSL");
                ResolveLocalPath();

                _lib = Path.ChangeExtension(_localPath, ".dll");

                CheckFile(_lib, "Library");
                return;
            }

            if (string.IsNullOrEmpty(_localPath))
            {
                CheckEmpty(_lib, "Library");

                ResolveLibPath();

                _localPath = Path.ChangeExtension(_lib, ".swf");

                DetectLocalPath();

                CheckFile(_localPath, "RSL");
            }
        }

        private void DetectLocalPath()
        {
            if (File.Exists(_localPath)) return;

            //try .swz file
            _localPath = Path.ChangeExtension(_localPath, ".swz");

            if (File.Exists(_localPath)) return;

            //try .swc file
            _localPath = Path.ChangeExtension(_localPath, ".swc");

            CheckFile(_localPath, "RSL");

            var lib = SwcHelper.ExtractLibrary(_localPath);
            if (lib == null)
            {
                string reason = string.Format(". Unable to extract library.swf from swc file '{0}'", _localPath);
                throw Errors.RSL.UnableToResolve.CreateException(this + reason);
            }

            _localPath = Path.ChangeExtension(_localPath, ".swf");
            Stream2.SaveStream(lib, _localPath);
        }

        private string ResolvePath(string prefix, string path, bool uri, bool optional)
        {
            if (uri)
                prefix = ". " + prefix + " URI ";
            else
                prefix = ". " + prefix + " file ";

            if (string.IsNullOrEmpty(path))
            {
                if (optional) return "";
                throw Errors.RSL.UnableToResolve.CreateException(this + prefix + "is not specified");
            }

            if (!Path.IsPathRooted(path))
                path = Path.Combine(Environment.CurrentDirectory, path);

            if (!uri && !File.Exists(path))
            {
                if (optional) return "";
                throw Errors.RSL.UnableToResolve.CreateException(this + prefix +
                                                                 string.Format("{0} does not exist", path));
            }

            return path;
        }

        private void CheckEmpty(string path, string propName)
        {
            if (string.IsNullOrEmpty(path))
            {
                string reason = string.Format(". {0} file is not specified.", propName);
                throw Errors.RSL.UnableToResolve.CreateException(this + reason);
            }
        }

        private void CheckFile(string path, string propName)
        {
            if (!File.Exists(path))
            {
                string reason = string.Format(". {0} file '{1}' does not exist", propName, path);
                throw Errors.RSL.UnableToResolve.CreateException(this + reason);
            }
        }
        #endregion

        #region Object Overrides
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("/rsl:");
            if (!string.IsNullOrEmpty(_lib))
                sb.AppendFormat("lib={0};", _lib);
            if (!string.IsNullOrEmpty(_localPath))
                sb.AppendFormat("local={0};", _localPath);
            if (!string.IsNullOrEmpty(_policyFile))
                sb.AppendFormat("policyFile={0};", _policyFile);
            if (!string.IsNullOrEmpty(HashType))
                sb.AppendFormat("hashType={0};", HashType);
            return sb.ToString();
        }
        #endregion
    }
    #endregion

    #region class RSLList
    public class RSLList : List<RSLItem>
    {
        public static RSLList Parse(CommandLine cl)
        {
            var list = new RSLList();
            foreach (var item in cl.Items)
            {
                if (item.IsOption)
                {
                    if (string.Compare(item.Name, "cdrsl", true) == 0)
                    {
                        var rsl = RSLItem.Parse(item.Value, true);
                        list.Add(rsl);
                    }
                    else if (string.Compare(item.Name, "rsl", true) == 0)
                    {
                        var rsl = RSLItem.Parse(item.Value, false);
                        list.Add(rsl);
                    }
                }
            }
            return list;
        }

        public override string ToString()
        {
            return TextFormatter.ToString(this, "", "", " ");
        }
    }
    #endregion
}