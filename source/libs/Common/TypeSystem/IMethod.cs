using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Collections;

namespace DataDynamics.PageFX.Common.TypeSystem
{
    public interface IParameterizedMember : ITypeMember
    {
        /// <summary>
        /// Gets the parameters for this member.
        /// </summary>
        IParameterCollection Parameters { get; }
	}

	public interface IParameterizedMemberCollection<T> : IReadOnlyList<T>, ICodeNode where T:IParameterizedMember
	{
		IEnumerable<T> Find(string name);
	}

	public enum Runtime { Avm, Js }

    /// <summary>
    /// Represents type method
    /// </summary>
    public interface IMethod : IPolymorphicMember, IParameterizedMember
    {
	    /// <summary>
	    /// Gets qualified name.
	    /// </summary>
	    /// <param name="runtime"> </param>
	    string GetSigName(Runtime runtime);

	    #region Flags
        /// <summary>
        /// Gets the flag specifying whether the method is entry point.
        /// </summary>
        bool IsEntryPoint { get; }

        /// <summary>
        /// Gets the flag indicating whether the method is constructor.
        /// </summary>
        bool IsConstructor { get; }

        /// <summary>
        /// Gets or sets flag indicating the method implementation is forwarded through PInvoke (Platform Invocation Services).
        /// </summary>
        bool PInvoke { get; set; }

	    #endregion

        #region Impl Flags
        /// <summary>
        /// Gets or sets value indicating what kind of implementation is provided for this method.
        /// </summary>
        MethodCodeType CodeType { get; set; }

        /// <summary>
        /// Gets or sets flag indicating whether the method is managed.
        /// </summary>
        bool IsManaged { get; set; }

        /// <summary>
        /// Gets or sets flag indicating that the method is declared, but its implementation is provided elsewhere.
        /// </summary>
        bool IsForwardRef { get; set; }

        /// <summary>
        /// Gets or sets flag indicating that the method signature is exported exactly as declared.
        /// </summary>
        bool IsPreserveSig { get; set; }

        /// <summary>
        /// Gets or sets flag indicating that the method implemented within the common language runtime itself.
        /// </summary>
        bool IsInternalCall { get; set; }

        /// <summary>
        /// Gets or sets flag indicating that the method can be executed by only one thread at a time.
        /// </summary>
        bool IsSynchronized { get; set; }

        /// <summary>
        /// Gets or sets flag indicating that the method can not be inlined.
        /// </summary>
        bool NoInlining { get; set; }
        #endregion

        IGenericParameterCollection GenericParameters { get; }

        IType[] GenericArguments { get; }

        /// <summary>
        /// Returns true if the method is generic.
        /// </summary>
        bool IsGeneric { get; }

        /// <summary>
        /// Returns true if the method is instance of generic method.
        /// </summary>
        bool IsGenericInstance { get; }

        /// <summary>
        /// Gets collection of custom attributes for return type.
        /// </summary>
        ICustomAttributeCollection ReturnCustomAttributes { get; }

        /// <summary>
        /// Gets or sets the type member (property or event) assotiated with the meber.
        /// </summary>
        ITypeMember Association { get; set; }

		/// <summary>
        /// Gets or sets boolean flag indicating whether the method is explicit implementation of some interface method.
        /// </summary>
        bool IsExplicitImplementation { get; set; }

        /// <summary>
        /// Gets or sets methods implemented by this method
        /// </summary>
        IMethod[] ImplementedMethods { get; set; }

        /// <summary>
        /// Gets or sets method body.
        /// </summary>
        IMethodBody Body { get; set; }

        /// <summary>
        /// Gets or sets documentation for return value.
        /// </summary>
        string ReturnDocumentation { get; set; }

        /// <summary>
        /// Gets the base method which this method overrides.
        /// </summary>
        IMethod BaseMethod { get; }

        IMethod ProxyOf { get; }

        IMethod InstanceOf { get; }
    }

	/// <summary>
    /// Represents collection of <see cref="IMethod"/>s.
    /// </summary>
	public interface IMethodCollection : IParameterizedMemberCollection<IMethod>
    {
        void Add(IMethod method);

    	IEnumerable<IMethod> Constructors { get; }

        IMethod StaticConstructor { get; }
    }
}