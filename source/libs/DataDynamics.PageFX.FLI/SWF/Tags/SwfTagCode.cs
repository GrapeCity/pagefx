using System;

namespace DataDynamics.PageFX.FLI.SWF
{
    /// <summary>
    /// Enumerates SWF tag codes.
    /// </summary>
    public enum SwfTagCode : ushort
    {
        #region 1.0
        /// <summary>
        /// Mark the end of the file. It can't appear anywhere else but the end of the file.
        /// </summary>
        [SwfVersion(1)]
        [SwfTagCategory(SwfTagCategory.Format)]
        End = 0,

        /// <summary>
        /// Display the current display list and pauses for 1 frame as defined in the file header.
        /// </summary>
        [SwfVersion(1)]
        [SwfTagCategory(SwfTagCategory.Display)]
        ShowFrame = 1,

        /// <summary>
        /// Define a simple geometric shape.
        /// </summary>
        [SwfVersion(1)]
        [SwfTagCategory(SwfTagCategory.Define)]
        DefineShape = 2,

        /// <summary>
        /// Release a character which won't be used anymore.
        /// </summary>
        [SwfVersion(1)]
        [SwfTagCategory(SwfTagCategory.Display)]
        FreeCharacter = 3,

        /// <summary>
        /// Place the specified object in the current display list.
        /// </summary>
        [SwfVersion(1)]
        [SwfTagCategory(SwfTagCategory.Display)]
        PlaceObject = 4,

        /// <summary>
        /// Remove the specified object at the specified depth.
        /// </summary>
        [SwfVersion(1)]
        [SwfTagCategory(SwfTagCategory.Display)]
        RemoveObject = 5,

        /// <summary>
        /// Define a JPEG bit stream.
        /// </summary>
        [SwfVersion(1)]
        [SwfTagCategory(SwfTagCategory.Define)]
        DefineBitsJPEG = 6,

        /// <summary>
        /// Define an action button.
        /// </summary>
        [SwfVersion(1)]
        [SwfTagCategory(SwfTagCategory.Define)]
        DefineButton = 7,

        /// <summary>
        /// Define the tables used to compress/decompress all the SWF 1.0 JPEG images
        /// </summary>
        [SwfVersion(1)]
        [SwfTagCategory(SwfTagCategory.Data)]
        JPEGTables = 8,

        /// <summary>
        /// Change the background color.
        /// </summary>
        [SwfVersion(1)]
        [SwfTagCategory(SwfTagCategory.Display)]
        SetBackgroundColor = 9,

        /// <summary>
        /// List shapes corresponding to glyphs.
        /// </summary>
        [SwfVersion(1)]
        [SwfTagCategory(SwfTagCategory.Define)]
        DefineFont = 10,

        /// <summary>
        /// Defines a text of characters displayed using a font. This definition doesn't support any transparency.
        /// </summary>
        [SwfVersion(1)]
        [SwfTagCategory(SwfTagCategory.Define)]
        DefineText = 11,

        /// <summary>
        /// Actions to perform at the time the next show frame is reached and before the result is being displayed. It can duplicate sprites, start/stop movie clips, etc.
        /// All the actions within a frame are executed sequencially in the order they are defined.
        /// </summary>
        /// <remarks>
        /// Note: Some actions are specific to other versions than SWF V1.0.
        /// </remarks>
        [SwfVersion(1)]
        [SwfTagCategory(SwfTagCategory.Action)]
        DoAction = 12,

        /// <summary>
        /// Information about a previously defined font. Includes the font style, a map and the font name.
        /// </summary>
        [SwfVersion(1)]
        [SwfTagCategory(SwfTagCategory.Define)]
        DefineFontInfo = 13,
        #endregion

        #region 2.0
        /// <summary>
        /// Declare a sound effect. This includes sound samples which can later be played back using either a StartSound or a DefineButtonSound.
        /// </summary>
        /// <remarks>
        /// Note: The same DefineSound block can actually include multiple sound files and only part of the entire sound can be played back as required.
        /// </remarks>
        [SwfVersion(2)]
        [SwfTagCategory(SwfTagCategory.Define)]
        DefineSound = 14,

