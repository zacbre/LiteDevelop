using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LiteDevelop.Extensions;
using LiteDevelop.Framework.Extensions;
using LiteDevelop.Framework.Gui;

namespace LiteDevelop.Gui.Settings
{
    public partial class AppearanceEditor : SettingsControl
    {
        private string _globalIdentifier = "[Default]";
        private readonly System.Collections.Generic.Dictionary<object, string> _componentMuiIdentifiers;
        private readonly ILiteExtensionHost _extensionHost;
        private readonly Dictionary<IAppearanceMapProvider, AppearanceMap> _appearanceMaps = new Dictionary<IAppearanceMapProvider, AppearanceMap>();
        private bool _handleChangedEvents = false;

        public AppearanceEditor()
        {
            InitializeComponent();
            _extensionHost = LiteDevelopApplication.Current.ExtensionHost;

            _componentMuiIdentifiers = new Dictionary<object, string>()
            {
                {this.extensionsLabel, "AppearanceEditor.Extensions"},
                {this.columnHeader1, "AppearanceEditor.ListHeaders.Key"},
                {this.columnHeader2, "AppearanceEditor.ListHeaders.Preview"},
                {this.foreColorLabel, "AppearanceEditor.Editor.ForeColor"},
                {this.backColorLabel, "AppearanceEditor.Editor.BackColor"},
                {this.foreColorTransparentButton, "AppearanceEditor.Editor.Transparent"},
                {this.backColorTransparentButton, "AppearanceEditor.Editor.Transparent"},
                {this.boldCheckBox, "AppearanceEditor.Editor.Bold"},
                {this.italicCheckBox, "AppearanceEditor.Editor.Italic"},
                {this.underlineCheckBox, "AppearanceEditor.Editor.Underline"},
                {this.strikeoutCheckBox, "AppearanceEditor.Editor.Strikeout"},
            };

            _extensionHost.UILanguageChanged += extensionHost_UILanguageChanged;
            extensionHost_UILanguageChanged(null, null);
        }

        private void extensionHost_UILanguageChanged(object sender, EventArgs e)
        {
            LiteDevelopApplication.Current.MuiProcessor.ApplyLanguageOnComponents(_componentMuiIdentifiers);
        }

        public override void ApplySettings()
        {
            foreach (var keyValuePair in _appearanceMaps)
            {
                keyValuePair.Value.CopyTo(keyValuePair.Key.CurrentAppearanceMap);
            }

            (_extensionHost.ControlManager as ControlManager).OnAppearanceChanged(EventArgs.Empty);
        }

        public override void LoadUserDefinedPresets()
        {
            _appearanceMaps.Clear();
            extensionsComboBox.Items.Clear();

            foreach (var extension in _extensionHost.ExtensionManager.LoadedExtensions)
            {
                var provider = extension as IAppearanceMapProvider;

                if (provider != null)
                {
                    var mapCopy = provider.CurrentAppearanceMap.Clone() as AppearanceMap;
                    _appearanceMaps.Add(provider, mapCopy);

                    if (extension is LiteDevelopExtension)
                        extensionsComboBox.Items.Add(_globalIdentifier);
                    else 
                        extensionsComboBox.Items.Add(extension.Name);
                }
            }

            splitContainer1.Panel2.Enabled = false;
            foreColorPanel.BackColor = backColorPanel.BackColor = Color.Transparent;

            extensionsComboBox.Sorted = true;
            extensionsComboBox.SelectedIndex = 0;
            UpdateListView();
        }

        private void UpdateListView()
        {
            descriptionsListView.BeginUpdate();
            descriptionsListView.Items.Clear();

            foreach (var keyValuePair in _appearanceMaps)
            {
                if ((extensionsComboBox.Text == _globalIdentifier && keyValuePair.Key is LiteDevelopExtension) || 
                    ((LiteExtension)keyValuePair.Key).Name == extensionsComboBox.Text)
                {
                    var appearanceMap = keyValuePair.Value;
                    foreach (var description in appearanceMap.Descriptions)
                    {
                        var listItem = new ListViewItem(new string[] { description.Text, "AaBbCc" });
                        listItem.SubItems[1].ForeColor = description.ForeColor;
                        listItem.Tag = description;
                        listItem.UseItemStyleForSubItems = false;
                        descriptionsListView.Items.Add(listItem);
                    }
                    break;
                }
            }

            descriptionsListView.EndUpdate();
        }

