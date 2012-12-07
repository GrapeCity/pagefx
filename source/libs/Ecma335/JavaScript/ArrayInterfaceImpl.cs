using System;
using DataDynamics.PageFX.Common.Services;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Ecma335.JavaScript
{
	internal sealed class ArrayInterfaceImpl
	{
		private readonly JsCompiler _host;

		public ArrayInterfaceImpl(JsCompiler host)
		{
			_host = host;
		}

		public void Compile(IMethod method)
		{
			if (method.Name == "GetEnumerator")
			{
				GetEnumeratorImpl(method);
				return;
			}
		}

		private void GetEnumeratorImpl(IMethod method)
		{
			var type = method.DeclaringType;
			if (type is IGenericInstance)
			{
				var elemType = type.GetTypeArgument(0);

				var enumerator = CompileArrayEnumerator(elemType);
				var ctor = enumerator.FindConstructor(1);

				var func = new JsFunction();
				var obj = "o".Id();
				func.Body.Add(enumerator.New().Var(obj.Value));
				func.Body.Add(obj.Get(ctor.JsName()).Call("this".Id()));
				func.Body.Add(obj.Return());

				AddImpl(method, func);
			}
			else
			{
				throw new NotImplementedException();
			}
		}

		private IType CompileArrayEnumerator(IType elemType)
		{
			var enumerator = TypeFactory.MakeGenericType(_host.CorlibTypes[GenericTypeId.ArrayEnumeratorT], elemType);
			_host.CompileClass(enumerator);
			foreach (var method in enumerator.Methods)
				_host.CompileMethod(method);
			return enumerator;
		}

		private void AddImpl(IMethod method, JsFunction func)
		{
			var type = _host.ResolveSystemType(SystemTypeCode.Array);
			var klass = _host.CompileClass(type);
			klass.Add(new JsGeneratedMethod(method.JsFullName(type), func));
		}
	}
}
