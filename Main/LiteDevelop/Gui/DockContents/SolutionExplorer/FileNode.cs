using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LiteDevelop.Framework;
using LiteDevelop.Framework.FileSystem;

namespace LiteDevelop.Gui.DockContents.SolutionExplorer
{
    public class FileNode : PathNode 
    {
        private readonly IconProvider _iconProvider;
        private bool _handlePathEvents = true;

        public FileNode(ProjectFileEntry file, IconProvider iconProvider)
            : base(file.FilePath)
        {
            if (file == null)
                throw new ArgumentNullException("file");

            if (iconProvider == null)
                throw new ArgumentNullException("iconProvider");

            FileEntry = file;
            FileEntry.FilePathChanged += FileEntry_FilePathChanged;
            
            ImageIndex = SelectedImageIndex = (_iconProvider = iconProvider).GetImageIndex(file);
        }

        public ProjectFileEntry FileEntry
        {
            get;
            private set;
        }

        public override bool CanAddFiles
        {
            get { return false; }
        }

        public override bool CanAddDirectories
        {
            get { return false; }
        }

        public override bool CanAddProjects
        {
            get { return false; }
        }

        public override bool CanRename
        {
            get { return true; }
        }

        public override bool CanDelete
        {
            get { return true; }
        }

        public override bool CanActivate
        {
            get { return true; }
        }

        public override void Activate()
        {
            var extensionHost = LiteDevelopApplication.Current.ExtensionHost;
            var file = extensionHost.FileService.OpenFile(FileEntry.FilePath);

            if (file.RegisteredDocumentContents.Count == 0)
            {
                var fileHandlers = extensionHost.ExtensionManager.GetFileHandlers(FileEntry.FilePath).ToArray();

                if (fileHandlers.Length == 0)
                {
                    MessageBox.Show(LiteDevelopApplication.Current.MuiProcessor.GetString("MainForm.Messages.NoEditorAvailable", "file=" + FileEntry.FilePath.FullPath), "LiteDevelop", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    var fileHandler = extensionHost.FileService.SelectFileHandler(fileHandlers, FileEntry.FilePath);

                    if (fileHandler != null)
                    {
                        fileHandler.OpenFile(extensionHost.FileService.OpenFile(FileEntry.FilePath));
                    }
                }
            }
            else
            {
                extensionHost.ControlManager.SelectedDocumentContent = (file.CurrentDocumentContent ?? (file.CurrentDocumentContent = file.RegisteredDocumentContents[0]));
            }
        }

        public override void RenameEntry(string newName)
        {
            var extensionHost = LiteDevelopApplication.Current.ExtensionHost;
            var newPath = FilePath.ChangeName(newName);
            extensionHost.FileService.MoveFile(FilePath, newPath);
            FilePath = newPath;
        }

        public override void DeleteEntry()
        {
            FileEntry.ParentProject.ProjectFiles.Remove(FileEntry);
            var extensionHost = LiteDevelopApplication.Current.ExtensionHost;
            extensionHost.FileService.DeleteFile(FilePath);
            this.Remove();
            FileEntry.FilePathChanged -= FileEntry_FilePathChanged;
        }

        private void FileEntry_FilePathChanged(object sender, PathChangedEventArgs e)
        {
            if (_handlePathEvents)
            {
                FilePath = e.NewPath;
            }
        }

        protected override void OnFilePathChanged(PathChangedEventArgs e)
        {
            if (FileEntry != null)
            {
                _handlePathEvents = false;
                FileEntry.FilePath = e.NewPath;
                _handlePathEvents = true;
            }

            base.OnFilePathChanged(e);
        }
    }
}
