using System;

namespace DataDynamics.PageFX.CodeModel
{
    public static class TypeBuilder
    {
        public static IType BuildAttributeType(string ns, string name, params object[] args)
        {
            var baseType = SystemTypes.Attribute;
            if (baseType == null)
                throw new InvalidOperationException();

            var type = new UserDefinedType(TypeKind.Class);
            type.Namespace = ns;
            type.Name = name;
            type.Visibility = Visibility.Public;

            type.BaseType = baseType;

            var ctor = new Method();
            ctor.Name = ".ctor";
            ctor.IsSpecialName = true;
            ctor.Type = SystemTypes.Void;
            ctor.Visibility = Visibility.Public;
            type.Members.Add(ctor);

            int n;
            if (args != null)
            {
                n = args.Length;
                for (int i = 0; i < n; i += 2)
                {
                    var p = new Parameter();
                    p.Type = args[i] as IType;
                    p.Name = args[i + 1] as string;
                    ctor.Parameters.Add(p);
                }
            }

            n = ctor.Parameters.Count;
            for (int i = 0; i < n; ++i)
            {
                var p = ctor.Parameters[i];
                p.Index = i + 1;
                var f = new Field();
                f.Name = NameHelper.Cap(p.Name);
                f.Visibility = Visibility.Public;
                f.Type = p.Type;
                type.Members.Add(f);
            }

            var body = new MethodBody(ctor);
            body.MaxStackSize = 2;
            ctor.Body = body;

            var code = new MethodCode(body);

            var baseCtor = baseType.Methods[".ctor", 0];

            code.Call(baseCtor);

            for (int i = 0; i < n; ++i)
            {
                code.SetField(type.Fields[i], code.ArgRef(i));
            }

            return type;
        }
    }
}