using System;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Common.Services
{
    public static class TypeBuilder
    {
        public static IType BuildAttributeType(string ns, string name, params object[] args)
        {
            var baseType = SystemTypes.Attribute;
            if (baseType == null)
                throw new InvalidOperationException();

        	var type = new UserDefinedType(TypeKind.Class)
        	           	{
        	           		Namespace = ns,
        	           		Name = name,
        	           		Visibility = Visibility.Public,
        	           		BaseType = baseType
        	           	};


        	var ctor = new Method
        	           	{
        	           		Name = ".ctor",
        	           		IsSpecialName = true,
        	           		Type = SystemTypes.Void,
        	           		Visibility = Visibility.Public
        	           	};
        	type.Members.Add(ctor);

            int n;
            if (args != null)
            {
                n = args.Length;
                for (int i = 0; i < n; i += 2)
                {
                	ctor.Parameters.Add(new Parameter
                	                    	{
                	                    		Type = args[i] as IType,
                	                    		Name = args[i + 1] as string
                	                    	});
                }
            }

            n = ctor.Parameters.Count;
            for (int i = 0; i < n; ++i)
            {
                var p = ctor.Parameters[i];
                p.Index = i + 1;
            	type.Members.Add(new Field
            	                 	{
            	                 		Name = p.Name.Capitalize(),
            	                 		Visibility = Visibility.Public,
            	                 		Type = p.Type
            	                 	});
            }

        	var body = new MethodBody(ctor) {MaxStackSize = 2};
        	ctor.Body = body;

            var code = new MethodCode(body);

            var baseCtor = baseType.Methods.Find(".ctor", 0);

            code.Call(baseCtor);

            for (int i = 0; i < n; ++i)
            {
                code.SetField(type.Fields[i], code.ArgRef(i));
            }

            return type;
        }
    }
}