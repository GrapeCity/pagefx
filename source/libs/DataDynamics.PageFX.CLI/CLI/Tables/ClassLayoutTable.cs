using System.Collections.Generic;
using DataDynamics.PageFX.CLI.Metadata;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.CLI.Tables
{
	internal sealed class ClassLayoutTable
	{
		private readonly Dictionary<int, ClassLayout> _layouts = new Dictionary<int, ClassLayout>();
		private int _lastIndex;

		public ClassLayout Find(MdbReader mdb, int typeIndex)
		{
			ClassLayout layout;
			if (_layouts.TryGetValue(typeIndex, out layout))
				return layout;

			int n = mdb.GetRowCount(MdbTableId.ClassLayout);
			for (; _lastIndex < n; _lastIndex++)
			{
				var row = mdb.GetRow(MdbTableId.ClassLayout, _lastIndex);

				var size = (int)row[MDB.ClassLayout.ClassSize].Value;
				var pack = (int)row[MDB.ClassLayout.PackingSize].Value;
				layout = new ClassLayout(size, pack);

				int index = row[MDB.ClassLayout.Parent].Index - 1;
				_layouts.Add(index, layout);

				if (index == typeIndex)
				{
					_lastIndex++;
					return layout;
				}
			}

			return null;
		}
	}
}
