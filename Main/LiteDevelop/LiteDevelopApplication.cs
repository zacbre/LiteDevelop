using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LiteDevelop.Extensions;
using WeifenLuo.WinFormsUI.Docking;
using LiteDevelop.Gui;
using LiteDevelop.Framework.Extensions;
using LiteDevelop.Framework.FileSystem;
using System.Threading;
using System.ComponentModel;
using LiteDevelop.Framework.Mui;
using LiteDevelop.Gui.Forms;
using LiteDevelop.Gui.DockContents;
using LiteDevelop.Gui.DockContents.SolutionExplorer;
using LiteDevelop.Framework.Gui;

namespace LiteDevelop
{
    public class LiteDevelopApplication
    {
        public static LiteDevelopApplication Current 
        {
            get;
            private set; 
        }

        public static string SourceRepositoryUrl
        {
            get { return "http://www.github.com/JerreS/LiteDevelop"; }
        }

        public event EventHandler InitializedApplication;
        private LiteExtensionHost _extensionHost;
        private SplashScreen _splashScreen;
        private MainForm _mainForm;
        private BackgroundWorker _worker;
        private MuiProcessor _muiProcessor;
        private AppearanceProcessor _appearanceProcessor;
 
        public static void Run(string[] args)
        {
            var app = new LiteDevelopApplication();
            app.Start(args);
        }

        private LiteDevelopApplication()
        {
            if (Current != null)
                throw new InvalidOperationException("Cannot create a second application instance.");
            
            Current = this;
        }

        public bool IsInitialized
        {
            get;
            private set;
        }

        public LiteExtensionHost ExtensionHost
        {
            get { return _extensionHost; }
        }

        public MuiProcessor MuiProcessor
        {
            get { return _muiProcessor; }
        }

        public void Start(string[] args)
        {
            // TODO handle command line arguments....

            _mainForm = new MainForm();
            _mainForm.Disposed += (o, e) =>
                {
                    LiteDevelopSettings.Instance.Save();
                    Current = null;
                };

            _worker = new BackgroundWorker();
            _worker.DoWork += (o, e) =>
                {
                    InitializeExtensionHost();
                    LoadExtensions();
                };
            _worker.RunWorkerCompleted += (o, e) =>
                {
                    OnInitializedApplication(EventArgs.Empty);
                    _splashScreen.Dispose();
                    
                };
            
            _splashScreen = new SplashScreen();
            _splashScreen.FadedIn += (o, e) =>
                {
                    _worker.RunWorkerAsync();
                };

            Application.Run(_splashScreen);
            Application.Run(_mainForm);            
        }
                
        private void InitializeExtensionHost()
        {
            _extensionHost = new LiteExtensionHost();
            var solutionExplorer = _mainForm.GetToolWindow<SolutionExplorerContent>();
            var outputWindow = _mainForm.GetToolWindow<OutputContent>();

            _extensionHost.SettingsManager = new ExtensionSettingsManager();

            _extensionHost.ControlManager = new ControlManager(_extensionHost)
            {
                DockPanel = _mainForm.DockPanel,
                ToolStripPanel = _mainForm.ToolStripPanel,
                MenuStrip = _mainForm.MenuStrip,
                EditMenu = _mainForm.EditItem,
                ViewMenu = _mainForm.ViewItem,
                DebugMenu = _mainForm.DebugItem,
                ToolsMenu = _mainForm.ToolsItem,
                StatusStrip = _mainForm.StatusStrip,
                SolutionExplorerMenu = FindControl(solutionExplorer, "mainTreeView").ContextMenuStrip
            };

            _extensionHost.FileService = new FileService(_extensionHost);
            _extensionHost.BookmarkManager = new BookmarkManager();
            _extensionHost.ErrorManager = new ErrorManager();
            _extensionHost.OutputContent = outputWindow;
            _extensionHost.ProgressBar = _mainForm.DefaultStatusProgressBar.ProgressBar;
            _extensionHost.CredentialManager = new CredentialManager();
            _extensionHost.UILanguage = UILanguage.GetLanguageById(LiteDevelopSettings.Instance.GetValue("Application.LanguageID"));

            _muiProcessor = new Framework.Mui.MuiProcessor(_extensionHost, Path.Combine(Application.StartupPath, "MUI"));

            _extensionHost.ExtensionManager = new ExtensionManager(_extensionHost);

            _appearanceProcessor = _extensionHost.ControlManager.GlobalAppearanceMap.Processor;
        }
        
        private void LoadExtensions()
        {
            var results = new List<ExtensionLoadResult>();
            foreach (var library in LiteDevelopSettings.Instance.GetArray<ExtensionLibraryData>("Application.Extensions"))
            {
                results.AddRange(_extensionHost.ExtensionManager.LoadExtensions(library));
            }
            foreach (var result in results)
            {
                if (!result.SuccesfullyLoaded)
                {
                    new ExtensionLoadDialog(results).ShowDialog();
                    break;
                }
            }
        }


        private Control FindControl(Control parent, string name)
        {
            foreach (Control control in parent.Controls)
            {
                if (control.Name == name)
                    return control;

                Control ctrl = FindControl(control, name);
                if (ctrl != null)
                    return ctrl;
            }

            return null;
        }

        protected void OnInitializedApplication(EventArgs e)
        {
            IsInitialized = true;
            if (InitializedApplication != null)
                InitializedApplication(this, e);
        }
    }
}
