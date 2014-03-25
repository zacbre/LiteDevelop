using System;
using System.Linq;
using System.Windows.Forms;
using LiteDevelop.Framework.Gui;

namespace LiteDevelop.Framework.FileSystem.Net
{
    public partial class NetProjectSettingsControl : UserControl
    {
        private NetProject _project;
        private bool _updateSettings = false;

        // defined to let designer work in vs.
        private NetProjectSettingsControl()
        {
            InitializeComponent();
            targetFrameworkComboBox.Items.AddRange(FrameworkDetector.GetInstalledVersions());
        }

        public NetProjectSettingsControl(NetProject project)
            : this()
        {
            _project = project;
            project.NameChanged += project_NameChanged;

            project.ApplicationTypeChanged += project_ApplicationTypeChanged;
            project.References.RemovedItem += References_RemovedItem;
            project.References.InsertedItem += References_InsertedItem;
            project.RootNamespaceChanged += new EventHandler(project_RootNamespaceChanged);
            project.TargetFrameworkChanged += project_TargetFrameworkChanged;

            nameTextBox.Text = project.Name;
            rootNamespaceTextBox.Text = project.RootNamespace;
            applicationTypeComboBox.SelectedIndex = ((int)project.ApplicationType) - 1;
            targetFrameworkComboBox.SelectedItem = _project.TargetFramework;

            listBox1.Items.AddRange(project.References.ToArray());

            _updateSettings = true;
        }

        private void project_NameChanged(object sender, EventArgs e)
        {
            _updateSettings = false;
            nameTextBox.Text = _project.Name;
            _updateSettings = true;
        }

        private void nameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (_updateSettings)
            {
                _project.Name = nameTextBox.Text;
            }
        }

        private void project_RootNamespaceChanged(object sender, EventArgs e)
        {
            _updateSettings = false;
            rootNamespaceTextBox.Text = _project.RootNamespace;
            _updateSettings = true;
        }

        private void rootNamespaceTextBox_TextChanged(object sender, EventArgs e)
        {
            if (_updateSettings)
            {
                _project.RootNamespace = rootNamespaceTextBox.Text;
            }
        }

        private void project_TargetFrameworkChanged(object sender, EventArgs e)
        {
            _updateSettings = false;
            targetFrameworkComboBox.SelectedItem = _project.TargetFramework;
            _updateSettings = true;
        }
        
        private void targetFrameworkComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updateSettings)
            {
                _project.TargetFramework = (FrameworkVersion)(targetFrameworkComboBox.SelectedItem);
            }
        }

        private void project_ApplicationTypeChanged(object sender, EventArgs e)
        {
            _updateSettings = false;
            applicationTypeComboBox.SelectedIndex = ((int)_project.ApplicationType) - 1;
            _updateSettings = true;
        }

        private void applicationTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updateSettings)
            {
                _project.ApplicationType = (SubSystem)(applicationTypeComboBox.SelectedIndex + 1);
            }
        }

        private void References_RemovedItem(object sender, CollectionChangedEventArgs e)
        {
            listBox1.Items.Remove(e.TargetObject);
        }

        private void References_InsertedItem(object sender, CollectionChangedEventArgs e)
        {
            listBox1.Items.Insert(e.TargetIndex, e.TargetObject);
        }

        private void addReferenceButton_Click(object sender, EventArgs e)
        {
            var dlg = new AddReferenceDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                _project.References.Add(dlg.SelectedAssembly);
            }
        }

        private void removeReferenceButton_Click(object sender, EventArgs e)
        {
            _project.References.Remove((string)listBox1.SelectedItem);
        }
    }

    public class NetProjectSettingsContent : LiteDocumentContent
    {
        public NetProjectSettingsContent(NetProject project)
        {
            Text = project.Name;

            Control = new NetProjectSettingsControl(project);
        }

        /// <inheritdoc />
        public override void Save(System.IO.Stream stream)
        {
            throw new NotImplementedException();
        }
    }

}