        /// <summary>
        /// Start the referenced sound on the next ShowFrame.
        /// </summary>
        [SwfVersion(2)]
        [SwfTagCategory(SwfTagCategory.Control)]
        StartSound = 15,

        /// <summary>
        /// Stop the referenced sound on the next ShowFrame.
        /// </summary>
        [SwfVersion(2)]
        [SwfTagCategory(SwfTagCategory.Control)]
        StopSound = 16,

        /// <summary>
        /// Defines how to play a sound effect for when an event occurs for the given button.
        /// </summary>
        [SwfVersion(2)]
        [SwfTagCategory(SwfTagCategory.Define)]
        DefineButtonSound = 17,

        /// <summary>
        /// Declare a sound effect which will be interleaved with a movie data so as to be loaded over a network connection while being played.
        /// </summary>
        [SwfVersion(2)]
        [SwfTagCategory(SwfTagCategory.Data)]
        SoundStreamHead = 18,

        /// <summary>
        /// A block of sound data. The size of this block of data will be defined in the previous SoundStreamHead tag. It can be used so sound is downloaded on a per frame basis instead of being loaded all at once.
        /// </summary>
        [SwfVersion(2)]
        [SwfTagCategory(SwfTagCategory.Data)]
        SoundStreamBlock = 19,

        /// <summary>
        /// A bitmap compressed using ZLIB (similar to the PNG format).
        /// </summary>
        [SwfVersion(2)]
        [SwfTagCategory(SwfTagCategory.Define)]
        DefineBitsLossless = 20,

        /// <summary>
        /// Declare a complete JPEG image (includes the bit stream and the tables all in one thus enabling multiple table to be used within the same SWF file).
        /// </summary>
        [SwfVersion(2)]
        [SwfTagCategory(SwfTagCategory.Define)]
        DefineBitsJPEG2 = 21,

        /// <summary>
        /// Declaration of complex 2D shapes.
        /// </summary>
        [SwfVersion(2)]
        [SwfTagCategory(SwfTagCategory.Define)]
        DefineShape2 = 22,

        /// <summary>
        /// Setup a color transformation for a button.
        /// </summary>
        [SwfVersion(2)]
        [SwfTagCategory(SwfTagCategory.Display)]
        DefineButtonCxform = 23,

        /// <summary>
        /// Disable edition capabilities of the given SWF file. Though this doesn't need to be enforced by an SWF editor, it marks the file as being copyrighted in a way.
        /// </summary>
        [SwfVersion(2)]
        [SwfTagCategory(SwfTagCategory.Security)]
        Protect = 24,

        /// <summary>
        /// The shape paths are defined as in postscript
        /// </summary>
        [SwfVersion(2)]
        [SwfTagCategory(SwfTagCategory.Display)]
        PathsArePostscript = 25,
        #endregion

        #region 3.0
        /// <summary>
        /// Place an object in the current display list (extends the functionality of the PlaceObject)
        /// </summary>
        [SwfVersion(3)]
        [SwfTagCategory(SwfTagCategory.Display)]
        PlaceObject2 = 26,

        /// <summary>
        /// Remove the object at the specified level. This tag should be used in SWF 3.0+ since it compressed better than the standard RemoveObject tag.
        /// </summary>
        [SwfVersion(3)]
        [SwfTagCategory(SwfTagCategory.Display)]
        RemoveObject2 = 28,

        /// <summary>
        /// OBSOLETE... Handle a synchronization of the display list
        /// </summary>
        [SwfVersion(3)]
        [SwfTagCategory(SwfTagCategory.Display)]
        [Obsolete]
        SyncFrame = 29,

        /// <summary>
        /// Free all of the characters
        /// </summary>
        [SwfVersion(3)]
        [SwfTagCategory(SwfTagCategory.Display)]
        [Obsolete]
        FreeAll = 31,

        /// <summary>
        /// A shape V3 includes alpha values.
        /// </summary>
        [SwfVersion(3)]
        [SwfTagCategory(SwfTagCategory.Define)]
        DefineShape3 = 32,

        /// <summary>
        /// A text V2 includes alpha values.
        /// </summary>
        [SwfVersion(3)]
        [SwfTagCategory(SwfTagCategory.Define)]
        DefineText2 = 33,

