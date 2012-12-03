using System;
using System.Data;
using System.Runtime.Serialization;
using System.Xml;

namespace IssueVision
{
    [Serializable()]
#if NOT_PFX
[System.ComponentModel.DesignerCategoryAttribute("code")]
#endif
    [System.Diagnostics.DebuggerStepThrough()]
#if NOT_PFX
[System.ComponentModel.ToolboxItem(true)]
#endif
    public class IVDataSet : DataSet {
        
        private IssueHistoryDataTable tableIssueHistory;
        
        private IssuesDataTable tableIssues;
        
        private ConflictsDataTable tableConflicts;
        
        private IssueTypesDataTable tableIssueTypes;
        
        private StaffersDataTable tableStaffers;
        
        public IVDataSet() {
            this.InitClass();
#if NOT_PFX
System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
            this.Tables.CollectionChanged += schemaChangedHandler;
            this.Relations.CollectionChanged += schemaChangedHandler;
#endif
        }
        
        protected IVDataSet(SerializationInfo info, StreamingContext context) {
            string strSchema = ((string)(info.GetValue("XmlSchema", typeof(string))));
            if ((strSchema != null)) {
                DataSet ds = new DataSet();
                ds.ReadXmlSchema(new XmlTextReader(new System.IO.StringReader(strSchema)));
                if ((ds.Tables["IssueHistory"] != null)) {
                    this.Tables.Add(new IssueHistoryDataTable(ds.Tables["IssueHistory"]));
                }
                if ((ds.Tables["Issues"] != null)) {
                    this.Tables.Add(new IssuesDataTable(ds.Tables["Issues"]));
                }
                if ((ds.Tables["Conflicts"] != null)) {
                    this.Tables.Add(new ConflictsDataTable(ds.Tables["Conflicts"]));
                }
                if ((ds.Tables["IssueTypes"] != null)) {
                    this.Tables.Add(new IssueTypesDataTable(ds.Tables["IssueTypes"]));
                }
                if ((ds.Tables["Staffers"] != null)) {
                    this.Tables.Add(new StaffersDataTable(ds.Tables["Staffers"]));
                }
                this.DataSetName = ds.DataSetName;
                this.Prefix = ds.Prefix;
                this.Namespace = ds.Namespace;
                this.Locale = ds.Locale;
                this.CaseSensitive = ds.CaseSensitive;
                this.EnforceConstraints = ds.EnforceConstraints;
#if NOT_PFX
this.Merge(ds, false, System.Data.MissingSchemaAction.Add);
#endif
                this.InitVars();
            }
            else {
                this.InitClass();
            }
            this.GetSerializationData(info, context);
#if NOT_PFX
System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
            this.Tables.CollectionChanged += schemaChangedHandler;
            this.Relations.CollectionChanged += schemaChangedHandler;
#endif
        }

#if NOT_PFX
[System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Content)]
#endif
        public IssueHistoryDataTable IssueHistory {
            get {
                return this.tableIssueHistory;
            }
        }

#if NOT_PFX
[System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Content)]
#endif
        public IssuesDataTable Issues {
            get {
                return this.tableIssues;
            }
        }

#if NOT_PFX
[System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Content)]
#endif
        public ConflictsDataTable Conflicts {
            get {
                return this.tableConflicts;
            }
        }

#if NOT_PFX
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Content)]
#endif
        public IssueTypesDataTable IssueTypes {
            get {
                return this.tableIssueTypes;
            }
        }

#if NOT_PFX
[System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Content)]
#endif
        public StaffersDataTable Staffers {
            get {
                return this.tableStaffers;
            }
        }
        
        public override DataSet Clone() {
            IVDataSet cln = ((IVDataSet)(base.Clone()));
            cln.InitVars();
            return cln;
        }
        
        protected override bool ShouldSerializeTables() {
            return false;
        }
        
        protected override bool ShouldSerializeRelations() {
            return false;
        }
        
        protected override void ReadXmlSerializable(XmlReader reader) {
            this.Reset();
            DataSet ds = new DataSet();
            ds.ReadXml(reader);
            if ((ds.Tables["IssueHistory"] != null)) {
                this.Tables.Add(new IssueHistoryDataTable(ds.Tables["IssueHistory"]));
            }
            if ((ds.Tables["Issues"] != null)) {
                this.Tables.Add(new IssuesDataTable(ds.Tables["Issues"]));
            }
            if ((ds.Tables["Conflicts"] != null)) {
                this.Tables.Add(new ConflictsDataTable(ds.Tables["Conflicts"]));
            }
            if ((ds.Tables["IssueTypes"] != null)) {
                this.Tables.Add(new IssueTypesDataTable(ds.Tables["IssueTypes"]));
            }
            if ((ds.Tables["Staffers"] != null)) {
                this.Tables.Add(new StaffersDataTable(ds.Tables["Staffers"]));
            }
            this.DataSetName = ds.DataSetName;
            this.Prefix = ds.Prefix;
            this.Namespace = ds.Namespace;
            this.Locale = ds.Locale;
            this.CaseSensitive = ds.CaseSensitive;
            this.EnforceConstraints = ds.EnforceConstraints;
#if NOT_PFX
   this.Merge(ds, false, System.Data.MissingSchemaAction.Add);
#endif
            this.InitVars();
        }
      
        // TODO: We don't provide XmlTextWriter as we trimmed all libs to Silverlight
#if NOT_PFX
        protected override System.Xml.Schema.XmlSchema GetSchemaSerializable() {
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            this.WriteXmlSchema(new XmlTextWriter(stream, null));
            stream.Position = 0;
            return System.Xml.Schema.XmlSchema.Read(new XmlTextReader(stream), null);
        }
#endif
        
        internal void InitVars() {
            this.tableIssueHistory = ((IssueHistoryDataTable)(this.Tables["IssueHistory"]));
            if ((this.tableIssueHistory != null)) {
                this.tableIssueHistory.InitVars();
            }
            this.tableIssues = ((IssuesDataTable)(this.Tables["Issues"]));
            if ((this.tableIssues != null)) {
                this.tableIssues.InitVars();
            }
            this.tableConflicts = ((ConflictsDataTable)(this.Tables["Conflicts"]));
            if ((this.tableConflicts != null)) {
                this.tableConflicts.InitVars();
            }
            this.tableIssueTypes = ((IssueTypesDataTable)(this.Tables["IssueTypes"]));
            if ((this.tableIssueTypes != null)) {
                this.tableIssueTypes.InitVars();
            }
            this.tableStaffers = ((StaffersDataTable)(this.Tables["Staffers"]));
            if ((this.tableStaffers != null)) {
                this.tableStaffers.InitVars();
            }
        }
        
        private void InitClass() {
            this.DataSetName = "IVDataSet";
            this.Prefix = "";
            this.Namespace = "http://www.tempuri.org/IVDataSet.xsd";
            this.Locale = new System.Globalization.CultureInfo("en-US");
            this.CaseSensitive = false;
            this.EnforceConstraints = true;
            this.tableIssueHistory = new IssueHistoryDataTable();
            this.Tables.Add(this.tableIssueHistory);
            this.tableIssues = new IssuesDataTable();
            this.Tables.Add(this.tableIssues);
            this.tableConflicts = new ConflictsDataTable();
            this.Tables.Add(this.tableConflicts);
            this.tableIssueTypes = new IssueTypesDataTable();
            this.Tables.Add(this.tableIssueTypes);
            this.tableStaffers = new StaffersDataTable();
            this.Tables.Add(this.tableStaffers);
        }
        
        private bool ShouldSerializeIssueHistory() {
            return false;
        }
        
        private bool ShouldSerializeIssues() {
            return false;
        }
        
        private bool ShouldSerializeConflicts() {
            return false;
        }
        
        private bool ShouldSerializeIssueTypes() {
            return false;
        }
        
        private bool ShouldSerializeStaffers() {
            return false;
        }

