using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LiteDevelop.Framework;
using LiteDevelop.Framework.FileSystem;

namespace LiteDevelop.Gui.DockContents.SolutionExplorer
{
    public class DirectoryNode : PathNode
    {
        private readonly IconProvider _iconProvider;
        private readonly Project _project;

        public DirectoryNode(Project project, FilePath path, IconProvider iconProvider)
            : base(path)
        {
            if (project != null)
            {
                _project = project;

                project.ProjectFiles.InsertedItem += ProjectFiles_InsertedItem;
                project.ProjectFiles.RemovedItem += ProjectFiles_RemovedItem;
            }

            AddStub();
            ImageIndex = SelectedImageIndex = (_iconProvider = iconProvider).GetImageIndex(new DirectoryInfo(path.FileName));
        }

        public override bool CanAddFiles
        {
            get { return true; }
        }

        public override bool CanAddDirectories
        {
            get { return true; }
        }

        public override bool CanAddProjects
        {
            get { return false; }
        }

        public override bool CanRename
        {
            get { return true; }
        }

        public override bool CanDelete
        {
            get { return true; }
        }

        public override bool CanActivate
        {
            get { return false; }
        }

        public override void OnSelect()
        {
            Populate();
        }

        private void AddStub()
        {
            Nodes.Add(new TreeNode());
        }

        protected virtual void Populate()
        {
            if (this.Nodes.Count == 1 && !(this.Nodes[0] is AbstractNode))
            {
                this.Nodes.Clear();

                var project = GetProjectNode();

                if (project != null && project.ProjectEntry != null)
                {
                    if (project.ProjectEntry.HasProject)
                    {
                        string[] directories = project.ProjectEntry.Project.GetDirectoriesInDirectory(FilePath.FullPath, false);
                        Array.Sort(directories);
                        foreach (var directory in directories)
                        {
                            this.Nodes.Add(new DirectoryNode(_project, new FilePath(FilePath, directory), _iconProvider));
                        }

                        var files = project.ProjectEntry.Project.GetProjectFilesInDirectory(FilePath.FullPath, false);
                        Array.Sort(files, (x, y) => x.FilePath.FileName.CompareTo(y.FilePath.FileName));
                        foreach (var file in files)
                        {
                            this.Nodes.Add(new FileNode(file, _iconProvider));
                        }
                    }
                }
            }
        }

        public override void RenameEntry(string newName)
        {
            var extensionHost = LiteDevelopApplication.Current.ExtensionHost;
            var newPath = FilePath.ChangeName(newName);
            extensionHost.FileService.MoveDirectory(FilePath, newPath);
            FilePath = newPath;
        }

        public override void DeleteEntry()
        {
            var extensionHost = LiteDevelopApplication.Current.ExtensionHost;
            _project.ProjectFiles.InsertedItem -= ProjectFiles_InsertedItem;
            _project.ProjectFiles.RemovedItem -= ProjectFiles_RemovedItem;

            // delete sub nodes.
            int minimum = 0;
            while (Nodes.Count != minimum)
            {
                if (Nodes[0] is PathNode)
                    (Nodes[0] as PathNode).DeleteEntry();
                else
                    minimum++;
            }

            extensionHost.FileService.DeleteDirectory(FilePath);

            this.Remove();
        }

        protected override void OnFilePathChanged(PathChangedEventArgs e)
        {
            if (_project != null)
            {
                // update files
                foreach (var file in _project.GetProjectFilesInDirectory(e.SourcePath.FullPath, false))
                {
                    string hintPath = file.FilePath.GetRelativePath(e.SourcePath);
                    file.FilePath = new FilePath(e.NewPath, hintPath);
                }

                // update sub folders
                foreach (var subNode in Nodes)
                {
                    if (subNode is DirectoryNode)
                    {
                        var subDirectoryNode = (subNode as DirectoryNode);
                        string hintPath = subDirectoryNode.FilePath.GetRelativePath(e.SourcePath);
                        subDirectoryNode.FilePath = new FilePath(e.NewPath, hintPath);
                    }
                }
            }

            base.OnFilePathChanged(e);
        }

        private void ProjectFiles_InsertedItem(object sender, CollectionChangedEventArgs e)
        {
            var file = e.TargetObject as ProjectFileEntry;

            if (file.FilePath.ParentDirectory == this.FilePath)
                this.Nodes.Add(new FileNode(file, _iconProvider));
        }

        private void ProjectFiles_RemovedItem(object sender, CollectionChangedEventArgs e)
        {
            var file = e.TargetObject as ProjectFileEntry;

            if (file.FilePath.ParentDirectory == this.FilePath)
            {
                foreach (TreeNode node in Nodes)
                {
                    if (node is FileNode && (node as FileNode).FilePath == file.FilePath)
                    {
                        node.Remove();
                        break;
                    }
                }
            }
        }

    }
}
