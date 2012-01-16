using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DataDynamics.PageFX.FLI.SWC;

namespace DataDynamics.PageFX.FLI
{
	/// <summary>
	/// Contains info about RSL
	/// </summary>
	public class RslItem
	{
		private string _hashType = HashHelper.TypeSHA256;
		private string _localPath;
		private string[] _policyFiles;

		/// <summary>
		/// Gets or sets path to RSL.
		/// </summary>
		public string LocalPath
		{
			get { return _localPath; }
			set { _localPath = value; }
		}

		/// <summary>
		/// Gets or sets URI to RSL.
		/// </summary>
		public string Uri { get; set; }

		/// <summary>
		/// Gets or sets path to managed assembly (wrapper).
		/// </summary>
		public string Library { get; set; }

		/// <summary>
		/// Gets or sets path to policyFile.
		/// </summary>
		public string PolicyFile { get; set; }

		public string[] PolicyFiles
		{
			get
			{
				if (_policyFiles == null)
				{
					if (string.IsNullOrEmpty(PolicyFile))
						_policyFiles = new[] {""};
					else
						_policyFiles = new[] {PolicyFile};
				}
				return _policyFiles;
			}
		}

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

		public SwcFile Swc { get; set; }

		#region Parse

		private static readonly char[] Semicolon = {';'};
		private static readonly char[] EqualitySign = {'='};

		private static string GetExt(string path)
		{
			if (string.IsNullOrEmpty(path)) return "";
			string ext = Path.GetExtension(path);
			if (string.IsNullOrEmpty(ext)) return "";
			if (ext[0] == '.')
				ext = ext.Substring(1);
			return ext.ToLower();
		}

