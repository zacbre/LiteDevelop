using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms;
using LiteDevelop.Framework.Extensions;
using LiteDevelop.Framework.Gui;
using WeifenLuo.WinFormsUI.Docking;

namespace LiteDevelop.Gui.DockContents
{
    public partial class ToolboxContent : DockContent
    {
        private Dictionary<object, string> _componentMuiIdentifiers;
        private ToolboxService _lastToolboxService;
        private Bitmap _folderImage;
        private Bitmap _pointer;

        public ToolboxContent()
        {
            InitializeComponent();
            this.HideOnClose = true;
            this.Icon = Icon.FromHandle(Properties.Resources.toolbox.GetHicon());

            _folderImage = Properties.Resources.folder;
            _pointer = Properties.Resources.pointer;
            LiteDevelopApplication.Current.InitializedApplication += Current_InitializedApplication;
        }

        private void Current_InitializedApplication(object sender, EventArgs e)
        {
            _componentMuiIdentifiers = new Dictionary<object, string>()
            {
                {this, "ToolBoxContent.Title"},
            };

            LiteDevelopApplication.Current.ExtensionHost.UILanguageChanged += ExtensionHost_UILanguageChanged;
            LiteDevelopApplication.Current.ExtensionHost.ControlManager.AppearanceChanged += ControlManager_AppearanceChanged;
            ExtensionHost_UILanguageChanged(null, null);
        }

        private void ExtensionHost_UILanguageChanged(object sender, EventArgs e)
        {
            LiteDevelopApplication.Current.MuiProcessor.ApplyLanguageOnComponents(_componentMuiIdentifiers);
        }

        public void SetToolBox(ToolboxService toolboxService)
        {
            if (toolboxService == _lastToolboxService)
                return;
            
            toolBoxTreeView.Nodes.Clear();
            if (toolBoxTreeView.ImageList != null)
            {
                toolBoxTreeView.ImageList.Dispose();
            }

            toolBoxTreeView.ImageList = new ImageList()
            {
                ColorDepth = ColorDepth.Depth32Bit,
                ImageSize = new Size(16,16),
            };

            toolBoxTreeView.ImageList.Images.Add(_folderImage);
            toolBoxTreeView.ImageList.Images.Add(_pointer);

            if (toolboxService != null)
            {
                toolboxService.SelectedItemChanged += toolboxService_SelectedItemChanged;

                foreach (string category in toolboxService.CategoryNames)
                {
                    TreeNode rootNode = new TreeNode(category);
                    rootNode.Nodes.Add(new TreeNode("Pointer") { ImageIndex = 1, SelectedImageIndex = 1 });

                    foreach (ToolboxItem item in toolboxService.GetToolboxItems(category))
                    {
                        toolBoxTreeView.ImageList.Images.Add(item.Bitmap);
                        rootNode.Nodes.Add(new TreeNode(item.DisplayName)
                        {
                            ToolTipText = string.Format("{0}\r\n{1} v{2}\r\n\r\n{3}", item.TypeName, item.AssemblyName, item.Version, item.Description),
                            Tag = item,
                            ImageIndex = toolBoxTreeView.ImageList.Images.Count - 1,
                            SelectedImageIndex = toolBoxTreeView.ImageList.Images.Count - 1,
                        });
                    }

                    toolBoxTreeView.Nodes.Add(rootNode);
                    if (category == toolboxService.SelectedCategory)
                        rootNode.Expand();
                }

                label1.Visible = toolBoxTreeView.Nodes.Count == 0;
            }
            else
            {
                label1.Visible = true;
            }

            _lastToolboxService = toolboxService;
        }

        private void toolboxService_SelectedItemChanged(object sender, ToolBoxItemEventArgs e)
        {
            foreach (TreeNode categoryNode in toolBoxTreeView.Nodes)
            {
                foreach (TreeNode toolNode in categoryNode.Nodes)
                    if (toolNode.Tag == e.Item)
                    {
                        toolBoxTreeView.SelectedNode = toolNode;
                        break;
                    }
            }

        }

        private void ControlManager_AppearanceChanged(object sender, EventArgs e)
        {
            var processor = LiteDevelopApplication.Current.ExtensionHost.ControlManager.GlobalAppearanceMap.Processor;
            processor.ApplyAppearanceOnObject(this, Framework.Gui.DefaultAppearanceDefinition.Window);
            processor.ApplyAppearanceOnObject(toolBoxTreeView, Framework.Gui.DefaultAppearanceDefinition.TreeView);
            processor.ApplyAppearanceOnObject(label1, Framework.Gui.DefaultAppearanceDefinition.TreeView);
        }

        private void toolBoxTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // TODO
        }

        private void toolBoxTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            _lastToolboxService.SetSelectedToolboxItem(e.Node.Tag as ToolboxItem);
        }

    }
}
