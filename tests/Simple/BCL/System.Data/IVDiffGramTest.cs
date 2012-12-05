using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using IssueVision;

#region IVDataSet
namespace IssueVision
{
    [Serializable()]
    public class IVDataSet : DataSet
    {
        private IssueHistoryDataTable tableIssueHistory;

        private IssuesDataTable tableIssues;

        private ConflictsDataTable tableConflicts;

        private IssueTypesDataTable tableIssueTypes;

        private StaffersDataTable tableStaffers;

        public IVDataSet()
        {
            InitClass();
        }

        public IssueHistoryDataTable IssueHistory
        {
            get { return tableIssueHistory; }
        }

        public IssuesDataTable Issues
        {
            get { return tableIssues; }
        }

        public ConflictsDataTable Conflicts
        {
            get { return tableConflicts; }
        }

        public IssueTypesDataTable IssueTypes
        {
            get { return tableIssueTypes; }
        }

        public StaffersDataTable Staffers
        {
            get { return tableStaffers; }
        }

        public override DataSet Clone()
        {
            IVDataSet cln = ((IVDataSet)(base.Clone()));
            cln.InitVars();
            return cln;
        }

        protected override bool ShouldSerializeTables()
        {
            return false;
        }

        protected override bool ShouldSerializeRelations()
        {
            return false;
        }

        internal void InitVars()
        {
            tableIssueHistory = ((IssueHistoryDataTable)(Tables["IssueHistory"]));
            if ((tableIssueHistory != null))
            {
                tableIssueHistory.InitVars();
            }
            tableIssues = ((IssuesDataTable)(Tables["Issues"]));
            if ((tableIssues != null))
            {
                tableIssues.InitVars();
            }
            tableConflicts = ((ConflictsDataTable)(Tables["Conflicts"]));
            if ((tableConflicts != null))
            {
                tableConflicts.InitVars();
            }
            tableIssueTypes = ((IssueTypesDataTable)(Tables["IssueTypes"]));
            if ((tableIssueTypes != null))
            {
                tableIssueTypes.InitVars();
            }
            tableStaffers = ((StaffersDataTable)(Tables["Staffers"]));
            if ((tableStaffers != null))
            {
                tableStaffers.InitVars();
            }
        }

        private void InitClass()
        {
            DataSetName = "IVDataSet";
            Prefix = "";
            Namespace = "http://www.tempuri.org/IVDataSet.xsd";
            Locale = new CultureInfo("en-US");
            CaseSensitive = false;
            EnforceConstraints = true;
            tableIssueHistory = new IssueHistoryDataTable();
            Tables.Add(tableIssueHistory);
            tableIssues = new IssuesDataTable();
            Tables.Add(tableIssues);
            tableConflicts = new ConflictsDataTable();
            Tables.Add(tableConflicts);
            tableIssueTypes = new IssueTypesDataTable();
            Tables.Add(tableIssueTypes);
            tableStaffers = new StaffersDataTable();
            Tables.Add(tableStaffers);
        }

        private bool ShouldSerializeIssueHistory()
        {
            return false;
        }

        private bool ShouldSerializeIssues()
        {
            return false;
        }

        private bool ShouldSerializeConflicts()
        {
            return false;
        }

        private bool ShouldSerializeIssueTypes()
        {
            return false;
        }

        private bool ShouldSerializeStaffers()
        {
            return false;
        }

        public delegate void IssueHistoryRowChangeEventHandler(object sender, IssueHistoryRowChangeEvent e);

        public delegate void IssuesRowChangeEventHandler(object sender, IssuesRowChangeEvent e);

        public delegate void ConflictsRowChangeEventHandler(object sender, ConflictsRowChangeEvent e);

        public delegate void IssueTypesRowChangeEventHandler(object sender, IssueTypesRowChangeEvent e);

        public delegate void StaffersRowChangeEventHandler(object sender, StaffersRowChangeEvent e);

        //[DebuggerStepThrough()]
        public class IssueHistoryDataTable : DataTable, IEnumerable
        {
            private DataColumn columnIssueHistoryID;

            private DataColumn columnStafferID;

            private DataColumn columnIssueID;

            private DataColumn columnComment;

            private DataColumn columnDateCreated;

            private DataColumn columnDisplayName;

            internal IssueHistoryDataTable()
                :
                base("IssueHistory")
            {
                InitClass();
            }

            internal IssueHistoryDataTable(DataTable table)
                :
                base(table.TableName)
            {
                if ((table.CaseSensitive != table.DataSet.CaseSensitive))
                {
                    CaseSensitive = table.CaseSensitive;
                }
                if ((table.Locale.ToString() != table.DataSet.Locale.ToString()))
                {
                    Locale = table.Locale;
                }
                if ((table.Namespace != table.DataSet.Namespace))
                {
                    Namespace = table.Namespace;
                }
                Prefix = table.Prefix;
                MinimumCapacity = table.MinimumCapacity;
                DisplayExpression = table.DisplayExpression;
            }

            public int Count
            {
                get { return Rows.Count; }
            }

            internal DataColumn IssueHistoryIDColumn
            {
                get { return columnIssueHistoryID; }
            }

            internal DataColumn StafferIDColumn
            {
                get { return columnStafferID; }
            }

            internal DataColumn IssueIDColumn
            {
                get { return columnIssueID; }
            }

            internal DataColumn CommentColumn
            {
                get { return columnComment; }
            }

            internal DataColumn DateCreatedColumn
            {
                get { return columnDateCreated; }
            }

            internal DataColumn DisplayNameColumn
            {
                get { return columnDisplayName; }
            }

            public IssueHistoryRow this[int index]
            {
                get { return ((IssueHistoryRow)(Rows[index])); }
            }

            public event IssueHistoryRowChangeEventHandler IssueHistoryRowChanged;

            public event IssueHistoryRowChangeEventHandler IssueHistoryRowChanging;

            public event IssueHistoryRowChangeEventHandler IssueHistoryRowDeleted;

            public event IssueHistoryRowChangeEventHandler IssueHistoryRowDeleting;

            public void AddIssueHistoryRow(IssueHistoryRow row)
            {
                Rows.Add(row);
            }

            public IssueHistoryRow AddIssueHistoryRow(int StafferID, int IssueID, string Comment, DateTime DateCreated,
                                                      string DisplayName)
            {
                IssueHistoryRow rowIssueHistoryRow = ((IssueHistoryRow)(NewRow()));
                rowIssueHistoryRow.ItemArray = new object[]
                    {
                        null,
                        StafferID,
                        IssueID,
                        Comment,
                        DateCreated,
                        DisplayName
                    };
                Rows.Add(rowIssueHistoryRow);
                return rowIssueHistoryRow;
            }

            public IEnumerator GetEnumerator()
            {
                return Rows.GetEnumerator();
            }

            public override DataTable Clone()
            {
                IssueHistoryDataTable cln = ((IssueHistoryDataTable)(base.Clone()));
                cln.InitVars();
                return cln;
            }

            protected override DataTable CreateInstance()
            {
                return new IssueHistoryDataTable();
            }

            internal void InitVars()
            {
                columnIssueHistoryID = Columns["IssueHistoryID"];
                columnStafferID = Columns["StafferID"];
                columnIssueID = Columns["IssueID"];
                columnComment = Columns["Comment"];
                columnDateCreated = Columns["DateCreated"];
                columnDisplayName = Columns["DisplayName"];
            }

            private void InitClass()
            {
                columnIssueHistoryID = new DataColumn("IssueHistoryID", typeof(int), null, MappingType.Element);
                Columns.Add(columnIssueHistoryID);
                columnStafferID = new DataColumn("StafferID", typeof(int), null, MappingType.Element);
                Columns.Add(columnStafferID);
                columnIssueID = new DataColumn("IssueID", typeof(int), null, MappingType.Element);
                Columns.Add(columnIssueID);
                columnComment = new DataColumn("Comment", typeof(string), null, MappingType.Element);
                Columns.Add(columnComment);
                columnDateCreated = new DataColumn("DateCreated", typeof(DateTime), null, MappingType.Element);
                Columns.Add(columnDateCreated);
                columnDisplayName = new DataColumn("DisplayName", typeof(string), null, MappingType.Element);
                Columns.Add(columnDisplayName);
                columnIssueHistoryID.AutoIncrement = true;
                columnIssueHistoryID.AllowDBNull = false;
                columnIssueHistoryID.ReadOnly = true;
                columnStafferID.AllowDBNull = false;
                columnIssueID.AllowDBNull = false;
                columnComment.AllowDBNull = false;
                columnDateCreated.AllowDBNull = false;
                columnDisplayName.AllowDBNull = false;
            }

            public IssueHistoryRow NewIssueHistoryRow()
            {
                return ((IssueHistoryRow)(NewRow()));
            }

            protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
            {
                return new IssueHistoryRow(builder);
            }

            protected override Type GetRowType()
            {
                return typeof(IssueHistoryRow);
            }

            protected override void OnRowChanged(DataRowChangeEventArgs e)
            {
                base.OnRowChanged(e);
                if ((IssueHistoryRowChanged != null))
                {
                    IssueHistoryRowChanged(this, new IssueHistoryRowChangeEvent(((IssueHistoryRow)(e.Row)), e.Action));
                }
            }

            protected override void OnRowChanging(DataRowChangeEventArgs e)
            {
                base.OnRowChanging(e);
                if ((IssueHistoryRowChanging != null))
                {
                    IssueHistoryRowChanging(this, new IssueHistoryRowChangeEvent(((IssueHistoryRow)(e.Row)), e.Action));
                }
            }

            protected override void OnRowDeleted(DataRowChangeEventArgs e)
            {
                base.OnRowDeleted(e);
                if ((IssueHistoryRowDeleted != null))
                {
                    IssueHistoryRowDeleted(this, new IssueHistoryRowChangeEvent(((IssueHistoryRow)(e.Row)), e.Action));
                }
            }

            protected override void OnRowDeleting(DataRowChangeEventArgs e)
            {
                base.OnRowDeleting(e);
                if ((IssueHistoryRowDeleting != null))
                {
                    IssueHistoryRowDeleting(this, new IssueHistoryRowChangeEvent(((IssueHistoryRow)(e.Row)), e.Action));
                }
            }

            public void RemoveIssueHistoryRow(IssueHistoryRow row)
            {
                Rows.Remove(row);
            }
        }

        //[DebuggerStepThrough()]
        public class IssueHistoryRow : DataRow
        {
            private IssueHistoryDataTable tableIssueHistory;

            internal IssueHistoryRow(DataRowBuilder rb)
                :
                base(rb)
            {
                tableIssueHistory = ((IssueHistoryDataTable)(Table));
            }

