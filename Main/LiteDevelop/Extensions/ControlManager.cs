using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LiteDevelop.Framework;
using LiteDevelop.Framework.Extensions;
using LiteDevelop.Framework.FileSystem;
using LiteDevelop.Framework.Gui;
using LiteDevelop.Gui;
using LiteDevelop.Gui.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace LiteDevelop.Extensions
{
    public class ControlManager : IControlManager
    {
        private EventBasedCollection<LiteDocumentContent> _documentContents;
        private EventBasedCollection<LiteToolWindow>_toolWindows;
        private EventBasedCollection<ToolStrip> _toolBars;
        private EventBasedCollection<ToolStripMenuItem> _menuItems;
        private EventBasedCollection<ToolStripItem> _editItems;
        private EventBasedCollection<ToolStripItem> _viewItems;
        private EventBasedCollection<ToolStripItem> _debugItems;
        private EventBasedCollection<ToolStripItem> _toolsItems;
        private EventBasedCollection<ToolStripItem> _statusStripItems;
        private EventBasedCollection<ToolStripItem> _solutionMenuItems;

        
        private readonly ToolStripAeroRenderer _renderer = new ToolStripAeroRenderer(ToolbarTheme.Toolbar);
        private readonly ILiteExtensionHost _extensionHost;
        private readonly Dictionary<LiteDocumentContent, OpenedFile> _openedFiles = new Dictionary<LiteDocumentContent, OpenedFile>();

        public ControlManager(ILiteExtensionHost extensionHost)
        {
            _extensionHost = extensionHost;

            _documentContents = new EventBasedCollection<LiteDocumentContent>();
            _documentContents.InsertedItem += viewContent_InsertedItem;
            _documentContents.RemovedItem += viewContent_RemovedItem;

            _toolWindows = new EventBasedCollection<LiteToolWindow>();
            _toolWindows.InsertedItem += viewContent_InsertedItem;
            _toolWindows.RemovedItem += viewContent_RemovedItem;

            _toolBars = new EventBasedCollection<ToolStrip>();
            _toolBars.InsertedItem += toolBars_InsertedItem;
            _toolBars.RemovedItem += toolBars_RemovedItem;

            _menuItems = new EventBasedCollection<ToolStripMenuItem>();
            _menuItems.InsertedItem += menuItems_InsertedItem;
            _menuItems.RemovedItem += menuItems_RemovedItem;

            _editItems = new EventBasedCollection<ToolStripItem>();
            _editItems.InsertedItem += editItems_InsertedItem;
            _editItems.RemovedItem += editItems_RemovedItem;

            _viewItems = new EventBasedCollection<ToolStripItem>();
            _viewItems.InsertedItem += viewItems_InsertedItem;
            _viewItems.RemovedItem += viewItems_RemovedItem;

            _debugItems = new EventBasedCollection<ToolStripItem>();
            _debugItems.InsertedItem += debugItems_InsertedItem;
            _debugItems.RemovedItem += debugItems_RemovedItem;

            _toolsItems = new EventBasedCollection<ToolStripItem>();
            _toolsItems.InsertedItem += toolsItems_InsertedItem;
            _toolsItems.RemovedItem += toolsItems_RemovedItem;

            _statusStripItems = new EventBasedCollection<ToolStripItem>();
            _statusStripItems.InsertedItem += _statusStripItems_InsertedItem;
            _statusStripItems.RemovedItem += _statusStripItems_RemovedItem;

            _solutionMenuItems = new EventBasedCollection<ToolStripItem>();
            _solutionMenuItems.InsertedItem += solutionMenuItems_InsertedItem;
            _solutionMenuItems.RemovedItem += solutionMenuItems_RemovedItem;

            NotifyUnsavedFilesWhenClosing = true;
        }

        #region IControlManager Members

        public EventBasedCollection<LiteDocumentContent> OpenDocumentContents
        {
            get { return _documentContents; }
        }

        public LiteDocumentContent SelectedDocumentContent
        {
            get 
            { 
                var selectedContent = DockPanel.GetActiveDocument();

                if (selectedContent != null)
                    return selectedContent.Tag as LiteDocumentContent; 
                return null;
            }
            set 
            {
                DockPanel.SetActiveDocument(value);
            }
        }


        public event EventHandler SelectedDocumentContentChanged;

        public EventBasedCollection<LiteToolWindow> ToolWindows
        {
            get { return _toolWindows; }
        }
        
        public EventBasedCollection<ToolStrip> ToolBars
        {
            get { return _toolBars; }
        }

        public EventBasedCollection<ToolStripMenuItem> MenuItems
        {
            get { return _menuItems; }
        }

        public EventBasedCollection<ToolStripItem> EditMenuItems
        {
            get { return _editItems; }
        }

        public EventBasedCollection<ToolStripItem> ViewMenuItems
        {
            get { return _viewItems; }
        }

        public EventBasedCollection<ToolStripItem> DebugMenuItems
        {
            get { return _debugItems; }
        }

        public EventBasedCollection<ToolStripItem> ToolsMenuItems
        {
            get { return _toolsItems; }
        }

        public EventBasedCollection<ToolStripItem> StatusBarItems
        {
            get { return _statusStripItems; }
        }

        public EventBasedCollection<ToolStripItem> SolutionExplorerItems
        {
            get { return _solutionMenuItems; }
        }

        public AppearanceMap GlobalAppearanceMap
        {
            get { return _extensionHost.ExtensionManager.GetLoadedExtension<LiteDevelopExtension>().CurrentAppearanceMap; }
        }

        public ToolStripRenderer MenuRenderer
        {
            get { return _renderer; }
        }

        public event EventHandler AppearanceChanged;

        #endregion

        #region Components

        public DockPanel DockPanel
        {
            get;
            internal set;
        }


        public ToolStripPanel ToolStripPanel
        {
            get;
            internal set;
        }

        public MenuStrip MenuStrip
        {
            get;
            internal set;
        }

        public ToolStripMenuItem EditMenu
        {
            get;
            internal set;
        }

        public ToolStripMenuItem ViewMenu
        {
            get;
            internal set;
        }

        public ToolStripMenuItem DebugMenu
        {
            get;
            internal set;
        }

        public ToolStripMenuItem ToolsMenu
        {
            get;
            internal set;
        }

        public StatusStrip StatusStrip
        {
            get;
            internal set;
        }

        public ContextMenuStrip SolutionExplorerMenu
        {
            get;
            internal set;
        }

        #endregion

        public bool NotifyUnsavedFilesWhenClosing
        {
            get;
            set;
        }
        
        private DockContent FindDockContent(LiteViewContent documentContent)
        {
            foreach (DockContent document in DockPanel.DocumentsToArray())
            {
                if (document.Tag == documentContent)
                    return document;
            }
            return null;
        }

        private void SetDockContentText(LiteViewContent content)
        {
            var dockContent = FindDockContent(content);
            if (dockContent != null)
            {
                string text = content.Text;

                if (content is LiteDocumentContent)
                {
                    var documentContent = content as LiteDocumentContent;
                    text += (documentContent.AssociatedFile != null ? (documentContent.AssociatedFile.HasUnsavedData ? "*" : "") : "");
                }

                dockContent.Text = text;
            }
        }

        internal virtual void OnSelectedDocumentContentChanged(EventArgs e)
        {
            if (SelectedDocumentContentChanged != null)
                SelectedDocumentContentChanged(this, e);
        }

        internal virtual void OnAppearanceChanged(EventArgs e)
        {
            if (AppearanceChanged != null)
                AppearanceChanged(this, e);
        }

        private void viewContent_InsertedItem(object sender, CollectionChangedEventArgs e)
        {
            LiteViewContent viewContent = e.TargetObject as LiteViewContent;
            DockContent dockContent = new DockContent() 
            {
                Text = viewContent.Text,
                Tag = viewContent,
                AllowDrop = true,
            };

            dockContent.Controls.Add(viewContent.Control);

            viewContent.TextChanged += viewContent_TextChanged;
            viewContent.ControlChanged += viewContent_ControlChanged;
            
            if (viewContent is LiteDocumentContent)
            {
                var documentContent = viewContent as LiteDocumentContent;
                dockContent.DockHandler.DockAreas = DockAreas.Document | DockAreas.Float;

                if (documentContent.AssociatedFile != null)
                {
                    documentContent.AssociatedFile.HasUnsavedDataChanged += AssociatedFile_HasUnsavedDataChanged;
                }

                documentContent.AssociatedFileChanged += documentContent_AssociatedFileChanged;

                _openedFiles.Add(documentContent, documentContent.AssociatedFile);
            }

            viewContent.Closing += viewContent_Closing;
            
            dockContent.Show(DockPanel);
            SetDockContentText(viewContent);            
        }

        private void viewContent_RemovedItem(object sender, CollectionChangedEventArgs e)
        {
            LiteDocumentContent documentContent = e.TargetObject as LiteDocumentContent;
            DockContent dockContent = FindDockContent(documentContent);

            if (dockContent == null)
                throw new ArgumentException("Document content is not found");

            documentContent.TextChanged -= viewContent_TextChanged;
            documentContent.ControlChanged -= viewContent_ControlChanged;
            if (_openedFiles[documentContent] != null)
            {
                _openedFiles[documentContent].HasUnsavedDataChanged -= AssociatedFile_HasUnsavedDataChanged;
            }
            documentContent.AssociatedFileChanged -= documentContent_AssociatedFileChanged;
            documentContent.Closing -= viewContent_Closing;
            dockContent.DockHandler.Close();
        }

        private void viewContent_Closing(object sender, FormClosingEventArgs e)
        {
            var documentContent = sender as LiteDocumentContent;
            if (NotifyUnsavedFilesWhenClosing && documentContent != null && documentContent.AssociatedFile != null)
            {
                if (documentContent.AssociatedFile.HasUnsavedData)
                {
                    var dialog = new UnsavedFilesDialog(new OpenedFile[1] { documentContent.AssociatedFile });
                    switch (dialog.ShowDialog())
                    {
                        case DialogResult.Yes:
                            dialog.GetItemsToSave()[0].Save(_extensionHost.CreateOrGetReporter("Build"));
                            break;
                        case DialogResult.Cancel:
                            e.Cancel = true;
                            break;
                    }
                }
            }
        }

        private void viewContent_TextChanged(object sender, EventArgs e)
        {
            SetDockContentText(sender as LiteViewContent);
        }

        private void viewContent_ControlChanged(object sender, EventArgs e)
        {
            DockContent content = FindDockContent(sender as LiteDocumentContent);
            if (content != null)
            {
                content.Controls.Clear();
                content.Controls.Add((sender as LiteDocumentContent).Control);
            }
        }

        private void documentContent_AssociatedFileChanged(object sender, EventArgs e)
        {
            var documentContent = sender as LiteDocumentContent;
            var oldFile = _openedFiles[documentContent];
            if (oldFile != null)
                oldFile.HasUnsavedDataChanged -= AssociatedFile_HasUnsavedDataChanged;
            var newFile = documentContent.AssociatedFile;
            _openedFiles[documentContent] = newFile;
            if (newFile != null)
            {
                newFile.HasUnsavedDataChanged += AssociatedFile_HasUnsavedDataChanged;
            }
        }

        private void AssociatedFile_HasUnsavedDataChanged(object sender, EventArgs e)
        {
            var keyPair = _openedFiles.FirstOrDefault(x => x.Value == sender);
            if (keyPair.Key != null)
                SetDockContentText(keyPair.Key);
        }

        private void toolBars_InsertedItem(object sender, CollectionChangedEventArgs e)
        {
            var toolstrip = e.TargetObject as ToolStrip;
            var lastRow = ToolStripPanel.Rows[ToolStripPanel.Rows.Length - 1];

            Point nextControlLocation = lastRow.DisplayRectangle.Location;

            foreach (var toolbar in lastRow.Controls)
            {
                nextControlLocation.Offset(toolbar.Margin.Left, toolbar.Margin.Top);
                nextControlLocation.X += toolbar.Width + toolbar.Margin.Right + ToolStripPanel.Padding.Horizontal;
            }

            ToolStripPanel.Join(toolstrip, nextControlLocation);
        }

        private void toolBars_RemovedItem(object sender, CollectionChangedEventArgs e)
        {
            ToolStripPanel.Controls.Remove(e.TargetObject as ToolStrip);
        }

        private void menuItems_InsertedItem(object sender, CollectionChangedEventArgs e)
        {
            // insert before 'Window' and 'Help' menu items.
            MenuStrip.Items.Insert(MenuStrip.Items.Count - 2, e.TargetObject as ToolStripMenuItem);
        }

        private void menuItems_RemovedItem(object sender, CollectionChangedEventArgs e)
        {
            MenuStrip.Items.Remove(e.TargetObject as ToolStripItem);
        }

        private void editItems_InsertedItem(object sender, CollectionChangedEventArgs e)
        {
            EditMenu.DropDownItems.Add(e.TargetObject as ToolStripItem);
        }

        private void editItems_RemovedItem(object sender, CollectionChangedEventArgs e)
        {
            EditMenu.DropDownItems.Remove(e.TargetObject as ToolStripItem);
        }

        private void viewItems_InsertedItem(object sender, CollectionChangedEventArgs e)
        {
            ViewMenu.DropDownItems.Add(e.TargetObject as ToolStripItem);
        }

        private void viewItems_RemovedItem(object sender, CollectionChangedEventArgs e)
        {
            ViewMenu.DropDownItems.Remove(e.TargetObject as ToolStripItem);
        }

        private void debugItems_InsertedItem(object sender, CollectionChangedEventArgs e)
        {
            DebugMenu.DropDownItems.Add(e.TargetObject as ToolStripItem);
        }

        private void debugItems_RemovedItem(object sender, CollectionChangedEventArgs e)
        {
            DebugMenu.DropDownItems.Remove(e.TargetObject as ToolStripItem);
        }

        private void toolsItems_InsertedItem(object sender, CollectionChangedEventArgs e)
        {
            ToolsMenu.DropDownItems.Add(e.TargetObject as ToolStripItem);
        }

        private void toolsItems_RemovedItem(object sender, CollectionChangedEventArgs e)
        {
            ToolsMenu.DropDownItems.Remove(e.TargetObject as ToolStripItem);
        }

        private void _statusStripItems_InsertedItem(object sender, CollectionChangedEventArgs e)
        {
            StatusStrip.Items.Add(e.TargetObject as ToolStripItem);
        }

        private void _statusStripItems_RemovedItem(object sender, CollectionChangedEventArgs e)
        {
            StatusStrip.Items.Remove(e.TargetObject as ToolStripItem);
        }

        private void solutionMenuItems_RemovedItem(object sender, CollectionChangedEventArgs e)
        {
            SolutionExplorerMenu.Items.Remove((ToolStripItem)e.TargetObject);
        }

        private void solutionMenuItems_InsertedItem(object sender, CollectionChangedEventArgs e)
        {
            SolutionExplorerMenu.Items.Add((ToolStripItem)e.TargetObject);
        }
    }
}
