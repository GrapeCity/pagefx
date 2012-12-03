using System;
using System.Linq;

namespace DataDynamics.PageFX.Common.TypeSystem
{
	public static class CollectionExtensions
	{
		public static T Find<T>(this IParameterizedMemberCollection<T> collection, string name, Func<T, bool> predicate)
			where T : IParameterizedMember
		{
			return collection.Find(name).FirstOrDefault(predicate);
		}

		public static T Find<T>(this IParameterizedMemberCollection<T> collection, string name, int argc)
			where T : IParameterizedMember
		{
			return collection.Find(name, m => m.Parameters.Count == argc);
		}

		public static T Find<T>(this IParameterizedMemberCollection<T> collection, string name, IType arg1)
			where T : IParameterizedMember
		{
			return collection.Find(name, m =>
			                             	{
			                             		var p = m.Parameters;
			                             		if (p.Count != 1) return false;
			                             		return Signature.TypeEquals(p[0].Type, arg1);
			                             	});
		}

		public static T Find<T>(this IParameterizedMemberCollection<T> collection, string name, IType arg1, IType arg2)
			where T : IParameterizedMember
		{
			return collection.Find(name, m =>
			                             	{
			                             		var p = m.Parameters;
			                             		if (p.Count != 2) return false;
			                             		return Signature.TypeEquals(p[0].Type, arg1)
			                             		       && Signature.TypeEquals(p[1].Type, arg2);
			                             	});
		}

		public static T Find<T>(this IParameterizedMemberCollection<T> collection, string name, IType arg1, IType arg2, IType arg3)
			where T : IParameterizedMember
		{
			return collection.Find(name, m =>
			                             	{
			                             		var p = m.Parameters;
			                             		if (p.Count != 3) return false;
			                             		return Signature.TypeEquals(p[0].Type, arg1)
			                             		       && Signature.TypeEquals(p[1].Type, arg2)
			                             		       && Signature.TypeEquals(p[2].Type, arg3);
			                             	});
		}

		public static T Find<T>(this IParameterizedMemberCollection<T> collection, string name, params IType[] types)
			where T : IParameterizedMember
		{
			return collection.Find(name, m => Signature.Equals(m.Parameters, types));
		}

		public static T Find<T>(this IParameterizedMemberCollection<T> collection, string name,
		                        Predicate<IParameterCollection> predicate)
			where T : IParameterizedMember
		{
			return collection.Find(name, m => predicate(m.Parameters));
		}

		public static T Find<T>(this IParameterizedMemberCollection<T> collection, string name, Func<IType, bool> arg1)
			where T : IParameterizedMember
		{
			return collection.Find(name, m =>
			                             	{
			                             		var p = m.Parameters;
			                             		if (p.Count != 1) return false;
			                             		return arg1(p[0].Type);
			                             	});
		}

		public static T Find<T>(this IParameterizedMemberCollection<T> collection, string name, Func<IType, bool> arg1, Func<IType, bool> arg2)
			where T : IParameterizedMember
		{
			return collection.Find(name, m =>
			                             	{
			                             		var p = m.Parameters;
			                             		if (p.Count != 2) return false;
			                             		return arg1(p[0].Type) && arg2(p[1].Type);
			                             	});
		}

		public static T Find<T>(this IParameterizedMemberCollection<T> collection, string name, Func<IType, bool> arg1, Func<IType, bool> arg2, Func<IType, bool> arg3)
			where T : IParameterizedMember
		{
			return collection.Find(name, m =>
			                             	{
			                             		var p = m.Parameters;
			                             		if (p.Count != 2) return false;
			                             		return arg1(p[0].Type) && arg2(p[1].Type) && arg3(p[2].Type);
			                             	});
		}
	}
}