            public int IssueHistoryID
            {
                get { return ((int)(this[tableIssueHistory.IssueHistoryIDColumn])); }
                set { this[tableIssueHistory.IssueHistoryIDColumn] = value; }
            }

            public int StafferID
            {
                get { return ((int)(this[tableIssueHistory.StafferIDColumn])); }
                set { this[tableIssueHistory.StafferIDColumn] = value; }
            }

            public int IssueID
            {
                get { return ((int)(this[tableIssueHistory.IssueIDColumn])); }
                set { this[tableIssueHistory.IssueIDColumn] = value; }
            }

            public string Comment
            {
                get { return ((string)(this[tableIssueHistory.CommentColumn])); }
                set { this[tableIssueHistory.CommentColumn] = value; }
            }

            public DateTime DateCreated
            {
                get { return ((DateTime)(this[tableIssueHistory.DateCreatedColumn])); }
                set { this[tableIssueHistory.DateCreatedColumn] = value; }
            }

            public string DisplayName
            {
                get { return ((string)(this[tableIssueHistory.DisplayNameColumn])); }
                set { this[tableIssueHistory.DisplayNameColumn] = value; }
            }
        }

        //[DebuggerStepThrough()]
        public class IssueHistoryRowChangeEvent : EventArgs
        {
            private IssueHistoryRow eventRow;

            private DataRowAction eventAction;

            public IssueHistoryRowChangeEvent(IssueHistoryRow row, DataRowAction action)
            {
                eventRow = row;
                eventAction = action;
            }

            public IssueHistoryRow Row
            {
                get { return eventRow; }
            }

            public DataRowAction Action
            {
                get { return eventAction; }
            }
        }

        //[DebuggerStepThrough()]
        public class IssuesDataTable : DataTable, IEnumerable
        {
            private DataColumn columnIssueID;

            private DataColumn columnStafferID;

            private DataColumn columnIssueTypeID;

            private DataColumn columnTitle;

            private DataColumn columnDescription;

            private DataColumn columnDateOpened;

            private DataColumn columnDateClosed;

            private DataColumn columnIsOpen;

            private DataColumn columnDateModified;

            private DataColumn columnUserName;

            private DataColumn columnDisplayName;

            private DataColumn columnIssueType;

            private DataColumn columnHasConflicts;

            private DataColumn columnIsRead;

            internal IssuesDataTable()
                :
                base("Issues")
            {
                InitClass();
            }

            internal IssuesDataTable(DataTable table)
                :
                base(table.TableName)
            {
                if ((table.CaseSensitive != table.DataSet.CaseSensitive))
                {
                    CaseSensitive = table.CaseSensitive;
                }
                if ((table.Locale.ToString() != table.DataSet.Locale.ToString()))
                {
                    Locale = table.Locale;
                }
                if ((table.Namespace != table.DataSet.Namespace))
                {
                    Namespace = table.Namespace;
                }
                Prefix = table.Prefix;
                MinimumCapacity = table.MinimumCapacity;
                DisplayExpression = table.DisplayExpression;
            }

            public int Count
            {
                get { return Rows.Count; }
            }

            internal DataColumn IssueIDColumn
            {
                get { return columnIssueID; }
            }

            internal DataColumn StafferIDColumn
            {
                get { return columnStafferID; }
            }

            internal DataColumn IssueTypeIDColumn
            {
                get { return columnIssueTypeID; }
            }

            internal DataColumn TitleColumn
            {
                get { return columnTitle; }
            }

            internal DataColumn DescriptionColumn
            {
                get { return columnDescription; }
            }

            internal DataColumn DateOpenedColumn
            {
                get { return columnDateOpened; }
            }

            internal DataColumn DateClosedColumn
            {
                get { return columnDateClosed; }
            }

            internal DataColumn IsOpenColumn
            {
                get { return columnIsOpen; }
            }

            internal DataColumn DateModifiedColumn
            {
                get { return columnDateModified; }
            }

            internal DataColumn UserNameColumn
            {
                get { return columnUserName; }
            }

            internal DataColumn DisplayNameColumn
            {
                get { return columnDisplayName; }
            }

            internal DataColumn IssueTypeColumn
            {
                get { return columnIssueType; }
            }

            internal DataColumn HasConflictsColumn
            {
                get { return columnHasConflicts; }
            }

            internal DataColumn IsReadColumn
            {
                get { return columnIsRead; }
            }

            public IssuesRow this[int index]
            {
                get { return ((IssuesRow)(Rows[index])); }
            }

            public event IssuesRowChangeEventHandler IssuesRowChanged;

            public event IssuesRowChangeEventHandler IssuesRowChanging;

            public event IssuesRowChangeEventHandler IssuesRowDeleted;

            public event IssuesRowChangeEventHandler IssuesRowDeleting;

            public void AddIssuesRow(IssuesRow row)
            {
                Rows.Add(row);
            }

            public IssuesRow AddIssuesRow(int StafferID, int IssueTypeID, string Title, string Description,
                                          DateTime DateOpened, DateTime DateClosed, bool IsOpen, DateTime DateModified,
                                          string UserName, string DisplayName, string IssueType, bool HasConflicts,
                                          bool IsRead)
            {
                IssuesRow rowIssuesRow = ((IssuesRow)(NewRow()));
                rowIssuesRow.ItemArray = new object[]
                    {
                        null,
                        StafferID,
                        IssueTypeID,
                        Title,
                        Description,
                        DateOpened,
                        DateClosed,
                        IsOpen,
                        DateModified,
                        UserName,
                        DisplayName,
                        IssueType,
                        HasConflicts,
                        IsRead
                    };
                Rows.Add(rowIssuesRow);
                return rowIssuesRow;
            }

            public IEnumerator GetEnumerator()
            {
                return Rows.GetEnumerator();
            }

            public override DataTable Clone()
            {
                IssuesDataTable cln = ((IssuesDataTable)(base.Clone()));
                cln.InitVars();
                return cln;
            }

            protected override DataTable CreateInstance()
            {
                return new IssuesDataTable();
            }

            internal void InitVars()
            {
                columnIssueID = Columns["IssueID"];
                columnStafferID = Columns["StafferID"];
                columnIssueTypeID = Columns["IssueTypeID"];
                columnTitle = Columns["Title"];
                columnDescription = Columns["Description"];
                columnDateOpened = Columns["DateOpened"];
                columnDateClosed = Columns["DateClosed"];
                columnIsOpen = Columns["IsOpen"];
                columnDateModified = Columns["DateModified"];
                columnUserName = Columns["UserName"];
                columnDisplayName = Columns["DisplayName"];
                columnIssueType = Columns["IssueType"];
                columnHasConflicts = Columns["HasConflicts"];
                columnIsRead = Columns["IsRead"];
            }

            private void InitClass()
            {
                columnIssueID = new DataColumn("IssueID", typeof(int), null, MappingType.Element);
                Columns.Add(columnIssueID);
                columnStafferID = new DataColumn("StafferID", typeof(int), null, MappingType.Element);
                Columns.Add(columnStafferID);
                columnIssueTypeID = new DataColumn("IssueTypeID", typeof(int), null, MappingType.Element);
                Columns.Add(columnIssueTypeID);
                columnTitle = new DataColumn("Title", typeof(string), null, MappingType.Element);
                Columns.Add(columnTitle);
                columnDescription = new DataColumn("Description", typeof(string), null, MappingType.Element);
                Columns.Add(columnDescription);
                columnDateOpened = new DataColumn("DateOpened", typeof(DateTime), null, MappingType.Element);
                Columns.Add(columnDateOpened);
                columnDateClosed = new DataColumn("DateClosed", typeof(DateTime), null, MappingType.Element);
                Columns.Add(columnDateClosed);
                columnIsOpen = new DataColumn("IsOpen", typeof(bool), null, MappingType.Element);
                Columns.Add(columnIsOpen);
                columnDateModified = new DataColumn("DateModified", typeof(DateTime), null, MappingType.Element);
                Columns.Add(columnDateModified);
                columnUserName = new DataColumn("UserName", typeof(string), null, MappingType.Element);
                Columns.Add(columnUserName);
                columnDisplayName = new DataColumn("DisplayName", typeof(string), null, MappingType.Element);
                Columns.Add(columnDisplayName);
                columnIssueType = new DataColumn("IssueType", typeof(string), null, MappingType.Element);
                Columns.Add(columnIssueType);
                columnHasConflicts = new DataColumn("HasConflicts", typeof(bool), null, MappingType.Element);
                Columns.Add(columnHasConflicts);
                columnIsRead = new DataColumn("IsRead", typeof(bool), null, MappingType.Element);
                Columns.Add(columnIsRead);
                columnIssueID.AutoIncrement = true;
                columnIssueID.AllowDBNull = false;
                columnIssueID.ReadOnly = true;
                columnStafferID.AllowDBNull = false;
                columnIssueTypeID.AllowDBNull = false;
                columnTitle.AllowDBNull = false;
                columnDescription.AllowDBNull = false;
                columnDateOpened.AllowDBNull = false;
                columnIsOpen.AllowDBNull = false;
                columnDateModified.AllowDBNull = false;
                columnUserName.AllowDBNull = false;
                columnDisplayName.AllowDBNull = false;
                columnIssueType.AllowDBNull = false;
                columnHasConflicts.DefaultValue = false;
                columnIsRead.DefaultValue = false;
            }

            public IssuesRow NewIssuesRow()
            {
                return ((IssuesRow)(NewRow()));
            }

            protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
            {
                return new IssuesRow(builder);
            }

            protected override Type GetRowType()
            {
                return typeof(IssuesRow);
            }

            protected override void OnRowChanged(DataRowChangeEventArgs e)
            {
                base.OnRowChanged(e);
                if ((IssuesRowChanged != null))
                {
                    IssuesRowChanged(this, new IssuesRowChangeEvent(((IssuesRow)(e.Row)), e.Action));
                }
            }

            protected override void OnRowChanging(DataRowChangeEventArgs e)
            {
                base.OnRowChanging(e);
                if ((IssuesRowChanging != null))
                {
                    IssuesRowChanging(this, new IssuesRowChangeEvent(((IssuesRow)(e.Row)), e.Action));
                }
            }

            protected override void OnRowDeleted(DataRowChangeEventArgs e)
            {
                base.OnRowDeleted(e);
                if ((IssuesRowDeleted != null))
                {
                    IssuesRowDeleted(this, new IssuesRowChangeEvent(((IssuesRow)(e.Row)), e.Action));
                }
            }

