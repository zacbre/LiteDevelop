using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LiteDevelop.Framework;
using LiteDevelop.Framework.Extensions;
using LiteDevelop.Framework.FileSystem;
using WeifenLuo.WinFormsUI.Docking;
using LiteDevelop.Extensions;

namespace LiteDevelop.Gui.DockContents
{
    public partial class ErrorContent : DockContent
    {
        private readonly Dictionary<object, string> _componentMuiIdentifiers;
        private readonly ErrorIconProvider _iconProvider;
        
        private LiteExtensionHost _extensionHost;
        private IErrorManager _errorManager;
        private int errorCount = 0, warningCount = 0, messageCount = 0;
        private bool _resizing;
        private IEnumerable<BuildError> _errors;

        public ErrorContent()
        {
            InitializeComponent();

            this.HideOnClose = true;
            this._iconProvider = IconProvider.GetProvider<ErrorIconProvider>();
            this.errorsToolStripButton.Image = _iconProvider.ImageList.Images[_iconProvider.GetImageIndex(MessageSeverity.Error)];
            this.warningsToolStripButton.Image = _iconProvider.ImageList.Images[_iconProvider.GetImageIndex(MessageSeverity.Warning)];
            this.messagesToolStripButton.Image = _iconProvider.ImageList.Images[_iconProvider.GetImageIndex(MessageSeverity.Message)];
            this.listView1.SmallImageList = _iconProvider.ImageList;
            this.Icon = Icon.FromHandle(Properties.Resources.errorlist.GetHicon());
            
            _componentMuiIdentifiers = new Dictionary<object, string>()
            {
                {this, "ErrorContent.Title"},
                {this.columnHeader1, "ErrorContent.ListHeaders.Message"},
                {this.columnHeader2, "ErrorContent.ListHeaders.File"},
                {this.columnHeader3, "ErrorContent.ListHeaders.Line"},
                {this.columnHeader4, "ErrorContent.ListHeaders.Column"},
                {this.copyToolStripMenuItem, "ErrorContent.ContextMenu.Copy"},
                {this.goToFileToolStripMenuItem, "ErrorContent.ContextMenu.GotoFile"},
            };
            
            LiteDevelopApplication.Current.InitializedApplication += Current_InitializedApplication;
        }

        private void Current_InitializedApplication(object sender, EventArgs e)
        {
            _extensionHost = LiteDevelopApplication.Current.ExtensionHost;

            errorsToolStripButton.Checked = LiteDevelopSettings.Instance.GetValue<bool>("ErrorList.ErrorsVisible");
            warningsToolStripButton.Checked = LiteDevelopSettings.Instance.GetValue<bool>("ErrorList.WarningsVisible");
            messagesToolStripButton.Checked = LiteDevelopSettings.Instance.GetValue<bool>("ErrorList.MessagesVisible");

            _extensionHost.UILanguageChanged += extensionHost_UILanguageChanged;
            _extensionHost.ControlManager.AppearanceChanged += ControlManager_AppearanceChanged;
            extensionHost_UILanguageChanged(null, null);
        }
                
        public void SetErrorManager(IErrorManager errorManager)
        {
            if (_errorManager != null)
            {
                _errorManager.ReportedError -= _errorManager_ReportedError;
            }
            _errorManager = errorManager;
            _errorManager.ReportedError += _errorManager_ReportedError;
        }

        public void ClearErrors()
        {
            _errors = null;
            UpdateListView();
            UpdateToolbar();
        }

        public void SetErrors(IEnumerable<BuildError> errors)
        {
            _errors = errors;
            UpdateListView();
            UpdateToolbar();
        }

        private void UpdateToolbar()
        {
            errorCount = warningCount = messageCount = 0;
            if (_errors != null)
            {
                foreach (var error in _errors)
                {
                    switch (error.Severity)
                    {
                        case MessageSeverity.Error: errorCount++; break;
                        case MessageSeverity.Warning: warningCount++; break;
                        case MessageSeverity.Message: messageCount++; break;
                    }
                }
            }

            errorsToolStripButton.Text = LiteDevelopApplication.Current.MuiProcessor.GetString("ErrorContent.Toolbar.Errors", "count=" + errorCount.ToString());
            warningsToolStripButton.Text = LiteDevelopApplication.Current.MuiProcessor.GetString("ErrorContent.Toolbar.Warnings", "count=" + warningCount.ToString());
            messagesToolStripButton.Text = LiteDevelopApplication.Current.MuiProcessor.GetString("ErrorContent.Toolbar.Messages", "count=" + messageCount.ToString());
        }

