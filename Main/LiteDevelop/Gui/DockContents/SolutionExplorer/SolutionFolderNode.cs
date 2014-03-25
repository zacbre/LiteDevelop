using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LiteDevelop.Framework;
using LiteDevelop.Framework.FileSystem;

namespace LiteDevelop.Gui.DockContents.SolutionExplorer
{
    public class SolutionFolderNode : PathNode
    {
        private IconProvider _iconProvider;

        public SolutionFolderNode(SolutionFolder solutionFolder, IconProvider iconProvider)
            : base(solutionFolder.FilePath)
        {
            this.SolutionFolder = solutionFolder;
            this._iconProvider = iconProvider;
            Text = solutionFolder.Name;
            UpdateTextOnFilePathChanged = false;

            AddStub();

            ImageIndex = SelectedImageIndex = (_iconProvider = iconProvider).GetImageIndex(solutionFolder);

            solutionFolder.Nodes.InsertedItem += Nodes_InsertedItem;
            solutionFolder.Nodes.RemovedItem += Nodes_RemovedItem;
            solutionFolder.FilePathChanged += solutionFolder_FilePathChanged;
        }

        public SolutionFolder SolutionFolder
        {
            get;
            private set;
        }

        public override bool CanAddFiles
        {
            get { return false; }
        }

        public override bool CanAddDirectories
        {
            get { return false; }
        }

        public override bool CanAddProjects
        {
            get { return true; }
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

        public override void RenameEntry(string newName)
        {
            throw new NotImplementedException();
        }

        public override void DeleteEntry()
        {
            throw new NotImplementedException();
        }

        private void AddStub()
        {
            Nodes.Add(new TreeNode());
        }

        public override void OnSelect()
        {
            Populate();
            base.OnSelect();
        }

        protected virtual void Populate()
        {
            if (this.Nodes.Count == 1 && !(this.Nodes[0] is AbstractNode))
            {
                Nodes.Clear();

                foreach (var node in SolutionFolder.Nodes)
                {
                    if (node is ProjectEntry)
                    {
                        Nodes.Add(new ProjectNode(node as ProjectEntry, _iconProvider));
                    }
                    else if (node is SolutionFolder)
                    {
                        Nodes.Add(new SolutionFolderNode(node as SolutionFolder, _iconProvider));
                    }
                }
            }
        }

        private void EnsureStubIsRemoved()
        {
            if (this.Nodes.Count == 1 && !(this.Nodes[0] is AbstractNode))
            {
                Nodes.Clear();
            }
        }

        private void Nodes_InsertedItem(object sender, CollectionChangedEventArgs e)
        {
            EnsureStubIsRemoved();
            Nodes.Add(new ProjectNode(e.TargetObject as ProjectEntry, _iconProvider));
        }

        private void Nodes_RemovedItem(object sender, CollectionChangedEventArgs e)
        {
            EnsureStubIsRemoved();
            foreach (TreeNode node in Nodes)
            {
                if ((node is ProjectNode && (node as ProjectNode).ProjectEntry == e.TargetObject as ProjectEntry) ||
                    (node is SolutionFolderNode && (node as SolutionFolderNode).SolutionFolder == e.TargetObject as SolutionFolder))
                {
                    node.Remove();
                    break;
                }
            }
        }

        private void solutionFolder_FilePathChanged(object sender, PathChangedEventArgs e)
        {
            FilePath = e.NewPath;
        }
    }
}