            protected override void OnRowDeleting(DataRowChangeEventArgs e)
            {
                base.OnRowDeleting(e);
                if ((IssuesRowDeleting != null))
                {
                    IssuesRowDeleting(this, new IssuesRowChangeEvent(((IssuesRow)(e.Row)), e.Action));
                }
            }

            public void RemoveIssuesRow(IssuesRow row)
            {
                Rows.Remove(row);
            }
        }

        //[DebuggerStepThrough()]
        public class IssuesRow : DataRow
        {
            private IssuesDataTable tableIssues;

            internal IssuesRow(DataRowBuilder rb)
                :
                base(rb)
            {
                tableIssues = ((IssuesDataTable)(Table));
            }

            public int IssueID
            {
                get { return ((int)(this[tableIssues.IssueIDColumn])); }
                set { this[tableIssues.IssueIDColumn] = value; }
            }

            public int StafferID
            {
                get { return ((int)(this[tableIssues.StafferIDColumn])); }
                set { this[tableIssues.StafferIDColumn] = value; }
            }

            public int IssueTypeID
            {
                get { return ((int)(this[tableIssues.IssueTypeIDColumn])); }
                set { this[tableIssues.IssueTypeIDColumn] = value; }
            }

            public string Title
            {
                get { return ((string)(this[tableIssues.TitleColumn])); }
                set { this[tableIssues.TitleColumn] = value; }
            }

            public string Description
            {
                get { return ((string)(this[tableIssues.DescriptionColumn])); }
                set { this[tableIssues.DescriptionColumn] = value; }
            }

            public DateTime DateOpened
            {
                get { return ((DateTime)(this[tableIssues.DateOpenedColumn])); }
                set { this[tableIssues.DateOpenedColumn] = value; }
            }

