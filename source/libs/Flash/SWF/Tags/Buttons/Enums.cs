using System;

namespace DataDynamics.PageFX.Flash.Swf.Tags.Buttons
{
    [Flags]
    public enum SwfButtonState
    {
        StateUp = 0x01,
        StateOver = 0x02,
        StateDown = 0x04,
        HitTest = 0x08,
        HasFilterList = 0x10,
        HasBlendMode = 0x20,
    }

    public enum SwfBlendMode
    {
        Normal = 0,
        Layer = 2,
        Multiply = 3,
        Screen = 4,
        Lighten = 5,
        Darken = 6,
        Add = 7,
        Subtract = 8,
        Difference = 9,
        Invert = 10,
        Alpha = 11,
        Erase = 12,
        Overlay = 13,
        Hardlight = 14,
    }
}