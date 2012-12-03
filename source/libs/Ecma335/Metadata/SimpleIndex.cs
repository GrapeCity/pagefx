using System;

namespace DataDynamics.PageFX.Ecma335.Metadata
{
    public struct SimpleIndex : IEquatable<SimpleIndex>
    {
        private uint _value;
        
        public SimpleIndex(TableId table, int index)
        {
            _value = ((uint)table << 24) | (uint)index;
        }

        public SimpleIndex(uint value)
        {
            _value = value;
        }

        public SimpleIndex(int value)
        {
            _value = (uint)value;
        }

        public TableId Table
        {
            get { return (TableId)(_value >> 24); }
            set { _value |= ((uint)value & 0xFF) << 24; }
        }

        public int Index
        {
            get { return (int)(_value & 0xFFFFFF); }
            set { _value |= (uint)(value & 0xFFFFFF); }
        }

        public static implicit operator int(SimpleIndex i)
        {
            return (int)i._value;
        }

        public static implicit operator uint (SimpleIndex i)
        {
            return i._value;
        }

        public static implicit operator SimpleIndex(int value)
        {
            return new SimpleIndex(value);
        }

        public static implicit operator SimpleIndex(uint value)
        {
            return new SimpleIndex(value);
        }

        public static bool operator !=(SimpleIndex i1, SimpleIndex i2)
        {
            return !i1.Equals(i2);
        }

        public static bool operator ==(SimpleIndex a, SimpleIndex b)
        {
            return a.Equals(b);
        }

        public bool Equals(SimpleIndex i)
        {
            return _value == i._value;
        }

        public override bool Equals(object obj)
        {
            return obj is SimpleIndex && Equals((SimpleIndex)obj);
        }

        public override int GetHashCode()
        {
            return (int)_value;
        }

        public override string ToString()
        {
            return string.Format("{0}[{1}]", Table, Index);
        }

        public static int MakeToken(TableId table, int index)
        {
            return (int)(((uint)table << 24) | (uint)index);
        }
    }
}