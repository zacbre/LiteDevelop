using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LiteDevelop.Framework;
using LiteDevelop.Framework.FileSystem;

namespace LiteDevelop.Gui.DockContents.SolutionExplorer
{
    public class ReferencesNode : AbstractNode 
    {
        private IconProvider _iconProvider;

        public ReferencesNode(IFileReferenceProvider referenceProvider, IconProvider iconProvider)
            : base("References")
        {
            ReferenceProvider = referenceProvider;
            ReferenceProvider.References.InsertedItem += References_InsertedItem;
            ReferenceProvider.References.RemovedItem += References_RemovedItem;

            _iconProvider = iconProvider;
            ImageIndex = SelectedImageIndex = SolutionExplorerIconProvider.Index_ReferencesDirectory;

            foreach (var reference in referenceProvider.References)
                Nodes.Add(new FileReferenceNode(reference, _iconProvider));
        }

        private void References_InsertedItem(object sender, CollectionChangedEventArgs e)
        {
            Nodes.Add(new FileReferenceNode(e.TargetObject as string, _iconProvider));
        }

        private void References_RemovedItem(object sender, CollectionChangedEventArgs e)
        {
            foreach (TreeNode node in Nodes)
            {
                if (node is FileReferenceNode && (node as FileReferenceNode).Reference == e.TargetObject as string)
                {
                    node.Remove();
                    break;
                }
            }
        }

        public IFileReferenceProvider ReferenceProvider
        {
            get;
            set;
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
            get { return false; }
        }

        public override bool CanRename
        {
            get { return false; }
        }

        public override bool CanDelete
        {
            get { return false; }
        }

        public override bool CanActivate
        {
            get { return false; }
        }
        
    }
}
