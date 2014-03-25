using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LiteDevelop.Framework.Extensions;
using LiteDevelop.Framework.Gui;

namespace LiteDevelop.Gui.Forms
{
    public partial class SettingsDialog : Form
    {
        private IEnumerable<ISettingsProvider> _extensions;

        private SettingsDialog()
        {
            InitializeComponent();
        }

        public SettingsDialog(IEnumerable<LiteExtension> extensions)
            : this()
        {
            SetupMuiComponents();

            var settingsProviders = new List<ISettingsProvider>();
            foreach (var extension in extensions)
            {
                if (extension is ISettingsProvider)
                {
                    var settingsProvider = extension as ISettingsProvider;
                    if (settingsProvider.RootSettingsNode != null)
                    {
                        settingsProvider.LoadUserDefinedPresets();
                        treeView1.Nodes.Add(settingsProvider.RootSettingsNode);
                        settingsProvider.RootSettingsNode.Expand();
                        settingsProviders.Add(settingsProvider);
                    }
                }
            }

            _extensions = settingsProviders;
        }

        private void SetupMuiComponents()
        {
            var componentMuiIdentifiers = new Dictionary<object, string>()
            {
                {this, "SettingsDialog.Title"},
                {this.button1, "Common.Ok"},
                {this.button2, "Common.Cancel"},
                {this.button3, "SettingsDialog.ResetToDefaults"},
            };

            LiteDevelopApplication.Current.MuiProcessor.ApplyLanguageOnComponents(componentMuiIdentifiers);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (var extension in _extensions)
            {
                extension.ApplySettings();
            }

            splitContainer1.Panel2.Controls.Clear();
            treeView1.Nodes.Clear();

            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel2.Controls.Clear();
            treeView1.Nodes.Clear();
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(LiteDevelopApplication.Current.MuiProcessor.GetString("SettingsDialog.ResetToDefaultsWarning"), "LiteDevelop", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
            {
                foreach (var extension in _extensions)
                {
                    extension.ResetSettings();
                    extension.LoadUserDefinedPresets();
                }
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var currentControl = splitContainer1.Panel2.Controls.Count == 0 ? null : splitContainer1.Panel2.Controls[0];
            if (e.Node is SettingsNode)
            {
                var node = e.Node as SettingsNode;
                if (currentControl != node.EditorControl)
                {
                    splitContainer1.Panel2.Controls.Clear();
                    splitContainer1.Panel2.Controls.Add(node.EditorControl);
                }
            }
        }

        private void SettingsDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            splitContainer1.Panel2.Controls.Clear();
            treeView1.Nodes.Clear();
        }
    }
}
