using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using LiteDevelop.Framework;
using LiteDevelop.Framework.Extensions;
using LiteDevelop.Framework.FileSystem;
using LiteDevelop.Framework.FileSystem.Net;
using LiteDevelop.Framework.Gui;
using LiteDevelop.Framework.Languages;
using LiteDevelop.Framework.Languages.Net;
using LiteDevelop.Framework.Languages.Web;
using LiteDevelop.Framework.Mui;
using LiteDevelop.Essentials.CodeEditor.Gui;
using LiteDevelop.Essentials.CodeEditor.Gui.Styles;

namespace LiteDevelop.Essentials.CodeEditor
{
    public class CodeEditorExtension : LiteExtension, IFileHandler, ISourceNavigator, ISettingsProvider, IAppearanceMapProvider
    {
        public static CodeEditorExtension Instance
        {
            get;
            private set;
        }

        public event EventHandler AppliedSettings;
        private Dictionary<OpenedFile, CodeEditorContent> _codeEditors;
        private Dictionary<object, string> _componentMuiIdentifiers;
        private ToolStrip _toolBar;
        private ToolStripStatusLabel _lineLabel;
        private ToolStripStatusLabel _columnLabel;
        private AppearanceMap _appearanceMap;
        private AppearanceMap _defaultAppearanceMap;
        private string _appearanceMapPath;

        public CodeEditorExtension()
        {
            if (Instance != null)
                throw new NotSupportedException("Can only initialize one instance of a CodeEditorExtension");

            Instance = this;
            _codeEditors = new Dictionary<OpenedFile, CodeEditorContent>();
            LastUsedItems = new List<string>();
        }

        #region LiteExtension Members

        public override string Name
        {
            get { return "Code Editor"; }
        }

        public override string Description
        {
            get { return "Default source code editor with syntax highlighting."; }
        }

        public override string Author
        {
            get { return "Jerre S."; }
        }

        public override string ReleaseInformation
        {
            get { return "Special thanks to Pavel Torgashov for developing FastColoredTextBox (https://github.com/PavelTorgashov/FastColoredTextBox)."; }
        }

        public override Version Version
        {
            get { return new Version(0, 9, 0, 0); }
        }

        public override string Copyright
        {
            get { return "Copyright © Jerre S. 2014"; }
        }

        public override void Initialize(ILiteExtensionHost extensionHost)
        {
            ExtensionHost = extensionHost;

            try
            {
                Settings = CodeEditorSettings.LoadSettings(extensionHost.SettingsManager);
            }
            catch
            {
                Settings = CodeEditorSettings.Default.Clone() as CodeEditorSettings;
            }

            SetupGui();
            SetupTemplates();
        }

        public override void Dispose()
        {
            foreach (var keyValuePair in _codeEditors)
                keyValuePair.Value.Close(true);
            Instance = null;
            base.Dispose();
        }

        #endregion

        #region IFileHandler Members

        public bool CanOpenFile(FilePath filePath)
        {
            return true;
        }

        public void OpenFile(OpenedFile file)
        {
            CodeEditorContent documentContent;
            if (!_codeEditors.TryGetValue(file, out documentContent))
            {
                documentContent = new CodeEditorContent(this, file);
                documentContent.Closed += content_Closed;
                _codeEditors.Add(file, documentContent);
                ExtensionHost.ControlManager.OpenDocumentContents.Add(documentContent);
            }

            ExtensionHost.ControlManager.SelectedDocumentContent = documentContent;
        }

        #endregion

        #region ISourceNavigator Members

        public void NavigateToLocation(SourceLocation location)
        {
            foreach (var editor in _codeEditors)
            {
                if (editor.Key.FilePath.Equals(location.FilePath))
                {
                    editor.Value.NavigateToLocation(location);
                }
            }
        }

        #endregion

        #region ISettingsProvider Members

        public SettingsNode RootSettingsNode
        {
            get;
            private set;
        }

