using System;

#region pfc attrs
namespace PageFX
{
    #region class ABCAttribute
    /// <summary>
    /// Defines that member is defined within ABC.
    /// </summary>
    public sealed class ABCAttribute : Attribute
    {
    }
    #endregion

    #region class QNameAttribute
    /// <summary>
    /// Used to explicitly define full member name
    /// </summary>
    public sealed class QNameAttribute : Attribute
    {
        public string Name;
        public string Namespace; //default is global package
        public string NamespaceKind;

        public QNameAttribute(string name)
        {
            Name = name;
        }

        public QNameAttribute(string name, string ns, string nskind)
        {
            Name = name;
            Namespace = ns;
            NamespaceKind = nskind;
        }
    }
    #endregion

    #region class GlobalFunctionsAttribute
    /// <summary>
    /// Used to mark class that contains global ABC functions.
    /// </summary>
    public sealed class GlobalFunctionsAttribute : Attribute
    {
    }
    #endregion

    #region class EventAttribute
    /// <summary>
    /// Used to mark named flash event.
    /// </summary>
    public sealed class EventAttribute : Attribute
    {
        public string Name;

        public EventAttribute(string name)
        {
            Name = name;
        }
    }
    #endregion

    #region class VectorAttribute
    public sealed class VectorAttribute : Attribute
    {
        public string Parameter;

        public VectorAttribute(string param)
        {
            Parameter = param;
        }
    }
    #endregion

    #region class GenericVectorAttribute
    public sealed class GenericVectorAttribute : Attribute
    {
    }
    #endregion

    #region class AIRAttribute
    /// <summary>
    /// Used to mark AIR API element.
    /// </summary>
    public sealed class AIRAttribute : Attribute
    {
        public string Version;

        public AIRAttribute()
        {
            Version = "1.0";
        }

        public AIRAttribute(string version)
        {
            Version = version;
        }
    }
    #endregion

    #region class FPAttribute
    /// <summary>
    /// Used to mark Flash API member.
    /// </summary>
    public sealed class FPAttribute : Attribute
    {
        public string Version;

        public FPAttribute(string version)
        {
            Version = version;
        }
    }
    #endregion

    #region class FP9Attribute
    /// <summary>
    /// Used to mark FP9 API member.
    /// </summary>
    public sealed class FP9Attribute : Attribute
    {
    }
    #endregion

    #region class FP10Attribute
    /// <summary>
    /// Used to mark FP10 API member.
    /// </summary>
    public sealed class FP10Attribute : Attribute
    {
    }
    #endregion

    #region class AVMAttribute
    /// <summary>
    /// Used to mark AVM API member.
    /// </summary>
    public sealed class AVMAttribute : Attribute
    {
        public string Version;

        public AVMAttribute()
        {
            Version = "1.0";
        }

        public AVMAttribute(string version)
        {
            Version = version;
        }
    }
    #endregion

    public sealed class AbcInstanceAttribute : Attribute
    {
        public int Index;

        public AbcInstanceAttribute(int index)
        {
            Index = index;
        }
    }

    public sealed class AbcScriptAttribute : Attribute
    {
        public int Index;

        public AbcScriptAttribute(int index)
        {
            Index = index;
        }
    }

    public sealed class AbcInstanceTraitAttribute : Attribute
    {
        public int Index;

        public AbcInstanceTraitAttribute(int index)
        {
            Index = index;
        }
    }

    public sealed class AbcClassTraitAttribute : Attribute
    {
        public int Index;

        public AbcClassTraitAttribute(int index)
        {
            Index = index;
        }
    }

    public sealed class AbcScriptTraitAttribute : Attribute
    {
        public int Index;

        public AbcScriptTraitAttribute(int index)
        {
            Index = index;
        }
    }

    public sealed class SwcAbcFileAttribute : Attribute
    {
        public int Library;
        public int File;

        public SwcAbcFileAttribute(int lib, int file)
        {
            Library = lib;
            File = file;
        }

        public SwcAbcFileAttribute(int file)
        {
            File = file;
        }
    }
}
#endregion

#region user attrs
/// <summary>
/// Used to indicate root sprite.
/// </summary>
public sealed class RootAttribute : Attribute
{
}

/// <summary>
/// Used to embed asset.
/// </summary>
public sealed class EmbedAttribute : Attribute
{
    public string Source;
    public string MimeType;
    public string Symbol;

    //TODO: Add scale grid params

    public EmbedAttribute(string source)
    {
        Source = source;
        //TODO: auto detect mime type
    }

    public EmbedAttribute(string source, string mimeType)
    {
        Source = source;
        MimeType = mimeType;
    }
}

[AttributeUsage(AttributeTargets.Class
    | AttributeTargets.Interface
    | AttributeTargets.Struct
    | AttributeTargets.Delegate
    | AttributeTargets.Enum)]
public sealed class NoRootNamespaceAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Class 
    | AttributeTargets.Interface 
    | AttributeTargets.Struct
    | AttributeTargets.Delegate 
    | AttributeTargets.Enum
    | AttributeTargets.Constructor 
    | AttributeTargets.Event 
    | AttributeTargets.Property
    | AttributeTargets.Field
    | AttributeTargets.Method)]
public sealed class ExposeAttribute : Attribute
{
}
#endregion