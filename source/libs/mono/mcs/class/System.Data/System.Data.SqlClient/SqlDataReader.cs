//
// System.Data.SqlClient.SqlDataReader.cs
//
// Author:
//   Rodrigo Moya (rodrigo@ximian.com)
//   Daniel Morgan (danmorg@sc.rr.com)
//   Tim Coleman (tim@timcoleman.com)
//
// (C) Ximian, Inc 2002
// (C) Daniel Morgan 2002
// Copyright (C) Tim Coleman, 2002
//

//
// Copyright (C) 2004 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using Mono.Data.Tds.Protocol;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlTypes;

namespace System.Data.SqlClient {
#if NET_2_0
	public class SqlDataReader : DbDataReader, IDataReader, IDisposable, IDataRecord
#else
	public sealed class SqlDataReader : MarshalByRefObject, IEnumerable, IDataReader, IDisposable, IDataRecord
#endif // NET_2_0
	{
		#region Fields

		SqlCommand command;
		ArrayList dataTypeNames;
		bool disposed = false;
		int fieldCount;
		bool isClosed;
		bool moreResults;
		int resultsRead;
		int rowsRead;
		DataTable schemaTable;
		bool hasRows;
		bool haveRead;
		bool readResult;
		bool readResultUsed;
#if NET_2_0
		int visibleFieldCount;
#endif

		#endregion // Fields

		#region Constructors

		internal SqlDataReader (SqlCommand command) 
		{
			readResult = false;
			haveRead = false;
			readResultUsed = false;
			this.command = command;
			schemaTable = ConstructSchemaTable ();
			resultsRead = 0;
			fieldCount = 0;
			isClosed = false;
			command.Tds.RecordsAffected = -1;
#if NET_2_0
			visibleFieldCount = 0;
#endif
			NextResult ();
		}

		#endregion // Constructors

		#region Properties

		public 
#if NET_2_0
		override
#endif // NET_2_0
		int Depth {
			get { return 0; }
		}

		public 
#if NET_2_0
		override
#endif // NET_2_0
		int FieldCount {
			get { return fieldCount; }
		}

		public 
#if NET_2_0
		override
#endif // NET_2_0
		bool IsClosed {
			get { return isClosed; }
		}

		public 
#if NET_2_0
		override
#endif // NET_2_0
		object this [int i] {
			get { return GetValue (i); }
		}

		public 
#if NET_2_0
		override
#endif // NET_2_0
		object this [string name] {
			get { return GetValue (GetOrdinal (name)); }
		}
	
		public 
#if NET_2_0
		override
#endif // NET_2_0
		int RecordsAffected {
			get { 
				return command.Tds.RecordsAffected; 
			}
		}

		public 
#if NET_2_0
		override
#endif // NET_2_0
		bool HasRows {
			get {
				if (haveRead) 
					return readResult;
			
				haveRead = true;
				readResult = ReadRecord ();
				return readResult;
			}
		}
#if NET_2_0
		public override int VisibleFieldCount { 
			get { return visibleFieldCount; }
		}

		protected SqlConnection Connection { 
			get { return command.Connection; }
		}

		protected bool IsCommandBehavior (CommandBehavior condition) { 
			return condition == command.CommandBehavior;
		}
#endif

		#endregion // Properties

		#region Methods

		public 
#if NET_2_0
		override
#endif // NET_2_0
		void Close ()
		{
			if (IsClosed)
				return;
			// skip to end & read output parameters.
			while (NextResult ())
				;
			isClosed = true;
			command.CloseDataReader (moreResults);
		}

		private static DataTable ConstructSchemaTable ()
		{
			Type booleanType = Type.GetType ("System.Boolean");
			Type stringType = Type.GetType ("System.String");
			Type intType = Type.GetType ("System.Int32");
			Type typeType = Type.GetType ("System.Type");
			Type shortType = Type.GetType ("System.Int16");

			DataTable schemaTable = new DataTable ("SchemaTable");
			schemaTable.Columns.Add ("ColumnName", stringType);
			schemaTable.Columns.Add ("ColumnOrdinal", intType);
			schemaTable.Columns.Add ("ColumnSize", intType);
			schemaTable.Columns.Add ("NumericPrecision", shortType);
			schemaTable.Columns.Add ("NumericScale", shortType);
			schemaTable.Columns.Add ("IsUnique", booleanType);
			schemaTable.Columns.Add ("IsKey", booleanType);
			schemaTable.Columns.Add ("BaseServerName", stringType);
			schemaTable.Columns.Add ("BaseCatalogName", stringType);
			schemaTable.Columns.Add ("BaseColumnName", stringType);
			schemaTable.Columns.Add ("BaseSchemaName", stringType);
			schemaTable.Columns.Add ("BaseTableName", stringType);
			schemaTable.Columns.Add ("DataType", typeType);
			schemaTable.Columns.Add ("AllowDBNull", booleanType);
			schemaTable.Columns.Add ("ProviderType", intType);
			schemaTable.Columns.Add ("IsAliased", booleanType);
			schemaTable.Columns.Add ("IsExpression", booleanType);
			schemaTable.Columns.Add ("IsIdentity", booleanType);
			schemaTable.Columns.Add ("IsAutoIncrement", booleanType);
			schemaTable.Columns.Add ("IsRowVersion", booleanType);
			schemaTable.Columns.Add ("IsHidden", booleanType);
			schemaTable.Columns.Add ("IsLong", booleanType);
			schemaTable.Columns.Add ("IsReadOnly", booleanType);

			return schemaTable;
		}
#if NET_2_0
		new
#endif
		void Dispose (bool disposing) 
		{
			if (!disposed) {
				if (disposing) {
					schemaTable.Dispose ();
					Close ();
					command = null;
				}
				disposed = true;
			}
		}

		public 
#if NET_2_0
		override
#endif // NET_2_0
		bool GetBoolean (int i)
		{
			object value = GetValue (i);
			if (!(value is bool)) {
				if (value is DBNull) throw new SqlNullValueException ();
				throw new InvalidCastException ("Type is " + value.GetType ().ToString ());
			}
			return (bool) value;
		}

		public 
#if NET_2_0
		override
#endif // NET_2_0
		byte GetByte (int i)
		{
			object value = GetValue (i);
			if (!(value is byte)) {
				if (value is DBNull) throw new SqlNullValueException ();
				throw new InvalidCastException ("Type is " + value.GetType ().ToString ());
			}
			return (byte) value;
		}

		public 
#if NET_2_0
		override
#endif // NET_2_0
		long GetBytes (int i, long dataIndex, byte[] buffer, int bufferIndex, int length)
		{
			if ((command.CommandBehavior & CommandBehavior.SequentialAccess) != 0) {
				try {
					long len = ((Tds)command.Tds).GetSequentialColumnValue (i, dataIndex, buffer, bufferIndex, length);
					if (len == -1)
						throw new InvalidCastException ("Invalid attempt to GetBytes on column "
										+ "'" + command.Tds.Columns[i]["ColumnName"] +
										"'." + "The GetBytes function"
										+ " can only be used on columns of type Text, NText, or Image");
					return len;
				} catch (TdsInternalException ex) {
					command.Connection.Close ();
					throw SqlException.FromTdsInternalException ((TdsInternalException) ex);
				}
			}

			object value = GetValue (i);
			if (!(value is byte [])) {
				if (value is DBNull) throw new SqlNullValueException ();
				throw new InvalidCastException ("Type is " + value.GetType ().ToString ());
			}
			
			if ( buffer == null )
				return ((byte []) value).Length; // Return length of data

			// Copy data into buffer
			int availLen = (int) ( ( (byte []) value).Length - dataIndex);
			if (availLen < length)
				length = availLen;
			Array.Copy ((byte []) value, (int) dataIndex, buffer, bufferIndex, length);
			return length; // return actual read count
		}

		[EditorBrowsableAttribute (EditorBrowsableState.Never)]
		public 
#if NET_2_0
		override
#endif // NET_2_0
		char GetChar (int i)
		{
			object value = GetValue (i);
			if (!(value is char)) {
				if (value is DBNull) throw new SqlNullValueException ();
				throw new InvalidCastException ("Type is " + value.GetType ().ToString ());
			}
			return (char) value;
		}

		public 
#if NET_2_0
		override
#endif // NET_2_0
		long GetChars (int i, long dataIndex, char[] buffer, int bufferIndex, int length)
		{
			if ((command.CommandBehavior & CommandBehavior.SequentialAccess) != 0) {
				Encoding encoding = null;
				byte mul = 1;
				TdsColumnType colType = (TdsColumnType) command.Tds.Columns[i]["ColumnType"];
				switch (colType) {
					case TdsColumnType.Text :
					case TdsColumnType.VarChar:
					case TdsColumnType.Char:
					case TdsColumnType.BigVarChar:
						encoding = Encoding.ASCII;
						break;
					case TdsColumnType.NText :
					case TdsColumnType.NVarChar:
					case TdsColumnType.NChar:
						encoding = Encoding.Unicode;
						mul = 2 ;
						break;
					default :
						return -1;
				}

				long count = 0;
				if (buffer == null) {
					count = GetBytes (i,0,(byte[]) null,0,0);
					return (count/mul);
				}

				length *= mul;
				byte[] arr = new byte [length];
				count = GetBytes (i, dataIndex, arr, 0, length);
				if (count == -1)
					throw new InvalidCastException ("Specified cast is not valid");

				Char[] val = encoding.GetChars (arr, 0, (int)count);
				val.CopyTo (buffer, bufferIndex);
				return val.Length;
			}

			char [] valueBuffer;
			object value = GetValue (i);
			
			if (value is char[])
				valueBuffer = (char[])value;
			else if (value is string)
				valueBuffer = ((string)value).ToCharArray();
			else {
				if (value is DBNull) throw new SqlNullValueException ();
				throw new InvalidCastException ("Type is " + value.GetType ().ToString ());
			}
			
			if ( buffer == null ) {
				// Return length of data
				return valueBuffer.Length;
			}
			else {
				// Copy data into buffer
				Array.Copy (valueBuffer, (int) dataIndex, buffer, bufferIndex, length);
				return valueBuffer.Length - dataIndex;
			}
		}
		
#if !NET_2_0
		[EditorBrowsableAttribute (EditorBrowsableState.Never)] 
		public new IDataReader GetData (int i)
		{
			return ((IDataReader) this [i]);
		}
#endif

		public 
#if NET_2_0
		override
#endif // NET_2_0
		string GetDataTypeName (int i)
		{
			if (i < 0 || i >= dataTypeNames.Count)
				throw new IndexOutOfRangeException ();
			return (string) dataTypeNames [i];
		}

		public 
#if NET_2_0
		override
#endif // NET_2_0
		DateTime GetDateTime (int i)
		{
			object value = GetValue (i);
			if (!(value is DateTime)) {
				if (value is DBNull) throw new SqlNullValueException ();
				else throw new InvalidCastException ("Type is " + value.GetType ().ToString ());
			}
			return (DateTime) value;
		}

		public 
#if NET_2_0
		override
#endif // NET_2_0
		decimal GetDecimal (int i)
		{
			object value = GetValue (i);
			if (!(value is decimal)) {
				if (value is DBNull) throw new SqlNullValueException ();
				throw new InvalidCastException ("Type is " + value.GetType ().ToString ());
			}
			return (decimal) value;
		}

		public 
#if NET_2_0
		override
#endif // NET_2_0
		double GetDouble (int i)
		{
			object value = GetValue (i);
			if (!(value is double)) {
				if (value is DBNull) throw new SqlNullValueException ();
				throw new InvalidCastException ("Type is " + value.GetType ().ToString ());
			}
			return (double) value;
		}

		public 
#if NET_2_0
		override
#endif // NET_2_0
		Type GetFieldType (int i)
		{
			if (i < 0 || i >= schemaTable.Rows.Count)
				throw new IndexOutOfRangeException ();
			return (Type) schemaTable.Rows[i]["DataType"];
		}

		public 
#if NET_2_0
		override
#endif // NET_2_0
		float GetFloat (int i)
		{
			object value = GetValue (i);
			if (!(value is float)) {
				if (value is DBNull) throw new SqlNullValueException ();
				throw new InvalidCastException ("Type is " + value.GetType ().ToString ());
			}
			return (float) value;
		}

		public 
#if NET_2_0
		override
#endif // NET_2_0
		Guid GetGuid (int i)
		{
			object value = GetValue (i);
			if (!(value is Guid)) {
				if (value is DBNull) throw new SqlNullValueException ();
				throw new InvalidCastException ("Type is " + value.GetType ().ToString ());
			}
			return (Guid) value;
		}

		public 
#if NET_2_0
		override
#endif // NET_2_0
		short GetInt16 (int i)
		{
			object value = GetValue (i);
			if (!(value is short)) {
				if (value is DBNull) throw new SqlNullValueException ();
				throw new InvalidCastException ("Type is " + value.GetType ().ToString ());
			}
			return (short) value;
		}

		public 
#if NET_2_0
		override
#endif // NET_2_0
		int GetInt32 (int i)
		{
			object value = GetValue (i);
			if (!(value is int)) {
				if (value is DBNull) throw new SqlNullValueException ();
				throw new InvalidCastException ("Type is " + value.GetType ().ToString ());
			}
			return (int) value;
		}

		public 
#if NET_2_0
		override
#endif // NET_2_0
		long GetInt64 (int i)
		{
			object value = GetValue (i);
			// TDS 7.0 returns bigint as decimal(19,0)
			if (value is decimal) {
				TdsDataColumn schema = command.Tds.Columns[i];
				if ((byte)schema["NumericPrecision"] == 19 && (byte)schema["NumericScale"] == 0)
					value = (long) (decimal) value;
			}
			if (!(value is long)) {
				if (value is DBNull) throw new SqlNullValueException ();
				throw new InvalidCastException ("Type is " + value.GetType ().ToString ());
			}
			return (long) value;
		}

		public 
#if NET_2_0
		override
#endif // NET_2_0
		string GetName (int i)
		{
			return (string) schemaTable.Rows[i]["ColumnName"];
		}

		public 
#if NET_2_0
		override
#endif // NET_2_0
		int GetOrdinal (string name)
		{
			foreach (DataRow schemaRow in schemaTable.Rows)
				if (((string) schemaRow ["ColumnName"]).Equals (name))
					return (int) schemaRow ["ColumnOrdinal"];
			foreach (DataRow schemaRow in schemaTable.Rows)
				if (String.Compare (((string) schemaRow ["ColumnName"]), name, true) == 0)
					return (int) schemaRow ["ColumnOrdinal"];
			throw new IndexOutOfRangeException ();
		}

		public 
#if NET_2_0
		override
#endif // NET_2_0
		DataTable GetSchemaTable ()
		{
			ValidateState ();

			if (schemaTable.Rows != null && schemaTable.Rows.Count > 0)
				return schemaTable;

			if (!moreResults)
				return null;

			fieldCount = 0;

			dataTypeNames = new ArrayList ();

			foreach (TdsDataColumn schema in command.Tds.Columns) {
				DataRow row = schemaTable.NewRow ();

				row ["ColumnName"]		= GetSchemaValue (schema, "ColumnName");
				row ["ColumnSize"]		= GetSchemaValue (schema, "ColumnSize"); 
				row ["ColumnOrdinal"]		= GetSchemaValue (schema, "ColumnOrdinal");
				row ["NumericPrecision"]	= GetSchemaValue (schema, "NumericPrecision");
				row ["NumericScale"]		= GetSchemaValue (schema, "NumericScale");
				row ["IsUnique"]		= GetSchemaValue (schema, "IsUnique");
				row ["IsKey"]			= GetSchemaValue (schema, "IsKey");
				row ["BaseServerName"]		= GetSchemaValue (schema, "BaseServerName");
				row ["BaseCatalogName"]		= GetSchemaValue (schema, "BaseCatalogName");
				row ["BaseColumnName"]		= GetSchemaValue (schema, "BaseColumnName");
				row ["BaseSchemaName"]		= GetSchemaValue (schema, "BaseSchemaName");
				row ["BaseTableName"]		= GetSchemaValue (schema, "BaseTableName");
				row ["AllowDBNull"]		= GetSchemaValue (schema, "AllowDBNull");
				row ["IsAliased"]		= GetSchemaValue (schema, "IsAliased");
				row ["IsExpression"]		= GetSchemaValue (schema, "IsExpression");
				row ["IsIdentity"]		= GetSchemaValue (schema, "IsIdentity");
				row ["IsAutoIncrement"]		= GetSchemaValue (schema, "IsAutoIncrement");
				row ["IsRowVersion"]		= GetSchemaValue (schema, "IsRowVersion");
				row ["IsHidden"]		= GetSchemaValue (schema, "IsHidden");
				row ["IsReadOnly"]		= GetSchemaValue (schema, "IsReadOnly");

				// We don't always get the base column name.
				if (row ["BaseColumnName"] == DBNull.Value)
					row ["BaseColumnName"] = row ["ColumnName"];

				switch ((TdsColumnType) schema ["ColumnType"]) {
					case TdsColumnType.Int1:
					case TdsColumnType.Int2:
					case TdsColumnType.Int4:
					case TdsColumnType.IntN:
						switch ((int) schema ["ColumnSize"]) {
						case 1:
							dataTypeNames.Add ("tinyint");
							row ["ProviderType"] = (int) SqlDbType.TinyInt;
							row ["DataType"] = typeof (byte);
							row ["IsLong"] = false;
							break;
						case 2:
							dataTypeNames.Add ("smallint");
							row ["ProviderType"] = (int) SqlDbType.SmallInt;
							row ["DataType"] = typeof (short);
							row ["IsLong"] = false;
							break;
						case 4:
							dataTypeNames.Add ("int");
							row ["ProviderType"] = (int) SqlDbType.Int;
							row ["DataType"] = typeof (int);
							row ["IsLong"] = false;
							break;
						case 8:
							dataTypeNames.Add ("bigint");
							row ["ProviderType"] = (int) SqlDbType.BigInt;
							row ["DataType"] = typeof (long);
							row ["IsLong"] = false;
							break;
						}
						break;
					case TdsColumnType.Real:
					case TdsColumnType.Float8:
					case TdsColumnType.FloatN:
						switch ((int) schema ["ColumnSize"]) {
						case 4:
							dataTypeNames.Add ("real");
							row ["ProviderType"] = (int) SqlDbType.Real;
							row ["DataType"] = typeof (float);
							row ["IsLong"] = false;
							break;
						case 8:
							dataTypeNames.Add ("float");
							row ["ProviderType"] = (int) SqlDbType.Float;
							row ["DataType"] = typeof (double);
							row ["IsLong"] = false;
							break;
						}
						break;
					case TdsColumnType.Image :
						dataTypeNames.Add ("image");
						row ["ProviderType"] = (int) SqlDbType.Image;
						row ["DataType"] = typeof (byte[]);
						row ["IsLong"] = true;
						break;
					case TdsColumnType.Text :
						dataTypeNames.Add ("text");
						row ["ProviderType"] = (int) SqlDbType.Text;
						row ["DataType"] = typeof (string);
						row ["IsLong"] = true;
						break;
					case TdsColumnType.UniqueIdentifier :
						dataTypeNames.Add ("uniqueidentifier");
						row ["ProviderType"] = (int) SqlDbType.UniqueIdentifier;
						row ["DataType"] = typeof (Guid);
						row ["IsLong"] = false;
						break;
					case TdsColumnType.VarBinary :
					case TdsColumnType.BigVarBinary :
						dataTypeNames.Add ("varbinary");
						row ["ProviderType"] = (int) SqlDbType.VarBinary;
						row ["DataType"] = typeof (byte[]);
						row ["IsLong"] = true;
						break;
					case TdsColumnType.VarChar :
					case TdsColumnType.BigVarChar :
						dataTypeNames.Add ("varchar");
						row ["ProviderType"] = (int) SqlDbType.VarChar;
						row ["DataType"] = typeof (string);
						row ["IsLong"] = false;
						break;
					case TdsColumnType.Binary :
					case TdsColumnType.BigBinary :
						dataTypeNames.Add ("binary");
						row ["ProviderType"] = (int) SqlDbType.Binary;
						row ["DataType"] = typeof (byte[]);
						row ["IsLong"] = true;
						break;
					case TdsColumnType.Char :
					case TdsColumnType.BigChar :
						dataTypeNames.Add ("char");
						row ["ProviderType"] = (int) SqlDbType.Char;
						row ["DataType"] = typeof (string);
						row ["IsLong"] = false;
						break;
					case TdsColumnType.Bit :
					case TdsColumnType.BitN :
						dataTypeNames.Add ("bit");
						row ["ProviderType"] = (int) SqlDbType.Bit;
						row ["DataType"] = typeof (bool);
						row ["IsLong"] = false;
						break;
					case TdsColumnType.DateTime4 :
					case TdsColumnType.DateTime :
					case TdsColumnType.DateTimeN :
						dataTypeNames.Add ("datetime");
						row ["ProviderType"] = (int) SqlDbType.DateTime;
						row ["DataType"] = typeof (DateTime);
						row ["IsLong"] = false;
						break;
					case TdsColumnType.Money :
					case TdsColumnType.MoneyN :
					case TdsColumnType.Money4 :
						dataTypeNames.Add ("money");
						row ["ProviderType"] = (int) SqlDbType.Money;
						row ["DataType"] = typeof (decimal);
						row ["IsLong"] = false;
						break;
					case TdsColumnType.NText :
						dataTypeNames.Add ("ntext");
						row ["ProviderType"] = (int) SqlDbType.NText;
						row ["DataType"] = typeof (string);
						row ["IsLong"] = true;
						break;
					case TdsColumnType.NVarChar :
						dataTypeNames.Add ("nvarchar");
						row ["ProviderType"] = (int) SqlDbType.NVarChar;
						row ["DataType"] = typeof (string);
						row ["IsLong"] = false;
						break;
					case TdsColumnType.Decimal :
					case TdsColumnType.Numeric :
						dataTypeNames.Add ("decimal");
						row ["ProviderType"] = (int) SqlDbType.Decimal;
						row ["DataType"] = typeof (decimal);
						row ["IsLong"] = false;
						break;
					case TdsColumnType.NChar :
						dataTypeNames.Add ("nchar");
						row ["ProviderType"] = (int) SqlDbType.NChar;
						row ["DataType"] = typeof (string);
						row ["IsLong"] = false;
						break;
					case TdsColumnType.SmallMoney :
						dataTypeNames.Add ("smallmoney");
						row ["ProviderType"] = (int) SqlDbType.SmallMoney;
						row ["DataType"] = typeof (decimal);
						row ["IsLong"] = false;
						break;
					default :
						dataTypeNames.Add ("variant");
						row ["ProviderType"] = (int) SqlDbType.Variant;
						row ["DataType"] = typeof (object);
						row ["IsLong"] = false;
						break;
				}
#if NET_2_0
				if ((bool)row ["IsHidden"] == false)
					visibleFieldCount += 1;
#endif

				schemaTable.Rows.Add (row);

				fieldCount += 1;
			}
			return schemaTable;
		}

		private static object GetSchemaValue (TdsDataColumn schema, object key)
		{
			if (schema.ContainsKey (key) && schema [key] != null)
				return schema [key];
			return DBNull.Value;
		}

		public
#if NET_2_0
		virtual
#endif
		SqlBinary GetSqlBinary (int i)
		{
			object value = GetSqlValue (i);
			if (!(value is SqlBinary))
				throw new InvalidCastException ("Type is " + value.GetType ().ToString ());
			return (SqlBinary) value;
		}

		public
#if NET_2_0
		virtual
#endif
		SqlBoolean GetSqlBoolean (int i) 
		{
			object value = GetSqlValue (i);
			if (!(value is SqlBoolean))
				throw new InvalidCastException ("Type is " + value.GetType ().ToString ());
			return (SqlBoolean) value;
		}

		public
#if NET_2_0
		virtual
#endif
		SqlByte GetSqlByte (int i)
		{
			object value = GetSqlValue (i);
			if (!(value is SqlByte))
				throw new InvalidCastException ("Type is " + value.GetType ().ToString ());
			return (SqlByte) value;
		}

		public
#if NET_2_0
		virtual
#endif
		SqlDateTime GetSqlDateTime (int i)
		{
			object value = GetSqlValue (i);
			if (!(value is SqlDateTime))
				throw new InvalidCastException ("Type is " + value.GetType ().ToString ());
			return (SqlDateTime) value;
		}

		public
#if NET_2_0
		virtual
#endif
		SqlDecimal GetSqlDecimal (int i)
		{
			object value = GetSqlValue (i);
			if (!(value is SqlDecimal))
				throw new InvalidCastException ("Type is " + value.GetType ().ToString ());
			return (SqlDecimal) value;
		}

		public
#if NET_2_0
		virtual
#endif
		SqlDouble GetSqlDouble (int i)
		{
			object value = GetSqlValue (i);
			if (!(value is SqlDouble))
				throw new InvalidCastException ("Type is " + value.GetType ().ToString ());
			return (SqlDouble) value;
		}

		public
#if NET_2_0
		virtual
#endif
		SqlGuid GetSqlGuid (int i)
		{
			object value = GetSqlValue (i);
			if (!(value is SqlGuid))
				throw new InvalidCastException ("Type is " + value.GetType ().ToString ());
			return (SqlGuid) value;
		}

		public
#if NET_2_0
		virtual
#endif
		SqlInt16 GetSqlInt16 (int i)
		{
			object value = GetSqlValue (i);
			if (!(value is SqlInt16))
				throw new InvalidCastException ("Type is " + value.GetType ().ToString ());
			return (SqlInt16) value;
		}

		public
#if NET_2_0
		virtual
#endif
		SqlInt32 GetSqlInt32 (int i)
		{
			object value = GetSqlValue (i);
			if (!(value is SqlInt32))
				throw new InvalidCastException ("Type is " + value.GetType ().ToString ());
			return (SqlInt32) value;
		}

		public
#if NET_2_0
		virtual
#endif
		SqlInt64 GetSqlInt64 (int i)
		{
			object value = GetSqlValue (i);
			// TDS 7.0 returns bigint as decimal(19,0)
			if (value is SqlDecimal) {
				TdsDataColumn schema = command.Tds.Columns[i];
				if ((byte)schema["NumericPrecision"] == 19 && (byte)schema["NumericScale"] == 0)
					value = (SqlInt64) (SqlDecimal) value;
			}
			if (!(value is SqlInt64))
				throw new InvalidCastException ("Type is " + value.GetType ().ToString ());
			return (SqlInt64) value;
		}

		public
#if NET_2_0
		virtual
#endif
		SqlMoney GetSqlMoney (int i)
		{
			object value = GetSqlValue (i);
			if (!(value is SqlMoney))
				throw new InvalidCastException ("Type is " + value.GetType ().ToString ());
			return (SqlMoney) value;
		}

		public
#if NET_2_0
		virtual
#endif
		SqlSingle GetSqlSingle (int i)
		{
			object value = GetSqlValue (i);
			if (!(value is SqlSingle))
				throw new InvalidCastException ("Type is " + value.GetType ().ToString ());
			return (SqlSingle) value;
		}

		public
#if NET_2_0
		virtual
#endif
		SqlString GetSqlString (int i)
		{
			object value = GetSqlValue (i);
			if (!(value is SqlString))
				throw new InvalidCastException ("Type is " + value.GetType ().ToString ());
			return (SqlString) value;
		}

#if NET_2_0
		public virtual SqlXml GetSqlXml (int i)
		{
			object value = GetSqlValue (i);
			if (!(value is SqlXml)) {
				if (value is DBNull) throw new SqlNullValueException ();
				throw new InvalidCastException ("Type is " + value.GetType ().ToString ());
			}
			return (SqlXml) value;
		}
#endif // NET_2_0

		public
#if NET_2_0
		virtual
#endif
		object GetSqlValue (int i)
		{
			SqlDbType type = (SqlDbType) (schemaTable.Rows [i]["ProviderType"]);
			object value = GetValue (i);

			switch (type) {
			case SqlDbType.BigInt:
				if (value == DBNull.Value)
					return SqlInt64.Null;
				return (SqlInt64) ((long) value);
			case SqlDbType.Binary:
			case SqlDbType.Image:
			case SqlDbType.VarBinary:
			case SqlDbType.Timestamp:
				if (value == DBNull.Value)
					return SqlBinary.Null;
				return (SqlBinary) ((byte[]) value);
			case SqlDbType.Bit:
				if (value == DBNull.Value)
					return SqlBoolean.Null;
				return (SqlBoolean) ((bool) value);
			case SqlDbType.Char:
			case SqlDbType.NChar:
			case SqlDbType.NText:
			case SqlDbType.NVarChar:
			case SqlDbType.Text:
			case SqlDbType.VarChar:
				if (value == DBNull.Value)
					return SqlString.Null;
				return (SqlString) ((string) value);
			case SqlDbType.DateTime:
			case SqlDbType.SmallDateTime:
				if (value == DBNull.Value)
					return SqlDateTime.Null;
				return (SqlDateTime) ((DateTime) value);
			case SqlDbType.Decimal:
				if (value == DBNull.Value)
					return SqlDecimal.Null;
				if (value is TdsBigDecimal)
					return SqlDecimal.FromTdsBigDecimal ((TdsBigDecimal) value);
				return (SqlDecimal) ((decimal) value);
			case SqlDbType.Float:
				if (value == DBNull.Value)
					return SqlDouble.Null;
				return (SqlDouble) ((double) value);
			case SqlDbType.Int:
				if (value == DBNull.Value)
					return SqlInt32.Null;
				return (SqlInt32) ((int) value);
			case SqlDbType.Money:
			case SqlDbType.SmallMoney:
				if (value == DBNull.Value)
					return SqlMoney.Null;
				return (SqlMoney) ((decimal) value);
			case SqlDbType.Real:
				if (value == DBNull.Value)
					return SqlSingle.Null;
				return (SqlSingle) ((float) value);
			case SqlDbType.UniqueIdentifier:
				if (value == DBNull.Value)
					return SqlGuid.Null;
				return (SqlGuid) ((Guid) value);
			case SqlDbType.SmallInt:
				if (value == DBNull.Value)
					return SqlInt16.Null;
				return (SqlInt16) ((short) value);
			case SqlDbType.TinyInt:
				if (value == DBNull.Value)
					return SqlByte.Null;
				return (SqlByte) ((byte) value);
#if NET_2_0
			case SqlDbType.Xml:
				if (value == DBNull.Value)
					return SqlByte.Null;
				return (SqlXml) value;
#endif
			}

			throw new InvalidOperationException ("The type of this column is unknown.");
		}

		public
#if NET_2_0
		virtual
#endif
		int GetSqlValues (object[] values)
		{
			int count = 0;
			int columnCount = schemaTable.Rows.Count;
			int arrayCount = values.Length;

			if (arrayCount > columnCount)
				count = columnCount;
			else
				count = arrayCount;

			for (int i = 0; i < count; i += 1) 
				values [i] = GetSqlValue (i);

			return count;
		}

		public 
#if NET_2_0
		override
#endif // NET_2_0
		string GetString (int i)
		{
			object value = GetValue (i);
			if (!(value is string)) {
				if (value is DBNull) throw new SqlNullValueException ();
				throw new InvalidCastException ("Type is " + value.GetType ().ToString ());
			}
			return (string) value;
		}

		public 
#if NET_2_0
		override
#endif // NET_2_0
		object GetValue (int i)
		{
			if (i < 0 || i >= command.Tds.Columns.Count)
				throw new IndexOutOfRangeException ();

			try {
				if ((command.CommandBehavior & CommandBehavior.SequentialAccess) != 0) {
					return ((Tds)command.Tds).GetSequentialColumnValue (i);
				}
			} catch (TdsInternalException ex) {
				command.Connection.Close ();
				throw SqlException.FromTdsInternalException ((TdsInternalException) ex);
			}

			return command.Tds.ColumnValues [i];
		}

		public 
#if NET_2_0
		override
#endif // NET_2_0
		int GetValues (object[] values)
		{
			int len = values.Length;
			int bigDecimalIndex = command.Tds.ColumnValues.BigDecimalIndex;

			// If a four-byte decimal is stored, then we can't convert to
			// a native type.  Throw an OverflowException.
			if (bigDecimalIndex >= 0 && bigDecimalIndex < len)
				throw new OverflowException ();
			try {
				command.Tds.ColumnValues.CopyTo (0, values, 0,
								 len > command.Tds.ColumnValues.Count ? command.Tds.ColumnValues.Count : len);
			} catch (TdsInternalException ex) {
				command.Connection.Close ();
				throw SqlException.FromTdsInternalException ((TdsInternalException) ex);
			}
			return (len < FieldCount ? len : FieldCount);
		}

#if !NET_2_0
		void IDisposable.Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}
#endif

#if NET_2_0
		public override IEnumerator GetEnumerator ()
#else
		IEnumerator IEnumerable.GetEnumerator ()
#endif
		{
			return new DbEnumerator (this);
		}

