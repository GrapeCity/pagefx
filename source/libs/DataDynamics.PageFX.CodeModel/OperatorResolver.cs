using System.Collections.Generic;
using DataDynamics.PageFX.CodeModel.TypeSystem;

namespace DataDynamics.PageFX.CodeModel
{
	public sealed class OperatorResolver
	{
		private readonly Dictionary<string, IMethod> _cache = new Dictionary<string, IMethod>();

		public IMethod Find(BinaryOperator op, IType left, IType right)
		{
			string key = ((int)op).ToString() + (int)left.SystemType().Code + (int)right.SystemType().Code;
			IMethod m;
			if (!_cache.TryGetValue(key, out m))
			{
				m = op.FindMethod(left, right);
				if (m != null)
					_cache.Add(key, m);
			}
			return m;
		}

		public IMethod Find(UnaryOperator op, IType type)
		{
			string key = ((int)op).ToString() + (int)type.SystemType().Code;
			IMethod m;
			if (!_cache.TryGetValue(key, out m))
			{
				m = op.FindMethod(type);
				if (m != null)
					_cache.Add(key, m);
			}
			return m;
		}

		public IMethod Find(IType type, bool isTrue)
		{
			string key = (isTrue ? OpNames.True : OpNames.False) + (int)type.SystemType().Code;
			IMethod m;
			if (!_cache.TryGetValue(key, out m))
			{
				m = type.FindBooleanOperator(isTrue);
				if (m != null)
					_cache.Add(key, m);
			}
			return m;
		}
	}
}
