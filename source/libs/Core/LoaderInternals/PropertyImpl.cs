using System;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Core.Metadata;

namespace DataDynamics.PageFX.Core.LoaderInternals
{
	internal sealed class PropertyImpl : MemberBase, IProperty
	{
		private readonly PropertyAttributes _flags;
		private IMethod _getter;
		private IParameterCollection _parameters;
		private IMethod _setter;

		public PropertyImpl(AssemblyLoader loader, MetadataRow row, int index)
			: base(loader, TableId.Property, index)
		{
			_flags = (PropertyAttributes) row[Schema.Property.Flags].Value;

			Name = row[Schema.Property.Name].String;

			Value = loader.Const[MetadataToken];
		}

		public string Documentation { get; set; }

		public MemberType MemberType
		{
			get { return MemberType.Property; }
		}

		public string FullName
		{
			get { return this.BuildFullName(); }
		}

		public IType DeclaringType
		{
			get { return FromMethod(x => x.DeclaringType); }
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
				return new[] { Visibility.Private }
					.Concat(GetMethods().Select(x => x.Visibility))
					.Max();
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

		public object Value { get; private set; }

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

		private bool IsFlag(Func<IMethod, bool> get)
		{
			return GetMethods().Where(x => x != null).Any(get);
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
			var rows = Loader.Metadata
			                  .LookupRows(TableId.MethodSemantics, Schema.MethodSemantics.Association, this.RowIndex(), true)
			                  .ToList();
			if (rows.Count == 0)
				return null;

			var row = rows.FirstOrDefault(
				x => (MethodSemanticsAttributes) x[Schema.MethodSemantics.Semantics].Value == sem);
			if (row == null)
				return null;

			var methodIndex = row[Schema.MethodSemantics.Method].Index - 1;
			return Loader.Methods[methodIndex];
		}

		private IEnumerable<IMethod> GetMethods()
		{
			yield return Getter;
			yield return Setter;
		}

		private T FromMethod<T>(Func<IMethod, T> eval)
		{
			var m = GetMethods().FirstOrDefault(x => x != null);
			return m != null ? eval(m) : default(T);
		}
	}
}