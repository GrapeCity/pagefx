using System;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.JavaScript
{
	internal abstract class JsClassMember : JsNode
	{
		public abstract bool IsStatic { get; }
	}

	internal sealed class JsClass : JsNode
	{
		private readonly List<JsClassMember> _instanceFields = new List<JsClassMember>();
		private readonly List<JsClassMember> _staticMembers = new List<JsClassMember>();
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
				_instanceFields.Add(member);
			else
				_staticMembers.Add(member);
		}

		public override void Write(JsWriter writer)
		{
			var name = Type.FullName;
			writer.WriteLine("{0} = function() {{", name);
			if (_instanceFields.Count > 0)
			{
				writer.Write(_instanceFields, "\n");
				writer.WriteLine();
			}			
			writer.WriteLine("};"); // end of class

			writer.Write(_staticMembers, "\n");

			if (Base != null)
			{
				writer.WriteLine();
				writer.WriteLine("$inherit({0}, {1});", name, Base.Type.FullName);
			}
		}

		public static void DefineCopyMethod(JsClass klass)
		{
			if (klass.Type.TypeKind != TypeKind.Struct) return;

			var copy = new JsFunction(null);

			copy.Body.Add(klass.Type.New().Var("o"));

			var obj = "o".Id();

			foreach (var field in klass.Type.Fields.Where(field => !field.IsStatic && !field.IsConstant))
			{
				var name = field.JsName();
				var value = "this".Id().Get(name);
				copy.Body.Add(obj.Set(name, value));
			}

			copy.Body.Add(obj.Return());

			klass.Add(new JsGeneratedMethod(String.Format("{0}.prototype.$copy", klass.Type.FullName), copy));
		}
	}
}
