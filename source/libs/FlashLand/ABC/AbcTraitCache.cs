using System;
using System.Collections.Generic;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Core;

namespace DataDynamics.PageFX.FlashLand.Abc
{
    internal sealed class AbcTraitCache
    {
		private readonly Dictionary<string, AbcTrait> _cache = new Dictionary<string, AbcTrait>();

        public void Add(AbcTrait trait)
        {
	        if (trait == null)
				throw new ArgumentNullException("trait");

	        string key = KeyOf(trait);
			_cache.Add(key, trait);
        }

        public void AddRange(IEnumerable<AbcTrait> traits)
        {
            foreach (var trait in traits)
                Add(trait);
        }

        public AbcTrait Find(ITypeMember member)
        {
	        var key = KeyOf(member);
	        AbcTrait trait;
	        return _cache.TryGetValue(key, out trait) ? trait : null;
        }

		private static string KeyOf(AbcTrait trait)
		{
			return Prefix(trait.Kind) + MemberKey.BuildKey(trait.Name);
		}

		private static string KeyOf(ITypeMember member)
		{
			return Prefix(KindOf(member)) + MemberKey.BuildKey(member);
		}

	    private static AbcTraitKind KindOf(ITypeMember member)
		{
			var method = member as IMethod;
            if (method != null)
            {
	            return method.ResolveTraitKind();
            }

			var type = member as IType;
			if (type != null)
			{
				return AbcTraitKind.Class;
			}

			return AbcTraitKind.Slot;
		}

	    private static string Prefix(AbcTraitKind kind)
	    {
		    switch (kind)
		    {
			    case AbcTraitKind.Slot:
			    case AbcTraitKind.Const:
				    return "f:";
			    case AbcTraitKind.Method:
			    case AbcTraitKind.Function:
				    return "m:";
			    case AbcTraitKind.Getter:
				    return "g:";
			    case AbcTraitKind.Setter:
				    return "s:";
			    case AbcTraitKind.Class:
				    return "t:";
			    default:
				    throw new ArgumentOutOfRangeException("kind");
		    }
	    }
    }
}