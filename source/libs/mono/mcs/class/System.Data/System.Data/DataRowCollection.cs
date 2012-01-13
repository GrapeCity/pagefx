//
// System.Data.DataRowCollection.cs
//
// Author:
//   Daniel Morgan <danmorg@sc.rr.com>
//   Tim Coleman <tim@timcoleman.com>
//
// (C) Ximian, Inc 2002
// (C) Copyright 2002 Tim Coleman
// (C) Copyright 2002 Daniel Morgan
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
using System.Data.Common;

namespace System.Data
{
	/// <summary>
	/// Collection of DataRows in a DataTable
	/// </summary>

#if !NET_2_0
	[Serializable]
#endif
	public
#if NET_2_0
	sealed
#endif
	class DataRowCollection : InternalDataCollectionBase 
	{
		private DataTable table;

#if NOT_PFX
internal event ListChangedEventHandler ListChanged;
#endif

		/// <summary>
		/// Internal constructor used to build a DataRowCollection.
		/// </summary>
		internal DataRowCollection (DataTable table) : base ()
		{
			this.table = table;
		}

		/// <summary>
		/// Gets the row at the specified index.
		/// </summary>
		public DataRow this[int index] 
		{
			get { 
				if (index < 0 || index >= Count)
					throw new IndexOutOfRangeException ("There is no row at position " + index + ".");

				return (DataRow) List[index]; 
			}
		}

#if NET_2_0
		public override int Count {
			get {
				return List.Count;
			}
		}
#endif

#if !NET_2_0
		/// <summary>
		/// This member overrides InternalDataCollectionBase.List
		/// </summary>
		protected override ArrayList List 
		{
			get { return base.List; }
		}
#endif

		/// <summary>
		/// Adds the specified DataRow to the DataRowCollection object.
		/// </summary>
		public void Add (DataRow row) 
		{
			//TODO: validation
			if (row == null)
				throw new ArgumentNullException("row", "'row' argument cannot be null.");

			if (row.Table != this.table)
				throw new ArgumentException ("This row already belongs to another table.");
			
			// If row id is not -1, we know that it is in the collection.
			if (row.RowID != -1)
				throw new ArgumentException ("This row already belongs to this table.");
			
			row.BeginEdit();

			row.Validate();

			AddInternal(row);
		}

#if NET_2_0
		public
#else
		internal
#endif
		int IndexOf (DataRow row) 
		{
			if (row == null)
				return -1;

			int i = 0;
			foreach (DataRow dr in this) {
				if (dr == row) {
					return i;
				}
				i++;
			}

			return -1;
		}

		internal void AddInternal (DataRow row) {
			AddInternal (row, DataRowAction.Add);
		}

		internal void AddInternal(DataRow row, DataRowAction action) {
			row.Table.ChangingDataRow (row, action);
			row.HasParentCollection = true;
			List.Add (row);
			// Set the row id.
			row.RowID = List.Count - 1;
			row.AttachRow ();
#if NET_2_0
			if ((action & (DataRowAction.ChangeCurrentAndOriginal |
							DataRowAction.ChangeOriginal)) != 0)
				row.Original = row.Current;
#endif
			row.Table.ChangedDataRow (row, action);
			if (row._rowChanged)
				row._rowChanged = false;
		}

		/// <summary>
		/// Creates a row using specified values and adds it to the DataRowCollection.
		/// </summary>
#if NET_2_0
		public DataRow Add (params object[] values) 
#else
		public virtual DataRow Add (object[] values) 
#endif
		{
			if (values == null)
				throw new NullReferenceException ();
			DataRow row = table.NewNotInitializedRow();
			int newRecord = table.CreateRecord(values);
			row.ImportRecord(newRecord);

			row.Validate();
			AddInternal (row);
			return row;
		}

		/// <summary>
		/// Clears the collection of all rows.
		/// </summary>
		public void Clear () 
		{
			if (this.table.DataSet != null && this.table.DataSet.EnforceConstraints) {
				foreach (Constraint c in table.Constraints) {
					UniqueConstraint uc = c as UniqueConstraint;
					if (uc == null) 
						continue;
					if (uc.ChildConstraint == null || uc.ChildConstraint.Table.Rows.Count == 0)
						continue;

					string err = String.Format ("Cannot clear table Parent because " +
								"ForeignKeyConstraint {0} enforces Child.", uc.ConstraintName);
#if NET_1_1
					throw new InvalidConstraintException (err);
#else
					throw new ArgumentException (err);
#endif
				}
			}

			List.Clear ();

#if NOT_PFX
            // Remove from indexes
			table.ResetIndexes ();
#endif

#if NOT_PFX
OnListChanged (this, new ListChangedEventArgs (ListChangedType.Reset, -1, -1));
#endif
		}

#if NOT_PFX
		/// <summary>
		/// Gets a value indicating whether the primary key of any row in the collection contains
		/// the specified value.
		/// </summary>
		public bool Contains (object key) 
		{
			return Find (key) != null;
		}

