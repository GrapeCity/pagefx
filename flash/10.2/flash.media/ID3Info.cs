using System;
using System.Runtime.CompilerServices;

namespace flash.media
{
    /// <summary>
    /// The ID3Info class contains properties that reflect ID3 metadata. You can get additional
    /// metadata for MP3 files by accessing the id3
    /// property of the Sound class; for example, mySound.id3.TIME .
    /// For more information, see the entry for Sound.id3  and
    /// the ID3 tag definitions at http://www.id3.org .
    /// </summary>
    [PageFX.AbcInstance(327)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class ID3Info : Avm.Object
    {
        /// <summary>The name of the song; corresponds to the ID3 2.0 tag TIT2.</summary>
        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public Avm.String songName;

        /// <summary>The name of the artist; corresponds to the ID3 2.0 tag TPE1.</summary>
        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public Avm.String artist;

        /// <summary>The name of the album; corresponds to the ID3 2.0 tag TALB.</summary>
        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        public Avm.String album;

        /// <summary>The year of the recording; corresponds to the ID3 2.0 tag TYER.</summary>
        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        public Avm.String year;

        /// <summary>A comment about the recording; corresponds to the ID3 2.0 tag COMM.</summary>
        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.FP9]
        public Avm.String comment;

        /// <summary>The genre of the song; corresponds to the ID3 2.0 tag TCON.</summary>
        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP9]
        public Avm.String genre;

        /// <summary>The track number; corresponds to the ID3 2.0 tag TRCK.</summary>
        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.FP9]
        public Avm.String track;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ID3Info();
    }
}