		public static RslItem Parse(string s, bool crossDomain)
		{
			var rsl = new RslItem {IsCrossDomain = crossDomain};
			string[] pairs = s.Split(Semicolon, StringSplitOptions.RemoveEmptyEntries);
			if (pairs.Length <= 0)
				throw new FormatException("Unable to parse RSL info");

			bool mx = false;
			foreach (string pair in pairs)
			{
				if (String.Compare(pair, "mx", StringComparison.OrdinalIgnoreCase) == 0)
				{
					rsl.Library = GlobalSettings.GetMxLibraryPath();
					mx = true;
				}
				else
				{
					string[] kv = pair.Split(EqualitySign, StringSplitOptions.RemoveEmptyEntries);
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
					rsl.LocalPath = GetMxRsl();
			}

			rsl.Resolve();

			return rsl;
		}

		private static string GetMxRsl()
		{
			string path = FindFrameworkRsl(Environment.CurrentDirectory);
			if (!string.IsNullOrEmpty(path))
				return path;

			return FindFrameworkRsl(GlobalSettings.RslsDirectory);
		}

		private static string FindFrameworkRsl(string dir)
		{
			if (!Directory.Exists(dir))
				return "";

			//TODO: Handle files.Length > 1
			string[] files = Directory.GetFiles(dir, "framework_*.swz", SearchOption.TopDirectoryOnly);
			if (files != null && files.Length == 1)
				return files[0];

			files = Directory.GetFiles(dir, "framework_*.swf", SearchOption.TopDirectoryOnly);
			if (files != null && files.Length == 1)
				return files[0];

			return "";
		}

		public bool SetValue(string key, string value)
		{
			if (String.Compare(key, "lib", StringComparison.OrdinalIgnoreCase) == 0
			    || String.Compare(key, "library", StringComparison.OrdinalIgnoreCase) == 0)
			{
				Library = value;
				return true;
			}

			if (String.Compare(key, "local", StringComparison.OrdinalIgnoreCase) == 0
			    || String.Compare(key, "localpath", StringComparison.OrdinalIgnoreCase) == 0
			    || String.Compare(key, "local-path", StringComparison.OrdinalIgnoreCase) == 0
			    || String.Compare(key, "local_path", StringComparison.OrdinalIgnoreCase) == 0
			    || String.Compare(key, "rsl", StringComparison.OrdinalIgnoreCase) == 0
			    || String.Compare(key, "rslpath", StringComparison.OrdinalIgnoreCase) == 0
			    || String.Compare(key, "rsl-path", StringComparison.OrdinalIgnoreCase) == 0
			    || String.Compare(key, "rsl_path", StringComparison.OrdinalIgnoreCase) == 0)
			{
				_localPath = value;
				return true;
			}

			if (String.Compare(key, "policyfile", StringComparison.OrdinalIgnoreCase) == 0
			    || String.Compare(key, "policy-file", StringComparison.OrdinalIgnoreCase) == 0
			    || String.Compare(key, "policy_file", StringComparison.OrdinalIgnoreCase) == 0)
			{
				PolicyFile = value;
				return true;
			}

			if (String.Compare(key, "hashtype", StringComparison.OrdinalIgnoreCase) == 0
			    || String.Compare(key, "hash-type", StringComparison.OrdinalIgnoreCase) == 0
			    || String.Compare(key, "hash_type", StringComparison.OrdinalIgnoreCase) == 0
			    || String.Compare(key, "hashalg", StringComparison.OrdinalIgnoreCase) == 0
			    || String.Compare(key, "hash-alg", StringComparison.OrdinalIgnoreCase) == 0
			    || String.Compare(key, "hash_alg", StringComparison.OrdinalIgnoreCase) == 0
			    || String.Compare(key, "hashalgorithm", StringComparison.OrdinalIgnoreCase) == 0
			    || String.Compare(key, "hash-algoritm", StringComparison.OrdinalIgnoreCase) == 0
			    || String.Compare(key, "hash_algoritm", StringComparison.OrdinalIgnoreCase) == 0)
			{
				_hashType = value;
				return true;
			}

			if (String.Compare(key, "uri", StringComparison.OrdinalIgnoreCase) == 0
			    || String.Compare(key, "rsluri", StringComparison.OrdinalIgnoreCase) == 0
			    || String.Compare(key, "rsl-uri", StringComparison.OrdinalIgnoreCase) == 0
			    || String.Compare(key, "rsl_uri", StringComparison.OrdinalIgnoreCase) == 0)
			{
				Uri = value;
				return true;
			}

			return true;
		}

		#endregion

		private void Resolve()
		{
			AutoComplete();
			ResolveLibPath();
			ResolveLocalPath();

			if (string.IsNullOrEmpty(Uri))
				Uri = Path.GetFileName(_localPath);

			PolicyFile = ResolvePath("Policy", PolicyFile, false, true);
		}

		private void ResolveLibPath()
		{
			Library = ResolvePath("Library", Library, false, false);
		}

		private void ResolveLocalPath()
		{
			_localPath = ResolvePath("RSL", _localPath, false, false);
		}

		private void AutoComplete()
		{
			if (string.IsNullOrEmpty(Library))
			{
				CheckEmpty(_localPath, "RSL");
				ResolveLocalPath();

				Library = Path.ChangeExtension(_localPath, ".dll");

				CheckFile(Library, "Library");
				return;
			}

			if (string.IsNullOrEmpty(_localPath))
			{
				CheckEmpty(Library, "Library");

				ResolveLibPath();

				_localPath = Path.ChangeExtension(Library, ".swf");

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

			Stream lib = SwcHelper.ExtractLibrary(_localPath);
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

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append("/rsl:");
			if (!string.IsNullOrEmpty(Library))
				sb.AppendFormat("lib={0};", Library);
			if (!string.IsNullOrEmpty(_localPath))
				sb.AppendFormat("local={0};", _localPath);
			if (!string.IsNullOrEmpty(PolicyFile))
				sb.AppendFormat("policyFile={0};", PolicyFile);
			if (!string.IsNullOrEmpty(HashType))
				sb.AppendFormat("hashType={0};", HashType);
			return sb.ToString();
		}
	}

	public class RslList : List<RslItem>
	{
		public static RslList Parse(CommandLine cl)
		{
			var list = new RslList();
			foreach (var item in cl.Items)
			{
				if (!item.IsOption) continue;
				bool crossDomain = false;
				if (String.Compare(item.Name, "cdrsl", StringComparison.OrdinalIgnoreCase) == 0)
				{
					crossDomain = true;
				}
				else if (String.Compare(item.Name, "rsl", StringComparison.OrdinalIgnoreCase) != 0)
				{
					continue;
				}
				var rsl = RslItem.Parse(item.Value, crossDomain);
				list.Add(rsl);
			}
			return list;
		}

		public override string ToString()
		{
			return TextFormatter.ToString(this, "", "", " ");
		}
	}
}