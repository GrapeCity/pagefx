using System;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.CLI.JavaScript
{
	internal abstract class JsClassMember : JsNode
	{
		public abstract bool IsStatic { get; }
	}

	internal sealed class JsClass : JsNode
	{
		[Flags]
		private enum ClassFlags
		{
			ClassInit = 0x01,
			StaticFieldsCompiled = 0x02,
			InstanceFieldsCompiled = 0x04,
			BoxCompiled = 0x08,
			UnboxCompiled = 0x10
		}

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

		#region Flags

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

		public bool BoxCompiled
		{
			get { return (_flags & ClassFlags.BoxCompiled) != 0; }
			set { SetFlag(ClassFlags.BoxCompiled, value); }
		}

		public bool UnboxCompiled
		{
			get { return (_flags & ClassFlags.UnboxCompiled) != 0; }
			set { SetFlag(ClassFlags.UnboxCompiled, value); }
		}

		private void SetFlag(ClassFlags f, bool value)
		{
			if (value) _flags |= f;
			else _flags &= ~f;
		}

		#endregion

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
			string baseName = null;
			if (Base != null)
			{
				baseName = Base.Type.JsFullName();
			}

			WriteClassFunction(writer, baseName, name);
			WriteGetFields(writer, baseName, name);
			WriteStaticFields(writer, name);

			writer.Write(_members, "\n");

			if (Base != null)
			{
				writer.WriteLine();
				//TODO: enable when explicit base ref will be needed
				//writer.WriteLine("{0}.prototype.$base = {1}.prototype;", name, baseName);
				writer.WriteLine("$inherit({0}, {1});", name, baseName);
			}
		}

		private void WriteClassFunction(JsWriter writer, string baseName, string name)
		{
			if (Type.IsAvmString()) return;

			if (Type.IsInt64())
			{
				writer.WriteLine("{0} = function(hi, lo) {{", name);
				writer.IncreaseIndent();
				writer.WriteLine("this.m_hi = hi ? hi : 0;");
				writer.WriteLine("this.m_lo = lo ? lo : 0;");
				writer.WriteLine("return this;");
				writer.DecreaseIndent();
				writer.WriteLine("};"); // end of function
				return;
			}

			if (Type.IsBoxableType())
			{
				var valueType = Type.IsEnum ? Type.ValueType : Type;
				writer.WriteLine("{0} = function(v) {{", name);
				writer.IncreaseIndent();
				writer.WriteLine("this.{0} = v !== undefined ? v : {1};", SpecialFields.BoxValue, valueType.InitialValue().ToJsString());
				writer.WriteLine("return this;");
				writer.DecreaseIndent();
				writer.WriteLine("};"); // end of function
				return;
			}

			if (Type.IsNullableInstance())
			{
				writer.WriteLine("{0} = function(v) {{", name);
				writer.IncreaseIndent();

				var valueType = Type.GetTypeArgument(0);
				writer.WriteLine("this.{0} = v !== undefined ? v : {1};", SpecialFields.BoxValue, valueType.InitialValue().ToJsString());
				writer.WriteLine("this.{0} = v !== undefined;", Type.GetHasValueField().JsName());
				writer.WriteLine("return this;");

				writer.DecreaseIndent();
				writer.WriteLine("};"); // end of function
				return;
			}

			writer.WriteLine("{0} = function() {{", name);
			if (_instanceFields.Count > 0 || baseName != null)
			{
				writer.IncreaseIndent();

				if (baseName != null)
				{
					writer.WriteLine("{0}.apply(this);", baseName);
				}

				if (_instanceFields.Count > 0)
				{
					writer.Write(_instanceFields, "\n");
					writer.WriteLine();
				}

				writer.WriteLine("return this;");
				writer.DecreaseIndent();
			}
			writer.WriteLine("};"); // end of function
		}

		private void WriteGetFields(JsWriter writer, string baseName, string name)
		{
			if (_instanceFields.Count > 0)
			{
				writer.WriteLine("{0}.prototype.$fields = function() {{", name);
				writer.IncreaseIndent();

				var f = new JsArray(_instanceFields.Select(x => (object)x.Field.JsName()));

				if (Base != null && Base.HasInstanceFields)
				{
					f.Var("f").Write(writer);
					writer.WriteLine();
					writer.WriteLine("return {0}.prototype.$fields.apply(this).concat(f);", baseName);
				}
				else
				{
					f.Return().Write(writer);
					writer.WriteLine();
				}

				writer.DecreaseIndent();
				writer.WriteLine("};"); // end of $fields
			}
		}

		private void WriteStaticFields(JsWriter writer, string name)
		{
			if (_staticFields.Count > 0)
			{
				writer.WriteLine("{0}.$init_fields = function() {{", name);
				writer.IncreaseIndent();
				writer.Write(_staticFields, "\n");
				writer.WriteLine();
				writer.DecreaseIndent();
				writer.WriteLine("};"); // end of static fields initializer
			}
		}

		private bool HasInstanceFields
		{
			get { return _instanceFields.Count > 0; }
		}
	}
}
