//
// System.Data.Odbc.OdbcConnection
//
// Authors:
//  Brian Ritchie (brianlritchie@hotmail.com) 
//
// Copyright (C) Brian Ritchie, 2002
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

using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Runtime.InteropServices;
using System.EnterpriseServices;
#if NET_2_0 && !TARGET_JVM
using System.Transactions;
#endif

namespace System.Data.Odbc
{
	[DefaultEvent ("InfoMessage")]
#if NET_2_0
	public sealed class OdbcConnection : DbConnection, ICloneable
#else
	public sealed class OdbcConnection : Component, ICloneable, IDbConnection
#endif //NET_2_0
	{
		#region Fields

		string connectionString;
		int connectionTimeout;
		internal OdbcTransaction transaction;
		IntPtr henv =IntPtr.Zero, hdbc=IntPtr.Zero;
		bool disposed;
		
		#endregion

		#region Constructors
		
		public OdbcConnection () : this (String.Empty)
		{
		}

		public OdbcConnection (string connectionString)
		{
			connectionTimeout = 15;
			ConnectionString = connectionString;
		}

		#endregion // Constructors

		#region Properties

		internal IntPtr hDbc {
			get { return hdbc; }
		}

		[OdbcCategoryAttribute ("DataCategory_Data")]
		[DefaultValue ("")]
		[OdbcDescriptionAttribute ("Information used to connect to a Data Source")]	
		[RefreshPropertiesAttribute (RefreshProperties.All)]
		[EditorAttribute ("Microsoft.VSDesigner.Data.Odbc.Design.OdbcConnectionStringEditor, "+ Consts.AssemblyMicrosoft_VSDesigner, "System.Drawing.Design.UITypeEditor, "+ Consts.AssemblySystem_Drawing )]
		[RecommendedAsConfigurableAttribute (true)]
		public
#if NET_2_0
		override
#endif
		string ConnectionString {
			get {
				if (connectionString == null)
					return string.Empty;
				return connectionString;
			}
			set { connectionString = value; }
		}
		
		[OdbcDescriptionAttribute ("Current connection timeout value, not settable  in the ConnectionString")]
		[DefaultValue (15)]
#if NET_2_0
		[DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
#endif
		public
#if NET_2_0
		new
#endif // NET_2_0
		int ConnectionTimeout {
			get {
				return connectionTimeout;
			}
			set {
				if (value < 0)
					throw new ArgumentException("Timout should not be less than zero.");
				connectionTimeout = value;
			}
		}

		[DesignerSerializationVisibilityAttribute (DesignerSerializationVisibility.Hidden)]
		[OdbcDescriptionAttribute ("Current data source Catlog value, 'Database=X' in the ConnectionString")]
		public
#if NET_2_0
		override
#endif // NET_2_0
		string Database {
			get {
				if (State == ConnectionState.Closed)
					return string.Empty;
				return GetInfo (OdbcInfo.DatabaseName);
			}
		}

		[DesignerSerializationVisibilityAttribute (DesignerSerializationVisibility.Hidden)]
		[OdbcDescriptionAttribute ("The ConnectionState indicating whether the connection is open or closed")]
		[BrowsableAttribute (false)]
		public
#if NET_2_0
		override
#endif // NET_2_0
		ConnectionState State {
			get {
				if (hdbc!=IntPtr.Zero)
					return ConnectionState.Open;
				else
					return ConnectionState.Closed;
			}
		}

		[DesignerSerializationVisibilityAttribute (DesignerSerializationVisibility.Hidden)]
		[OdbcDescriptionAttribute ("Current data source, 'Server=X' in the ConnectionString")]
#if NET_2_0
		[Browsable (false)]
#endif
		public
#if NET_2_0
		override
#endif // NET_2_0
		string DataSource {
			get {
				if (State == ConnectionState.Closed)
					return string.Empty;
				return GetInfo (OdbcInfo.DataSourceName);
			}
		}

#if NET_2_0
		[Browsable (false)]
#endif
		[DesignerSerializationVisibilityAttribute (DesignerSerializationVisibility.Hidden)]
		[OdbcDescriptionAttribute ("Current ODBC Driver")]
		public string Driver {
			get {
				if (State == ConnectionState.Closed)
					return string.Empty;
				return GetInfo (OdbcInfo.DriverName);
			}
		}
		
		[DesignerSerializationVisibilityAttribute (DesignerSerializationVisibility.Hidden)]
		[OdbcDescriptionAttribute ("Version of the product accessed by the ODBC Driver")]
		[BrowsableAttribute (false)]
		public
#if NET_2_0
		override
#endif // NET_2_0
		string ServerVersion {
			get {
				return GetInfo (OdbcInfo.DbmsVersion);
			}
		}

		#endregion // Properties
	
		#region Methods
	
		public
#if NET_2_0
		new
#endif // NET_2_0
		OdbcTransaction BeginTransaction ()
		{
			return BeginTransaction(IsolationLevel.Unspecified);
		}

#if ONLY_1_1
		IDbTransaction IDbConnection.BeginTransaction ()
		{
			return (IDbTransaction) BeginTransaction();
		}
#endif // ONLY_1_1
#if NET_2_0
		protected override DbTransaction BeginDbTransaction (IsolationLevel level)
		{
			return BeginTransaction (level);
		}
#endif
		
		public
#if NET_2_0
		new
#endif // NET_2_0
		OdbcTransaction BeginTransaction (IsolationLevel level)
		{
			if (State == ConnectionState.Closed)
				throw ExceptionHelper.ConnectionClosed ();

			if (transaction == null) {
				transaction = new OdbcTransaction (this,level);
				return transaction;
			} else
				throw new InvalidOperationException();
		}

#if ONLY_1_1
		IDbTransaction IDbConnection.BeginTransaction (IsolationLevel level)
		{
			return (IDbTransaction) BeginTransaction (level);
		}
#endif // ONLY_1_1

		public
#if NET_2_0
		override
#endif // NET_2_0
		void Close ()
		{
			OdbcReturn ret = OdbcReturn.Error;
			if (State == ConnectionState.Open) {
				// disconnect
				ret = libodbc.SQLDisconnect (hdbc);
				if ((ret != OdbcReturn.Success) && (ret != OdbcReturn.SuccessWithInfo))
					throw new OdbcException (new OdbcError ("SQLDisconnect", OdbcHandleType.Dbc, hdbc));

				FreeHandles ();
				transaction = null;
				RaiseStateChange (ConnectionState.Open, ConnectionState.Closed);
			}
		}

		public
#if NET_2_0
		new
#endif // NET_2_0
		OdbcCommand CreateCommand ()
		{
			return new OdbcCommand (string.Empty, this, transaction);
		}

		public
#if NET_2_0
		override
#endif // NET_2_0
		void ChangeDatabase(string Database)
		{
			IntPtr ptr = IntPtr.Zero;
			OdbcReturn ret = OdbcReturn.Error;

			try {
				ptr = Marshal.StringToHGlobalAnsi (Database);
				ret = libodbc.SQLSetConnectAttr (hdbc, OdbcConnectionAttribute.CurrentCatalog, ptr, Database.Length);

				if (ret != OdbcReturn.Success && ret != OdbcReturn.SuccessWithInfo)
					throw new OdbcException (new OdbcError ("SQLSetConnectAttr", OdbcHandleType.Dbc, hdbc));
			} finally {
				if (ptr != IntPtr.Zero)
					Marshal.FreeCoTaskMem (ptr);
			}
		}

		protected override void Dispose (bool disposing)
		{
			if (!this.disposed) {
				try {
					// release the native unmananged resources
					this.Close();
					this.disposed = true;
				} finally {
					// call Dispose on the base class
					base.Dispose(disposing);
				}
			}
		}

		[MonoTODO]
		object ICloneable.Clone ()
		{
			throw new NotImplementedException();
		}

#if ONLY_1_1
		IDbCommand IDbConnection.CreateCommand ()
		{
			return (IDbCommand) CreateCommand ();
		}
#endif //ONLY_1_1

#if NET_2_0
		protected override DbCommand CreateDbCommand ()
		{
			return CreateCommand ();
		}
#endif

		public
#if NET_2_0
		override
#endif // NET_2_0
		void Open ()
		{
			if (State == ConnectionState.Open)
				throw new InvalidOperationException ();

			OdbcReturn ret = OdbcReturn.Error;
			OdbcException e = null;
		
			try {
				// allocate Environment handle
				ret = libodbc.SQLAllocHandle (OdbcHandleType.Env, IntPtr.Zero, ref henv);
				if ((ret != OdbcReturn.Success) && (ret != OdbcReturn.SuccessWithInfo)) {
					e = new OdbcException (new OdbcError ("SQLAllocHandle"));
					MessageHandler (e);
					throw e;
				}

				ret = libodbc.SQLSetEnvAttr (henv, OdbcEnv.OdbcVersion, (IntPtr) libodbc.SQL_OV_ODBC3 , 0); 
				if ((ret != OdbcReturn.Success) && (ret != OdbcReturn.SuccessWithInfo))
					throw new OdbcException (new OdbcError ("SQLSetEnvAttr", OdbcHandleType.Env, henv));

				// allocate connection handle
				ret = libodbc.SQLAllocHandle (OdbcHandleType.Dbc, henv, ref hdbc);
				if ((ret != OdbcReturn.Success) && (ret != OdbcReturn.SuccessWithInfo))
					throw new OdbcException (new OdbcError ("SQLAllocHandle", OdbcHandleType.Env, henv));

				// DSN connection
				if (ConnectionString.ToLower ().IndexOf ("dsn=") >= 0)
				{
					string _uid = string.Empty, _pwd = string.Empty, _dsn = string.Empty;
					string [] items = ConnectionString.Split (new char[1]{';'});
					foreach (string item in items)
					{
						string [] parts = item.Split (new char[1] {'='});
						switch (parts [0].Trim ().ToLower ())
						{
						case "dsn":
							_dsn = parts [1].Trim ();
							break;
						case "uid":
							_uid = parts [1].Trim ();
							break;
						case "pwd":
							_pwd = parts [1].Trim ();
							break;
						}
					}
					ret = libodbc.SQLConnect(hdbc, _dsn, -3, _uid, -3, _pwd, -3);
					if ((ret != OdbcReturn.Success) && (ret != OdbcReturn.SuccessWithInfo)) 
						throw new OdbcException (new OdbcError ("SQLConnect", OdbcHandleType.Dbc, hdbc));
				}
				else 
				{
					// DSN-less Connection
					string OutConnectionString = new String (' ',1024);
					short OutLen = 0;
					ret = libodbc.SQLDriverConnect (hdbc, IntPtr.Zero, ConnectionString, -3, 
								     OutConnectionString, (short) OutConnectionString.Length, ref OutLen, 0);
					if ((ret != OdbcReturn.Success) && (ret != OdbcReturn.SuccessWithInfo)) 
						throw new OdbcException (new OdbcError ("SQLDriverConnect", OdbcHandleType.Dbc, hdbc));
				}

				RaiseStateChange (ConnectionState.Closed, ConnectionState.Open);
			} catch {
				// free handles if any.
				FreeHandles ();
				throw;
			}
			disposed = false;
		}

		[MonoTODO]
		public static void ReleaseObjectPool ()
		{
			throw new NotImplementedException ();
		}

		private void FreeHandles ()
		{
			OdbcReturn ret = OdbcReturn.Error;
			if (hdbc != IntPtr.Zero) {
				ret = libodbc.SQLFreeHandle ((ushort) OdbcHandleType.Dbc, hdbc);
				if ( (ret != OdbcReturn.Success) && (ret != OdbcReturn.SuccessWithInfo)) 
					throw new OdbcException (new OdbcError ("SQLFreeHandle", OdbcHandleType.Dbc, hdbc));
			}
			hdbc = IntPtr.Zero;

			if (henv != IntPtr.Zero) {
				ret = libodbc.SQLFreeHandle ((ushort) OdbcHandleType.Env, henv);
				if ( (ret != OdbcReturn.Success) && (ret != OdbcReturn.SuccessWithInfo)) 
					throw new OdbcException (new OdbcError ("SQLFreeHandle", OdbcHandleType.Env, henv));
			}
			henv = IntPtr.Zero;
		}

#if NET_2_0
		public override DataTable GetSchema ()
		{
			if (State == ConnectionState.Closed)
				throw ExceptionHelper.ConnectionClosed ();
			return MetaDataCollections.Instance;
		}

		public override DataTable GetSchema (string collectionName)
		{
			return GetSchema (collectionName, null);
		}

		public override DataTable GetSchema (string collectionName, string [] restrictionValues)
		{
			if (State == ConnectionState.Closed)
				throw ExceptionHelper.ConnectionClosed ();
			return GetSchema (collectionName, null);
		}

		[MonoTODO]
		public override void EnlistTransaction (Transaction transaction)
		{
			throw new NotImplementedException ();
		}
#endif

		[MonoTODO]
		public void EnlistDistributedTransaction ( ITransaction transaction) 
		{
			throw new NotImplementedException ();
		}

		internal string GetInfo (OdbcInfo info)
		{
			if (State == ConnectionState.Closed)
				throw new InvalidOperationException ("The connection is closed.");

			OdbcReturn ret = OdbcReturn.Error;
			short max_length = 256;
			byte [] buffer = new byte [max_length];
			short actualLength = 0;

			ret = libodbc.SQLGetInfo (hdbc, info, buffer, max_length, ref actualLength);
			if (ret != OdbcReturn.Success && ret != OdbcReturn.SuccessWithInfo)
				throw new OdbcException (new OdbcError ("SQLGetInfo",
									OdbcHandleType.Dbc,
									hdbc));

			return System.Text.Encoding.Default.GetString (buffer).Substring (0, actualLength);
		}

		private void RaiseStateChange (ConnectionState from, ConnectionState to)
		{
#if ONLY_1_1
			if (StateChange != null)
				StateChange (this, new StateChangeEventArgs (from, to));
#else
			base.OnStateChange (new StateChangeEventArgs (from, to));
#endif
		}

		private OdbcInfoMessageEventArgs CreateOdbcInfoMessageEvent (OdbcErrorCollection errors)
		{
			return new OdbcInfoMessageEventArgs (errors);
		}

		private void OnOdbcInfoMessage (OdbcInfoMessageEventArgs e) {
			if (InfoMessage != null)
				InfoMessage (this, e);
		}

		#endregion

		#region Events and Delegates

#if ONLY_1_1
		[OdbcDescription ("DbConnection_StateChange")]
		[OdbcCategory ("DataCategory_StateChange")]
		public event StateChangeEventHandler StateChange;
#endif // ONLY_1_1

 		[OdbcDescription ("DbConnection_InfoMessage")]
		[OdbcCategory ("DataCategory_InfoMessage")]
		public event OdbcInfoMessageEventHandler InfoMessage;

		private void MessageHandler (OdbcException e)
		{
			OnOdbcInfoMessage (CreateOdbcInfoMessageEvent (e.Errors));
		}

		#endregion
	}
}
