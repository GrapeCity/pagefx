using System;
using System.Globalization;

namespace DataDynamics.PageFX.CLI.Metadata
{
    /// <summary>
    /// Represents cell in MDB table.
    /// </summary>
    public struct MdbCell
    {
        #region Constructors
        internal MdbCell(MdbColumn column, uint value)
        {
            _column = column;
            _value = value;
            _data = null;
        }

        internal MdbCell(MdbColumn column, uint value, object data)
        {
            _column = column;
            _value = value;
            _data = data;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the column of this cell.
        /// </summary>
        public MdbColumn Column
        {
            get { return _column; }
        }
        readonly MdbColumn _column;

        /// <summary>
        /// Gets the name of this cell.
        /// </summary>
        public string Name
        {
            get { return _column.Name; }
        }

        /// <summary>
        /// Gets the value of this cell.
        /// </summary>
        public uint Value
        {
            get { return _value; }
        }
        readonly uint _value;

        /// <summary>
        /// Gets the index value.
        /// </summary>
        public int Index
        {
            get
            {
                switch(_column.Type)
                {
                    case MdbColumnType.SimpleIndex:
                    case MdbColumnType.CodedIndex:
                        {
                            MdbIndex i = _value;
                            return i.Index;
                        }

                    default:
                        return (int)_value;
                }
            }
        }

        /// <summary>
        /// Gets the text presentation of column value
        /// </summary>
        public string ValueString
        {
            get
            {
                if (_column.EnumType != null)
                {
                    var val = Enum.ToObject(_column.EnumType, _value);
                    return val.ToString();
                }

                switch (_column.Type)
                {
                    case MdbColumnType.StringIndex:
                        return String;

                    case MdbColumnType.BlobIndex:
                        return string.Format("BLOB({0})", _value);

                    case MdbColumnType.GuidIndex:
                        return Guid.ToString();

                    case MdbColumnType.SimpleIndex:
                    case MdbColumnType.CodedIndex:
                        {
                            MdbIndex i = _value;
                            return string.Format("Index({0},{1})", i.Table, i.Index);
                        }
                }
                return _value.ToString();
            }
        }
        #endregion

        #region Data
        public object Data
        {
            get { return _data; }
        }
        readonly object _data;

        public string String
        {
            get { return _data as string; }
        }

        public byte[] Blob
        {
            get { return _data as byte[]; }
        }

        public Guid Guid
        {
            get
            {
                if (_data is Guid)
                    return (Guid)_data;
                return Guid.Empty;
            }
        }

        public CultureInfo Culture
        {
            get
            {
                string name = String;
                if (string.IsNullOrEmpty(name))
                    return CultureInfo.InvariantCulture;
                return CultureInfo.CreateSpecificCulture(name);
            }
        }
        #endregion

        #region Object Override Members
        public override string ToString()
        {
            return string.Format("{0} = {1}", Name, ValueString);
        }
        #endregion
    }
}