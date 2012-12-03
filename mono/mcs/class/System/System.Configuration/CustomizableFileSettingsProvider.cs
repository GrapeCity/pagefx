//
// CustomizableLocalFileSettingsProvider.cs
//
// Authors:
//	Noriaki Okimoto  <seara@ojk.sppd.ne.jp>
//	Atsushi Enomoto  <atsushi@ximian.com>
//
// (C)2007 Noriaki Okimoto
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

#if NET_2_0 && CONFIGURATION_DEP

#if !TARGET_JVM
extern alias PrebuiltSystem;
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

#if TARGET_JVM
using NameValueCollection = System.Collections.Specialized.NameValueCollection;
#else
using NameValueCollection = PrebuiltSystem.System.Collections.Specialized.NameValueCollection;
#endif

namespace System.Configuration
{
	// location to store user configuration settings.
	internal enum UserConfigLocationOption : uint
	{
		Product = 0x20,
		Product_VersionMajor = 0x21,
		Product_VersionMinor = 0x22,
		Product_VersionBuild = 0x24,
		Product_VersionRevision = 0x28,
		Company_Product = 0x30,
		Company_Product_VersionMajor = 0x31,
		Company_Product_VersionMinor = 0x32,
		Company_Product_VersionBuild = 0x34,
		Company_Product_VersionRevision = 0x38,
		Other = 0x8000
	}
	
	internal class CustomizableFileSettingsProvider : SettingsProvider, IApplicationSettingsProvider
	{
		private static string userRoamingPath = "";
		private static string userLocalPath = "";
		private static string userRoamingPathPrevVersion = "";
		private static string userLocalPathPrevVersion = "";
		private static string userRoamingName = "user.config";
		private static string userLocalName = "user.config";
		private static string userRoamingBasePath = "";
		private static string userLocalBasePath = "";
		private static string CompanyName = "";
		private static string ProductName = "";
		private static string ForceVersion = "";
		private static string[] ProductVersion;

		// whether to include parts in the folder name or not:
		private static bool isVersionMajor = false;	// 0x0001	major version
		private static bool isVersionMinor = false;	// 0x0002	minor version
		private static bool isVersionBuild = false;	// 0x0004	build version
		private static bool isVersionRevision = false;	// 0x0008	revision
		private static bool isCompany = true;		// 0x0010	corporate name
		private static bool isProduct = true;		// 0x0020	product name

		private static bool userDefine = false;		// 0x8000	ignore all above and use user definition

		private static UserConfigLocationOption userConfig = UserConfigLocationOption.Company_Product;

		public override void Initialize (string name, NameValueCollection config)
		{
			base.Initialize (name, config);
		}

		// full path to roaming user.config
		internal static string UserRoamingFullPath {
			get { return Path.Combine (userRoamingPath, userRoamingName); }
		}

		// full path to local user.config
		internal static string UserLocalFullPath {
			get { return Path.Combine (userLocalPath, userLocalName); }
		}

		// previous full path to roaming user.config
		public static string PrevUserRoamingFullPath {
			get { return Path.Combine (userRoamingPathPrevVersion, userRoamingName); }
		}

		// previous full path to local user.config
		public static string PrevUserLocalFullPath {
			get { return Path.Combine (userLocalPathPrevVersion, userLocalName); }
		}

		// path to roaming user.config
		public static string UserRoamingPath {
			get { return userRoamingPath; }
		}

		// path to local user.config
		public static string UserLocalPath {
			get { return userLocalPath; }
		}

		// file name which is equivalent to user.config, for roaming user
		public static string UserRoamingName {
			get { return userRoamingName; }
		}

		// file name which is equivalent to user.config, for local user
		public static string UserLocalName {
			get { return userLocalName; }
		}

