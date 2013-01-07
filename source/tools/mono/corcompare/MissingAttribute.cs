// Mono.Util.CorCompare.MissingAttribute
//
// Author(s):
//   Piers Haken (piersh@friskit.com)
//
// (C) 2001-2002 Piers Haken
using System;
using System.Xml;
using System.Reflection;
using System.Collections;

namespace Mono.Util.CorCompare
{

	/// <summary>
	/// 	Represents an Attribute that is completely missing
	/// </summary>
	/// <remarks>
	/// 	created by - Piersh
	/// 	created on - 3/2/2002 9:47:00 pm
	/// </remarks>
	class MissingAttribute : MissingBase
	{
		// e.g. <attribute name="Equals" status="missing"/>
		Object attributeMono;
		Object attributeMS;
		static Hashtable htIgnore;

		static MissingAttribute ()
		{
			htIgnore = new Hashtable
				{
					{"System.Runtime.InteropServices.ClassInterfaceAttribute", null},
					{"System.Diagnostics.DebuggerHiddenAttribute", null},
					{"System.Diagnostics.DebuggerStepThroughAttribute", null},
					{"System.Runtime.InteropServices.GuidAttribute", null},
					{"System.Runtime.InteropServices.InterfaceTypeAttribute", null},
					{"System.Runtime.InteropServices.ComVisibleAttribute", null}
				};
		}

		public MissingAttribute (Object _attributeMono, Object _attributeMS) 
		{
			attributeMono = _attributeMono;
			attributeMS = _attributeMS;
			m_nodeStatus = new NodeStatus (attributeMono, attributeMS);
		}

		public override string Name 
		{
			get { return Attribute.ToString (); }
		}

		public override string Type
		{
			get { return "attribute"; }
		}

		public override NodeStatus Analyze ()
		{
			return m_nodeStatus;
		}


		public Object Attribute
		{
			get { return attributeMono ?? attributeMS; }
		}

		/// <summary>
		/// creates a map from a list of attributes
		/// the hashtable maps from name to attribute
		/// </summary>
		/// <param name="rgAttributes">the list of attributes</param>
		/// <returns>a map</returns>
		public static Hashtable GetAttributeMap (Object [] rgAttributes)
		{
			var map = new Hashtable ();
			foreach (var attribute in rgAttributes)
			{
				if (attribute != null)
				{
					string strName = attribute.ToString ();
					if (!map.Contains (strName) && !htIgnore.Contains (strName))
						map.Add (strName, attribute);
				}
			}
			return map;
		}

		/// <summary>
		/// analyzes two sets of reflected attributes, generates a list
		/// of MissingAttributes according to the completion of the first set wrt the second.
		/// </summary>
		/// <param name="rgAttributesMono">mono attributes</param>
		/// <param name="rgAttributesMS">microsoft attributes</param>
		/// <param name="rgAttributes">where the results are put</param>
		/// <returns>completion info for the whole set</returns>
		public static NodeStatus AnalyzeAttributes (Object [] rgAttributesMono, Object [] rgAttributesMS, ArrayList rgAttributes)
		{
			var nodeStatus = new NodeStatus ();

			var mapAttributesMono = (rgAttributesMono == null) ? new Hashtable () : GetAttributeMap (rgAttributesMono);
			var mapAttributesMS   = (rgAttributesMS   == null) ? new Hashtable () : GetAttributeMap (rgAttributesMS);

			foreach (var attribute in mapAttributesMS.Values)
			{
				string strAttribute = attribute.ToString ();
				var attributeMono = mapAttributesMono [strAttribute];
				var ma = new MissingAttribute (attributeMono, attribute);
				rgAttributes.Add (ma);
				var nsAttribute = ma.Analyze ();
				nodeStatus.AddChildren (nsAttribute);

				if (attributeMono != null)
					mapAttributesMono.Remove (strAttribute);
			}
			foreach (var attribute in mapAttributesMono.Values)
			{
				if (attribute.ToString ().EndsWith ("MonoTODOAttribute"))
				{
					nodeStatus.SetError (ErrorTypes.Todo);
					//nodeStatus.statusCountsChildren.errorCounts.Add (ErrorTypes.Todo);
					//nodeStatus.statusCountsTotal.errorCounts.Add (ErrorTypes.Todo);
					//nodeStatus.cTodo ++;	// this is where ALL the 'todo's come from
				}
				else if (attribute.ToString ().EndsWith ("DllImportAttribute") || attribute.ToString ().EndsWith ("PreserveSigAttribute")) {
					// Ignore these
				}
				else
				{
					var ma = new MissingAttribute (attribute, null);
					rgAttributes.Add (ma);
					var nsAttribute = ma.Analyze ();
					nodeStatus.AddChildren (nsAttribute);
				}
			}
			return nodeStatus;
		}
	}
}
