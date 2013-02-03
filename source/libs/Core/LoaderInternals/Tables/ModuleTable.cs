using System;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Syntax;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Core.Metadata;

namespace DataDynamics.PageFX.Core.LoaderInternals.Tables
{
	internal sealed class ModuleTable : MetadataTable<IModule>, IModuleCollection
	{
		public ModuleTable(AssemblyLoader loader)
			: base(loader)
		{
		}

		public override TableId Id
		{
			get { return TableId.Module; }
		}

		protected override IModule ParseRow(MetadataRow row, int index)
		{
			var name = row[Schema.Module.Name].String;
			var file = Loader.Files[name];
			if (file != null)
			{
				throw new NotImplementedException();
			}

			return new InternalModule(Loader, row, index);
		}

		public IModule this[string name]
		{
			get { return this.FirstOrDefault(x => x.Name == name); }
		}

		public void Add(IModule module)
		{
			throw new NotSupportedException("This collection is readonly.");
		}

		public string ToString(string format, IFormatProvider formatProvider)
		{
			return SyntaxFormatter.Format(this, format, formatProvider);
		}

		public IEnumerable<ICodeNode> ChildNodes
		{
			get { return this.Cast<ICodeNode>(); }
		}

		public object Data { get; set; }
	}
}
