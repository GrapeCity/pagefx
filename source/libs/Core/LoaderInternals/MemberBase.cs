using System;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Syntax;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Core.LoaderInternals.Collections;
using DataDynamics.PageFX.Core.Metadata;

namespace DataDynamics.PageFX.Core.LoaderInternals
{
	internal abstract class MemberBase : ICustomAttributeProvider, ICodeNode
	{
		protected readonly AssemblyLoader Loader;
		private ICustomAttributeCollection _customAttributes;

		protected MemberBase(AssemblyLoader loader, TableId table, int index)
		{
			Loader = loader;
			MetadataToken = SimpleIndex.MakeToken(table, index + 1);
		}

		public int MetadataToken { get; private set; }

		public string Name { get; protected set; }

		public virtual string DisplayName
		{
			get { return Name; }
		}

		public IAssembly Assembly
		{
			get { return Loader.Assembly; }
		}

		public IModule Module
		{
			get { return Assembly.MainModule; }
		}

		public ICustomAttributeCollection CustomAttributes
		{
			get { return _customAttributes ?? (_customAttributes = new CustomAttributes(Loader, this)); }
		}

		public IEnumerable<ICodeNode> ChildNodes
		{
			get { return Enumerable.Empty<ICodeNode>(); }
		}

		public object Data { get; set; }

		public string ToString(string format, IFormatProvider formatProvider)
		{
			return SyntaxFormatter.Format(this, format, formatProvider);
		}

		public override string ToString()
		{
			return ToString(null, null);
		}
	}
}