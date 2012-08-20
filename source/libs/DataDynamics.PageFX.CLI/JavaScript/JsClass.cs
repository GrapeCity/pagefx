using System;
using System.Collections.Generic;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.JavaScript
{
	internal abstract class JsClassMember : JsNode
	{
		public abstract bool IsStatic { get; }
	}

	internal sealed class JsClass : JsNode
	{
		private readonly List<JsField> _instanceFields = new List<JsField>();
		private readonly List<JsField> _staticFields = new List<JsField>();
		private readonly List<JsClassMember> _members = new List<JsClassMember>();
		private readonly IList<JsClass> _subclasses = new List<JsClass>();
		private ClassFlags _flags;

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

		public bool HasClassInit
		{
			get { return (_flags & ClassFlags.ClassInit) != 0; }
			set { SetFlag(ClassFlags.ClassInit, value); }
		}

		public bool StaticFieldsCompiled
		{
			get { return (_flags & ClassFlags.StaticFieldsCompiled) != 0; }
			set { SetFlag(ClassFlags.StaticFieldsCompiled, value); }
		}

		public bool InstanceFieldsCompiled
		{
			get { return (_flags & ClassFlags.InstanceFieldsCompiled) != 0; }
			set { SetFlag(ClassFlags.InstanceFieldsCompiled, value); }
		}

		private void SetFlag(ClassFlags f, bool value)
		{
			if (value) _flags |= f;
			else _flags &= ~f;
		}

		public void Add(JsClassMember member)
		{
			var field = member as JsField;
			if (field != null)
			{
				if (field.IsStatic) _staticFields.Add(field);
				else _instanceFields.Add(field);
				return;
			}
			
			_members.Add(member);
		}

		public override void Write(JsWriter writer)
		{
			var name = Type.JsFullName();

			writer.WriteLine("{0} = function() {{", name);
			if (_instanceFields.Count > 0)
			{
				writer.IncreaseIndent();
				writer.Write(_instanceFields, "\n");
				writer.WriteLine();
				writer.DecreaseIndent();
			}			
			writer.WriteLine("};"); // end of class

			if (_staticFields.Count > 0)
			{
				writer.WriteLine("{0}.$init_fields = function() {{", name);
				writer.IncreaseIndent();
				writer.Write(_staticFields, "\n");
				writer.WriteLine();
				writer.DecreaseIndent();
				writer.WriteLine("};"); // end of static fields initializer
			}

			writer.Write(_members, "\n");

			//TODO: !Base.Type.IsString() should be done in JsCompiler
			if (Base != null && !Base.Type.IsString())
			{
				writer.WriteLine();
				var baseName = Base.Type.JsFullName();
				//TODO: enable when explicit base ref will be needed
				//writer.WriteLine("{0}.prototype.$base = {1}.prototype;", name, baseName);
				writer.WriteLine("$inherit({0}, {1});", name, baseName);
			}
		}
	}

	[Flags]
	internal enum ClassFlags
	{
		ClassInit = 0x01,
		StaticFieldsCompiled = 0x02,
		InstanceFieldsCompiled = 0x04,
	}
}
