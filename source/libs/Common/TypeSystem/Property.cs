using System;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Syntax;

namespace DataDynamics.PageFX.Common.TypeSystem
{
	/// <summary>
	///     Mutable property implementation.
	/// </summary>
	public sealed class Property : IProperty
	{
		[Flags]
		private enum PropertyFlags : byte
		{
			HasDefault = 0x01,
			IsSpecialName = 0x02,
			IsRuntimeSpecialName = 0x04
		}

		private ICustomAttributeCollection _customAttributes;
		private IMethod _getter;
		private IParameterCollection _parameters;
		private IMethod _setter;
		private PropertyFlags _flags;

		public IAssembly Assembly
		{
			get { return FromMethod(x => x.Assembly); }
		}

		public IModule Module
		{
			get { return FromMethod(x => x.Module); }
		}

		/// <summary>
		///     Gets the kind of this member.
		/// </summary>
		public MemberType MemberType
		{
			get { return MemberType.Property; }
		}

		public string Name { get; set; }

		public string FullName
		{
			get { return this.BuildFullName(); }
		}

		public string DisplayName
		{
			get { return Name; }
		}

		public IType DeclaringType
		{
			get { return FromMethod(x => x.DeclaringType); }
		}

		public IType Type
		{
			get { return ResolveType(); }
		}

		public Visibility Visibility
		{
			get
			{
				return new[] {Visibility.Private}
					.Concat(GetMethods().Select(x => x.Visibility))
					.Max();
			}
		}

		public bool HasDefault
		{
			get { return (_flags & PropertyFlags.HasDefault) != 0; }
			set
			{
				if (value) _flags |= PropertyFlags.HasDefault;
				else _flags &= ~PropertyFlags.HasDefault;
			}
		}

		/// <summary>
		///     Gets or sets the flag indicating whether the property is abtract.
		/// </summary>
		public bool IsAbstract
		{
			get { return IsFlag(x => x.IsAbstract); }
		}

		/// <summary>
		///     Gets or sets the flag indicating whether the property is virtual (overridable)
		/// </summary>
		public bool IsVirtual
		{
			get { return IsFlag(x => x.IsVirtual); }
		}

		/// <summary>
		///     Gets or sets the flag indicating whether the property is final (can not be overriden)
		/// </summary>
		public bool IsFinal
		{
			get { return IsFlag(x => x.IsFinal); }
		}

		/// <summary>
		///     Gets or sets the flag indicating whether property overrides return type.
		/// </summary>
		public bool IsNewSlot
		{
			get { return IsFlag(x => x.IsNewSlot); }
		}

		/// <summary>
		///     Gets or sets the flag indicating whether the property overrides implementation of base type.
		/// </summary>
		public bool IsOverride
		{
			get { return IsFlag(x => x.IsOverride); }
		}

		public bool IsStatic
		{
			get { return IsFlag(x => x.IsStatic); }
		}

		public bool IsSpecialName
		{
			get { return (_flags & PropertyFlags.IsSpecialName) != 0; }
			set
			{
				if (value) _flags |= PropertyFlags.IsSpecialName;
				else _flags &= ~PropertyFlags.IsSpecialName;
			}
		}

		public bool IsRuntimeSpecialName
		{
			get { return (_flags & PropertyFlags.IsRuntimeSpecialName) != 0; }
			set
			{
				if (value) _flags |= PropertyFlags.IsRuntimeSpecialName;
				else _flags &= ~PropertyFlags.IsRuntimeSpecialName;
			}
		}

		public IParameterCollection Parameters
		{
			get { return _parameters ?? (_parameters = new PropertyParameterCollection(this)); }
		}

		public IMethod Getter
		{
			get { return _getter ?? (_getter = ResolveGetter()); }
			set { _getter = value; }
		}

		public IMethod Setter
		{
			get { return _setter ?? (_setter = ResolveSetter()); }
			set { _setter = value; }
		}

		public object Value { get; set; }

		public int MetadataToken { get; set; }

		public ICustomAttributeCollection CustomAttributes
		{
			get { return _customAttributes ?? (_customAttributes = new CustomAttributeCollection()); }
			set { _customAttributes = value; }
		}

		public IEnumerable<ICodeNode> ChildNodes
		{
			get { return Enumerable.Empty<ICodeNode>(); }
		}

		public object Data { get; set; }

		public string Documentation { get; set; }

		public string ToString(string format, IFormatProvider formatProvider)
		{
			return SyntaxFormatter.Format(this, format, formatProvider);
		}

		private bool IsFlag(Func<IMethod, bool> get)
		{
			return GetMethods().Where(x => x != null).Any(get);
		}

		private IMethod ResolveGetter()
		{
			var declType = DeclaringType;
			return declType != null ? declType.Methods.FirstOrDefault(x => x.Association == this && !x.IsVoid()) : null;
		}

		private IMethod ResolveSetter()
		{
			var declType = DeclaringType;
			return declType != null ? declType.Methods.FirstOrDefault(x => x.Association == this && x.IsVoid()) : null;
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

		private IType ResolveType()
		{
			if (_getter != null)
			{
				return _getter.Type;
			}

			if (_setter != null)
			{
				return _setter.Parameters[_setter.Parameters.Count - 1].Type;
			}

			return null;
		}

		public override string ToString()
		{
			return ToString(null, null);
		}
	}
}