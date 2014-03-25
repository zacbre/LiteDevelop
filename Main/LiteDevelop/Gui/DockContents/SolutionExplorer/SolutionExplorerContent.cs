using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LiteDevelop.Framework;
using LiteDevelop.Framework.FileSystem;
using WeifenLuo.WinFormsUI.Docking;
using LiteDevelop.Extensions;
using LiteDevelop.Gui.Forms;

namespace LiteDevelop.Gui.DockContents.SolutionExplorer
{
    public partial class SolutionExplorerContent : DockContent
    {
        private Dictionary<object, string> _componentMuiIdentifiers;
        private IconProvider _iconProvider;
        private LiteExtensionHost _extensionHost;
        private Solution _solution;

        public SolutionExplorerContent()
        {
            InitializeComponent();

            this.HideOnClose = true;
            this.Icon = Icon.FromHandle(Properties.Resources.folder.GetHicon());

            _iconProvider = IconProvider.GetProvider<SolutionExplorerIconProvider>();
            mainTreeView.ImageList = _iconProvider.ImageList;

            LiteDevelopApplication.Current.InitializedApplication += Current_InitializedApplication;
        }

        private void Current_InitializedApplication(object sender, EventArgs e)
        {
            _componentMuiIdentifiers = new Dictionary<object, string>()
            {
                {this, "SolutionExplorerContent.Title"},
                {this.addToolStripMenuItem, "SolutionExplorerContent.ContextMenu.Add"},
                {this.newFileToolStripMenuItem, "SolutionExplorerContent.ContextMenu.Add.NewFile"},
                {this.newDirectoryToolStripMenuItem, "SolutionExplorerContent.ContextMenu.Add.NewDirectory"},
                {this.newProjectToolStripMenuItem, "SolutionExplorerContent.ContextMenu.Add.NewProject"},
                {this.existingFileToolStripMenuItem, "SolutionExplorerContent.ContextMenu.Add.ExistingFile"},
                {this.openToolStripMenuItem, "SolutionExplorerContent.ContextMenu.Open"},
                {this.removeToolStripMenuItem, "SolutionExplorerContent.ContextMenu.Remove"},
                {this.renameToolStripMenuItem, "SolutionExplorerContent.ContextMenu.Rename"},
                {this.setAsStartupProjectToolStripMenuItem, "SolutionExplorerContent.ContextMenu.SetAsStartupProject"},
                {this.viewInExplorerToolStripMenuItem, "SolutionExplorerContent.ContextMenu.ViewInExplorer"},
            };

            _extensionHost = LiteDevelopApplication.Current.ExtensionHost;

            _extensionHost.UILanguageChanged += _extensionHost_UILanguageChanged;
            _extensionHost.SolutionLoad += _extensionHost_SolutionLoad;
            _extensionHost.SolutionUnload += _extensionHost_SolutionUnload;
            _extensionHost.ControlManager.AppearanceChanged += ControlManager_AppearanceChanged;
            _extensionHost_UILanguageChanged(null, null);
        }

        private void _extensionHost_UILanguageChanged(object sender, EventArgs e)
        {
            LiteDevelopApplication.Current.MuiProcessor.ApplyLanguageOnComponents(_componentMuiIdentifiers);
        }
        
        private DirectoryNode GetCurrentDirectoryNode()
        {
            DirectoryNode directoryNode = mainTreeView.SelectedNode as DirectoryNode;

            if (directoryNode == null)
                directoryNode = mainTreeView.SelectedNode.Parent as DirectoryNode;

            return directoryNode;
        }

        private void _extensionHost_SolutionLoad(object sender, SolutionEventArgs e)
        {
            _solution = _extensionHost.CurrentSolution;
            _solution.Settings.StartupProjects.InsertedItem += StartupProjects_Changed;
            _solution.Settings.StartupProjects.RemovedItem += StartupProjects_Changed;
            contextMenuStrip1.Renderer = _extensionHost.ControlManager.MenuRenderer;
            mainTreeView.Nodes.Add(new SolutionNode(_solution, _iconProvider));
        }

        private void ControlManager_AppearanceChanged(object sender, EventArgs e)
        {
            var processor = LiteDevelopApplication.Current.ExtensionHost.ControlManager.GlobalAppearanceMap.Processor;
            processor.ApplyAppearanceOnObject(this, Framework.Gui.DefaultAppearanceDefinition.Window);
            processor.ApplyAppearanceOnObject(mainTreeView, Framework.Gui.DefaultAppearanceDefinition.TreeView);
        }