        public void ApplySettings()
        {
            RootSettingsNode.ApplySettingsInAllNodes();
            Settings.SaveSettings(ExtensionHost.SettingsManager);
            _appearanceMap.Save(_appearanceMapPath);

            if (AppliedSettings != null)
                AppliedSettings(this, EventArgs.Empty);
        }

        public void LoadUserDefinedPresets()
        {
            RootSettingsNode.LoadUserDefinedPresetsInAllNodes();
        }

        public void ResetSettings()
        {
            Settings = CodeEditorSettings.Default.Clone() as CodeEditorSettings;
            Settings.SaveSettings(ExtensionHost.SettingsManager);

            _defaultAppearanceMap.CopyTo(_appearanceMap);
        }

        public CodeEditorSettings Settings
        {
            get;
            private set;
        }

        #endregion

        #region IAppearanceMapProvider Members

        public AppearanceMap CurrentAppearanceMap
        {
            get { return _appearanceMap; }
        }

        public AppearanceMap DefaultAppearanceMap
        {
            get { return _defaultAppearanceMap; }
        }
        
        #endregion

        public StyleMap StyleMap
        {
            get;
            private set;
        }

        public ILiteExtensionHost ExtensionHost
        {
            get;
            private set;
        }
        
        public MuiProcessor MuiProcessor
        {
            get;
            private set;
        }

        internal List<string> LastUsedItems { get; private set; }
        
        internal void AddLastUsedItem(string item)
        {
            if (LastUsedItems.Count > 0 && LastUsedItems[0] == item)
                return;

            if (LastUsedItems.Contains(item))
                LastUsedItems.Remove(item);

            LastUsedItems.Insert(0, item);

            if (LastUsedItems.Count > 10)
                LastUsedItems.RemoveRange(10, LastUsedItems.Count - 10);
        }

        internal void SetCurrentLocation(int line, int column)
        {
            _lineLabel.Text = MuiProcessor.GetString("CodeEditorExtension.Statusbar.CurrentLine", "line=" + line.ToString());
            _columnLabel.Text = MuiProcessor.GetString("CodeEditorExtension.Statusbar.CurrentColumn", "column=" + column.ToString());
        }

        private void SetupTemplates()
        {
        	// TODO: move to xml files.
            SetupFileTemplates();
            SetupProjectTemplates();
        }

        private void SetupFileTemplates()
        {
            var icon = Properties.Resources.file;
            foreach (var descriptor in LanguageDescriptor.RegisteredLanguages)
            {
                descriptor.Templates.Add(new SourceFileTemplate("Empty File", icon, this, string.Empty));
            }

            LanguageDescriptor.GetLanguage<CSharpLanguage>().Templates.Add(
                new NetAstFileTemplate(
                    "Class File", 
                    null, 
                    Properties.Resources.csharp_file_icon.ToBitmap(),
                    this, 
                    CodeDomUnitFactory.CreateClassUnit("%folder%", "%file%")));

            LanguageDescriptor.GetLanguage<VisualBasicLanguage>().Templates.Add(
                new NetAstFileTemplate(
                    "Class File", 
                    null,
                    Properties.Resources.vb_file_icon.ToBitmap(), 
                    this, 
                    CodeDomUnitFactory.CreateClassUnit("%folder%", "%file%")));

            LanguageDescriptor.GetLanguage<HtmlLanguage>().Templates.Add(
                new SourceFileTemplate(
                    "Web page",
                    Properties.Resources.html_file_icon,
                    this,
                    Properties.Resources.HtmlPageFile,
                    (f) => { f.SetContents(f.GetContentsAsString().Replace("%file%", f.FilePath.FileName)); }));
        }

