using DataDynamics.PageFX.CLI.Metadata;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.Tables
{
	internal sealed class ClassLayoutTable
	{
		public ClassLayout Find(MdbReader mdb, int typeIndex)
		{
			var row = mdb.LookupRowByIndex(MdbTableId.ClassLayout, MDB.ClassLayout.Parent, typeIndex);
			if (row == null)
				return null;

			var size = (int)row[MDB.ClassLayout.ClassSize].Value;
			var pack = (int)row[MDB.ClassLayout.PackingSize].Value;
			return new ClassLayout(size, pack);
		}
	}
}
