using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using LiteDevelop.Framework;
using LiteDevelop.Framework.Extensions;
using LiteDevelop.Framework.FileSystem;
using LiteDevelop.Framework.Gui;
using WeifenLuo.WinFormsUI.Docking;
using LiteDevelop.Extensions;
using LiteDevelop.Gui.DockContents;
using LiteDevelop.Gui.DockContents.SolutionExplorer;
using System.Threading;

namespace LiteDevelop.Gui.Forms
{
    public partial class MainForm : Form
    {
        private enum PostBuildAction
        {
            None,
            Run,
            Debug,
        }

        private Dictionary<object, string> _componentMuiIdentifiers;
        private readonly DockContent[] _contents;
        private readonly ToolboxContent _toolBox;
        private readonly PropertiesContent _propertiesWindow;
        private readonly SolutionExplorerContent _solutionExplorer;
        private readonly ErrorContent _errorList;
        private readonly OutputContent _outputWindow;
        private LiteExtensionHost _extensionHost;
        private readonly string _dockConfigPath = Path.Combine(Constants.AppDataDirectory, "dock.xml");
        private readonly DockPanel _mainDockPanel;
        private PostBuildAction _postBuildAction;
        private FormWindowState _lastWindowState;

        public MainForm()
        {
            InitializeComponent();

            Location = LiteDevelopSettings.Instance.GetValue<Point>("MainWindow.Location");
            Size = LiteDevelopSettings.Instance.GetValue<Size>("MainWindow.Size");

            if (LiteDevelopSettings.Instance.GetValue<bool>("MainWindow.Maximized"))
                WindowState = FormWindowState.Maximized;

            _mainDockPanel = new DockPanel() 
            {
                Name = "mainDockPanel",
                Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom,
                Left = 0,
                Top = toolStripPanel1.Bottom,
                Height = mainStatusStrip.Top - toolStripPanel1.Bottom,
                Width = ClientSize.Width,
                BackColor = SystemColors.Control,
                DocumentStyle = DocumentStyle.DockingWindow,
                ShowDocumentIcon = true,
                AllowDrop = true,
            };

            _mainDockPanel.DockBottomPortion = _mainDockPanel.DockLeftPortion = _mainDockPanel.DockRightPortion = _mainDockPanel.DockBottomPortion = 200;
            _mainDockPanel.DragDrop += _mainDockPanel_DragDrop;
            _mainDockPanel.DragEnter += _mainDockPanel_DragEnter;
            
            _contents = new DockContent[]
            { 
                _toolBox = new ToolboxContent(),
                _propertiesWindow = new PropertiesContent(),
                _solutionExplorer = new SolutionExplorerContent(),
                _errorList = new ErrorContent(),
                _outputWindow = new OutputContent()
            };

            // manual set keys, visual studio's designer doesn't show the enter key (at least not for me).
            fullScreenToolStripMenuItem.ShortcutKeys = Keys.Alt | Keys.Enter; 

            SetupMuiComponents();
            LiteDevelopApplication.Current.InitializedApplication += new EventHandler(ThisApplication_InitializedApplication);
        }

        private void ThisApplication_InitializedApplication(object sender, EventArgs e)
        {
            if (!TryLoadDockPanelState())
            {
                _toolBox.Show(_mainDockPanel, DockState.DockLeft);
                _propertiesWindow.Show(_mainDockPanel, DockState.DockRight);
                _errorList.Show(_mainDockPanel, DockState.DockBottomAutoHide);
                _outputWindow.Show(_mainDockPanel, DockState.DockBottomAutoHide);
                _solutionExplorer.Show(_propertiesWindow.Pane, DockAlignment.Top, 0.5);
            }

            this.Controls.Add(_mainDockPanel);
            
            _mainDockPanel.ContentAdded += _mainDockPanel_ContentAdded;
            _mainDockPanel.ActiveContentChanged += _mainDockPanel_ActiveContentChanged;

            _extensionHost = LiteDevelopApplication.Current.ExtensionHost as LiteExtensionHost;
            _extensionHost.ErrorManager.NavigateToErrorRequested += ErrorManager_NavigateToErrorRequested;
            _extensionHost.SolutionLoad += _extensionHost_SolutionLoad;
            _extensionHost.SolutionUnload += _extensionHost_SolutionUnload;

            _extensionHost.ControlManager.AppearanceChanged += ControlManager_AppearanceChanged;
            ControlManager_AppearanceChanged(null, null);

            this.toolStripPanel1.Resize += toolStripPanel1_Resize;
            this.Resize += MainForm_Resize;

            _errorList.SetErrorManager(_extensionHost.ErrorManager);
      
            mainMenuStrip.Renderer = mainToolBar.Renderer = mainStatusStrip.Renderer = toolStripPanel1.Renderer = _extensionHost.ControlManager.MenuRenderer;

            _extensionHost.UILanguageChanged += _extensionHost_UILanguageChanged;
            _extensionHost_UILanguageChanged(null, null);
            
        }

