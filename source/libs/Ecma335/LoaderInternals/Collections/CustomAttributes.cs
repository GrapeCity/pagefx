using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Collections;
using DataDynamics.PageFX.Common.Syntax;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Ecma335.Metadata;

namespace DataDynamics.PageFX.Ecma335.LoaderInternals.Collections
{
	internal sealed class CustomAttributes : ICustomAttributeCollection
	{
		private readonly AssemblyLoader _loader;
		private readonly ICustomAttributeProvider _owner;
		private IReadOnlyList<ICustomAttribute> _list;

		public CustomAttributes(AssemblyLoader loader, ICustomAttributeProvider owner)
		{
			_loader = loader;
			_owner = owner;
		}

		public IEnumerator<ICustomAttribute> GetEnumerator()
		{
			return List.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public int Count
		{
			get { return List.Count; }
		}

		public ICustomAttribute this[int index]
		{
			get { return List[index]; }
		}

		public string ToString(string format, IFormatProvider formatProvider)
		{
			return SyntaxFormatter.Format(this, format, formatProvider);
		}

		public IEnumerable<ICodeNode> ChildNodes
		{
			get { return this.Cast<ICodeNode>(); }
		}

		public object Tag { get; set; }

		public ICustomAttribute[] this[IType type]
		{
			get { return this.Where(x => ReferenceEquals(x.Type, type)).ToArray(); }
		}

		public ICustomAttribute[] this[string typeFullName]
		{
			get { return this.Where(x => x.Type.FullName == typeFullName).ToArray(); }
		}

		public void Add(ICustomAttribute attribute)
		{
			throw new NotSupportedException();
		}

		private IReadOnlyList<ICustomAttribute> List
		{
			get { return _list ?? (_list = Populate().Memoize()); }
		}

		private IEnumerable<ICustomAttribute> Populate()
		{
			var target = (SimpleIndex)_owner.MetadataToken;
			var rows = _loader.Metadata.LookupRows(TableId.CustomAttribute, Schema.CustomAttribute.Parent, target, false);

			var context = ResolveAttributeContext(_owner);

			return (from row in rows
			        let ctorIndex = row[Schema.CustomAttribute.Type].Value
			        let ctor = GetCustomAttributeConstructor(ctorIndex, context)
			        where ctor != null
					select CreateAttribute(row, ctor, ctor.DeclaringType) into attr
			        where ReviewAttribute(attr)
			        select attr).Cast<ICustomAttribute>();
		}

		private CustomAttribute CreateAttribute(MetadataRow row, IMethod ctor, IType type)
		{
			var attribute = new CustomAttribute
				{
					Constructor = ctor,
					Type = type,
					Owner = _owner
				};

			var args = row[Schema.CustomAttribute.Value].Blob;
			if (args != null && args.Length > 0) //non null
			{
				attribute.Arguments = new ArgumentList(attribute, args, _loader);
			}

			return attribute;
		}

		private static Context ResolveAttributeContext(ICustomAttributeProvider provider)
		{
			var type = provider as IType;
			if (type != null)
			{
				return new Context(type);
			}

			var method = provider as IMethod;
			if (method != null)
			{
				return new Context(method);
			}

			var member = provider as ITypeMember;
			if (member != null)
			{
				return new Context(member.DeclaringType);
			}

			return null;
		}

		private IMethod GetCustomAttributeConstructor(SimpleIndex i, Context context)
		{
			try
			{
				switch (i.Table)
				{
					case TableId.MethodDef:
						return _loader.Methods[i.Index - 1];

					case TableId.MemberRef:
						return _loader.GetMemberRef(i.Index - 1, context) as IMethod;

					default:
						throw new BadMetadataException(string.Format("Invalid custom attribute type index {0}", i));
				}
			}
			catch (Exception)
			{
				return null;
			}
		}

		private static bool ReviewAttribute(ICustomAttribute attr)
		{
			var type = attr.Owner as IType;
			if (type != null)
			{
				if (attr.Type.FullName == "System.Runtime.CompilerServices.CompilerGeneratedAttribute")
				{
					type.IsCompilerGenerated = true;
				}
				return true;
			}

			var param = attr.Owner as IParameter;
			if (param != null)
			{
				if (attr.Type.FullName == "System.ParamArrayAttribute")
				{
					param.HasParams = true;
					return false;
				}
			}

			return true;
		}
	}
}