		public static UserConfigLocationOption UserConfigSelector
		{
			get { return userConfig; }
			set {
				userConfig = value;

				if (((uint) userConfig & 0x8000) != 0) {
					isVersionMajor = false;
					isVersionMinor = false;
					isVersionBuild = false;
					isVersionRevision = false;
					isCompany = false;
					return;
				}

				isVersionRevision = ((uint) userConfig & 0x0008) != 0;
				isVersionBuild = isVersionRevision | ((uint)userConfig & 0x0004) != 0;
				isVersionMinor = isVersionBuild | ((uint)userConfig & 0x0002) != 0;
				isVersionMajor = IsVersionMinor | ((uint)userConfig & 0x0001) != 0;

				isCompany = ((uint) userConfig & 0x0010) != 0;
				isProduct = ((uint) userConfig & 0x0020) != 0;
			}
		}

		// whether the path to include the major version.
		public static bool IsVersionMajor
		{
			get { return isVersionMajor; }
			set
			{
				isVersionMajor = value;
				isVersionMinor = false;
				isVersionBuild = false;
				isVersionRevision = false;
			}
		}

		// whether the path to include minor version.
		public static bool IsVersionMinor
		{
			get { return isVersionMinor; }
			set
			{
				isVersionMinor = value;
				if (isVersionMinor)
					isVersionMajor = true;
				isVersionBuild = false;
				isVersionRevision = false;
			}
		}

		// whether the path to include build version.
		public static bool IsVersionBuild
		{
			get { return isVersionBuild; }
			set
			{
				isVersionBuild = value;
				if (isVersionBuild) {
					isVersionMajor = true;
					isVersionMinor = true;
				}
				isVersionRevision = false;
			}
		}

		// whether the path to include revision.
		public static bool IsVersionRevision
		{
			get { return isVersionRevision; }
			set
			{
				isVersionRevision = value;
				if (isVersionRevision) {
					isVersionMajor = true;
					isVersionMinor = true;
					isVersionBuild = true;
				}
			}
		}

		// whether the path to include company name.
		public static bool IsCompany
		{
			get { return isCompany; }
			set { isCompany = value; }
		}

		// AssemblyCompanyAttribute->Namespace->"Program"
		private static string GetCompanyName ()
		{
			Assembly assembly = Assembly.GetEntryAssembly ();
			if (assembly == null)
				assembly = Assembly.GetCallingAssembly ();

			AssemblyCompanyAttribute [] attrs = (AssemblyCompanyAttribute []) assembly.GetCustomAttributes (typeof (AssemblyCompanyAttribute), true);
		
			if ((attrs != null) && attrs.Length > 0) {
				return attrs [0].Company;
			}

#if !TARGET_JVM
			MethodInfo entryPoint = assembly.EntryPoint;
			Type entryType = entryPoint != null ? entryPoint.DeclaringType : null;
			if (entryType != null && !String.IsNullOrEmpty (entryType.Namespace)) {
				int end = entryType.Namespace.IndexOf ('.');
				return end < 0 ? entryType.Namespace : entryType.Namespace.Substring (0, end);
			}
			return "Program";
#else
			return assembly.GetName ().Name;
#endif
		}

		private static string GetProductName ()
		{
			Assembly assembly = Assembly.GetEntryAssembly ();
			if (assembly == null)
				assembly = Assembly.GetCallingAssembly ();

#if !TARGET_JVM

			byte [] pkt = assembly.GetName ().GetPublicKeyToken ();
			byte [] hash = SHA1.Create ().ComputeHash (pkt != null ? pkt : Encoding.UTF8.GetBytes (assembly.EscapedCodeBase));
			return String.Format ("{0}_{1}_{2}",
				AppDomain.CurrentDomain.FriendlyName,
				pkt != null ? "StrongName" : "Url",
				// FIXME: it seems that something else is used
				// here, to convert hash bytes to string.
				Convert.ToBase64String (hash));

#else // AssemblyProductAttribute-based code
			AssemblyProductAttribute [] attrs = (AssemblyProductAttribute[]) assembly.GetCustomAttributes (typeof (AssemblyProductAttribute), true);
		
			if ((attrs != null) && attrs.Length > 0) {
				return attrs [0].Product;
			}
			return assembly.GetName ().Name;
#endif
		}

