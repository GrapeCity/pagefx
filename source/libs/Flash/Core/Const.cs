namespace DataDynamics.PageFX.Flash.Core
{
    internal static class Const
    {
        public static class Locales
        {
            public const string en_US = "en_US";
            public const string ja_JP = "ja_JP";
        }

        public static class Namespaces
        {
            public const string PFX = "pfx";
        }

        public static class AvmGlobalTypes
        {
            public const string Object = "Object";
            public const string Error = "Error";
        }

        public static class Object
        {
            public const string MethodGetType = "GetType";
            public const string MethodGetHashCode = "GetHashCode";
            public const string MethodToString = "ToString";
            public const string MethodGetTypeId = "GetTypeId";
            public const string MethodEquals = "Equals";
        }

        public static class Boxing
        {
            public const string Value = "m_value";
            public const string MethodBox = "__box__";
            public const string MethodUnbox = "__unbox__";
        }

        public static class ConstructorInfo
        {
            public const string CreateFunction = "m_create";
        }

        public static class Array
        {
            public const string Value = "m_value";
            public const string Rank = "m_rank";
            public const string Lengths = "m_lengths";
            public const string Dims = "m_dims";
        }

        public static class Int64
        {
            public const string LoField = "m_lo";
            public const string HiField = "m_hi";
        }

        public static class Enum
        {
            public const string GetValue = "GetValue";
            public const string SetValue = "SetValue";
        }

        public static class Delegate
        {
            public const string Target = "m_target";
            public const string Function = "m_function";
            public const string Prev = "m_prev";
            public const string AddEventListener = "addEventListener";
            public const string RemoveEventListener = "removeEventListener";
            public const string AddEventListeners = "AddEventListeners";
            public const string RemoveEventListeners = "RemoveEventListeners";
        }

        public static class MethodBase
        {
            public const string Name = "m_name";
            public const string Function = "m_function";
            public const string Attributes = "m_attributes";
            public const string Parameters = "m_parameters";
        }

        public static class ParameterInfo
        {
            public const string MemberImpl = "MemberImpl";
            public const string ClassImpl = "m_class_impl";
            public const string NameImpl = "NameImpl";
        }

        public static class Nullable
        {
            public const string HasValue = "has_value";
        }

        public static class MemberInfo
        {
            public const string CustomAttrsInit = "m_customAttrsInit";
        }

        public static class Type
        {
            public const string Namespace = "ns";
            public const string NamespaceObject = "nsobj";
            public const string Name = "name";
            public const string BaseType = "baseType";
            public const string Kind = "kind";
            public const string Rank = "rank"; //only for arrays
            public const string ElementType = "elemType";
            public const string EnumInfoInit = "m_enumInfoInit";
            public const string MyFields = "myfields";
            public const string Interfaces = "ifaces";
            public const string UnderlyingType = "utype";
            public const string BoxFunction = "m_box";
            public const string UnboxFunction = "m_unbox";
            public const string CopyFunction = "m_copy";
            public const string CreateFunction = "m_create";
            public const string MethodsInit = "m_methodsInit";
            public const string ConstructorsInit = "m_constructorsInit";
            public const string PropertiesInit = "m_propertiesInit";
            public const string MyFieldsInit = "m_myfieldsInit";
        }

        public static class EnumInfo
        {
            public const string Names = "names";
            public const string Values = "values";
            public const string Flags = "flags";
        }

        public static class String
        {
            public const string Value = "m_value";
        }

        public const string InitTypePrefix = "__inittype__";

        public const string GetTypeId = "GetTypeId";

        public const string Instance = "instance";
    }

    internal static class AS3
    {
        public const string NS2006 = "http://adobe.com/AS3/2006/builtin";
        public const string NS2008_FP10 = "http://www.adobe.com/2008/actionscript/Flash10/";

        public static class Vector
        {
            public const string Namespace = "__AS3__.vec";
            public const string Name = "Vector";
            public const string FullName = Namespace + "." + Name;   
        }
    }

    internal static class MX
    {
        public const string NamespaceInternal2006 = "http://www.adobe.com/2006/flex/mx/internal";

    	public const string IFlexModuleFactory = "mx.core.IFlexModuleFactory";
    	public const string IFlexModule = "mx.core.IFlexModule";
    	public const string ChildManager = "mx.managers.systemClasses.ChildManager";
    	public const string StyleManager = "mx.styles.StyleManager";
    	public const string IStyleManager2 = "mx.styles.IStyleManager2";
    	public const string StyleManagerImpl = "mx.styles.StyleManagerImpl";

    	public const string FlexEvent = "mx.events.FlexEvent";
    }
}