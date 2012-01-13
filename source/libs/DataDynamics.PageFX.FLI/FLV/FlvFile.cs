using System;
using System.IO;
using DataDynamics.PageFX.FLI.SWF;

namespace DataDynamics.PageFX.FLI.FLV
{
    //FLV files, unlike SWF files, store multibyte integers in big-endian byte order. This means
    //that, for example, the number 300 (0x12C) as a UI16 in SWF file format is represented
    //by the byte sequence 0x2C 0x01, while as a UI16 in FLV file format, it is represented by
    //the byte sequence 0x01 0x2C. Also note that FLV uses a 3-byte integer type, UI24, that
    //is not used in SWF.

    public class FlvFile
    {
        #region Header
        public int Version
        {
            get { return _version; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value");
                _version = value;
            }
        }
        private int _version = 1;

        public FlvFileFlags Flags
        {
            get { return _flags; }
        }
        private FlvFileFlags _flags;
        #endregion

        public FlvTagList Tags
        {
            get { return _tags; }
        }
        private readonly FlvTagList _tags = new FlvTagList();

        #region IO
        #region Reading
        public void Load(string path)
        {
            using (var fs = File.OpenRead(path))
                Load(fs);
        }

        public void Load(Stream input)
        {
            if (input == null)
                throw new ArgumentNullException("input");
            Load(new SwfReader(input));
        }

        public void Load(SwfReader reader)
        {
            //header
            string sig = reader.ReadASCII(3);
            if (sig != "FLV")
                throw new BadFormatException("Not a valid Flash Video file signature");

            _version = reader.ReadUInt8();

            _flags = (FlvFileFlags)reader.ReadUInt8();

            uint dataOffset = reader.ReadUInt32BE();

            reader.Position = dataOffset;

            _tags.Read(reader);
        }
        #endregion

        #region Writing
        public void Save(string path)
        {
            using (var fs = File.OpenWrite(path))
                Save(fs);
        }

        public void Save(Stream output)
        {
            if (output == null)
                throw new ArgumentNullException("output");
            Save(new SwfWriter(output));
        }

        public void Save(SwfWriter writer)
        {
            //header
            writer.WriteUInt8((byte)'F');
            writer.WriteUInt8((byte)'L');
            writer.WriteUInt8((byte)'V');
            writer.WriteUInt8((byte)_version);
            writer.WriteUInt8((byte)_flags);
            writer.WriteUInt8(9); //data offset

            //tags
            _tags.Write(writer);
        }
        #endregion
        #endregion
    }
}