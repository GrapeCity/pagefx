using System;
using System.Text;
using DataDynamics.PageFX.FLI.SWF;

namespace DataDynamics.PageFX.FLI.ABC
{
    public class AbcConst<T> : IAbcConst
    {
        public AbcConst()
        {
        }

        public AbcConst(T value)
        {
            _value = value;
        }

        internal static readonly AbcConstKind SharedKind;
        internal static readonly bool IsString;
        internal static readonly AbcConst<T> Default;

        static AbcConst()
        {
            switch (Type.GetTypeCode(typeof(T)))
            {
                case TypeCode.Int32:
                    SharedKind = AbcConstKind.Int;
                    Default = new AbcConst<T>(default(T)) {Index = 0};
                    break;

                case TypeCode.UInt32:
                    SharedKind = AbcConstKind.UInt;
                    Default = new AbcConst<T>(default(T)) {Index = 0};
                    break;

                case TypeCode.Double:
                    SharedKind = AbcConstKind.Double;
                    Default = new AbcConst<T>(default(T)) {Index = 0};
                    break;

                case TypeCode.String:
                    IsString = true;
                    SharedKind = AbcConstKind.String;
                    Default = new AbcConst<T>((T)(object)null) {Index = 0};
                    break;

                default:
                    SharedKind = AbcConstKind.Undefined;
                    break;
            }
        }

        public AbcConstKind Kind
        {
            get { return SharedKind; }
        }

        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }
        int _index = -1;

        public T Value
        {
            get { return _value; }
            set { _value = value; }
        }
        T _value;

        object IAbcConst.Value
        {
            get { return _value; }
            set { _value = (T)value; }
        }

        public string Key
        {
            get
            {
                if (_value == null) return "";
                return _value.ToString();
            }
        }

        /// <summary>
        /// Gets the size of this constant in bytes
        /// </summary>
        public int Size
        {
            get
            {
                var tc = Type.GetTypeCode(typeof(T));
                switch (tc)
                {
                    case TypeCode.Int32:
                        return SwfWriter.SizeOfIntEncoded((int)(object)_value);

                    case TypeCode.UInt32:
                        return SwfWriter.SizeOfUIntEncoded((uint)(object)_value);

                    case TypeCode.Double:
                        return 8;

                    case TypeCode.String:
                        {
                            string s = (string)(object)_value;
                            if (s == null) return 1;
                            return SwfWriter.SizeOfUIntEncoded((uint)s.Length)
                                + Encoding.UTF8.GetByteCount(s);
                        }
                }
                return 0;
            }
        }

        #region IAbcAtom Members
        public void Read(SwfReader reader)
        {
            _value = AbcIO.ReadConst<T>(reader, SharedKind);
        }

        public void Write(SwfWriter writer)
        {
            AbcIO.WriteConst(writer, _value);
        }
        #endregion

        #region Object Override Methods
        public override string ToString()
        {
            //string s = (object)_value as string;
            //if (s != null) return Escaper.Escape(s);
            //return string.Format("{0}: {1}", _index, _value);
            return string.Format("{0}", _value);
        }

        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var c = obj as AbcConst<T>;
            if (c != null)
                return Equals(c._value, _value);
            return false;
        }

        private static readonly int hs = typeof(T).GetHashCode();

        public override int GetHashCode()
        {
            int h = hs;
            object obj = _value;
            if (obj != null)
                h ^= obj.GetHashCode();
            return h;
        }
        #endregion
    }
}