        private void SetupMuiComponents()
        {
            // i know its a bit ugly :3
            _componentMuiIdentifiers = new Dictionary<object, string>()
            {
                // menu bar 
                {this.fileToolStripMenuItem, "MainForm.Menu.File.Title"},
                {this.newToolStripMenuItem, "MainForm.Menu.File.New"},
                {this.newFileToolStripMenuItem, "MainForm.Menu.File.New.NewFile"},
                {this.newProjectToolStripMenuItem, "MainForm.Menu.File.New.NewProject"},
                {this.openToolStripMenuItem , "MainForm.Menu.File.Open"},
                {this.saveToolStripMenuItem , "MainForm.Menu.File.Save"},
                {this.saveAsToolStripMenuItem, "MainForm.Menu.File.SaveAs"},
                {this.saveAllToolStripMenuItem, "MainForm.Menu.File.SaveAll"},
                {this.closeToolStripMenuItem, "MainForm.Menu.File.Close"},
                {this.closeSolutionToolStripMenuItem, "MainForm.Menu.File.CloseSolution"},
                {this.printToolStripMenuItem, "MainForm.Menu.File.Print"},
                {this.printPreviewToolStripMenuItem, "MainForm.Menu.File.PrintPreview"},
                {this.exitToolStripMenuItem, "MainForm.Menu.File.Exit"},

                {this.editToolStripMenuItem, "MainForm.Menu.Edit.Title"},
                {this.undoToolStripMenuItem, "MainForm.Menu.Edit.Undo"},
                {this.redoToolStripMenuItem, "MainForm.Menu.Edit.Redo"},
                {this.cutToolStripMenuItem, "MainForm.Menu.Edit.Cut"},
                {this.copyToolStripMenuItem, "MainForm.Menu.Edit.Copy"},
                {this.pasteToolStripMenuItem, "MainForm.Menu.Edit.Paste"},
                {this.selectAllToolStripMenuItem, "MainForm.Menu.Edit.SelectAll"},

                {this.viewToolStripMenuItem, "MainForm.Menu.View.Title"},
                {this.toolBoxToolStripMenuItem, "MainForm.Menu.View.Toolbox"},
                {this.solutionExplorerToolStripMenuItem, "MainForm.Menu.View.SolutionExplorer"},
                {this.propertiesToolStripMenuItem, "MainForm.Menu.View.PropertiesWindow"},
                {this.errorListToolStripMenuItem, "MainForm.Menu.View.ErrorList"},
                {this.outputToolStripMenuItem, "MainForm.Menu.View.OutputWindow"},
                {this.fullScreenToolStripMenuItem, "MainForm.Menu.View.Fullscreen"},

                {this.buildToolStripMenuItem, "MainForm.Menu.Build.Title"},
                {this.buildSolutionToolStripMenuItem, "MainForm.Menu.Build.BuildSolution"},
                {this.cleanSolutionToolStripMenuItem, "MainForm.Menu.Build.CleanSolution"},
                
                {this.debugToolStripMenuItem, "MainForm.Menu.Debug.Title"},
                {this.runToolStripMenuItem, "MainForm.Menu.Debug.Run"},
                {this.runWithoutDebuggerToolStripMenuItem, "MainForm.Menu.Debug.RunWithoutDebugger"},
                {this.runLastBuildToolStripMenuItem, "MainForm.Menu.Debug.RunLastBuild"},
                {this.breakToolStripMenuItem, "MainForm.Menu.Debug.Break"},
                {this.stopDebuggingToolStripMenuItem, "MainForm.Menu.Debug.StopDebugging"},
                {this.stepOverToolStripMenuItem, "MainForm.Menu.Debug.StepOver"},
                {this.stepIntoToolStripMenuItem, "MainForm.Menu.Debug.StepInto"},
                {this.stepOutToolStripMenuItem, "MainForm.Menu.Debug.StepOut"},

                {this.toolsToolStripMenuItem, "MainForm.Menu.Tools.Title"},
                {this.settingsToolStripMenuItem, "MainForm.Menu.Tools.Settings"},
                {this.extensionsToolStripMenuItem, "MainForm.Menu.Tools.Extensions"},

                {this.windowToolStripMenuItem, "MainForm.Menu.Window.Title"},
                {this.nextWindowToolStripMenuItem, "MainForm.Menu.Window.NextWindow"},
                {this.previousWindowToolStripMenuItem, "MainForm.Menu.Window.PreviousWindow"},
                {this.closeAllDocumentsToolStripMenuItem, "MainForm.Menu.Window.CloseAllDocuments"},

                {this.helpToolStripMenuItem, "MainForm.Menu.Help.Title"},
                {this.sourceCodeToolStripMenuItem, "MainForm.Menu.Help.SourceCode"},
                {this.aboutToolStripMenuItem, "MainForm.Menu.Help.About"},

                // tool bar
                {this.newToolStripButton, "MainForm.Menu.File.New.NewFile"},
                {this.openToolStripButton, "MainForm.Menu.File.Open"},
                {this.saveToolStripButton, "MainForm.Menu.File.Save"},
                {this.printToolStripButton, "MainForm.Menu.File.Print"},
                {this.cutToolStripButton, "MainForm.Menu.Edit.Cut"},
                {this.copyToolStripButton, "MainForm.Menu.Edit.Copy"},
                {this.pasteToolStripButton, "MainForm.Menu.Edit.Paste"},
                {this.buildSolutionStripButton, "MainForm.Menu.Build.BuildSolution"},
                {this.runToolStripButton, "MainForm.Menu.Debug.Run"},
                {this.runWithoutDebuggerToolStripButton, "MainForm.Menu.Debug.RunWithoutDebugger"},
                {this.aboutToolStripButton, "MainForm.Menu.Help.About"},
            };

        }

