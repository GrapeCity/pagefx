using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.CodeModel.TypeSystem;

namespace DataDynamics.PageFX.FLI
{
    internal static class Attrs
    {
        public const string PfxNamespace = "PageFX";
        const string NsPrefix = PfxNamespace + ".";

        //pfc attrs
        public const string ABC = NsPrefix + "ABCAttribute";
        public const string QName = NsPrefix + "QNameAttribute";
        public const string GlobalFunctions = NsPrefix + "GlobalFunctionsAttribute";
        public const string Event = NsPrefix + "EventAttribute";
        public const string Vector = NsPrefix + "VectorAttribute";
        public const string GenericVector = NsPrefix + "GenericVectorAttribute";

	    public const string AS3 = NsPrefix + "AS3Attribute";
	    public const string InlineFunction = NsPrefix + "InlineFunctionAttribute";
	    public const string InlineProperty = NsPrefix + "InlinePropertyAttribute";
	    public const string InlineOperator = NsPrefix + "InlineOperatorAttribute";

        public const string FP = NsPrefix + "FPAttribute";
        public const string FP9 = NsPrefix + "FP9Attribute";
        public const string FP10 = NsPrefix + "FP10Attribute";
        public const string AIR = NsPrefix + "AIRAttribute";
        public const string AVM = NsPrefix + "AVMAttribute";

        //used to optimize linker
        public const string AbcInstance = NsPrefix + "AbcInstanceAttribute";
        public const string AbcScript = NsPrefix + "AbcScriptAttribute";
        public const string AbcInstanceTrait = NsPrefix + "AbcInstanceTraitAttribute";
        public const string AbcClassTrait = NsPrefix + "AbcClassTraitAttribute";
        public const string AbcScriptTrait = NsPrefix + "AbcScriptTraitAttribute";
        public const string SwcAbcFile = NsPrefix + "SwcAbcFileAttribute";

        //user attrs
        public const string RootSprite = "RootAttribute";
        public const string Embed = "EmbedAttribute";
        private const string NoRootNamespace = "NoRootNamespaceAttribute";
		private const string NoRootNamespace2 = "IgnoreRootNamespaceAttribute";
        private const string Expose = "ExposeAttribute";

        public const string DebuggerDisplay = "System.Diagnostics.DebuggerDisplayAttribute";

        public static bool HasRootNamespace(this IType type)
        {
            if (type == null) return false;

            var st = type.SystemType();
            if (st != null) return false;

            switch (type.FullName)
            {
                case "System.Type":
                case "System.Array":
                case "System.Reflection.MemberInfo":
                case "System.Reflection.MethodBase":
                case "System.Reflection.PropertyInfo":
                    return false;
            }

            return !(type.HasAttribute(NoRootNamespace) || type.HasAttribute(NoRootNamespace2));
        }

        public static bool IsExposed(this ITypeMember member)
        {
            if (member == null) return false;

            var type = member as IType;
            if (type != null)
            {
                if (GenericType.HasGenericParams(type))
                    return false;

                if (type.IsTestFixture())
                    return true;
            }

            var method = member as IMethod;
            if (method != null)
            {
                if (method.DeclaringType.IsTestFixture())
                {
                    if (method.IsConstructor)
                        return true;
                    if (NUnitExtensions.IsNUnitMethod(method))
                        return true;
                }

                if (method.Association != null
                    && method.Association.IsExposed())
                    return true;
            }

            return member.HasAttribute(Expose);
        }
    }
}