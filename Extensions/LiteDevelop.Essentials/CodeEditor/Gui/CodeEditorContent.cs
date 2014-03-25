using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FastColoredTextBoxNS;
using LiteDevelop.Framework.Extensions;
using LiteDevelop.Framework.FileSystem;
using LiteDevelop.Framework.Gui;

namespace LiteDevelop.Essentials.CodeEditor.Gui
{
    public class CodeEditorContent : LiteDocumentContent, IClipboardHandler, IHistoryProvider, ISourceNavigator
    {
        private readonly CodeEditorControl _editorControl;
        private readonly ILiteExtensionHost _extensionHost;

        public CodeEditorContent(CodeEditorExtension extension, OpenedFile sourceFile) 
            : base(extension)
        {
            this.AssociatedFile = sourceFile;
            this.AssociatedFile.FilePathChanged += AssociatedFile_FilePathChanged;

            _extensionHost = extension.ExtensionHost;

            this.Text = sourceFile.FilePath.FileName + sourceFile.FilePath.Extension;
            this.Control = _editorControl = new CodeEditorControl(this, sourceFile)
            {
                Dock = DockStyle.Fill,
            };
            
        }

        #region LiteDocumentContent Members

        public override void Save(Stream stream)
        {
            // write byte order mask.
            byte[] bytes = Encoding.UTF8.GetPreamble();
            stream.Write(bytes, 0, bytes.Length); 

            // write contents
            bytes = Encoding.UTF8.GetBytes(_editorControl.TextBox.Text);
            stream.Write(bytes, 0, bytes.Length); 
        }

        #endregion

        #region IClipboardHandler Members

        public bool IsCutEnabled
        {
            get { return true; }
        }

        public bool IsCopyEnabled
        {
            get { return true; }
        }

        public bool IsPasteEnabled
        {
            get { return true; }
        }

        public void Cut()
        {
            _editorControl.TextBox.Cut();
        }

        public void Copy()
        {
            _editorControl.TextBox.Copy();
        }

        public void Paste()
        {
            _editorControl.TextBox.Paste();
        }

        #endregion

        #region IHistoryProvider Members

        public void Undo()
        {
            _editorControl.TextBox.Undo();
        }

        public void Redo()
        {
            _editorControl.TextBox.Redo();
        }

        #endregion

        #region IErrorHandler Members

        public void NavigateToLocation(SourceLocation location)
        {
            if (_editorControl.TextBox.LinesCount >= location.Line)
            {
                _editorControl.TextBox.Selection = new Range(_editorControl.TextBox, location.Column - 1, location.Line - 1, location.Column - 1, location.Line - 1);
                _editorControl.TextBox.DoCaretVisible();
            }
        }

        #endregion

        private void AssociatedFile_FilePathChanged(object sender, PathChangedEventArgs e)
        {
            Text = e.NewPath.FileName + e.NewPath.Extension;
        }

    }
}
