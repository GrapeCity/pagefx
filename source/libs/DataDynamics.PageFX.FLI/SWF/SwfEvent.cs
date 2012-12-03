using System;
using System.Collections.Generic;
using DataDynamics.PageFX.FlashLand.Swf.Actions;

namespace DataDynamics.PageFX.FlashLand.Swf
{
    public class SwfEvent
    {
        public SwfEventFlags Flags
        {
            get { return _flags; }
            set { _flags = value; }
        }
        private SwfEventFlags _flags;

        public byte KeyCode
        {
            get { return _keyCode; }
            set { _keyCode = value; }
        }
        private byte _keyCode;

        public SwfActionList Actions
        {
            get { return _actions; }
        }
        private readonly SwfActionList _actions = new SwfActionList();

        public static SwfEventFlags ToFlags(uint v, int ver)
        {
            return 0;
        }

        public static uint ToUInt32(SwfEventFlags f, int ver)
        {
            return (uint)f;
        }

        public static SwfEventFlags ReadFlags(SwfReader reader)
        {
        	int ver = reader.FileVersion;
            uint v = ver >= 6 ? reader.ReadUInt32() : reader.ReadUInt16();
            return ToFlags(v, ver);
        }

        public static void WriteFlags(SwfWriter writer, SwfEventFlags f)
        {
            int ver = writer.FileVersion;
            uint v = ToUInt32(f, ver);
            if (ver >= 6)
                writer.WriteUInt32(v);
            else 
                writer.WriteUInt16((ushort)v);
        }

        //WARNING: Reads without event flags
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        public void Read(SwfReader reader)
        {
            uint len = reader.ReadUInt32();
            if ((_flags & SwfEventFlags.KeyPress) != 0)
            {
                _keyCode = reader.ReadUInt8();
            }
            //TODO: use len
            _actions.Read(reader);
        }

        public void Write(SwfWriter writer)
        {
            WriteFlags(writer, _flags);

        	var eventWriter = new SwfWriter {FileVersion = writer.FileVersion};
        	if ((_flags & SwfEventFlags.KeyPress) != 0)
                eventWriter.WriteUInt8(_keyCode);
            _actions.Write(eventWriter);

            var eventData = eventWriter.ToByteArray();
            writer.WriteUInt32((uint)eventData.Length);
            writer.Write(eventData);
        }
    }

    public class SwfEventList : List<SwfEvent>
    {
        public SwfEventFlags AllEventFlags
        {
            get { return _allFlags; }
            set { _allFlags = value; }
        }
        private SwfEventFlags _allFlags;

        public void Read(SwfReader reader)
        {
            reader.ReadUInt16(); //reserved
            _allFlags = SwfEvent.ReadFlags(reader);

            int ver = reader.FileVersion;
            while (true)
            {
            	uint flags = ver >= 6 ? reader.ReadUInt32() : reader.ReadUInt16();
                if (flags == 0) break;
            	var e = new SwfEvent {Flags = SwfEvent.ToFlags(flags, ver)};
            	e.Read(reader);
            }
        }

        public void Write(SwfWriter writer)
        {
            writer.WriteUInt16(0); //reserved
            SwfEvent.WriteFlags(writer, _allFlags);

            int n = Count;
            for (int i = 0; i < n; ++i)
                this[i].Write(writer);

            if (writer.FileVersion >= 6)
                writer.WriteUInt32(0);
            else
                writer.WriteUInt16(0);
        }
    }

    [Flags]
    public enum SwfEventFlags
    {
        KeyUp = 0x00000001,
        KeyDown = 0x00000002,
        MouseUp = 0x00000004,
        MouseDown = 0x00000008,
        MouseMove = 0x00000010,
        Unload = 0x00000020,
        EnterFrame = 0x00000040,
        Load = 0x00000080,
        DragOver = 0x00000100,
        RollOut = 0x00000200,
        RollOver = 0x00000400,
        ReleaseOutside = 0x00000800,
        Release = 0x00001000,
        Press = 0x00002000,
        Initialize = 0x00004000,
        Data = 0x00008000,
        Construct = 0x00010000,
        KeyPress = 0x00020000,
        DragOut = 0x00040000,
    }
}