        /// <summary>
        /// A Flash 3 button that contains color transform and sound info
        /// </summary>
        [SwfVersion(3)]
        [SwfTagCategory(SwfTagCategory.Define)]
        DefineButton2 = 34,

        /// <summary>
        /// Defines a complete JPEG (includes the tables and data bit stream) twice. Once with the RGB data and once with the alpha channel.
        /// </summary>
        /// <remarks>
        ///  Note: The alpha channel uses the Z-lib compression instead of the JPEG one.
        /// </remarks>
        [SwfVersion(3)]
        [SwfTagCategory(SwfTagCategory.Define)]
        DefineBitsJPEG3 = 35,

        /// <summary>
        /// Defines an RGBA bitmap compressed using ZLIB (similar to the PNG format).
        /// </summary>
        [SwfVersion(3)]
        [SwfTagCategory(SwfTagCategory.Define)]
        DefineBitsLossless2 = 36,

        /// <summary>
        /// Define a sequence of tags that describe the behavior of a sprite.
        /// </summary>
        [SwfVersion(3)]
        [SwfTagCategory(SwfTagCategory.Define)]
        DefineSprite = 39,

        /// <summary>
        /// Names a character definition, character id and a string, (used for buttons, bitmaps, sprites and sounds)
        /// </summary>
        [SwfVersion(3)]
        [SwfTagCategory(SwfTagCategory.Define)]
        [Obsolete]
        NameCharacter = 40,

        /// <summary>
        /// A tag command for the Flash Generator customer serial id and cpu information. 
        /// [preilly] Repurposed for Flex Audit info.
        /// </summary>
        [SwfVersion(3)]
        [SwfTagCategory(SwfTagCategory.Metadata)]
        ProductInfo = 41,

        /// <summary>
        /// Defines the contents of a text block with formating information
        /// </summary>
        [SwfVersion(3)]
        [SwfTagCategory(SwfTagCategory.Define)]
        [Obsolete]
        DefineTextFormat = 42,

        /// <summary>
        /// A string label for the current frame.
        /// </summary>
        [SwfVersion(3)]
        [SwfTagCategory(SwfTagCategory.Display)]
        FrameLabel = 43,

        //For lossless streaming sound; should not have needed this...
        /// <summary>
        /// Declare a sound effect which will be interleaved with a movie data so as to be loaded over a network connection while being played.
        /// </summary>
        [SwfVersion(3)]
        [SwfTagCategory(SwfTagCategory.Data)]
        SoundStreamHead2 = 45,

        /// <summary>
        /// A morph shape definition
        /// </summary>
        [SwfVersion(3)]
        [SwfTagCategory(SwfTagCategory.Define)]
        DefineMorphShape = 46,

        /// <summary>
        /// A tag command for the Flash Generator (WORD duration, STRING label)
        /// </summary>
        [SwfVersion(3)]
        [SwfTagCategory(SwfTagCategory.Unknown)]
        [Obsolete]
        GenerateFrame = 47,

        /// <summary>
        /// Defines a font with extended information
        /// </summary>
        [SwfVersion(3)]
        [SwfTagCategory(SwfTagCategory.Define)]
        DefineFont2 = 48,

        /// <summary>
        /// A tag command for the Flash Generator intrinsic
        /// </summary>
        [SwfVersion(3)]
        [SwfTagCategory(SwfTagCategory.Generator)]
        [Obsolete]
        GeneratorCommand = 49,

        /// <summary>
        /// A tag command for the Flash Generator intrinsic Command
        /// </summary>
        [SwfVersion(3)]
        [SwfTagCategory(SwfTagCategory.Generator)]
        [Obsolete]
        DefineCommandObj = 50,

        /// <summary>
        /// Defines the character set used to store strings
        /// </summary>
        [SwfVersion(3)]
        [SwfTagCategory(SwfTagCategory.Define)]
        [Obsolete]
        CharacterSet = 51,

        /// <summary>
        /// Defines a reference to an external font source
        /// </summary>
        [SwfVersion(3)]
        [SwfTagCategory(SwfTagCategory.Define)]
        [Obsolete]
        FontRef = 52,
        #endregion

        #region 4.0
        /// <summary>
        /// An edit text enables the end users to enter text in a Flash window.
        /// </summary>
        [SwfVersion(4)]
        [SwfTagCategory(SwfTagCategory.Define)]
        DefineEditText = 37,

