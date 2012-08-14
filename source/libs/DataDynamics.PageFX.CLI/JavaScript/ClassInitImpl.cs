using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.JavaScript
{
	internal sealed class ClassInitImpl
	{
		private readonly JsCompiler _host;

		public ClassInitImpl(JsCompiler host)
		{
			_host = host;
		}

		public void InitClass(MethodContext context, JsFunction func, ITypeMember member)
		{
			if (!NeedInit(member)) return;

			var type = member as IType;
			type = type ?? member.DeclaringType;

			CallClassInit(context, func, type);
		}

		private static bool NeedInit(ITypeMember member)
		{
			var method = member as IMethod;
			if (method != null && (method.IsStatic || method.IsConstructor))
			{
				return true;
			}

			var type = member as IType;
			if (type != null && !type.IsInterface)
			{
				return true;
			}

			var field = member as IField;
			if (field != null && field.IsStatic)
			{
				return true;
			}

			return false;
		}

		private void CallClassInit(MethodContext context, JsFunction func, IType type)
		{
			if (type == null) return;

			CallClassInit(context, func, type.BaseType);

			var ctor = type.GetStaticCtor();
			if (ctor == null || ctor == context.Method) return;

			_host.CompileMethod(ctor);

			var cinitName = AddClassIinitMethod(type, ctor);

			func.Body.Add(cinitName.Id().Call().AsStatement());
		}

		private string AddClassIinitMethod(IType type, IMethod ctor)
		{
			var klass = _host.CompileClass(type);

			var cinitName = string.Format("{0}.$cinit", type.FullName);

			if (klass.HasClassInit) return cinitName;

			var flag = string.Format("{0}.$cinit_done", type.FullName);

			var cinit = new JsFunction(null);
			cinit.Body.Add(new JsText(string.Format("if ({0} != undefined) return;", flag)));
			cinit.Body.Add(new JsText(string.Format("{0} = 1;", flag)));
			cinit.Body.Add(ctor.JsFullName().Id().Call().AsStatement());

			klass.Add(new JsGeneratedMethod(cinitName, cinit));

			klass.HasClassInit = true;

			return cinitName;
		}
	}
}