        private void UpdatePreview()
        {
            _handleChangedEvents = false;
            var item = descriptionsListView.SelectedItems[0];
            var description = item.Tag as AppearanceDescription;
            item.SubItems[1].ForeColor = previewLabel.ForeColor = description.ForeColor;
            previewLabel.BackColor = description.BackColor;
            item.SubItems[1].Font = previewLabel.Font = new System.Drawing.Font("Consolas", 9.75F, description.FontStyle);
            boldCheckBox.Checked = (description.FontStyle & FontStyle.Bold) == FontStyle.Bold;
            italicCheckBox.Checked = (description.FontStyle & FontStyle.Italic) == FontStyle.Italic;
            underlineCheckBox.Checked = (description.FontStyle & FontStyle.Underline) == FontStyle.Underline;
            strikeoutCheckBox.Checked = (description.FontStyle & FontStyle.Strikeout) == FontStyle.Strikeout;
            _handleChangedEvents = true;
        }

        private void extensionsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateListView();
        }

        private void useDefaultsButton_Click(object sender, EventArgs e)
        {
            var index = descriptionsListView.SelectedIndices.Count > 0 ? descriptionsListView.SelectedIndices[0] : -1;

            foreach (var keyValuePair in _appearanceMaps)
            {
                keyValuePair.Key.DefaultAppearanceMap.CopyTo(keyValuePair.Value);
            }

            UpdateListView();

            if (index >= 0)
                descriptionsListView.Items[index].Selected = true;
        }

        private void descriptionsListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (splitContainer1.Panel2.Enabled = descriptionsListView.SelectedItems.Count > 0)
            {
                var description = descriptionsListView.SelectedItems[0].Tag as AppearanceDescription;
                foreColorPanel.BackColor = description.ForeColor;
                backColorPanel.BackColor = description.BackColor;
                UpdatePreview();
            }
        }

        private void colorPanel_Click(object sender, EventArgs e)
        {
            if (descriptionsListView.SelectedItems.Count > 0)
            {
                var panel = sender as Panel;

                using (var dialog = new ColorDialog())
                {
                    dialog.Color = panel.BackColor;
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        var description = descriptionsListView.SelectedItems[0].Tag as AppearanceDescription;
                        panel.BackColor = dialog.Color;

                        if (panel == foreColorPanel)
                            description.ForeColor = panel.BackColor;
                        else if (panel == backColorPanel)
                            description.BackColor = panel.BackColor;

                        UpdatePreview();
                    }
                }
            }
        }

        private void descriptionsListView_SizeChanged(object sender, EventArgs e)
        {
            columnHeader1.Width = descriptionsListView.ClientRectangle.Width - columnHeader2.Width;
        }

        private void foreColorTransparentButton_Click(object sender, EventArgs e)
        {
            if (descriptionsListView.SelectedItems.Count > 0)
            {
                var description = descriptionsListView.SelectedItems[0].Tag as AppearanceDescription;
                description.ForeColor = foreColorPanel.BackColor = Color.Transparent;
                UpdatePreview();
            }
        }

        private void backColorTransparentButton_Click(object sender, EventArgs e)
        {
            if (descriptionsListView.SelectedItems.Count > 0)
            {
                var description = descriptionsListView.SelectedItems[0].Tag as AppearanceDescription;
                description.BackColor = backColorPanel.BackColor = Color.Transparent;
                UpdatePreview();
            }
        }

        private void styleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (_handleChangedEvents && descriptionsListView.SelectedItems.Count > 0)
            {
                FontStyle style = FontStyle.Regular;
                if (boldCheckBox.Checked)
                    style |= FontStyle.Bold;
                if (italicCheckBox.Checked)
                    style |= FontStyle.Italic;
                if (underlineCheckBox.Checked)
                    style |= FontStyle.Underline;
                if (strikeoutCheckBox.Checked)
                    style |= FontStyle.Strikeout;

                var description = descriptionsListView.SelectedItems[0].Tag as AppearanceDescription;
                description.FontStyle = style;

                UpdatePreview();
            }
        }
    }
}
