using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel
{
    /// <summary>
    /// Represents type event.
    /// </summary>
    public interface IEvent : ITypeMember
    {
        /// <summary>
        /// Gets or sets method used to add event handlers.
        /// </summary>
        IMethod Adder { get; set; }

        /// <summary>
        /// Gets or sets method used to remove event handlers.
        /// </summary>
        IMethod Remover { get; set; }

        /// <summary>
        /// Gets or sets method used to raise the event.
        /// </summary>
        IMethod Raiser { get; set; }

        bool IsFlash { get; set; }
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
    }
}