using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Syntax;

namespace DataDynamics.PageFX.Common.TypeSystem
{
	/// <summary>
	/// List of <see cref="Module"/> objects.
	/// </summary>
	public sealed class ModuleCollection : IModuleCollection
	{
		private readonly List<IModule> _list = new List<IModule>();

		public int Count
		{
			get { return _list.Count; }
		}

		public IModule this[int index]
		{
			get { return _list[index]; }
		}

		public void Add(IModule module)
		{
			if (module == null)
				throw new ArgumentNullException("module");

			_list.Add(module);
		}

		public IModule this[string name]
		{
			get { return _list.Find(m => m.Name == name); }
		}

		public IEnumerable<ICodeNode> ChildNodes
		{
			get { return this.Cast<ICodeNode>(); }
		}

		/// <summary>
		/// Gets or sets user defined data assotiated with this object.
		/// </summary>
		public object Data { get; set; }

		public string ToString(string format, IFormatProvider formatProvider)
		{
			return SyntaxFormatter.Format(this, format, formatProvider);
		}

		public override string ToString()
		{
			return ToString(null, null);
		}

		public IEnumerator<IModule> GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}