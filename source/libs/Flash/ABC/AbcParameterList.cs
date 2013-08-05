using System;
using System.Collections.Generic;
using System.Xml;
using DataDynamics.PageFX.Common.Collections;

namespace DataDynamics.PageFX.FlashLand.Abc
{
	/// <summary>
	/// List of <see cref="AbcParameter"/> objects.
	/// </summary>
	public sealed class AbcParameterList : List<AbcParameter>, ISupportXmlDump, IReadOnlyList<AbcParameter>
	{
		public void DumpXml(XmlWriter writer)
		{
			if (Count > 0)
			{
				writer.WriteStartElement("params");
				foreach (var p in this)
					p.DumpXml(writer);
				writer.WriteEndElement();
			}
		}

		public void CopyFrom(AbcMethod method)
		{
			if (method == null)
				throw new ArgumentNullException("method");
			CopyFrom(method.Parameters);
		}

		public void CopyFrom(AbcParameterList list)
		{
			if (list == null)
				throw new ArgumentNullException("list");
			foreach (var p in list)
			{
				Add(new AbcParameter(p.Type, p.Name));
			}
		}
	}
}