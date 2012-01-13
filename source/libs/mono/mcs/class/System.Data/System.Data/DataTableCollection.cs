//
// System.Data.DataTableCollection.cs
//
// Authors:
//   Christopher Podurgiel (cpodurgiel@msn.com)
//   Tim Coleman <tim@timcoleman.com>
//
// (C) Chris Podurgiel
// (C) Copyright 2002 Tim Coleman
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

using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;

namespace System.Data {
	/// <summary>
	/// Represents the collection of tables for the DataSet.
	/// </summary>
#if NOT_PFX
	[Editor ("Microsoft.VSDesigner.Data.Design.TablesCollectionEditor, " + Consts.AssemblyMicrosoft_VSDesigner,
		 "System.Drawing.Design.UITypeEditor, " + Consts.AssemblySystem_Drawing)]
	[DefaultEvent ("CollectionChanged")]
#endif
#if NOT_PFX
[ListBindable (false)]
#endif
#if !NET_2_0
	[Serializable]
#endif
	public
#if NET_2_0
	sealed
#endif
	class DataTableCollection : InternalDataCollectionBase
	{
		DataSet dataSet;
		DataTable[] mostRecentTables;
		#region Constructors 

		internal DataTableCollection (DataSet dataSet)
			: base ()
		{
			this.dataSet = dataSet;
		}
		
		#endregion
		
		#region Properties

		public DataTable this[int index] {
			get {
				if (index < 0 || index >= List.Count)
					throw new IndexOutOfRangeException(String.Format("Cannot find table {0}", index));
				return (DataTable)(List[index]);
			}
		}

		public DataTable this[string name] {
			get { 
				int index = IndexOf (name, true);
				return index < 0 ? null : (DataTable) List[index];
			}
		}

#if NET_2_0
		public DataTable this [string name, string tbNamespace] {
			get { 
				int index = IndexOf (name, tbNamespace, true);
				return index < 0 ? null : (DataTable) List[index];
			}
		}
#endif

		protected override ArrayList List {
			get { return base.List; }
		}

		#endregion
	
		#region Methods	

		public
#if !NET_2_0
		virtual
#endif
		DataTable Add () 
		{
			DataTable Table = new DataTable ();
			Add (Table);
			return Table;
		}

		public
#if !NET_2_0
		virtual
#endif
		void Add (DataTable table) 
		{
#if NOT_PFX
			OnCollectionChanging (new CollectionChangeEventArgs (CollectionChangeAction.Add, table));
#endif
			// check if the reference is a null reference
			if(table == null)
				throw new ArgumentNullException("table");

			// check if the list already contains this tabe.
			if(List.Contains(table))
				throw new ArgumentException("DataTable already belongs to this DataSet.");

			// check if table is part of another DataSet 
			if (table.DataSet != null && table.DataSet != this.dataSet)
				throw new ArgumentException ("DataTable already belongs to another DataSet");

			// if the table name is null or empty string.
			// give her a name. 
			if (table.TableName == null || table.TableName == string.Empty)
				NameTable (table);
		    
			// check if the collection has a table with the same name.
#if !NET_2_0
			int tmp = IndexOf (table.TableName);
#else
			int tmp = IndexOf (table.TableName, table.Namespace);
#endif
			// if we found a table with same name we have to check
			// that it is the same case.
			// indexof can return a table with different case letters.
			if (tmp != -1)
			{
				if(table.TableName == this[tmp].TableName)
					throw new DuplicateNameException("A DataTable named '" + table.TableName + "' already belongs to this DataSet.");
			}

			List.Add (table);
			table.dataSet = dataSet;
#if NOT_PFX
			OnCollectionChanged (new CollectionChangeEventArgs (CollectionChangeAction.Add, table));
#endif
		}

		public
#if !NET_2_0
		virtual
#endif
		DataTable Add (string name) 
		{
			DataTable table = new DataTable (name);
			this.Add (table);
			return table;
		}

#if NET_2_0
		public DataTable Add (string name, string tbNamespace)
		{
			DataTable table = new DataTable (name, tbNamespace);
			this.Add (table);
			return table;
		}
#endif

		public void AddRange (DataTable[] tables)
		{
			if (dataSet != null && dataSet.InitInProgress) {
				mostRecentTables = tables;
				return;
			}

			if (tables == null)
				return;

			foreach (DataTable table in tables) {
				if (table == null)
					continue;
				Add (table);
			}
		}

		internal void PostAddRange ()
		{
			if (mostRecentTables == null)
				return;

			foreach (DataTable table in mostRecentTables){
				if (table == null)
					continue;
				Add (table);
			}
			mostRecentTables = null;
		}

		public bool CanRemove (DataTable table) 
		{
			return CanRemove(table, false);
		}

		public void Clear () 
		{
			List.Clear ();
		}

		public bool Contains (string name) 
		{
			return (-1 != IndexOf (name, false));
		}
		
#if NET_2_0
		public bool Contains (string name, string tableNamespace) 
		{
			return (IndexOf (name, tableNamespace) != -1);
		}

		public void CopyTo (DataTable [] array, int index)
		{
			CopyTo ((Array) array, index);
		}
#endif

		public
#if !NET_2_0
		virtual
#endif
		int IndexOf (DataTable table) 
		{
			return List.IndexOf (table);
		}

		public
#if !NET_2_0
		virtual
#endif
		int IndexOf (string name) 
		{
			return IndexOf (name, false);
		}

#if NET_2_0
		public int IndexOf (string name, string tableNamespace)
		{
			if (tableNamespace == null)
				throw new ArgumentNullException ("'tableNamespace' argument cannot be null.",
						"tableNamespace");
			return IndexOf (name, tableNamespace, false);
		}
#endif
		
