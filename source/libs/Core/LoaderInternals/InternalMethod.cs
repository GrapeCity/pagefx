using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using DataDynamics.PageFX.Common;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Collections;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Core.LoaderInternals.Collections;
using DataDynamics.PageFX.Core.Metadata;

namespace DataDynamics.PageFX.Core.LoaderInternals
{
	internal sealed class InternalMethod : MemberBase, IMethod
	{
		private readonly string[] _sigNames = new string[2];
		private readonly MethodAttributes _flags;
		private readonly MethodImplAttributes _implFlags;
		private readonly MethodSignature _signature;
		private bool _associationResolved;
		private IType _declType;
		private IType _type;
		private ITypeMember _association;
		private IMethod _baseMethod;
		private bool _resolveBaseMethod = true;
		private IGenericParameterCollection _genericParameters;
		private readonly uint _rva;
		private IMethodBody _body;
		private IReadOnlyList<IMethod> _impls;
		private bool _isExplicitImpl;

		public InternalMethod(AssemblyLoader loader, MetadataRow row, int index)
			: base(loader, TableId.MethodDef, index)
		{
			Name = row[Schema.MethodDef.Name].String;

			_implFlags = (MethodImplAttributes)row[Schema.MethodDef.ImplFlags].Value;
			_flags = (MethodAttributes)row[Schema.MethodDef.Flags].Value;

			var sigBlob = row[Schema.MethodDef.Signature].Blob;
			_signature = MethodSignature.Decode(sigBlob);

			Parameters = ResolveParameters(row);

			_rva = row[Schema.MethodDef.RVA].Value;
		}

		public string Documentation { get; set; }

