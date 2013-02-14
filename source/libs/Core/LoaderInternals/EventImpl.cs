using System;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Syntax;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Core.LoaderInternals.Collections;
using DataDynamics.PageFX.Core.Metadata;

namespace DataDynamics.PageFX.Core.LoaderInternals
{
	internal sealed class EventImpl : IEvent
	{
		private readonly EventAttributes _flags;
		private readonly AssemblyLoader _loader;
		private IMethod _adder;
		private ICustomAttributeCollection _customAttributes;
		private IMethod _raiser;
		private IMethod _remover;

		public EventImpl(AssemblyLoader loader, MetadataRow row, int index)
		{
			_loader = loader;

			MetadataToken = SimpleIndex.MakeToken(TableId.Event, index + 1);
			Name = row[Schema.Event.Name].String;
			_flags = (EventAttributes) row[Schema.Event.EventFlags].Value;
		}

		public int MetadataToken { get; private set; }

		public ICustomAttributeCollection CustomAttributes
		{
			get { return _customAttributes ?? (_customAttributes = new CustomAttributes(_loader, this)); }
		}

		public IEnumerable<ICodeNode> ChildNodes
		{
			get { return Enumerable.Empty<ICodeNode>(); }
		}

		public object Data { get; set; }

		public string Documentation { get; set; }

		public IAssembly Assembly
		{
			get { return _loader.Assembly; }
		}

		public IModule Module
		{
			get { return _loader.MainModule; }
		}

		public MemberType MemberType
		{
			get { return MemberType.Event; }
		}

		public string Name { get; private set; }

		public string FullName
		{
			get { return this.BuildFullName(); }
		}

		public string DisplayName
		{
			get { return Name; }
		}

		public IType DeclaringType
		{
			get { return FromMethod(x => x.DeclaringType); }
		}

		public IType Type
		{
			get
			{
				var m = GetMethods().FirstOrDefault(x => x != null && x.Parameters.Count > 0);
				return m == null ? null : m.Parameters[0].Type;
			}
		}

		public Visibility Visibility
		{
			get
			{
				return new[] {Visibility.Private}
					.Concat(GetMethods().Select(x => x.Visibility))
					.Max();
			}
		}

		public bool IsStatic
		{
			get { return GetMethods().Any(x => x.IsStatic); }
		}

		public bool IsSpecialName
		{
			get { return (_flags & EventAttributes.SpecialName) != 0; }
		}

		public bool IsRuntimeSpecialName
		{
			get { return (_flags & EventAttributes.RTSpecialName) != 0; }
		}

		public IMethod Adder
		{
			get { return _adder ?? (_adder = ResolveMethod(MethodSemanticsAttributes.AddOn)); }
		}

		public IMethod Remover
		{
			get { return _remover ?? (_remover = ResolveMethod(MethodSemanticsAttributes.RemoveOn)); }
		}

		public IMethod Raiser
		{
			get { return _raiser ?? (_raiser = ResolveMethod(MethodSemanticsAttributes.Fire)); }
		}

		public string ToString(string format, IFormatProvider formatProvider)
		{
			return SyntaxFormatter.Format(this, format, formatProvider);
		}

		private IEnumerable<IMethod> GetMethods()
		{
			yield return Adder;
			yield return Remover;
			yield return Raiser;
		}

		private T FromMethod<T>(Func<IMethod, T> eval)
		{
			var m = GetMethods().FirstOrDefault(x => x != null);
			return m != null ? eval(m) : default(T);
		}

		private IMethod ResolveMethod(MethodSemanticsAttributes sem)
		{
			var rows = _loader.Metadata
			                  .LookupRows(TableId.MethodSemantics, Schema.MethodSemantics.Association, this.RowIndex(), true)
			                  .ToList();
			if (rows.Count == 0)
				return null;

			var row = rows.FirstOrDefault(
				x => (MethodSemanticsAttributes) x[Schema.MethodSemantics.Semantics].Value == sem);
			if (row == null)
				return null;

			var methodIndex = row[Schema.MethodSemantics.Method].Index - 1;
			return _loader.Methods[methodIndex];
		}

		public override string ToString()
		{
			return ToString(null, null);
		}
	}
}