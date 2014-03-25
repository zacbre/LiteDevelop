using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LiteDevelop.Framework.FileSystem;

namespace LiteDevelop.Gui.Forms
{
    public partial class UnsavedFilesDialog : Form
    {
        private ISavableFile[] _files;

        // required for viewing in the designer
        private UnsavedFilesDialog()
        {
            InitializeComponent();
        }

        public UnsavedFilesDialog(ISavableFile[] files)
        {
            InitializeComponent();
         
            _files = files;
            foreach (var file in files)
            {
                filesListBox.Items.Add(file.FilePath, true);
            }
        }

        public ISavableFile[] GetItemsToSave()
        {
            var files = new ISavableFile[filesListBox.CheckedItems.Count];
            for (int i = 0; i < files.Length; i++)
            {
                files[i] = _files.First(x => x.FilePath.Equals(filesListBox.CheckedItems[i] as FilePath));
            }
            return files;
        }

        private void SetupMuiComponents()
        {
            var componentMuiIdentifiers = new Dictionary<object, string>()
            {
                {this, "UnsavedFilesDialog.Title"},
                {this.messageLabel, "UnsavedFilesDialog.Message"},
                {this.saveButton, "UnsavedFilesDialog.SaveSelected"},
                {this.dontSaveButton, "UnsavedFilesDialog.DontSave"},
                {this.cancelButton, "Common.Cancel"}
            };

            LiteDevelopApplication.Current.MuiProcessor.ApplyLanguageOnComponents(componentMuiIdentifiers);
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Yes;
            Close();
        }

        private void dontSaveButton_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.No;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }
    }
}
