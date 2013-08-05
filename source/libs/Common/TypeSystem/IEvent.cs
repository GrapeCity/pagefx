using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Collections;

namespace DataDynamics.PageFX.Common.TypeSystem
{
    /// <summary>
    /// Represents type event.
    /// </summary>
    public interface IEvent : ITypeMember
    {
        /// <summary>
        /// Gets method used to add event handlers.
        /// </summary>
		IMethod Adder { get; }

        /// <summary>
        /// Gets method used to remove event handlers.
        /// </summary>
		IMethod Remover { get; }

        /// <summary>
        /// Gets method used to raise the event.
        /// </summary>
		IMethod Raiser { get; }
    }

    /// <summary>
    /// Represents collection of <see cref="IEvent"/>s.
    /// </summary>
    public interface IEventCollection : IReadOnlyList<IEvent>, ICodeNode
    {
	    /// <summary>
        /// Finds event with given name.
        /// </summary>
        /// <param name="name">name of event to find.</param>
        /// <returns>event with given name or null</returns>
        IEvent this[string name] { get; }

		// TODO try to remove this method at least from this interface, provide explicit method for event registration where it is needed

	    void Add(IEvent item);
    }
}