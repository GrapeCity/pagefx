using System.Collections.Generic;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.JavaScript
{
	internal sealed class JsClass : JsNode
	{
		private readonly List<JsClassMember> _instance = new List<JsClassMember>();
		private readonly List<JsClassMember> _static = new List<JsClassMember>();
		private readonly IList<JsClass> _subclasses = new List<JsClass>();

		public JsClass(IType type, JsClass baseClass)
		{
			Type = type;
			Base = baseClass;
		}

		public IType Type { get; private set; }

		public JsClass Base { get; private set; }

		public IList<JsClass> Subclasses
		{
			get { return _subclasses; }
		}

		public void Add(JsClassMember member)
		{
			if (member is JsField && !member.IsStatic)
				_instance.Add(member);
			else
				_static.Add(member);
		}

		public override void Write(JsWriter writer)
		{
			var name = Type.FullName;
			writer.WriteLine("{0} = function() {{", name);
			if (_instance.Count > 0)
			{
				writer.Write(_instance, "\n");
				writer.WriteLine();
			}			
			writer.WriteLine("};"); // end of class

			writer.Write(_static, "\n");

			if (Base != null)
			{
				writer.WriteLine("$inherit({0}, {1});", name, Base.Type.FullName);
			}
		}
	}

	internal abstract class JsClassMember : JsNode
	{
		public abstract bool IsStatic { get; }
	}
}
