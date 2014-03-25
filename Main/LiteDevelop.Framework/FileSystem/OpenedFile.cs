using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LiteDevelop.Framework.Gui;

namespace LiteDevelop.Framework.FileSystem
{
    /// <summary>
    /// Represents a file opened in LiteDevelop.
    /// </summary>
    public abstract class OpenedFile : IFilePathProvider, ISavableFile
    {
        public event EventHandler ContentsChanged;
        public event EventHandler HasUnsavedDataChanged;
        public event EventHandler CurrentDocumentContentChanged;
        public event PathChangedEventHandler FilePathChanged;

        private byte[] _contents;
        private FilePath _filePath;
        private LiteDocumentContent _currentPage;
        private List<LiteDocumentContent> _registeredContents;
        private bool _hasUnsavedData;

        public OpenedFile(FilePath filePath)
        {
            FilePath = filePath;
            _registeredContents = new List<LiteDocumentContent>();
            Dependencies = new EventBasedCollection<string>();
        }

        /// <summary>
        /// Gets the file path to the opened file.
        /// </summary>
        public FilePath FilePath
        {
            get { return _filePath; }
            set
            {
                if (_filePath == null || !_filePath.Equals(value))
                {
                    FilePath oldPath = _filePath;
                    _filePath = value;
                    OnFilePathChanged(new PathChangedEventArgs(oldPath, value));
                }
            }
        }

        /// <summary>
        /// Gets a read-only collection of document view contents that have currently viewing the file.
        /// </summary>
        public IList<LiteDocumentContent> RegisteredDocumentContents
        {
            get { return _registeredContents.AsReadOnly(); }
        }

        /// <summary>
        /// Gets the document view content that is currently in use by the user to view or edit the file.
        /// </summary>
        public LiteDocumentContent CurrentDocumentContent
        {
            get { return _currentPage; }
            set
            {
                if (_currentPage != value)
                {
                    _currentPage = value;
                    OnCurrentDocumentContentChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the file has unsaved data.
        /// </summary>
        public bool HasUnsavedData 
        {
            get { return _hasUnsavedData; }
            protected set
            {
                if (_hasUnsavedData != value)
                {
                    _hasUnsavedData = value;
                    OnHasUnsavedDataChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets a collection of files this file is depending on.
        /// </summary>
        public EventBasedCollection<string> Dependencies
        {
            get;
            private set;
        }

        /// <summary>
        /// Reports the file has unsaved data that needs to be saved in order to conserve the changes that are made.
        /// </summary>
        public void GiveUnsavedData()
        {
            HasUnsavedData = true;
        }

        /// <summary>
        /// Registers a document view content that uses the file.
        /// </summary>
        public virtual void RegisterDocumentContent(LiteDocumentContent documentContent)
        {
            _registeredContents.Add(documentContent);
        }

        /// <summary>
        /// Unregisters a document view content that no longer uses the file.
        /// </summary>
        public virtual void UnRegisterDocumentContent(LiteDocumentContent documentContent)
        {
            _registeredContents.Remove(documentContent);
        }

        /// <summary>
        /// Closes all document view contents associated with this file.
        /// </summary>
        public void CloseAllDocumentContents()
        {
            while (_registeredContents.Count != 0)
                _registeredContents[0].Close(true);
        }

        /// <summary>
        /// Sets the contents of this opened file and marks the file as unsaved.
        /// </summary>
        /// <param name = "contents">The contents in string format to set</param>
        public void SetContents(string contents)
        {
            byte[] bom = Encoding.UTF8.GetPreamble();
            byte[] stringBytes = Encoding.UTF8.GetBytes(contents);
            byte[] totalBytes = new byte[bom.Length + stringBytes.Length];
            Buffer.BlockCopy(bom, 0, totalBytes, 0, bom.Length);
            Buffer.BlockCopy(stringBytes, 0, totalBytes, bom.Length, stringBytes.Length);
            SetContents(totalBytes);
        }

        /// <summary>
        /// Sets the contents of this opened file and marks the file as unsaved.
        /// </summary>
        /// <param name = "contents">The contents in byte format to set</param>
        public void SetContents(byte[] contents)
        {
            _contents = contents; 
            OnContentsChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Returns the contents of this (unsaved) file in string format.
        /// </summary>
        public string GetContentsAsString()
        {
            byte[] bytes = GetContentsAsBytes();
            byte[] bom = null;
            Encoding encodingToUse = null;

            var encodings = Encoding.GetEncodings();

            foreach (var encodingInfo in encodings)
            {
                var encoding = encodingInfo.GetEncoding();
                bom = encoding.GetPreamble();

                if (bom.Length > 0)
                {
                    bool matchesBom = true;
                    for (int i = 0; i < bom.Length; i++)
                    {
                        if (bom[i] != bytes[i])
                        {
                            matchesBom = false;
                            break;
                        }
                    }

                    if (matchesBom)
                    {
                        encodingToUse = encoding;
                        break;
                    }
                }
            }

            if (encodingToUse == null)
            {
                encodingToUse = Encoding.UTF8;
                bom = new byte[0];
            }

            return encodingToUse.GetString(bytes, bom.Length, bytes.Length - bom.Length);
        }

        /// <summary>
        /// Returns the contents of this (unsaved) file.
        /// </summary>
        public byte[] GetContentsAsBytes()
        {
            using (var stream = OpenRead())
            {
                var bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                return bytes;
            }
        }

        /// <summary>
        /// Returns a stream to use for reading the contents.
        /// </summary>
        public Stream OpenRead()
        {
            if (_contents != null)
                return new MemoryStream(_contents);
            return File.OpenRead(FilePath.FullPath);
        }

        /// <summary>
        /// Saves the file to the current file path.
        /// </summary>
        public void Save(IProgressReporter progressReporter)
        {
            if (string.IsNullOrEmpty(FilePath.FullPath))
                throw new NotSupportedException("File path must be specified.");

            if (HasUnsavedData)
            {
                using (var fileStream = File.Create(FilePath.FullPath))
                {
                    if (CurrentDocumentContent != null)
                    {
                        CurrentDocumentContent.Save(fileStream);
                        _contents = null;
                    }
                    else
                    {
                        fileStream.Write(_contents, 0, _contents.Length);
                    }
                    fileStream.Flush();
                }
                HasUnsavedData = false;
            }
        }

        protected virtual void OnContentsChanged(EventArgs e)
        {
            GiveUnsavedData();
            if (ContentsChanged != null)
                ContentsChanged(this, e);
        }

        protected virtual void OnFilePathChanged(PathChangedEventArgs e)
        {
            if (FilePathChanged != null)
                FilePathChanged(this, e);
        }

        protected virtual void OnHasUnsavedDataChanged(EventArgs e)
        {
            if (HasUnsavedDataChanged != null)
                HasUnsavedDataChanged(this, e);
        }

        protected virtual void OnCurrentDocumentContentChanged(EventArgs e)
        {
            if (CurrentDocumentContentChanged != null)
                CurrentDocumentContentChanged(this, e);
        }
    }
}
