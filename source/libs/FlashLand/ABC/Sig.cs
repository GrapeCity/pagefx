using System;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Avm;

namespace DataDynamics.PageFX.FlashLand.Abc
{
	/// <summary>
	/// Defines method signature.
	/// </summary>
	internal sealed class Sig
	{
		public static readonly Sig ptr_get = get(QName.PtrValue, AvmTypeCode.Object);
		public static readonly Sig ptr_set = set(QName.PtrValue, AvmTypeCode.Object);

		public object Name;
		public object ReturnType;
		public object[] Args;
		
		public AbcTraitKind Kind = AbcTraitKind.Method;
		public MethodSemantics Semantics;

		public IMethod Source;
		public bool IsInitilizer;

		public Sig()
		{
		}

		public Sig(object name, object returnType, params object[] args)
		{
			Name = name;
			ReturnType = returnType;
			Args = args;
		}

		public bool IsStatic
		{
			get { return (Semantics & MethodSemantics.Static) != 0; }
			set
			{
				if (value) Semantics |= MethodSemantics.Static;
				else Semantics &= ~MethodSemantics.Static;
			}
		}

		public bool IsVirtual
		{
			get { return (Semantics & MethodSemantics.Virtual) != 0; }
			set
			{
				if (value) Semantics |= MethodSemantics.Virtual;
				else Semantics &= ~MethodSemantics.Virtual;
			}
		}

		public bool IsOverride
		{
			get { return (Semantics & MethodSemantics.Override) != 0; }
			set
			{
				if (value) Semantics |= MethodSemantics.Override;
				else Semantics &= ~MethodSemantics.Override;
			}
		}

		public bool IsAbstract
		{
			get { return (Semantics & MethodSemantics.Abstract) != 0; }
			set
			{
				if (value) Semantics |= MethodSemantics.Abstract;
				else Semantics &= ~MethodSemantics.Abstract;
			}
		}

		public bool IsGetter
		{
			get { return Kind == AbcTraitKind.Getter; }
			set { Kind = value ? AbcTraitKind.Getter : AbcTraitKind.Method; }
		}

		public bool IsSetter
		{
			get { return Kind == AbcTraitKind.Setter; }
			set { Kind = value ? AbcTraitKind.Setter : AbcTraitKind.Method; }
		}

		public Sig @static()
		{
			IsStatic = true;
			return this;
		}

		public Sig @virtual()
		{
			IsVirtual = true;
			return this;
		}

		public Sig @override()
		{
			IsOverride = true;
			return this;
		}

		public Sig @override(bool value)
		{
			IsOverride = value;
			return this;
		}

		public static Sig @from(AbcMethod prototype)
		{
			if (prototype == null)
				throw new ArgumentNullException("prototype");

			var t = prototype.Trait;
			if (t == null)
				throw new InvalidOperationException();

			return new Sig(t.Name, prototype.ReturnType, prototype)
				{
					Kind = t.Kind,
					Semantics = t.MethodSemantics,
				};
		}

		public static Sig @get(object name, object returnType)
		{
			return new Sig(name, returnType) {IsGetter = true};
		}

		public static Sig @set(object name, object valueType)
		{
			return new Sig(name, AvmTypeCode.Void, valueType, "value") {IsSetter = true};
		}

		public static Sig @static(object name, object returnType, params object[] args)
		{
			return new Sig(name, returnType, args) {IsStatic = true};
		}

		public static Sig @this(object name, object returnType, params object[] args)
		{
			return new Sig(name, returnType, args);
		}

		public static Sig @virtual(object name, object returnType, params object[] args)
		{
			return new Sig(name, returnType, args) {IsVirtual = true};
		}

		public static Sig @override(object name, object returnType, params object[] args)
		{
			return new Sig(name, returnType, args) {IsOverride = true};
		}
	}
}