        private void StartupProjects_Changed(object sender, CollectionChangedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => StartupProjects_Changed(sender, e)));
                return;
            }

            mainTreeView.Update();
        }

        private void _extensionHost_SolutionUnload(object sender, SolutionEventArgs e)
        {
            _solution = null;
            mainTreeView.Nodes.Clear();
        }

        private void mainTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node is AbstractNode)
            {
                (e.Node as AbstractNode).OnSelect();
            }
        }

        private void mainTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // select item even on right click
            mainTreeView.SelectedNode = e.Node;
        }

        private void mainTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node is AbstractNode)
            {
                (e.Node as AbstractNode).Activate();
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mainTreeView.SelectedNode is AbstractNode)
            {
                (mainTreeView.SelectedNode as AbstractNode).Activate();
            }
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mainTreeView.SelectedNode is PathNode)
            {
                var node = (mainTreeView.SelectedNode as PathNode);
                if (MessageBox.Show(LiteDevelopApplication.Current.MuiProcessor.GetString("SolutionExplorerContent.DeleteFileWarning", "file=" + node.FilePath.FullPath), "LiteDevelop", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        node.DeleteEntry();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainTreeView.SelectedNode.BeginEdit();
        }

        private void mainTreeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Label))
            {
                try
                {
                    if (e.Node is PathNode)
                    {
                        (e.Node as PathNode).RenameEntry(e.Label);
                    }
                    else
                    {
                        e.CancelEdit = true;
                    }
                }
                catch (Exception ex)
                {
                    e.CancelEdit = true;
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                e.CancelEdit = true;
            }
        }

        private void newFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mainTreeView.SelectedNode != null)
            {
                DirectoryNode directoryNode = GetCurrentDirectoryNode();

                CreateFileDialog.UserCreateFile(_extensionHost, directoryNode.GetProjectNode().ProjectEntry.Project, directoryNode.FilePath.FullPath);
                directoryNode.Expand();
            }
        }

        private void newDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mainTreeView.SelectedNode != null)
            {
                DirectoryNode directoryNode = GetCurrentDirectoryNode();

                var folderPath = directoryNode.FilePath.Combine("New Folder");
                _extensionHost.FileService.CreateDirectory(folderPath);
                var node = new DirectoryNode(directoryNode.GetProjectNode().ProjectEntry.Project, folderPath, _iconProvider);
                directoryNode.Nodes.Add(node);
                directoryNode.Expand();
                node.BeginEdit();
            }
        }

        private void newProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateProjectDialog.UserCreateProject(_extensionHost);
        }

        private void existingFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var directoryNode = GetCurrentDirectoryNode();
                    var projectNode = directoryNode.GetProjectNode();
                    var sourcePath = new FilePath(dialog.FileName);
                    var newPath = new FilePath(directoryNode.FilePath, sourcePath.FileName + sourcePath.Extension);
                    _extensionHost.FileService.CopyFile(sourcePath, newPath);
                    var fileEntry = new ProjectFileEntry(newPath);
                    projectNode.ProjectEntry.Project.ProjectFiles.Add(fileEntry);
                }
            }
        }
        
        private void setAsStartupProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainTreeView.BeginUpdate();
            var projectNode = GetCurrentDirectoryNode().GetProjectNode();
            _solution.Settings.StartupProjects.Clear();
            _solution.Settings.StartupProjects.Add(projectNode.ProjectEntry.ObjectGuid);
            mainTreeView.EndUpdate();
        }

        private void viewInExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var pathNode = mainTreeView.SelectedNode as PathNode;
            if (pathNode != null)
                Process.Start("explorer.exe", string.Format("/select,\"{0}\"", pathNode.FilePath.FullPath));
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            var abstractNode = (mainTreeView.SelectedNode as AbstractNode);
            openToolStripMenuItem.Visible = abstractNode.CanActivate;
            removeToolStripMenuItem.Visible = abstractNode.CanDelete;
            renameToolStripMenuItem.Visible = abstractNode.CanRename;
            addToolStripMenuItem.Visible = abstractNode.CanAddDirectories || abstractNode.CanAddFiles || abstractNode.CanAddProjects;
            existingFileToolStripMenuItem.Visible = existingItemSeparator.Visible = newFileToolStripMenuItem.Visible = abstractNode.CanAddFiles;
            newDirectoryToolStripMenuItem.Visible = abstractNode.CanAddDirectories;
            newProjectToolStripMenuItem.Visible = abstractNode.CanAddProjects;

            viewInExplorerToolStripMenuItem.Visible = explorerSeparator.Visible = abstractNode is IFilePathProvider;
            setAsStartupProjectToolStripMenuItem.Visible = startupProjectSeparator.Visible = abstractNode is ProjectNode;
        }

    }

}