        private void UpdateListView()
        {
            listView1.BeginUpdate();

            listView1.Items.Clear();
            if (_errors != null)
            {
                foreach (var error in _errors)
                {
                    if (GetRowVisibility(error.Severity))
                        AddError(error);
                }
            }

            listView1.EndUpdate();
        }

        private void AddError(BuildError error)
        {
            var item = new ListViewItem(new string[] 
                {
                    error.Message,
                    error.Location.FilePath.FileName,
                    error.Location.Line.ToString(),
                    error.Location.Column.ToString(),
                });
            item.Tag = error;
            item.ImageIndex = _iconProvider.GetImageIndex(error.Severity);
            listView1.Items.Add(item);
        }

        private bool GetRowVisibility(MessageSeverity severity)
        {
            switch (severity)
            {
                case MessageSeverity.Error:
                    return errorsToolStripButton.Checked;
                case MessageSeverity.Warning:
                    return warningsToolStripButton.Checked;
                case MessageSeverity.Message:
                    return messagesToolStripButton.Checked;
            }
            return false;
        }

        private void extensionHost_UILanguageChanged(object sender, EventArgs e)
        {
            LiteDevelopApplication.Current.MuiProcessor.ApplyLanguageOnComponents(_componentMuiIdentifiers);
            UpdateToolbar();
        }

        private void ControlManager_AppearanceChanged(object sender, EventArgs e)
        {
            var processor = LiteDevelopApplication.Current.ExtensionHost.ControlManager.GlobalAppearanceMap.Processor;
            processor.ApplyAppearanceOnObject(this, Framework.Gui.DefaultAppearanceDefinition.Window);
            processor.ApplyAppearanceOnObject(listView1, Framework.Gui.DefaultAppearanceDefinition.ListView);
        }

        private void _errorManager_ReportedError(object sender, BuildErrorEventArgs e)
        {
            AddError(e.Error);
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                var item = listView1.SelectedItems[0];
                var builder = new StringBuilder();
                for (int i = 0; i < item.SubItems.Count; i++)
                    builder.Append(item.SubItems[i].Text + (i == item.SubItems.Count - 1 ? "" : " | "));
                Clipboard.SetText(builder.ToString());
            }
        }

        private void goToFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                var item = listView1.SelectedItems[0];
                _errorManager.RequestNavigateToError(item.Tag as BuildError);
            }
        }

        private void toolStripButtons_CheckedChanged(object sender, EventArgs e)
        {
            UpdateListView();

            LiteDevelopSettings.Instance.SetValue("ErrorList.ErrorsVisible", errorsToolStripButton.Checked);
            LiteDevelopSettings.Instance.SetValue("ErrorList.WarningsVisible", warningsToolStripButton.Checked);
            LiteDevelopSettings.Instance.SetValue("ErrorList.MessagesVisible", messagesToolStripButton.Checked); 
        }

        private void listView1_SizeChanged(object sender, EventArgs e)
        {
            if (!_resizing)
            {
                _resizing = true;

                int clientWidth = listView1.ClientRectangle.Width - 1;
                float totalColumnWidth = 0;

                for (int i = 0; i < listView1.Columns.Count; i++)
                {
                    if (string.IsNullOrEmpty(listView1.Columns[i].Tag as string))
                        clientWidth -= listView1.Columns[i].Width;
                    else
                        totalColumnWidth += Convert.ToInt32(listView1.Columns[i].Tag);
                }

                for (int i = 0; i < listView1.Columns.Count; i++)
                {
                    if (!string.IsNullOrEmpty(listView1.Columns[i].Tag as string))
                    {
                        float widthPercentage = (Convert.ToInt32(listView1.Columns[i].Tag) / totalColumnWidth);
                        listView1.Columns[i].Width = (int)(widthPercentage * clientWidth);
                    }
                }
                
            }

            _resizing = false;
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            goToFileToolStripMenuItem.PerformClick();
        }
    }

}
