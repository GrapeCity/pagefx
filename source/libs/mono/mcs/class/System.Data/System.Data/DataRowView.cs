//
// System.Data.DataRowView.cs
//
// Author:
//    Rodrigo Moya <rodrigo@ximian.com>
//    Miguel de Icaza <miguel@ximian.com>
//    Daniel Morgan <danmorg@sc.rr.com>
//
// (C) Ximian, Inc 2002
// (C) Daniel Morgan 2002
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
using System.Reflection;

namespace System.Data
{
	/// <summary>
	/// Represents a customized view of a DataRow exposed as a fully featured Windows Forms control.
	/// </summary>
	// FIXME: correct exceptions in this[] methods
#if NET_2_0
	public class DataRowView : ICustomTypeDescriptor, IEditableObject, IDataErrorInfo, INotifyPropertyChanged
#else
	public class DataRowView
#if NOT_PFX
: ICustomTypeDescriptor, IEditableObject, IDataErrorInfo
#endif
#endif
	{
		#region Fields

		DataView _dataView;
		DataRow _dataRow;
		int _index = -1;

		#endregion // Fields

		#region Constructors

		internal DataRowView (DataView dataView, DataRow row, int index) {
			_dataView = dataView;
			_dataRow = row;
			_index = index;
		}

		#endregion // Constructors

		#region Methods

		public override bool Equals(object other)
		{
			return (other != null &&
					other is DataRowView && 
					((DataRowView)other)._dataRow != null && 
					((DataRowView)other)._dataRow.Equals(_dataRow));
		}

		public void BeginEdit ()
		{
			_dataRow.BeginEdit();
		}

		public void CancelEdit ()
		{
			// FIXME:
			if (this.Row == DataView._lastAdded) {
				DataView.CompleteLastAdded(false);
			}
			else {
				_dataRow.CancelEdit();
			}
		}

		public DataView CreateChildView (DataRelation relation)
		{
			return DataView.CreateChildView(relation,_index);
		}

		public DataView CreateChildView (string name)
		{
			return CreateChildView (
				Row.Table.ChildRelations [name]);
		}

		public void Delete ()
		{
			DataView.Delete(_index);
		}

		public void EndEdit ()
		{
			// FIXME:
			if (this.Row == DataView._lastAdded) {
				DataView.CompleteLastAdded(true);
			}
			else {
				_dataRow.EndEdit();
			}
		}

		private void CheckAllowEdit()
		{
			if (!DataView.AllowEdit && (Row != DataView._lastAdded))
				throw new DataException("Cannot edit on a DataSource where AllowEdit is false.");
		}

		#endregion // Methods

		#region Properties
		
		public DataView DataView {
			get { return _dataView; }
		}

		public bool IsEdit {
			get { return _dataRow.HasVersion(DataRowVersion.Proposed); }
		}

		// It becomes true when this instance is created by
		// DataView.AddNew(). If it is true, then the DataRow is
		// "Detached", and when this.EndEdit() is invoked, the row
		// will be added to the table.
		public bool IsNew {
			get {
				return Row == DataView._lastAdded;
			}
		}
		
		[System.Runtime.CompilerServices.IndexerName("Item")]
		public object this[string column] {
			get {
				DataColumn dc = _dataView.Table.Columns[column];

				if (dc == null) {
					string error = column + " is neither a DataColumn nor a DataRelation for table " + _dataView.Table.TableName;
					throw new ArgumentException(error);
				}
				return _dataRow[dc, GetActualRowVersion ()];
			}
			set {
				CheckAllowEdit();
				DataColumn dc = _dataView.Table.Columns[column];

				if (dc == null) {
					string error = column + " is neither a DataColumn nor a DataRelation for table " + _dataView.Table.TableName;
					throw new ArgumentException(error);
				}
				_dataRow[dc] = value;
			}
		}

		// the compiler creates a DefaultMemeberAttribute from
		// this IndexerNameAttribute
		public object this[int column] {
			get {
				DataColumn dc = _dataView.Table.Columns[column];

				if (dc == null) {
					string error = column + " is neither a DataColumn nor a DataRelation for table " + _dataView.Table.TableName;
					throw new ArgumentException(error);
				}
				return _dataRow[dc, GetActualRowVersion ()];
			}
			set {
				CheckAllowEdit();
				DataColumn dc = _dataView.Table.Columns[column];

				if (dc == null) {
					string error = column + " is neither a DataColumn nor a DataRelation for table " + _dataView.Table.TableName;
					throw new ArgumentException(error);
				}
				_dataRow[dc] = value;

			}
		}

		private DataRowVersion GetActualRowVersion ()
		{
			switch (_dataView.RowStateFilter) {
			case DataViewRowState.Added:
				return DataRowVersion.Proposed;
			case DataViewRowState.ModifiedOriginal:
			case DataViewRowState.Deleted:
			case DataViewRowState.Unchanged:
			case DataViewRowState.OriginalRows:
				return DataRowVersion.Original;
			case DataViewRowState.ModifiedCurrent:
				return DataRowVersion.Current;
			}
			return DataRowVersion.Default;
		}

		public DataRow Row {
			get {
				return _dataRow;
			}
		}

		public DataRowVersion RowVersion {
			get {
				DataRowVersion version = DataView.GetRowVersion(_index);
				if (version != DataRowVersion.Original)
					version = DataRowVersion.Current;

				return version;
			}
		}

		public override int GetHashCode() {
			return _dataRow.GetHashCode();
		}	

		internal int Index {
			get { return _index; }
		}

		#endregion // Properties
		
		#region ICustomTypeDescriptor implementations
#if NOT_PFX
		AttributeCollection ICustomTypeDescriptor.GetAttributes  ()
		{
			System.ComponentModel.AttributeCollection attributes;
			attributes = AttributeCollection.Empty;
			return attributes;
		}

		[MonoTODO ("Not implemented.   Always returns String.Empty")]
		string ICustomTypeDescriptor.GetClassName ()
		{
			return "";
		}
		
		[MonoTODO ("Not implemented.   Always returns null")]
		string ICustomTypeDescriptor.GetComponentName ()
		{
			return null;
		}

		[MonoTODO ("Not implemented.   Always returns null")]
		TypeConverter ICustomTypeDescriptor.GetConverter ()
		{
			return null;
		}

		[MonoTODO ("Not implemented.   Always returns null")]
		EventDescriptor ICustomTypeDescriptor.GetDefaultEvent ()
		{
			return null;
		}
		
		[MonoTODO ("Not implemented.   Always returns null")]
		PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty ()
		{
			return null;
		}
		
		[MonoTODO ("Not implemented.   Always returns null")]
		object ICustomTypeDescriptor.GetEditor (Type editorBaseType)
		{
			return null;
		}
		
		[MonoTODO ("Not implemented.   Always returns an empty collection")]
		EventDescriptorCollection ICustomTypeDescriptor.GetEvents ()
		{
			return new EventDescriptorCollection(null);
		}

		[MonoTODO ("Not implemented.   Always returns an empty collection")]
		EventDescriptorCollection ICustomTypeDescriptor.GetEvents (Attribute [] attributes)
		{
			return new EventDescriptorCollection(null);
		}

		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties ()
		{
			if (DataView == null) {
				ITypedList typedList = (ITypedList) _dataView;
			return typedList.GetItemProperties(new PropertyDescriptor[0]);
			}
			else {
				return DataView.Table.GetPropertyDescriptorCollection();
			}
		}

		[MonoTODO ("It currently reports more descriptors than necessary")]
		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties (Attribute [] attributes)
		{
			PropertyDescriptorCollection descriptors;
			descriptors = ((ICustomTypeDescriptor) this).GetProperties ();
			// TODO: filter out descriptors which do not contain
			//       any of those attributes
			//       except, those descriptors 
			//       that contain DefaultMemeberAttribute
			return descriptors;
		}
		
		[MonoTODO]
		object ICustomTypeDescriptor.GetPropertyOwner (PropertyDescriptor pd)
		{
			return this;
		}
#endif
		#endregion // ICustomTypeDescriptor implementations

		#region IDataErrorInfo implementation
#if NOT_PFX
		string IDataErrorInfo.Error {
			[MonoTODO("Not implemented, always returns String.Empty")]
			get {
				return ""; // FIXME
			}
		}

		string IDataErrorInfo.this[string columnName] {
			[MonoTODO("Not implemented, always returns String.Empty")]
			get {
				return ""; // FIXME
			}
		}
#endif

		#endregion // IDataErrorInfo implementation
		
#if NET_2_0
		#region INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;

		void OnPropertyChanged (string propertyName)
		{
			if (PropertyChanged != null) {
				PropertyChangedEventArgs args = new PropertyChangedEventArgs (propertyName);
				PropertyChanged (this, args);
			}
		}
		#endregion
#endif
	}
}
