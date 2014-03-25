using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using LiteDevelop.Framework.Extensions;
using LiteDevelop.Extensions;

namespace LiteDevelop.Gui.Forms
{
    public partial class ExtensionsDialog : Form
    {
        private bool _hasRemovedExtensions = false;

        public ExtensionsDialog()
        {
            InitializeComponent();

            SetupMuiComponents();

            foreach (var extension in LiteDevelopApplication.Current.ExtensionHost.ExtensionManager.LoadedExtensions)
            {
                AddExtensionNode(extension);
            }

            foreach (var file in LiteDevelopSettings.Instance.GetArray<ExtensionLibraryData>("Application.Extensions"))
            {
                GetAssemblyNode(file.GetAbsolutePath(), true);
            }
        }

        private void SetupMuiComponents()
        {
            var componentMuiIdentifiers = new Dictionary<object, string>()
            {
                {this, "ExtensionsDialog.Title"},
                {this.addButton, "Common.Add"},
                {this.removeButton, "Common.Remove"},
                {this.closeButton, "Common.Close"},
            };
            LiteDevelopApplication.Current.MuiProcessor.ApplyLanguageOnComponents(componentMuiIdentifiers);
        }

        private void AddExtensionNode(LiteExtension extension)
        {
            Assembly assembly = extension.GetType().Assembly;
            TreeNode node = GetAssemblyNode(assembly);
            node.Checked = true;
            node.Expand();
            node.Nodes.Add(string.Format("{0} ({1})", extension.Name, extension.Version));
        }

        private TreeNode GetAssemblyNode(string file, bool createIfNotPresent)
        {
            foreach (TreeNode node in extensionsTreeView.Nodes)
            {
                if ((node.Tag is Assembly && (node.Tag as Assembly).Location == file) ||
                    ((node.Tag as string) == file))
                    return node;
            }

            if (createIfNotPresent)
            {
                var node = new TreeNode(file) { Tag = file, ForeColor = Color.Red };
                extensionsTreeView.Nodes.Add(node);
                return node;
            }

            return null;
        }

        private TreeNode GetAssemblyNode(Assembly assembly)
        {
            var node = GetAssemblyNode(assembly.Location, false);
            if (node == null)
            {
                var assemblyName = assembly.GetName();
                node = new TreeNode(string.Format("{0} (v{1})", assemblyName.Name, assemblyName.Version));
                node.Tag = assembly;
                extensionsTreeView.Nodes.Add(node);
            }
            return node;
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = string.Format("{0}|*.dll", LiteDevelopApplication.Current.MuiProcessor.GetString("ExtensionsDialog.Add.FileFilter"));

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                StringBuilder errorBuilder = new StringBuilder();
                var results = LiteDevelopApplication.Current.ExtensionHost.ExtensionManager.LoadExtensions(new ExtensionLibraryData(ofd.FileName));

                foreach (var result in results)
                {
                    if (result.SuccesfullyLoaded)
                    {
                        AddExtensionNode(result.Extension);
                    }
                }

                GetAssemblyNode(ofd.FileName, true);

                var extensions = LiteDevelopSettings.Instance.GetArray<ExtensionLibraryData>("Application.Extensions").ToList();
                if (extensions.FirstOrDefault(x => x.GetAbsolutePath().Equals(ofd.FileName, StringComparison.OrdinalIgnoreCase)) == null)
                {
                    extensions.Add(new ExtensionLibraryData(ofd.FileName));
                    LiteDevelopSettings.Instance.SetArray("Application.Extensions", extensions);
                }

                new ExtensionLoadDialog(results).ShowDialog();
            }
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            if (extensionsTreeView.SelectedNode != null)
            {
                if (MessageBox.Show(LiteDevelopApplication.Current.MuiProcessor.GetString("ExtensionsDialog.Remove.Warning"), "LiteDevelop", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
                {
                    bool isAssembly = extensionsTreeView.SelectedNode.Tag is Assembly;
                    string path = (isAssembly ? (extensionsTreeView.SelectedNode.Tag as Assembly).Location : extensionsTreeView.SelectedNode.Tag as string);

                    var extensions = LiteDevelopSettings.Instance.GetArray<ExtensionLibraryData>("Application.Extensions").ToList();
                    var item = extensions.FirstOrDefault(x => x.GetAbsolutePath().Equals(path, StringComparison.OrdinalIgnoreCase));
                    if (!string.IsNullOrEmpty(path) && item != null)
                    {
                        extensions.Remove(item);
                        LiteDevelopSettings.Instance.SetArray("Application.Extensions", extensions);
                    }

                    extensionsTreeView.Nodes.Remove(extensionsTreeView.SelectedNode);

                    if (isAssembly)
                    {
                        _hasRemovedExtensions = true;
                    }
                }
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            if (_hasRemovedExtensions)
            {
                if (MessageBox.Show(LiteDevelopApplication.Current.MuiProcessor.GetString("ExtensionsDialog.RestartRequired"), "LiteDevelop", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                {
                    Application.Restart();
                }
            }
            Close();
        }

        private void extensionsTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            removeButton.Enabled = e.Node.Parent == null;
        }
    }
}
