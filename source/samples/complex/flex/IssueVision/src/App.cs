using System;
using System.Data;
using flash.events;
using flash.system;
using mx.core;
using mx.controls;
using mx.containers;
using EventHandler=System.EventHandler;

namespace IssueVision
{
    public class App : Application
    {
        #region Fields
        private readonly HBox _toolbar;
        public readonly MainPane MainPane;
        private IVService _service;
        private readonly IVData _data;
        #endregion

        #region ctor
        public static App Instance;

        public App()
        {
            Instance = this;

            Security.loadPolicyFile(IVService.RootUrl + "crossdomain.xml");

            _toolbar = new HBox();
            _toolbar.height = 28;
            _toolbar.percentWidth = 100;
            addChild(_toolbar);

            AddButton("Load Data", OnLoadData);
            //AddButton("Load Issues Xml", OnLoadIssues);
            //AddButton("Load Lookup Xml", OnLoadLookupTables);
            //AddButton("Load WSDL", OnLoadWSDL);
            //AddButton("Load Schema", OnLoadSchema);
            //AddButton("Show Data", OnShowData);

            MainPane = new MainPane();
            Style.SetConstraints(MainPane, Constraint.LRB, 10);
            Style.SetConstraints(MainPane, Constraint.Top, 30);
            addChild(MainPane);

            _data = new IVData();
            InitService();
            LoadData();

            _rootController = new RootController();
        }

        private RootController _rootController;
        #endregion

        #region Properties
        public IVData Data
        {
            get { return _data; }
        }
        #endregion

        #region Events
        public event EventHandler DataLoaded;

        private void FireDataLoaded()
        {
            if (_data.IsLoaded)
            {
                if (DataLoaded != null)
                    DataLoaded(this, EventArgs.Empty);
            }
        }
        #endregion

        #region Toolbar
        private void AddButton(string label, MouseEventHandler handler)
        {
            Button btn = new Button();
            btn.label = label;
            btn.click += handler;
            _toolbar.addChild(btn);
        }

        private void OnLoadData(MouseEvent e)
        {
            LoadData();
        }

        private void OnLoadIssues(MouseEvent e)
        {
            InitService();
            _service.GetIssuesXml();
        }

        private void OnLoadLookupTables(MouseEvent e)
        {
            InitService();
            _service.GetLookupTablesXml();
        }

        private static void OnLoadWSDL(MouseEvent e)
        {
            HttpHelper.LoadXml(IVService.WSDL,
                               delegate(string xml)
                               {
                                   TextWindow.Show("WSDL", xml);
                               });
        }

        private static void OnLoadSchema(MouseEvent e)
        {
            HttpHelper.LoadXml(IVService.RootUrl + "IVDataSet.xsd",
                               delegate(string xml)
                               {
                                   TextWindow.Show("Schema", xml);
                               });
        }

        private void OnShowData(MouseEvent e)
        {
            string s = "";
            if (_data.Lookup != null)
            {
                s += Utils.ToString(_data.Lookup);
                s += "\n";
            }
            if (_data.Issues != null)
            {
                s += Utils.ToString(_data.Issues);
                s += "\n";
            }
            TextWindow.Show("Data", s);
        }
        #endregion

        #region Communication with WebService
        private void InitService()
        {
            if (_service == null)
            {
                _service = new IVService();
                _service.IssuesLoadedXml += OnIssueLoadedXml;
                _service.IssuesLoaded += OnIssuesLoaded;
                _service.LookupLoadedXml += OnLookupLoadedXml;
                _service.LookupLoaded += OnLookupLoaded;
            }
        }

        private void LoadData()
        {
            _service.GetLookupTables();
        }

        private void OnLookupLoaded(IVDataSet data)
        {
            //TextWindow.Show("Lookup Tables", Utils.ToString(data));
            _data.Lookup = data;
            _service.GetIssues();
            //FireDataLoaded();
        }

        private void OnIssuesLoaded(IVDataSet data)
        {
            //TextWindow.Show("Issues", Utils.ToString(data));
            _data.Issues = data;
            FireDataLoaded();
        }

        private static void OnIssueLoadedXml(string xml)
        {
            TextWindow.Show("Issues Xml", xml);
        }

        private static void OnLookupLoadedXml(string xml)
        {
            TextWindow.Show("Lookup Tables Xml", xml);
        }
        #endregion
    }
}