		private static string GetProductVersion ()
		{
			Assembly assembly = Assembly.GetEntryAssembly ();
			if (assembly == null)
				assembly = Assembly.GetCallingAssembly ();
			if (assembly == null)
				return string.Empty;

			return assembly.GetName ().Version.ToString ();
		}

		private static void CreateUserConfigPath ()
		{
			if (userDefine)
				return;

			if (ProductName == "")
				ProductName = GetProductName ();
			if (CompanyName == "")
				CompanyName = GetCompanyName ();
			if (ForceVersion == "")
				ProductVersion = GetProductVersion ().Split('.');

			// C:\Documents and Settings\(user)\Application Data
#if !TARGET_JVM
			if (userRoamingBasePath == "")
				userRoamingPath = Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData);
			else
#endif
				userRoamingPath = userRoamingBasePath;

			// C:\Documents and Settings\(user)\Local Settings\Application Data (on Windows)
#if !TARGET_JVM
			if (userLocalBasePath == "")
				userLocalPath = Environment.GetFolderPath (Environment.SpecialFolder.LocalApplicationData);
			else
#endif
				userLocalPath = userLocalBasePath;

			if (isCompany) {
				userRoamingPath = Path.Combine (userRoamingPath, CompanyName);
				userLocalPath = Path.Combine (userLocalPath, CompanyName);
			}

			if (isProduct) {
				userRoamingPath = Path.Combine (userRoamingPath, ProductName);
				userLocalPath = Path.Combine (userLocalPath, ProductName);
			}

			string versionName;

			if (ForceVersion == "") {
				if (isVersionRevision)
					versionName = String.Format ("{0}.{1}.{2}.{3}", ProductVersion [0], ProductVersion [1], ProductVersion [2], ProductVersion [3]);
				else if (isVersionBuild)
					versionName = String.Format ("{0}.{1}.{2}", ProductVersion [0], ProductVersion [1], ProductVersion [2]);
				else if (isVersionMinor)
					versionName = String.Format ("{0}.{1}", ProductVersion [0], ProductVersion [1]);
				else if (isVersionMajor)
					versionName = ProductVersion [0];
				else
					versionName = "";
			}
			else
				versionName = ForceVersion;

			string prevVersionRoaming = PrevVersionPath (userRoamingPath, versionName);
			string prevVersionLocal = PrevVersionPath (userLocalPath, versionName);
			
			userRoamingPath = Path.Combine (userRoamingPath, versionName);
			userLocalPath = Path.Combine (userLocalPath, versionName);
			if (prevVersionRoaming != "")
				userRoamingPathPrevVersion = Path.Combine(userRoamingPath, prevVersionRoaming);
			if (prevVersionLocal != "")
				userLocalPathPrevVersion = Path.Combine(userLocalPath, prevVersionLocal);
		}

		// string for the previous version. It ignores newer ones.
		private static string PrevVersionPath (string dirName, string currentVersion)
		{
			string prevVersionString = "";

			if (!Directory.Exists(dirName))
				return prevVersionString;
			DirectoryInfo currentDir = new DirectoryInfo (dirName);
			foreach (DirectoryInfo dirInfo in currentDir.GetDirectories ())
				if (String.Compare (currentVersion, dirInfo.Name, StringComparison.Ordinal) > 0)
					if (String.Compare (prevVersionString, dirInfo.Name, StringComparison.Ordinal) < 0)
						prevVersionString = dirInfo.Name;

			return prevVersionString;
		}

		// sets the explicit path to store roaming user.config or equivalent.
		// (returns the path validity.)
		public static bool SetUserRoamingPath (string configPath)
		{
			if (CheckPath (configPath))
			{
				userRoamingBasePath = configPath;
				return true;
			}
			else
				return false;
		}

		// sets the explicit path to store local user.config or equivalent.
		// (returns the path validity.)
		public static bool SetUserLocalPath (string configPath)
		{
			if (CheckPath (configPath))
			{
				userLocalBasePath = configPath;
				return true;
			}
			else
				return false;
		}

