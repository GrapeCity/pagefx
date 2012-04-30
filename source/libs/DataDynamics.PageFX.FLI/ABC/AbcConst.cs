using System;
using System.Text;
using DataDynamics.PageFX.FLI.SWF;

namespace DataDynamics.PageFX.FLI.ABC
{
    public sealed class AbcConst<T> : IAbcConst
    {
        public AbcConst()
        {
        }

        public AbcConst(T value)
        {
            Value = value;
        }

    	internal static AbcConstKind SharedKind
    	{
    		get
    		{
				switch (Type.GetTypeCode(typeof(T)))
				{
					case TypeCode.Int32:
						return AbcConstKind.Int;

					case TypeCode.UInt32:
						return AbcConstKind.UInt;

					case TypeCode.Double:
						return AbcConstKind.Double;

					case TypeCode.String:
						return AbcConstKind.String;

					default:
						return AbcConstKind.Undefined;
				}
    		}
    	}

    	internal static bool IsString
    	{
			get { return SharedKind == AbcConstKind.String; }
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

    	public T Value { get; set; }

    	object IAbcConst.Value
        {
            get { return Value; }
            set { Value = (T)value; }
        }

        public string Key
        {
            get { return Equals(Value, null) ? "" : Value.ToString(); }
        }

        /// <summary>
        /// Gets the size of this constant in bytes
        /// </summary>
        public int Size
        {
            get
            {
            	switch (Type.GetTypeCode(typeof(T)))
                {
                    case TypeCode.Int32:
                        return SwfWriter.SizeOfIntEncoded((int)(object)Value);

                    case TypeCode.UInt32:
                        return SwfWriter.SizeOfUIntEncoded((uint)(object)Value);

                    case TypeCode.Double:
                        return 8;

					case TypeCode.String:
                		{
                			var s = (string)(object)Value;
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
            Value = reader.ReadAbcConst<T>(SharedKind);
        }

        public void Write(SwfWriter writer)
        {
            writer.WriteAbcConst(Value);
        }
        #endregion

        #region Object Override Methods
        public override string ToString()
        {
            //string s = (object)_value as string;
            //if (s != null) return Escaper.Escape(s);
            //return string.Format("{0}: {1}", _index, _value);
            return string.Format("{0}", Value);
        }

        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var c = obj as AbcConst<T>;
            if (c != null)
                return Equals(c.Value, Value);
            return false;
        }

        private static readonly int HashSalt = typeof(T).GetHashCode();

        public override int GetHashCode()
        {
            int h = HashSalt;
            object obj = Value;
            if (obj != null)
                h ^= obj.GetHashCode();
            return h;
        }
        #endregion
    }
}