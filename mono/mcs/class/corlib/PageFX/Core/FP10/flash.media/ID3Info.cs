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
    [PageFX.ABC]
    [PageFX.FP9]
    public class ID3Info : Avm.Object
    {
        /// <summary>The name of the song; corresponds to the ID3 2.0 tag TIT2.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public Avm.String songName;

        /// <summary>The genre of the song; corresponds to the ID3 2.0 tag TCON.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public Avm.String genre;

        /// <summary>The name of the artist; corresponds to the ID3 2.0 tag TPE1.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public Avm.String artist;

        /// <summary>The track number; corresponds to the ID3 2.0 tag TRCK.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public Avm.String track;

        /// <summary>The name of the album; corresponds to the ID3 2.0 tag TALB.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public Avm.String album;

        /// <summary>The year of the recording; corresponds to the ID3 2.0 tag TYER.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public Avm.String year;

        /// <summary>A comment about the recording; corresponds to the ID3 2.0 tag COMM.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public Avm.String comment;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ID3Info();
    }
}
