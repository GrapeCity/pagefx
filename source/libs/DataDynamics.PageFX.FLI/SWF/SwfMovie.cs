using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using DataDynamics.Imaging;
using DataDynamics.PageFX.FLI.ABC;
using DataDynamics.PageFX.FLI.SWC;

namespace DataDynamics.PageFX.FLI.SWF
{
    /// <summary>
    /// Represents flash movie (SWF file).
    /// </summary>
    public class SwfMovie
    {
        #region Constructors
        /// <summary>
        /// Initializes new instance of <see cref="SwfMovie"/> class.
        /// </summary>
        public SwfMovie()
        {
            _displayList = new HashList<ushort, ISwfDisplayObject>(obj => obj.Depth);
        }

        /// <summary>
        /// Initializes new instance of <see cref="SwfMovie"/> class and loads it from given swiff file.
        /// </summary>
        /// <param name="path">swiff file path to load</param>
        public SwfMovie(string path)
            : this()
        {
            Load(path);
        }

        /// <summary>
        /// Initializes new instance of <see cref="SwfMovie"/> class and loads it from given input stream.
        /// </summary>
        /// <param name="input">stream with swiff file to load</param>
        public SwfMovie(Stream input)
            : this()
        {
            Load(input);
        }

        public SwfMovie(string path, SwfTagDecodeOptions decodeOptions)
            : this()
        {
            Load(path, decodeOptions);
        }

