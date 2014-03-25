using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LiteDevelop.Framework.Extensions;
using LiteDevelop.Framework.Mui;
using LiteDevelop.Extensions;

namespace LiteDevelop.Gui.Forms
{
    public partial class AboutDialog : Form
    {
        private Dictionary<object, string> _componentMuiIdentifiers;
        private ExtensionManager _manager;
        private MuiProcessor _muiProcessor;

        public AboutDialog(ExtensionManager manager)
        {
            _manager = manager;
            InitializeComponent();

            SetupMuiComponents();

            versionLabel.Text = Application.ProductVersion.ToString();
            repositoryLinkLabel.Text = LiteDevelopApplication.SourceRepositoryUrl;

            foreach (var extension in manager.LoadedExtensions)
            {
                listView1.Items.Add(new ListViewItem(new string[] { extension.Name, extension.Version.ToString() } ) { Tag = extension });
            }
        }

        private void SetupMuiComponents()
        {
            _muiProcessor = LiteDevelopApplication.Current.MuiProcessor;
            _componentMuiIdentifiers = new Dictionary<object, string>()
            {
                {this, "AboutDialog.Title"},
                {closeButton, "Common.Close"},
                {projectFounderHeaderLabel,"AboutDialog.ProjectFounder"},
                {versionHeaderLabel, "AboutDialog.Version"},
                {repositoryHeaderLabel, "AboutDialog.Repository"},
                {columnHeader1, "AboutDialog.ModuleListHeaders.Module"},
                {columnHeader2, "AboutDialog.ModuleListHeaders.Version"},
                {extensionNameHeaderLabel, "AboutDialog.ExtensionDetails.Name"},
                {extensionVersionHeaderLabel, "AboutDialog.ExtensionDetails.Version"},
                {extensionAuthorHeaderLabel, "AboutDialog.ExtensionDetails.Author"},
                {extensionDescriptionHeaderLabel, "AboutDialog.ExtensionDetails.Description"},
                {extensionCopyrightHeaderLabel, "AboutDialog.ExtensionDetails.Copyright"},
                {additionalReleaseInfoLabel, "AboutDialog.ExtensionDetails.AdditionalReleaseInformation"},
            };
            _muiProcessor.ApplyLanguageOnComponents(_componentMuiIdentifiers);
        }
        
        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void repositoryLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(repositoryLinkLabel.Text);
        }

        private static string FormatDetailedAuthorDescription(IDictionary<string, string[]> authorDescription)
        {
            var builder = new StringBuilder();
            foreach (var keyValuePair in authorDescription)
            {
                builder.AppendLine(keyValuePair.Key + ":");
                foreach (var author in keyValuePair.Value)
                {
                    builder.AppendLine(author);
                }
                builder.AppendLine();
            }
            return builder.ToString();
        }

        private void listView1_SizeChanged(object sender, EventArgs e)
        {
            columnHeader1.Width = listView1.ClientRectangle.Width - columnHeader2.Width;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 0 && listView1.SelectedItems[0].Tag is LiteExtension)
            {
                var extension = listView1.SelectedItems[0].Tag as LiteExtension;
                extensionNameLabel.Text = extension.Name;
                extensionVersionLabel.Text = extension.Version.ToString();
                extensionAuthorLabel.Text = extension.Author;
                extensionDescriptionLabel.Text = extension.Description;
                extensionCopyrightLabel.Text = extension.Copyright;
                additionalReleaseInfoTextBox.Text = extension.ReleaseInformation;
            }
        }
    }
}