		public void Remove (DataTable table) 
		{
#if NOT_PFX
			OnCollectionChanging (new CollectionChangeEventArgs (CollectionChangeAction.Remove, table));
#endif
			if (CanRemove(table, true))
				table.dataSet = null;

			List.Remove(table);
			table.dataSet = null;
#if NOT_PFX
			OnCollectionChanged (new CollectionChangeEventArgs (CollectionChangeAction.Remove, table));
#endif
		}

		public void Remove (string name) 
		{
			int index = IndexOf (name, false);
			if (index == -1)
				throw new ArgumentException ("Table " + name + " does not belong to this DataSet"); 
			RemoveAt (index);
		}

#if NET_2_0
		public void Remove (string name, string tableNamespace)
		{
			int index = IndexOf (name, tableNamespace, true);
			if (index == -1)
				 throw new ArgumentException ("Table " + name + " does not belong to this DataSet");

			RemoveAt (index);
		}
#endif

		public void RemoveAt (int index) 
		{
			Remove(this[index]);
		}

		#endregion

		#region Protected methods

		
#if NOT_PFX
#if !NET_2_0
		protected internal virtual
#else
		internal
#endif
		void OnCollectionChanging (CollectionChangeEventArgs Args)
		{
			if (CollectionChanging != null)
				CollectionChanging (this, Args);
		}
#endif

		
#if NOT_PFX
#if !NET_2_0
		protected virtual
#else
		internal
#endif
		void OnCollectionChanged (CollectionChangeEventArgs Args)
		{
			if (CollectionChanged != null)
				CollectionChanged (this, Args);
		}
#endif

		#endregion

		#region Private methods
#if NET_2_0
		private int IndexOf (string name, string ns, bool error)
		{
			int index = -1, count = 0, match = -1;
			do {
				index = IndexOf (name, error, index+1);

				if (index == -1)
					break;

				if (ns == null) {
					if (count > 1)
						break;
					count++;
					match = index;
				} else if (this [index].Namespace.Equals (ns))
					return index;

			} while (index != -1 && index < Count);
			
			if (count == 1)
				return match;

			if (count == 0 || !error)
				return -1;

			throw new ArgumentException ("The given name '" + name + "' matches atleast two names" +
					"in the collection object with different namespaces");
		}
#endif

		private int IndexOf (string name, bool error)
		{
#if NET_2_0
			return IndexOf (name, null, error);
# else
			return IndexOf (name, error, 0);
#endif
		}

		private int IndexOf (string name, bool error, int start)
		{
			int count = 0, match = -1;
			for (int i = start; i < List.Count; i++)
			{
				String name2 = ((DataTable) List[i]).TableName;
				if (String.Compare (name, name2, false) == 0)
					return i;
				if (String.Compare (name, name2, true) == 0) {
					match = i;
					count++;
				}
			}
			if (count == 1)
				return match;
			if (count > 1 && error)
				throw new ArgumentException ("There is no match for the name in the same case and there are multiple matches in different case.");
			return -1;
		}

		/// <summary>
		/// gives name to Table (Table1, Table2, Table3,...)
		/// </summary>
		private void NameTable (DataTable Table)
		{
			string Name = "Table";
			int i = 1;
			while (Contains (Name + i))
				i++;

			Table.TableName = Name + i;
		}
		
		// check if a table can be removed from this collectiuon.
		// if the table can not be remved act according to throwException parameter.
		// if it is true throws an Exception, else return false.
		private bool CanRemove(DataTable table, bool throwException)
		{

			// check if table is null reference
			if (table == null) {
				if(throwException)
					throw new ArgumentNullException("table");
				return false;
			}

			// check if the table has the same DataSet as this collection.
			if(table.DataSet != this.dataSet) {
				if(!throwException)
					return false;
				throw new ArgumentException("Table " + table.TableName + " does not belong to this DataSet.");
			}
			
			// check the table has a relation attached to it.
			if (table.ParentRelations.Count > 0 || table.ChildRelations.Count > 0) {
				if(!throwException)
					return false;
				throw new ArgumentException("Cannot remove a table that has existing relations. Remove relations first.");
			}

			// now we check if any ForeignKeyConstraint is referncing 'table'.
			foreach (Constraint c in table.Constraints) {
				UniqueConstraint uc = c as UniqueConstraint;
				if (uc != null) {
					if (uc.ChildConstraint == null)
						continue;

					if (!throwException)
						return false;
					RaiseForeignKeyReferenceException (table.TableName, uc.ChildConstraint.ConstraintName);
				}

				ForeignKeyConstraint fc = c as ForeignKeyConstraint;
				if (fc == null) 
					continue;

				if (!throwException)
					return false;
				RaiseForeignKeyReferenceException (table.TableName, fc.ConstraintName);
			}

			return true;
		}

		private void RaiseForeignKeyReferenceException (string table, string constraint)
		{
			throw new ArgumentException (String.Format ("Cannot remove table {0}, because it is referenced" +
								" in ForeignKeyConstraint {1}. Remove the constraint first.",
								table, constraint));
		}

		#endregion // Private methods

		#region Events
#if NOT_PFX
		[ResDescriptionAttribute ("Occurs whenever this collection's membership changes.")]
		public event CollectionChangeEventHandler CollectionChanged;

		public event CollectionChangeEventHandler CollectionChanging;
#endif
		#endregion
	}
}
