using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LiteDevelop.Framework;

namespace LiteDevelop.Gui.DockContents.SolutionExplorer
{
    public class FileReferenceNode : AbstractNode
    {
        public FileReferenceNode(string reference, IconProvider iconProvider)
            : base(Path.GetFileName(reference))
        {
            ImageIndex = SelectedImageIndex = iconProvider.GetImageIndex(reference);
            Reference = reference;
        }

        public string Reference
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
