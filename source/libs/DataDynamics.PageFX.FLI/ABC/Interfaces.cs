using System.Collections.Generic;
using DataDynamics.PageFX.FlashLand.Swf;

namespace DataDynamics.PageFX.FlashLand.Abc
{
    /// <summary>
    /// Interfaces ABC constant.
    /// </summary>
    public interface IAbcConst : ISwfIndexedAtom
    {
        /// <summary>
        /// Returns kind of the constant.
        /// </summary>
        AbcConstKind Kind { get; }

        /// <summary>
        /// Gets or sets value of the constant.
        /// </summary>
        object Value { get; set; }

        string Key { get; }
    }

    /// <summary>
    /// Interfaces ABC constant pool.
    /// </summary>
    public interface IAbcConstPool : IEnumerable<IAbcConst>
    {
        /// <summary>
        /// Gets number of constants in pool.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets const at specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        IAbcConst this[int index] { get; }

        /// <summary>
        /// Determines whether given constant is defined in this pool.
        /// </summary>
        /// <param name="c">constant to check.</param>
        /// <returns>true if defined; otherwise, false</returns>
        bool IsDefined(IAbcConst c);

        /// <summary>
        /// Imports given constant.
        /// </summary>
        /// <param name="c">constant to import.</param>
        /// <returns>imported constant.</returns>
        IAbcConst Import(IAbcConst c);
    }

    public interface IAbcFileSubject
    {
        AbcFile ABC { get; set; }
    }

    public interface IImportable
    {
        void ImportTo(AbcFile abc);
    }
}