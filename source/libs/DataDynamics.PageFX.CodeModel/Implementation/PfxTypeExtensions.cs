using System.Collections.Generic;
using System.Linq;

namespace DataDynamics.PageFX.CodeModel
{
    public static class PfxTypeExtensions
    {
    	private static readonly HashSet<string> ExcludedTypes = new HashSet<string>(
    		new[]
    			{
    				"avm",
    				"AvmErrors",
    				"RootAttribute",
    				"EmbedAttribute",
    			});

    	private static readonly string[] PfxNamespaces =
    		{
    			"adobe",
    			"Avm",
    			"__AS3__",
    			"flash",
    			"NUnit",
    			"PageFX",
    			"DataDynamics.PageFX"
    		};

        public static bool IsPfxSpecific(this IType type)
        {
            if (type == null) return false;

        	var ns = type.Namespace ?? "";
        	return PfxNamespaces.Any(ns.StartsWith) || ExcludedTypes.Contains(type.FullName);
        }
    }
}