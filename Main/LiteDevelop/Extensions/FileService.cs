using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using LiteDevelop.Framework.Extensions;
using LiteDevelop.Framework.FileSystem;
using LiteDevelop.Gui.Forms;

namespace LiteDevelop.Extensions
{
    public class FileService : IFileService
    {
        private ILiteExtensionHost _extensionHost;
        private List<OpenedFile> _openedFiles = new List<OpenedFile>();

        public FileService(ILiteExtensionHost extensionHost)
        {
            _extensionHost = extensionHost;
        }

        public IList<OpenedFile> OpenedFiles
        {
            get { return _openedFiles.AsReadOnly(); }
        }

        public IFileHandler SelectFileHandler(IEnumerable<IFileHandler> fileHandlers, FilePath filePath)
        {
            var handlersArray = fileHandlers.ToArray();

            if (handlersArray.Length == 1)
                return handlersArray[0];

            if (handlersArray.Length > 1)
            {
                using (var openWithDlg = new OpenWithDialog(handlersArray.Cast<LiteExtension>()))
                {
                    if (openWithDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        return openWithDlg.SelectedExtension as IFileHandler;
                    }
                }
            }

            return null;
        }

        public OpenedFile GetOpenedFile(FilePath filePath)
        {
            return _openedFiles.FirstOrDefault(x => x.FilePath.Equals(filePath));
        }

        public OpenedFile OpenFile(FilePath filePath)
        {
            var file = GetOpenedFile(filePath);
            if (file == null)
            {
                file = new FileServiceOpenedFile(_extensionHost, filePath);

                if (_extensionHost.CurrentSolution != null)
                {
                    var projectFile = _extensionHost.CurrentSolution.FindProjectFile(file.FilePath);
                    if (projectFile != null)
                    {
                        file.Dependencies.AddRange(projectFile.Dependencies);
                    }
                }

                _openedFiles.Add(file);
                OnFileOpened(new FileEventArgs(file));
            }
            return file;
        }

        public OpenedFile CreateFile(FilePath filePath, byte[] contents)
        {
            File.WriteAllBytes(filePath.FullPath, contents);
            var file = new FileServiceOpenedFile(_extensionHost, filePath);
            _openedFiles.Add(file);
            OnFileCreated(new FileEventArgs(file));
            return file;
        }

        public void MoveFile(FilePath file, FilePath newPath)
        {
            File.Move(file.FullPath, newPath.FullPath);
            foreach (var openedFile in _openedFiles)
            {
                if (openedFile.FilePath.Equals(file))
                {
                    openedFile.FilePath = newPath;
                    OnFileMoved(new PathChangedEventArgs(file, newPath));
                    break;
                }
            }
        }

        public void CopyFile(FilePath file, FilePath newPath)
        {
            File.Copy(file.FullPath, newPath.FullPath);
            OnFileCopied(new PathChangedEventArgs(file, newPath));
        }

        public void DeleteFile(FilePath filePath)
        {
            File.Delete(filePath.FullPath);
            foreach (var openedFile in _openedFiles)
            {
                if (openedFile.FilePath.FullPath.StartsWith(filePath.FullPath))
                {
                    openedFile.CloseAllDocumentContents();
                }
            }

            OnFileDeleted(new PathEventArgs(filePath));
        }

        public void CreateDirectory(FilePath path)
        {
            Directory.CreateDirectory(path.FullPath);
            OnDirectoryCreated(new PathEventArgs(path));
        }

        public void MoveDirectory(FilePath path, FilePath newPath)
        {
            Directory.Move(path.FullPath, newPath.FullPath);

            foreach (var openedFile in _openedFiles)
            {
                if (openedFile.FilePath.FullPath.StartsWith(path.FullPath))
                {
                    string remainingPart = openedFile.FilePath.FullPath.Substring(path.FullPath.Length + 1);
                    openedFile.FilePath = newPath.Combine(remainingPart);
                }
            }

            OnDirectoryMoved(new PathChangedEventArgs(path, newPath)); 
        }

        public void DeleteDirectory(FilePath path)
        {
            Directory.Delete(path.FullPath, true);

            foreach (var openedFile in _openedFiles)
            {
                if (openedFile.FilePath.FullPath.StartsWith(path.FullPath) && openedFile.RegisteredDocumentContents.Count > 0)
                {
                    openedFile.CloseAllDocumentContents();
                }
            }

            OnDirectoryDeleted(new PathEventArgs(path));
        }

        public event FileEventHandler FileCreated;
        internal virtual void OnFileCreated(FileEventArgs e)
        {
            if (FileCreated != null)
                FileCreated(this, e);
        }

        public event FileEventHandler FileOpened;
        internal virtual void OnFileOpened(FileEventArgs e)
        {
            if (FileOpened != null)
                FileOpened(this, e);
        }

        public event PathChangedEventHandler FileCopied;
        internal void OnFileCopied(PathChangedEventArgs e)
        {
            if (FileCopied != null)
                FileCopied(this, e);
        }

        public event PathChangedEventHandler FileMoved;
        internal void OnFileMoved(PathChangedEventArgs e)
        {
            if (FileMoved != null)
                FileMoved(this, e);
        }

        public event PathEventHandler FileDeleted;
        internal void OnFileDeleted(PathEventArgs e)
        {
            if (FileDeleted != null)
                FileDeleted(this, e);
        }

        public event PathEventHandler DirectoryCreated;
        internal void OnDirectoryCreated(PathEventArgs e)
        {
            if (DirectoryCreated != null)
                DirectoryCreated(this, e);
        }

        public event PathChangedEventHandler DirectoryMoved;
        internal void OnDirectoryMoved(PathChangedEventArgs e)
        {
            if (DirectoryMoved != null)
                DirectoryMoved(this, e);
        }

        public event PathEventHandler DirectoryDeleted;
        internal void OnDirectoryDeleted(PathEventArgs e)
        {
            if (DirectoryDeleted != null)
                DirectoryDeleted(this, e);
        }
    }
}
