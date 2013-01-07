// Mono.Util.CorCompare.ToDoAssembly
//
// Author(s):
//   Nick Drochak (ndrochak@gol.com)
//
// (C) 2001-2002 Nick Drochak

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;

namespace Mono.Util.CorCompare {

	/// <summary>
	/// 	Represents an assembly that has missing or MonoTODO classes
	/// </summary>
	/// <remarks>
	/// 	created by - Nick
	/// 	created on - 2/20/2002 10:43:57 PM
	/// </remarks>
	class ToDoAssembly : MissingBase
	{
		// these types are in mono corlib, but not in the dll we are going to examine.
		ArrayList MissingTypes = new ArrayList();
		ArrayList rgNamespaces = new ArrayList();
		string strName;
		Assembly assMono;
		Assembly assMS;
		Type [] rgTypesMono;
		Type [] rgTypesMS;

		protected static Hashtable htGhostTypes;
		private static string[] rgstrGhostTypes = {"System.Object", "System.ValueType", "System.Delegate", "System.Enum"};


		static ToDoAssembly ()
		{
			htGhostTypes = new Hashtable ();

			foreach (string strGhostType in rgstrGhostTypes)
			{
				htGhostTypes.Add (strGhostType, null);
			}
		}

		public static ToDoAssembly Load (string strFileMono, string strName, string strNameMS)
		{
			Assembly assemblyMono = Assembly.LoadFrom (strFileMono);
			Assembly assemblyMS = Assembly.LoadWithPartialName (strNameMS);

			return new ToDoAssembly (strName, assemblyMono, assemblyMS);
		}

		public ToDoAssembly (string _strName, Assembly _assMono, Assembly _assMS)
		{
			strName = _strName;
			assMono = _assMono;
			assMS = _assMS;

			rgTypesMono = assMono.GetTypes ();
			rgTypesMS = assMS.GetTypes ();
			m_nodeStatus = new NodeStatus (_assMono, _assMS);
		}

		public override string Name {
			get {
				return strName;
			}
		}

		public override string Type
		{
			get { return "assembly"; }
		}

		private Hashtable GetNamespaceMap (IEnumerable<Type> rgTypes)
		{
			var mapTypes = new Hashtable ();
			foreach (var t in rgTypes)
			{
				if (t != null)
				{
					string strName = t.FullName;
					string strNamespace = t.Namespace;
					if (!string.IsNullOrEmpty(strNamespace) &&
						!string.IsNullOrEmpty(strName) &&
						!htGhostTypes.Contains (strName))
					{
						var rgContainedTypes = (ArrayList) mapTypes [strNamespace];
						if (rgContainedTypes == null)
						{
							rgContainedTypes = new ArrayList ();
							mapTypes [strNamespace] = rgContainedTypes;
						}
						rgContainedTypes.Add (t);
					}
				}
			}
			return mapTypes;
		}

		public override NodeStatus Analyze ()
		{
			var mapTypesMono = GetNamespaceMap (rgTypesMono);
			var mapTypesMS = GetNamespaceMap (rgTypesMS);

			foreach (string strNamespaceMS in mapTypesMS.Keys)
			{
				if (strNamespaceMS != null)
				{
					var rgContainedTypesMS = (ArrayList) mapTypesMS [strNamespaceMS];
					var rgContainedTypesMono = (ArrayList) mapTypesMono [strNamespaceMS];
					var mns = new MissingNameSpace (strNamespaceMS, rgContainedTypesMono, rgContainedTypesMS);
					var nsNamespace = mns.Analyze ();
					m_nodeStatus.AddChildren (nsNamespace);
					if (rgTypesMono != null)
						mapTypesMono.Remove (strNamespaceMS);
					rgNamespaces.Add (mns);
				}
			}
			foreach (string strNamespaceMono in mapTypesMono.Keys)
			{
				if (strNamespaceMono != null)
				{
					var rgContainedTypesMono = (ArrayList) mapTypesMono [strNamespaceMono];
					var mns = new MissingNameSpace (strNamespaceMono, rgContainedTypesMono, null);
					var nsNamespace = mns.Analyze ();
					m_nodeStatus.AddChildren (nsNamespace);
					rgNamespaces.Add (mns);
				}
			}

			rgAttributes = new ArrayList ();
			var nsAttributes = MissingAttribute.AnalyzeAttributes (
				assMono.GetCustomAttributes (true),
				assMS.GetCustomAttributes (true),
				rgAttributes);
			m_nodeStatus.Add (nsAttributes);

			return m_nodeStatus;
		}


		public string CreateClassListReport() {
			Analyze ();
			if (rgNamespaces.Count == 0) return "";

			var output = new StringBuilder();
			foreach (MissingNameSpace ns in rgNamespaces)
			{
				string[] missingTypes = ns.MissingTypeNames(true);
				if (missingTypes != null && missingTypes.Length > 0) {
					string joinedNames = String.Join("\n", missingTypes);
					output.Append(joinedNames + "\n");
				}
			}
			return output.ToString();
		}

		public override XmlElement CreateXML (XmlDocument doc)
		{
			var assemblyElem = base.CreateXML (doc);

			if (rgNamespaces.Count > 0)
			{
				var eltNamespaces = doc.CreateElement ("namespaces");
				assemblyElem.AppendChild (eltNamespaces);

				foreach (MissingNameSpace ns in rgNamespaces)
				{
					var eltNameSpace = ns.CreateXML (doc);
					if (eltNameSpace != null)
						eltNamespaces.AppendChild (eltNameSpace);
				}
			}
			return assemblyElem;
		}

		public void CreateXMLReport(string filename) {
			Analyze();

			var outDoc = new XmlDocument();
			outDoc.AppendChild(outDoc.CreateXmlDeclaration("1.0", null, null));

			var assembliesElem = outDoc.CreateElement("assemblies");
			outDoc.AppendChild(assembliesElem);

			var assemblyElem = CreateXML (outDoc);
			assembliesElem.AppendChild(assemblyElem);

			outDoc.Save(filename);
		}
	}
}