        /// <summary>
        /// A reference to an external video stream
        /// </summary>
        [SwfVersion(4)]
        [SwfTagCategory(SwfTagCategory.Define)]
        [Obsolete]
        DefineVideo = 38,
        #endregion

        #region 5.0
        /// <summary>
        /// Export assets from this swf file.
        /// </summary>
        [SwfVersion(5)]
        [SwfTagCategory(SwfTagCategory.Format)]
        ExportAssets = 56,

        /// <summary>
        /// Import assets into this swf file
        /// </summary>
        [SwfVersion(5)]
        [SwfTagCategory(SwfTagCategory.Format)]
        ImportAssets = 57,

        //this movie may be debugged
        /// <summary>
        /// The data of this tag is an MD5 password like the Protect tag.
        /// When it exists and you know the password, you will be given the right to debug the movie with Flash V5.x.
        /// </summary>
        [SwfVersion(5)]
        [SwfTagCategory(SwfTagCategory.Security)]
        EnableDebugger = 58,
        #endregion

        #region 6.0
        /// <summary>
        /// Actions to perform the first time the following show frame is reached. 
        /// All the initialization actions are executed before any other actions. 
        /// You have to specify a sprite to which the actions are applied. 
        /// Thus you don't need a set target action. When multiple initialization action blocks are within the same frame, they are executed one after another.
        /// </summary>
        [SwfVersion(6)]
        [SwfTagCategory(SwfTagCategory.Action)]
        DoInitAction = 59,

        /// <summary>
        /// Defines the necessary information for the player to display a video stream (i.e. size, codec, how to decode the data, etc.). Play the frames with VideoFrame tags.
        /// </summary>
        [SwfVersion(6)]
        [SwfTagCategory(SwfTagCategory.Define)]
        DefineVideoStream = 60,

        /// <summary>
        /// Show the specified video frame of a movie.
        /// </summary>
        [SwfVersion(6)]
        [SwfTagCategory(SwfTagCategory.Display)]
        VideoFrame = 61,

        /// <summary>
        /// Defines information about a font, like the DefineFontInfo  tag plus a language reference. To force the use of a given language, this tag should be used in v6.0+ movies instead of the DefineFontInfo.
        /// </summary>
        [SwfVersion(6)]
        [SwfTagCategory(SwfTagCategory.Define)]
        DefineFontInfo2 = 62,

        //unique id to match up swf to swd
        /// <summary>
        /// This tag is used when debugging an SWF movie. It gives information about what debug file to load to match the SWF movie with the source. The identifier is a UUID.
        /// </summary>
        [SwfVersion(6)]
        [SwfTagCategory(SwfTagCategory.Metadata)]
        DebugID = 63,

        //this movie may be debugged (see 59)
        /// <summary>
        /// The data of this tag is a 16 bits word followed by an MD5 password like the Protect tag. When it exists and you know the password, you will be given the right to debug the movie with Flash V6.x and over.
        /// </summary>
        [SwfVersion(6)]
        [SwfTagCategory(SwfTagCategory.Security)]
        EnableDebugger2 = 64,

        /// <summary>
        /// Change limits used to ensure scripts don't use more resources than you choose. In version 7, it supports a maximum recursive depth and a maximum amount of time scripts can be run for in seconds.
        /// </summary>
        [SwfVersion(6)]
        [SwfTagCategory(SwfTagCategory.Security)]
        ScriptLimits = 65,
        #endregion

        #region 7.0
        //allows us to set .tabindex via tags, not actionscript
        /// <summary>
        /// Define the order index so the player knows where to go next when the Tab key is pressed by the user.
        /// </summary>
        [SwfVersion(7)]
        [SwfTagCategory(SwfTagCategory.Control)]
        SetTabIndex = 66,
        #endregion

        #region 8.0
        //includes enhanced line style and gradient properties
        /// <summary>
        /// Declare a shape which supports new line caps, scaling and fill options.
        /// </summary>
        [SwfVersion(8)]
        [SwfTagCategory(SwfTagCategory.Define)]
        DefineShape4 = 67,

