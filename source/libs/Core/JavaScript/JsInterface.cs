using System;
using System.Collections.Generic;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Core.JavaScript
{
	internal sealed class JsInterface : JsNode
	{
		private readonly List<JsClass> _impls = new List<JsClass>();

		public JsInterface(IType type)
		{
			Type = type;
		}

		public IType Type { get; private set; }

		public IList<JsClass> Implementations
		{
			get { return _impls; }
		}

		public static JsInterface Make(IType type)
		{
			var iface = type.Data as JsInterface;
			if (iface == null)
			{
				iface = new JsInterface(type);
				type.Data = iface;
			}
			return iface;
		}

		public override void Write(JsWriter writer)
		{
			throw new NotSupportedException();
		}
	}
}