#if NOT_PFX
        private void SchemaChanged(object sender, System.ComponentModel.CollectionChangeEventArgs e) {
            if ((e.Action == System.ComponentModel.CollectionChangeAction.Remove)) {
                this.InitVars();
            }
        }
#endif
        
        public delegate void IssueHistoryRowChangeEventHandler(object sender, IssueHistoryRowChangeEvent e);
        
        public delegate void IssuesRowChangeEventHandler(object sender, IssuesRowChangeEvent e);
        
        public delegate void ConflictsRowChangeEventHandler(object sender, ConflictsRowChangeEvent e);
        
        public delegate void IssueTypesRowChangeEventHandler(object sender, IssueTypesRowChangeEvent e);
        
        public delegate void StaffersRowChangeEventHandler(object sender, StaffersRowChangeEvent e);
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class IssueHistoryDataTable : DataTable, System.Collections.IEnumerable {
            
            private DataColumn columnIssueHistoryID;
            
            private DataColumn columnStafferID;
            
            private DataColumn columnIssueID;
            
            private DataColumn columnComment;
            
            private DataColumn columnDateCreated;
            
            private DataColumn columnDisplayName;
            
            internal IssueHistoryDataTable() : 
                base("IssueHistory") {
                this.InitClass();
                }
            
            internal IssueHistoryDataTable(DataTable table) : 
                base(table.TableName) {
                if ((table.CaseSensitive != table.DataSet.CaseSensitive)) {
                    this.CaseSensitive = table.CaseSensitive;
                }
                if ((table.Locale.ToString() != table.DataSet.Locale.ToString())) {
                    this.Locale = table.Locale;
                }
                if ((table.Namespace != table.DataSet.Namespace)) {
                    this.Namespace = table.Namespace;
                }
                this.Prefix = table.Prefix;
                this.MinimumCapacity = table.MinimumCapacity;
                this.DisplayExpression = table.DisplayExpression;
                }

#if NOT_PFX
[System.ComponentModel.Browsable(false)]
#endif
            public int Count {
                get {
                    return this.Rows.Count;
                }
            }
            
            internal DataColumn IssueHistoryIDColumn {
                get {
                    return this.columnIssueHistoryID;
                }
            }
            
            internal DataColumn StafferIDColumn {
                get {
                    return this.columnStafferID;
                }
            }
            
            internal DataColumn IssueIDColumn {
                get {
                    return this.columnIssueID;
                }
            }
            
            internal DataColumn CommentColumn {
                get {
                    return this.columnComment;
                }
            }
            
            internal DataColumn DateCreatedColumn {
                get {
                    return this.columnDateCreated;
                }
            }
            
            internal DataColumn DisplayNameColumn {
                get {
                    return this.columnDisplayName;
                }
            }
            
            public IssueHistoryRow this[int index] {
                get {
                    return ((IssueHistoryRow)(this.Rows[index]));
                }
            }
            
            public event IssueHistoryRowChangeEventHandler IssueHistoryRowChanged;
            
            public event IssueHistoryRowChangeEventHandler IssueHistoryRowChanging;
            
            public event IssueHistoryRowChangeEventHandler IssueHistoryRowDeleted;
            
            public event IssueHistoryRowChangeEventHandler IssueHistoryRowDeleting;
            
            public void AddIssueHistoryRow(IssueHistoryRow row) {
                this.Rows.Add(row);
            }
            
            public IssueHistoryRow AddIssueHistoryRow(int StafferID, int IssueID, string Comment, System.DateTime DateCreated, string DisplayName) {
                IssueHistoryRow rowIssueHistoryRow = ((IssueHistoryRow)(this.NewRow()));
                rowIssueHistoryRow.ItemArray = new object[] {
                                                                null,
                                                                StafferID,
                                                                IssueID,
                                                                Comment,
                                                                DateCreated,
                                                                DisplayName};
                this.Rows.Add(rowIssueHistoryRow);
                return rowIssueHistoryRow;
            }
            
            public System.Collections.IEnumerator GetEnumerator() {
                return this.Rows.GetEnumerator();
            }
            
            public override DataTable Clone() {
                IssueHistoryDataTable cln = ((IssueHistoryDataTable)(base.Clone()));
                cln.InitVars();
                return cln;
            }
            
            protected override DataTable CreateInstance() {
                return new IssueHistoryDataTable();
            }
            
            internal void InitVars() {
                this.columnIssueHistoryID = this.Columns["IssueHistoryID"];
                this.columnStafferID = this.Columns["StafferID"];
                this.columnIssueID = this.Columns["IssueID"];
                this.columnComment = this.Columns["Comment"];
                this.columnDateCreated = this.Columns["DateCreated"];
                this.columnDisplayName = this.Columns["DisplayName"];
            }
            
            private void InitClass() {
                this.columnIssueHistoryID = new DataColumn("IssueHistoryID", typeof(int), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnIssueHistoryID);
                this.columnStafferID = new DataColumn("StafferID", typeof(int), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnStafferID);
                this.columnIssueID = new DataColumn("IssueID", typeof(int), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnIssueID);
                this.columnComment = new DataColumn("Comment", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnComment);
                this.columnDateCreated = new DataColumn("DateCreated", typeof(System.DateTime), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnDateCreated);
                this.columnDisplayName = new DataColumn("DisplayName", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnDisplayName);
                this.columnIssueHistoryID.AutoIncrement = true;
                this.columnIssueHistoryID.AllowDBNull = false;
                this.columnIssueHistoryID.ReadOnly = true;
                this.columnStafferID.AllowDBNull = false;
                this.columnIssueID.AllowDBNull = false;
                this.columnComment.AllowDBNull = false;
                this.columnDateCreated.AllowDBNull = false;
                this.columnDisplayName.AllowDBNull = false;
            }
            
            public IssueHistoryRow NewIssueHistoryRow() {
                return ((IssueHistoryRow)(this.NewRow()));
            }
            
            protected override DataRow NewRowFromBuilder(DataRowBuilder builder) {
                return new IssueHistoryRow(builder);
            }
            
            protected override System.Type GetRowType() {
                return typeof(IssueHistoryRow);
            }
            
            protected override void OnRowChanged(DataRowChangeEventArgs e) {
                base.OnRowChanged(e);
                if ((this.IssueHistoryRowChanged != null)) {
                    this.IssueHistoryRowChanged(this, new IssueHistoryRowChangeEvent(((IssueHistoryRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowChanging(DataRowChangeEventArgs e) {
                base.OnRowChanging(e);
                if ((this.IssueHistoryRowChanging != null)) {
                    this.IssueHistoryRowChanging(this, new IssueHistoryRowChangeEvent(((IssueHistoryRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleted(DataRowChangeEventArgs e) {
                base.OnRowDeleted(e);
                if ((this.IssueHistoryRowDeleted != null)) {
                    this.IssueHistoryRowDeleted(this, new IssueHistoryRowChangeEvent(((IssueHistoryRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleting(DataRowChangeEventArgs e) {
                base.OnRowDeleting(e);
                if ((this.IssueHistoryRowDeleting != null)) {
                    this.IssueHistoryRowDeleting(this, new IssueHistoryRowChangeEvent(((IssueHistoryRow)(e.Row)), e.Action));
                }
            }
            
            public void RemoveIssueHistoryRow(IssueHistoryRow row) {
                this.Rows.Remove(row);
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class IssueHistoryRow : DataRow {
            
            private IssueHistoryDataTable tableIssueHistory;
            
            internal IssueHistoryRow(DataRowBuilder rb) : 
                base(rb) {
                this.tableIssueHistory = ((IssueHistoryDataTable)(this.Table));
                }
            
            public int IssueHistoryID {
                get {
                    return ((int)(this[this.tableIssueHistory.IssueHistoryIDColumn]));
                }
                set {
                    this[this.tableIssueHistory.IssueHistoryIDColumn] = value;
                }
            }
            
            public int StafferID {
                get {
                    return ((int)(this[this.tableIssueHistory.StafferIDColumn]));
                }
                set {
                    this[this.tableIssueHistory.StafferIDColumn] = value;
                }
            }
            
            public int IssueID {
                get {
                    return ((int)(this[this.tableIssueHistory.IssueIDColumn]));
                }
                set {
                    this[this.tableIssueHistory.IssueIDColumn] = value;
                }
            }
            
            public string Comment {
                get {
                    return ((string)(this[this.tableIssueHistory.CommentColumn]));
                }
                set {
                    this[this.tableIssueHistory.CommentColumn] = value;
                }
            }
            
            public System.DateTime DateCreated {
                get {
                    return ((System.DateTime)(this[this.tableIssueHistory.DateCreatedColumn]));
                }
                set {
                    this[this.tableIssueHistory.DateCreatedColumn] = value;
                }
            }
            
            public string DisplayName {
                get {
                    return ((string)(this[this.tableIssueHistory.DisplayNameColumn]));
                }
                set {
                    this[this.tableIssueHistory.DisplayNameColumn] = value;
                }
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class IssueHistoryRowChangeEvent : EventArgs {
            
            private IssueHistoryRow eventRow;
            
            private DataRowAction eventAction;
            
            public IssueHistoryRowChangeEvent(IssueHistoryRow row, DataRowAction action) {
                this.eventRow = row;
                this.eventAction = action;
            }
            
            public IssueHistoryRow Row {
                get {
                    return this.eventRow;
                }
            }
            
            public DataRowAction Action {
                get {
                    return this.eventAction;
                }
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class IssuesDataTable : DataTable, System.Collections.IEnumerable {
            
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
            
            internal IssuesDataTable() : 
                base("Issues") {
                this.InitClass();
                }
            
            internal IssuesDataTable(DataTable table) : 
                base(table.TableName) {
                if ((table.CaseSensitive != table.DataSet.CaseSensitive)) {
                    this.CaseSensitive = table.CaseSensitive;
                }
                if ((table.Locale.ToString() != table.DataSet.Locale.ToString())) {
                    this.Locale = table.Locale;
                }
                if ((table.Namespace != table.DataSet.Namespace)) {
                    this.Namespace = table.Namespace;
                }
                this.Prefix = table.Prefix;
                this.MinimumCapacity = table.MinimumCapacity;
                this.DisplayExpression = table.DisplayExpression;
                }

#if NOT_PFX
[System.ComponentModel.Browsable(false)]
#endif
            public int Count {
                get {
                    return this.Rows.Count;
                }
            }
            
            internal DataColumn IssueIDColumn {
                get {
                    return this.columnIssueID;
                }
            }
            
            internal DataColumn StafferIDColumn {
                get {
                    return this.columnStafferID;
                }
            }
            
            internal DataColumn IssueTypeIDColumn {
                get {
                    return this.columnIssueTypeID;
                }
            }
            
            internal DataColumn TitleColumn {
                get {
                    return this.columnTitle;
                }
            }
            
            internal DataColumn DescriptionColumn {
                get {
                    return this.columnDescription;
                }
            }
            
            internal DataColumn DateOpenedColumn {
                get {
                    return this.columnDateOpened;
                }
            }
            
            internal DataColumn DateClosedColumn {
                get {
                    return this.columnDateClosed;
                }
            }
            
            internal DataColumn IsOpenColumn {
                get {
                    return this.columnIsOpen;
                }
            }
            
            internal DataColumn DateModifiedColumn {
                get {
                    return this.columnDateModified;
                }
            }
            
            internal DataColumn UserNameColumn {
                get {
                    return this.columnUserName;
                }
            }
            
            internal DataColumn DisplayNameColumn {
                get {
                    return this.columnDisplayName;
                }
            }
            
            internal DataColumn IssueTypeColumn {
                get {
                    return this.columnIssueType;
                }
            }
            
            internal DataColumn HasConflictsColumn {
                get {
                    return this.columnHasConflicts;
                }
            }
            
            internal DataColumn IsReadColumn {
                get {
                    return this.columnIsRead;
                }
            }
            
            public IssuesRow this[int index] {
                get {
                    return ((IssuesRow)(this.Rows[index]));
                }
            }
            
            public event IssuesRowChangeEventHandler IssuesRowChanged;
            
            public event IssuesRowChangeEventHandler IssuesRowChanging;
            
            public event IssuesRowChangeEventHandler IssuesRowDeleted;
            
            public event IssuesRowChangeEventHandler IssuesRowDeleting;
            
            public void AddIssuesRow(IssuesRow row) {
                this.Rows.Add(row);
            }
            
            public IssuesRow AddIssuesRow(int StafferID, int IssueTypeID, string Title, string Description, System.DateTime DateOpened, System.DateTime DateClosed, bool IsOpen, System.DateTime DateModified, string UserName, string DisplayName, string IssueType, bool HasConflicts, bool IsRead) {
                IssuesRow rowIssuesRow = ((IssuesRow)(this.NewRow()));
                rowIssuesRow.ItemArray = new object[] {
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
                                                          IsRead};
                this.Rows.Add(rowIssuesRow);
                return rowIssuesRow;
            }
            
            public System.Collections.IEnumerator GetEnumerator() {
                return this.Rows.GetEnumerator();
            }
            
            public override DataTable Clone() {
                IssuesDataTable cln = ((IssuesDataTable)(base.Clone()));
                cln.InitVars();
                return cln;
            }
            
            protected override DataTable CreateInstance() {
                return new IssuesDataTable();
            }
            
            internal void InitVars() {
                this.columnIssueID = this.Columns["IssueID"];
                this.columnStafferID = this.Columns["StafferID"];
                this.columnIssueTypeID = this.Columns["IssueTypeID"];
                this.columnTitle = this.Columns["Title"];
                this.columnDescription = this.Columns["Description"];
                this.columnDateOpened = this.Columns["DateOpened"];
                this.columnDateClosed = this.Columns["DateClosed"];
                this.columnIsOpen = this.Columns["IsOpen"];
                this.columnDateModified = this.Columns["DateModified"];
                this.columnUserName = this.Columns["UserName"];
                this.columnDisplayName = this.Columns["DisplayName"];
                this.columnIssueType = this.Columns["IssueType"];
                this.columnHasConflicts = this.Columns["HasConflicts"];
                this.columnIsRead = this.Columns["IsRead"];
            }
            
            private void InitClass() {
                this.columnIssueID = new DataColumn("IssueID", typeof(int), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnIssueID);
                this.columnStafferID = new DataColumn("StafferID", typeof(int), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnStafferID);
                this.columnIssueTypeID = new DataColumn("IssueTypeID", typeof(int), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnIssueTypeID);
                this.columnTitle = new DataColumn("Title", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnTitle);
                this.columnDescription = new DataColumn("Description", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnDescription);
                this.columnDateOpened = new DataColumn("DateOpened", typeof(System.DateTime), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnDateOpened);
                this.columnDateClosed = new DataColumn("DateClosed", typeof(System.DateTime), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnDateClosed);
                this.columnIsOpen = new DataColumn("IsOpen", typeof(bool), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnIsOpen);
                this.columnDateModified = new DataColumn("DateModified", typeof(System.DateTime), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnDateModified);
                this.columnUserName = new DataColumn("UserName", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnUserName);
                this.columnDisplayName = new DataColumn("DisplayName", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnDisplayName);
                this.columnIssueType = new DataColumn("IssueType", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnIssueType);
                this.columnHasConflicts = new DataColumn("HasConflicts", typeof(bool), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnHasConflicts);
                this.columnIsRead = new DataColumn("IsRead", typeof(bool), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnIsRead);
                this.columnIssueID.AutoIncrement = true;
                this.columnIssueID.AllowDBNull = false;
                this.columnIssueID.ReadOnly = true;
                this.columnStafferID.AllowDBNull = false;
                this.columnIssueTypeID.AllowDBNull = false;
                this.columnTitle.AllowDBNull = false;
                this.columnDescription.AllowDBNull = false;
                this.columnDateOpened.AllowDBNull = false;
                this.columnIsOpen.AllowDBNull = false;
                this.columnDateModified.AllowDBNull = false;
                this.columnUserName.AllowDBNull = false;
                this.columnDisplayName.AllowDBNull = false;
                this.columnIssueType.AllowDBNull = false;
                this.columnHasConflicts.DefaultValue = false;
                this.columnIsRead.DefaultValue = false;
            }
            
            public IssuesRow NewIssuesRow() {
                return ((IssuesRow)(this.NewRow()));
            }
            
            protected override DataRow NewRowFromBuilder(DataRowBuilder builder) {
                return new IssuesRow(builder);
            }
            
            protected override System.Type GetRowType() {
                return typeof(IssuesRow);
            }
            
            protected override void OnRowChanged(DataRowChangeEventArgs e) {
                base.OnRowChanged(e);
                if ((this.IssuesRowChanged != null)) {
                    this.IssuesRowChanged(this, new IssuesRowChangeEvent(((IssuesRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowChanging(DataRowChangeEventArgs e) {
                base.OnRowChanging(e);
                if ((this.IssuesRowChanging != null)) {
                    this.IssuesRowChanging(this, new IssuesRowChangeEvent(((IssuesRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleted(DataRowChangeEventArgs e) {
                base.OnRowDeleted(e);
                if ((this.IssuesRowDeleted != null)) {
                    this.IssuesRowDeleted(this, new IssuesRowChangeEvent(((IssuesRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleting(DataRowChangeEventArgs e) {
                base.OnRowDeleting(e);
                if ((this.IssuesRowDeleting != null)) {
                    this.IssuesRowDeleting(this, new IssuesRowChangeEvent(((IssuesRow)(e.Row)), e.Action));
                }
            }
            
            public void RemoveIssuesRow(IssuesRow row) {
                this.Rows.Remove(row);
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class IssuesRow : DataRow {
            
            private IssuesDataTable tableIssues;
            
            internal IssuesRow(DataRowBuilder rb) : 
                base(rb) {
                this.tableIssues = ((IssuesDataTable)(this.Table));
                }
            
            public int IssueID {
                get {
                    return ((int)(this[this.tableIssues.IssueIDColumn]));
                }
                set {
                    this[this.tableIssues.IssueIDColumn] = value;
                }
            }
            
            public int StafferID {
                get {
                    return ((int)(this[this.tableIssues.StafferIDColumn]));
                }
                set {
                    this[this.tableIssues.StafferIDColumn] = value;
                }
            }
            
            public int IssueTypeID {
                get {
                    return ((int)(this[this.tableIssues.IssueTypeIDColumn]));
                }
                set {
                    this[this.tableIssues.IssueTypeIDColumn] = value;
                }
            }
            
            public string Title {
                get {
                    return ((string)(this[this.tableIssues.TitleColumn]));
                }
                set {
                    this[this.tableIssues.TitleColumn] = value;
                }
            }
            
            public string Description {
                get {
                    return ((string)(this[this.tableIssues.DescriptionColumn]));
                }
                set {
                    this[this.tableIssues.DescriptionColumn] = value;
                }
            }
            
            public System.DateTime DateOpened {
                get {
                    return ((System.DateTime)(this[this.tableIssues.DateOpenedColumn]));
                }
                set {
                    this[this.tableIssues.DateOpenedColumn] = value;
                }
            }
            
            public System.DateTime DateClosed {
                get {
                    try {
                        return ((System.DateTime)(this[this.tableIssues.DateClosedColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableIssues.DateClosedColumn] = value;
                }
            }
            
            public bool IsOpen {
                get {
                    return ((bool)(this[this.tableIssues.IsOpenColumn]));
                }
                set {
                    this[this.tableIssues.IsOpenColumn] = value;
                }
            }
            
            public System.DateTime DateModified {
                get {
                    return ((System.DateTime)(this[this.tableIssues.DateModifiedColumn]));
                }
                set {
                    this[this.tableIssues.DateModifiedColumn] = value;
                }
            }
            
            public string UserName {
                get {
                    return ((string)(this[this.tableIssues.UserNameColumn]));
                }
                set {
                    this[this.tableIssues.UserNameColumn] = value;
                }
            }
            
            public string DisplayName {
                get {
                    return ((string)(this[this.tableIssues.DisplayNameColumn]));
                }
                set {
                    this[this.tableIssues.DisplayNameColumn] = value;
                }
            }
            
            public string IssueType {
                get {
                    return ((string)(this[this.tableIssues.IssueTypeColumn]));
                }
                set {
                    this[this.tableIssues.IssueTypeColumn] = value;
                }
            }
            
            public bool HasConflicts {
                get {
                    try {
                        return ((bool)(this[this.tableIssues.HasConflictsColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableIssues.HasConflictsColumn] = value;
                }
            }
            
            public bool IsRead {
                get {
                    try {
                        return ((bool)(this[this.tableIssues.IsReadColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableIssues.IsReadColumn] = value;
                }
            }
            
            public bool IsDateClosedNull() {
                return this.IsNull(this.tableIssues.DateClosedColumn);
            }
            
            public void SetDateClosedNull() {
                this[this.tableIssues.DateClosedColumn] = System.Convert.DBNull;
            }
            
            public bool IsHasConflictsNull() {
                return this.IsNull(this.tableIssues.HasConflictsColumn);
            }
            
            public void SetHasConflictsNull() {
                this[this.tableIssues.HasConflictsColumn] = System.Convert.DBNull;
            }
            
            public bool IsIsReadNull() {
                return this.IsNull(this.tableIssues.IsReadColumn);
            }
            
            public void SetIsReadNull() {
                this[this.tableIssues.IsReadColumn] = System.Convert.DBNull;
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class IssuesRowChangeEvent : EventArgs {
            
            private IssuesRow eventRow;
            
            private DataRowAction eventAction;
            
            public IssuesRowChangeEvent(IssuesRow row, DataRowAction action) {
                this.eventRow = row;
                this.eventAction = action;
            }
            
            public IssuesRow Row {
                get {
                    return this.eventRow;
                }
            }
            
            public DataRowAction Action {
                get {
                    return this.eventAction;
                }
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class ConflictsDataTable : DataTable, System.Collections.IEnumerable {
            
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
            
            internal ConflictsDataTable() : 
                base("Conflicts") {
                this.InitClass();
                }
            
            internal ConflictsDataTable(DataTable table) : 
                base(table.TableName) {
                if ((table.CaseSensitive != table.DataSet.CaseSensitive)) {
                    this.CaseSensitive = table.CaseSensitive;
                }
                if ((table.Locale.ToString() != table.DataSet.Locale.ToString())) {
                    this.Locale = table.Locale;
                }
                if ((table.Namespace != table.DataSet.Namespace)) {
                    this.Namespace = table.Namespace;
                }
                this.Prefix = table.Prefix;
                this.MinimumCapacity = table.MinimumCapacity;
                this.DisplayExpression = table.DisplayExpression;
                }

#if NOT_PFX
[System.ComponentModel.Browsable(false)]
#endif
            public int Count {
                get {
                    return this.Rows.Count;
                }
            }
            
            internal DataColumn IssueIDColumn {
                get {
                    return this.columnIssueID;
                }
            }
            
            internal DataColumn StafferIDColumn {
                get {
                    return this.columnStafferID;
                }
            }
            
            internal DataColumn IssueTypeIDColumn {
                get {
                    return this.columnIssueTypeID;
                }
            }
            
            internal DataColumn TitleColumn {
                get {
                    return this.columnTitle;
                }
            }
            
            internal DataColumn DescriptionColumn {
                get {
                    return this.columnDescription;
                }
            }
            
            internal DataColumn DateOpenedColumn {
                get {
                    return this.columnDateOpened;
                }
            }
            
            internal DataColumn DateClosedColumn {
                get {
                    return this.columnDateClosed;
                }
            }
            
            internal DataColumn IsOpenColumn {
                get {
                    return this.columnIsOpen;
                }
            }
            
            internal DataColumn DateModifiedColumn {
                get {
                    return this.columnDateModified;
                }
            }
            
            internal DataColumn UserNameColumn {
                get {
                    return this.columnUserName;
                }
            }
            
            internal DataColumn DisplayNameColumn {
                get {
                    return this.columnDisplayName;
                }
            }
            
            internal DataColumn IssueTypeColumn {
                get {
                    return this.columnIssueType;
                }
            }
            
            internal DataColumn HasConflictsColumn {
                get {
                    return this.columnHasConflicts;
                }
            }
            
            internal DataColumn IsReadColumn {
                get {
                    return this.columnIsRead;
                }
            }
            
            public ConflictsRow this[int index] {
                get {
                    return ((ConflictsRow)(this.Rows[index]));
                }
            }
            
            public event ConflictsRowChangeEventHandler ConflictsRowChanged;
            
            public event ConflictsRowChangeEventHandler ConflictsRowChanging;
            
            public event ConflictsRowChangeEventHandler ConflictsRowDeleted;
            
            public event ConflictsRowChangeEventHandler ConflictsRowDeleting;
            
            public void AddConflictsRow(ConflictsRow row) {
                this.Rows.Add(row);
            }
            
            public ConflictsRow AddConflictsRow(int StafferID, int IssueTypeID, string Title, string Description, System.DateTime DateOpened, System.DateTime DateClosed, bool IsOpen, System.DateTime DateModified, string UserName, string DisplayName, string IssueType, bool HasConflicts, bool IsRead) {
                ConflictsRow rowConflictsRow = ((ConflictsRow)(this.NewRow()));
                rowConflictsRow.ItemArray = new object[] {
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
                                                             IsRead};
                this.Rows.Add(rowConflictsRow);
                return rowConflictsRow;
            }
            
            public System.Collections.IEnumerator GetEnumerator() {
                return this.Rows.GetEnumerator();
            }
            
            public override DataTable Clone() {
                ConflictsDataTable cln = ((ConflictsDataTable)(base.Clone()));
                cln.InitVars();
                return cln;
            }
            
            protected override DataTable CreateInstance() {
                return new ConflictsDataTable();
            }
            
            internal void InitVars() {
                this.columnIssueID = this.Columns["IssueID"];
                this.columnStafferID = this.Columns["StafferID"];
                this.columnIssueTypeID = this.Columns["IssueTypeID"];
                this.columnTitle = this.Columns["Title"];
                this.columnDescription = this.Columns["Description"];
                this.columnDateOpened = this.Columns["DateOpened"];
                this.columnDateClosed = this.Columns["DateClosed"];
                this.columnIsOpen = this.Columns["IsOpen"];
                this.columnDateModified = this.Columns["DateModified"];
                this.columnUserName = this.Columns["UserName"];
                this.columnDisplayName = this.Columns["DisplayName"];
                this.columnIssueType = this.Columns["IssueType"];
                this.columnHasConflicts = this.Columns["HasConflicts"];
                this.columnIsRead = this.Columns["IsRead"];
            }
            
            private void InitClass() {
                this.columnIssueID = new DataColumn("IssueID", typeof(int), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnIssueID);
                this.columnStafferID = new DataColumn("StafferID", typeof(int), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnStafferID);
                this.columnIssueTypeID = new DataColumn("IssueTypeID", typeof(int), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnIssueTypeID);
                this.columnTitle = new DataColumn("Title", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnTitle);
                this.columnDescription = new DataColumn("Description", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnDescription);
                this.columnDateOpened = new DataColumn("DateOpened", typeof(System.DateTime), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnDateOpened);
                this.columnDateClosed = new DataColumn("DateClosed", typeof(System.DateTime), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnDateClosed);
                this.columnIsOpen = new DataColumn("IsOpen", typeof(bool), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnIsOpen);
                this.columnDateModified = new DataColumn("DateModified", typeof(System.DateTime), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnDateModified);
                this.columnUserName = new DataColumn("UserName", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnUserName);
                this.columnDisplayName = new DataColumn("DisplayName", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnDisplayName);
                this.columnIssueType = new DataColumn("IssueType", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnIssueType);
                this.columnHasConflicts = new DataColumn("HasConflicts", typeof(bool), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnHasConflicts);
                this.columnIsRead = new DataColumn("IsRead", typeof(bool), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnIsRead);
                this.columnIssueID.AutoIncrement = true;
                this.columnIssueID.AllowDBNull = false;
                this.columnIssueID.ReadOnly = true;
                this.columnStafferID.AllowDBNull = false;
                this.columnIssueTypeID.AllowDBNull = false;
                this.columnTitle.AllowDBNull = false;
                this.columnDescription.AllowDBNull = false;
                this.columnDateOpened.AllowDBNull = false;
                this.columnIsOpen.AllowDBNull = false;
                this.columnDateModified.AllowDBNull = false;
                this.columnUserName.AllowDBNull = false;
                this.columnDisplayName.AllowDBNull = false;
                this.columnIssueType.AllowDBNull = false;
                this.columnHasConflicts.DefaultValue = false;
                this.columnIsRead.DefaultValue = false;
            }
            
            public ConflictsRow NewConflictsRow() {
                return ((ConflictsRow)(this.NewRow()));
            }
            
            protected override DataRow NewRowFromBuilder(DataRowBuilder builder) {
                return new ConflictsRow(builder);
            }
            
            protected override System.Type GetRowType() {
                return typeof(ConflictsRow);
            }
            
            protected override void OnRowChanged(DataRowChangeEventArgs e) {
                base.OnRowChanged(e);
                if ((this.ConflictsRowChanged != null)) {
                    this.ConflictsRowChanged(this, new ConflictsRowChangeEvent(((ConflictsRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowChanging(DataRowChangeEventArgs e) {
                base.OnRowChanging(e);
                if ((this.ConflictsRowChanging != null)) {
                    this.ConflictsRowChanging(this, new ConflictsRowChangeEvent(((ConflictsRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleted(DataRowChangeEventArgs e) {
                base.OnRowDeleted(e);
                if ((this.ConflictsRowDeleted != null)) {
                    this.ConflictsRowDeleted(this, new ConflictsRowChangeEvent(((ConflictsRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleting(DataRowChangeEventArgs e) {
                base.OnRowDeleting(e);
                if ((this.ConflictsRowDeleting != null)) {
                    this.ConflictsRowDeleting(this, new ConflictsRowChangeEvent(((ConflictsRow)(e.Row)), e.Action));
                }
            }
            
            public void RemoveConflictsRow(ConflictsRow row) {
                this.Rows.Remove(row);
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class ConflictsRow : DataRow {
            
            private ConflictsDataTable tableConflicts;
            
            internal ConflictsRow(DataRowBuilder rb) : 
                base(rb) {
                this.tableConflicts = ((ConflictsDataTable)(this.Table));
                }
            
            public int IssueID {
                get {
                    return ((int)(this[this.tableConflicts.IssueIDColumn]));
                }
                set {
                    this[this.tableConflicts.IssueIDColumn] = value;
                }
            }
            
            public int StafferID {
                get {
                    return ((int)(this[this.tableConflicts.StafferIDColumn]));
                }
                set {
                    this[this.tableConflicts.StafferIDColumn] = value;
                }
            }
            
            public int IssueTypeID {
                get {
                    return ((int)(this[this.tableConflicts.IssueTypeIDColumn]));
                }
                set {
                    this[this.tableConflicts.IssueTypeIDColumn] = value;
                }
            }
            
            public string Title {
                get {
                    return ((string)(this[this.tableConflicts.TitleColumn]));
                }
                set {
                    this[this.tableConflicts.TitleColumn] = value;
                }
            }
            
            public string Description {
                get {
                    return ((string)(this[this.tableConflicts.DescriptionColumn]));
                }
                set {
                    this[this.tableConflicts.DescriptionColumn] = value;
                }
            }
            
            public System.DateTime DateOpened {
                get {
                    return ((System.DateTime)(this[this.tableConflicts.DateOpenedColumn]));
                }
                set {
                    this[this.tableConflicts.DateOpenedColumn] = value;
                }
            }
            
            public System.DateTime DateClosed {
                get {
                    try {
                        return ((System.DateTime)(this[this.tableConflicts.DateClosedColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableConflicts.DateClosedColumn] = value;
                }
            }
            
            public bool IsOpen {
                get {
                    return ((bool)(this[this.tableConflicts.IsOpenColumn]));
                }
                set {
                    this[this.tableConflicts.IsOpenColumn] = value;
                }
            }
            
            public System.DateTime DateModified {
                get {
                    return ((System.DateTime)(this[this.tableConflicts.DateModifiedColumn]));
                }
                set {
                    this[this.tableConflicts.DateModifiedColumn] = value;
                }
            }
            
            public string UserName {
                get {
                    return ((string)(this[this.tableConflicts.UserNameColumn]));
                }
                set {
                    this[this.tableConflicts.UserNameColumn] = value;
                }
            }
            
            public string DisplayName {
                get {
                    return ((string)(this[this.tableConflicts.DisplayNameColumn]));
                }
                set {
                    this[this.tableConflicts.DisplayNameColumn] = value;
                }
            }
            
            public string IssueType {
                get {
                    return ((string)(this[this.tableConflicts.IssueTypeColumn]));
                }
                set {
                    this[this.tableConflicts.IssueTypeColumn] = value;
                }
            }
            
            public bool HasConflicts {
                get {
                    try {
                        return ((bool)(this[this.tableConflicts.HasConflictsColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableConflicts.HasConflictsColumn] = value;
                }
            }
            
            public bool IsRead {
                get {
                    try {
                        return ((bool)(this[this.tableConflicts.IsReadColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableConflicts.IsReadColumn] = value;
                }
            }
            
            public bool IsDateClosedNull() {
                return this.IsNull(this.tableConflicts.DateClosedColumn);
            }
            
            public void SetDateClosedNull() {
                this[this.tableConflicts.DateClosedColumn] = System.Convert.DBNull;
            }
            
            public bool IsHasConflictsNull() {
                return this.IsNull(this.tableConflicts.HasConflictsColumn);
            }
            
            public void SetHasConflictsNull() {
                this[this.tableConflicts.HasConflictsColumn] = System.Convert.DBNull;
            }
            
            public bool IsIsReadNull() {
                return this.IsNull(this.tableConflicts.IsReadColumn);
            }
            
            public void SetIsReadNull() {
                this[this.tableConflicts.IsReadColumn] = System.Convert.DBNull;
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class ConflictsRowChangeEvent : EventArgs {
            
            private ConflictsRow eventRow;
            
            private DataRowAction eventAction;
            
            public ConflictsRowChangeEvent(ConflictsRow row, DataRowAction action) {
                this.eventRow = row;
                this.eventAction = action;
            }
            
            public ConflictsRow Row {
                get {
                    return this.eventRow;
                }
            }
            
            public DataRowAction Action {
                get {
                    return this.eventAction;
                }
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class IssueTypesDataTable : DataTable, System.Collections.IEnumerable {
            
            private DataColumn columnIssueTypeID;
            
            private DataColumn columnIssueType;
            
            internal IssueTypesDataTable() : 
                base("IssueTypes") {
                this.InitClass();
                }
            
            internal IssueTypesDataTable(DataTable table) : 
                base(table.TableName) {
                if ((table.CaseSensitive != table.DataSet.CaseSensitive)) {
                    this.CaseSensitive = table.CaseSensitive;
                }
                if ((table.Locale.ToString() != table.DataSet.Locale.ToString())) {
                    this.Locale = table.Locale;
                }
                if ((table.Namespace != table.DataSet.Namespace)) {
                    this.Namespace = table.Namespace;
                }
                this.Prefix = table.Prefix;
                this.MinimumCapacity = table.MinimumCapacity;
                this.DisplayExpression = table.DisplayExpression;
                }

#if NOT_PFX
[System.ComponentModel.Browsable(false)]
#endif
            public int Count {
                get {
                    return this.Rows.Count;
                }
            }
            
            internal DataColumn IssueTypeIDColumn {
                get {
                    return this.columnIssueTypeID;
                }
            }
            
            internal DataColumn IssueTypeColumn {
                get {
                    return this.columnIssueType;
                }
            }
            
            public IssueTypesRow this[int index] {
                get {
                    return ((IssueTypesRow)(this.Rows[index]));
                }
            }
            
            public event IssueTypesRowChangeEventHandler IssueTypesRowChanged;
            
            public event IssueTypesRowChangeEventHandler IssueTypesRowChanging;
            
            public event IssueTypesRowChangeEventHandler IssueTypesRowDeleted;
            
            public event IssueTypesRowChangeEventHandler IssueTypesRowDeleting;
            
            public void AddIssueTypesRow(IssueTypesRow row) {
                this.Rows.Add(row);
            }
            
            public IssueTypesRow AddIssueTypesRow(string IssueType) {
                IssueTypesRow rowIssueTypesRow = ((IssueTypesRow)(this.NewRow()));
                rowIssueTypesRow.ItemArray = new object[] {
                                                              null,
                                                              IssueType};
                this.Rows.Add(rowIssueTypesRow);
                return rowIssueTypesRow;
            }
            
            public System.Collections.IEnumerator GetEnumerator() {
                return this.Rows.GetEnumerator();
            }
            
            public override DataTable Clone() {
                IssueTypesDataTable cln = ((IssueTypesDataTable)(base.Clone()));
                cln.InitVars();
                return cln;
            }
            
            protected override DataTable CreateInstance() {
                return new IssueTypesDataTable();
            }
            
            internal void InitVars() {
                this.columnIssueTypeID = this.Columns["IssueTypeID"];
                this.columnIssueType = this.Columns["IssueType"];
            }
            
            private void InitClass() {
                this.columnIssueTypeID = new DataColumn("IssueTypeID", typeof(int), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnIssueTypeID);
                this.columnIssueType = new DataColumn("IssueType", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnIssueType);
                this.columnIssueTypeID.AutoIncrement = true;
                this.columnIssueTypeID.AllowDBNull = false;
                this.columnIssueTypeID.ReadOnly = true;
                this.columnIssueType.AllowDBNull = false;
            }
            
            public IssueTypesRow NewIssueTypesRow() {
                return ((IssueTypesRow)(this.NewRow()));
            }
            
            protected override DataRow NewRowFromBuilder(DataRowBuilder builder) {
                return new IssueTypesRow(builder);
            }
            
            protected override System.Type GetRowType() {
                return typeof(IssueTypesRow);
            }
            
            protected override void OnRowChanged(DataRowChangeEventArgs e) {
                base.OnRowChanged(e);
                if ((this.IssueTypesRowChanged != null)) {
                    this.IssueTypesRowChanged(this, new IssueTypesRowChangeEvent(((IssueTypesRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowChanging(DataRowChangeEventArgs e) {
                base.OnRowChanging(e);
                if ((this.IssueTypesRowChanging != null)) {
                    this.IssueTypesRowChanging(this, new IssueTypesRowChangeEvent(((IssueTypesRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleted(DataRowChangeEventArgs e) {
                base.OnRowDeleted(e);
                if ((this.IssueTypesRowDeleted != null)) {
                    this.IssueTypesRowDeleted(this, new IssueTypesRowChangeEvent(((IssueTypesRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleting(DataRowChangeEventArgs e) {
                base.OnRowDeleting(e);
                if ((this.IssueTypesRowDeleting != null)) {
                    this.IssueTypesRowDeleting(this, new IssueTypesRowChangeEvent(((IssueTypesRow)(e.Row)), e.Action));
                }
            }
            
            public void RemoveIssueTypesRow(IssueTypesRow row) {
                this.Rows.Remove(row);
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class IssueTypesRow : DataRow {
            
            private IssueTypesDataTable tableIssueTypes;
            
            internal IssueTypesRow(DataRowBuilder rb) : 
                base(rb) {
                this.tableIssueTypes = ((IssueTypesDataTable)(this.Table));
                }
            
            public int IssueTypeID {
                get {
                    return ((int)(this[this.tableIssueTypes.IssueTypeIDColumn]));
                }
                set {
                    this[this.tableIssueTypes.IssueTypeIDColumn] = value;
                }
            }
            
            public string IssueType {
                get {
                    return ((string)(this[this.tableIssueTypes.IssueTypeColumn]));
                }
                set {
                    this[this.tableIssueTypes.IssueTypeColumn] = value;
                }
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class IssueTypesRowChangeEvent : EventArgs {
            
            private IssueTypesRow eventRow;
            
            private DataRowAction eventAction;
            
            public IssueTypesRowChangeEvent(IssueTypesRow row, DataRowAction action) {
                this.eventRow = row;
                this.eventAction = action;
            }
            
            public IssueTypesRow Row {
                get {
                    return this.eventRow;
                }
            }
            
            public DataRowAction Action {
                get {
                    return this.eventAction;
                }
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class StaffersDataTable : DataTable, System.Collections.IEnumerable {
            
            private DataColumn columnStafferID;
            
            private DataColumn columnUserName;
            
            private DataColumn columnDisplayName;
            
            private DataColumn columnStafferType;
            
            internal StaffersDataTable() : 
                base("Staffers") {
                this.InitClass();
                }
            
            internal StaffersDataTable(DataTable table) : 
                base(table.TableName) {
                if ((table.CaseSensitive != table.DataSet.CaseSensitive)) {
                    this.CaseSensitive = table.CaseSensitive;
                }
                if ((table.Locale.ToString() != table.DataSet.Locale.ToString())) {
                    this.Locale = table.Locale;
                }
                if ((table.Namespace != table.DataSet.Namespace)) {
                    this.Namespace = table.Namespace;
                }
                this.Prefix = table.Prefix;
                this.MinimumCapacity = table.MinimumCapacity;
                this.DisplayExpression = table.DisplayExpression;
                }

#if NOT_PFX
[System.ComponentModel.Browsable(false)]
#endif
            public int Count {
                get {
                    return this.Rows.Count;
                }
            }
            
            internal DataColumn StafferIDColumn {
                get {
                    return this.columnStafferID;
                }
            }
            
            internal DataColumn UserNameColumn {
                get {
                    return this.columnUserName;
                }
            }
            
            internal DataColumn DisplayNameColumn {
                get {
                    return this.columnDisplayName;
                }
            }
            
            internal DataColumn StafferTypeColumn {
                get {
                    return this.columnStafferType;
                }
            }
            
            public StaffersRow this[int index] {
                get {
                    return ((StaffersRow)(this.Rows[index]));
                }
            }
            
            public event StaffersRowChangeEventHandler StaffersRowChanged;
            
            public event StaffersRowChangeEventHandler StaffersRowChanging;
            
            public event StaffersRowChangeEventHandler StaffersRowDeleted;
            
            public event StaffersRowChangeEventHandler StaffersRowDeleting;
            
            public void AddStaffersRow(StaffersRow row) {
                this.Rows.Add(row);
            }
            
            public StaffersRow AddStaffersRow(string UserName, string DisplayName, string StafferType) {
                StaffersRow rowStaffersRow = ((StaffersRow)(this.NewRow()));
                rowStaffersRow.ItemArray = new object[] {
                                                            null,
                                                            UserName,
                                                            DisplayName,
                                                            StafferType};
                this.Rows.Add(rowStaffersRow);
                return rowStaffersRow;
            }
            
            public System.Collections.IEnumerator GetEnumerator() {
                return this.Rows.GetEnumerator();
            }
            
            public override DataTable Clone() {
                StaffersDataTable cln = ((StaffersDataTable)(base.Clone()));
                cln.InitVars();
                return cln;
            }
            
            protected override DataTable CreateInstance() {
                return new StaffersDataTable();
            }
            
            internal void InitVars() {
                this.columnStafferID = this.Columns["StafferID"];
                this.columnUserName = this.Columns["UserName"];
                this.columnDisplayName = this.Columns["DisplayName"];
                this.columnStafferType = this.Columns["StafferType"];
            }
            
            private void InitClass() {
                this.columnStafferID = new DataColumn("StafferID", typeof(int), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnStafferID);
                this.columnUserName = new DataColumn("UserName", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnUserName);
                this.columnDisplayName = new DataColumn("DisplayName", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnDisplayName);
                this.columnStafferType = new DataColumn("StafferType", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnStafferType);
                this.columnStafferID.AutoIncrement = true;
                this.columnStafferID.AllowDBNull = false;
                this.columnStafferID.ReadOnly = true;
                this.columnUserName.AllowDBNull = false;
                this.columnDisplayName.AllowDBNull = false;
                this.columnStafferType.AllowDBNull = false;
            }
            
            public StaffersRow NewStaffersRow() {
                return ((StaffersRow)(this.NewRow()));
            }
            
            protected override DataRow NewRowFromBuilder(DataRowBuilder builder) {
                return new StaffersRow(builder);
            }
            
            protected override System.Type GetRowType() {
                return typeof(StaffersRow);
            }
            
            protected override void OnRowChanged(DataRowChangeEventArgs e) {
                base.OnRowChanged(e);
                if ((this.StaffersRowChanged != null)) {
                    this.StaffersRowChanged(this, new StaffersRowChangeEvent(((StaffersRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowChanging(DataRowChangeEventArgs e) {
                base.OnRowChanging(e);
                if ((this.StaffersRowChanging != null)) {
                    this.StaffersRowChanging(this, new StaffersRowChangeEvent(((StaffersRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleted(DataRowChangeEventArgs e) {
                base.OnRowDeleted(e);
                if ((this.StaffersRowDeleted != null)) {
                    this.StaffersRowDeleted(this, new StaffersRowChangeEvent(((StaffersRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleting(DataRowChangeEventArgs e) {
                base.OnRowDeleting(e);
                if ((this.StaffersRowDeleting != null)) {
                    this.StaffersRowDeleting(this, new StaffersRowChangeEvent(((StaffersRow)(e.Row)), e.Action));
                }
            }
            
            public void RemoveStaffersRow(StaffersRow row) {
                this.Rows.Remove(row);
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class StaffersRow : DataRow {
            
            private StaffersDataTable tableStaffers;
            
            internal StaffersRow(DataRowBuilder rb) : 
                base(rb) {
                this.tableStaffers = ((StaffersDataTable)(this.Table));
                }
            
            public int StafferID {
                get {
                    return ((int)(this[this.tableStaffers.StafferIDColumn]));
                }
                set {
                    this[this.tableStaffers.StafferIDColumn] = value;
                }
            }
            
            public string UserName {
                get {
                    return ((string)(this[this.tableStaffers.UserNameColumn]));
                }
                set {
                    this[this.tableStaffers.UserNameColumn] = value;
                }
            }
            
            public string DisplayName {
                get {
                    return ((string)(this[this.tableStaffers.DisplayNameColumn]));
                }
                set {
                    this[this.tableStaffers.DisplayNameColumn] = value;
                }
            }
            
            public string StafferType {
                get {
                    return ((string)(this[this.tableStaffers.StafferTypeColumn]));
                }
                set {
                    this[this.tableStaffers.StafferTypeColumn] = value;
                }
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class StaffersRowChangeEvent : EventArgs {
            
            private StaffersRow eventRow;
            
            private DataRowAction eventAction;
            
            public StaffersRowChangeEvent(StaffersRow row, DataRowAction action) {
                this.eventRow = row;
                this.eventAction = action;
            }
            
            public StaffersRow Row {
                get {
                    return this.eventRow;
                }
            }
            
            public DataRowAction Action {
                get {
                    return this.eventAction;
                }
            }
        }
    }
}