        public SwfMovie(Stream input, SwfTagDecodeOptions decodeOptions)
            : this()
        {
            Load(input, decodeOptions);
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets name which is used when SWF file is in SWC file.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private string _name;

        public int Index { get; set; }

        #region SWC
        public SwcFile SWC { get; set; }

        public bool InSwc
        {
            get { return SWC != null; }
        }

        public XmlElement SwcElement { get; set; }

        internal bool SwcScriptsCached;
        #endregion

        #region Header
        /// <summary>
        /// Gets or sets SWF file format version
        /// </summary>
        public int Version
        {
            get { return _version; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("value");
                _version = value;
            }
        }
        private int _version = 9;

        /// <summary>
        /// Gets or sets frame size.
        /// </summary>
        public SizeF FrameSize
        {
            get { return _frameSize; }
            set { _frameSize = value; }
        }
        private SizeF _frameSize = DefaultFrameSize;

        internal static readonly SizeF DefaultFrameSize = new SizeF(800, 600);

        public float FrameWidth
        {
            get { return _frameSize.Width; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("value");
                _frameSize.Width = value;
            }
        }

        public float FrameHeight
        {
            get { return _frameSize.Height; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("value");
                _frameSize.Height = value;
            }
        }

        /// <summary>
        /// Gets or sets frame rate.
        /// </summary>
        public float FrameRate
        {
            get { return _frameRate; }
            set { _frameRate = value; }
        }
        private float _frameRate = DefaultFrameRate;

        internal const float DefaultFrameRate = 25;

        /// <summary>
        /// Gets or sets frame count.
        /// </summary>
        public ushort FrameCount
        {
            get { return _frameCount; }
            set { _frameCount = value; }
        }
        private ushort _frameCount;

        public bool AutoFrameCount
        {
            get { return _autoFrameCount; }
            set { _autoFrameCount = value; }
        }
        private bool _autoFrameCount = true;
        #endregion

        /// <summary>
        /// Gets the list of tags contained in this movie.
        /// </summary>
        public SwfTagList Tags
        {
            get { return _tags; }
        }
        private readonly SwfTagList _tags = new SwfTagList();

        public ushort LastID
        {
            get { return CID; }
        }
        internal ushort CID;

        public ushort NewCharacterID()
        {
            return ++CID;
        }
        #endregion

        #region Control Tags
        public void SetFileAttributes(SwfFileAttributes attrs)
        {
            Tags.Add(new SwfTagFileAttributes(attrs));
        }

        public void SetDefaultFileAttributes()
        {
            switch (Version)
            {
                case 9:
                    SetFileAttributes(SwfFileAttributes.Default9);
                    break;

                case 10:
                    SetFileAttributes(SwfFileAttributes.Default10);
                    break;

                default:
                    SetFileAttributes(SwfFileAttributes.Default);
                    break;
            }
        }

        public void SetScriptLimits(ushort maxRecusionDepth, ushort timeout)
        {
            Tags.Add(new SwfTagScriptLimits(maxRecusionDepth, timeout));
        }

        public void SetDefaultScriptLimits()
        {
            SetScriptLimits(1000, 60);
        }

        public void SetBackgroundColor(Color color)
        {
            Tags.Add(new SwfTagSetBackgroundColor(color));
        }

        public static readonly Color DefaultBackgroundColor = Color.FromArgb(0x86, 0x96, 0xA7);

        public void SetDefaultBackgroundColor()
        {
            SetBackgroundColor(DefaultBackgroundColor);
        }

        public void SetFrameLabel(string label)
        {
            Tags.Add(new SwfTagFrameLabel(label));
        }

        public void EnableDebugger(string password)
        {
            Tags.Add(new SwfTagEnableDebugger(password));
        }

        public void EnableDebugger(ushort flags, string password)
        {
            Tags.Add(new SwfTagEnableDebugger2(flags, password));
        }
        #endregion

        #region Define Tags
        public SwfCharacter CreateBitmapTag(Image image)
        {
            ushort id = NewCharacterID();
            if (Version >= 3)
                return new SwfTagDefineBitsLossless2(id, image);
            return new SwfTagDefineBitsLossless(id, image);
        }

        public ushort DefineBitmap(Image image)
        {
            var tag = CreateBitmapTag(image);
            Tags.Add(tag);
            return tag.CharacterID;
        }

        public SwfCharacter CreateJpegTag(Image image)
        {
            ushort id = NewCharacterID();
            return new SwfTagDefineBitsJPEG2(id, image);
        }

        private SwfTag CreateImageTag(int id, Image image)
        {
            int ver = Version;
            if (image.RawFormat == ImageFormat.Jpeg)
                return new SwfTagDefineBitsJPEG2(id, image);

            if (ver >= 3)
                return new SwfTagDefineBitsLossless2(id, image);
            return new SwfTagDefineBitsLossless(id, image);
        }

        public ushort DefineImage(Image image)
        {
            if (image == null)
                throw new ArgumentNullException("image");
            ushort id = NewCharacterID();
            var tag = CreateImageTag(id, image);
            Tags.Add(tag);
            return id;
        }

        public ISwfCharacter GetCharacter(int id)
        {
            return Tags.GetCharacter(id);
        }

        public ISwfDisplayObject GetDisplayObject(int id)
        {
            return Tags.GetCharacter(id) as ISwfDisplayObject;
        }

        public Bitmap GetBitmap(int id)
        {
            var c = GetCharacter(id) as ISwfImageCharacter;
            if (c != null)
                return c.Image.ToBitmap();
            return null;
        }

        public void Initialize()
        {
            SetDefaultFileAttributes();
            SetDefaultScriptLimits();
        }
        #endregion

        #region DisplayList
        /// <summary>
        /// Adds ShowFrame tag.
        /// </summary>
        public void ShowFrame()
        {
            Tags.ShowFrame();
        }

        /// <summary>
        /// Adds or modifies object in display list.
        /// </summary>
        /// <param name="obj">The object to add or modify.</param>
        /// <param name="transform">The transformation of object.</param>
        public void PlaceObject(ISwfDisplayObject obj, Matrix transform)
        {
            var tag = obj as SwfTag;
            if (tag == null)
                throw new ArgumentException();

            if (_displayList.Contains(obj))
            {
                Tags.Add(new SwfTagPlaceObject2(obj, transform, SwfPlaceMode.Modify));
            }
            else
            {
                obj.Depth = (ushort)(_displayList.Count + 1);
                _displayList.Add(obj);
                Tags.Add(new SwfTagPlaceObject2(obj, transform, SwfPlaceMode.New));
            }
        }

        public void MoveObject(ISwfDisplayObject obj, float dx, float dy)
        {
            var m = new Matrix();
            m.Translate(dx, dy);
            PlaceObject(obj, m);
        }

        public void MoveObject(ushort id, float dx, float dy)
        {
            var obj = GetDisplayObject(id);
            if (obj != null)
            {
                MoveObject(obj, dx, dy);
            }
        }

        /// <summary>
        /// Removes object from the display list.
        /// </summary>
        /// <param name="depth">The depth of object to remove.</param>
        public bool RemoveObject(ushort depth)
        {
            int n = _displayList.Count;
            for (int i = 0; i < n; ++i)
            {
                var obj = _displayList[i];
                if (obj.Depth == depth)
                {
                    obj.Depth = 0;
                    _displayList.RemoveAt(i);
                    Tags.Add(new SwfTagRemoveObject2(depth));
                    return true;
                }
            }
            return false;
        }

        private readonly HashList<ushort, ISwfDisplayObject> _displayList;
        #endregion

        #region Graphics

        public SwfGraphics Graphics
        {
            get { return _graphics ?? (_graphics = new SwfGraphics(this)); }
        }
        private SwfGraphics _graphics;

        #endregion

        #region IO
        #region Read
        /// <summary>
        /// Loads flash movie from specified file.
        /// </summary>
        /// <param name="path">file path to load</param>
        public void Load(string path)
        {    
            Load(path, SwfTagDecodeOptions.Default);
        }

        /// <summary>
        /// Loads flash movie from specified file.
        /// </summary>
        /// <param name="path">file path to load</param>
        /// <param name="options">options to decode tags</param>
        public void Load(string path, SwfTagDecodeOptions options)
        {
            using (var fs = File.OpenRead(path))
                Load(fs, options);
        }

        /// <summary>
        /// Loads flash movie from specified stream.
        /// </summary>
        /// <param name="input">input stream to load.</param>
        public void Load(Stream input)
        {
            Load(input, SwfTagDecodeOptions.Default);
        }

        /// <summary>
        /// Loads flash movie from specified stream.
        /// </summary>
        /// <param name="input">input stream to load.</param>
        /// <param name="options">options to decode tags.</param>
        public void Load(Stream input, SwfTagDecodeOptions options)
        {
        	var reader = new SwfReader(input) {TagDecodeOptions = options};

        	//File Header
            string sig = reader.ReadASCII(3);
            // "FWS" or "CWS" for ZLIB compressed files (v6.0 or later)
            if (sig != "FWS" && sig != "CWS")
            {
                throw new BadFormatException("Not a valid SWF (Flash) file signature");
            }

            _version = reader.ReadUInt8(); // file version
            reader.FileVersion = _version;
            uint fileLength = reader.ReadUInt32(); // file length
            //Debug.WriteLine(string.Format("SWF File Length = {0}", fileLength));

            // If the file is compressed, this is where the ZLIB decompression ("inflate") begins
            if (sig[0] == 'C')
                reader.Stream = Zip.Uncompress(input);

            //Movie Header
            var frameSize = reader.ReadRect();
            _frameSize.Width = frameSize.Width.FromTwips();
            _frameSize.Height = frameSize.Height.FromTwips();
            _frameRate = reader.ReadFixed16();
            _frameCount = reader.ReadUInt16();

            //Movie Content
            _tags.Read(reader);
        }
        #endregion

        #region Write
        private byte[] GetFileData()
        {
            if (_autoFrameCount)
            {
                _frameCount = (ushort)_tags.Count(tag => tag.TagCode == SwfTagCode.ShowFrame);
            }

            var writer = new SwfWriter();

            int w = _frameSize.Width.ToTwips();
            int h = _frameSize.Height.ToTwips();
            writer.WriteRect(0, 0, w, h); //frame size
            writer.WriteUFloat16(_frameRate); //frame rate
            writer.WriteUInt16(_frameCount); //frame count

            //tags (content)
            _tags.Write(writer);
            
            return writer.ToByteArray();
        }

        private static readonly byte[] SigCWS = { (byte)'C', (byte)'W', (byte)'S' };
        private static readonly byte[] SigFWS = { (byte)'F', (byte)'W', (byte)'S' };
        private const byte MinVersionSupportedComression = 6;

        /// <summary>
        /// Enables or disables file compression.
        /// </summary>
        public bool AllowCompression
        {
            get { return _allowCompression; }
            set { _allowCompression = value; }
        }
        private bool _allowCompression = true;

        public byte[] ToByteArray()
        {
            var writer = new SwfWriter();

            bool compress = _allowCompression && _version >= MinVersionSupportedComression;

            //write header
        	writer.Write(compress ? SigCWS : SigFWS);

        	writer.WriteUInt8((byte)_version); //file version

            //NOTE: Comment below is from SWF specification.
            //The FileLength field is the total length of the SWF file, including the header. If this is an
            //uncompressed SWF file (FWS signature), the FileLength field should exactly match the file
            //size. If this is a compressed SWF file (CWS signature), the FileLength field indicates the total
            //length of the file after decompression, and thus generally does not match the file size.

            var data = GetFileData();
            int len = data.Length;
            if (compress)
            {
                data = Zip.Compress(data);
                //if (data.Length > len)
                //    len = data.Length;
            }

            writer.WriteUInt32((uint)(8 + len)); //file length
            writer.Write(data);

            data = writer.ToByteArray();
            return data;
        }

        /// <summary>
        /// Saves the flash movie to specified file.
        /// </summary>
        /// <param name="path">file path to which you want to save this movie.</param>
        public void Save(string path)
        {
            var data = ToByteArray();
            File.WriteAllBytes(path, data);
        }

        /// <summary>
        /// Saves the flash movie to specified stream.
        /// </summary>
        /// <param name="output">stream to which you want to save this movie.</param>
        public void Save(Stream output)
        {
            var data = ToByteArray();
            output.Write(data, 0, data.Length);
        }
        #endregion

        #region Dump
        public void Dump(XmlWriter writer)
        {
            writer.WriteStartElement("swf");

            writer.WriteStartElement("head");
            writer.WriteElementString("version", Version.ToString());
            writer.WriteElementString("frameWidth", _frameSize.Width.ToString());
            writer.WriteElementString("frameHeight", _frameSize.Height.ToString());
            writer.WriteElementString("frameRate", _frameRate.ToString());
            writer.WriteElementString("frameCount", _frameCount.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("tags");
            writer.WriteAttributeString("count", _tags.Count.ToString());
            foreach (var tag in _tags)
                tag.DumpXml(writer);

            writer.WriteEndElement();

            writer.WriteEndElement();
        }

        public void Dump(string path)
        {
            using (var xml = XmlWriter.Create(path, new XmlWriterSettings
                                                    	{
                                                    		Indent = true,
                                                    		IndentChars = "  ",
                                                    		Encoding = Encoding.UTF8
                                                    	}))
            {
                xml.WriteStartDocument();
                Dump(xml);
                xml.WriteEndDocument();
            }
        }

        public void Dump(TextWriter writer)
        {
            using (var xml = XmlWriter.Create(writer, new XmlWriterSettings
                                                      	{
                                                      		Indent = true,
                                                      		IndentChars = "  ",
                                                      		Encoding = Encoding.UTF8
                                                      	}))
            {
                xml.WriteStartDocument();
                Dump(xml);
                xml.WriteEndDocument();
            }
        }
        #endregion
        #endregion

        #region AbcUtils
        /// <summary>
        /// Gets all ABC files defined in this movie.
        /// </summary>
        /// <returns>list of all defined ABC files</returns>
        public List<AbcFile> GetAbcFiles()
        {
            if (_abclist == null)
            {
                _abclist = new List<AbcFile>();
                _abccache = new Hashtable();
                foreach (var tag in _tags)
                {
                    var t = tag as SwfTagDoAbc;
                    if (t == null) continue;
                    var abc = t.ByteCode;
                    abc.SWF = this;
                    abc.SWC = SWC;
                    if (string.IsNullOrEmpty(abc.Name))
                        abc.Name = "frame" + (_abclist.Count + 1);
                    abc.Index = _abclist.Count;
                    _abclist.Add(abc);
                    _abccache[abc.Name] = abc;
                }
            }
            return _abclist;
        }
        List<AbcFile> _abclist;
        Hashtable _abccache;

        public AbcFile FindAbc(string name)
        {
            GetAbcFiles();
            var abc = (AbcFile)_abccache[name];
            if (abc != null) return abc;
            string name2 = name.Replace('.', '/');
            name2 = name2.Replace(':', '/');
            abc = (AbcFile)_abccache[name2];
            return abc;
        }

        public static List<byte[]> GetRawAbcFiles(Stream input)
        {
            var swf = new SwfMovie();
            swf.Load(input, SwfTagDecodeOptions.Fast);
            return swf.GetRawAbcFiles();
        }

        public List<byte[]> GetRawAbcFiles()
        {
            var list = new List<byte[]>();

            int n = Tags.Count;
            for (int i = 0; i < n; ++i)
            {
                var tag = Tags[i];
                if (tag.TagCode == SwfTagCode.DoAbc)
                {
                    var data = tag.GetData();
                    list.Add(data);
                }
                else if (tag.TagCode == SwfTagCode.DoAbc2)
                {
                    var data = tag.GetData();
                    using (var reader = new SwfReader(data))
                    {
                        reader.FileVersion = Version;
                        reader.ReadUInt32(); //flags
                        reader.ReadString(); //frame name
                        var abc = reader.ReadToEnd();
                        list.Add(abc);
                    }
                }
            }
            return list;
        }
        #endregion

        #region Assets
        public SwfAsset FindAsset(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;
            LoadAssetCache();
            SwfAsset asset;
            if (_assetCache.TryGetValue(name, out asset))
                return asset;
            return null;
        }

        private void LoadAssetCache()
        {
            if (_assetCache != null) return;

            _assetCache = new Dictionary<string, SwfAsset>();

            foreach (var tag in _tags)
            {
                var symTable = tag as SwfTagSymbolClass;
                if (symTable != null)
                {
                    CacheAssets(symTable.Symbols, false);
                    continue;
                }

                var export = tag as SwfTagExportAssets;
                if (export != null)
                {
                    CacheAssets(export.Assets, true);
                }
            }
        }

        private void CacheAssets(IEnumerable<SwfAsset> assets, bool export)
        {
            foreach (var asset in assets)
            {
                var ch = GetCharacter(asset.Id);
                if (ch != null)
                {
                    asset.Character = ch;
                    _assetCache[asset.Name] = asset;
                    if (export)
                        ch.Name = asset.Name;
                }
            }
        }

        private Dictionary<string, SwfAsset> _assetCache;

        private void LinkAsset(SwfAsset a)
        {
            if (a.Character == null)
            {
                var ch = GetCharacter(a.Id);
                if (ch != null)
                {
                    a.Character = ch;
                    if (a.IsExported)
                    {
                        ch.Name = a.Name;
                    }
                    else if (a.IsSymbol)
                    {
                        if (string.IsNullOrEmpty(ch.Name))
                            ch.Name = a.Name;
                    }
                }
            }
        }

        private void LinkAssets(IEnumerable<SwfAsset> set)
        {
            foreach (var asset in set)
                LinkAsset(asset);
        }

        public IEnumerable<SwfAsset> GetImportAssets()
        {
            foreach (var tag in _tags)
            {
                var import = tag as SwfTagImportAssets;
                if (import != null)
                {
                    foreach (var a in import.Assets)
                    {
                        LinkAsset(a);
                        yield return a;
                    }
                }
            }
        }

        public IEnumerable<SwfAsset> GetExportAssets()
        {
            foreach (var tag in _tags)
            {
                var export = tag as SwfTagExportAssets;
                if (export != null)
                {
                    foreach (var a in export.Assets)
                    {
                        LinkAsset(a);
                        yield return a;
                    }
                }
            }
        }

        public IEnumerable<SwfAsset> GetSymbolAssets()
        {
            foreach (var tag in _tags)
            {
                var symbolTable = tag as SwfTagSymbolClass;
                if (symbolTable != null)
                {
                    foreach (var a in symbolTable.Symbols)
                    {
                        LinkAsset(a);
                        yield return a;
                    }
                }
            }
        }

        public void LinkAssets()
        {
            foreach (var tag in _tags)
            {
                tag.Swf = this;

                var export = tag as SwfTagExportAssets;
                if (export != null)
                {
                    LinkAssets(export.Assets);
                    continue;
                }

                var symbolTable = tag as SwfTagSymbolClass;
                if (symbolTable != null)
                {
                    LinkAssets(symbolTable.Symbols);
                    continue;
                }

                var import = tag as SwfTagImportAssets;
                if (import != null)
                {
                    LinkAssets(import.Assets);
                }
            }
        }
        #endregion

        #region Import
        //private readonly Hashtable _importMap = new Hashtable();
        private readonly Stack<SwfSprite> _spriteStack = new Stack<SwfSprite>();

        private void AddImport(SwfTag tag, SwfTag mytag)
        {
            tag.ImportedTag = mytag;
            //_importMap[tag] = mytag;

            //NOTE: Definition tags (such as DefineShape) are not allowed in the DefineSprite tag. All of the
            //characters that control tags refer to in the sprite must be defined in the main body of the file
            //before the sprite is defined.
            if (_spriteStack.Count > 0 && !SwfTag.IsCharacter(mytag.TagCode))
            {
                var sprite = _spriteStack.Peek();
                sprite.Tags.Add(mytag);
            }
            else
            {
                Tags.Add(mytag);
            }
        }

        private static SwfTag GetImportedTag(SwfTag tag)
        {
            //return _importMap[tag] as SwfTag;
            return tag.ImportedTag;
        }

        public SwfTag Import(SwfMovie from, SwfTag tag)
        {
            if (from == null)
                throw new ArgumentNullException("from");
            if (tag == null)
                throw new ArgumentNullException("tag");

            var mytag = GetImportedTag(tag);
            if (mytag != null)
            {
                Debug.Assert(Tags.Contains(mytag));
                return mytag;
            }

            if (tag.TagCode == SwfTagCode.DefineSprite)
            {
                var sprite = tag as SwfSprite;
                if (sprite == null)
                    throw new InvalidOperationException();
                return ImportSprite(from, sprite);
            }

            mytag = tag.Clone();
            mytag.ImportDependencies(from, this);
            
            var obj = mytag as ISwfCharacter;
            if (obj != null)
            {
                obj.CharacterID = NewCharacterID();
            }

            AddImport(tag, mytag);

            return mytag;
        }

        public SwfSprite ImportSprite(SwfMovie from, SwfSprite sprite)
        {
            var mysprite = GetImportedTag(sprite) as SwfSprite;
            if (mysprite != null)
                return mysprite;

        	mysprite = new SwfSprite {FrameCount = sprite.FrameCount};

        	_spriteStack.Push(mysprite);
            foreach (var tag in sprite.Tags)
                Import(from, tag);
            _spriteStack.Pop();

            mysprite.CharacterID = NewCharacterID();
            AddImport(sprite, mysprite);

            return mysprite;
        }

        private static Exception UnableToImportChar(ushort id)
        {
            return new InvalidOperationException(string.Format("Unable to import char with ID = {0}", id));
        }

        public void ImportCharacter(SwfMovie from, ref ushort id)
        {
            var c = from.GetCharacter(id);
            if (c == null)
                throw UnableToImportChar(id);
            c = Import(from, c as SwfTag) as ISwfCharacter;
            if (c == null)
                throw UnableToImportChar(id);
            id = c.CharacterID;
        }
        #endregion

        #region Object Overrides
        public override string ToString()
        {
            return _name;
        }
        #endregion
    }
}