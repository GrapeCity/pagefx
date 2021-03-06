using System;
using System.Globalization;
using System.IO;
using DataDynamics.PageFX.Common.IO;

namespace DataDynamics.PageFX.Core.Metadata
{
    /// <summary>
    /// Represents cell in metadata table.
    /// </summary>
    internal struct MetadataCell
    {
		private readonly MetadataReader _metadata;

	    internal MetadataCell(MetadataReader metadata, MetadataColumn column, uint value) : this()
	    {
		    _metadata = metadata;
            Column = column;
            Value = value;
        }

        /// <summary>
	    /// Gets the column of this cell.
	    /// </summary>
	    public MetadataColumn Column { get; private set; }

	    /// <summary>
        /// Gets the name of this cell.
        /// </summary>
        public string Name
        {
            get { return Column.Name; }
        }

	    /// <summary>
	    /// Gets the value of this cell.
	    /// </summary>
	    public uint Value { get; private set; }

	    /// <summary>
        /// Gets the index value.
        /// </summary>
        public int Index
        {
            get
            {
                switch(Column.Type)
                {
                    case ColumnType.SimpleIndex:
                    case ColumnType.CodedIndex:
                        {
                            SimpleIndex i = Value;
                            return i.Index;
                        }

                    default:
                        return (int)Value;
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
                if (Column.EnumType != null)
                {
                    var val = Enum.ToObject(Column.EnumType, Value);
                    return val.ToString();
                }

                switch (Column.Type)
                {
                    case ColumnType.StringIndex:
                        return String;

                    case ColumnType.BlobIndex:
                        return string.Format("BLOB({0})", Value);

                    case ColumnType.GuidIndex:
                        return Guid.ToString();

                    case ColumnType.SimpleIndex:
                    case ColumnType.CodedIndex:
                        {
                            SimpleIndex i = Value;
                            return string.Format("Index({0},{1})", i.Table, i.Index);
                        }
                }
                return Value.ToString();
            }
        }

	    public object Data
	    {
		    get
		    {
			    switch (Column.Type)
			    {
				    case ColumnType.StringIndex:
					    return _metadata.FetchString(Value);

				    case ColumnType.BlobIndex:
					    return _metadata.FetchBlob(Value);

				    case ColumnType.GuidIndex:
					    return _metadata.FetchGuid(Value);

				    default:
					    return null;
			    }
		    }
	    }

		public string String
        {
            get { return Data as string; }
        }

        public BufferedBinaryReader Blob
        {
            get
            {
				var reader = Data as BufferedBinaryReader;
				if (reader != null)
				{
					reader.Seek(0, SeekOrigin.Begin);
				}
	            return reader;
            }
        }

        public Guid Guid
        {
            get { return Data is Guid ? (Guid)Data : Guid.Empty; }
        }

        public CultureInfo Culture
        {
	        get
	        {
		        try
		        {
			        string name = String;
			        return string.IsNullOrEmpty(name) ? CultureInfo.InvariantCulture : CultureInfo.CreateSpecificCulture(name);
		        }
		        catch (Exception)
		        {
			        return CultureInfo.InvariantCulture;
		        }
	        }
        }

	    public override string ToString()
        {
            return string.Format("{0} = {1}", Name, ValueString);
        }
    }
}