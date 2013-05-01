using System;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Core.LoaderInternals
{
	internal sealed class Context
	{
		public Context(IType type) : this(type, false){}

		public Context(IType type, bool generic)
		{
			if (type == null)
				throw new ArgumentNullException("type");

			Type = type;
			Method = type.DeclaringMethod;
			IsGeneric = generic;
		}

		public Context(IMethod method) : this(method, false) {}

		public Context(IMethod method, bool generic)
		{
			if (method == null)
				throw new ArgumentNullException("method");
			if (method.DeclaringType == null)
				throw new ArgumentException("method");

			Type = method.DeclaringType;
			Method = method;
			IsGeneric = generic;
		}

		public Context(IType type, IMethod method)
		{
			if (type == null && method == null)
				throw new ArgumentException("type or method is null");
			
			Type = type;
			Method = method;
		}

		public Context(Context context, bool generic)
		{
			if (context == null)
				throw new ArgumentNullException("context");

			Type = context.Type;
			Method = context.Method;
			IsGeneric = generic;
		}

		public IType Type { get; private set; }

		public IMethod Method { get; private set; }

		public bool IsGeneric { get; private set; }
	}
}