        private void SetupProjectTemplates()
        {
            var programTemplate = new NetAstFileTemplate("Program", "Program", null, this, CodeDomUnitFactory.CreateEntryPointModuleUnit("%folder%", "Program"));

            LanguageDescriptor.GetLanguage<CSharpLanguage>().Templates.Add(new NetProjectTemplate(
                "Console Application",
                Properties.Resources.console,
                LanguageDescriptor.GetLanguage<CSharpLanguage>(),
                SubSystem.Console,programTemplate));

            LanguageDescriptor.GetLanguage<VisualBasicLanguage>().Templates.Add(new NetProjectTemplate(
                "Console Application",
                Properties.Resources.console,
                LanguageDescriptor.GetLanguage<VisualBasicLanguage>(),
                SubSystem.Console,
                programTemplate));

            programTemplate = new NetAstFileTemplate("Class1", "Class1", null, this, CodeDomUnitFactory.CreateClassUnit("%folder%", "Class1"));

            LanguageDescriptor.GetLanguage<CSharpLanguage>().Templates.Add(new NetProjectTemplate(
                "Class Library",
                Properties.Resources.dll,
                LanguageDescriptor.GetLanguage<CSharpLanguage>(),
                SubSystem.Library, programTemplate));

            LanguageDescriptor.GetLanguage<VisualBasicLanguage>().Templates.Add(new NetProjectTemplate(
                "Class Library",
                Properties.Resources.dll,
                LanguageDescriptor.GetLanguage<VisualBasicLanguage>(),
                SubSystem.Library,
                programTemplate));
        }

        private void SetupGui()
        {
            MuiProcessor = new MuiProcessor(ExtensionHost, Path.Combine(Path.GetDirectoryName(typeof(CodeEditorExtension).Assembly.Location), "MUI"));

            AddToMuiIdentifiers(SetupSettingsControls());
            AddToMuiIdentifiers(SetupToolbar());
            AddStatusBarItems();

            _appearanceMapPath = Path.Combine(ExtensionHost.SettingsManager.GetSettingsDirectory(this), "appearance.xml");
            _defaultAppearanceMap = AppearanceMap.LoadFromFile(Path.Combine(
                    Path.GetDirectoryName(typeof(CodeEditorExtension).Assembly.Location),
                    "CodeEditor", "default_appearance.xml"));

            try { _appearanceMap = AppearanceMap.LoadFromFile(_appearanceMapPath); }
            catch
            {
                _appearanceMap = new AppearanceMap();
                _defaultAppearanceMap.CopyTo(_appearanceMap);
            }

            StyleMap = new Gui.Styles.StyleMap(_appearanceMap, _defaultAppearanceMap);

            ExtensionHost.UILanguageChanged += ExtensionHost_UILanguageChanged;
            ExtensionHost_UILanguageChanged(null, null);
        }

        private void AddToMuiIdentifiers(Dictionary<object, string> dictionary)
        {
            if (_componentMuiIdentifiers == null)
                _componentMuiIdentifiers = new Dictionary<object, string>();

            foreach (var keyPair in dictionary)
                _componentMuiIdentifiers.Add(keyPair.Key, keyPair.Value);
        }

        private Dictionary<object, string> SetupSettingsControls()
        {
            var generalSettingsControl = new GeneralSettingsControl(ExtensionHost, MuiProcessor, Settings) { Dock = DockStyle.Fill };
            var generalSettingsNode = new SettingsNode("General", generalSettingsControl);
            var autoCompleteSettingsControl = new AutoCompleteSettingsControl(ExtensionHost, MuiProcessor, Settings) { Dock = DockStyle.Fill };
            var autoCompleteSettingsNode = new SettingsNode("Auto completion", autoCompleteSettingsControl);

            RootSettingsNode = new SettingsNode("Code Editor", generalSettingsControl);
            RootSettingsNode.Nodes.AddRange(new TreeNode[] { generalSettingsNode, autoCompleteSettingsNode });

            return new Dictionary<object, string>()
            {
                {generalSettingsNode, "CodeEditorExtension.GeneralSettings"},
                {autoCompleteSettingsNode, "CodeEditorExtension.AutoCompleteSettings"},
            };
        }

