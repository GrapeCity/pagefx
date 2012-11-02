using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using DataDynamics.PageFX.FLI;
using DataDynamics.PageFX.FLI.ABC;
using DataDynamics.PageFX.UI;
using DataDynamics.UI;
using Fireball.CodeEditor.SyntaxFiles;
using Fireball.Syntax;
using Fireball.Windows.Forms;

namespace DataDynamics.PageFX
{
    public partial class QAForm : Form
    {
        public QAForm()
        {
            InitializeComponent();

            CodeEditorSyntaxLoader.SetSyntax(editDecompiledCode, SyntaxLanguage.CSharp);
            CodeEditorSyntaxLoader.SetSyntax(editOutput1, SyntaxLanguage.Text);
            CodeEditorSyntaxLoader.SetSyntax(editOutput2, SyntaxLanguage.Text);
            CodeEditorSyntaxLoader.SetSyntax(editError, SyntaxLanguage.Text);
            
            InitCodeEditor(editDecompiledCode);
            InitCodeEditor(editOutput1);
            InitCodeEditor(editOutput2);
            InitCodeEditor(editError);

            miPDBTestMode.Tag = KeyTestDebugSupport;
            cbAvmShellMode.SelectedIndex = 0;
            
            _cancelCallback = (() => worker.CancellationPending);

            _twLog = new TextBoxWriter(tbLog);

#if DEBUG
            DebugService.CancelCallback = _cancelCallback;
            DebugHooks.CancelCallback = _cancelCallback;
#endif
            miOptimizeCode.Tag = GlobalOptionName.OptimizeCode;
            miEmitDebugInfo.Tag = GlobalOptionName.EmitDebugInfo;
            miUseCommonDirectory.Tag = GlobalOptionName.UseCommonDirectory;
        }

        void OnOptionMenuItemClick(object sender, EventArgs e)
        {
            var mi = sender as ToolStripMenuItem;
            if (mi != null)
            {
                var tag = mi.Tag;
                if (tag is GlobalOptionName)
                {
                    var name = (GlobalOptionName)tag;
                    QA.SetOption(name, mi.Checked);
                }
                else
                {
                    ApplySettings();
                }
            }
        }

        readonly Func<bool> _cancelCallback;

        static void InitCodeEditor(CodeEditorControl edit)
        {
            edit.CopyAsRTF = true;
            edit.HighLightActiveLine = true;
            edit.HighLightedLineColor = Color.FromArgb(190, 250, 205);
            edit.SplitView = false;
            edit.ShowGutterMargin = false;
        }

        readonly TextWriter _twLog;

        TestCase SelectedTestCase
        {
            get
            {
                var node = testCases.SelectedNode;
                if (node == null) return null;
                return node.Tag as TestCase;
            }
        }

        void QAForm_Load(object sender, EventArgs e)
        {
            miAbcSerialization.Tag = TestDriver.AbcSerialization;
            miSwfSerialization.Tag = TestDriver.SwfSerialization;
            miCliDeserialization.Tag = TestDriver.CliDeserialization;
            miClrEmulation.Tag = TestDriver.ClrEmulation;
            miJavaScript.Tag = TestDriver.JavaScript;

            //progressBar.Visible = false;

            AddTestCases();
            UpdateNodeTitles(testCases.Nodes);

            LoadState();

            //GenerateNUnitTestFixtures();

            Application.Idle += Application_Idle;
        }

        static void UpdateNodeTitles(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                var ts = node.Tag as TestSuite;
                if (ts != null)
                {
                    if (ts.TotalCount > 0)
                    {
                        node.Text = node.Text + string.Format(" [{0}]", ts.TotalCount);
                    }
                    UpdateNodeTitles(node.Nodes);
                }
            }
        }

        void QAForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            SaveState();