		public MemberType MemberType
		{
			get { return IsConstructor ? MemberType.Constructor : MemberType.Method; }
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

		public string GetSigName(Runtime runtime)
		{
			return _sigNames[(int)runtime] ?? (_sigNames[(int)runtime] = this.BuildSigName(runtime));
		}

		public IType DeclaringType
		{
			get { return _declType ?? (_declType = ResolveDeclaringType()); }
		}

		public IType Type
		{
			get { return _type ?? (_type = ResolveType()); }
		}

		public Visibility Visibility
		{
			get
			{
				var v = _flags & MethodAttributes.MemberAccessMask;
				switch (v)
				{
					case MethodAttributes.PrivateScope:
						return Visibility.PrivateScope;
					case MethodAttributes.Private:
						return Visibility.Private;
					case MethodAttributes.FamANDAssem:
					case MethodAttributes.FamORAssem:
						return Visibility.ProtectedInternal;
					case MethodAttributes.Assembly:
						return Visibility.Internal;
					case MethodAttributes.Family:
						return Visibility.Protected;
				}
				return Visibility.Public;
			}
		}

		public bool IsStatic
		{
			get { return (_flags & MethodAttributes.Static) != 0; }
		}

		public bool IsSpecialName
		{
			get { return (_flags & MethodAttributes.SpecialName) != 0; }
		}

		public bool IsRuntimeSpecialName
		{
			get { return (_flags & MethodAttributes.RTSpecialName) != 0; }
		}

		public bool IsAbstract
		{
			get { return !IsStatic && (_flags & MethodAttributes.Abstract) != 0; }
		}

		public bool IsVirtual
		{
			get { return !IsStatic && (_flags & MethodAttributes.Virtual) != 0; }
		}

		public bool IsFinal
		{
			get { return !IsStatic && (_flags & MethodAttributes.Final) != 0; }
		}

		public bool IsNewSlot
		{
			get { return !IsStatic && (_flags & MethodAttributes.NewSlot) != 0; }
		}

		public bool IsOverride
		{
			get { return IsVirtual && !IsNewSlot; }
		}

		public bool IsEntryPoint
		{
			get
			{
				var entryPoint = Loader.Metadata.EntryPointToken;
				return entryPoint.Table == TableId.MethodDef && entryPoint.Index - 1 == this.RowIndex();
			}
		}

		public bool IsConstructor
		{
			get { return IsSpecialName && IsCtorName(Name); }
		}

		private static bool IsCtorName(string name)
		{
			return name == CLRNames.Constructor || name == CLRNames.StaticConstructor || name == "<init>";
		}

		public bool PInvoke
		{
			get
			{
				// TODO: implement via CustomAttributes
				return false;
			}
		}

		public MethodCodeType CodeType
		{
			get { return _implFlags.CodeType(); }
		}

		public bool IsManaged
		{
			get { return (_implFlags & MethodImplAttributes.Unmanaged) == 0; }
		}

		public bool IsForwardRef
		{
			get { return (_implFlags & MethodImplAttributes.ForwardRef) != 0; }
		}

		public bool IsPreserveSig
		{
			get { return (_implFlags & MethodImplAttributes.PreserveSig) != 0; }
		}

		public bool IsInternalCall
		{
			get { return (_implFlags & MethodImplAttributes.InternalCall) != 0; }
		}

		public bool IsSynchronized
		{
			get { return (_implFlags & MethodImplAttributes.Synchronized) != 0; }
		}

		public bool NoInlining
		{
			get { return (_implFlags & MethodImplAttributes.NoInlining) != 0; }
		}

		public IParameterCollection Parameters { get; private set; }

		public IGenericParameterCollection GenericParameters
		{
			get { return _genericParameters ?? (_genericParameters = new GenericParamList(Loader, this)); }
		}

		public IType[] GenericArguments
		{
			get { return TypeImpl.EmptyTypes; }
		}

		public bool IsGeneric
		{
			get { return GenericParameters.Count > 0; }
		}

		public bool IsGenericInstance
		{
			get { return false; }
		}

		public ICustomAttributeCollection ReturnCustomAttributes
		{
			get
			{
				//TODO:
				return new CustomAttributeCollection();
			}
		}

		public ITypeMember Association
		{
			get { return _association ?? (_association = ResolveAssociation()); }
		}

		public bool IsExplicitImplementation
		{
			get { return Implements.Count == 1 && _isExplicitImpl; }
		}

		public IReadOnlyList<IMethod> Implements
		{
			get
			{
				if (NoImpls(this))
				{
					return EmptyReadOnlyList.Create<IMethod>();
				}

				if (_impls == null)
				{
					var iface = FindExplicitImpl();
					if (iface != null)
					{
						_isExplicitImpl = true;
						_impls = new[] {iface}.AsReadOnlyList();
					}
					else
					{
						_impls = ResolveImpls();
					}
				}

				return _impls;
			}
		}

		public IMethodBody Body
		{
			get
			{
				if (IsAbstract || _rva == 0)
					return null;

				return _body ?? (_body = Loader.LoadMethodBody(this, _rva));
			}
		}

		public string ReturnDocumentation { get; set; }

		public IMethod BaseMethod
		{
			get
			{
				if (_baseMethod == null && _resolveBaseMethod)
				{
					_resolveBaseMethod = false;
					_baseMethod = Method.ResolveBaseMethod(this);
				}
				return _baseMethod;
			}
		}

		public IMethod ProxyOf
		{
			get { return null; }
		}

		public IMethod InstanceOf
		{
			get { return null; }
		}

		private IParameterCollection ResolveParameters(MetadataRow row)
		{
			if (_signature.Params.Length == 0)
				return ParameterCollection.Empty;

			int from = row[Schema.MethodDef.ParamList].Index - 1;
			if (from < 0) return ParameterCollection.Empty;

			return new ParamList(Loader, this, from, _signature);
		}

		private IType ResolveDeclaringType()
		{
			var type = Loader.ResolveDeclType(this);
			if (type == null)
				throw new InvalidOperationException();
			return type;
		}

		private IType ResolveType()
		{
			var context = new Context(DeclaringType, this);

			var type = Loader.ResolveType(_signature.Type, context);
			if (type == null)
				throw new InvalidOperationException();

			return type;
		}

		private ITypeMember ResolveAssociation()
		{
			if (_associationResolved) return null;

			_associationResolved = true;

			SimpleIndex token = MetadataToken;

			var row = Loader.Metadata.LookupRow(TableId.MethodSemantics, Schema.MethodSemantics.Method, token.Index - 1, true);
			if (row == null) return null;

			SimpleIndex assoc = row[Schema.MethodSemantics.Association].Value;
			var index = assoc.Index - 1;
			switch (assoc.Table)
			{
				case TableId.Property:
					var property = Loader.Properties[index];
					return property;

				case TableId.Event:
					var @event = Loader.Events[index];
					return @event;

				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private IReadOnlyList<IMethod> ResolveImpls()
		{
			var list = new List<IMethod>();

			_impls = list.AsReadOnlyList();

			PopulateImplicitImpls(list);

			return _impls;
		}

		private static bool NoImpls(IMethod method)
		{
			if (method.IsStatic || method.IsConstructor)
				return true;

			// exclude protected, internal
			switch (method.Visibility)
			{
				case Visibility.NestedProtected:
				case Visibility.NestedProtectedInternal:
				case Visibility.NestedInternal:
				case Visibility.Protected:
				case Visibility.ProtectedInternal:
				case Visibility.Internal:
					return true;
			}

			return method.DeclaringType.IsInterface;
		}

		private void PopulateImplicitImpls(List<IMethod> list)
		{
			var declType = DeclaringType;
			var explictImpls =
				declType.Methods.Where(x => !NoImpls(x) && x.IsExplicitImplementation).Select(x => x.Implements[0]).ToList();

			// get iface methods that has no explicit impls
			var ifaces = declType.Interfaces.SelectMany(x => x.Methods).Where(x => explictImpls.All(t => t != x));

			var impls = ifaces.Where(x => Signature.Equals(this, x, true));
			list.AddRange(impls);

			// get impls of base method
			if (IsOverride && BaseMethod != null && Signature.TypeEquals(BaseMethod.Type, Type))
			{
				list.AddRange(BaseMethod.Implements);
			}
		}

		private IMethod FindExplicitImpl()
		{
			var row = Loader.Metadata.LookupRow(TableId.MethodImpl, Schema.MethodImpl.MethodBody, this.RowIndex(), true);
			if (row == null)
			{
				return null;
			}

			SimpleIndex declIdx = row[Schema.MethodImpl.MethodDeclaration].Value;

			var decl = Loader.GetMethodDefOrRef(declIdx, new Context(DeclaringType, this));
			if (decl == null)
				throw new InvalidOperationException();

			return decl;
		}
	}
}