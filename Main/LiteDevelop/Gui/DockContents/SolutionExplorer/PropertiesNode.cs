using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiteDevelop.Framework;
using LiteDevelop.Framework.FileSystem;

namespace LiteDevelop.Gui.DockContents.SolutionExplorer
{
    public class PropertiesNode : AbstractNode 
    {
        public PropertiesNode(IconProvider iconProvider)
            : base("Properties")
        {
            ImageIndex = SelectedImageIndex = SolutionExplorerIconProvider.Index_Properties;
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
            get { return true; }
        }

        public override void Activate()
        {
            var node = GetProjectNode();
            if (node != null && node.ProjectEntry != null && node.ProjectEntry.HasProject)
            {
                var extensionHost = LiteDevelopApplication.Current.ExtensionHost;
                var editor = node.ProjectEntry.Project.EditorContent;

                if (!extensionHost.ControlManager.OpenDocumentContents.Contains(editor))
                    extensionHost.ControlManager.OpenDocumentContents.Add(editor);

                extensionHost.ControlManager.SelectedDocumentContent = editor;
            }
        }
    }
}
