using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using LiteDevelop.Framework;
using LiteDevelop.Extensions;

namespace LiteDevelop.Gui.Forms
{
    public partial class ExtensionLoadDialog : Form
    {
        public ExtensionLoadDialog(IEnumerable<ExtensionLoadResult> results)
        {
            InitializeComponent();

            SetupMuiComponents();

            var iconProvider = IconProvider.GetProvider<ErrorIconProvider>();
            resultsListView.SmallImageList = iconProvider.ImageList;
            foreach (var result in results)
            {
                var item = new ListViewItem(new string[]
                {
                    (result.Extension != null ? result.Extension.Name : (result.ExtensionType != null ? result.ExtensionType.FullName : Path.GetFileName(result.FilePath))),
                    (result.FilePath),
                    (result.SuccesfullyLoaded ? LiteDevelopApplication.Current.MuiProcessor.GetString("ExtensionLoadDialog.LoadedSuccessfully") : result.Error.Message)
                });
                item.ImageIndex = iconProvider.GetImageIndex(result.SuccesfullyLoaded ? MessageSeverity.Success : MessageSeverity.Error);
                item.Tag = result;
                resultsListView.Items.Add(item);
            }
        }

        private void SetupMuiComponents()
        {
            var componentMuiIdentifiers = new Dictionary<object, string>()
            {
                {this, "ExtensionLoadDialog.Title"},
                {this.columnHeader1, "ExtensionLoadDialog.ListHeaders.Extension"},
                {this.columnHeader2, "ExtensionLoadDialog.ListHeaders.Path"},
                {this.columnHeader3, "ExtensionLoadDialog.ListHeaders.Result"},
                {this.button1, "Common.Ok"}
            };

            LiteDevelopApplication.Current.MuiProcessor.ApplyLanguageOnComponents(componentMuiIdentifiers);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void resultsListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (resultsListView.SelectedItems.Count != 0)
            {
                var result = resultsListView.SelectedItems[0].Tag as ExtensionLoadResult;
                if (result != null)
                {
                    string message = LiteDevelopApplication.Current.MuiProcessor.GetString("ExtensionLoadDialog.Details.Message",
                        "extension=" + (result.ExtensionType != null ? result.ExtensionType.FullName : result.FilePath),
                        "result=" + (result.SuccesfullyLoaded ? LiteDevelopApplication.Current.MuiProcessor.GetString("ExtensionLoadDialog.Details.Successfully") : LiteDevelopApplication.Current.MuiProcessor.GetString("ExtensionLoadDialog.Details.Unsuccessfully")),
                        "details=" + (result.Error != null ? result.Error.ToString() : LiteDevelopApplication.Current.MuiProcessor.GetString("Common.None")));

                    MessageBox.Show(message, LiteDevelopApplication.Current.MuiProcessor.GetString("ExtensionLoadDialog.Details.Title"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