		public 
#if NET_2_0
		override
#endif // NET_2_0
		bool IsDBNull (int i)
		{
			return GetValue (i) == DBNull.Value;
		}

		public 
#if NET_2_0
		override
#endif // NET_2_0
		bool NextResult ()
		{
			ValidateState ();

			if ((command.CommandBehavior & CommandBehavior.SingleResult) != 0 && resultsRead > 0)
				return false;

			try {
				moreResults = command.Tds.NextResult ();
			} catch (TdsInternalException ex) {
				command.Connection.Close ();
				throw SqlException.FromTdsInternalException ((TdsInternalException) ex);
			}
			if (!moreResults)
				command.GetOutputParameters ();
			else {
				//new schema
				schemaTable = ConstructSchemaTable ();
				GetSchemaTable ();
			}

			rowsRead = 0;
			resultsRead += 1;
			return moreResults;
		}

		public 
#if NET_2_0
		override
#endif // NET_2_0
		bool Read ()
		{
			ValidateState ();

			if ((command.CommandBehavior & CommandBehavior.SingleRow) != 0 && rowsRead > 0)
				return false;
			if ((command.CommandBehavior & CommandBehavior.SchemaOnly) != 0)
				return false;
			if (!moreResults)
				return false;
	
			if ((haveRead) && (!readResultUsed))
			{
				readResultUsed = true;
				return true;
			}
			return (ReadRecord ());
		}

		internal bool ReadRecord ()
		{
			try {
				bool result = command.Tds.NextRow ();
			
				rowsRead += 1;
				return result;
			} catch (TdsInternalException ex) {
				command.Connection.Close ();
				throw SqlException.FromTdsInternalException ((TdsInternalException) ex);
			}
		}
		
		void ValidateState ()
		{
			if (IsClosed)
				throw new InvalidOperationException ("Invalid attempt to read data when reader is closed");
		}
		
#if NET_2_0

		public override Type GetProviderSpecificFieldType (int position)
		{
			return (GetSqlValue (position).GetType());
		}

		public override object GetProviderSpecificValue (int position)
		{
			return (GetSqlValue (position));
		}

		public override int GetProviderSpecificValues (object [] values)
		{
			return (GetSqlValues (values));
		}

		public virtual SqlBytes GetSqlBytes (int i)
		{
			//object value = GetSqlValue (i);
			//if (!(value is SqlBinary))
			//	throw new InvalidCastException ("Type is " + value.GetType ().ToString ());
			Byte[] val = (byte[])GetValue(i);
			SqlBytes sb = new SqlBytes (val);
			return (sb);
		}
#endif // NET_2_0

		#endregion // Methods
	}
}
