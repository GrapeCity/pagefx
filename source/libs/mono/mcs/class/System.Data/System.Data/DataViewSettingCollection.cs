//
// System.Data.DataViewSettingCollection.cs
//
// Authors:
//   Rodrigo Moya (rodrigo@ximian.com)
//   Miguel de Icaza (miguel@gnome.org)
//   Tim Coleman (tim@timcoleman.com)
//   Atsushi Enomoto (atsushi@ximian.com)
//
// (C) 2002 Ximian, Inc.  http://www.ximian.com
// Copyright (C) Tim Coleman, 2002
// Copyright (C) 2005 Novell Inc,
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

namespace System.Data {
	/// <summary>
	/// Contains a read-only collection of DataViewSetting objects for each DataTable in a DataSet.
	/// </summary>
#if NOT_PFX
	[Editor ("Microsoft.VSDesigner.Data.Design.DataViewSettingsCollectionEditor, " + Consts.AssemblyMicrosoft_VSDesigner,
		 "System.Drawing.Design.UITypeEditor, " + Consts.AssemblySystem_Drawing)]
#endif
#if !NET_2_0
	[Serializable]
#endif
	public class DataViewSettingCollection : ICollection, IEnumerable 
	{
		#region Fields

		ArrayList settingList;

		#endregion // Fields

		#region Constructors

		internal DataViewSettingCollection (DataViewManager manager)
		{
			settingList = new ArrayList ();
			if (manager.DataSet != null)
				foreach (DataTable dt in manager.DataSet.Tables)
					settingList.Add (new DataViewSetting (
						manager, dt));
		}

		#endregion // Constructors

		#region Properties
	
#if NOT_PFX
		[Browsable (false)]	
#endif
		public virtual int Count {
			get { return settingList.Count; }
		}

#if NOT_PFX
		[Browsable (false)]	
#endif
		public bool IsReadOnly {
			get { return settingList.IsReadOnly; }
		}

#if NOT_PFX
		[Browsable (false)]	
#endif
		public bool IsSynchronized {
			get { return settingList.IsSynchronized; }
		}

		public virtual DataViewSetting this [DataTable dt] {
			get {
				for (int i = 0; i < settingList.Count; i++) {
					DataViewSetting dvs = (DataViewSetting) settingList[i];
					if (dvs.Table == dt)
						return dvs;
				}
				return null;
			}
			set {
				this[dt] = value;
			}
		}

		public virtual DataViewSetting this[string name] {
			get {
				for (int i = 0; i < settingList.Count; i++) {
					DataViewSetting dvs = (DataViewSetting) settingList[i];
					if (dvs.Table.TableName == name)
						return dvs;
				}
				return null;
			}
		}

		public virtual DataViewSetting this[int index] {
			get { return (DataViewSetting) settingList[index]; }
			set { settingList[index] = value; }
		}

#if NOT_PFX
		[Browsable (false)]	
#endif
		public object SyncRoot {
			get { return settingList.SyncRoot; }
		}

		#endregion // Properties

		#region Methods

		public void CopyTo (Array ar, int index) 
		{
			settingList.CopyTo (ar, index);
		}

#if NET_2_0
		public void CopyTo (DataViewSetting [] ar, int index) 
		{
			settingList.CopyTo (ar, index);
		}
#endif

		public IEnumerator GetEnumerator () 
		{
			return settingList.GetEnumerator ();
		}

		#endregion // Methods
	}
}
