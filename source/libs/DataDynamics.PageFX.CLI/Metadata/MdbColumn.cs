using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DataDynamics.PageFX.CLI.Metadata
{
    #region enum MdbColumnType
    /// <summary>
    /// Enumerates MDB column types.
    /// </summary>
    public enum MdbColumnType
    {
        /// <summary>
        /// 4 byte constant.
        /// </summary>
        Int32,

        /// <summary>
        /// 2-byte constant.
        /// </summary>
        Int16,

        /// <summary>
        /// Index in strings heap
        /// </summary>
        StringIndex,

        /// <summary>
        /// Index in blob heap
        /// </summary>
        BlobIndex,

        /// <summary>
        /// Index in #GUID heap
        /// </summary>
        GuidIndex,

        /// <summary>
        /// Simple table index.
        /// </summary>
        SimpleIndex,

        /// <summary>
        /// Coded table index.
        /// </summary>
        CodedIndex
    }
    #endregion

    #region class MdbColumn
    /// <summary>
    /// Represents Metadata Table Column.
    /// </summary>
    public sealed class MdbColumn : ICloneable
    {
        #region Fields
        private MdbCodedIndex _codedIndex;
        private string _desc;
        private readonly int _index;
        private string _name;
        private int _offset;
        private MdbTableId _simpleIndex;
        private int _size;
        private MdbTableId _tableId;
        private MdbColumnType _type;
        private Type _enumType;
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the table id of this column.
        /// </summary>
        public MdbTableId TableId
        {
            get { return _tableId; }
            internal set { _tableId = value; }
        }

        /// <summary>
        /// Gets the column name.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Gets the column index.
        /// </summary>
        public int Index
        {
            get { return _index; }
        }

        /// <summary>
        /// Gets the column description.
        /// </summary>
        public string Description
        {
            get { return _desc; }
        }

        /// <summary>
        /// Gets the column type.
        /// </summary>
        public MdbColumnType Type
        {
            get { return _type; }
        }

        /// <summary>
        /// Gets the simple index.
        /// </summary>
        public MdbTableId SimpleIndex
        {
            get { return _simpleIndex; }
        }

        /// <summary>
        /// Gets the coded index.
        /// </summary>
        public MdbCodedIndex CodedIndex
        {
            get { return _codedIndex; }
        }

        /// <summary>
        /// Gets the enum type for this column.
        /// </summary>
        public Type EnumType
        {
            get { return _enumType; }
        }
        #endregion

        #region Constructors
        internal MdbColumn()
        {
        }

        internal MdbColumn(int index, string name, MdbColumnType type, string desc)
        {
            _index = index;
            _name = name;
            _desc = desc;
            _type = type;
        }

        internal MdbColumn(int index, string name, MdbColumnType type, Type enumType, string desc)
        {
            _index = index;
            _name = name;
            _desc = desc;
            _type = type;
            _enumType = enumType;
        }

        internal MdbColumn(int index, string name, MdbTableId simpleIndex, string desc)
        {
            _index = index;
            _name = name;
            _desc = desc;
            _type = MdbColumnType.SimpleIndex;
            _simpleIndex = simpleIndex;
        }

        internal MdbColumn(int index, string name, MdbCodedIndex codedIndex, string desc)
        {
            _index = index;
            _name = name;
            _desc = desc;
            _codedIndex = codedIndex;
            _type = MdbColumnType.CodedIndex;
        }
        #endregion

        #region ICloneable Members
        object ICloneable.Clone()
        {
            return Clone();
        }
        #endregion

        #region Public Members
        public MdbColumn Clone()
        {
            var c = new MdbColumn();
            c._tableId = _tableId;
            c._name = _name;
            c._desc = _desc;
            c._type = _type;
            c._enumType = _enumType;
            c._codedIndex = _codedIndex;
            c._simpleIndex = _simpleIndex;
            return c;
        }

        /// <summary>
        /// Gets the column type name.
        /// </summary>
        public string TypeName
        {
            get
            {
                if (_enumType != null)
                    return _enumType.Name;
                switch (_type)
                {
                    case MdbColumnType.SimpleIndex:
                        return _simpleIndex.ToString();

                    case MdbColumnType.CodedIndex:
                        return _codedIndex.ToString();
                }
                return _type.ToString();
            }
        }

        public override string ToString()
        {
            var s = new StringBuilder();
            s.AppendFormat("Column({0} : {1}", _name, TypeName);
            if (_size > 0)
            {
                s.AppendFormat("[{0}, {1}, {2}]", _index, _offset, _size);
            }
            s.Append(")");
            return s.ToString();
        }
        #endregion
    }
    #endregion

    #region class MdbColumnList
    /// <summary>
    /// List of <see cref="MdbColumn"/>s.
    /// </summary>
    public sealed class MdbColumnList : IEnumerable<MdbColumn>
    {
        private readonly Hashtable _hash = new Hashtable();
        private readonly List<MdbColumn> _list = new List<MdbColumn>();

        /// <summary>
        /// Gets the number of columns.
        /// </summary>
        public int Count
        {
            get { return _list.Count; }
        }

        public MdbColumn this[int index]
        {
            get { return _list[index]; }
        }

        public MdbColumn this[string name]
        {
            get { return (MdbColumn)_hash[name]; }
        }

        #region IEnumerable<MdbColumn> Members
        public IEnumerator<MdbColumn> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
        #endregion

        internal void Add(MdbColumn col)
        {
            if (this[col.Name] != null)
                throw new ArgumentException(string.Format("Column with name {0} already exists", col.Name));
            _list.Add(col);
            _hash[col.Name] = col;
        }

        public override string ToString()
        {
            var s = new StringBuilder();
            s.Append("Columns(");
            for (int i = 0; i < _list.Count; ++i)
            {
                if (i > 0) s.Append(", ");
                s.Append(_list[i].Name);
            }
            s.Append(")");
            return base.ToString();
        }
    }
    #endregion   
}