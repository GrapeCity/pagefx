using System;
using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel
{
    public sealed class ThisReferenceExpression : Expression, IThisReferenceExpression, ITypeReferenceProvider
    {
    	public ThisReferenceExpression(IType type)
        {
            Type = type;
        }

    	public IType Type { get; set; }

    	public override IType ResultType
        {
            get { return Type; }
        }

    	public IEnumerable<IType> GetTypeReferences()
        {
            return new[] { Type };
        }

    	public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var e = obj as IThisReferenceExpression;
            if (e == null) return false;
            return e.Type == Type;
        }

        private static readonly int HashSalt = typeof(IThisReferenceExpression).GetHashCode();

        public override int GetHashCode()
        {
        	int h = HashSalt;
            if (Type != null)
                h ^= Type.GetHashCode();
            return h;
        }
    }

    public sealed class BaseReferenceExpression : Expression, IBaseReferenceExpression, ITypeReferenceProvider
    {
    	public BaseReferenceExpression(IType type)
        {
            Type = type;
        }

    	public IType Type { get; set; }

    	public override IType ResultType
        {
            get { return Type; }
        }

    	public IEnumerable<IType> GetTypeReferences()
        {
            return new[] { Type };
        }

    	public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var e = obj as IBaseReferenceExpression;
            if (e == null) return false;
            return e.Type == Type;
        }

        private static readonly int HashSalt = typeof(IBaseReferenceExpression).GetHashCode();

        public override int GetHashCode()
        {
        	int h = HashSalt;
            if (Type != null)
                h ^= Type.GetHashCode();
            return h;
        }
    }

    public sealed class ArgumentReferenceExpression : Expression, IArgumentReferenceExpression, ITypeReferenceProvider
    {
    	public ArgumentReferenceExpression(IParameter p)
        {
            Argument = p;
        }

    	public IParameter Argument { get; set; }

    	public override IType ResultType
        {
            get
            {
                var type = Argument.Type;
                return type.UnwrapRef();
            }
        }

    	public IEnumerable<IType> GetTypeReferences()
        {
            return new[] { Argument.Type };
        }

    	public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var e = obj as IArgumentReferenceExpression;
            if (e == null) return false;
            return e.Argument == Argument;
        }

        private static readonly int HashSalt = typeof(IArgumentReferenceExpression).GetHashCode();

        public override int GetHashCode()
        {
        	int h = HashSalt;
            if (Argument != null)
                h ^= Argument.GetHashCode();
            return h;
        }
    }

    public abstract class MemberReferenceExpression : Expression, IMemberReferenceExpression, ITypeReferenceProvider
    {
    	protected MemberReferenceExpression()
        {
        }

    	protected MemberReferenceExpression(IExpression target)
        {
            Target = target;
        }

    	public IExpression Target { get; set; }

    	public abstract ITypeMember Member
        {
            get;
            set;
        }

    	public override IType ResultType
        {
            get
            {
                var m = Member;
                if (m != null) return m.Type;
                return null;
            }
        }

    	public override IEnumerable<ICodeNode> ChildNodes
        {
            get { return new ICodeNode[] {Target}; }
        }

    	public virtual IEnumerable<IType> GetTypeReferences()
        {
            if (Member != null)
                return new[] { Member.Type };
            return null;
        }

    	public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var e = obj as IMemberReferenceExpression;
            if (e == null) return false;
            if (e.Member != Member) return false;
            if (!Equals(Target, e.Target)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            int h = 0;
            if (Target != null)
                h ^= Target.GetHashCode();
            var m = Member;
            if (m != null)
                h ^= m.GetHashCode();
            return h;
        }
    }

    public sealed class FieldReferenceExpression : MemberReferenceExpression, IFieldReferenceExpression
    {
    	public FieldReferenceExpression(IExpression target, IField field)
            : base(target)
        {
            Field = field;
        }

    	public IField Field { get; set; }

    	public override ITypeMember Member
        {
            get { return Field; }
            set
            {
                if (value != null)
                {
                    var f = value as IField;
                    if (f == null)
                        throw new InvalidOperationException();
                    Field = f;
                }
                else
                {
                    Field = null;
                }
            }
        }

    	private static readonly int HashSalt = typeof(FieldReferenceExpression).GetHashCode();

		public override int GetHashCode()
		{
			return base.GetHashCode() ^ HashSalt;
		}
    }

    public sealed class PropertyReferenceExpression : MemberReferenceExpression, IPropertyReferenceExpression
    {
    	public PropertyReferenceExpression(IExpression target, IProperty prop)
            : base(target)
        {
            Property = prop;
        }

    	public IProperty Property { get; set; }

    	public override ITypeMember Member
        {
            get { return Property; }
            set
            {
                if (value != null)
                {
                    var p = value as IProperty;
                    if (p == null)
                        throw new InvalidOperationException();
                    Property = p;
                }
                else
                {
                    Property = null;
                }
            }
        }

    	private static readonly int HashSalt = typeof(PropertyReferenceExpression).GetHashCode();

		public override int GetHashCode()
		{
			return base.GetHashCode() ^ HashSalt;
		}
    }

    public sealed class MethodReferenceExpression : MemberReferenceExpression, IMethodReferenceExpression
    {
    	public MethodReferenceExpression(IExpression target, IMethod method)
            : base(target)
        {
            Method = method;
        }

        public MethodReferenceExpression(IMethod method)
        {
            Method = method;
        }

    	public IMethod Method { get; set; }

    	public override ITypeMember Member
        {
            get { return Method; }
            set
            {
                if (value != null)
                {
                    var m = value as IMethod;
                    if (m == null)
                        throw new InvalidOperationException();
                    Method = m;
                }
                else
                {
                    Method = null;
                }
            }
        }

    	public override IEnumerable<IType> GetTypeReferences()
        {
            if (Method != null)
            {
                yield return Method.DeclaringType;
                yield return Method.Type;
                foreach (var p in Method.Parameters)
                {
                    yield return p.Type;
                }
            }
        }

    	private static readonly int HashSalt = typeof(MethodReferenceExpression).GetHashCode();

		public override int GetHashCode()
		{
			return base.GetHashCode() ^ HashSalt;
		}
    }

    public sealed class EventReferenceExpression : MemberReferenceExpression, IEventReferenceExpression
    {
    	public EventReferenceExpression(IExpression target, IEvent e)
            : base(target)
        {
            Event = e;
        }

    	public IEvent Event { get; set; }

    	public override ITypeMember Member
        {
            get { return Event; }
            set
            {
                if (value != null)
                {
                    var e = value as IEvent;
                    if (e == null)
                        throw new InvalidOperationException();
                    Event = e;
                }
                else
                {
                    Event = null;
                }
            }
        }

    	private static readonly int HashSalt = typeof(EventReferenceExpression).GetHashCode();

		public override int GetHashCode()
		{
			return base.GetHashCode() ^ HashSalt;
		}
    }

    public sealed class TypeReferenceExpression : Expression, ITypeReferenceExpression, ITypeReferenceProvider
    {
    	public TypeReferenceExpression(IType type)
        {
            Type = type;
        }

    	public IType Type { get; set; }

    	public override IType ResultType
        {
            get { return null; }
        }

    	public override bool Equals(object obj)
        {
            if (obj == this) return false;
            var e = obj as ITypeReferenceExpression;
            if (e == null) return false;
            return e.Type == Type;
        }

        private static readonly int HashSalt = typeof(ITypeReferenceExpression).GetHashCode();

        public override int GetHashCode()
        {
        	int h = HashSalt;
            if (Type != null)
                h ^= Type.GetHashCode();
            return h;
        }

    	public IEnumerable<IType> GetTypeReferences()
        {
			return Type != null ? new[] { Type } : new IType[0];
        }
    }

    public sealed class VariableReferenceExpression : Expression, IVariableReferenceExpression, ITypeReferenceProvider
    {
    	public VariableReferenceExpression(IVariable var)
        {
            Variable = var;
        }

    	public IVariable Variable { get; set; }

    	public override IType ResultType
        {
            get { return Variable != null ? Variable.Type : null; }
        }

    	public IEnumerable<IType> GetTypeReferences()
        {
            if (Variable != null)
                return new[] { Variable.Type };
            return null;
        }

    	public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var e = obj as IVariableReferenceExpression;
            if (e == null) return false;
            return e.Variable == Variable;
        }

        private static readonly int HashSalt = typeof(IVariableReferenceExpression).GetHashCode();

        public override int GetHashCode()
        {
        	int h = HashSalt;
            if (Variable != null)
                h ^= Variable.GetHashCode();
            return h;
        }
    }
}