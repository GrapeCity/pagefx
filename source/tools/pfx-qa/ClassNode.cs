using System.Linq;
using DataDynamics.PageFX.Common;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX
{
    class ClassNode : ApiNode
    {
        public string FullName;
        public IType Type;

        public override NodeKind NodeKind
        {
            get { return NodeKind.Class; }
        }

        private static readonly string[] ExcludedTypes =
        {
            "System.EventArgs",
            "System.DBNull",
            "System.GC",
            "System.ComponentModel.PropertyChangedEventArgs",
            "System.Diagnostics.Debug",
            "System.Diagnostics.Debugger"
        };

        private static readonly string[] ExcludedNamespaces =
        {
            "System.Reflection",
            "System.Runtime.Serialization",
            "System.Runtime.CompilerServices",
            "System.Threading",
        };

        public static bool TypeFilter(IType type)
        {
            if (!type.IsVisible) return false;
            if (type.IsEnum) return false;
            if (type.TypeKind == TypeKind.Delegate) return false;
            if (type.IsInterface) return false;
            if (type.IsPfxSpecific()) return false;
            if (type == SystemTypes.Void) return false;
            if (type.IsAttributeType()) return false;
            if (type.IsExceptionType()) return false;
			if (ExcludedNamespaces.Contains(type.Namespace)) return false;
			if (ExcludedTypes.Contains(type.FullName)) return false;
            return true;
        }

        public override string Class
        {
            get { return "type"; }
        }

        public string ClassType
        {
            get 
            {
                switch (Type.TypeKind)
                {
                    case TypeKind.Struct:
                    case TypeKind.Primitive:
                        return "struct";
                    case TypeKind.Delegate:
                        return "delegate";
                    case TypeKind.Enum:
                        return "enum";
                    case TypeKind.Interface:
                        return "interface";
                }
                return "class";
            }
        }

        public override string Image
        {
            get
            {
                switch (Type.TypeKind)
                {                        
                    case TypeKind.Struct:
                    case TypeKind.Primitive:
                        return Images.Struct;
                    case TypeKind.Delegate:
                        return Images.Delegate;
                    case TypeKind.Enum:
                        return Images.Enum;
                    case TypeKind.Interface:
                        return Images.Interface;
                }
                return Images.Class;
            }
        }

        public void Load()
        {
            Name = Type.DisplayName;
            FullName = Type.FullName;
            
            foreach (var type in Type.Types)
            {
                if (!TypeFilter(type)) continue;
                var tn = new ClassNode { Type = type };
                Add(tn);
                tn.Load();
            }

            foreach (var method in Type.Methods)
            {
                if (!method.IsVisible) continue;
                var mn = new MethodNode(method);
                Add(mn);
                mn.CalcStat();
            }
        }

        public override string FormatUrl(int id)
        {
            return ClassType + "." + FullName.Replace('<', '{').Replace('>', '}');
        }
    }
}