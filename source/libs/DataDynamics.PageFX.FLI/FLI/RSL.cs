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
		private string _hashType = HashExtensions.TypeSHA256;
		private string[] _policyFiles;

		/// <summary>
		/// Gets or sets path to RSL.
		/// </summary>
		public string LocalPath { get; set; }

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
			get { return _policyFiles ?? (_policyFiles = string.IsNullOrEmpty(PolicyFile) ? new[] {""} : new[] {PolicyFile}); }
		}

		public bool IsCrossDomain { get; set; }

		public string HashType
		{
			get
			{
				if (string.IsNullOrEmpty(_hashType))
					return HashExtensions.TypeSHA256;
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
				if (String.Equals(pair, "mx", StringComparison.OrdinalIgnoreCase))
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

		private static readonly Dictionary<string, Action<RslItem, string>> Setters
			= new Dictionary<string, Action<RslItem, string>>(StringComparer.OrdinalIgnoreCase)
				{
					{"lib", (x, v) => x.Library = v},
					{"library", (x, v) => x.Library = v},

					{"local", (x, v) => x.LocalPath = v},
					{"localpath", (x, v) => x.LocalPath = v},
					{"local-path", (x, v) => x.LocalPath = v},
					{"local_path", (x, v) => x.LocalPath = v},
					{"rsl", (x, v) => x.LocalPath = v},
					{"rslpath", (x, v) => x.LocalPath = v},
					{"rsl-path", (x, v) => x.LocalPath = v},
					{"rsl_path", (x, v) => x.LocalPath = v},

					{"policyfile", (x, v) => x.PolicyFile = v},
					{"policy-file", (x, v) => x.PolicyFile = v},
					{"policy_file", (x, v) => x.PolicyFile = v},

					{"hashtype", (x, v) => x.HashType = v},
					{"hash-type", (x, v) => x.HashType = v},
					{"hash_type", (x, v) => x.HashType = v},
					{"hashalg", (x, v) => x.HashType = v},
					{"hash-alg", (x, v) => x.HashType = v},
					{"hash_alg", (x, v) => x.HashType = v},
					{"hashalgorithm", (x, v) => x.HashType = v},
					{"hash-algorithm", (x, v) => x.HashType = v},
					{"hash_algorithm", (x, v) => x.HashType = v},

					{"uri", (x, v) => x.Uri = v},
					{"rsluri", (x, v) => x.Uri = v},
					{"rsl-uri", (x, v) => x.Uri = v},
					{"rsl_uri", (x, v) => x.Uri = v},
				};

		public bool SetValue(string key, string value)
		{
			Action<RslItem, string> setter;
			if (Setters.TryGetValue(key, out setter))
			{
				setter(this, value);
				return true;
			}
			return false;
		}

		#endregion

		private void Resolve()
		{
			AutoComplete();
			ResolveLibPath();
			ResolveLocalPath();

			if (string.IsNullOrEmpty(Uri))
				Uri = Path.GetFileName(LocalPath);

			PolicyFile = ResolvePath("Policy", PolicyFile, false, true);
		}

		private void ResolveLibPath()
		{
			Library = ResolvePath("Library", Library, false, false);
		}

		private void ResolveLocalPath()
		{
			LocalPath = ResolvePath("RSL", LocalPath, false, false);
		}

		private void AutoComplete()
		{
			if (string.IsNullOrEmpty(Library))
			{
				CheckEmpty(LocalPath, "RSL");
				ResolveLocalPath();

				Library = Path.ChangeExtension(LocalPath, ".dll");

				CheckFile(Library, "Library");
				return;
			}

			if (string.IsNullOrEmpty(LocalPath))
			{
				CheckEmpty(Library, "Library");

				ResolveLibPath();

				LocalPath = Path.ChangeExtension(Library, ".swf");

				DetectLocalPath();

				CheckFile(LocalPath, "RSL");
			}
		}

		private void DetectLocalPath()
		{
			if (File.Exists(LocalPath)) return;

			//try .swz file
			LocalPath = Path.ChangeExtension(LocalPath, ".swz");

			if (File.Exists(LocalPath)) return;

			//try .swc file
			LocalPath = Path.ChangeExtension(LocalPath, ".swc");

			CheckFile(LocalPath, "RSL");

			Stream lib = LocalPath.ExtractSwfLibrary();
			if (lib == null)
			{
				string reason = string.Format(". Unable to extract library.swf from swc file '{0}'", LocalPath);
				throw Errors.RSL.UnableToResolve.CreateException(this + reason);
			}

			LocalPath = Path.ChangeExtension(LocalPath, ".swf");
			lib.Save(LocalPath);
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
			if (!string.IsNullOrEmpty(LocalPath))
				sb.AppendFormat("local={0};", LocalPath);
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
				if (String.Equals(item.Name, "cdrsl", StringComparison.OrdinalIgnoreCase))
				{
					var rsl = RslItem.Parse(item.Value, true);
					list.Add(rsl);
				}
				else if (String.Equals(item.Name, "rsl", StringComparison.OrdinalIgnoreCase))
				{
					var rsl = RslItem.Parse(item.Value, false);
					list.Add(rsl);
				}
			}
			return list;
		}

		public override string ToString()
		{
			return this.Join("", "", " ");
		}
	}
}