using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Collections;

namespace DataDynamics.PageFX.Common.TypeSystem
{
	/// <summary>
	/// Represents the type member with parameters (method or property).
	/// </summary>
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

	    /// <summary>
        /// Gets the flag specifying whether the method is entry point.
        /// </summary>
        bool IsEntryPoint { get; }

        /// <summary>
        /// Gets the flag indicating whether the method is constructor.
        /// </summary>
        bool IsConstructor { get; }

        /// <summary>
        /// Gets a flag indicating the method implementation is forwarded through PInvoke (Platform Invocation Services).
        /// </summary>
        bool PInvoke { get; }

	    /// <summary>
        /// Gets value indicating what kind of implementation is provided for this method.
        /// </summary>
        MethodCodeType CodeType { get; }

        /// <summary>
        /// Gets flag indicating whether the method is managed.
        /// </summary>
        bool IsManaged { get; }

        /// <summary>
        /// Gets flag indicating that the method is declared, but its implementation is provided elsewhere.
        /// </summary>
        bool IsForwardRef { get; }

        /// <summary>
        /// Gets flag indicating that the method signature is exported exactly as declared.
        /// </summary>
        bool IsPreserveSig { get; }

        /// <summary>
        /// Gets flag indicating that the method implemented within the common language runtime itself.
        /// </summary>
        bool IsInternalCall { get; }

        /// <summary>
        /// Gets flag indicating that the method can be executed by only one thread at a time.
        /// </summary>
        bool IsSynchronized { get; }

        /// <summary>
        /// Gets flag indicating that the method can not be inlined.
        /// </summary>
        bool NoInlining { get; }

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
        /// Gets the type member (property or event) assotiated with the method.
        /// </summary>
        ITypeMember Association { get; }

		/// <summary>
        /// Indicates whether the method is explicit implementation of some interface method.
        /// </summary>
        bool IsExplicitImplementation { get; }

		/// <summary>
        /// Gets methods implemented by this method.
        /// </summary>
        IReadOnlyList<IMethod> Implements { get; }

        /// <summary>
        /// Gets method body.
        /// </summary>
        IMethodBody Body { get; }

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