		private static bool CheckFileName (string configFile)
		{
			/*
			char[] invalidFileChars = Path.GetInvalidFileNameChars();

			foreach (char invalidChar in invalidFileChars)
			{
				if (configFile.Contains(invalidChar.ToString()))
				{
					return false;
				}
			}
			return true;
			*/
			return configFile.IndexOfAny (Path.GetInvalidFileNameChars ()) < 0;
		}

		// sets the explicit roaming file name which is user.config equivalent.
		// (returns the file name validity.)
		public static bool SetUserRoamingFileName (string configFile)
		{
			if (CheckFileName (configFile))
			{
				userRoamingName = configFile;
				return true;
			}
			else
				return false;
		}

		// sets the explicit local file name which is user.config equivalent.
		// (returns the file name validity.)
		public static bool SetUserLocalFileName (string configFile)
		{
			if (CheckFileName (configFile))
			{
				userLocalName = configFile;
				return true;
			}
			else
				return false;
		}

		// sets the explicit company name for folder.
		// (returns the file name validity.)
		public static bool SetCompanyName (string companyName)
		{
			if (CheckFileName (companyName))
			{
				CompanyName = companyName;
				return true;
			}
			else
				return false;
		}

		// sets the explicit product name for folder.
		// (returns the file name validity.)
		public static bool SetProductName (string productName)
		{
			if (CheckFileName (productName))
			{
				ProductName = productName;
				return true;
			}
			else
				return false;
		}

		// sets the explicit major version for folder.
		public static bool SetVersion (int major)
		{
			ForceVersion = string.Format ("{0}", major);
			return true;
		}

		// sets the explicit major and minor versions for folder.
		public static bool SetVersion (int major, int minor)
		{
			ForceVersion = string.Format ("{0}.{1}", major, minor);
			return true;
		}

		// sets the explicit major/minor/build numbers for folder.
		public static bool SetVersion (int major, int minor, int build)
		{
			ForceVersion = string.Format ("{0}.{1}.{2}", major, minor, build);
			return true;
		}

		// sets the explicit major/minor/build/revision numbers for folder.
		public static bool SetVersion (int major, int minor, int build, int revision)
		{
			ForceVersion = string.Format ("{0}.{1}.{2}.{3}", major, minor, build, revision);
			return true;
		}

		// sets the explicit version number string for folder.
		public static bool SetVersion (string forceVersion)
		{
			if (CheckFileName (forceVersion))
			{
				ForceVersion = forceVersion;
				return true;
			}
			else
				return false;
		}

		private static bool CheckPath (string configPath)
		{
			char[] invalidPathChars = Path.GetInvalidPathChars ();

			/*
			foreach (char invalidChar in invalidPathChars)
			{
				if (configPath.Contains (invalidChar.ToString()))
				{
					return false;
				}
			}
			*/
			if (configPath.IndexOfAny (invalidPathChars) >= 0)
				return false;

			string folder = configPath;
			string fileName;
			while ((fileName = Path.GetFileName (folder)) != "")
			{
				if (!CheckFileName (fileName))
				{
					return false;
				}
				folder = Path.GetDirectoryName (folder);
			}

			return true;
		}


		public override string Name {
			get { return base.Name; }
		}

		string app_name = String.Empty;//"OJK.CustomSetting.CustomizableLocalFileSettingsProvider";
		public override string ApplicationName {
			get { return app_name; }
			set { app_name = value; }
		}

		private ExeConfigurationFileMap exeMapCurrent = null;
		private ExeConfigurationFileMap exeMapPrev = null;
		private SettingsPropertyValueCollection values = null;

