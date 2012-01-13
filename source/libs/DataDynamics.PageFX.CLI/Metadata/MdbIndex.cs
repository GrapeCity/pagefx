using System;

namespace DataDynamics.PageFX.CLI.Metadata
{
    public struct MdbIndex : IEquatable<MdbIndex>
    {
        private uint _value;
        
        public MdbIndex(MdbTableId table, int index)
        {
            _value = ((uint)table << 24) | (uint)index;
        }

        public MdbIndex(uint value)
        {
            _value = value;
        }

        public MdbIndex(int value)
        {
            _value = (uint)value;
        }

        public MdbTableId Table
        {
            get { return (MdbTableId)(_value >> 24); }
            set { _value |= ((uint)value & 0xFF) << 24; }
        }

        public int Index
        {
            get { return (int)(_value & 0xFFFFFF); }
            set { _value |= (uint)(value & 0xFFFFFF); }
        }

        public static implicit operator int(MdbIndex i)
        {
            return (int)i._value;
        }

        public static implicit operator uint (MdbIndex i)
        {
            return i._value;
        }

        public static implicit operator MdbIndex(int value)
        {
            return new MdbIndex(value);
        }

        public static implicit operator MdbIndex(uint value)
        {
            return new MdbIndex(value);
        }

        public static bool operator !=(MdbIndex i1, MdbIndex i2)
        {
            return !i1.Equals(i2);
        }

        public static bool operator ==(MdbIndex a, MdbIndex b)
        {
            return a.Equals(b);
        }

        public bool Equals(MdbIndex i)
        {
            return _value == i._value;
        }

        public override bool Equals(object obj)
        {
            return obj is MdbIndex ? Equals((MdbIndex)obj) : false;
        }

        public override int GetHashCode()
        {
            return (int)_value;
        }

        public override string ToString()
        {
            return string.Format("{0}[{1}]", Table, Index);
        }

        public static int MakeToken(MdbTableId table, int index)
        {
            return (int)(((uint)table << 24) | (uint)index);
        }
    }
}