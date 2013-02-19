using System;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Core.Metadata;

namespace DataDynamics.PageFX.Core.LoaderInternals
{
	internal sealed class EventImpl : MemberBase, IEvent
	{
		private readonly EventAttributes _flags;
		private IMethod _adder;
		private IMethod _raiser;
		private IMethod _remover;

		public EventImpl(AssemblyLoader loader, MetadataRow row, int index)
			: base(loader, TableId.Event, index)
		{
			Name = row[Schema.Event.Name].String;
			_flags = (EventAttributes) row[Schema.Event.EventFlags].Value;
		}

		public string Documentation { get; set; }

		public MemberType MemberType
		{
			get { return MemberType.Event; }
		}

		public string FullName
		{
			get { return this.BuildFullName(); }
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
			var rows = Loader.Metadata
			                  .LookupRows(TableId.MethodSemantics, Schema.MethodSemantics.Association, this.RowIndex(), true)
			                  .ToList();
			if (rows.Count == 0)
				return null;

			var row = rows.FirstOrDefault(
				x => (MethodSemanticsAttributes) x[Schema.MethodSemantics.Semantics].Value == sem);
			if (row == null)
				return null;

			var methodIndex = row[Schema.MethodSemantics.Method].Index - 1;
			return Loader.Methods[methodIndex];
		}
	}
}