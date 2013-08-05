using System;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Syntax;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.FlashLand.TypeSystem
{
	internal sealed class FlashEvent : IEvent
	{
		private ICustomAttributeCollection _customAttributes;

		public FlashEvent(string name, IType declType, IType type)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");
			if (declType == null)
				throw new ArgumentNullException("declType");
			if (type == null)
				throw new ArgumentNullException("type");

			Name = name;
			DeclaringType = declType;
			Type = type;
		}

		public int MetadataToken
		{
			get { return 0; }
		}

		public ICustomAttributeCollection CustomAttributes
		{
			get { return _customAttributes ?? (_customAttributes = new CustomAttributeCollection()); }
		}

		public string ToString(string format, IFormatProvider formatProvider)
		{
			return SyntaxFormatter.Format(this, format, formatProvider);
		}

		public IEnumerable<ICodeNode> ChildNodes
		{
			get { return Enumerable.Empty<ICodeNode>(); }
		}

		public object Data { get; set; }

		public string Documentation { get; set; }

		public IAssembly Assembly
		{
			get { return DeclaringType.Assembly; }
		}

		public IModule Module
		{
			get { return Assembly.MainModule; }
		}

		public MemberType MemberType
		{
			get { return MemberType.Event; }
		}

		public string Name { get; private set; }

		public string FullName
		{
			get { return this.BuildFullName(); }
		}

		public string DisplayName
		{
			get { return Name; }
		}

		public IType DeclaringType { get; private set; }
		public IType Type { get; private set; }
		public Visibility Visibility { get; set; }
		public bool IsStatic { get; set; }
		public bool IsSpecialName { get; set; }
		public bool IsRuntimeSpecialName { get; set; }
		public IMethod Adder { get; set; }
		public IMethod Remover { get; set; }
		public IMethod Raiser { get; set; }

		public override string ToString()
		{
			return ToString(null, null);
		}
	}
}
