using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DataDynamics.Collections;

namespace DataDynamics.PageFX.CodeModel
{
    public interface IParameterizedMember
    {
        /// <summary>
        /// Gets the parameters for this member.
        /// </summary>
        IParameterCollection Parameters { get; }
    }

    /// <summary>
    /// Represents type method
    /// </summary>
    public interface IMethod : IPolymorphicMember, IParameterizedMember
    {
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

        MethodCallingConvention CallingConvention { get; set; }
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
        /// Determines whether the method is getter for specified property.
        /// </summary>
        bool IsGetter { get; }

        /// <summary>
        /// Determines whether the method is setter for specified property.
        /// </summary>
        bool IsSetter { get; }

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

        /// <summary>
        /// Returns true if signature was changed during resolving.
        /// </summary>
        bool SignatureChanged { get; }
    }

    /// <summary>
    /// Represents collection of <see cref="IMethod"/>s.
    /// </summary>
    public interface IMethodCollection : ISimpleList<IMethod>, ICodeNode
    {
        void Add(IMethod method);

        IEnumerable<IMethod> this[string name] { get; }

        IMethod this[string name, Predicate<IMethod> predicate] { get; }

        IMethod this[string name, int argc] { get; }

        IMethod this[string name, IType arg1] { get; }

        IMethod this[string name, IType arg1, IType arg2] { get; }

        IMethod this[string name, IType arg1, IType arg2, IType arg3] { get; }

        IMethod this[string name, params IType[] args] { get; }

        IMethod this[string name, Predicate<IParameterCollection> predicate] { get; }

        IMethod this[string name, Predicate<IType> arg1] { get; }

        IMethod this[string name, Predicate<IType> arg1, Predicate<IType> arg2] { get; }

        IMethod this[string name, Predicate<IType> arg1, Predicate<IType> arg2, Predicate<IType> arg3] { get; }

        IEnumerable<IMethod> Constructors { get; }

        IMethod StaticConstructor { get; }
    }
}