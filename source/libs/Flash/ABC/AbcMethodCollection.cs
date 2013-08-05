using System.IO;
using System.Xml;

namespace DataDynamics.PageFX.Flash.Abc
{
	public sealed class AbcMethodCollection : AbcAtomList<AbcMethod>
	{
		private readonly AbcFile _abc;

		public AbcMethodCollection(AbcFile abc)
		{
			_abc = abc;
		}

		protected override void OnAdded(AbcMethod item)
		{
			item.Abc = _abc;
		}

		public override void DumpXml(XmlWriter writer)
		{
			if (!AbcDumpService.DumpFunctions) return;
			writer.WriteStartElement("methods");
			writer.WriteAttributeString("count", Count.ToString());
			foreach (var m in this)
			{
				if (m.Trait != null) continue;
				if (m.IsInitializer) continue;
				m.DumpXml(writer);
			}
			writer.WriteEndElement();
		}

		public void Dump(TextWriter writer, string tab, bool isStatic)
		{
			int n = Count;
			for (int i = 0; i < n; ++i)
			{
				if (i > 0) writer.WriteLine();
				this[i].Dump(writer, tab, isStatic);
			}
		}
	}
}