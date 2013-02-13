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
	internal sealed class PropertyImpl : IProperty
	{
		private readonly AssemblyLoader _loader;
		private ICustomAttributeCollection _customAttributes;
		private readonly PropertyAttributes _flags;
		private IParameterCollection _parameters;
		private IMethod _getter;
		private IMethod _setter;

		public PropertyImpl(AssemblyLoader loader, MetadataRow row, int index)
		{
			_loader = loader;

			var token = SimpleIndex.MakeToken(TableId.Property, index + 1);
			MetadataToken = token;

			_flags = (PropertyAttributes)row[Schema.Property.Flags].Value;

			Name = row[Schema.Property.Name].String;

			Value = loader.Const[token];
		}

		public int MetadataToken { get; private set; }

		public ICustomAttributeCollection CustomAttributes
		{
			get { return _customAttributes ?? (_customAttributes = new CustomAttributes(_loader, this)); }
		}

		public IEnumerable<ICodeNode> ChildNodes
		{
			get { return Enumerable.Empty<ICodeNode>(); }
		}

		public object Data { get; set; }

		public string Documentation { get; set; }

		public IAssembly Assembly
		{
			get { return _loader.Assembly; }
		}

		public IModule Module
		{
			get { return _loader.MainModule; }
		}

		public MemberType MemberType
		{
			get { return MemberType.Property; }
		}

		public string Name { get; private set; }

		public string FullName
		{
			get
			{
				var declType = DeclaringType;
				if (declType != null)
					return declType.FullName + "." + Name;
				return Name;
			}
		}

		public string DisplayName
		{
			get { return Name; }
		}

		public IType DeclaringType
		{
			get
			{
				var m = new[] {Getter, Setter}.FirstOrDefault(x => x != null);
				return m != null ? m.DeclaringType : null;
			}
		}

		public IType Type
		{
			get 
			{
				var getter = Getter;
				if (getter != null)
				{
					return getter.Type;
				}

				var setter = Setter;
				if (setter != null)
				{
					return setter.Parameters[setter.Parameters.Count - 1].Type;
				}

				return null;
			}
		}

		public Visibility Visibility
		{
			get
			{
				var getter = Getter;
				var setter = Setter;
				
				if (getter != null)
				{
					if (setter != null)
					{
						var gv = getter.Visibility;
						var sv = setter.Visibility;
						return gv > sv ? gv : sv;
					}

					return getter.Visibility;
				}

				return setter != null ? setter.Visibility : Visibility.Private;
			}
		}

		public bool IsSpecialName
		{
			get { return (_flags & PropertyAttributes.SpecialName) != 0; }
		}

		public bool IsRuntimeSpecialName
		{
			get { return (_flags & PropertyAttributes.RTSpecialName) != 0; }
		}

		private bool IsFlag(Func<IMethod, bool> get)
		{
			return new[] { Getter, Setter }
				.Where(x => x != null)
				.Any(get);
		}

		public bool IsStatic
		{
			get { return IsFlag(x => x.IsStatic); }
		}

		public bool IsAbstract
		{
			get { return IsFlag(x => x.IsAbstract); }
		}

		public bool IsVirtual
		{
			get { return IsFlag(x => x.IsVirtual); }
		}

		public bool IsFinal
		{
			get { return IsFlag(x => x.IsFinal); }
		}

		public bool IsNewSlot
		{
			get { return IsFlag(x => x.IsNewSlot); }
		}

		public bool IsOverride
		{
			get { return IsFlag(x => x.IsOverride); }
		}

		public object Value { get; set; }

		public IParameterCollection Parameters
		{
			get { return _parameters ?? (_parameters = new PropertyParameterCollection(this)); }
		}

		public bool HasDefault
		{
			get { return (_flags & PropertyAttributes.HasDefault) != 0; }
		}

		public IMethod Getter
		{
			get { return _getter ?? (_getter = ResolveGetter()); }
		}

		public IMethod Setter
		{
			get { return _setter ?? (_setter = ResolveSetter()); }
		}

		private IMethod ResolveGetter()
		{
			return ResolveMethod(MethodSemanticsAttributes.Getter);
		}

		private IMethod ResolveSetter()
		{
			return ResolveMethod(MethodSemanticsAttributes.Setter);
		}

		private IMethod ResolveMethod(MethodSemanticsAttributes sem)
		{
			var rows = _loader.Metadata
			                  .LookupRows(TableId.MethodSemantics, Schema.MethodSemantics.Association, this.RowIndex(), true)
			                  .ToList();
			if (rows.Count == 0)
				return null;

			var row = rows.FirstOrDefault(
				x => (MethodSemanticsAttributes)x[Schema.MethodSemantics.Semantics].Value == sem);
			if (row == null)
				return null;

			var methodIndex = row[Schema.MethodSemantics.Method].Index - 1;
			return _loader.Methods[methodIndex];
		}

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
