using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LiteDevelop.Framework;
using LiteDevelop.Framework.FileSystem;

namespace LiteDevelop.Essentials.FormsDesigner.Gui
{
    public partial class ErrorControl : UserControl
    {
        private ErrorIconProvider _iconProvider;
        public event EventHandler ReloadRequested;

        public ErrorControl()
        {
            InitializeComponent();
            _iconProvider = IconProvider.GetProvider<ErrorIconProvider>();
        }

        public void SetBuildErrors(IEnumerable<BuildError> errors)
        {
            dgvErrors.Rows.Clear();
            foreach (var error in errors)
            {
                var row = new DataGridViewRow();
                row.Tag = error;

                var image = _iconProvider.ImageList.Images[_iconProvider.GetImageIndex(error.Severity)];

                row.Cells.Add(new DataGridViewImageCell() { Value = image, ToolTipText = error.Severity.ToString() });
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = error.Message });
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = error.Location.FilePath.FileName });
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = error.Location.Line });
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = error.Location.Column });
                dgvErrors.Rows.Add(row);
            }
        }

        public void SetException(Exception exception)
        {
            dgvErrors.Rows.Clear();

            do
            {
                var stackTrace = new StackTrace(exception);
                var frame = stackTrace.GetFrame(0);
                var row = new DataGridViewRow();
                row.Tag = exception;

                var image = _iconProvider.ImageList.Images[_iconProvider.GetImageIndex(MessageSeverity.Error)];

                row.Cells.Add(new DataGridViewImageCell() { Value = image });
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = exception.Message });
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = frame.GetFileName() + " - " + exception.TargetSite.ToString()  });
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = frame.GetFileLineNumber() });
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = frame.GetFileColumnNumber() });
                dgvErrors.Rows.Add(row);
                exception = exception.InnerException;
            } while (exception != null);
        }

        private void reloadLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (ReloadRequested != null)
                ReloadRequested(this, EventArgs.Empty);
        }
    }
}