            Application.Idle -= Application_Idle;
        }

        enum TestDriver
        {
			[String("ABC")]
            AbcSerialization,
			[String("SWF")]
            SwfSerialization,
			[String("CLI")]
            CliDeserialization,
			[String("CLRE")]
			ClrEmulation,
			[String("JS")]
			JavaScript,
        }
        TestDriver _testDriver;

        void Application_Idle(object sender, EventArgs e)
        {
            bool isBusy = worker.IsBusy;
            btnRun.Enabled = miRun.Enabled = !isBusy;
            btnRunAll.Enabled = miRunAll.Enabled = !isBusy;
            btnStop.Enabled = miStop.Enabled = isBusy;

            miAbcSerialization.Checked = _testDriver == TestDriver.AbcSerialization;
            miSwfSerialization.Checked = _testDriver == TestDriver.SwfSerialization;
            miCliDeserialization.Checked = _testDriver == TestDriver.CliDeserialization;
            miClrEmulation.Checked = _testDriver == TestDriver.ClrEmulation;
            miJavaScript.Checked = _testDriver == TestDriver.JavaScript;
        }

        static TreeNode FindByText(TreeNodeCollection list, string text)
        {
        	return list.Cast<TreeNode>().FirstOrDefault(node => node.Text == text);
        }

        static TreeNode FindByPath(TreeNodeCollection list, string path)
        {
            var names = path.Split('\\', '/');
            TreeNode node = null;
            var parent = list;
            foreach (string name in names)
            {
            	node = FindByText(parent, name);
            	if (node == null) return null;
            	parent = node.Nodes;
            }
            return node;
        }

    	readonly Hashtable _nodeCache = new Hashtable();

        void AddTestCases(TestSuite ts, TreeNode tsnode)
        {
            foreach (var tc in ts.Cases)
            {
                var node = new TreeNode(tc.Name) {Tag = tc};
                _nodeCache[tc.FullName] = node;
                tsnode.Nodes.Add(node);
            }
        }

        void AddTestSuite(TestSuite ts, TreeNode parent)
        {
            if (QA.SortTests)
                ts.Sort();

            if (!ts.IsRoot)
            {
                var node = new TreeNode(ts.Name) {Tag = ts};

                if (parent != null)
                    parent.Nodes.Add(node);
                else
                    testCases.Nodes.Add(node);

                AddTestCases(ts, node);

                parent = node;
            }

            foreach (var kid in ts.ChildSuites)
                AddTestSuite(kid, parent);
        }

        void AddTestCases()
        {
            //Ensure test cases
            var l = SimpleTestCases.All;
            AddTestSuite(TestSuite.Root, null);

            var root = testCases.Nodes[0];
            root.Expand();
            foreach (TreeNode node in root.Nodes)
                node.Expand();
        }

        static SyntaxLanguage GetSyntaxLanguage(TestCase tc)
        {
            switch (tc.Language)
            {
                case CompilerLanguage.CSharp:
                    return SyntaxLanguage.CSharp;
                case CompilerLanguage.VB:
                    return SyntaxLanguage.VB;
                case CompilerLanguage.JSharp:
                    return SyntaxLanguage.Java;
                case CompilerLanguage.CIL:
                    return SyntaxLanguage.MSIL;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        void OnSelectionChanged()
        {
            sourceFiles.TabPages.Clear();
            var tc = SelectedTestCase;
            if (tc != null)
            {
                sourceFiles.SuspendLayout();
                int n = tc.SourceFiles.Count;
                for (int i = 0; i < n; ++i)
                {
                    var sf = tc.SourceFiles[i];

                	var tab = new TabPage
                	          	{
                	          		Name = string.Format("srcPage{0}", i),
                	          		Padding = new Padding(3),
                	          		Size = new Size(646, 461),
                	          		TabIndex = 0,
                	          		Text = sf.Name,
                	          		UseVisualStyleBackColor = true
                	          	};

                	var edit = new CodeEditorControl {Dock = DockStyle.Fill};
                	InitCodeEditor(edit);

                    var doc = new SyntaxDocument();
                    edit.Document = doc;
                    CodeEditorSyntaxLoader.SetSyntax(edit, GetSyntaxLanguage(tc));
                    doc.Text = sf.Text;

                    tab.Controls.Add(edit);

                    sourceFiles.TabPages.Add(tab);
                }
                sourceFiles.ResumeLayout();

                docError.Text = tc.Error;
                docDecompiledCode.Text = tc.DecompiledCode;
                docOutput1.Text = tc.Output1;
                docOutput2.Text = tc.Output2;
            }
            else
            {
                docError.Text = "";
                docDecompiledCode.Text = "";
                docOutput1.Text = "";
                docOutput2.Text = "";
            }
        }

        void testCases_AfterSelect(object sender, TreeViewEventArgs e)
        {
            OnSelectionChanged();
        }

        void testCases_AfterCollapse(object sender, TreeViewEventArgs e)
        {

        }

        void testCases_AfterExpand(object sender, TreeViewEventArgs e)
        {

        }

        static IEnumerable<TreeNode> GetDescendants(TreeNode node)
        {
            foreach (TreeNode kid in node.Nodes)
            {
                if (kid.Nodes.Count > 0)
                {
                    yield return kid;
                    foreach (var d in GetDescendants(kid))
                    {
                        yield return d;
                    }
                }
                else
                {
                    yield return kid;
                }
            }
        }

        bool _checkHandling;

        void testCases_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (_checkHandling) return;

            bool isChecked = e.Node.Checked;

            if (!(e.Node.Tag is TestCase))
            {
                foreach (TreeNode d in e.Node.Nodes)
                {
                    d.Checked = isChecked;
                }
            }

            _checkHandling = true;

            var parent = e.Node.Parent;

            while (parent != null)
            {
                if (isChecked)
                {
                    bool isAll = parent.Nodes.Cast<TreeNode>().All(d => d.Checked);
                	if (isAll)
                    {
                        if (!parent.Checked)
                            parent.Checked = true;
                    }
                }
                else
                {
                    if (parent.Checked)
                        parent.Checked = false;
                }
                parent = parent.Parent;
            }

            _checkHandling = false;
        }

    	static IEnumerable<TreeNode> GetSelectedTestCases(TreeNode node)
        {
            var tc = node.Tag as TestCase;
            if (tc != null)
            {
                if (node.Checked)
                    yield return node;
            }
            else
            {
                foreach (TreeNode kid in node.Nodes)
                {
                    foreach (var c in GetSelectedTestCases(kid))
                    {
                        yield return c;
                    }
                }
            }
        }

        List<TreeNode> GetSelectedTestCases()
        {
        	return (from TreeNode node in testCases.Nodes
					from tc in GetSelectedTestCases(node)
					select tc).ToList();
        }

    	List<TreeNode> GetAllTestCases()
    	{
    		return (from TreeNode node in testCases.Nodes
					from d in GetDescendants(node)
					where d.Tag is TestCase select d).ToList();
    	}

    	void Run()
        {
            if (!worker.IsBusy)
            {
                var list = GetSelectedTestCases();
                if (list != null && list.Count > 0)
                {
                    SaveState();

                    if (QA.UseCommonDirectory)
                        QA.CopyNUnitTests();

                    worker.RunWorkerAsync(list);
                }
            }
        }

        void btnRun_Click(object sender, EventArgs e)
        {
            Run();
        }

        #region Process
        delegate void Action();

        void safe(Action a)
        {
            if (InvokeRequired)
            {
                var res = BeginInvoke(a);
                res.AsyncWaitHandle.WaitOne(1000, true);
                //EndInvoke(res);
                //Invoke(a);
            }
            else
            {
                a();
            }
        }

        TreeNode _currentNode;
        bool _blinkState;
        readonly bool _blinkEnabled;
        
        void BlinkCurrentNode()
        {
            while (true)
            {
                try
                {
                    if (_currentNode != null)
                    {
                        lock (_currentNode)
                        {
                            safe(delegate
                                     {
                                         var color = _blinkState ? Color.Lime : Color.Yellow;
                                         _blinkState = !_blinkState;
                                         if (_currentNode != null)
                                         {
                                             _currentNode.BackColor = color;
                                         }
                                     });
                        }
                        Thread.Sleep(500);
                    }
                }
                catch
                {
                    break;
                }
            }
        }

        void RunTestCase(TestCase tc)
        {
            tc.Optimize = QA.OptimizeCode;
            tc.Debug = QA.EmitDebugInfo;
            tc.IsStarted = true;
            var settings = new TestDriverSettings
                          {
                              ExportCSharpFile = false,
                              CancelCallback = _cancelCallback
                          };

        	switch (_testDriver)
        	{
        		case TestDriver.AbcSerialization:
					settings.OutputFormat = "abc";
					TestEngine.RunTestCase(tc, settings);
        			break;
        		case TestDriver.SwfSerialization:
					settings.OutputFormat = "swf";
					TestEngine.RunTestCase(tc, settings);
        			break;
				case TestDriver.JavaScript:
					settings.OutputFormat = "js";
					TestEngine.RunTestCase(tc, settings);
					break;
        		case TestDriver.CliDeserialization:
        			break;
        		case TestDriver.ClrEmulation:
					settings.IsClrEmulation = true;
					TestEngine.RunTestCase(tc, settings);
        			break;
				
        		default:
        			throw new ArgumentOutOfRangeException();
        	}

        	if (settings.IsCancel)
                tc.IsCancelled = true;
            else
                tc.IsFinished = true;
        }

        bool _hasErrors;
        bool _reportProgress;

        void Process(object sender, DoWorkEventArgs e)
        {
            var testCases = e.Argument as List<TreeNode>;
            e.Result = testCases;
            Process(testCases);
        }

        void Process(ICollection<TreeNode> testCases)
        {
            if (testCases == null) return;

            _hasErrors = false;

            safe(ApplySettings);

            Thread blinkThread = null;
            if (_blinkEnabled)
            {
                blinkThread = new Thread(BlinkCurrentNode) {IsBackground = true};
                blinkThread.Start();
            }

            int i = 0;

            if (_reportProgress)
                worker.ReportProgress(0, string.Format("Process Started..."));

            foreach (var node in testCases)
            {
                if (worker.CancellationPending) break;

                var tc = node.Tag as TestCase;
                if (tc != null)
                {
                    if (_reportProgress)
                    {
                        int p = (int)(i * 100.0 / testCases.Count);
                        worker.ReportProgress(p, string.Format("{0} Started...", tc.Name));
                    }
                    RunTestCase(node, tc);
                    if (worker.CancellationPending) break;
                    ++i;
                }
            }

            if (_reportProgress)
            {
                if (worker.CancellationPending)
                {
                    worker.ReportProgress(100, string.Format("Process Canceled..."));
                }
                else if (_hasErrors)
                {
                    worker.ReportProgress(100, "Process Failed");
                }
                else
                {
                    worker.ReportProgress(100, "Process Succeded");
                }
            }

            _currentNode = null;
            if (blinkThread != null)
                blinkThread.Abort();

            safe(() => ShowReport(testCases));
        }

        void ApplySettings()
        {
            //NOTE: Currently we are targetted on NET 2.0
            //CompilerConsole.FrameworkVersion = FrameworkVersion;

            AvmShell.Options.InterpretDefaultValue = !JITModeEnabled;
            QA.TestDebugSupport = miPDBTestMode.Checked;

#if DEBUG
            tbLog.Clear();
            if (btnLogSwitch.Checked)
            {
                DebugService.Log = _twLog;
                DebugHooks.Log = _twLog;
            }
            else
            {
                DebugService.Log = null;
                DebugHooks.Log = null;
            }

            DebugHooks.DumpILCode = miDumpILCode.Checked;
            DebugHooks.DumpILMap = miDumpILMap.Checked;
            DebugHooks.DumpSrc = miDumpSrc.Checked;
            DebugHooks.VisualizeGraphAfter = miGraphViz.Checked;
            DebugHooks.VisualizeGraphBefore = miGraphViz.Checked;
            DebugHooks.VisualizeGraphStructuring = miGraphVizStruct.Checked;

            DebugHooks.RecordLastError = miRecordLastError.Checked;
            DebugHooks.DebuggerBreak = miDebuggerBreak.Checked;
            DebugHooks.BreakWhileLoops = miBreakWhileLoops.Checked;
            DebugHooks.BreakDoWhileLoops = miBreakDoWhileLoops.Checked;
            DebugHooks.BreakEndlessLoops = miBreakEndlessLoops.Checked;
            DebugHooks.BreakInvalidMetadataToken = miBreakInvalidMetadataToken.Checked;
            DebugHooks.BreakInvalidMemberReference = miBreakInvalidMemberReference.Checked;
            DebugHooks.BreakInvalidTypeReference = miBreakInvalidTypeReference.Checked;

            DebugHooks.TypeName = cbTypeName.Text;
            DebugHooks.MethodName = cbMethodName.Text;

            DebugHooks.Phase = -1;
            string s = tbPhase.Text;
            int phase;
            if (!string.IsNullOrEmpty(s) && int.TryParse(s, out phase))
                DebugHooks.Phase = phase;

            DebugService.AvmDump = miAvmDump.Checked;
            DebugService.AbcDump = miAbcDumpEnabled.Checked;
            DebugService.DumpILMap = DebugHooks.DumpILMap;
            DebugService.BreakInternalCall = miBreakUnresolvedInternalCall.Checked;

            AbcDumpService.TextFormat = miAbcDumpTextFormat.Checked;
            AbcDumpService.XmlFormat = miAbcDumpXmlFormat.Checked;
            AbcDumpService.DumpCode = miAbcDumpCode.Checked;
            AbcDumpService.DumpClassList = miAbcDumpClassList.Checked;
            AbcDumpService.DumpTraits = miAbcDumpTraits.Checked;
#endif
        }

        void RunTestCase(TreeNode node, TestCase tc)
        {
            if (worker.CancellationPending) return;

            //if (_currentNode != null)
            //{
            //    safe(delegate
            //         {
            //             CollapseParent(_currentNode);
            //         });
            //}

            _currentNode = node;

            safe(() => ExpandParent(node));

            if (worker.CancellationPending) return;

            safe(() => node.BackColor = Color.Yellow);

            RunTestCase(tc);

            safe(() => UpdateState(node, tc));

            if (worker.CancellationPending)
            {
                _currentNode = null;
                return;
            }

            _currentNode = null;

            if (!_hasErrors)
            {
                if (tc.IsFinished && tc.HasErrors)
                    _hasErrors = true;
            }
        }

        void OnProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //progressBar.Value = e.ProgressPercentage;
            //string str = e.UserState as string;
            //if (!string.IsNullOrEmpty(str))
            //    status.Text = str;
        }

        void OnProcessCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var list = e.Result as List<TreeNode>;
            if (list == null) return;

            //progressBar.Visible = false;

            foreach (var node in list)
            {
                var tc = node.Tag as TestCase;
                if (tc != null)
                {
                    UpdateState(node, tc);
                }
            }
            OnSelectionChanged();
        }

        void UpdateState(TreeNode node, TestCase tc)
        {
            if (tc.IsStarted)
                node.BackColor = Color.Yellow;
            else if (tc.IsFinished)
            {
                node.BackColor = tc.HasErrors ? Color.Red : Color.Lime;
            }
            //testCases.Invalidate();
        }
        #endregion

        #region State/Settings
        static string GetStatePath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                "DataDynamics.PageFX.QA\\state.xml");
        }

        void LoadState()
        {
            LoadState(GetStatePath());
            LoadSettings();
        }

        static void LoadFlags(ToolStripItemCollection items)
        {
            foreach (ToolStripItem item in items)
            {
                var mi = item as ToolStripMenuItem;
                if (mi != null)
                {
                    var tag = mi.Tag;
                    string key = tag as string;
                    if (key != null)
                    {
                        mi.Checked = QA.GetValue(key, false);
                    }
                    else if (tag is GlobalOptionName)
                    {
                        var name = (GlobalOptionName)tag;
                        mi.Checked = QA.GetBoolOption(name);
                    }
                    LoadFlags(mi.DropDownItems);
                }
            }
        }

        static void SaveFlags(ToolStripItemCollection items)
        {
            foreach (ToolStripItem item in items)
            {
                var mi = item as ToolStripMenuItem;
                if (mi != null)
                {
                    string key = mi.Tag as string;
                    if (key != null)
                    {
                        QA.SetValue(key, mi.Checked);
                    }
                    SaveFlags(mi.DropDownItems);
                }
            }
        }

        const string KeyTestDriver = "TestDriver";
        const string KeyTypeName = "TypeName";
        const string KeyMethodName = "MethodName";
        const string KeyPhase = "Phase";
        const string KeyFormState = "QAForm.WindowState";
        const string KeyFormLocation = "QAForm.Location";
        const string KeyFormSize = "QAForm.Size";
        const string KeyAvmShellMode = "AvmShellMode";
        const string KeyTestDebugSupport = "TestDebugSupport";

        static class AvmShellMode
        {
            public const string Interpretation = "Interpretation";
            public const string JIT = "JIT";
        }

        bool JITModeEnabled
        {
            get { return cbAvmShellMode.SelectedIndex == 1; }
        }

        void LoadSettings()
        {
            _testDriver = TestDriver.AbcSerialization;
            string str = QA.GetValue(KeyTestDriver, "ABC");
	        _testDriver = str.EnumParse(TestDriver.AbcSerialization);
            
            str = QA.GetValue(KeyAvmShellMode, AvmShellMode.Interpretation);
            if (string.Compare(str, AvmShellMode.JIT, true) == 0)
                cbAvmShellMode.SelectedIndex = 1;

            LoadFlags(btnOptions.DropDownItems);

            btnLogSwitch.Checked = QA.GetValue("LogSwitch", true);

            cbTypeName.Text = QA.GetValue(KeyTypeName, "");
            cbMethodName.Text = QA.GetValue(KeyMethodName, "");
            tbPhase.Text = QA.GetValue(KeyPhase, "");
            
            int v = QA.GetValue(KeyFormState, (int)FormWindowState.Normal);
            switch (v)
            {
                case (int)FormWindowState.Minimized:
                    WindowState = FormWindowState.Minimized;
                    break;

                case (int)FormWindowState.Maximized:
                    WindowState = FormWindowState.Maximized;
                    break;

                default:
                    {
                        Location = QA.GetValue(KeyFormLocation, Location);
                        Size = QA.GetValue(KeyFormSize, Size);
                    }
                    break;
            }
        }

        void SaveSettings()
        {
			QA.SetValue(KeyTestDriver, _testDriver.EnumString());

            SaveFlags(btnOptions.DropDownItems);

            QA.SetValue("LogSwitch", btnLogSwitch.Checked);

            QA.SetValue(KeyTypeName, cbTypeName.Text);
            QA.SetValue(KeyMethodName, cbMethodName.Text);
            QA.SetValue(KeyPhase, tbPhase.Text);
            
            QA.SetValue(KeyFormState, (int)WindowState);
            QA.SetValue(KeyFormLocation, Location);
            QA.SetValue(KeyFormSize, Size);

            string mode = JITModeEnabled
                              ? AvmShellMode.JIT
                              : AvmShellMode.Interpretation;
            QA.SetValue(KeyAvmShellMode, mode);
        }

        bool _firstTestCase = true;

        void LoadState(string path)
        {
            var list = LoadTestCases(path);
            if (list != null)
            {
                foreach (var node in list)
                {
                    node.Checked = true;
                    if (_firstTestCase)
                    {
                        _firstTestCase = false;
                        ExpandParent(node);
                        node.EnsureVisible();
                        testCases.SelectedNode = node;
                    }
                }
            }
        }

        void SaveState(string path)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));

        	var xws = new XmlWriterSettings {Indent = true, IndentChars = "  "};
        	using (var writer = XmlWriter.Create(path, xws))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("qa");
                foreach (var node in GetSelectedTestCases())
                {
                    var tc = node.Tag as TestCase;
                    if (tc != null)
                    {
                        writer.WriteStartElement("tc");
                        writer.WriteAttributeString("name", tc.FullName);
                        writer.WriteEndElement();
                    }
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        void SaveState()
        {
            SaveState(GetStatePath());
            SaveSettings();
        }
        #endregion

        IEnumerable<TreeNode> LoadTestCases(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                	var doc = new XmlDocument();
                    doc.Load(path);
					if (doc.DocumentElement == null) return null;
                	return (from XmlElement e in doc.DocumentElement.GetElementsByTagName("tc")
							select e.GetAttribute("name") into name
							select _nodeCache[name]).OfType<TreeNode>().ToList();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return null;
        }

        static void ExpandParent(TreeNode node)
        {
            var parent = node.Parent;
            while (parent != null)
            {
                parent.Expand();
                parent = parent.Parent;
            }
        }

    	void btnRunAll_Click(object sender, EventArgs e)
        {
            if (!worker.IsBusy)
                worker.RunWorkerAsync(GetAllTestCases());
        }

        void OnSelectTestDriver(object sender, EventArgs e)
        {
            var mi = sender as ToolStripMenuItem;
            if (mi == null) return;
            if (mi.Tag is TestDriver)
            {
                _testDriver = (TestDriver)mi.Tag;
            }
        }

        void btnStop_Click(object sender, EventArgs e)
        {
            if (worker.IsBusy)
            {
                worker.CancelAsync();
            }
        }

        void miExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        void btnSaveSettings_Click(object sender, EventArgs e)
        {
            SaveState();
        }

        void OnResetDebugHooks(object sender, EventArgs e)
        {
#if DEBUG
            DebugHooks.Reset();
            DebugHooks.ResetLastError();
            miDumpILCode.Checked = DebugHooks.DumpILCode;
            miDumpILMap.Checked = DebugHooks.DumpILMap;
            miDumpSrc.Checked = DebugHooks.DumpSrc;
            miGraphViz.Checked = DebugHooks.VisualizeGraphAfter;
            miGraphViz.Checked = DebugHooks.VisualizeGraphBefore ;
            miGraphVizStruct.Checked = DebugHooks.VisualizeGraphStructuring;
            cbTypeName.Text = DebugHooks.TypeName;
            cbMethodName.Text = DebugHooks.MethodName;
            miDebuggerBreak.Checked = DebugHooks.DebuggerBreak;
            miBreakWhileLoops.Checked = DebugHooks.BreakWhileLoops;
            miBreakDoWhileLoops.Checked = DebugHooks.BreakDoWhileLoops;
            miBreakEndlessLoops.Checked = DebugHooks.BreakEndlessLoops;
            miBreakInvalidMetadataToken.Checked = DebugHooks.BreakInvalidMetadataToken;
            miBreakInvalidMemberReference.Checked = DebugHooks.BreakInvalidMemberReference;
            miBreakInvalidTypeReference.Checked = DebugHooks.BreakInvalidTypeReference;
#endif
            tbPhase.Text = "";
        }

        void btnResetLastError_Click(object sender, EventArgs e)
        {
#if DEBUG
            DebugHooks.ResetLastError();
#endif
        }

        void btnCompile_Click(object sender, EventArgs e)
        {
            var selnode = testCases.SelectedNode;
            if (selnode == null) return;

            var tc = selnode.Tag as TestCase;
            if (tc == null) return;

            tc.VM = VM.AVM;
            docOutput.Text = "";

            if (!QA.Compile(tc))
            {
                docOutput.Text = tc.Error;
            }
        }

        void miCleanQADir_Click(object sender, EventArgs e)
        {
            try
            {
                Directory.Delete(QA.BaseDir, true);
            }
            catch (Exception exc)
            {
                MessageBox.Show("Unable to clean QA dir.\nException:\n" + exc,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        List<TestSuite> GetSelectedTestSuites(List<TestCase> failedTestCases)
        {
            var list = new List<TestSuite>();
            foreach (TreeNode node in testCases.Nodes)
            {
                GetSelectedTestSuites(list, node, failedTestCases);
            }
            return list;
        }

        void UpdateTestSuiteStat(TestSuite ts, TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                var tc = node.Tag as TestCase;
                if (tc != null)
                {
                    if (node.Checked)
                    {
                        if (tc.IsFailed)
                            ts.TotalFailed++;
                        else
                            ts.TotalPassed++;
                    }
                }
                else
                {
                    UpdateTestSuiteStat(ts, node.Nodes);
                }
            }
        }

        static bool IsSelected(TreeNode node)
        {
            if (node.Checked) return true;
        	return node.Nodes.Cast<TreeNode>().Any(kid => kid.Checked);
        }

        void GetSelectedTestSuites(List<TestSuite> list, TreeNode node, List<TestCase> failedTestCases)
        {
            var ts = node.Tag as TestSuite;
            if (ts != null)
            {
                if (IsSelected(node))
                {
                    list.Add(ts);
                    ts.Reset();
                    UpdateTestSuiteStat(ts, node.Nodes);   
                }
                foreach (TreeNode kid in node.Nodes)
                {
                    GetSelectedTestSuites(list, kid, failedTestCases);
                }
            }
            else if (failedTestCases != null)
            {
                var tc = node.Tag as TestCase;
                if (tc != null && node.Checked)
                {
                    if (tc.IsFailed)
                        failedTestCases.Add(tc);
                }
            }
        }

        #region Reporting
        void ShowReport(ICollection<TreeNode> list)
        {
            if (!miShowReport.Checked) return;
            if (list.Count <= 1) return;
            string path = GetReportPath();
            GenerateHtmlReport(path);
            QA.ShowBrowser("Test Results", path, false);
        }

        static string GetReportPath()
        {
            string dir = QA.Root;
            Directory.CreateDirectory(dir);
            return Path.Combine(dir, "report.htm");
        }

        TestResult GetTestResult()
        {
            var failedTestCases = new List<TestCase>();
            var suites = GetSelectedTestSuites(failedTestCases);

        	return new TestResult
        	       	{
        	       		FailedTestCases = failedTestCases,
        	       		Suites = suites
        	       	};
        }

        void GenerateHtmlReport(string path)
        {
            using (var writer = new StreamWriter(path))
                GenerateHtmlReport(writer);
        }

        void GenerateHtmlReport(TextWriter writer)
        {
            var tr = GetTestResult();
            tr.GenerateHtmlReport(writer);
        }
        #endregion

        void miOptions_Click(object sender, EventArgs e)
        {
            using (var dlg = new OptionsDialog())
            {
                dlg.Root = new OptionNode("Options", null,
                                          new OptionNode("General", new GeneralOptionsPage()));
                dlg.ShowDialog();
            }
        }

        void miGenerateWikiReport_Click(object sender, EventArgs e)
        {
            using (var dlg = new SaveFileDialog())
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    GenerateWikiReport(dlg.FileName);
                }
            }
        }

        void GenerateWikiReport(string path)
        {
            using (var writer = new StreamWriter(path))
                GenerateWikiReport(writer);
        }

        void GenerateWikiReport(TextWriter writer)
        {
            var tr = GetTestResult();
            tr.GenerateWikiReport(writer);
        }

        void miGenerateHtmlReport_Click(object sender, EventArgs e)
        {
            using (var dlg = new SaveFileDialog())
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    GenerateHtmlReport(dlg.FileName);
                    QA.ShowBrowser("Test Results", dlg.FileName, false);
                }
            }
        }

        #region Compare Text Files
        void miCompareTextFiles_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Multiselect = true;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    var files = dlg.FileNames;
                    if (files.Length == 2)
                    {
                        try
                        {
                            string out1 = File.ReadAllText(files[0]);
                            string out2 = File.ReadAllText(files[1]);
                            string err = QA.CompareLines(out1, out2, true);
                            err = Path.GetFileName(files[0]) + " " + Path.GetFileName(files[1]) + "\n" + err;
                            docOutput.Text = err;
                        }
                        catch (Exception exc)
                        {
                            MessageBox.Show(exc.ToString());
                        }
                    }
                }
            }
        }
        #endregion

        void btnSelectFailed_Click(object sender, EventArgs e)
        {
            var list = GetSelectedTestCases();
            foreach (var node in list)
            {
                var tc = node.Tag as TestCase;
                if (tc != null)
                {
                    node.Checked = tc.IsFinished && tc.IsFailed;
                }
            }
        }

        void btnCopy_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            var list = GetSelectedTestCases();
            foreach (var node in list)
            {
                var tc = node.Tag as TestCase;
                if (tc != null)
                {
                    //sb.AppendLine(tc.FullDisplayName);
                    sb.AppendLine(tc.FullName);
                }
            }
            if (sb.Length > 0)
                Clipboard.SetText(sb.ToString());
        }

        void miGenerateAllNUnitTests_Click(object sender, EventArgs e)
        {
        }

    	void cbAvmShellMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplySettings();
        }

        void btnNUnitSession_Click(object sender, EventArgs e)
        {
            QA.IsNUnitSession = btnNUnitSession.Checked;
        }
    }
}