        #region Properties

        public LiteDocumentContent CurrentDocument
        {
            get;
            private set;
        }

        public DockPanel DockPanel
        {
            get { return _mainDockPanel; }
        }

        public ToolStripPanel ToolStripPanel
        {
            get { return toolStripPanel1; }
        }

        public MenuStrip MenuStrip
        {
            get { return mainMenuStrip; }
        }

        public ToolStripMenuItem EditItem
        {
            get { return editToolStripMenuItem; }
        }

        public ToolStripMenuItem ViewItem
        {
            get { return viewToolStripMenuItem; }
        }

        public ToolStripMenuItem DebugItem
        {
            get { return debugToolStripMenuItem; }
        }

        public ToolStripMenuItem ToolsItem
        {
            get { return toolsToolStripMenuItem; }
        }

        public ToolStrip ToolBar
        {
            get { return mainToolBar; }
        }

        public StatusStrip StatusStrip
        {
            get { return mainStatusStrip; }
        }

        public ToolStripProgressBar DefaultStatusProgressBar
        {
            get { return toolStripProgressBar1; }
        }

        #endregion

        #region Utilities

        internal T GetToolWindow<T>() where T: DockContent
        {
            return _contents.FirstOrDefault(x => x is T) as T;
        }

        private bool TryLoadDockPanelState()
        {
            if (File.Exists(_dockConfigPath))
            {
                using (var configStream = File.OpenRead(_dockConfigPath))
                {
                    _mainDockPanel.LoadFromXml(configStream, GetDockContentInstance);
                    return true;
                }
            }
            return false;
        }

        private IDockContent GetDockContentInstance(string persistName)
        {
            foreach (var dockConcent in _contents)
                if (dockConcent.GetType().FullName == persistName)
                    return dockConcent;
            return null;
        }

        private void SaveDockPanelState()
        {
            _mainDockPanel.SaveAsXml(_dockConfigPath);
        }

