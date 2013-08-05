using System.IO;

namespace DataDynamics.PageFX.Flash.Abc
{
	public sealed class AbcScriptCollection : AbcAtomList<AbcScript>
	{
		private readonly AbcFile _abc;

		public AbcScriptCollection(AbcFile abc)
		{
			_abc = abc;
		}

		protected override void OnAdded(AbcScript item)
		{
			item.Abc = _abc;
		}

		protected override string DumpElementName
		{
			get { return "scripts"; }
		}
		
		public void Dump(TextWriter writer)
		{
			bool eol = false;
			foreach (var script in this)
			{
				if (eol) writer.WriteLine();
				script.Dump(writer);
				eol = true;
			}
		}
	}
}