        private Dictionary<object, string> SetupToolbar()
        {
            _toolBar = new ToolStrip();
            _toolBar.Renderer = ExtensionHost.ControlManager.MenuRenderer;
            _toolBar.Visible = false;
            _toolBar.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;

            var showSuggestionsToolStripButton = CreateToolBarButton(Properties.Resources.list, showSuggestionsToolStripButton_Click);
            var commentRegionToolStripButton = CreateToolBarButton(Properties.Resources.comment_accept, commentRegionToolStripButton_Click);
            var uncommentRegionToolStripButton = CreateToolBarButton(Properties.Resources.comment_cancel, uncommentRegionToolStripButton_Click);

            _toolBar.Items.AddRange(new ToolStripItem[]
            {
                showSuggestionsToolStripButton,
                new ToolStripSeparator(),
                commentRegionToolStripButton,
                uncommentRegionToolStripButton
            });

            ExtensionHost.ControlManager.ToolBars.Add(_toolBar);
            ExtensionHost.ControlManager.SelectedDocumentContentChanged += ControlManager_SelectedDocumentContentChanged;

            return new Dictionary<object, string>()
            {
                {showSuggestionsToolStripButton, "CodeEditorExtension.Toolbar.ShowSuggestions"},
                {commentRegionToolStripButton, "CodeEditorExtension.Toolbar.CommentRegion"},
                {uncommentRegionToolStripButton, "CodeEditorExtension.Toolbar.UncommentRegion"},
            };
        }

        private ToolStripButton CreateToolBarButton(Image image, EventHandler onClick)
        {
            var button = new ToolStripButton(string.Empty, image, onClick);
            button.DisplayStyle = ToolStripItemDisplayStyle.Image;
            return button;
        }

        private void AddStatusBarItems()
        {
            _lineLabel = CreateStatusLabel(100);
            _columnLabel = CreateStatusLabel(100);

            ExtensionHost.ControlManager.StatusBarItems.AddRange(new ToolStripItem[]
                {
                    _lineLabel,
                    _columnLabel
                });
        }

        private ToolStripStatusLabel CreateStatusLabel()
        {
            return CreateStatusLabel(0);
        }

        private ToolStripStatusLabel CreateStatusLabel(int width)
        {
            var item = new ToolStripStatusLabel();
            item.TextAlign = ContentAlignment.MiddleLeft;
            item.AutoSize = width == 0;
            item.Width = width;
            item.Visible = false;
            return item;
        }

        private void ControlManager_SelectedDocumentContentChanged(object sender, EventArgs e)
        {
            _toolBar.Visible = 
                _lineLabel.Visible = 
                _columnLabel.Visible = ExtensionHost.ControlManager.SelectedDocumentContent is CodeEditorContent;
        }

        private void showSuggestionsToolStripButton_Click(object sender, EventArgs e)
        {
            var editorControl = GetCurrentTextBox();
            if (editorControl != null)
                editorControl.ForceShowSuggestions();
        }

        private void commentRegionToolStripButton_Click(object sender, EventArgs e)
        {
            var editorControl = GetCurrentTextBox();
            if (editorControl != null)
                editorControl.CommentSelected();
        }

        private void uncommentRegionToolStripButton_Click(object sender, EventArgs e)
        {
            var editorControl = GetCurrentTextBox();
            if (editorControl != null)
                editorControl.UncommentSelected();
        }

        private CodeEditorControl GetCurrentTextBox()
        {
            var selectedContent = ExtensionHost.ControlManager.SelectedDocumentContent as CodeEditorContent;
            if (selectedContent != null)
                return (selectedContent.Control as CodeEditorControl);
            return null;
        }
        
        private void content_Closed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            if (_codeEditors.ContainsValue(sender as CodeEditorContent))
            {
                _codeEditors.Remove((sender as LiteDocumentContent).AssociatedFile);
            }
        }

        private void ExtensionHost_UILanguageChanged(object sender, EventArgs e)
        {
            MuiProcessor.ApplyLanguageOnComponents(_componentMuiIdentifiers);
        }

    }
}
