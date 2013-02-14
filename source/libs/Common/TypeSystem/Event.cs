using System;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Syntax;

namespace DataDynamics.PageFX.Common.TypeSystem
{
	/// <summary>
	///     Mutable event implementation.
	/// </summary>
	public sealed class Event : IEvent
	{
		[Flags]
		private enum EventFlags : byte
		{
			IsSpecialName = 0x01,
			IsRuntimeSpecialName = 0x02
		}

		private ICustomAttributeCollection _customAttributes;
		private EventFlags _flags;

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
			get { return MemberType.Event; }
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
			get
			{
				var m = GetMethods().FirstOrDefault(x => x != null && x.Parameters.Count > 0);
				return m == null ? null : m.Parameters[0].Type;
			}
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

		public bool IsStatic
		{
			get { return GetMethods().Any(x => x.IsStatic); }
		}

		public bool IsSpecialName
		{
			get { return (_flags & EventFlags.IsSpecialName) != 0; }
			set
			{
				if (value) _flags |= EventFlags.IsSpecialName;
				else _flags &= ~EventFlags.IsSpecialName;
			}
		}

		public bool IsRuntimeSpecialName
		{
			get { return (_flags & EventFlags.IsRuntimeSpecialName) != 0; }
			set
			{
				if (value) _flags |= EventFlags.IsRuntimeSpecialName;
				else _flags &= ~EventFlags.IsRuntimeSpecialName;
			}
		}

		public IMethod Adder { get; set; }

		public IMethod Remover { get; set; }

		public IMethod Raiser { get; set; }

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

		private IEnumerable<IMethod> GetMethods()
		{
			yield return Adder;
			yield return Remover;
			yield return Raiser;
		}

		private T FromMethod<T>(Func<IMethod, T> eval)
		{
			var m = GetMethods().FirstOrDefault(x => x != null);
			return m != null ? eval(m) : default(T);
		}

		public override string ToString()
		{
			return ToString(null, null);
		}
	}
}