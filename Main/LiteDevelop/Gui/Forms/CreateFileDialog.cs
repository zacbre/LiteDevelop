using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using LiteDevelop.Framework.FileSystem;
using LiteDevelop.Framework.Languages;
using LiteDevelop.Extensions;

namespace LiteDevelop.Gui.Forms
{
    public partial class CreateFileDialog : Form
    {
        public static void UserCreateFile(LiteExtensionHost extensionHost)
        {
            UserCreateFile(extensionHost, string.Empty);
        }

        public static void UserCreateFile(LiteExtensionHost extensionHost, string directory)
        {
            UserCreateFile(extensionHost, extensionHost.GetCurrentSelectedProject(), directory);
        }

        public static void UserCreateFile(LiteExtensionHost extensionHost, Project currentProject, string directory)
        {
            using (var dlg = new CreateFileDialog(currentProject))
            {
                if (!string.IsNullOrEmpty(directory))
                {
                    dlg.Directory = directory;
                }

                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var result = (dlg.Template as FileTemplate).CreateFile(extensionHost.FileService, currentProject, new FilePath(dlg.FileName));

                    foreach (var createdFile in result.CreatedFiles)
                    {
                        var openedFile = createdFile.File as OpenedFile;
                        openedFile.Save(extensionHost.CreateOrGetReporter("Build"));

                        if (currentProject != null)
                        {
                            currentProject.ProjectFiles.Add(new ProjectFileEntry(openedFile));
                        }

                        createdFile.ExtensionToUse.OpenFile(openedFile);
                    }
                }
            }
        }

        private Project _parentProject;
        
        // required for the designer.
        private CreateFileDialog()
        {
            InitializeComponent();
        }

        public CreateFileDialog(Project parentProject)
        {
            _parentProject = parentProject;
            InitializeComponent();

            if (parentProject != null)
            {
                Text += " - " + parentProject.Name;
            }

            SetupMuiComponents();

            if (parentProject != null)
                directoryTextBox.Text = parentProject.ProjectDirectory; 

            List<TreeNode> rootNodes = new List<TreeNode>();
            foreach (var entry in LanguageDescriptor.RegisteredLanguages)
            {
                if (entry.Templates.FirstOrDefault(x => x is FileTemplate) != null)
                {
                    var node = GetLanguageOrderNode(rootNodes, entry.LanguageOrder);
                    node.Nodes.Add(new TreeNode(entry.Name) { Tag = entry });
                    node.Expand();
                }
            }
            languagesTreeView.Nodes.AddRange(rootNodes.ToArray());

            templatesListView.SmallImageList = new ImageList()
            {
                ColorDepth = ColorDepth.Depth32Bit,
                ImageSize = new Size(24, 24),
            };

            templatesListView.LargeImageList = new ImageList()
            {
                ColorDepth = ColorDepth.Depth32Bit,
                ImageSize = new Size(32, 32),
            };

        }

        public string FileName
        {
            get { return Path.Combine(directoryTextBox.Text, fileNameTextBox.Text); }
        }

        public string Directory
        {
            get { return directoryTextBox.Text; }
            set { directoryTextBox.Text = value; }
        }

        public FileTemplate Template
        {
            get { return templatesListView.SelectedItems[0].Tag as FileTemplate; }
        }

        public LanguageDescriptor Language
        {
            get { return languagesTreeView.SelectedNode.Tag as LanguageDescriptor; }
        }

        private void SetupMuiComponents()
        {
            var componentMuiIdentifiers = new Dictionary<object, string>()
            {
                {this, "CreateFileDialog.Title"},
                {nameHeaderLabel, "Common.Name"},
                {directoryHeaderLabel, "Common.Directory"},
                {browseButton, "Common.Browse"},
                {okButton, "Common.Ok"},
                {cancelButton, "Common.Cancel"},
            };

            LiteDevelopApplication.Current.MuiProcessor.ApplyLanguageOnComponents(componentMuiIdentifiers);
        }

        private void UpdateOkButton()
        {
            okButton.Enabled = templatesListView.SelectedItems.Count != 0 && !string.IsNullOrEmpty(fileNameTextBox.Text) && !string.IsNullOrEmpty(directoryTextBox.Text);
        }

        private TreeNode GetLanguageOrderNode(List<TreeNode> nodes, string languageOrder)
        {
            foreach (TreeNode node in nodes)
                if (node.Text == languageOrder)
                    return node;
            var newNode = new TreeNode(languageOrder);
            nodes.Add(newNode);
            return newNode;
        }

        private void languagesTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            templatesListView.Clear();
            templatesListView.SmallImageList.Images.Clear();
            templatesListView.LargeImageList.Images.Clear();

            if (languagesTreeView.SelectedNode != null && languagesTreeView.SelectedNode.Tag is LanguageDescriptor)
            {
                LanguageDescriptor descriptor = languagesTreeView.SelectedNode.Tag as LanguageDescriptor;

                foreach (var template in descriptor.Templates)
                {
                    if (template is FileTemplate && ((template as FileTemplate).ProjectRequired ? _parentProject != null : true))
                    {
                        int index = -1;
                        if (template.Icon != null)
                        {
                            index = templatesListView.SmallImageList.Images.Count;
                            templatesListView.SmallImageList.Images.Add(template.Icon);
                            templatesListView.LargeImageList.Images.Add(template.Icon);
                        }

                        templatesListView.Items.Add(new ListViewItem(template.Name)
                        {
                            Tag = template,
                            ImageIndex = index,
                        });
                    }
                }

                if (templatesListView.Items.Count > 0)
                    templatesListView.Items[0].Selected = true;
            }

            UpdateOkButton();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            string extension = Path.GetExtension(fileNameTextBox.Text);

            if (!Language.SupportAnyExtension && !Language.FileExtensions.Contains(extension))
            {
                fileNameTextBox.Text += Language.StandardFileExtension;
            }

            if (!File.Exists(FileName) || MessageBox.Show(string.Format("The file {0} already exists. Overwrite?", Path.GetFileName(FileName)), "LiteDevelop", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
            {
                DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CreateFileDialog_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && okButton.Enabled)
            {
                okButton.PerformClick();
            }
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.SelectedPath = directoryTextBox.Text;

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    directoryTextBox.Text = dialog.SelectedPath;
                }
            }
        }

        private void templatesListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            okButton.Enabled = templatesListView.SelectedItems.Count != 0;
        }


    }
}