		/// <summary>
		/// Gets a value indicating whether the primary key column(s) of any row in the 
		/// collection contains the values specified in the object array.
		/// </summary>
		public bool Contains (object[] keys) 
		{
			return Find (keys) != null;
		}
#endif

#if NET_2_0
		public void CopyTo (DataRow [] array, int index)
		{
			CopyTo ((Array) array, index);
		}

		public override void CopyTo (Array array, int index)
		{
			base.CopyTo (array, index);
		}

		public override IEnumerator GetEnumerator ()
  		{
  			return base.GetEnumerator ();
  		}
#endif

#if NOT_PFX
		/// <summary>
		/// Gets the row specified by the primary key value.
		/// </summary>
		public DataRow Find (object key) 
		{
			return Find (new object[]{key}, DataViewRowState.CurrentRows);
                }

		/// <summary>
		/// Gets the row containing the specified primary key values.
		/// </summary>
		public DataRow Find (object[] keys)
		{
			return Find (keys, DataViewRowState.CurrentRows);
		}

		/// <summary>
		/// Gets the row containing the specified primary key values by searching the rows 
		/// filtered by the state.
		/// </summary>
		internal DataRow Find (object [] keys, DataViewRowState rowStateFilter)
		{
			if (table.PrimaryKey.Length == 0)
				throw new MissingPrimaryKeyException ("Table doesn't have a primary key.");

			if (keys == null)
				throw new ArgumentException ("Expecting " + table.PrimaryKey.Length +" value(s) for the key being indexed, but received 0 value(s).");

			Index index = table.GetIndex (table.PrimaryKey, null, rowStateFilter, null, false);
			int record = index.Find (keys);

			if (record != -1 || !table._duringDataLoad)
				return (record != -1 ? table.RecordCache [record] : null);

			// If the key is not found using Index *and* if DataTable is under BeginLoadData 
			// then, check all the DataRows for the key
			record = table.RecordCache.NewRecord ();
			try {
				for (int i=0; i < table.PrimaryKey.Length; ++i)
					table.PrimaryKey [i].DataContainer [record] = keys [i];

				bool found;
				foreach (DataRow row in this) {

					int rowIndex = Key.GetRecord (row, rowStateFilter);
					if (rowIndex == -1)
						continue;

					found = true;
					for (int columnCnt = 0; columnCnt < table.PrimaryKey.Length; ++columnCnt) {
						if (table.PrimaryKey [columnCnt].CompareValues (rowIndex, record) == 0)
							continue;
						found = false;
						break;
					}
					if (found)
						return row;
				}
				return null;
			} finally {
				table.RecordCache.DisposeRecord (record);
			}
		}
#endif

		/// <summary>
		/// Inserts a new row into the collection at the specified location.
		/// </summary>
		public void InsertAt (DataRow row, int pos) 
		{
			if (pos < 0)
				throw new IndexOutOfRangeException ("The row insert position " + pos + " is invalid.");
			
			if (row == null)
				throw new ArgumentNullException("row", "'row' argument cannot be null.");
	
			if (row.Table != this.table)
				throw new ArgumentException ("This row already belongs to another table.");

			// If row id is not -1, we know that it is in the collection.
			if (row.RowID != -1)
				throw new ArgumentException ("This row already belongs to this table.");
			
			row.Validate();
				
			row.Table.ChangingDataRow (row, DataRowAction.Add);

			if (pos >= List.Count) {
				row.RowID = List.Count;
				List.Add (row);
			}
			else {
				List.Insert (pos, row);
				row.RowID = pos;
				for (int i = pos+1; i < List.Count; i++) {
        	                        ((DataRow)List [i]).RowID = i;
	                        }
			}
				
			row.HasParentCollection = true;
			row.AttachRow ();
			row.Table.ChangedDataRow (row, DataRowAction.Add);
		}

		/// <summary>
		/// Removes the specified DataRow from the internal list. Used by DataRow to commit the removing.
		/// </summary>
		internal void RemoveInternal (DataRow row) {
			if (row == null) {
				throw new IndexOutOfRangeException ("The given datarow is not in the current DataRowCollection.");
			}
			int index = List.IndexOf(row);
			if (index < 0) {
				throw new IndexOutOfRangeException ("The given datarow is not in the current DataRowCollection.");
			}
			List.RemoveAt(index);
		}

		/// <summary>
		/// Removes the specified DataRow from the collection.
		/// </summary>
		public void Remove (DataRow row) 
		{
			if (!List.Contains(row))
				throw new IndexOutOfRangeException ("The given datarow is not in the current DataRowCollection.");

			DataRowState state = row.RowState;
			if (state != DataRowState.Deleted &&
				state != DataRowState.Detached) {
				row.Delete();
				// if the row was in added state it will be in Detached state after the
				// delete operation, so we have to check it.
				if (row.RowState != DataRowState.Detached)
					row.AcceptChanges();
			}
		}

		/// <summary>
		/// Removes the row at the specified index from the collection.
		/// </summary>
		public void RemoveAt (int index) 
		{			
			Remove(this[index]);
		}

#if NOT_PFX
internal void OnListChanged (object sender, ListChangedEventArgs args)
		{
			if (ListChanged != null)
				ListChanged (sender, args);
		}
#endif

	}
}
