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
    public partial class CreateProjectDialog : Form
    {
        public static void UserCreateProject(LiteExtensionHost extensionHost)
        {
            var solution = extensionHost.CurrentSolution;

            using (var dlg = new CreateProjectDialog(solution))
            {
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string projectDirectory = Path.Combine(dlg.Directory, Path.GetFileNameWithoutExtension(dlg.FileName));

                    if (dlg.CreateSolutionDirectory)
                        projectDirectory = Path.Combine(projectDirectory, Path.GetFileNameWithoutExtension(dlg.FileName));

                    if (System.IO.Directory.Exists(projectDirectory))
                    {
                        if (MessageBox.Show(LiteDevelopApplication.Current.MuiProcessor.GetString("CreateProjectDialog.FolderAlreadyExists", "folder=" + projectDirectory), "LiteDevelop", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                            return;
                    }
                    else
                    {
                        System.IO.Directory.CreateDirectory(projectDirectory);
                    }

                    var projectTemplate = dlg.Template as ProjectTemplate;
                    var result = projectTemplate.CreateProject(extensionHost.FileService, new FilePath(projectDirectory, Path.GetFileName(dlg.FileName)));

                    result.Project.Save(extensionHost.CreateOrGetReporter("Build"));
                    
                    if (solution == null)
                    {
                        string solutionDirectory = dlg.CreateSolutionDirectory ? Path.GetDirectoryName(projectDirectory) : projectDirectory;

                        solution = Solution.CreateSolution(result.Project.Name);
                        solution.FilePath = new FilePath(solutionDirectory, Path.GetFileNameWithoutExtension(dlg.FileName) + ".sln");
                        solution.Settings.StartupProjects.Add(result.Project.ProjectGuid);
                        solution.Save(extensionHost.CreateOrGetReporter("Build"));
                        extensionHost.DispatchSolutionCreated(new SolutionEventArgs(solution));
                        solution.Nodes.Add(new ProjectEntry(result.Project));
                        extensionHost.DispatchSolutionLoad(new SolutionEventArgs(solution));
                    }
                    else
                        solution.Nodes.Add(new ProjectEntry(result.Project));


                    for (int i = 0; i < result.CreatedFiles.Count; i++)
                    {
                        result.CreatedFiles[i].ExtensionToUse.OpenFile(result.CreatedFiles[i].File as OpenedFile);
                    }
                }
            }
        }

        private CreateProjectDialog()
        {
            InitializeComponent();
        }

        private SolutionFolder _parentFolder;

        public CreateProjectDialog(SolutionFolder parentFolder)
        {
            InitializeComponent();
            SetupMuiComponents();

            checkBox1.Checked = checkBox1.Visible = parentFolder == null;

            _parentFolder = parentFolder;

            if (parentFolder == null)
                directoryTextBox.Text = LiteDevelopSettings.Instance.GetValue("Projects.DefaultProjectsPath");
            else
                directoryTextBox.Text = parentFolder.FilePath.HasExtension ? parentFolder.FilePath.ParentDirectory.FullPath : parentFolder.FilePath.FullPath;

            List<TreeNode> rootNodes = new List<TreeNode>();
            foreach (var entry in LanguageDescriptor.RegisteredLanguages)
            {
                if (entry.Templates.FirstOrDefault(x => x is ProjectTemplate) != null)
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
            get { return fileNameTextBox.Text; }
        }

        public string Directory
        {
            get { return directoryTextBox.Text; }
        }

        public bool CreateSolutionDirectory
        {
            get { return _parentFolder == null && checkBox1.Checked; }
        }

        public ProjectTemplate Template
        {
            get { return templatesListView.SelectedItems[0].Tag as ProjectTemplate; }
        }

        public LanguageDescriptor Language
        {
            get { return languagesTreeView.SelectedNode.Tag as LanguageDescriptor; }
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

        private void UpdateOkButton()
        {
            okButton.Enabled = templatesListView.SelectedItems.Count != 0 && !string.IsNullOrEmpty(fileNameTextBox.Text) && !string.IsNullOrEmpty(directoryTextBox.Text);
        }

        private void SetupMuiComponents()
        {
            var componentMuiIdentifiers = new Dictionary<object, string>()
            {
                {this, "CreateProjectDialog.Title"},
                {nameHeaderLabel, "Common.Name"},
                {directoryHeaderLabel, "Common.Directory"},
                {browseButton, "Common.Browse"},
                {okButton, "Common.Ok"},
                {cancelButton, "Common.Cancel"},
            };

            LiteDevelopApplication.Current.MuiProcessor.ApplyLanguageOnComponents(componentMuiIdentifiers);
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
                    if (template is ProjectTemplate)
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

        private void control_Changed(object sender, EventArgs e)
        {
            UpdateOkButton();
            
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

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            string extension = Path.GetExtension(fileNameTextBox.Text);
            string defaultExtension = null;
            bool extensionAdded = false;

            foreach (var descriptor in Language.GetProjectDescriptors())
            {
                if (defaultExtension == null)
                    defaultExtension = descriptor.ProjectExtension;

                if (extension.Equals(descriptor.ProjectExtension))
                {
                    extensionAdded = true;
                    break;
                }
            }

            if (!extensionAdded)
            {
                fileNameTextBox.Text += (defaultExtension ?? ".proj");
            }

            if (!File.Exists(FileName) || MessageBox.Show(LiteDevelopApplication.Current.MuiProcessor.GetString("CreateProjectDialog.FileAlreadyExists", "file=" + Path.GetFileName(FileName)), "LiteDevelop", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
            {
                DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }
        }

        private void CreateProjectDialog_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && okButton.Enabled)
            {
                okButton.PerformClick();
            }
        }
    }
}