		private void SaveProperties (ExeConfigurationFileMap exeMap, SettingsPropertyValueCollection collection, ConfigurationUserLevel level, SettingsContext context, bool checkUserLevel)
		{
			Configuration config = ConfigurationManager.OpenMappedExeConfiguration (exeMap, level);
			
			UserSettingsGroup userGroup = config.GetSectionGroup ("userSettings") as UserSettingsGroup;
			bool isRoaming = (level == ConfigurationUserLevel.PerUserRoaming);

#if true // my reimplementation

			if (userGroup == null) {
				userGroup = new UserSettingsGroup ();
				config.SectionGroups.Add ("userSettings", userGroup);
				ApplicationSettingsBase asb = context.CurrentSettings;
				ClientSettingsSection cs = new ClientSettingsSection ();
				userGroup.Sections.Add (asb.GetType ().FullName, cs);
			}

			bool hasChanges = false;

			foreach (ConfigurationSection section in userGroup.Sections) {
				ClientSettingsSection userSection = section as ClientSettingsSection;
				if (userSection == null)
					continue;

				foreach (SettingsPropertyValue value in collection) {
					if (checkUserLevel && value.Property.Attributes.Contains (typeof (SettingsManageabilityAttribute)) != isRoaming)
						continue;
					hasChanges = true;
					SettingElement element = userSection.Settings.Get (value.Name);
					if (element == null) {
						element = new SettingElement (value.Name, value.Property.SerializeAs);
						userSection.Settings.Add (element);
					}
					if (element.Value.ValueXml == null)
						element.Value.ValueXml = new XmlDocument ().CreateDocumentFragment ();
					switch (value.Property.SerializeAs) {
					case SettingsSerializeAs.Xml:
						element.Value.ValueXml.InnerXml = value.SerializedValue as string;
						break;
					case SettingsSerializeAs.String:
						element.Value.ValueXml.InnerText = value.SerializedValue as string;
						break;
					case SettingsSerializeAs.Binary:
						element.Value.ValueXml.InnerText = Convert.ToBase64String (value.SerializedValue as byte []);
						break;
					default:
						throw new NotImplementedException ();
					}
				}
			}
			if (hasChanges)
				config.Save (ConfigurationSaveMode.Minimal, true);

#else // original impl. - likely buggy to miss some properties to save

			foreach (ConfigurationSection configSection in userGroup.Sections)
			{
				ClientSettingsSection userSection = configSection as ClientSettingsSection;
				if (userSection != null)
				{
/*
					userSection.Settings.Clear();

					foreach (SettingsPropertyValue propertyValue in collection)
					{
						if (propertyValue.IsDirty)
						{
							SettingElement element = new SettingElement(propertyValue.Name, SettingsSerializeAs.String);
							element.Value.ValueXml = new XmlDocument();
							element.Value.ValueXml.InnerXml = (string)propertyValue.SerializedValue;
							userSection.Settings.Add(element);
						}
					}
*/
					foreach (SettingElement element in userSection.Settings)
					{
						if (collection [element.Name] != null) {
							if (collection [element.Name].Property.Attributes.Contains (typeof (SettingsManageabilityAttribute)) != isRoaming)
								continue;

							element.SerializeAs = SettingsSerializeAs.String;
							element.Value.ValueXml.InnerXml = (string) collection [element.Name].SerializedValue;	///Value = XmlElement
						}
					}
 
				}
			}
			config.Save (ConfigurationSaveMode.Minimal, true);
#endif
		}

		private void LoadPropertyValue (SettingsPropertyCollection collection, SettingElement element, bool allowOverwrite)
		{
			SettingsProperty prop = collection [element.Name];
			if (prop == null) { // see bug #343459
				prop = new SettingsProperty (element.Name);
				collection.Add (prop);
			}

			SettingsPropertyValue value = new SettingsPropertyValue (prop);
			value.IsDirty = false;
			value.SerializedValue = element.Value.ValueXml != null ? element.Value.ValueXml.InnerText : prop.DefaultValue;
			try
			{
				if (allowOverwrite)
					values.Remove (element.Name);
				values.Add (value);
			} catch (ArgumentException) {
				throw new ConfigurationErrorsException ();
			}
		}

