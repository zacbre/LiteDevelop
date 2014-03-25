using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LiteDevelop.Framework;
using LiteDevelop.Framework.FileSystem;

namespace LiteDevelop.Gui.DockContents.SolutionExplorer
{
    public class ProjectNode : DirectoryNode 
    {
        private readonly IconProvider _iconProvider;
        private readonly PropertiesNode _propertiesNode;
        private readonly ReferencesNode _referencesNode;

        public ProjectNode(ProjectEntry project, IconProvider iconProvider)
            : base(project.Project, project.FilePath.ParentDirectory, iconProvider)
        {
            ProjectEntry = project;
            ProjectEntry.LoadComplete += project_LoadComplete;

            var solution = ProjectEntry.GetRoot() as Solution;
            if (solution != null)
            { 
                solution.Settings.StartupProjects.InsertedItem += StartupProjects_InsertedItem;
                solution.Settings.StartupProjects.RemovedItem += StartupProjects_RemovedItem;
            }

            if (project.HasProject)
            {
                ImageIndex = SelectedImageIndex = (_iconProvider = iconProvider).GetImageIndex(project);

                _propertiesNode = new PropertiesNode(iconProvider);

                if (project.Project is IFileReferenceProvider)
                    _referencesNode = new ReferencesNode(project.Project as IFileReferenceProvider, iconProvider);
            }

            if (project.IsLoaded)
                Populate();
        }

        public ProjectEntry ProjectEntry
        {
            get;
            private set;
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

        public override void RenameEntry(string newName)
        {
            base.RenameEntry(newName);
            this.ProjectEntry.Name = newName;
            var oldPath = this.ProjectEntry.FilePath.ParentDirectory.ChangeName(newName).Combine(this.ProjectEntry.FilePath.FileName + this.ProjectEntry.FilePath.Extension);
            this.ProjectEntry.FilePath = this.ProjectEntry.FilePath.ParentDirectory.ChangeName(newName).Combine(newName + this.ProjectEntry.FilePath.Extension);
            LiteDevelopApplication.Current.ExtensionHost.FileService.MoveFile(oldPath, this.ProjectEntry.FilePath);
        }

        public override void DeleteEntry()
        {
            base.DeleteEntry();
            this.ProjectEntry.Parent.Nodes.Remove(this.ProjectEntry);
        }

        private void project_LoadComplete(object sender, SolutionNodeLoadEventArgs e)
        {
            Populate();
        }

        protected override void Populate()
        {
            base.Populate();

            if (!Nodes.Contains(_propertiesNode))
            {
                if (_referencesNode != null)
                    this.Nodes.Insert(0, _referencesNode);
                if (_propertiesNode != null)
                    this.Nodes.Insert(0, _propertiesNode);
            }
        }

        private void StartupProjects_InsertedItem(object sender, CollectionChangedEventArgs e)
        {
            var guid = (Guid)e.TargetObject;
            if (ProjectEntry.ObjectGuid == guid)
            {
                this.NodeFont = new System.Drawing.Font(this.TreeView.Font, System.Drawing.FontStyle.Bold);
            }
        }

        private void StartupProjects_RemovedItem(object sender, CollectionChangedEventArgs e)
        {
            var guid = (Guid)e.TargetObject;
            if (ProjectEntry.ObjectGuid == guid)
            {
                this.NodeFont = null;
            }
        }
    }
}