            public DateTime DateClosed
            {
                get
                {
                    try
                    {
                        return ((DateTime)(this[tableIssues.DateClosedColumn]));
                    }
                    catch (InvalidCastException e)
                    {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set { this[tableIssues.DateClosedColumn] = value; }
            }

            public bool IsOpen
            {
                get { return ((bool)(this[tableIssues.IsOpenColumn])); }
                set { this[tableIssues.IsOpenColumn] = value; }
            }

            public DateTime DateModified
            {
                get { return ((DateTime)(this[tableIssues.DateModifiedColumn])); }
                set { this[tableIssues.DateModifiedColumn] = value; }
            }

            public string UserName
            {
                get { return ((string)(this[tableIssues.UserNameColumn])); }
                set { this[tableIssues.UserNameColumn] = value; }
            }

            public string DisplayName
            {
                get { return ((string)(this[tableIssues.DisplayNameColumn])); }
                set { this[tableIssues.DisplayNameColumn] = value; }
            }

            public string IssueType
            {
                get { return ((string)(this[tableIssues.IssueTypeColumn])); }
                set { this[tableIssues.IssueTypeColumn] = value; }
            }

            public bool HasConflicts
            {
                get
                {
                    try
                    {
                        return ((bool)(this[tableIssues.HasConflictsColumn]));
                    }
                    catch (InvalidCastException e)
                    {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set { this[tableIssues.HasConflictsColumn] = value; }
            }

            public bool IsRead
            {
                get
                {
                    try
                    {
                        return ((bool)(this[tableIssues.IsReadColumn]));
                    }
                    catch (InvalidCastException e)
                    {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set { this[tableIssues.IsReadColumn] = value; }
            }

            public bool IsDateClosedNull()
            {
                return IsNull(tableIssues.DateClosedColumn);
            }

            public void SetDateClosedNull()
            {
                this[tableIssues.DateClosedColumn] = Convert.DBNull;
            }

            public bool IsHasConflictsNull()
            {
                return IsNull(tableIssues.HasConflictsColumn);
            }

            public void SetHasConflictsNull()
            {
                this[tableIssues.HasConflictsColumn] = Convert.DBNull;
            }

            public bool IsIsReadNull()
            {
                return IsNull(tableIssues.IsReadColumn);
            }

            public void SetIsReadNull()
            {
                this[tableIssues.IsReadColumn] = Convert.DBNull;
            }
        }

        //[DebuggerStepThrough()]
        public class IssuesRowChangeEvent : EventArgs
        {
            private IssuesRow eventRow;

            private DataRowAction eventAction;

            public IssuesRowChangeEvent(IssuesRow row, DataRowAction action)
            {
                eventRow = row;
                eventAction = action;
            }

            public IssuesRow Row
            {
                get { return eventRow; }
            }

            public DataRowAction Action
            {
                get { return eventAction; }
            }
        }

        //[DebuggerStepThrough()]
        public class ConflictsDataTable : DataTable, IEnumerable
        {
            private DataColumn columnIssueID;

            private DataColumn columnStafferID;

            private DataColumn columnIssueTypeID;

            private DataColumn columnTitle;

            private DataColumn columnDescription;

            private DataColumn columnDateOpened;

            private DataColumn columnDateClosed;

            private DataColumn columnIsOpen;

            private DataColumn columnDateModified;

            private DataColumn columnUserName;

            private DataColumn columnDisplayName;

            private DataColumn columnIssueType;

            private DataColumn columnHasConflicts;

            private DataColumn columnIsRead;

            internal ConflictsDataTable()
                :
                base("Conflicts")
            {
                InitClass();
            }

            internal ConflictsDataTable(DataTable table)
                :
                base(table.TableName)
            {
                if ((table.CaseSensitive != table.DataSet.CaseSensitive))
                {
                    CaseSensitive = table.CaseSensitive;
                }
                if ((table.Locale.ToString() != table.DataSet.Locale.ToString()))
                {
                    Locale = table.Locale;
                }
                if ((table.Namespace != table.DataSet.Namespace))
                {
                    Namespace = table.Namespace;
                }
                Prefix = table.Prefix;
                MinimumCapacity = table.MinimumCapacity;
                DisplayExpression = table.DisplayExpression;
            }

            public int Count
            {
                get { return Rows.Count; }
            }

            internal DataColumn IssueIDColumn
            {
                get { return columnIssueID; }
            }

            internal DataColumn StafferIDColumn
            {
                get { return columnStafferID; }
            }

            internal DataColumn IssueTypeIDColumn
            {
                get { return columnIssueTypeID; }
            }

            internal DataColumn TitleColumn
            {
                get { return columnTitle; }
            }

            internal DataColumn DescriptionColumn
            {
                get { return columnDescription; }
            }

            internal DataColumn DateOpenedColumn
            {
                get { return columnDateOpened; }
            }

            internal DataColumn DateClosedColumn
            {
                get { return columnDateClosed; }
            }

            internal DataColumn IsOpenColumn
            {
                get { return columnIsOpen; }
            }

            internal DataColumn DateModifiedColumn
            {
                get { return columnDateModified; }
            }

            internal DataColumn UserNameColumn
            {
                get { return columnUserName; }
            }

            internal DataColumn DisplayNameColumn
            {
                get { return columnDisplayName; }
            }

            internal DataColumn IssueTypeColumn
            {
                get { return columnIssueType; }
            }

            internal DataColumn HasConflictsColumn
            {
                get { return columnHasConflicts; }
            }

            internal DataColumn IsReadColumn
            {
                get { return columnIsRead; }
            }

            public ConflictsRow this[int index]
            {
                get { return ((ConflictsRow)(Rows[index])); }
            }

            public event ConflictsRowChangeEventHandler ConflictsRowChanged;

            public event ConflictsRowChangeEventHandler ConflictsRowChanging;

            public event ConflictsRowChangeEventHandler ConflictsRowDeleted;

            public event ConflictsRowChangeEventHandler ConflictsRowDeleting;

            public void AddConflictsRow(ConflictsRow row)
            {
                Rows.Add(row);
            }

            public ConflictsRow AddConflictsRow(int StafferID, int IssueTypeID, string Title, string Description,
                                                DateTime DateOpened, DateTime DateClosed, bool IsOpen,
                                                DateTime DateModified, string UserName, string DisplayName,
                                                string IssueType, bool HasConflicts, bool IsRead)
            {
                ConflictsRow rowConflictsRow = ((ConflictsRow)(NewRow()));
                rowConflictsRow.ItemArray = new object[]
                    {
                        null,
                        StafferID,
                        IssueTypeID,
                        Title,
                        Description,
                        DateOpened,
                        DateClosed,
                        IsOpen,
                        DateModified,
                        UserName,
                        DisplayName,
                        IssueType,
                        HasConflicts,
                        IsRead
                    };
                Rows.Add(rowConflictsRow);
                return rowConflictsRow;
            }

            public IEnumerator GetEnumerator()
            {
                return Rows.GetEnumerator();
            }

            public override DataTable Clone()
            {
                ConflictsDataTable cln = ((ConflictsDataTable)(base.Clone()));
                cln.InitVars();
                return cln;
            }

            protected override DataTable CreateInstance()
            {
                return new ConflictsDataTable();
            }

            internal void InitVars()
            {
                columnIssueID = Columns["IssueID"];
                columnStafferID = Columns["StafferID"];
                columnIssueTypeID = Columns["IssueTypeID"];
                columnTitle = Columns["Title"];
                columnDescription = Columns["Description"];
                columnDateOpened = Columns["DateOpened"];
                columnDateClosed = Columns["DateClosed"];
                columnIsOpen = Columns["IsOpen"];
                columnDateModified = Columns["DateModified"];
                columnUserName = Columns["UserName"];
                columnDisplayName = Columns["DisplayName"];
                columnIssueType = Columns["IssueType"];
                columnHasConflicts = Columns["HasConflicts"];
                columnIsRead = Columns["IsRead"];
            }

            private void InitClass()
            {
                columnIssueID = new DataColumn("IssueID", typeof(int), null, MappingType.Element);
                Columns.Add(columnIssueID);
                columnStafferID = new DataColumn("StafferID", typeof(int), null, MappingType.Element);
                Columns.Add(columnStafferID);
                columnIssueTypeID = new DataColumn("IssueTypeID", typeof(int), null, MappingType.Element);
                Columns.Add(columnIssueTypeID);
                columnTitle = new DataColumn("Title", typeof(string), null, MappingType.Element);
                Columns.Add(columnTitle);
                columnDescription = new DataColumn("Description", typeof(string), null, MappingType.Element);
                Columns.Add(columnDescription);
                columnDateOpened = new DataColumn("DateOpened", typeof(DateTime), null, MappingType.Element);
                Columns.Add(columnDateOpened);
                columnDateClosed = new DataColumn("DateClosed", typeof(DateTime), null, MappingType.Element);
                Columns.Add(columnDateClosed);
                columnIsOpen = new DataColumn("IsOpen", typeof(bool), null, MappingType.Element);
                Columns.Add(columnIsOpen);
                columnDateModified = new DataColumn("DateModified", typeof(DateTime), null, MappingType.Element);
                Columns.Add(columnDateModified);
                columnUserName = new DataColumn("UserName", typeof(string), null, MappingType.Element);
                Columns.Add(columnUserName);
                columnDisplayName = new DataColumn("DisplayName", typeof(string), null, MappingType.Element);
                Columns.Add(columnDisplayName);
                columnIssueType = new DataColumn("IssueType", typeof(string), null, MappingType.Element);
                Columns.Add(columnIssueType);
                columnHasConflicts = new DataColumn("HasConflicts", typeof(bool), null, MappingType.Element);
                Columns.Add(columnHasConflicts);
                columnIsRead = new DataColumn("IsRead", typeof(bool), null, MappingType.Element);
                Columns.Add(columnIsRead);
                columnIssueID.AutoIncrement = true;
                columnIssueID.AllowDBNull = false;
                columnIssueID.ReadOnly = true;
                columnStafferID.AllowDBNull = false;
                columnIssueTypeID.AllowDBNull = false;
                columnTitle.AllowDBNull = false;
                columnDescription.AllowDBNull = false;
                columnDateOpened.AllowDBNull = false;
                columnIsOpen.AllowDBNull = false;
                columnDateModified.AllowDBNull = false;
                columnUserName.AllowDBNull = false;
                columnDisplayName.AllowDBNull = false;
                columnIssueType.AllowDBNull = false;
                columnHasConflicts.DefaultValue = false;
                columnIsRead.DefaultValue = false;
            }

            public ConflictsRow NewConflictsRow()
            {
                return ((ConflictsRow)(NewRow()));
            }

            protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
            {
                return new ConflictsRow(builder);
            }

            protected override Type GetRowType()
            {
                return typeof(ConflictsRow);
            }

            protected override void OnRowChanged(DataRowChangeEventArgs e)
            {
                base.OnRowChanged(e);
                if ((ConflictsRowChanged != null))
                {
                    ConflictsRowChanged(this, new ConflictsRowChangeEvent(((ConflictsRow)(e.Row)), e.Action));
                }
            }

            protected override void OnRowChanging(DataRowChangeEventArgs e)
            {
                base.OnRowChanging(e);
                if ((ConflictsRowChanging != null))
                {
                    ConflictsRowChanging(this, new ConflictsRowChangeEvent(((ConflictsRow)(e.Row)), e.Action));
                }
            }

            protected override void OnRowDeleted(DataRowChangeEventArgs e)
            {
                base.OnRowDeleted(e);
                if ((ConflictsRowDeleted != null))
                {
                    ConflictsRowDeleted(this, new ConflictsRowChangeEvent(((ConflictsRow)(e.Row)), e.Action));
                }
            }

            protected override void OnRowDeleting(DataRowChangeEventArgs e)
            {
                base.OnRowDeleting(e);
                if ((ConflictsRowDeleting != null))
                {
                    ConflictsRowDeleting(this, new ConflictsRowChangeEvent(((ConflictsRow)(e.Row)), e.Action));
                }
            }

            public void RemoveConflictsRow(ConflictsRow row)
            {
                Rows.Remove(row);
            }
        }

        //[DebuggerStepThrough()]
        public class ConflictsRow : DataRow
        {
            private ConflictsDataTable tableConflicts;

            internal ConflictsRow(DataRowBuilder rb)
                :
                base(rb)
            {
                tableConflicts = ((ConflictsDataTable)(Table));
            }

            public int IssueID
            {
                get { return ((int)(this[tableConflicts.IssueIDColumn])); }
                set { this[tableConflicts.IssueIDColumn] = value; }
            }

            public int StafferID
            {
                get { return ((int)(this[tableConflicts.StafferIDColumn])); }
                set { this[tableConflicts.StafferIDColumn] = value; }
            }

            public int IssueTypeID
            {
                get { return ((int)(this[tableConflicts.IssueTypeIDColumn])); }
                set { this[tableConflicts.IssueTypeIDColumn] = value; }
            }

            public string Title
            {
                get { return ((string)(this[tableConflicts.TitleColumn])); }
                set { this[tableConflicts.TitleColumn] = value; }
            }

            public string Description
            {
                get { return ((string)(this[tableConflicts.DescriptionColumn])); }
                set { this[tableConflicts.DescriptionColumn] = value; }
            }

            public DateTime DateOpened
            {
                get { return ((DateTime)(this[tableConflicts.DateOpenedColumn])); }
                set { this[tableConflicts.DateOpenedColumn] = value; }
            }

            public DateTime DateClosed
            {
                get
                {
                    try
                    {
                        return ((DateTime)(this[tableConflicts.DateClosedColumn]));
                    }
                    catch (InvalidCastException e)
                    {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set { this[tableConflicts.DateClosedColumn] = value; }
            }

            public bool IsOpen
            {
                get { return ((bool)(this[tableConflicts.IsOpenColumn])); }
                set { this[tableConflicts.IsOpenColumn] = value; }
            }

            public DateTime DateModified
            {
                get { return ((DateTime)(this[tableConflicts.DateModifiedColumn])); }
                set { this[tableConflicts.DateModifiedColumn] = value; }
            }

            public string UserName
            {
                get { return ((string)(this[tableConflicts.UserNameColumn])); }
                set { this[tableConflicts.UserNameColumn] = value; }
            }

            public string DisplayName
            {
                get { return ((string)(this[tableConflicts.DisplayNameColumn])); }
                set { this[tableConflicts.DisplayNameColumn] = value; }
            }

            public string IssueType
            {
                get { return ((string)(this[tableConflicts.IssueTypeColumn])); }
                set { this[tableConflicts.IssueTypeColumn] = value; }
            }

            public bool HasConflicts
            {
                get
                {
                    try
                    {
                        return ((bool)(this[tableConflicts.HasConflictsColumn]));
                    }
                    catch (InvalidCastException e)
                    {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set { this[tableConflicts.HasConflictsColumn] = value; }
            }

            public bool IsRead
            {
                get
                {
                    try
                    {
                        return ((bool)(this[tableConflicts.IsReadColumn]));
                    }
                    catch (InvalidCastException e)
                    {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set { this[tableConflicts.IsReadColumn] = value; }
            }

            public bool IsDateClosedNull()
            {
                return IsNull(tableConflicts.DateClosedColumn);
            }

            public void SetDateClosedNull()
            {
                this[tableConflicts.DateClosedColumn] = Convert.DBNull;
            }

            public bool IsHasConflictsNull()
            {
                return IsNull(tableConflicts.HasConflictsColumn);
            }

            public void SetHasConflictsNull()
            {
                this[tableConflicts.HasConflictsColumn] = Convert.DBNull;
            }

            public bool IsIsReadNull()
            {
                return IsNull(tableConflicts.IsReadColumn);
            }

            public void SetIsReadNull()
            {
                this[tableConflicts.IsReadColumn] = Convert.DBNull;
            }
        }

        //[DebuggerStepThrough()]
        public class ConflictsRowChangeEvent : EventArgs
        {
            private ConflictsRow eventRow;

            private DataRowAction eventAction;

            public ConflictsRowChangeEvent(ConflictsRow row, DataRowAction action)
            {
                eventRow = row;
                eventAction = action;
            }

            public ConflictsRow Row
            {
                get { return eventRow; }
            }

            public DataRowAction Action
            {
                get { return eventAction; }
            }
        }

        //[DebuggerStepThrough()]
        public class IssueTypesDataTable : DataTable, IEnumerable
        {
            private DataColumn columnIssueTypeID;

            private DataColumn columnIssueType;

            internal IssueTypesDataTable()
                :
                base("IssueTypes")
            {
                InitClass();
            }

            internal IssueTypesDataTable(DataTable table)
                :
                base(table.TableName)
            {
                if ((table.CaseSensitive != table.DataSet.CaseSensitive))
                {
                    CaseSensitive = table.CaseSensitive;
                }
                if ((table.Locale.ToString() != table.DataSet.Locale.ToString()))
                {
                    Locale = table.Locale;
                }
                if ((table.Namespace != table.DataSet.Namespace))
                {
                    Namespace = table.Namespace;
                }
                Prefix = table.Prefix;
                MinimumCapacity = table.MinimumCapacity;
                DisplayExpression = table.DisplayExpression;
            }

            public int Count
            {
                get { return Rows.Count; }
            }

            internal DataColumn IssueTypeIDColumn
            {
                get { return columnIssueTypeID; }
            }

            internal DataColumn IssueTypeColumn
            {
                get { return columnIssueType; }
            }

            public IssueTypesRow this[int index]
            {
                get { return ((IssueTypesRow)(Rows[index])); }
            }

            public event IssueTypesRowChangeEventHandler IssueTypesRowChanged;

            public event IssueTypesRowChangeEventHandler IssueTypesRowChanging;

            public event IssueTypesRowChangeEventHandler IssueTypesRowDeleted;

            public event IssueTypesRowChangeEventHandler IssueTypesRowDeleting;

            public void AddIssueTypesRow(IssueTypesRow row)
            {
                Rows.Add(row);
            }

            public IssueTypesRow AddIssueTypesRow(string IssueType)
            {
                IssueTypesRow rowIssueTypesRow = ((IssueTypesRow)(NewRow()));
                rowIssueTypesRow.ItemArray = new object[]
                    {
                        null,
                        IssueType
                    };
                Rows.Add(rowIssueTypesRow);
                return rowIssueTypesRow;
            }

            public IEnumerator GetEnumerator()
            {
                return Rows.GetEnumerator();
            }

            public override DataTable Clone()
            {
                IssueTypesDataTable cln = ((IssueTypesDataTable)(base.Clone()));
                cln.InitVars();
                return cln;
            }

            protected override DataTable CreateInstance()
            {
                return new IssueTypesDataTable();
            }

            internal void InitVars()
            {
                columnIssueTypeID = Columns["IssueTypeID"];
                columnIssueType = Columns["IssueType"];
            }

            private void InitClass()
            {
                columnIssueTypeID = new DataColumn("IssueTypeID", typeof(int), null, MappingType.Element);
                Columns.Add(columnIssueTypeID);
                columnIssueType = new DataColumn("IssueType", typeof(string), null, MappingType.Element);
                Columns.Add(columnIssueType);
                columnIssueTypeID.AutoIncrement = true;
                columnIssueTypeID.AllowDBNull = false;
                columnIssueTypeID.ReadOnly = true;
                columnIssueType.AllowDBNull = false;
            }

            public IssueTypesRow NewIssueTypesRow()
            {
                return ((IssueTypesRow)(NewRow()));
            }

            protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
            {
                return new IssueTypesRow(builder);
            }

            protected override Type GetRowType()
            {
                return typeof(IssueTypesRow);
            }

            protected override void OnRowChanged(DataRowChangeEventArgs e)
            {
                base.OnRowChanged(e);
                if ((IssueTypesRowChanged != null))
                {
                    IssueTypesRowChanged(this, new IssueTypesRowChangeEvent(((IssueTypesRow)(e.Row)), e.Action));
                }
            }

            protected override void OnRowChanging(DataRowChangeEventArgs e)
            {
                base.OnRowChanging(e);
                if ((IssueTypesRowChanging != null))
                {
                    IssueTypesRowChanging(this, new IssueTypesRowChangeEvent(((IssueTypesRow)(e.Row)), e.Action));
                }
            }

            protected override void OnRowDeleted(DataRowChangeEventArgs e)
            {
                base.OnRowDeleted(e);
                if ((IssueTypesRowDeleted != null))
                {
                    IssueTypesRowDeleted(this, new IssueTypesRowChangeEvent(((IssueTypesRow)(e.Row)), e.Action));
                }
            }

            protected override void OnRowDeleting(DataRowChangeEventArgs e)
            {
                base.OnRowDeleting(e);
                if ((IssueTypesRowDeleting != null))
                {
                    IssueTypesRowDeleting(this, new IssueTypesRowChangeEvent(((IssueTypesRow)(e.Row)), e.Action));
                }
            }

            public void RemoveIssueTypesRow(IssueTypesRow row)
            {
                Rows.Remove(row);
            }
        }

        //[DebuggerStepThrough()]
        public class IssueTypesRow : DataRow
        {
            private IssueTypesDataTable tableIssueTypes;

            internal IssueTypesRow(DataRowBuilder rb)
                :
                base(rb)
            {
                tableIssueTypes = ((IssueTypesDataTable)(Table));
            }

            public int IssueTypeID
            {
                get { return ((int)(this[tableIssueTypes.IssueTypeIDColumn])); }
                set { this[tableIssueTypes.IssueTypeIDColumn] = value; }
            }

            public string IssueType
            {
                get { return ((string)(this[tableIssueTypes.IssueTypeColumn])); }
                set { this[tableIssueTypes.IssueTypeColumn] = value; }
            }
        }

        //[DebuggerStepThrough()]
        public class IssueTypesRowChangeEvent : EventArgs
        {
            private IssueTypesRow eventRow;

            private DataRowAction eventAction;

            public IssueTypesRowChangeEvent(IssueTypesRow row, DataRowAction action)
            {
                eventRow = row;
                eventAction = action;
            }

            public IssueTypesRow Row
            {
                get { return eventRow; }
            }

            public DataRowAction Action
            {
                get { return eventAction; }
            }
        }

        //[DebuggerStepThrough()]
        public class StaffersDataTable : DataTable, IEnumerable
        {
            private DataColumn columnStafferID;

            private DataColumn columnUserName;

            private DataColumn columnDisplayName;

            private DataColumn columnStafferType;

            internal StaffersDataTable()
                :
                base("Staffers")
            {
                InitClass();
            }

            internal StaffersDataTable(DataTable table)
                :
                base(table.TableName)
            {
                if ((table.CaseSensitive != table.DataSet.CaseSensitive))
                {
                    CaseSensitive = table.CaseSensitive;
                }
                if ((table.Locale.ToString() != table.DataSet.Locale.ToString()))
                {
                    Locale = table.Locale;
                }
                if ((table.Namespace != table.DataSet.Namespace))
                {
                    Namespace = table.Namespace;
                }
                Prefix = table.Prefix;
                MinimumCapacity = table.MinimumCapacity;
                DisplayExpression = table.DisplayExpression;
            }

            public int Count
            {
                get { return Rows.Count; }
            }

            internal DataColumn StafferIDColumn
            {
                get { return columnStafferID; }
            }

            internal DataColumn UserNameColumn
            {
                get { return columnUserName; }
            }

            internal DataColumn DisplayNameColumn
            {
                get { return columnDisplayName; }
            }

            internal DataColumn StafferTypeColumn
            {
                get { return columnStafferType; }
            }

            public StaffersRow this[int index]
            {
                get { return ((StaffersRow)(Rows[index])); }
            }

            public event StaffersRowChangeEventHandler StaffersRowChanged;

            public event StaffersRowChangeEventHandler StaffersRowChanging;

            public event StaffersRowChangeEventHandler StaffersRowDeleted;

            public event StaffersRowChangeEventHandler StaffersRowDeleting;

            public void AddStaffersRow(StaffersRow row)
            {
                Rows.Add(row);
            }

            public StaffersRow AddStaffersRow(string UserName, string DisplayName, string StafferType)
            {
                StaffersRow rowStaffersRow = ((StaffersRow)(NewRow()));
                rowStaffersRow.ItemArray = new object[]
                    {
                        null,
                        UserName,
                        DisplayName,
                        StafferType
                    };
                Rows.Add(rowStaffersRow);
                return rowStaffersRow;
            }

            public IEnumerator GetEnumerator()
            {
                return Rows.GetEnumerator();
            }

            public override DataTable Clone()
            {
                StaffersDataTable cln = ((StaffersDataTable)(base.Clone()));
                cln.InitVars();
                return cln;
            }

            protected override DataTable CreateInstance()
            {
                return new StaffersDataTable();
            }

            internal void InitVars()
            {
                columnStafferID = Columns["StafferID"];
                columnUserName = Columns["UserName"];
                columnDisplayName = Columns["DisplayName"];
                columnStafferType = Columns["StafferType"];
            }

            private void InitClass()
            {
                columnStafferID = new DataColumn("StafferID", typeof(int), null, MappingType.Element);
                Columns.Add(columnStafferID);
                columnUserName = new DataColumn("UserName", typeof(string), null, MappingType.Element);
                Columns.Add(columnUserName);
                columnDisplayName = new DataColumn("DisplayName", typeof(string), null, MappingType.Element);
                Columns.Add(columnDisplayName);
                columnStafferType = new DataColumn("StafferType", typeof(string), null, MappingType.Element);
                Columns.Add(columnStafferType);
                columnStafferID.AutoIncrement = true;
                columnStafferID.AllowDBNull = false;
                columnStafferID.ReadOnly = true;
                columnUserName.AllowDBNull = false;
                columnDisplayName.AllowDBNull = false;
                columnStafferType.AllowDBNull = false;
            }

            public StaffersRow NewStaffersRow()
            {
                return ((StaffersRow)(NewRow()));
            }

            protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
            {
                return new StaffersRow(builder);
            }

            protected override Type GetRowType()
            {
                return typeof(StaffersRow);
            }

            protected override void OnRowChanged(DataRowChangeEventArgs e)
            {
                base.OnRowChanged(e);
                if ((StaffersRowChanged != null))
                {
                    StaffersRowChanged(this, new StaffersRowChangeEvent(((StaffersRow)(e.Row)), e.Action));
                }
            }

            protected override void OnRowChanging(DataRowChangeEventArgs e)
            {
                base.OnRowChanging(e);
                if ((StaffersRowChanging != null))
                {
                    StaffersRowChanging(this, new StaffersRowChangeEvent(((StaffersRow)(e.Row)), e.Action));
                }
            }

            protected override void OnRowDeleted(DataRowChangeEventArgs e)
            {
                base.OnRowDeleted(e);
                if ((StaffersRowDeleted != null))
                {
                    StaffersRowDeleted(this, new StaffersRowChangeEvent(((StaffersRow)(e.Row)), e.Action));
                }
            }

            protected override void OnRowDeleting(DataRowChangeEventArgs e)
            {
                base.OnRowDeleting(e);
                if ((StaffersRowDeleting != null))
                {
                    StaffersRowDeleting(this, new StaffersRowChangeEvent(((StaffersRow)(e.Row)), e.Action));
                }
            }

            public void RemoveStaffersRow(StaffersRow row)
            {
                Rows.Remove(row);
            }
        }

        //[DebuggerStepThrough()]
        public class StaffersRow : DataRow
        {
            private StaffersDataTable tableStaffers;

            internal StaffersRow(DataRowBuilder rb)
                :
                base(rb)
            {
                tableStaffers = ((StaffersDataTable)(Table));
            }

            public int StafferID
            {
                get { return ((int)(this[tableStaffers.StafferIDColumn])); }
                set { this[tableStaffers.StafferIDColumn] = value; }
            }

            public string UserName
            {
                get { return ((string)(this[tableStaffers.UserNameColumn])); }
                set { this[tableStaffers.UserNameColumn] = value; }
            }

            public string DisplayName
            {
                get { return ((string)(this[tableStaffers.DisplayNameColumn])); }
                set { this[tableStaffers.DisplayNameColumn] = value; }
            }

            public string StafferType
            {
                get { return ((string)(this[tableStaffers.StafferTypeColumn])); }
                set { this[tableStaffers.StafferTypeColumn] = value; }
            }
        }

        //[DebuggerStepThrough()]
        public class StaffersRowChangeEvent : EventArgs
        {
            private StaffersRow eventRow;

            private DataRowAction eventAction;

            public StaffersRowChangeEvent(StaffersRow row, DataRowAction action)
            {
                eventRow = row;
                eventAction = action;
            }

            public StaffersRow Row
            {
                get { return eventRow; }
            }

            public DataRowAction Action
            {
                get { return eventAction; }
            }
        }
    }
}
#endregion

class X
{
    static string diffgram =
"<diffgr:diffgram xmlns:msdata=\"urn:schemas-microsoft-com:xml-msdata\" xmlns:diffgr=\"urn:schemas-microsoft-com:xml-diffgram-v1\">\n"
+ "  <IssueDetails xmlns=\"http://www.tempuri.org/IVDataSet.xsd\">\n"
+ "    <IssueHistory diffgr:id=\"IssueHistory1\" msdata:rowOrder=\"0\">\n"
+ "      <IssueHistoryID>11</IssueHistoryID>\n"
+ "      <StafferID>1</StafferID>\n"
+ "      <IssueID>12</IssueID>\n"
+ "      <Comment>Issue created.</Comment>\n"
+ "      <DateCreated>2003-12-29T16:03:42.0000000+06:00</DateCreated>\n"
+ "      <DisplayName>Adam Barr</DisplayName>\n"
+ "    </IssueHistory>\n"
+ "    <IssueHistory diffgr:id=\"IssueHistory2\" msdata:rowOrder=\"1\">\n"
+ "      <IssueHistoryID>12</IssueHistoryID>\n"
+ "      <StafferID>2</StafferID>\n"
+ "      <IssueID>13</IssueID>\n"
+ "      <Comment>Issue created.</Comment>\n"
+ "      <DateCreated>2003-12-29T16:07:13.0000000+06:00</DateCreated>\n"
+ "      <DisplayName>Kim Abercrombie</DisplayName>\n"
+ "    </IssueHistory>\n"
+ "    <IssueHistory diffgr:id=\"IssueHistory3\" msdata:rowOrder=\"2\">\n"
+ "      <IssueHistoryID>13</IssueHistoryID>\n"
+ "      <StafferID>3</StafferID>\n"
+ "      <IssueID>14</IssueID>\n"
+ "      <Comment>Issue created.</Comment>\n"
+ "      <DateCreated>2003-12-29T16:09:54.0000000+06:00</DateCreated>\n"
+ "      <DisplayName>Rob Young</DisplayName>\n"
+ "    </IssueHistory>\n"
+ "    <IssueHistory diffgr:id=\"IssueHistory4\" msdata:rowOrder=\"3\">\n"
+ "      <IssueHistoryID>14</IssueHistoryID>\n"
+ "      <StafferID>2</StafferID>\n"
+ "      <IssueID>15</IssueID>\n"
+ "      <Comment>Issue created.</Comment>\n"
+ "      <DateCreated>2003-12-29T16:12:33.0000000+06:00</DateCreated>\n"
+ "      <DisplayName>Kim Abercrombie</DisplayName>\n"
+ "    </IssueHistory>\n"
+ "    <IssueHistory diffgr:id=\"IssueHistory5\" msdata:rowOrder=\"4\">\n"
+ "      <IssueHistoryID>15</IssueHistoryID>\n"
+ "      <StafferID>1</StafferID>\n"
+ "      <IssueID>16</IssueID>\n"
+ "      <Comment>Issue created.</Comment>\n"
+ "      <DateCreated>2003-12-29T16:14:08.0000000+06:00</DateCreated>\n"
+ "      <DisplayName>Adam Barr</DisplayName>\n"
+ "    </IssueHistory>\n"
+ "    <IssueHistory diffgr:id=\"IssueHistory6\" msdata:rowOrder=\"5\">\n"
+ "      <IssueHistoryID>16</IssueHistoryID>\n"
+ "      <StafferID>3</StafferID>\n"
+ "      <IssueID>17</IssueID>\n"
+ "      <Comment>Issue created.</Comment>\n"
+ "      <DateCreated>2003-12-29T16:15:19.0000000+06:00</DateCreated>\n"
+ "      <DisplayName>Rob Young</DisplayName>\n"
+ "    </IssueHistory>\n"
+ "    <IssueHistory diffgr:id=\"IssueHistory7\" msdata:rowOrder=\"6\">\n"
+ "      <IssueHistoryID>17</IssueHistoryID>\n"
+ "      <StafferID>2</StafferID>\n"
+ "      <IssueID>18</IssueID>\n"
+ "      <Comment>Issue created.</Comment>\n"
+ "      <DateCreated>2003-12-29T16:17:25.0000000+06:00</DateCreated>\n"
+ "      <DisplayName>Kim Abercrombie</DisplayName>\n"
+ "    </IssueHistory>\n"
+ "    <IssueHistory diffgr:id=\"IssueHistory8\" msdata:rowOrder=\"7\">\n"
+ "      <IssueHistoryID>18</IssueHistoryID>\n"
+ "      <StafferID>12</StafferID>\n"
+ "      <IssueID>13</IssueID>\n"
+ "      <Comment>test</Comment>\n"
+ "      <DateCreated>2004-01-10T10:09:11.0000000+06:00</DateCreated>\n"
+ "      <DisplayName>Demo</DisplayName>\n"
+ "    </IssueHistory>\n"
+ "    <IssueHistory diffgr:id=\"IssueHistory9\" msdata:rowOrder=\"8\">\n"
+ "      <IssueHistoryID>19</IssueHistoryID>\n"
+ "      <StafferID>1</StafferID>\n"
+ "      <IssueID>19</IssueID>\n"
+ "      <Comment>Issue created.</Comment>\n"
+ "      <DateCreated>2004-01-11T12:41:55.0000000+06:00</DateCreated>\n"
+ "      <DisplayName>Adam Barr</DisplayName>\n"
+ "    </IssueHistory>\n"
+ "    <IssueHistory diffgr:id=\"IssueHistory10\" msdata:rowOrder=\"9\">\n"
+ "      <IssueHistoryID>20</IssueHistoryID>\n"
+ "      <StafferID>2</StafferID>\n"
+ "      <IssueID>20</IssueID>\n"
+ "      <Comment>Issue created.</Comment>\n"
+ "      <DateCreated>2004-01-11T12:41:56.0000000+06:00</DateCreated>\n"
+ "      <DisplayName>Kim Abercrombie</DisplayName>\n"
+ "    </IssueHistory>\n"
+ "    <IssueHistory diffgr:id=\"IssueHistory11\" msdata:rowOrder=\"10\">\n"
+ "      <IssueHistoryID>21</IssueHistoryID>\n"
+ "      <StafferID>3</StafferID>\n"
+ "      <IssueID>21</IssueID>\n"
+ "      <Comment>Issue created.</Comment>\n"
+ "      <DateCreated>2004-01-11T12:41:56.0000000+06:00</DateCreated>\n"
+ "      <DisplayName>Rob Young</DisplayName>\n"
+ "    </IssueHistory>\n"
+ "    <IssueHistory diffgr:id=\"IssueHistory12\" msdata:rowOrder=\"11\">\n"
+ "      <IssueHistoryID>22</IssueHistoryID>\n"
+ "      <StafferID>4</StafferID>\n"
+ "      <IssueID>22</IssueID>\n"
+ "      <Comment>Issue created.</Comment>\n"
+ "      <DateCreated>2004-01-11T12:41:56.0000000+06:00</DateCreated>\n"
+ "      <DisplayName>Tom Youtsey</DisplayName>\n"
+ "    </IssueHistory>\n"
+ "    <IssueHistory diffgr:id=\"IssueHistory13\" msdata:rowOrder=\"12\">\n"
+ "      <IssueHistoryID>23</IssueHistoryID>\n"
+ "      <StafferID>5</StafferID>\n"
+ "      <IssueID>23</IssueID>\n"
+ "      <Comment>Issue created.</Comment>\n"
+ "      <DateCreated>2004-01-11T12:41:56.0000000+06:00</DateCreated>\n"
+ "      <DisplayName>Gary W. Yukish</DisplayName>\n"
+ "    </IssueHistory>\n"
+ "    <IssueHistory diffgr:id=\"IssueHistory14\" msdata:rowOrder=\"13\">\n"
+ "      <IssueHistoryID>24</IssueHistoryID>\n"
+ "      <StafferID>6</StafferID>\n"
+ "      <IssueID>24</IssueID>\n"
+ "      <Comment>Issue created.</Comment>\n"
+ "      <DateCreated>2004-01-11T12:41:56.0000000+06:00</DateCreated>\n"
+ "      <DisplayName>Rob Caron</DisplayName>\n"
+ "    </IssueHistory>\n"
+ "    <IssueHistory diffgr:id=\"IssueHistory15\" msdata:rowOrder=\"14\">\n"
+ "      <IssueHistoryID>25</IssueHistoryID>\n"
+ "      <StafferID>7</StafferID>\n"
+ "      <IssueID>25</IssueID>\n"
+ "      <Comment>Issue created.</Comment>\n"
+ "      <DateCreated>2004-01-11T12:41:56.0000000+06:00</DateCreated>\n"
+ "      <DisplayName>Karin Zimprich</DisplayName>\n"
+ "    </IssueHistory>\n"
+ "    <IssueHistory diffgr:id=\"IssueHistory16\" msdata:rowOrder=\"15\">\n"
+ "      <IssueHistoryID>26</IssueHistoryID>\n"
+ "      <StafferID>8</StafferID>\n"
+ "      <IssueID>26</IssueID>\n"
+ "      <Comment>Issue created.</Comment>\n"
+ "      <DateCreated>2004-01-11T12:41:56.0000000+06:00</DateCreated>\n"
+ "      <DisplayName>Randall Boseman</DisplayName>\n"
+ "    </IssueHistory>\n"
+ "    <IssueHistory diffgr:id=\"IssueHistory17\" msdata:rowOrder=\"16\">\n"
+ "      <IssueHistoryID>27</IssueHistoryID>\n"
+ "      <StafferID>9</StafferID>\n"
+ "      <IssueID>27</IssueID>\n"
+ "      <Comment>Issue created.</Comment>\n"
+ "      <DateCreated>2004-01-11T12:41:56.0000000+06:00</DateCreated>\n"
+ "      <DisplayName>Kevin Kennedy</DisplayName>\n"
+ "    </IssueHistory>\n"
+ "    <IssueHistory diffgr:id=\"IssueHistory18\" msdata:rowOrder=\"17\">\n"
+ "      <IssueHistoryID>28</IssueHistoryID>\n"
+ "      <StafferID>10</StafferID>\n"
+ "      <IssueID>28</IssueID>\n"
+ "      <Comment>Issue created.</Comment>\n"
+ "      <DateCreated>2004-01-11T12:41:56.0000000+06:00</DateCreated>\n"
+ "      <DisplayName>Diane Tibbott</DisplayName>\n"
+ "    </IssueHistory>\n"
+ "    <IssueHistory diffgr:id=\"IssueHistory19\" msdata:rowOrder=\"18\">\n"
+ "      <IssueHistoryID>29</IssueHistoryID>\n"
+ "      <StafferID>11</StafferID>\n"
+ "      <IssueID>29</IssueID>\n"
+ "      <Comment>Issue created.</Comment>\n"
+ "      <DateCreated>2004-01-11T12:41:56.0000000+06:00</DateCreated>\n"
+ "      <DisplayName>Garrett Young</DisplayName>\n"
+ "    </IssueHistory>\n"
+ "    <IssueHistory diffgr:id=\"IssueHistory20\" msdata:rowOrder=\"19\">\n"
+ "      <IssueHistoryID>30</IssueHistoryID>\n"
+ "      <StafferID>12</StafferID>\n"
+ "      <IssueID>30</IssueID>\n"
+ "      <Comment>Issue created.</Comment>\n"
+ "      <DateCreated>2004-01-11T12:41:56.0000000+06:00</DateCreated>\n"
+ "      <DisplayName>Demo</DisplayName>\n"
+ "    </IssueHistory>\n"
+ "    <Issues diffgr:id=\"Issues1\" msdata:rowOrder=\"0\">\n"
+ "      <IssueID>12</IssueID>\n"
+ "      <StafferID>1</StafferID>\n"
+ "      <IssueTypeID>3</IssueTypeID>\n"
+ "      <Title>Computer won't shut down</Title>\n"
+ "      <Description>Computer just hangs after clicking Shut Down. User must manually turn computer off.</Description>\n"
+ "      <DateOpened>2003-12-29T16:03:42.0000000+06:00</DateOpened>\n"
+ "      <IsOpen>true</IsOpen>\n"
+ "      <DateModified>2008-03-25T15:08:47.2530000+06:00</DateModified>\n"
+ "      <UserName>abarr</UserName>\n"
+ "      <DisplayName>Adam Barr</DisplayName>\n"
+ "      <IssueType>Computer</IssueType>\n"
+ "      <HasConflicts>false</HasConflicts>\n"
+ "      <IsRead>false</IsRead>\n"
+ "    </Issues>\n"
+ "    <Issues diffgr:id=\"Issues2\" msdata:rowOrder=\"1\">\n"
+ "      <IssueID>13</IssueID>\n"
+ "      <StafferID>2</StafferID>\n"
+ "      <IssueTypeID>1</IssueTypeID>\n"
+ "      <Title>Voice mail password doesn't work</Title>\n"
+ "      <Description>The user went away on vacation and came back to find that his voice mail password didn't work.</Description>\n"
+ "      <DateOpened>2003-12-29T16:07:13.0000000+06:00</DateOpened>\n"
+ "      <IsOpen>true</IsOpen>\n"
+ "      <DateModified>2008-03-25T15:08:47.2530000+06:00</DateModified>\n"
+ "      <UserName>kabercrombie</UserName>\n"
+ "      <DisplayName>Kim Abercrombie</DisplayName>\n"
+ "      <IssueType>Telecommunications</IssueType>\n"
+ "      <HasConflicts>false</HasConflicts>\n"
+ "      <IsRead>false</IsRead>\n"
+ "    </Issues>\n"
+ "    <Issues diffgr:id=\"Issues3\" msdata:rowOrder=\"2\">\n"
+ "      <IssueID>14</IssueID>\n"
+ "      <StafferID>3</StafferID>\n"
+ "      <IssueTypeID>10</IssueTypeID>\n"
+ "      <Title>User can't log into network</Title>\n"
+ "      <Description>The user gets a message that her account has been locked.</Description>\n"
+ "      <DateOpened>2003-12-29T16:09:54.0000000+06:00</DateOpened>\n"
+ "      <IsOpen>true</IsOpen>\n"
+ "      <DateModified>2008-03-25T15:08:47.2530000+06:00</DateModified>\n"
+ "      <UserName>ryoung</UserName>\n"
+ "      <DisplayName>Rob Young</DisplayName>\n"
+ "      <IssueType>Network</IssueType>\n"
+ "      <HasConflicts>false</HasConflicts>\n"
+ "      <IsRead>false</IsRead>\n"
+ "    </Issues>\n"
+ "    <Issues diffgr:id=\"Issues4\" msdata:rowOrder=\"3\">\n"
+ "      <IssueID>15</IssueID>\n"
+ "      <StafferID>2</StafferID>\n"
+ "      <IssueTypeID>5</IssueTypeID>\n"
+ "      <Title>HERCULES printer driver not installed</Title>\n"
+ "      <Description>The user gets a message that the driver for printer HERCULES is not installed.</Description>\n"
+ "      <DateOpened>2003-12-29T16:12:33.0000000+06:00</DateOpened>\n"
+ "      <IsOpen>true</IsOpen>\n"
+ "      <DateModified>2008-03-25T15:08:47.2530000+06:00</DateModified>\n"
+ "      <UserName>kabercrombie</UserName>\n"
+ "      <DisplayName>Kim Abercrombie</DisplayName>\n"
+ "      <IssueType>Printer</IssueType>\n"
+ "      <HasConflicts>false</HasConflicts>\n"
+ "      <IsRead>false</IsRead>\n"
+ "    </Issues>\n"
+ "    <Issues diffgr:id=\"Issues5\" msdata:rowOrder=\"4\">\n"
+ "      <IssueID>16</IssueID>\n"
+ "      <StafferID>1</StafferID>\n"
+ "      <IssueTypeID>10</IssueTypeID>\n"
+ "      <Title>User's VPN access not working</Title>\n"
+ "      <Description>The user's home computer can't connect to the VPN server remotely.</Description>\n"
+ "      <DateOpened>2003-12-29T16:14:08.0000000+06:00</DateOpened>\n"
+ "      <IsOpen>true</IsOpen>\n"
+ "      <DateModified>2008-03-25T15:08:47.2530000+06:00</DateModified>\n"
+ "      <UserName>abarr</UserName>\n"
+ "      <DisplayName>Adam Barr</DisplayName>\n"
+ "      <IssueType>Network</IssueType>\n"
+ "      <HasConflicts>false</HasConflicts>\n"
+ "      <IsRead>false</IsRead>\n"
+ "    </Issues>\n"
+ "    <Issues diffgr:id=\"Issues6\" msdata:rowOrder=\"5\">\n"
+ "      <IssueID>17</IssueID>\n"
+ "      <StafferID>3</StafferID>\n"
+ "      <IssueTypeID>1</IssueTypeID>\n"
+ "      <Title>9 key not working</Title>\n"
+ "      <Description>The 9 key on the user's telephone does not work.</Description>\n"
+ "      <DateOpened>2003-12-29T16:15:19.0000000+06:00</DateOpened>\n"
+ "      <IsOpen>true</IsOpen>\n"
+ "      <DateModified>2008-03-25T15:08:47.2530000+06:00</DateModified>\n"
+ "      <UserName>ryoung</UserName>\n"
+ "      <DisplayName>Rob Young</DisplayName>\n"
+ "      <IssueType>Telecommunications</IssueType>\n"
+ "      <HasConflicts>false</HasConflicts>\n"
+ "      <IsRead>false</IsRead>\n"
+ "    </Issues>\n"
+ "    <Issues diffgr:id=\"Issues7\" msdata:rowOrder=\"6\">\n"
+ "      <IssueID>18</IssueID>\n"
+ "      <StafferID>2</StafferID>\n"
+ "      <IssueTypeID>3</IssueTypeID>\n"
+ "      <Title>Needs an upgrade to Office 2003</Title>\n"
+ "      <Description>Authorized and requested by department head.</Description>\n"
+ "      <DateOpened>2003-12-29T16:17:25.0000000+06:00</DateOpened>\n"
+ "      <IsOpen>true</IsOpen>\n"
+ "      <DateModified>2008-03-25T15:08:47.2530000+06:00</DateModified>\n"
+ "      <UserName>kabercrombie</UserName>\n"
+ "      <DisplayName>Kim Abercrombie</DisplayName>\n"
+ "      <IssueType>Computer</IssueType>\n"
+ "      <HasConflicts>false</HasConflicts>\n"
+ "      <IsRead>false</IsRead>\n"
+ "    </Issues>\n"
+ "    <Issues diffgr:id=\"Issues8\" msdata:rowOrder=\"7\">\n"
+ "      <IssueID>19</IssueID>\n"
+ "      <StafferID>1</StafferID>\n"
+ "      <IssueTypeID>1</IssueTypeID>\n"
+ "      <Title>Voice mail light does not work</Title>\n"
+ "      <Description/>\n"
+ "      <DateOpened>2004-01-11T12:41:55.0000000+06:00</DateOpened>\n"
+ "      <IsOpen>true</IsOpen>\n"
+ "      <DateModified>2008-03-25T15:08:47.2530000+06:00</DateModified>\n"
+ "      <UserName>abarr</UserName>\n"
+ "      <DisplayName>Adam Barr</DisplayName>\n"
+ "      <IssueType>Telecommunications</IssueType>\n"
+ "      <HasConflicts>false</HasConflicts>\n"
+ "      <IsRead>false</IsRead>\n"
+ "    </Issues>\n"
+ "    <Issues diffgr:id=\"Issues9\" msdata:rowOrder=\"8\">\n"
+ "      <IssueID>20</IssueID>\n"
+ "      <StafferID>2</StafferID>\n"
+ "      <IssueTypeID>10</IssueTypeID>\n"
+ "      <Title>Laptop in cubicle 119 can't access the WiFi network</Title>\n"
+ "      <Description>The laptop is 802.11a enabled.</Description>\n"
+ "      <DateOpened>2004-01-11T12:41:56.0000000+06:00</DateOpened>\n"
+ "      <IsOpen>true</IsOpen>\n"
+ "      <DateModified>2008-03-25T15:08:47.2530000+06:00</DateModified>\n"
+ "      <UserName>kabercrombie</UserName>\n"
+ "      <DisplayName>Kim Abercrombie</DisplayName>\n"
+ "      <IssueType>Network</IssueType>\n"
+ "      <HasConflicts>false</HasConflicts>\n"
+ "      <IsRead>false</IsRead>\n"
+ "    </Issues>\n"
+ "    <Issues diffgr:id=\"Issues10\" msdata:rowOrder=\"9\">\n"
+ "      <IssueID>21</IssueID>\n"
+ "      <StafferID>3</StafferID>\n"
+ "      <IssueTypeID>3</IssueTypeID>\n"
+ "      <Title>Docking station in cubicle 37 doesn't work</Title>\n"
+ "      <Description/>\n"
+ "      <DateOpened>2004-01-11T12:41:56.0000000+06:00</DateOpened>\n"
+ "      <IsOpen>true</IsOpen>\n"
+ "      <DateModified>2008-03-25T15:08:47.2530000+06:00</DateModified>\n"
+ "      <UserName>ryoung</UserName>\n"
+ "      <DisplayName>Rob Young</DisplayName>\n"
+ "      <IssueType>Computer</IssueType>\n"
+ "      <HasConflicts>false</HasConflicts>\n"
+ "      <IsRead>false</IsRead>\n"
+ "    </Issues>\n"
+ "    <Issues diffgr:id=\"Issues11\" msdata:rowOrder=\"10\">\n"
+ "      <IssueID>22</IssueID>\n"
+ "      <StafferID>4</StafferID>\n"
+ "      <IssueTypeID>1</IssueTypeID>\n"
+ "      <Title>Phone at cubicle 217 has an echo when dialing out</Title>\n"
+ "      <Description/>\n"
+ "      <DateOpened>2004-01-11T12:41:56.0000000+06:00</DateOpened>\n"
+ "      <IsOpen>true</IsOpen>\n"
+ "      <DateModified>2008-03-25T15:08:47.2530000+06:00</DateModified>\n"
+ "      <UserName>tyoutsey</UserName>\n"
+ "      <DisplayName>Tom Youtsey</DisplayName>\n"
+ "      <IssueType>Telecommunications</IssueType>\n"
+ "      <HasConflicts>false</HasConflicts>\n"
+ "      <IsRead>false</IsRead>\n"
+ "    </Issues>\n"
+ "    <Issues diffgr:id=\"Issues12\" msdata:rowOrder=\"11\">\n"
+ "      <IssueID>23</IssueID>\n"
+ "      <StafferID>5</StafferID>\n"
+ "      <IssueTypeID>5</IssueTypeID>\n"
+ "      <Title>Printer Zeus has run out of legal size paper</Title>\n"
+ "      <Description/>\n"
+ "      <DateOpened>2004-01-11T12:41:56.0000000+06:00</DateOpened>\n"
+ "      <IsOpen>true</IsOpen>\n"
+ "      <DateModified>2008-03-25T15:08:47.2530000+06:00</DateModified>\n"
+ "      <UserName>gyukish</UserName>\n"
+ "      <DisplayName>Gary W. Yukish</DisplayName>\n"
+ "      <IssueType>Printer</IssueType>\n"
+ "      <HasConflicts>false</HasConflicts>\n"
+ "      <IsRead>false</IsRead>\n"
+ "    </Issues>\n"
+ "    <Issues diffgr:id=\"Issues13\" msdata:rowOrder=\"12\">\n"
+ "      <IssueID>24</IssueID>\n"
+ "      <StafferID>6</StafferID>\n"
+ "      <IssueTypeID>10</IssueTypeID>\n"
+ "      <Title>Network load balancer is not seeing server #9</Title>\n"
+ "      <Description>Server #9 was re-imaged last week.</Description>\n"
+ "      <DateOpened>2004-01-11T12:41:56.0000000+06:00</DateOpened>\n"
+ "      <IsOpen>true</IsOpen>\n"
+ "      <DateModified>2008-03-25T15:08:47.2530000+06:00</DateModified>\n"
+ "      <UserName>rcaron</UserName>\n"
+ "      <DisplayName>Rob Caron</DisplayName>\n"
+ "      <IssueType>Network</IssueType>\n"
+ "      <HasConflicts>false</HasConflicts>\n"
+ "      <IsRead>false</IsRead>\n"
+ "    </Issues>\n"
+ "    <Issues diffgr:id=\"Issues14\" msdata:rowOrder=\"13\">\n"
+ "      <IssueID>25</IssueID>\n"
+ "      <StafferID>7</StafferID>\n"
+ "      <IssueTypeID>1</IssueTypeID>\n"
+ "      <Title>People on the other end of the line can't hear when the speaker phone is turned on</Title>\n"
+ "      <Description>It works fine using the handset or a headset.</Description>\n"
+ "      <DateOpened>2004-01-11T12:41:56.0000000+06:00</DateOpened>\n"
+ "      <IsOpen>true</IsOpen>\n"
+ "      <DateModified>2008-03-25T15:08:47.2530000+06:00</DateModified>\n"
+ "      <UserName>kzimprich</UserName>\n"
+ "      <DisplayName>Karin Zimprich</DisplayName>\n"
+ "      <IssueType>Telecommunications</IssueType>\n"
+ "      <HasConflicts>false</HasConflicts>\n"
+ "      <IsRead>false</IsRead>\n"
+ "    </Issues>\n"
+ "    <Issues diffgr:id=\"Issues15\" msdata:rowOrder=\"14\">\n"
+ "      <IssueID>26</IssueID>\n"
+ "      <StafferID>8</StafferID>\n"
+ "      <IssueTypeID>3</IssueTypeID>\n"
+ "      <Title>Graphics station in cubicle 79 needs an upgrade to a 20 inch monitor</Title>\n"
+ "      <Description>This has already been approved.</Description>\n"
+ "      <DateOpened>2004-01-11T12:41:56.0000000+06:00</DateOpened>\n"
+ "      <IsOpen>true</IsOpen>\n"
+ "      <DateModified>2008-03-25T15:08:47.2530000+06:00</DateModified>\n"
+ "      <UserName>rboseman</UserName>\n"
+ "      <DisplayName>Randall Boseman</DisplayName>\n"
+ "      <IssueType>Computer</IssueType>\n"
+ "      <HasConflicts>false</HasConflicts>\n"
+ "      <IsRead>false</IsRead>\n"
+ "    </Issues>\n"
+ "    <Issues diffgr:id=\"Issues16\" msdata:rowOrder=\"15\">\n"
+ "      <IssueID>27</IssueID>\n"
+ "      <StafferID>9</StafferID>\n"
+ "      <IssueTypeID>10</IssueTypeID>\n"
+ "      <Title>Firewall is blocking instant messenger</Title>\n"
+ "      <Description>Neither Yahoo nor AOL work. MSN does however.</Description>\n"
+ "      <DateOpened>2004-01-11T12:41:56.0000000+06:00</DateOpened>\n"
+ "      <IsOpen>true</IsOpen>\n"
+ "      <DateModified>2008-03-25T15:08:47.2530000+06:00</DateModified>\n"
+ "      <UserName>kkennedy</UserName>\n"
+ "      <DisplayName>Kevin Kennedy</DisplayName>\n"
+ "      <IssueType>Network</IssueType>\n"
+ "      <HasConflicts>false</HasConflicts>\n"
+ "      <IsRead>false</IsRead>\n"
+ "    </Issues>\n"
+ "    <Issues diffgr:id=\"Issues17\" msdata:rowOrder=\"16\">\n"
+ "      <IssueID>28</IssueID>\n"
+ "      <StafferID>10</StafferID>\n"
+ "      <IssueTypeID>5</IssueTypeID>\n"
+ "      <Title>Printer Apollo is low on toner</Title>\n"
+ "      <Description/>\n"
+ "      <DateOpened>2004-01-11T12:41:56.0000000+06:00</DateOpened>\n"
+ "      <IsOpen>true</IsOpen>\n"
+ "      <DateModified>2008-03-25T15:08:47.2530000+06:00</DateModified>\n"
+ "      <UserName>dtibbott</UserName>\n"
+ "      <DisplayName>Diane Tibbott</DisplayName>\n"
+ "      <IssueType>Printer</IssueType>\n"
+ "      <HasConflicts>false</HasConflicts>\n"
+ "      <IsRead>false</IsRead>\n"
+ "    </Issues>\n"
+ "    <Issues diffgr:id=\"Issues18\" msdata:rowOrder=\"17\">\n"
+ "      <IssueID>29</IssueID>\n"
+ "      <StafferID>11</StafferID>\n"
+ "      <IssueTypeID>3</IssueTypeID>\n"
+ "      <Title>Cubicle 597 needs a developer workstation</Title>\n"
+ "      <Description/>\n"
+ "      <DateOpened>2004-01-11T12:41:56.0000000+06:00</DateOpened>\n"
+ "      <IsOpen>true</IsOpen>\n"
+ "      <DateModified>2008-03-25T15:08:47.2530000+06:00</DateModified>\n"
+ "      <UserName>gyoung</UserName>\n"
+ "      <DisplayName>Garrett Young</DisplayName>\n"
+ "      <IssueType>Computer</IssueType>\n"
+ "      <HasConflicts>false</HasConflicts>\n"
+ "      <IsRead>false</IsRead>\n"
+ "    </Issues>\n"
+ "    <Issues diffgr:id=\"Issues19\" msdata:rowOrder=\"18\">\n"
+ "      <IssueID>30</IssueID>\n"
+ "      <StafferID>12</StafferID>\n"
+ "      <IssueTypeID>5</IssueTypeID>\n"
+ "      <Title>The printer in cubicle 18 needs to be moved to cubicle 67A</Title>\n"
+ "      <Description/>\n"
+ "      <DateOpened>2004-01-11T12:41:56.0000000+06:00</DateOpened>\n"
+ "      <IsOpen>true</IsOpen>\n"
+ "      <DateModified>2008-03-25T15:08:47.2530000+06:00</DateModified>\n"
+ "      <UserName>demo</UserName>\n"
+ "      <DisplayName>Demo</DisplayName>\n"
+ "      <IssueType>Printer</IssueType>\n"
+ "      <HasConflicts>false</HasConflicts>\n"
+ "      <IsRead>false</IsRead>\n"
+ "    </Issues>\n"
+ "    <Issues diffgr:id=\"Issues20\" msdata:rowOrder=\"19\">\n"
+ "      <IssueID>31</IssueID>\n"
+ "      <StafferID>1</StafferID>\n"
+ "      <IssueTypeID>10</IssueTypeID>\n"
+ "      <Title>The fucking system does not work</Title>\n"
+ "      <Description/>\n"
+ "      <DateOpened>2008-03-25T15:10:28.8930000+06:00</DateOpened>\n"
+ "      <IsOpen>true</IsOpen>\n"
+ "      <DateModified>2008-03-25T15:10:28.9100000+06:00</DateModified>\n"
+ "      <UserName>abarr</UserName>\n"
+ "      <DisplayName>Adam Barr</DisplayName>\n"
+ "      <IssueType>Network</IssueType>\n"
+ "      <HasConflicts>false</HasConflicts>\n"
+ "      <IsRead>false</IsRead>\n"
+ "    </Issues>\n"
+ "  </IssueDetails>\n"
+ "</diffgr:diffgram>\n"
;

    static void Main()
    {
        try
        {
            IVDataSet data = new IVDataSet();
            data.ReadXml(new StringReader(diffgram), XmlReadMode.DiffGram);
            Print(data);
        }
        catch (XmlException exc)
        {
            Console.WriteLine("Location: {0}:{1}", exc.LineNumber, exc.LinePosition);
        }
        Console.WriteLine("<%END%>");
    }

    static void Print(DataSet ds)
    {
        Console.WriteLine(ds.DataSetName);
        foreach (DataTable table in ds.Tables)
            PrintTable(table);
    }

    static void PrintTable(DataTable table)
    {
        Console.WriteLine("--- Table {0}", table.TableName);
        string str = "";
        int n = table.Columns.Count;
        for (int i = 0; i < n; ++i)
        {
            if (i > 0) str += " | ";
            str += table.Columns[i].ColumnName;
        }
        Console.WriteLine(str);
        foreach (DataRow row in table.Rows)
        {
            str = "";
            for (int i = 0; i < n; ++i)
            {
                if (i > 0) str += " | ";
                object v = row[table.Columns[i]];
                if (v is DateTime)
                {
                    string vs = ((DateTime)v).ToString("dd-MM-yy");
                    str += vs;
                }
                else
                {
                    str += v;
                }
            }
            Console.WriteLine(str);
        }
    }
}