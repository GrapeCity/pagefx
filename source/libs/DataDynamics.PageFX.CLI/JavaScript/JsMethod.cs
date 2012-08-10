using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.JavaScript
{
	internal abstract class JsMethodBase : JsClassMember
	{
		public JsFunction Function { get; set; }

		public abstract string FullName { get; }

		public override void Write(JsWriter writer)
		{
			writer.Write("{0} = ", FullName);
			
			Function.Write(writer);

			writer.Write(";");
		}
	}

	internal sealed class JsMethod : JsMethodBase
	{
		public JsMethod(IMethod method)
		{
			Method = method;
		}

		public override bool IsStatic
		{
			get { return Method.IsStatic; }
		}

		public IMethod Method { get; private set; }

		public string Name
		{
			get { return Method.JsName(); }
		}

		public override string FullName
		{
			get { return Method.JsFullName(); }
		}
	}

	internal sealed class JsStaticGeneratedMethod : JsMethodBase
	{
		private readonly string _fullName;

		public JsStaticGeneratedMethod(string fullName)
		{
			_fullName = fullName;
		}

		public override bool IsStatic
		{
			get { return true; }
		}

		public override string FullName
		{
			get { return _fullName; }
		}
	}

	internal static class JsMethodExtensions
	{
		public static string JsName(this IMethod method)
		{
			return method.GetSigName(SigKind.Js);
		}

		public static string JsFullName(this IMethod method)
		{
			return string.Format("{0}.{1}{2}", method.DeclaringType.FullName, method.IsStatic ? "" : "prototype.", method.JsName());
		}
	}
}