		private void LoadProperies (ExeConfigurationFileMap exeMap, SettingsPropertyCollection collection, ConfigurationUserLevel level, string sectionGroupName, bool allowOverwrite)
		{
			Configuration config = ConfigurationManager.OpenMappedExeConfiguration (exeMap,level);
			
			ConfigurationSectionGroup sectionGroup = config.GetSectionGroup (sectionGroupName);
			if (sectionGroup != null) {
				foreach (ConfigurationSection configSection in sectionGroup.Sections) {
					ClientSettingsSection clientSection = configSection as ClientSettingsSection;
					if (clientSection != null)
						foreach (SettingElement element in clientSection.Settings)
							LoadPropertyValue(collection, element, allowOverwrite);
				}
			}

		}

		public override void SetPropertyValues (SettingsContext context, SettingsPropertyValueCollection collection)
		{
			CreateExeMap ();

			if (UserLocalFullPath == UserRoamingFullPath)
			{
				SaveProperties (exeMapCurrent, collection, ConfigurationUserLevel.PerUserRoaming, context, false);
			} else {
				SaveProperties (exeMapCurrent, collection, ConfigurationUserLevel.PerUserRoaming, context, true);
				SaveProperties (exeMapCurrent, collection, ConfigurationUserLevel.PerUserRoamingAndLocal, context, true);
			}
		}

		public override SettingsPropertyValueCollection GetPropertyValues (SettingsContext context, SettingsPropertyCollection collection)
		{
			CreateExeMap ();

			if (values == null) {
				values = new SettingsPropertyValueCollection ();
				LoadProperies (exeMapCurrent, collection, ConfigurationUserLevel.None, "applicationSettings", false);
				LoadProperies (exeMapCurrent, collection, ConfigurationUserLevel.None, "userSettings", false);

				LoadProperies (exeMapCurrent, collection, ConfigurationUserLevel.PerUserRoaming, "userSettings", true);
				LoadProperies (exeMapCurrent, collection, ConfigurationUserLevel.PerUserRoamingAndLocal, "userSettings", true);

				// create default values if not exist
				foreach (SettingsProperty p in collection)
					if (values [p.Name] == null)
						values.Add (new SettingsPropertyValue (p));
			}
			return values;
		}

		/// creates an ExeConfigurationFileMap
		private void CreateExeMap ()
		{
			if (exeMapCurrent == null) {
				CreateUserConfigPath ();

				// current version
				exeMapCurrent = new ExeConfigurationFileMap ();
				// exeMapCurrent.ExeConfigFilename = System.Windows.Forms.Application.ExecutablePath + ".config";
				Assembly entry = Assembly.GetEntryAssembly () ?? Assembly.GetExecutingAssembly ();
				exeMapCurrent.ExeConfigFilename = entry.Location + ".config";
				exeMapCurrent.LocalUserConfigFilename = UserLocalFullPath;
				exeMapCurrent.RoamingUserConfigFilename = UserRoamingFullPath;

				// previous version
				if ((PrevUserLocalFullPath != "") && (PrevUserRoamingFullPath != ""))
				{
					exeMapPrev = new ExeConfigurationFileMap();
 					// exeMapPrev.ExeConfigFilename = System.Windows.Forms.Application.ExecutablePath + ".config";
					exeMapPrev.ExeConfigFilename = entry.Location + ".config";
					exeMapPrev.LocalUserConfigFilename = PrevUserLocalFullPath;
					exeMapPrev.RoamingUserConfigFilename = PrevUserRoamingFullPath;
				}
			}
		}

		// FIXME: implement
		public SettingsPropertyValue GetPreviousVersion (SettingsContext context, SettingsProperty property)
		{
			return null;
		}

		public void Reset (SettingsContext context)
		{
			CreateExeMap ();
			foreach (SettingsPropertyValue propertyValue in values) {
				propertyValue.PropertyValue = propertyValue.Property.DefaultValue;
				propertyValue.IsDirty = true;
			}
			SetPropertyValues (context, values);
		}

		// FIXME: implement
		public void Upgrade (SettingsContext context, SettingsPropertyCollection properties)
		{
		}

		public static void setCreate ()
		{
			CreateUserConfigPath();
		}
	}
}

#endif