        /// <summary>
        /// Defines whole-SWF attributes (must be the FIRST tag after the SWF header)
        /// </summary>
        [SwfVersion(8)]
        [SwfTagCategory(SwfTagCategory.Metadata)]
        FileAttributes = 69,

        /// <summary>
        /// Place an object in the display list. The object can include bitmap caching information, a blend mode and a set of filters.
        /// </summary>
        [SwfVersion(8)]
        [SwfTagCategory(SwfTagCategory.Display)]
        PlaceObject3 = 70,

        /// <summary>
        /// Import assets into this swf file using the SHA-1 digest to enable cached cross domain RSL downloads.
        /// </summary>
        [SwfVersion(8)]
        [SwfTagCategory(SwfTagCategory.Format)]
        ImportAssets2 = 71,

        /// <summary>
        /// Embedded .abc (AVM+) bytecode
        /// </summary>
        [SwfVersion(8)]
        [SwfTagCategory(SwfTagCategory.Action)]
        DoAbc = 72,

        //ADF alignment zones
        /// <summary>
        /// Define advanced hints about a font glyphs to place them on a pixel boundary.
        /// </summary>
        [SwfVersion(8)]
        [SwfTagCategory(SwfTagCategory.Data)]
        DefineFontAlignZones = 73,

        /// <summary>
        /// Define whether CSM text should be used in a previous DefineText, DefineText2 or DefineEditText.
        /// </summary>
        [SwfVersion(8)]
        [SwfTagCategory(SwfTagCategory.Data)]
        CSMTextSettings = 74,

        /// <summary>
        /// Defines a font with saffron information
        /// </summary>
        [SwfVersion(8)]
        [SwfTagCategory(SwfTagCategory.Define)]
        DefineFont3 = 75,

        /// <summary>
        /// Instantiate objects from a set of classes.
        /// </summary>
        [SwfVersion(8)]
        [SwfTagCategory(SwfTagCategory.Action)]
        SymbolClass = 76,

        /// <summary>
        /// XML blob with comments, description, copyright, etc.
        /// The format is RDF compliant to the XMP as defined on W3C.
        /// </summary>
        [SwfVersion(8)]
        [SwfTagCategory(SwfTagCategory.Metadata)]
        Metadata = 77,

        //Scale9 grid
        /// <summary>
        /// Define scale factors for a window, a button, or other similar objects.
        /// </summary>
        [SwfVersion(8)]
        [SwfTagCategory(SwfTagCategory.Display)]
        DefineScalingGrid = 78,

        /// <summary>
        /// Revised ABC version with a name
        /// </summary>
        [SwfVersion(8)]
        [SwfTagCategory(SwfTagCategory.Action)]
        DoAbc2 = 82,

        [SwfVersion(8)]
        [SwfTagCategory(SwfTagCategory.Define)]
        DefineShape6,

        /// <summary>
        /// Includes enhanced line style abd gradient properties.
        /// </summary>
        [SwfVersion(8)]
        [SwfTagCategory(SwfTagCategory.Define)]
        DefineMorphShape2 = 84,

        //new in 8.5, only works on root timeline
        /// <summary>
        /// Define raw data for scenes and frames.
        /// </summary>
        [SwfVersion(8)]
        [SwfTagCategory(SwfTagCategory.Data)]
        DefineSceneAndFrameLabelData = 86,

        /// <summary>
        /// Defines a buffer of any size with any binary user data.
        /// </summary>
        [SwfVersion(8)]
        [SwfTagCategory(SwfTagCategory.Data)]
        DefineBinaryData = 87,

        /// <summary>
        /// Adds name and copyright information for a font
        /// </summary>
        [SwfVersion(8)]
        [SwfTagCategory(SwfTagCategory.Data)]
        DefineFontName = 88,
        #endregion

        #region 9.0
        #endregion

        #region Special
        [SwfVersion(1000)]
        [SwfTagCategory(SwfTagCategory.Editor)]
        /// <summary>
        /// A special tag used only in the editor
        /// </summary>
        DefineBitsPtr = 1023,
        #endregion
    }

    public enum SwfTagCategory
    {
        Unknown,
        Format,
        Control,
        Display,
        Define,
        Action,
        Data,
        Metadata,
        Security,
        Editor,
        Generator
    }
}