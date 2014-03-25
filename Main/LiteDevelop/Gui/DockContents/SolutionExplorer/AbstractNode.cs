using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LiteDevelop.Framework;

namespace LiteDevelop.Gui.DockContents.SolutionExplorer
{
    public abstract class AbstractNode : TreeNode
    {
        public AbstractNode(string text)
            : base(text)
        {
        }

        public abstract bool CanAddFiles
        {
            get;
        }

        public abstract bool CanAddDirectories
        {
            get;
        }

        public abstract bool CanAddProjects
        {
            get;
        }

        public abstract bool CanRename
        {
            get;
        }

        public abstract bool CanDelete
        {
            get;
        }

        public abstract bool CanActivate
        {
            get;
        }

        public virtual IEnumerable<ToolStripItem> AdditionalMenuItems
        {
            get { return null; }
        }

        public virtual void Activate() { }

        public virtual void OnSelect() { }

        public ProjectNode GetProjectNode()
        {
            TreeNode node = this;

            while (node != null && !(node is ProjectNode))
                node = node.Parent;

            return node as ProjectNode;
        }

        public SolutionNode GetSolutionNode()
        {
            TreeNode node = this;

            while (node != null && !(node is SolutionNode))
                node = node.Parent;

            return node as SolutionNode;
        }
    }
}
