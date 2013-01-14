using System.Linq;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Ecma335.JavaScript
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
			if (type != null)
			{
				return !type.IsInterface && !(type is ICompoundType);
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
			if (type == null || type.Is(SystemTypeCode.Object) || type.Is(SystemTypeCode.ValueType)) return;

			if (type.BaseType != null)
			{
				CallClassInit(context, func, type.BaseType);
			}

			var ctor = type.GetStaticCtor();
			if (!HasClassInit(context, type, ctor)) return;

			var cinitName = AddClassIinitMethod(type, ctor);

			func.Body.Add(cinitName.Id().Call().AsStatement());
		}

		private static bool HasClassInit(MethodContext context, IType type, IMethod ctor)
		{
			if (ctor == null)
			{
				return type.GetFields(true).Any();
			}

			return ctor != context.Method;
		}

		private string AddClassIinitMethod(IType type, IMethod ctor)
		{
			var klass = _host.CompileClass(type);

			var typeName = type.JsFullName();
			var cinitName = string.Format("{0}.$cinit", typeName);

			if (klass.HasClassInit) return cinitName;

			klass.HasClassInit = true;

			if (ctor != null)
			{
				_host.CompileMethod(ctor);
			}

			var flag = string.Format("{0}.$cinit_done", typeName);

			var cinit = new JsFunction(null);
			cinit.Body.Add(new JsText(string.Format("if ({0} != undefined) return;", flag)));
			cinit.Body.Add(new JsText(string.Format("{0} = 1;", flag)));

			if (type.GetFields(true).Any())
			{
				var initFields = string.Format("{0}.$init_fields", typeName);
				cinit.Body.Add(new JsText(string.Format("if ({0} != undefined)", initFields)));
				cinit.Body.Add(initFields.Id().Call().AsStatement());
			}

			if (ctor != null)
			{
				cinit.Body.Add(ctor.JsFullName().Id().Call().AsStatement());
			}

			klass.Add(new JsGeneratedMethod(cinitName, cinit));

			return cinitName;
		}
	}
}