        private void ProcessClipboardCommands(LiteDocumentContent documentContent)
        {
            var clipboardHandler = documentContent as IClipboardHandler;

            if (clipboardHandler == null)
            {
                cutToolStripButton.Enabled = cutToolStripMenuItem.Enabled =
                    copyToolStripButton.Enabled = copyToolStripMenuItem.Enabled =
                    pasteToolStripButton.Enabled = pasteToolStripMenuItem.Enabled = false;
            }
            else
            {
                cutToolStripButton.Enabled = cutToolStripMenuItem.Enabled = clipboardHandler.IsCutEnabled;
                copyToolStripButton.Enabled = copyToolStripMenuItem.Enabled = clipboardHandler.IsCopyEnabled;
                pasteToolStripButton.Enabled = pasteToolStripMenuItem.Enabled = clipboardHandler.IsPasteEnabled;
            }

        }

        private void ProcessHistoryCommands(LiteDocumentContent documentContent)
        {
            undoToolStripMenuItem.Enabled = redoToolStripMenuItem.Enabled = documentContent is IHistoryProvider;
        }
        
        private void UserOpenWith(FilePath path)
        {
            var fileHandlers = _extensionHost.ExtensionManager.GetFileHandlers(path).ToArray();

            if (fileHandlers.Length == 0)
            {
                MessageBox.Show(LiteDevelopApplication.Current.MuiProcessor.GetString("MainForm.Messages.NoEditorAvailable", "file=" + path.FullPath), "LiteDevelop", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                var fileHandler = _extensionHost.FileService.SelectFileHandler(fileHandlers, path);

                if (fileHandler != null)
                {
                    fileHandler.OpenFile(_extensionHost.FileService.OpenFile(path));
                }
            }
        }

        private void UserOpenWith(string file)
        {
            UserOpenWith(new FilePath(file));
        }

        private void CloseCurrentSolution()
        {
            int remainingTabCount = 0;
            while (_mainDockPanel.DocumentsCount != remainingTabCount)
            {
                var dockContent = (DockContent)_mainDockPanel.DocumentsToArray()[0];
                if (dockContent.Tag is LiteDocumentContent)
                    dockContent.Close();
                else
                    remainingTabCount++;
            }
            _extensionHost.DispatchSolutionUnload(new SolutionEventArgs(_extensionHost.CurrentSolution));
        }

        private void SetToolBox(LiteDocumentContent currentDocument)
        {
            var pane = _mainDockPanel.FindContent<ToolboxContent>();
            if (pane != null)
            {
                pane.SetToolBox(currentDocument is IToolboxServiceProvider ? (currentDocument as IToolboxServiceProvider).ToolboxService : null);
            }
        }

        private void SetPropertyContainer(LiteDocumentContent currentDocument)
        {
            var pane = _mainDockPanel.FindContent<PropertiesContent>();
            if (pane != null)
            {
                pane.SetPropertyContainer(currentDocument is IPropertyContainerProvider ? (currentDocument as IPropertyContainerProvider).PropertyContainer : null);
            }
        }

        private ISavableFile[] GetUnsavedItems()
        {
            ISavableFile[] entries = GetUnsavedFiles();

            if (_extensionHost.CurrentSolution != null)
            {
                if (_extensionHost.CurrentSolution.HasUnsavedData)
                    entries = entries.MergeWith(new ISavableFile[] { _extensionHost.CurrentSolution });
                entries = entries.MergeWith(GetUnsavedProjects(_extensionHost.CurrentSolution));
            }

            return entries;
        }

        private Project[] GetUnsavedProjects(SolutionFolder folder)
        {
            var projects = new List<Project>();

            foreach (var node in folder.Nodes)
            {
                if (node is ProjectEntry)
                {
                    var projectEntry = node as ProjectEntry;
                    if (projectEntry.HasProject && projectEntry.Project.HasUnsavedData)
                    {
                        projects.Add(projectEntry.Project);
                    }
                }
                else if (node is SolutionFolder)
                    projects.AddRange(GetUnsavedProjects(node as SolutionFolder));
            }

            return projects.ToArray();
        }

        private OpenedFile[] GetUnsavedFiles()
        {
            var files = new List<OpenedFile>();

            foreach (var file in _extensionHost.FileService.OpenedFiles)
            {
                if (file.HasUnsavedData)
                {
                    files.Add(file);
                }
            }

            return files.ToArray();
        }

        private void UpdateCurrentActiveContent()
        {
            foreach (ToolStripItem item in windowToolStripMenuItem.DropDownItems)
            {
                if (item is ToolStripMenuItem)
                {
                    (item as ToolStripMenuItem).Checked = CurrentDocument != null && CurrentDocument == item.Tag;
                }
            }
        }

        #endregion

        #region Menu event handlers

        #region File menu event handlers

        private void newFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateFileDialog.UserCreateFile(_extensionHost);
        }

