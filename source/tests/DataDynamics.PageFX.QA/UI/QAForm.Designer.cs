namespace DataDynamics.PageFX
{
    partial class QAForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QAForm));
			Fireball.Windows.Forms.LineMarginRender lineMarginRender3 = new Fireball.Windows.Forms.LineMarginRender();
			Fireball.Windows.Forms.LineMarginRender lineMarginRender1 = new Fireball.Windows.Forms.LineMarginRender();
			this.toolbar = new System.Windows.Forms.ToolStrip();
			this.btnSaveSettings = new System.Windows.Forms.ToolStripButton();
			this.btnRunAll = new System.Windows.Forms.ToolStripButton();
			this.btnRun = new System.Windows.Forms.ToolStripButton();
			this.btnStop = new System.Windows.Forms.ToolStripButton();
			this.btnSelectFailed = new System.Windows.Forms.ToolStripButton();
			this.btnCopy = new System.Windows.Forms.ToolStripButton();
			this.btnCompile = new System.Windows.Forms.ToolStripButton();
			this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
			this.miAbcSerialization = new System.Windows.Forms.ToolStripMenuItem();
			this.miSwfSerialization = new System.Windows.Forms.ToolStripMenuItem();
			this.miCliDeserialization = new System.Windows.Forms.ToolStripMenuItem();
			this.btnOptions = new System.Windows.Forms.ToolStripDropDownButton();
			this.miUseCommonDirectory = new System.Windows.Forms.ToolStripMenuItem();
			this.miCompilerOptions = new System.Windows.Forms.ToolStripMenuItem();
			this.miOptimizeCode = new System.Windows.Forms.ToolStripMenuItem();
			this.miEmitDebugInfo = new System.Windows.Forms.ToolStripMenuItem();
			this.miDumpILCode = new System.Windows.Forms.ToolStripMenuItem();
			this.miDumpILMap = new System.Windows.Forms.ToolStripMenuItem();
			this.miDumpSrc = new System.Windows.Forms.ToolStripMenuItem();
			this.miAvmDump = new System.Windows.Forms.ToolStripMenuItem();
			this.miAbcDumpOptions = new System.Windows.Forms.ToolStripMenuItem();
			this.miAbcDumpEnabled = new System.Windows.Forms.ToolStripMenuItem();
			this.miAbcDumpTextFormat = new System.Windows.Forms.ToolStripMenuItem();
			this.miAbcDumpXmlFormat = new System.Windows.Forms.ToolStripMenuItem();
			this.miAbcDumpCode = new System.Windows.Forms.ToolStripMenuItem();
			this.miAbcDumpClassList = new System.Windows.Forms.ToolStripMenuItem();
			this.miAbcDumpTraits = new System.Windows.Forms.ToolStripMenuItem();
			this.miGraphViz = new System.Windows.Forms.ToolStripMenuItem();
			this.miGraphVizStruct = new System.Windows.Forms.ToolStripMenuItem();
			this.miRecordLastError = new System.Windows.Forms.ToolStripMenuItem();
			this.miDebuggerBreak = new System.Windows.Forms.ToolStripMenuItem();
			this.miBreaks = new System.Windows.Forms.ToolStripMenuItem();
			this.miBreakWhileLoops = new System.Windows.Forms.ToolStripMenuItem();
			this.miBreakDoWhileLoops = new System.Windows.Forms.ToolStripMenuItem();
			this.miBreakEndlessLoops = new System.Windows.Forms.ToolStripMenuItem();
			this.miBreakInvalidMetadataToken = new System.Windows.Forms.ToolStripMenuItem();
			this.miBreakInvalidMemberReference = new System.Windows.Forms.ToolStripMenuItem();
			this.miBreakInvalidTypeReference = new System.Windows.Forms.ToolStripMenuItem();
			this.miBreakUnresolvedInternalCall = new System.Windows.Forms.ToolStripMenuItem();
			this.miShowReport = new System.Windows.Forms.ToolStripMenuItem();
			this.miPDBTestMode = new System.Windows.Forms.ToolStripMenuItem();
			this.cbAvmShellMode = new System.Windows.Forms.ToolStripComboBox();
			this.btnResetDebugHooks = new System.Windows.Forms.ToolStripButton();
			this.btnResetLastError = new System.Windows.Forms.ToolStripButton();
			this.btnLogSwitch = new System.Windows.Forms.ToolStripButton();
			this.cbTypeName = new System.Windows.Forms.ToolStripComboBox();
			this.cbMethodName = new System.Windows.Forms.ToolStripComboBox();
			this.tbPhase = new System.Windows.Forms.ToolStripTextBox();
			this.btnNUnitSession = new System.Windows.Forms.ToolStripButton();
			this.worker = new System.ComponentModel.BackgroundWorker();
			this.splitMain = new System.Windows.Forms.SplitContainer();
			this.testCases = new System.Windows.Forms.TreeView();
			this.tabsRight = new System.Windows.Forms.TabControl();
			this.tabSourceCode = new System.Windows.Forms.TabPage();
			this.sourceFiles = new System.Windows.Forms.TabControl();
			this.tabDecompiledCode = new System.Windows.Forms.TabPage();
			this.editDecompiledCode = new Fireball.Windows.Forms.CodeEditorControl();
			this.docDecompiledCode = new Fireball.Syntax.SyntaxDocument(this.components);
			this.tabOutput1 = new System.Windows.Forms.TabPage();
			this.editOutput1 = new Fireball.Windows.Forms.CodeEditorControl();
			this.docOutput1 = new Fireball.Syntax.SyntaxDocument(this.components);
			this.tabOutput2 = new System.Windows.Forms.TabPage();
			this.editOutput2 = new Fireball.Windows.Forms.CodeEditorControl();
			this.docOutput2 = new Fireball.Syntax.SyntaxDocument(this.components);
			this.tabError = new System.Windows.Forms.TabPage();
			this.editError = new Fireball.Windows.Forms.CodeEditorControl();
			this.docError = new Fireball.Syntax.SyntaxDocument(this.components);
			this.docAvmDump = new Fireball.Syntax.SyntaxDocument(this.components);
			this.docAbcDump = new Fireball.Syntax.SyntaxDocument(this.components);
			this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
			this.statusbar = new System.Windows.Forms.StatusStrip();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.btabs = new System.Windows.Forms.TabControl();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.tbLog = new System.Windows.Forms.RichTextBox();
			this.menubar = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.miGenerateHtmlReport = new System.Windows.Forms.ToolStripMenuItem();
			this.miGenerateWikiReport = new System.Windows.Forms.ToolStripMenuItem();
			this.miExit = new System.Windows.Forms.ToolStripMenuItem();
			this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.miRun = new System.Windows.Forms.ToolStripMenuItem();
			this.miRunAll = new System.Windows.Forms.ToolStripMenuItem();
			this.miStop = new System.Windows.Forms.ToolStripMenuItem();
			this.miCleanQADir = new System.Windows.Forms.ToolStripMenuItem();
			this.miCompareTextFiles = new System.Windows.Forms.ToolStripMenuItem();
			this.miGenerateNUnitTests = new System.Windows.Forms.ToolStripMenuItem();
			this.miGenerateSelectedNUnitTests = new System.Windows.Forms.ToolStripMenuItem();
			this.miOptions = new System.Windows.Forms.ToolStripMenuItem();
			this.docOutput = new Fireball.Syntax.SyntaxDocument(this.components);
			this.miClrEmulation = new System.Windows.Forms.ToolStripMenuItem();
			this.toolbar.SuspendLayout();
			this.splitMain.Panel1.SuspendLayout();
			this.splitMain.Panel2.SuspendLayout();
			this.splitMain.SuspendLayout();
			this.tabsRight.SuspendLayout();
			this.tabSourceCode.SuspendLayout();
			this.tabDecompiledCode.SuspendLayout();
			this.tabOutput1.SuspendLayout();
			this.tabOutput2.SuspendLayout();
			this.tabError.SuspendLayout();
			this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
			this.toolStripContainer1.ContentPanel.SuspendLayout();
			this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
			this.toolStripContainer1.SuspendLayout();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.btabs.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.menubar.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolbar
			// 
			this.toolbar.Dock = System.Windows.Forms.DockStyle.None;
			this.toolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSaveSettings,
            this.btnRunAll,
            this.btnRun,
            this.btnStop,
            this.btnSelectFailed,
            this.btnCopy,
            this.btnCompile,
            this.toolStripDropDownButton1,
            this.btnOptions,
            this.cbAvmShellMode,
            this.btnResetDebugHooks,
            this.btnResetLastError,
            this.btnLogSwitch,
            this.cbTypeName,
            this.cbMethodName,
            this.tbPhase,
            this.btnNUnitSession});
			this.toolbar.Location = new System.Drawing.Point(0, 24);
			this.toolbar.Name = "toolbar";
			this.toolbar.Size = new System.Drawing.Size(892, 25);
			this.toolbar.Stretch = true;
			this.toolbar.TabIndex = 0;
			this.toolbar.Text = "toolStrip1";
			// 
			// btnSaveSettings
			// 
			this.btnSaveSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnSaveSettings.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveSettings.Image")));
			this.btnSaveSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnSaveSettings.Name = "btnSaveSettings";
			this.btnSaveSettings.Size = new System.Drawing.Size(23, 22);
			this.btnSaveSettings.Text = "Save Settings";
			this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
			// 
			// btnRunAll
			// 
			this.btnRunAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnRunAll.Image = ((System.Drawing.Image)(resources.GetObject("btnRunAll.Image")));
			this.btnRunAll.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnRunAll.Name = "btnRunAll";
			this.btnRunAll.Size = new System.Drawing.Size(23, 22);
			this.btnRunAll.Text = "Run All Test Cases";
			this.btnRunAll.Click += new System.EventHandler(this.btnRunAll_Click);
			// 
			// btnRun
			// 
			this.btnRun.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnRun.Image = ((System.Drawing.Image)(resources.GetObject("btnRun.Image")));
			this.btnRun.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnRun.Name = "btnRun";
			this.btnRun.Size = new System.Drawing.Size(23, 22);
			this.btnRun.Text = "Run Selected Test Cases";
			this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
			// 
			// btnStop
			// 
			this.btnStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnStop.Image = ((System.Drawing.Image)(resources.GetObject("btnStop.Image")));
			this.btnStop.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(23, 22);
			this.btnStop.Text = "Stop";
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// btnSelectFailed
			// 
			this.btnSelectFailed.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnSelectFailed.Image = ((System.Drawing.Image)(resources.GetObject("btnSelectFailed.Image")));
			this.btnSelectFailed.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnSelectFailed.Name = "btnSelectFailed";
			this.btnSelectFailed.Size = new System.Drawing.Size(23, 22);
			this.btnSelectFailed.Text = "Select Failed";
			this.btnSelectFailed.Click += new System.EventHandler(this.btnSelectFailed_Click);
			// 
			// btnCopy
			// 
			this.btnCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnCopy.Image = ((System.Drawing.Image)(resources.GetObject("btnCopy.Image")));
			this.btnCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnCopy.Name = "btnCopy";
			this.btnCopy.Size = new System.Drawing.Size(23, 22);
			this.btnCopy.Text = "Copy";
			this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
			// 
			// btnCompile
			// 
			this.btnCompile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnCompile.Image = ((System.Drawing.Image)(resources.GetObject("btnCompile.Image")));
			this.btnCompile.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnCompile.Name = "btnCompile";
			this.btnCompile.Size = new System.Drawing.Size(23, 22);
			this.btnCompile.Text = "Compile";
			this.btnCompile.Click += new System.EventHandler(this.btnCompile_Click);
			// 
			// toolStripDropDownButton1
			// 
			this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miAbcSerialization,
            this.miSwfSerialization,
            this.miCliDeserialization,
            this.miClrEmulation});
			this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
			this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
			this.toolStripDropDownButton1.Size = new System.Drawing.Size(29, 22);
			this.toolStripDropDownButton1.Text = "Test Driver";
			// 
			// miAbcSerialization
			// 
			this.miAbcSerialization.Checked = true;
			this.miAbcSerialization.CheckState = System.Windows.Forms.CheckState.Checked;
			this.miAbcSerialization.Name = "miAbcSerialization";
			this.miAbcSerialization.Size = new System.Drawing.Size(170, 22);
			this.miAbcSerialization.Text = "ABC Serialization";
			this.miAbcSerialization.Click += new System.EventHandler(this.OnSelectTestDriver);
			// 
			// miSwfSerialization
			// 
			this.miSwfSerialization.Name = "miSwfSerialization";
			this.miSwfSerialization.Size = new System.Drawing.Size(170, 22);
			this.miSwfSerialization.Text = "SWF Serialization";
			this.miSwfSerialization.Click += new System.EventHandler(this.OnSelectTestDriver);
			// 
			// miCliDeserialization
			// 
			this.miCliDeserialization.Name = "miCliDeserialization";
			this.miCliDeserialization.Size = new System.Drawing.Size(170, 22);
			this.miCliDeserialization.Text = "CLI Deserialization";
			this.miCliDeserialization.Click += new System.EventHandler(this.OnSelectTestDriver);
			// 
			// btnOptions
			// 
			this.btnOptions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miUseCommonDirectory,
            this.miCompilerOptions,
            this.miDumpILCode,
            this.miDumpILMap,
            this.miDumpSrc,
            this.miAvmDump,
            this.miAbcDumpOptions,
            this.miGraphViz,
            this.miGraphVizStruct,
            this.miRecordLastError,
            this.miDebuggerBreak,
            this.miBreaks,
            this.miShowReport,
            this.miPDBTestMode});
			this.btnOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnOptions.Image")));
			this.btnOptions.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnOptions.Name = "btnOptions";
			this.btnOptions.Size = new System.Drawing.Size(29, 22);
			this.btnOptions.Text = "Options";
			this.btnOptions.ToolTipText = "Options";
			// 
			// miUseCommonDirectory
			// 
			this.miUseCommonDirectory.CheckOnClick = true;
			this.miUseCommonDirectory.Name = "miUseCommonDirectory";
			this.miUseCommonDirectory.Size = new System.Drawing.Size(216, 22);
			this.miUseCommonDirectory.Text = "Use Common Directory";
			this.miUseCommonDirectory.Click += new System.EventHandler(this.OnOptionMenuItemClick);
			// 
			// miCompilerOptions
			// 
			this.miCompilerOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miOptimizeCode,
            this.miEmitDebugInfo});
			this.miCompilerOptions.Name = "miCompilerOptions";
			this.miCompilerOptions.Size = new System.Drawing.Size(216, 22);
			this.miCompilerOptions.Text = "Compiler Options";
			// 
			// miOptimizeCode
			// 
			this.miOptimizeCode.CheckOnClick = true;
			this.miOptimizeCode.Name = "miOptimizeCode";
			this.miOptimizeCode.Size = new System.Drawing.Size(160, 22);
			this.miOptimizeCode.Text = "Optimize";
			this.miOptimizeCode.Click += new System.EventHandler(this.OnOptionMenuItemClick);
			// 
			// miEmitDebugInfo
			// 
			this.miEmitDebugInfo.CheckOnClick = true;
			this.miEmitDebugInfo.Name = "miEmitDebugInfo";
			this.miEmitDebugInfo.Size = new System.Drawing.Size(160, 22);
			this.miEmitDebugInfo.Text = "Emit Debug Info";
			this.miEmitDebugInfo.Click += new System.EventHandler(this.OnOptionMenuItemClick);
			// 
			// miDumpILCode
			// 
			this.miDumpILCode.CheckOnClick = true;
			this.miDumpILCode.Name = "miDumpILCode";
			this.miDumpILCode.Size = new System.Drawing.Size(216, 22);
			this.miDumpILCode.Tag = "DumpILCode";
			this.miDumpILCode.Text = "Dump IL Code";
			this.miDumpILCode.Click += new System.EventHandler(this.OnOptionMenuItemClick);
			// 
			// miDumpILMap
			// 
			this.miDumpILMap.CheckOnClick = true;
			this.miDumpILMap.Name = "miDumpILMap";
			this.miDumpILMap.Size = new System.Drawing.Size(216, 22);
			this.miDumpILMap.Tag = "DumpILMap";
			this.miDumpILMap.Text = "Dump IL Map";
			this.miDumpILMap.Click += new System.EventHandler(this.OnOptionMenuItemClick);
			// 
			// miDumpSrc
			// 
			this.miDumpSrc.Checked = true;
			this.miDumpSrc.CheckOnClick = true;
			this.miDumpSrc.CheckState = System.Windows.Forms.CheckState.Checked;
			this.miDumpSrc.Name = "miDumpSrc";
			this.miDumpSrc.Size = new System.Drawing.Size(216, 22);
			this.miDumpSrc.Tag = "DumpSrc";
			this.miDumpSrc.Text = "Dump C# Code";
			this.miDumpSrc.Click += new System.EventHandler(this.OnOptionMenuItemClick);
			// 
			// miAvmDump
			// 
			this.miAvmDump.CheckOnClick = true;
			this.miAvmDump.Name = "miAvmDump";
			this.miAvmDump.Size = new System.Drawing.Size(216, 22);
			this.miAvmDump.Tag = "AvmDump";
			this.miAvmDump.Text = "AVM Dump";
			this.miAvmDump.Click += new System.EventHandler(this.OnOptionMenuItemClick);
			// 
			// miAbcDumpOptions
			// 
			this.miAbcDumpOptions.CheckOnClick = true;
			this.miAbcDumpOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miAbcDumpEnabled,
            this.miAbcDumpTextFormat,
            this.miAbcDumpXmlFormat,
            this.miAbcDumpCode,
            this.miAbcDumpClassList,
            this.miAbcDumpTraits});
			this.miAbcDumpOptions.Name = "miAbcDumpOptions";
			this.miAbcDumpOptions.Size = new System.Drawing.Size(216, 22);
			this.miAbcDumpOptions.Text = "ABC Dump";
			// 
			// miAbcDumpEnabled
			// 
			this.miAbcDumpEnabled.CheckOnClick = true;
			this.miAbcDumpEnabled.Name = "miAbcDumpEnabled";
			this.miAbcDumpEnabled.Size = new System.Drawing.Size(158, 22);
			this.miAbcDumpEnabled.Tag = "AbcDump";
			this.miAbcDumpEnabled.Text = "Enabled";
			this.miAbcDumpEnabled.Click += new System.EventHandler(this.OnOptionMenuItemClick);
			// 
			// miAbcDumpTextFormat
			// 
			this.miAbcDumpTextFormat.CheckOnClick = true;
			this.miAbcDumpTextFormat.Name = "miAbcDumpTextFormat";
			this.miAbcDumpTextFormat.Size = new System.Drawing.Size(158, 22);
			this.miAbcDumpTextFormat.Tag = "AbcDumpTextFormat";
			this.miAbcDumpTextFormat.Text = "Text Format";
			this.miAbcDumpTextFormat.Click += new System.EventHandler(this.OnOptionMenuItemClick);
			// 
			// miAbcDumpXmlFormat
			// 
			this.miAbcDumpXmlFormat.CheckOnClick = true;
			this.miAbcDumpXmlFormat.Name = "miAbcDumpXmlFormat";
			this.miAbcDumpXmlFormat.Size = new System.Drawing.Size(158, 22);
			this.miAbcDumpXmlFormat.Tag = "AbcDumpXmlFormat";
			this.miAbcDumpXmlFormat.Text = "Xml Format";
			this.miAbcDumpXmlFormat.Click += new System.EventHandler(this.OnOptionMenuItemClick);
			// 
			// miAbcDumpCode
			// 
			this.miAbcDumpCode.CheckOnClick = true;
			this.miAbcDumpCode.Name = "miAbcDumpCode";
			this.miAbcDumpCode.Size = new System.Drawing.Size(158, 22);
			this.miAbcDumpCode.Tag = "AbcDumpCode";
			this.miAbcDumpCode.Text = "Dump Code";
			this.miAbcDumpCode.Click += new System.EventHandler(this.OnOptionMenuItemClick);
			// 
			// miAbcDumpClassList
			// 
			this.miAbcDumpClassList.CheckOnClick = true;
			this.miAbcDumpClassList.Name = "miAbcDumpClassList";
			this.miAbcDumpClassList.Size = new System.Drawing.Size(158, 22);
			this.miAbcDumpClassList.Tag = "AbcDumpClassList";
			this.miAbcDumpClassList.Text = "Dump Class List";
			this.miAbcDumpClassList.Click += new System.EventHandler(this.OnOptionMenuItemClick);
			// 
			// miAbcDumpTraits
			// 
			this.miAbcDumpTraits.Name = "miAbcDumpTraits";
			this.miAbcDumpTraits.Size = new System.Drawing.Size(158, 22);
			this.miAbcDumpTraits.Tag = "AbcDumpTraits";
			this.miAbcDumpTraits.Text = "Dump Traits";
			this.miAbcDumpTraits.Click += new System.EventHandler(this.OnOptionMenuItemClick);
			// 
			// miGraphViz
			// 
			this.miGraphViz.CheckOnClick = true;
			this.miGraphViz.Name = "miGraphViz";
			this.miGraphViz.Size = new System.Drawing.Size(216, 22);
			this.miGraphViz.Tag = "GraphViz";
			this.miGraphViz.Text = "Visualize Graph";
			this.miGraphViz.Click += new System.EventHandler(this.OnOptionMenuItemClick);
			// 
			// miGraphVizStruct
			// 
			this.miGraphVizStruct.CheckOnClick = true;
			this.miGraphVizStruct.Name = "miGraphVizStruct";
			this.miGraphVizStruct.Size = new System.Drawing.Size(216, 22);
			this.miGraphVizStruct.Tag = "GraphVizStruct";
			this.miGraphVizStruct.Text = "Visualize Graph Structuring";
			this.miGraphVizStruct.Click += new System.EventHandler(this.OnOptionMenuItemClick);
			// 
			// miRecordLastError
			// 
			this.miRecordLastError.CheckOnClick = true;
			this.miRecordLastError.Name = "miRecordLastError";
			this.miRecordLastError.Size = new System.Drawing.Size(216, 22);
			this.miRecordLastError.Tag = "RecordLastError";
			this.miRecordLastError.Text = "Record Last Error";
			this.miRecordLastError.Click += new System.EventHandler(this.OnOptionMenuItemClick);
			// 
			// miDebuggerBreak
			// 
			this.miDebuggerBreak.CheckOnClick = true;
			this.miDebuggerBreak.Name = "miDebuggerBreak";
			this.miDebuggerBreak.Size = new System.Drawing.Size(216, 22);
			this.miDebuggerBreak.Tag = "DebuggerBreak";
			this.miDebuggerBreak.Text = "Debugger Break";
			this.miDebuggerBreak.Click += new System.EventHandler(this.OnOptionMenuItemClick);
			// 
			// miBreaks
			// 
			this.miBreaks.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miBreakWhileLoops,
            this.miBreakDoWhileLoops,
            this.miBreakEndlessLoops,
            this.miBreakInvalidMetadataToken,
            this.miBreakInvalidMemberReference,
            this.miBreakInvalidTypeReference,
            this.miBreakUnresolvedInternalCall});
			this.miBreaks.Name = "miBreaks";
			this.miBreaks.Size = new System.Drawing.Size(216, 22);
			this.miBreaks.Text = "Breaks";
			// 
			// miBreakWhileLoops
			// 
			this.miBreakWhileLoops.CheckOnClick = true;
			this.miBreakWhileLoops.Name = "miBreakWhileLoops";
			this.miBreakWhileLoops.Size = new System.Drawing.Size(212, 22);
			this.miBreakWhileLoops.Tag = "BreakWhileLoops";
			this.miBreakWhileLoops.Text = "Break While Loops";
			this.miBreakWhileLoops.Click += new System.EventHandler(this.OnOptionMenuItemClick);
			// 
			// miBreakDoWhileLoops
			// 
			this.miBreakDoWhileLoops.CheckOnClick = true;
			this.miBreakDoWhileLoops.Name = "miBreakDoWhileLoops";
			this.miBreakDoWhileLoops.Size = new System.Drawing.Size(212, 22);
			this.miBreakDoWhileLoops.Tag = "BreakDoWhileLoops";
			this.miBreakDoWhileLoops.Text = "Break DoWhile Loops";
			this.miBreakDoWhileLoops.Click += new System.EventHandler(this.OnOptionMenuItemClick);
			// 
			// miBreakEndlessLoops
			// 
			this.miBreakEndlessLoops.CheckOnClick = true;
			this.miBreakEndlessLoops.Name = "miBreakEndlessLoops";
			this.miBreakEndlessLoops.Size = new System.Drawing.Size(212, 22);
			this.miBreakEndlessLoops.Tag = "BreakEndlessLoops";
			this.miBreakEndlessLoops.Text = "Break Endless Loops";
			this.miBreakEndlessLoops.Click += new System.EventHandler(this.OnOptionMenuItemClick);
			// 
			// miBreakInvalidMetadataToken
			// 
			this.miBreakInvalidMetadataToken.CheckOnClick = true;
			this.miBreakInvalidMetadataToken.Name = "miBreakInvalidMetadataToken";
			this.miBreakInvalidMetadataToken.Size = new System.Drawing.Size(212, 22);
			this.miBreakInvalidMetadataToken.Tag = "BreakInvalidMetadataToken";
			this.miBreakInvalidMetadataToken.Text = "Invalid Metadata Token";
			this.miBreakInvalidMetadataToken.Click += new System.EventHandler(this.OnOptionMenuItemClick);
			// 
			// miBreakInvalidMemberReference
			// 
			this.miBreakInvalidMemberReference.CheckOnClick = true;
			this.miBreakInvalidMemberReference.Name = "miBreakInvalidMemberReference";
			this.miBreakInvalidMemberReference.Size = new System.Drawing.Size(212, 22);
			this.miBreakInvalidMemberReference.Tag = "BreakInvalidMemberReference";
			this.miBreakInvalidMemberReference.Text = "Invalid Member Reference";
			this.miBreakInvalidMemberReference.Click += new System.EventHandler(this.OnOptionMenuItemClick);
			// 
			// miBreakInvalidTypeReference
			// 
			this.miBreakInvalidTypeReference.CheckOnClick = true;
			this.miBreakInvalidTypeReference.Name = "miBreakInvalidTypeReference";
			this.miBreakInvalidTypeReference.Size = new System.Drawing.Size(212, 22);
			this.miBreakInvalidTypeReference.Tag = "BreakInvalidTypeReference";
			this.miBreakInvalidTypeReference.Text = "Invalid Type Reference";
			this.miBreakInvalidTypeReference.Click += new System.EventHandler(this.OnOptionMenuItemClick);
			// 
			// miBreakUnresolvedInternalCall
			// 
			this.miBreakUnresolvedInternalCall.CheckOnClick = true;
			this.miBreakUnresolvedInternalCall.Name = "miBreakUnresolvedInternalCall";
			this.miBreakUnresolvedInternalCall.Size = new System.Drawing.Size(212, 22);
			this.miBreakUnresolvedInternalCall.Tag = "BreakUnresolvedInternalCall";
			this.miBreakUnresolvedInternalCall.Text = "Unresolved Internal Call";
			this.miBreakUnresolvedInternalCall.Click += new System.EventHandler(this.OnOptionMenuItemClick);
			// 
			// miShowReport
			// 
			this.miShowReport.Checked = true;
			this.miShowReport.CheckOnClick = true;
			this.miShowReport.CheckState = System.Windows.Forms.CheckState.Checked;
			this.miShowReport.Name = "miShowReport";
			this.miShowReport.Size = new System.Drawing.Size(216, 22);
			this.miShowReport.Tag = "ShowReport";
			this.miShowReport.Text = "Show Report";
			this.miShowReport.Click += new System.EventHandler(this.OnOptionMenuItemClick);
			// 
			// miPDBTestMode
			// 
			this.miPDBTestMode.CheckOnClick = true;
			this.miPDBTestMode.Name = "miPDBTestMode";
			this.miPDBTestMode.Size = new System.Drawing.Size(216, 22);
			this.miPDBTestMode.Text = "PDB Test Mode";
			this.miPDBTestMode.Click += new System.EventHandler(this.OnOptionMenuItemClick);
			// 
			// cbAvmShellMode
			// 
			this.cbAvmShellMode.AutoSize = false;
			this.cbAvmShellMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbAvmShellMode.Items.AddRange(new object[] {
            "Interpretation",
            "JIT"});
			this.cbAvmShellMode.Name = "cbAvmShellMode";
			this.cbAvmShellMode.Size = new System.Drawing.Size(100, 23);
			this.cbAvmShellMode.SelectedIndexChanged += new System.EventHandler(this.cbAvmShellMode_SelectedIndexChanged);
			// 
			// btnResetDebugHooks
			// 
			this.btnResetDebugHooks.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnResetDebugHooks.Image = ((System.Drawing.Image)(resources.GetObject("btnResetDebugHooks.Image")));
			this.btnResetDebugHooks.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnResetDebugHooks.Name = "btnResetDebugHooks";
			this.btnResetDebugHooks.Size = new System.Drawing.Size(23, 22);
			this.btnResetDebugHooks.Text = "Reset Debug Hooks";
			this.btnResetDebugHooks.Click += new System.EventHandler(this.OnResetDebugHooks);
			// 
			// btnResetLastError
			// 
			this.btnResetLastError.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnResetLastError.Image = ((System.Drawing.Image)(resources.GetObject("btnResetLastError.Image")));
			this.btnResetLastError.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnResetLastError.Name = "btnResetLastError";
			this.btnResetLastError.Size = new System.Drawing.Size(23, 22);
			this.btnResetLastError.Text = "Reset Last Error";
			this.btnResetLastError.Click += new System.EventHandler(this.btnResetLastError_Click);
			// 
			// btnLogSwitch
			// 
			this.btnLogSwitch.Checked = true;
			this.btnLogSwitch.CheckOnClick = true;
			this.btnLogSwitch.CheckState = System.Windows.Forms.CheckState.Checked;
			this.btnLogSwitch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnLogSwitch.Image = ((System.Drawing.Image)(resources.GetObject("btnLogSwitch.Image")));
			this.btnLogSwitch.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnLogSwitch.Name = "btnLogSwitch";
			this.btnLogSwitch.Size = new System.Drawing.Size(23, 22);
			this.btnLogSwitch.Tag = "LogSwitch";
			this.btnLogSwitch.Text = "Log Switch";
			// 
			// cbTypeName
			// 
			this.cbTypeName.Name = "cbTypeName";
			this.cbTypeName.Size = new System.Drawing.Size(221, 25);
			this.cbTypeName.ToolTipText = "Full Type Name";
			// 
			// cbMethodName
			// 
			this.cbMethodName.Name = "cbMethodName";
			this.cbMethodName.Size = new System.Drawing.Size(121, 25);
			this.cbMethodName.ToolTipText = "Method Name";
			// 
			// tbPhase
			// 
			this.tbPhase.AutoSize = false;
			this.tbPhase.Name = "tbPhase";
			this.tbPhase.Size = new System.Drawing.Size(50, 25);
			// 
			// btnNUnitSession
			// 
			this.btnNUnitSession.CheckOnClick = true;
			this.btnNUnitSession.Image = ((System.Drawing.Image)(resources.GetObject("btnNUnitSession.Image")));
			this.btnNUnitSession.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnNUnitSession.Name = "btnNUnitSession";
			this.btnNUnitSession.Size = new System.Drawing.Size(100, 20);
			this.btnNUnitSession.Text = "NUnit Session";
			this.btnNUnitSession.Click += new System.EventHandler(this.btnNUnitSession_Click);
			// 
			// worker
			// 
			this.worker.WorkerReportsProgress = true;
			this.worker.WorkerSupportsCancellation = true;
			this.worker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Process);
			this.worker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.OnProcessCompleted);
			this.worker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.OnProgressChanged);
			// 
			// splitMain
			// 
			this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitMain.Location = new System.Drawing.Point(0, 0);
			this.splitMain.Name = "splitMain";
			// 
			// splitMain.Panel1
			// 
			this.splitMain.Panel1.Controls.Add(this.testCases);
			// 
			// splitMain.Panel2
			// 
			this.splitMain.Panel2.Controls.Add(this.tabsRight);
			this.splitMain.Size = new System.Drawing.Size(892, 431);
			this.splitMain.SplitterDistance = 220;
			this.splitMain.TabIndex = 0;
			// 
			// testCases
			// 
			this.testCases.CheckBoxes = true;
			this.testCases.Dock = System.Windows.Forms.DockStyle.Fill;
			this.testCases.HideSelection = false;
			this.testCases.Location = new System.Drawing.Point(0, 0);
			this.testCases.Name = "testCases";
			this.testCases.Size = new System.Drawing.Size(220, 431);
			this.testCases.TabIndex = 0;
			this.testCases.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.testCases_AfterCheck);
			this.testCases.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.testCases_AfterCollapse);
			this.testCases.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.testCases_AfterSelect);
			this.testCases.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.testCases_AfterExpand);
			// 
			// tabsRight
			// 
			this.tabsRight.Controls.Add(this.tabSourceCode);
			this.tabsRight.Controls.Add(this.tabDecompiledCode);
			this.tabsRight.Controls.Add(this.tabOutput1);
			this.tabsRight.Controls.Add(this.tabOutput2);
			this.tabsRight.Controls.Add(this.tabError);
			this.tabsRight.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabsRight.Location = new System.Drawing.Point(0, 0);
			this.tabsRight.Name = "tabsRight";
			this.tabsRight.SelectedIndex = 0;
			this.tabsRight.Size = new System.Drawing.Size(668, 431);
			this.tabsRight.TabIndex = 0;
			// 
			// tabSourceCode
			// 
			this.tabSourceCode.Controls.Add(this.sourceFiles);
			this.tabSourceCode.Location = new System.Drawing.Point(4, 22);
			this.tabSourceCode.Name = "tabSourceCode";
			this.tabSourceCode.Padding = new System.Windows.Forms.Padding(3);
			this.tabSourceCode.Size = new System.Drawing.Size(660, 405);
			this.tabSourceCode.TabIndex = 0;
			this.tabSourceCode.Text = "Source Code";
			this.tabSourceCode.UseVisualStyleBackColor = true;
			// 
			// sourceFiles
			// 
			this.sourceFiles.Dock = System.Windows.Forms.DockStyle.Fill;
			this.sourceFiles.Location = new System.Drawing.Point(3, 3);
			this.sourceFiles.Name = "sourceFiles";
			this.sourceFiles.SelectedIndex = 0;
			this.sourceFiles.Size = new System.Drawing.Size(654, 399);
			this.sourceFiles.TabIndex = 0;
			// 
			// tabDecompiledCode
			// 
			this.tabDecompiledCode.Controls.Add(this.editDecompiledCode);
			this.tabDecompiledCode.Location = new System.Drawing.Point(4, 22);
			this.tabDecompiledCode.Name = "tabDecompiledCode";
			this.tabDecompiledCode.Padding = new System.Windows.Forms.Padding(3);
			this.tabDecompiledCode.Size = new System.Drawing.Size(660, 405);
			this.tabDecompiledCode.TabIndex = 2;
			this.tabDecompiledCode.Text = "Decompiled Code";
			this.tabDecompiledCode.UseVisualStyleBackColor = true;
			// 
			// editDecompiledCode
			// 
			this.editDecompiledCode.ActiveView = Fireball.Windows.Forms.CodeEditor.ActiveView.BottomRight;
			this.editDecompiledCode.AutoListPosition = null;
			this.editDecompiledCode.AutoListSelectedText = "a123";
			this.editDecompiledCode.AutoListVisible = false;
			this.editDecompiledCode.CopyAsRTF = true;
			this.editDecompiledCode.Dock = System.Windows.Forms.DockStyle.Fill;
			this.editDecompiledCode.Document = this.docDecompiledCode;
			this.editDecompiledCode.InfoTipCount = 1;
			this.editDecompiledCode.InfoTipPosition = null;
			this.editDecompiledCode.InfoTipSelectedIndex = 1;
			this.editDecompiledCode.InfoTipVisible = false;
			lineMarginRender3.Bounds = new System.Drawing.Rectangle(19, 0, 19, 16);
			this.editDecompiledCode.LineMarginRender = lineMarginRender3;
			this.editDecompiledCode.Location = new System.Drawing.Point(3, 3);
			this.editDecompiledCode.LockCursorUpdate = false;
			this.editDecompiledCode.Name = "editDecompiledCode";
			this.editDecompiledCode.Saved = false;
			this.editDecompiledCode.ShowScopeIndicator = false;
			this.editDecompiledCode.Size = new System.Drawing.Size(654, 399);
			this.editDecompiledCode.SmoothScroll = false;
			this.editDecompiledCode.SplitView = false;
			this.editDecompiledCode.SplitviewH = -4;
			this.editDecompiledCode.SplitviewV = -4;
			this.editDecompiledCode.TabGuideColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
			this.editDecompiledCode.TabIndex = 0;
			this.editDecompiledCode.Text = "codeEditorControl1";
			this.editDecompiledCode.WhitespaceColor = System.Drawing.SystemColors.ControlDark;
			// 
			// docDecompiledCode
			// 
			this.docDecompiledCode.Lines = new string[] {
        ""};
			this.docDecompiledCode.MaxUndoBufferSize = 1000;
			this.docDecompiledCode.Modified = false;
			this.docDecompiledCode.UndoStep = 0;
			// 
			// tabOutput1
			// 
			this.tabOutput1.Controls.Add(this.editOutput1);
			this.tabOutput1.Location = new System.Drawing.Point(4, 22);
			this.tabOutput1.Name = "tabOutput1";
			this.tabOutput1.Padding = new System.Windows.Forms.Padding(3);
			this.tabOutput1.Size = new System.Drawing.Size(660, 405);
			this.tabOutput1.TabIndex = 4;
			this.tabOutput1.Text = "Output1";
			this.tabOutput1.UseVisualStyleBackColor = true;
			// 
			// editOutput1
			// 
			this.editOutput1.ActiveView = Fireball.Windows.Forms.CodeEditor.ActiveView.BottomRight;
			this.editOutput1.AutoListPosition = null;
			this.editOutput1.AutoListSelectedText = "a123";
			this.editOutput1.AutoListVisible = false;
			this.editOutput1.CopyAsRTF = false;
			this.editOutput1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.editOutput1.Document = this.docOutput1;
			this.editOutput1.InfoTipCount = 1;
			this.editOutput1.InfoTipPosition = null;
			this.editOutput1.InfoTipSelectedIndex = 1;
			this.editOutput1.InfoTipVisible = false;
			lineMarginRender1.Bounds = new System.Drawing.Rectangle(19, 0, 19, 16);
			this.editOutput1.LineMarginRender = lineMarginRender1;
			this.editOutput1.Location = new System.Drawing.Point(3, 3);
			this.editOutput1.LockCursorUpdate = false;
			this.editOutput1.Name = "editOutput1";
			this.editOutput1.Saved = false;
			this.editOutput1.ShowScopeIndicator = false;
			this.editOutput1.Size = new System.Drawing.Size(654, 399);
			this.editOutput1.SmoothScroll = false;
			this.editOutput1.SplitView = false;
			this.editOutput1.SplitviewH = -4;
			this.editOutput1.SplitviewV = -4;
			this.editOutput1.TabGuideColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
			this.editOutput1.TabIndex = 0;
			this.editOutput1.Text = "codeEditorControl1";
			this.editOutput1.WhitespaceColor = System.Drawing.SystemColors.ControlDark;
			// 
			// docOutput1
			// 
			this.docOutput1.Lines = new string[] {
        ""};
			this.docOutput1.MaxUndoBufferSize = 1000;
			this.docOutput1.Modified = false;
			this.docOutput1.UndoStep = 0;
			// 
			// tabOutput2
			// 
			this.tabOutput2.Controls.Add(this.editOutput2);
			this.tabOutput2.Location = new System.Drawing.Point(4, 22);
			this.tabOutput2.Name = "tabOutput2";
			this.tabOutput2.Padding = new System.Windows.Forms.Padding(3);
			this.tabOutput2.Size = new System.Drawing.Size(660, 405);
			this.tabOutput2.TabIndex = 5;
			this.tabOutput2.Text = "Output2";
			this.tabOutput2.UseVisualStyleBackColor = true;
			// 
			// editOutput2
			// 
			this.editOutput2.ActiveView = Fireball.Windows.Forms.CodeEditor.ActiveView.BottomRight;
			this.editOutput2.AutoListPosition = null;
			this.editOutput2.AutoListSelectedText = "a123";
			this.editOutput2.AutoListVisible = false;
			this.editOutput2.CopyAsRTF = false;
			this.editOutput2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.editOutput2.Document = this.docOutput2;
			this.editOutput2.InfoTipCount = 1;
			this.editOutput2.InfoTipPosition = null;
			this.editOutput2.InfoTipSelectedIndex = 1;
			this.editOutput2.InfoTipVisible = false;
			this.editOutput2.LineMarginRender = lineMarginRender1;
			this.editOutput2.Location = new System.Drawing.Point(3, 3);
			this.editOutput2.LockCursorUpdate = false;
			this.editOutput2.Name = "editOutput2";
			this.editOutput2.Saved = false;
			this.editOutput2.ShowScopeIndicator = false;
			this.editOutput2.Size = new System.Drawing.Size(654, 399);
			this.editOutput2.SmoothScroll = false;
			this.editOutput2.SplitView = false;
			this.editOutput2.SplitviewH = -4;
			this.editOutput2.SplitviewV = -4;
			this.editOutput2.TabGuideColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
			this.editOutput2.TabIndex = 0;
			this.editOutput2.Text = "codeEditorControl1";
			this.editOutput2.WhitespaceColor = System.Drawing.SystemColors.ControlDark;
			// 
			// docOutput2
			// 
			this.docOutput2.Lines = new string[] {
        ""};
			this.docOutput2.MaxUndoBufferSize = 1000;
			this.docOutput2.Modified = false;
			this.docOutput2.UndoStep = 0;
			// 
			// tabError
			// 
			this.tabError.Controls.Add(this.editError);
			this.tabError.Location = new System.Drawing.Point(4, 22);
			this.tabError.Name = "tabError";
			this.tabError.Padding = new System.Windows.Forms.Padding(3);
			this.tabError.Size = new System.Drawing.Size(660, 405);
			this.tabError.TabIndex = 1;
			this.tabError.Text = "Error";
			this.tabError.UseVisualStyleBackColor = true;
			// 
			// editError
			// 
			this.editError.ActiveView = Fireball.Windows.Forms.CodeEditor.ActiveView.BottomRight;
			this.editError.AutoListPosition = null;
			this.editError.AutoListSelectedText = "a123";
			this.editError.AutoListVisible = false;
			this.editError.CopyAsRTF = false;
			this.editError.Dock = System.Windows.Forms.DockStyle.Fill;
			this.editError.Document = this.docError;
			this.editError.InfoTipCount = 1;
			this.editError.InfoTipPosition = null;
			this.editError.InfoTipSelectedIndex = 1;
			this.editError.InfoTipVisible = false;
			this.editError.LineMarginRender = lineMarginRender1;
			this.editError.Location = new System.Drawing.Point(3, 3);
			this.editError.LockCursorUpdate = false;
			this.editError.Name = "editError";
			this.editError.Saved = false;
			this.editError.ShowScopeIndicator = false;
			this.editError.Size = new System.Drawing.Size(654, 399);
			this.editError.SmoothScroll = false;
			this.editError.SplitView = false;
			this.editError.SplitviewH = -4;
			this.editError.SplitviewV = -4;
			this.editError.TabGuideColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
			this.editError.TabIndex = 0;
			this.editError.Text = "codeEditorControl2";
			this.editError.WhitespaceColor = System.Drawing.SystemColors.ControlDark;
			// 
			// docError
			// 
			this.docError.Lines = new string[] {
        ""};
			this.docError.MaxUndoBufferSize = 1000;
			this.docError.Modified = false;
			this.docError.UndoStep = 0;
			// 
			// docAvmDump
			// 
			this.docAvmDump.Lines = new string[] {
        ""};
			this.docAvmDump.MaxUndoBufferSize = 1000;
			this.docAvmDump.Modified = false;
			this.docAvmDump.UndoStep = 0;
			// 
			// docAbcDump
			// 
			this.docAbcDump.Lines = new string[] {
        ""};
			this.docAbcDump.MaxUndoBufferSize = 1000;
			this.docAbcDump.Modified = false;
			this.docAbcDump.UndoStep = 0;
			// 
			// toolStripContainer1
			// 
			// 
			// toolStripContainer1.BottomToolStripPanel
			// 
			this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.statusbar);
			// 
			// toolStripContainer1.ContentPanel
			// 
			this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainer1);
			this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(892, 695);
			this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
			this.toolStripContainer1.Name = "toolStripContainer1";
			this.toolStripContainer1.Size = new System.Drawing.Size(892, 766);
			this.toolStripContainer1.TabIndex = 1;
			this.toolStripContainer1.Text = "toolStripContainer1";
			// 
			// toolStripContainer1.TopToolStripPanel
			// 
			this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.menubar);
			this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolbar);
			// 
			// statusbar
			// 
			this.statusbar.Dock = System.Windows.Forms.DockStyle.None;
			this.statusbar.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
			this.statusbar.Location = new System.Drawing.Point(0, 0);
			this.statusbar.Name = "statusbar";
			this.statusbar.Size = new System.Drawing.Size(892, 22);
			this.statusbar.TabIndex = 0;
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.splitMain);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.btabs);
			this.splitContainer1.Size = new System.Drawing.Size(892, 695);
			this.splitContainer1.SplitterDistance = 431;
			this.splitContainer1.TabIndex = 1;
			// 
			// btabs
			// 
			this.btabs.Controls.Add(this.tabPage2);
			this.btabs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btabs.Location = new System.Drawing.Point(0, 0);
			this.btabs.Name = "btabs";
			this.btabs.SelectedIndex = 0;
			this.btabs.Size = new System.Drawing.Size(892, 260);
			this.btabs.TabIndex = 0;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.tbLog);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(884, 234);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Log";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// tbLog
			// 
			this.tbLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tbLog.Location = new System.Drawing.Point(3, 3);
			this.tbLog.Name = "tbLog";
			this.tbLog.Size = new System.Drawing.Size(878, 228);
			this.tbLog.TabIndex = 0;
			this.tbLog.Text = "";
			// 
			// menubar
			// 
			this.menubar.Dock = System.Windows.Forms.DockStyle.None;
			this.menubar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem});
			this.menubar.Location = new System.Drawing.Point(0, 0);
			this.menubar.Name = "menubar";
			this.menubar.Size = new System.Drawing.Size(892, 24);
			this.menubar.TabIndex = 1;
			this.menubar.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miGenerateHtmlReport,
            this.miGenerateWikiReport,
            this.miExit});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// miGenerateHtmlReport
			// 
			this.miGenerateHtmlReport.Name = "miGenerateHtmlReport";
			this.miGenerateHtmlReport.Size = new System.Drawing.Size(195, 22);
			this.miGenerateHtmlReport.Text = "Generate HTML Report";
			this.miGenerateHtmlReport.Click += new System.EventHandler(this.miGenerateHtmlReport_Click);
			// 
			// miGenerateWikiReport
			// 
			this.miGenerateWikiReport.Name = "miGenerateWikiReport";
			this.miGenerateWikiReport.Size = new System.Drawing.Size(195, 22);
			this.miGenerateWikiReport.Text = "Generate Wiki Report";
			this.miGenerateWikiReport.Click += new System.EventHandler(this.miGenerateWikiReport_Click);
			// 
			// miExit
			// 
			this.miExit.Name = "miExit";
			this.miExit.Size = new System.Drawing.Size(195, 22);
			this.miExit.Text = "E&xit";
			this.miExit.Click += new System.EventHandler(this.miExit_Click);
			// 
			// toolsToolStripMenuItem
			// 
			this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miRun,
            this.miRunAll,
            this.miStop,
            this.miCleanQADir,
            this.miCompareTextFiles,
            this.miGenerateNUnitTests,
            this.miGenerateSelectedNUnitTests,
            this.miOptions});
			this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
			this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
			this.toolsToolStripMenuItem.Text = "&Tools";
			// 
			// miRun
			// 
			this.miRun.Name = "miRun";
			this.miRun.ShortcutKeys = System.Windows.Forms.Keys.F5;
			this.miRun.Size = new System.Drawing.Size(232, 22);
			this.miRun.Text = "Run";
			this.miRun.Click += new System.EventHandler(this.btnRun_Click);
			// 
			// miRunAll
			// 
			this.miRunAll.Name = "miRunAll";
			this.miRunAll.Size = new System.Drawing.Size(232, 22);
			this.miRunAll.Text = "Run &all";
			this.miRunAll.Click += new System.EventHandler(this.btnRunAll_Click);
			// 
			// miStop
			// 
			this.miStop.Name = "miStop";
			this.miStop.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F5)));
			this.miStop.Size = new System.Drawing.Size(232, 22);
			this.miStop.Text = "&Stop";
			this.miStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// miCleanQADir
			// 
			this.miCleanQADir.Name = "miCleanQADir";
			this.miCleanQADir.Size = new System.Drawing.Size(232, 22);
			this.miCleanQADir.Text = "Clean QA Directory";
			this.miCleanQADir.Click += new System.EventHandler(this.miCleanQADir_Click);
			// 
			// miCompareTextFiles
			// 
			this.miCompareTextFiles.Name = "miCompareTextFiles";
			this.miCompareTextFiles.Size = new System.Drawing.Size(232, 22);
			this.miCompareTextFiles.Text = "Compare Text Files";
			this.miCompareTextFiles.Click += new System.EventHandler(this.miCompareTextFiles_Click);
			// 
			// miGenerateNUnitTests
			// 
			this.miGenerateNUnitTests.Name = "miGenerateNUnitTests";
			this.miGenerateNUnitTests.Size = new System.Drawing.Size(232, 22);
			this.miGenerateNUnitTests.Text = "Generate All NUnit Tests";
			this.miGenerateNUnitTests.Click += new System.EventHandler(this.miGenerateAllNUnitTests_Click);
			// 
			// miGenerateSelectedNUnitTests
			// 
			this.miGenerateSelectedNUnitTests.Name = "miGenerateSelectedNUnitTests";
			this.miGenerateSelectedNUnitTests.Size = new System.Drawing.Size(232, 22);
			this.miGenerateSelectedNUnitTests.Text = "Generate Selected NUnit Tests";
			// 
			// miOptions
			// 
			this.miOptions.Name = "miOptions";
			this.miOptions.Size = new System.Drawing.Size(232, 22);
			this.miOptions.Text = "Options...";
			this.miOptions.Click += new System.EventHandler(this.miOptions_Click);
			// 
			// docOutput
			// 
			this.docOutput.Lines = new string[] {
        ""};
			this.docOutput.MaxUndoBufferSize = 1000;
			this.docOutput.Modified = false;
			this.docOutput.UndoStep = 0;
			// 
			// miClrEmulation
			// 
			this.miClrEmulation.Name = "miClrEmulation";
			this.miClrEmulation.Size = new System.Drawing.Size(170, 22);
			this.miClrEmulation.Text = "Interpretation";
			this.miClrEmulation.Click += new System.EventHandler(this.OnSelectTestDriver);
			// 
			// QAForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(892, 766);
			this.Controls.Add(this.toolStripContainer1);
			this.MainMenuStrip = this.menubar;
			this.Name = "QAForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "PageFX Test Runner";
			this.Load += new System.EventHandler(this.QAForm_Load);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.QAForm_FormClosed);
			this.toolbar.ResumeLayout(false);
			this.toolbar.PerformLayout();
			this.splitMain.Panel1.ResumeLayout(false);
			this.splitMain.Panel2.ResumeLayout(false);
			this.splitMain.ResumeLayout(false);
			this.tabsRight.ResumeLayout(false);
			this.tabSourceCode.ResumeLayout(false);
			this.tabDecompiledCode.ResumeLayout(false);
			this.tabOutput1.ResumeLayout(false);
			this.tabOutput2.ResumeLayout(false);
			this.tabError.ResumeLayout(false);
			this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
			this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
			this.toolStripContainer1.ContentPanel.ResumeLayout(false);
			this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
			this.toolStripContainer1.TopToolStripPanel.PerformLayout();
			this.toolStripContainer1.ResumeLayout(false);
			this.toolStripContainer1.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.btabs.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.menubar.ResumeLayout(false);
			this.menubar.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolbar;
        private System.Windows.Forms.ToolStripButton btnRun;
        private System.ComponentModel.BackgroundWorker worker;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.TreeView testCases;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.StatusStrip statusbar;
        private System.Windows.Forms.TabControl tabsRight;
        private System.Windows.Forms.TabPage tabSourceCode;
        private System.Windows.Forms.TabPage tabError;
        private System.Windows.Forms.ToolStripButton btnRunAll;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem miAbcSerialization;
        private System.Windows.Forms.ToolStripMenuItem miCliDeserialization;
        private System.Windows.Forms.TabPage tabDecompiledCode;
        private System.Windows.Forms.ToolStripButton btnStop;
        private System.Windows.Forms.TabPage tabOutput1;
        private System.Windows.Forms.TabPage tabOutput2;
        private System.Windows.Forms.ToolStripDropDownButton btnOptions;
        private System.Windows.Forms.ToolStripMenuItem miDumpILCode;
        private System.Windows.Forms.ToolStripMenuItem miGraphViz;
        private System.Windows.Forms.ToolStripMenuItem miGraphVizStruct;
        private System.Windows.Forms.TabControl sourceFiles;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl btabs;
        private System.Windows.Forms.ToolStripMenuItem miAvmDump;
        private System.Windows.Forms.ToolStripMenuItem miAbcDumpOptions;
        private System.Windows.Forms.MenuStrip menubar;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem miRun;
        private System.Windows.Forms.ToolStripMenuItem miStop;
        private System.Windows.Forms.ToolStripMenuItem miExit;
        private System.Windows.Forms.ToolStripMenuItem miRunAll;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.RichTextBox tbLog;
        private System.Windows.Forms.ToolStripButton btnSaveSettings;
        private System.Windows.Forms.ToolStripComboBox cbTypeName;
        private System.Windows.Forms.ToolStripComboBox cbMethodName;
        private System.Windows.Forms.ToolStripMenuItem miDebuggerBreak;
        private System.Windows.Forms.ToolStripTextBox tbPhase;
        private System.Windows.Forms.ToolStripMenuItem miDumpSrc;
        private System.Windows.Forms.ToolStripButton btnResetDebugHooks;
        private Fireball.Windows.Forms.CodeEditorControl editDecompiledCode;
        private Fireball.Syntax.SyntaxDocument docDecompiledCode;
        private Fireball.Windows.Forms.CodeEditorControl editOutput1;
        private Fireball.Syntax.SyntaxDocument docOutput1;
        private Fireball.Windows.Forms.CodeEditorControl editOutput2;
        private Fireball.Syntax.SyntaxDocument docOutput2;
        private Fireball.Windows.Forms.CodeEditorControl editError;
        private Fireball.Syntax.SyntaxDocument docError;
        private Fireball.Syntax.SyntaxDocument docAvmDump;
        private Fireball.Syntax.SyntaxDocument docAbcDump;
        private System.Windows.Forms.ToolStripMenuItem miAbcDumpTextFormat;
        private System.Windows.Forms.ToolStripMenuItem miAbcDumpXmlFormat;
        private System.Windows.Forms.ToolStripMenuItem miAbcDumpCode;
        private System.Windows.Forms.ToolStripMenuItem miAbcDumpClassList;
        private System.Windows.Forms.ToolStripMenuItem miAbcDumpEnabled;
        private System.Windows.Forms.ToolStripButton btnLogSwitch;
        private System.Windows.Forms.ToolStripMenuItem miRecordLastError;
        private System.Windows.Forms.ToolStripMenuItem miBreaks;
        private System.Windows.Forms.ToolStripMenuItem miBreakWhileLoops;
        private System.Windows.Forms.ToolStripMenuItem miBreakDoWhileLoops;
        private System.Windows.Forms.ToolStripMenuItem miBreakEndlessLoops;
        private System.Windows.Forms.ToolStripMenuItem miDumpILMap;
        private System.Windows.Forms.ToolStripButton btnResetLastError;
        private System.Windows.Forms.ToolStripButton btnCompile;
        private Fireball.Syntax.SyntaxDocument docOutput;
        private System.Windows.Forms.ToolStripMenuItem miCleanQADir;
        private System.Windows.Forms.ToolStripMenuItem miAbcDumpTraits;
        private System.Windows.Forms.ToolStripMenuItem miShowReport;
        private System.Windows.Forms.ToolStripMenuItem miBreakInvalidMetadataToken;
        private System.Windows.Forms.ToolStripMenuItem miBreakInvalidMemberReference;
        private System.Windows.Forms.ToolStripMenuItem miBreakInvalidTypeReference;
        private System.Windows.Forms.ToolStripMenuItem miBreakUnresolvedInternalCall;
        private System.Windows.Forms.ToolStripMenuItem miOptions;
        private System.Windows.Forms.ToolStripMenuItem miGenerateWikiReport;
        private System.Windows.Forms.ToolStripMenuItem miGenerateHtmlReport;
        private System.Windows.Forms.ToolStripMenuItem miCompareTextFiles;
        private System.Windows.Forms.ToolStripMenuItem miSwfSerialization;
        private System.Windows.Forms.ToolStripMenuItem miCompilerOptions;
        private System.Windows.Forms.ToolStripMenuItem miOptimizeCode;
        private System.Windows.Forms.ToolStripMenuItem miEmitDebugInfo;
        private System.Windows.Forms.ToolStripButton btnSelectFailed;
        private System.Windows.Forms.ToolStripButton btnCopy;
        private System.Windows.Forms.ToolStripMenuItem miUseCommonDirectory;
        private System.Windows.Forms.ToolStripMenuItem miGenerateNUnitTests;
        private System.Windows.Forms.ToolStripMenuItem miGenerateSelectedNUnitTests;
        private System.Windows.Forms.ToolStripComboBox cbAvmShellMode;
        private System.Windows.Forms.ToolStripMenuItem miPDBTestMode;
        private System.Windows.Forms.ToolStripButton btnNUnitSession;
		private System.Windows.Forms.ToolStripMenuItem miClrEmulation;
    }
}