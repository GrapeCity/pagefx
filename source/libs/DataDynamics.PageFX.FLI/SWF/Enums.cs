using System;

namespace DataDynamics.PageFX.FLI.SWF
{
    [Flags]
    public enum SwfTagDecodeOptions
    {
        /// <summary>
        /// Decoding tags and characters is enabled.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Disables decoding of tags. Every tag will be presented using <see cref="SwfTagAny"/>.
        /// Note: sprite tags will be decoded in any case.
        /// </summary>
        DonotDecodeTags = 1,

        /// <summary>
        /// Disables decoding of characters. Every character will be presented using <see cref="SwfCharacterAny"/>.
        /// Note: sprite tags will be decoded in any case.
        /// </summary>
        DonotDecodeCharacters = 2,

        /// <summary>
        /// Disables decoding of image characters. Image tags will be presented using <see cref="SwfCharacterAny"/>.
        /// Note: sprite tags will be decoded in any case.
        /// </summary>
        DonotDecodeImages = 4,

        /// <summary>
        /// Disables decoding of sprite tags. Sprite tags will be presented using <see cref="SwfCharacterAny"/>.
        /// </summary>
        DonotDecodeSprites = 8,

        /// <summary>
        /// Synonym for <see cref="DonotDecodeTags"/>.
        /// </summary>
        Fast = DonotDecodeTags,
    }
}