        private void newProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateProjectDialog.UserCreateProject(_extensionHost);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (Path.GetExtension(ofd.FileName) == ".sln")
                    {
                        if (_extensionHost.CurrentSolution != null)
                            CloseCurrentSolution();

                        var solution = Solution.OpenSolution(ofd.FileName);
                        solution.LoadComplete += new SolutionNodeLoadEventHandler(solution_LoadComplete);
                        solution.BeginLoad(_extensionHost.CreateOrGetReporter(string.Empty));
                    }
                    else
                    {
                        UserOpenWith(ofd.FileName);
                    }
                }
            }
        }

        private void solution_LoadComplete(object sender, SolutionNodeLoadEventArgs e)
        {
            Invoke(new Action(() =>
            {
                var solution = sender as Solution;
                solution.LoadComplete -= new SolutionNodeLoadEventHandler(solution_LoadComplete);
                _extensionHost.DispatchSolutionLoad(new SolutionEventArgs(solution));
            }));
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentDocument != null)
            {
                CurrentDocument.AssociatedFile.Save(_extensionHost.CreateOrGetReporter("Build"));
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog())
            {
                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (CurrentDocument != null)
                    {
                        using (var stream = File.OpenWrite(sfd.FileName))
                        {
                            CurrentDocument.Save(stream);
                        }
                    }
                }
            }
        }

        private void saveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_extensionHost.CurrentSolution != null)
            {
                _extensionHost.CurrentSolution.Save(_extensionHost.CreateOrGetReporter("Build"));
            }    

            foreach (var file in _extensionHost.FileService.OpenedFiles)
            {
                if (file.HasUnsavedData)
                    file.Save(_extensionHost.CreateOrGetReporter("Build"));
            }
            
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _mainDockPanel.GetActiveDocument().Close();
        }

        private void closeSolutionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_extensionHost.CurrentSolution != null)
            {
                if (MessageBox.Show(LiteDevelopApplication.Current.MuiProcessor.GetString("MainForm.Messages.UnloadCurrentSolutionWarning"), "LiteDevelop", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
                {
                    CloseCurrentSolution();
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #endregion

        #region Edit menu event handlers

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.CurrentDocument is IClipboardHandler)
            {
                (this.CurrentDocument as IClipboardHandler).Cut();
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.CurrentDocument is IClipboardHandler)
            {
                (this.CurrentDocument as IClipboardHandler).Copy();
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.CurrentDocument is IClipboardHandler)
            {
                (this.CurrentDocument as IClipboardHandler).Paste();
            }
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.CurrentDocument is IHistoryProvider)
            {
                (this.CurrentDocument as IHistoryProvider).Undo();
            }
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.CurrentDocument is IHistoryProvider)
            {
                (this.CurrentDocument as IHistoryProvider).Redo();
            }
        }

        #endregion

        #region View menu event handlers 

        private void toolBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _toolBox.ShowAndActivate(_mainDockPanel);
        }

        private void solutionExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _solutionExplorer.ShowAndActivate(_mainDockPanel);
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _propertiesWindow.ShowAndActivate(_mainDockPanel);
        }

        private void errorListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _errorList.ShowAndActivate(_mainDockPanel);
        }

        private void outputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _outputWindow.ShowAndActivate(_mainDockPanel);
        }

        private void fullScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fullScreenToolStripMenuItem.Checked)
            {
                FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                _lastWindowState = WindowState;
                WindowState = FormWindowState.Normal;
                WindowState = FormWindowState.Maximized;
            }
            else
            {
                FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
                WindowState = _lastWindowState;
            }
        }

        #endregion

        #region Build menu event handlers
  
        private void buildSolutionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (var file in _extensionHost.FileService.OpenedFiles)
                {
                    if (file.HasUnsavedData)
                        file.Save(_extensionHost.CreateOrGetReporter("Build"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (LiteDevelopSettings.Instance.GetValue<bool>("Projects.ShowOutputWhenBuilding"))
            {
                _outputWindow.ShowAndActivate(_mainDockPanel);
            }

            _extensionHost.CurrentSolution.BuildAsync(_extensionHost.CreateOrGetReporter("Build"));
        }

        private void CurrentSolution_BuildOrCleanStarted(object sender, CancelEventArgs e)
        {
			var solution = sender as Solution;
            if (!(e.Cancel = solution.Builder.IsBusy))
            {
                var errorList = _mainDockPanel.FindContent<ErrorContent>();
                errorList.ClearErrors();

                this.UseWaitCursor = true;
            }
        }
        
        private void CurrentSolution_BuildCompleted(object sender, BuildResultEventArgs e)
        {
            var solution = sender as Solution;

            Invoke(new Action(() =>
            {
                try
                {
                    if (e.Result.Errors.Length != 0)
                    {
                        _errorList.SetErrors(e.Result.Errors);

                        if (LiteDevelopSettings.Instance.GetValue<bool>("Projects.ShowErrorsWhenBuildFailed"))
                        {
                            _errorList.ShowAndActivate(_mainDockPanel);
                        }
                    }

                    if (!e.Result.Success)
                    {
                        MessageBox.Show(LiteDevelopApplication.Current.MuiProcessor.GetString("MainForm.Messages.BuildFailed"), "LiteDevelop", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        switch (_postBuildAction)
                        {
                            case PostBuildAction.None:
                                // do nothing
                                break;
                            case PostBuildAction.Run:
                                solution.Execute();
                                break;
                            case PostBuildAction.Debug:
                                // TODO: use more reliable method of selecting debugger.
                                new Thread(() =>
                                    {
                                        _extensionHost.CurrentDebuggerSession = _extensionHost.ExtensionManager.GetPreferredDebugger(solution.GetFirstExecutableProject()).CreateSession();
                                        _extensionHost.CurrentSolution.Debug(_extensionHost.CurrentDebuggerSession);
                                        Invoke(new Action(() => _extensionHost.DispatchDebugStarted(EventArgs.Empty)));
                                    }).Start();

                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    _postBuildAction = PostBuildAction.None;
                    this.UseWaitCursor = false;
                }
            }));

        }

        private void cleanSolutionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _extensionHost.CurrentSolution.CleanAsync(_extensionHost.CreateOrGetReporter("Build"));
        }

        private void CurrentSolution_CleanCompleted(object sender, BuildResultEventArgs e)
        {
            var solution = sender as Solution;

            if (!e.Result.Success)
            {
                Invoke(new Action(() =>
                {
                    var errorList = _mainDockPanel.FindContent<ErrorContent>();
                    errorList.SetErrors(e.Result.Errors);
                    errorList.Focus();
                }));
            }
            
        }


        #endregion

        #region Debug menu event handlers

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _postBuildAction = PostBuildAction.Debug;
            buildSolutionToolStripMenuItem.PerformClick();
        }

        private void runWithoutDebuggerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _postBuildAction = PostBuildAction.Run;
            buildSolutionToolStripMenuItem.PerformClick();
        }

        private void runLastBuildToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _extensionHost.CurrentSolution.Execute();
        }

        private void stepOverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _extensionHost.CurrentDebuggerSession.StepOver();
        }

        private void stepIntoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _extensionHost.CurrentDebuggerSession.StepInto();
        }

        private void stepOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _extensionHost.CurrentDebuggerSession.StepOut();
        }


        #endregion

        #region Tools menu event handlers

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dlg = new SettingsDialog(LiteDevelopApplication.Current.ExtensionHost.ExtensionManager.LoadedExtensions))
                dlg.ShowDialog();
        }

        private void extensionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dlg = new ExtensionsDialog())
                dlg.ShowDialog();
        }

        #endregion

        #region Window menu event handlers

        private void nextWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var documents = _mainDockPanel.DocumentsToArray();
            int currentIndex = Array.IndexOf(documents, _mainDockPanel.ActiveDocument);

            if (currentIndex != -1)
            {
                currentIndex++;
                if (currentIndex >= documents.Length)
                    currentIndex = 0;

                (documents[currentIndex] as DockContent).ShowAndActivate(_mainDockPanel);
            }
        }

        private void previousWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var documents = _mainDockPanel.DocumentsToArray();
            int currentIndex = Array.IndexOf(documents, _mainDockPanel.ActiveDocument);

            if (currentIndex != -1)
            {
                currentIndex--;
                if (currentIndex < 0)
                    currentIndex = documents.Length - 1;

                (documents[currentIndex] as DockContent).ShowAndActivate(_mainDockPanel);
            }
        }

        private void closeAllDocumentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                LiteDevelopApplication.Current.MuiProcessor.GetString("MainForm.Messages.CloseAllDocumentsWarning"), 
                "LiteDevelop", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
            {
                var documents = _mainDockPanel.DocumentsToArray();

                foreach (var document in documents)
                {
                    document.DockHandler.Close();
                }
            }
        }

        #endregion

        #region Help menu event handlers

        private void sourceCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(LiteDevelopApplication.SourceRepositoryUrl);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dlg = new AboutDialog(LiteDevelopApplication.Current.ExtensionHost.ExtensionManager))
                dlg.ShowDialog();
        }

        #endregion

        #endregion

        #region LiteExtensionHost event handlers

        private void _extensionHost_SolutionLoad(object sender, SolutionEventArgs e)
        {
            _extensionHost.CurrentSolution.BuildStarted += CurrentSolution_BuildOrCleanStarted;
            _extensionHost.CurrentSolution.BuildCompleted += CurrentSolution_BuildCompleted;
            _extensionHost.CurrentSolution.CleanStarted += CurrentSolution_BuildOrCleanStarted;
            _extensionHost.CurrentSolution.CleanCompleted += CurrentSolution_CleanCompleted;
            
            buildSolutionToolStripMenuItem.Enabled =
                buildSolutionStripButton.Enabled =
                cleanSolutionToolStripMenuItem.Enabled =
                runWithoutDebuggerToolStripButton.Enabled =
                runWithoutDebuggerToolStripMenuItem.Enabled =
                runLastBuildToolStripMenuItem.Enabled = true;

            runToolStripButton.Enabled = 
                runToolStripMenuItem.Enabled =
                stepOverToolStripMenuItem.Enabled = 
                stepIntoToolStripMenuItem.Enabled =
                stepOutToolStripMenuItem.Enabled = _extensionHost.CurrentSolution.HasDebuggableProjects(_extensionHost.ExtensionManager);
        }

        private void _extensionHost_SolutionUnload(object sender, SolutionEventArgs e)
        {
            /*
            _extensionHost.CurrentSolution.BuildStarted -= CurrentSolution_BuildOrCleanStarted;
            _extensionHost.CurrentSolution.BuildCompleted -= CurrentSolution_BuildCompleted;
            _extensionHost.CurrentSolution.CleanStarted -= CurrentSolution_BuildOrCleanStarted;
            _extensionHost.CurrentSolution.CleanCompleted -= CurrentSolution_CleanCompleted;
            */

            buildSolutionToolStripMenuItem.Enabled =
                buildSolutionStripButton.Enabled =
                cleanSolutionToolStripMenuItem.Enabled =
                runToolStripButton.Enabled =
                runToolStripMenuItem.Enabled =
                runWithoutDebuggerToolStripButton.Enabled =
                runWithoutDebuggerToolStripMenuItem.Enabled =
                runLastBuildToolStripMenuItem.Enabled = false;
        }

        private void _extensionHost_UILanguageChanged(object sender, EventArgs e)
        {
            LiteDevelopApplication.Current.MuiProcessor.ApplyLanguageOnComponents(_componentMuiIdentifiers);
        }

        private void ControlManager_AppearanceChanged(object sender, EventArgs e)
        {
            var processor = LiteDevelopApplication.Current.ExtensionHost.ControlManager.GlobalAppearanceMap.Processor;
            processor.ApplyAppearanceOnObject(_mainDockPanel, DefaultAppearanceDefinition.Window);
        }

        #endregion

        #region DockPanel event handlers

        private void _mainDockPanel_ActiveContentChanged(object sender, EventArgs e)
        {
            if (_mainDockPanel.GetActiveDocument() != null)
            {
                CurrentDocument = _mainDockPanel.GetActiveDocument().Tag as LiteDocumentContent;
                (_extensionHost.ControlManager as ControlManager).OnSelectedDocumentContentChanged(EventArgs.Empty);
                SetToolBox(CurrentDocument);
                SetPropertyContainer(CurrentDocument);
                ProcessClipboardCommands(CurrentDocument);
                ProcessHistoryCommands(CurrentDocument);
            }
            UpdateCurrentActiveContent();
        }

        private void _mainDockPanel_ContentAdded(object sender, DockContentEventArgs e)
        {
            DockContent content = e.Content as DockContent;
            if (content.Tag is LiteDocumentContent)
            {
                var documentContent = content.Tag as LiteDocumentContent;
                content.FormClosing += content_FormClosing;
                documentContent.Closed += documentContent_Closed;
                documentContent.DragEnter += _mainDockPanel_DragEnter;
                documentContent.DragDrop += _mainDockPanel_DragDrop;
                
                windowListSeparator.Visible = true;
                windowToolStripMenuItem.DropDownItems.Add(
                    new ToolStripMenuItem(documentContent.Text, null,
                        new EventHandler((s, args) =>
                        {
                            _mainDockPanel.GetContent(x => x.Tag == documentContent).ShowAndActivate(_mainDockPanel);
                        })) { Tag = documentContent });
            }
        }

        #endregion

        #region DockContent event handlers

        private void documentContent_Closed(object sender, FormClosedEventArgs e)
        {
            var documentContent = sender as LiteDocumentContent;
            var content = _mainDockPanel.GetContent(x => x.Tag == documentContent);
            content.Tag = null;
            content.Close();

            foreach (ToolStripItem item in windowToolStripMenuItem.DropDownItems)
            {
                if (item.Tag == documentContent)
                {
                    windowToolStripMenuItem.DropDownItems.Remove(item);
                    break;
                }
            }

            windowListSeparator.Visible = _mainDockPanel.DocumentsCount != 0;
        }
      
        private void content_FormClosing(object sender, FormClosingEventArgs e)
        {
            var documentContent = (sender as DockContent).Tag as LiteDocumentContent;
            e.Cancel = documentContent != null;
            if (documentContent != null)
            {
                documentContent.Close();
            }
        }

        #endregion

        #region Other event handlers

        private void _mainDockPanel_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void _mainDockPanel_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (var file in files)
                    UserOpenWith(file);
            }

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            (_extensionHost.ControlManager as ControlManager).NotifyUnsavedFilesWhenClosing = false;

            var unsavedItems = GetUnsavedItems();

            if (unsavedItems.Length > 0)
            {
                using (var dialog = new UnsavedFilesDialog(unsavedItems))
                {
                    switch (dialog.ShowDialog())
                    {
                        case System.Windows.Forms.DialogResult.Yes:
                            foreach (var item in dialog.GetItemsToSave())
                                item.Save(_extensionHost.CreateOrGetReporter("Build"));
                            break;
                        case System.Windows.Forms.DialogResult.No:
                            break;
                        case System.Windows.Forms.DialogResult.Cancel:
                            e.Cancel = true;
                            break;
                    }
                }
            }

            if (!e.Cancel)
            {
                SaveDockPanelState();
                Rectangle usingBounds = (WindowState == FormWindowState.Normal ? this.Bounds : this.RestoreBounds);
                LiteDevelopSettings.Instance.SetValue("MainWindow.Location", usingBounds.Location);
                LiteDevelopSettings.Instance.SetValue("MainWindow.Size", usingBounds.Size);
                LiteDevelopSettings.Instance.SetValue("MainWindow.Maximized", WindowState == FormWindowState.Maximized);
            }

            (_extensionHost.ControlManager as ControlManager).NotifyUnsavedFilesWhenClosing = true;
        }

        private void ErrorManager_NavigateToErrorRequested(object sender, BuildErrorEventArgs e)
        {
            var content = _mainDockPanel.GetContentByFilePath(e.Error.Location.FilePath);
            if (content == null)
            {
                UserOpenWith(e.Error.Location.FilePath);
                content = _mainDockPanel.GetContentByFilePath(e.Error.Location.FilePath);
            }

            if (content != null)
            {
                var documentContent = content.Tag as LiteDocumentContent;
                if (documentContent != null)
                {
                    _mainDockPanel.SetActiveDocument(documentContent);
                    if (documentContent.ParentExtension is ISourceNavigator)
                    {
                        (documentContent.ParentExtension as ISourceNavigator).NavigateToLocation(e.Error.Location);
                    }
                }
            }
        }

        private void toolStripPanel1_Resize(object sender, EventArgs e)
        {
            _mainDockPanel.Top = toolStripPanel1.Bottom;
            _mainDockPanel.Height = mainStatusStrip.Top - toolStripPanel1.Bottom;
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            _mainDockPanel.Height = mainStatusStrip.Top - toolStripPanel1.Bottom;
            _mainDockPanel.Width = ClientSize.Width;
        }

        #endregion


    }

}
