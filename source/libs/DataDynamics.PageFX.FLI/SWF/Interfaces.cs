using System.Drawing;
using DataDynamics.PageFX.FlashLand.Swf.Tags;

namespace DataDynamics.PageFX.FlashLand.Swf
{
    public interface ISwfAtom
    {
        void Read(SwfReader reader);
        void Write(SwfWriter writer);
    }

    public interface ISwfIndexedAtom : ISwfAtom
    {
        int Index { get; set; }
    }

    public interface ISwfTag
    {
        SwfTagCode TagCode { get; }
    }

    /// <summary>
    /// Represents SWF character.
    /// </summary>
    public interface ISwfCharacter : ISwfTag
    {
        ushort CharacterID { get; set; }

        string Name { get; set; }
    }

    public interface ISwfImageCharacter : ISwfCharacter
    {
        Image Image { get; }
    }

    public interface ISwfDisplayObject : ISwfCharacter
    {
        ushort Depth { get; set; }
    }
}