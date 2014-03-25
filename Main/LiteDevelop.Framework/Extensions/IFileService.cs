
using System;
using System.Collections.Generic;
using System.Linq;
using LiteDevelop.Framework.FileSystem;

namespace LiteDevelop.Framework.Extensions
{
    /// <summary>
    /// Provides members for file operations such as creating and opening files in LiteDevelop.
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Occurs when a file has been created by the user in LiteDevelop.
        /// </summary>
        event FileEventHandler FileCreated;

        /// <summary>
        /// Occurs when a file has been loaded by the user in LiteDevelop.
        /// </summary>
        event FileEventHandler FileOpened;

        /// <summary>
        /// Occurs when a file has been copied to a new destination by this service.
        /// </summary>
        event PathChangedEventHandler FileCopied;

        /// <summary>
        /// Occurs when a file has been moved to a new destination by this service.
        /// </summary>
        event PathChangedEventHandler FileMoved;

        /// <summary>
        /// Occurs when a file has been deleted by this service.
        /// </summary>
        event PathEventHandler FileDeleted;

        /// <summary>
        /// Occurs when a directory has been created by this service.
        /// </summary>
        event PathEventHandler DirectoryCreated;

        /// <summary>
        /// Occurs when a directory has been moved to a new destination by this service.
        /// </summary>
        event PathChangedEventHandler DirectoryMoved;

        /// <summary>
        /// Occurs when a directory has been deleted by this service.
        /// </summary>
        event PathEventHandler DirectoryDeleted;

        /// <summary>
        /// Get all opened files.
        /// </summary>
        IList<OpenedFile> OpenedFiles { get; }

        /// <summary>
        /// Selects a file handler from a collection to use for opening a file.
        /// </summary>
        /// <param name="fileHandlers">The file handlers to select from.</param>
        /// <param name="filePath">The file to open.</param>
        /// <returns> If there are no elements in the collection, this method will return null. If there is a single one, 
        /// the file handler will be returned immediately. If there are multiple ones, the user will be prompted with a 
        /// dialog containing the possible content editors, and returns the one selected by the user, or null if 
        /// the user canceled.</returns>
        IFileHandler SelectFileHandler(IEnumerable<IFileHandler> fileHandlers, FilePath filePath);

        /// <summary>
        /// Gets an opened file by its file path.
        /// </summary>
        /// <param name="filePath">The file path to the opened file.</param>
        /// <returns>An opened file with the specified file path, or null if the file is not opened.</returns>
        OpenedFile GetOpenedFile(FilePath filePath);

        /// <summary>
        /// Opens or gets a file.
        /// </summary>
        /// <param name="filePath">The file path to the opened file.</param>
        /// <returns>An opened file with the specified file path.</returns>
        OpenedFile OpenFile(FilePath filePath);

        /// <summary>
        /// Creates and opens a new file.
        /// </summary>
        /// <param name="filePath">The file path for new the file.</param>
        /// <param name="contents">The file contents for new the file.</param>
        /// <returns>An opened file with the specified file path and contents.</returns>
        OpenedFile CreateFile(FilePath filePath, byte[] contents);

        /// <summary>
        /// Moves or renames a file to a new destination, and updates any occurance
        /// </summary>
        /// <param name="filePath">The file to be moved.</param>
        /// <param name="newPath">The path to move the file to.</param>
        void MoveFile(FilePath filePath, FilePath newPath);

        /// <summary>
        /// Copies a file to a new destination.
        /// </summary>
        /// <param name="file">The file to copy.</param>
        /// <param name="newPath">The destination of the copied file.</param>
        void CopyFile(FilePath file, FilePath newPath);
        
        /// <summary>
        /// Deletes a file and closes all document view editors associated with the specified file.
        /// </summary>
        /// <param name="filePath">The file to delete.</param>
        void DeleteFile(FilePath filePath);

        /// <summary>
        /// Creates a directory and closes all document view editors associated with files inside this directory.
        /// </summary>
        /// <param name="path">The path of the new directory.</param>
        void CreateDirectory(FilePath path);

        /// <summary>
        /// Moves a directory and updates all opened files.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="newPath"></param>
        void MoveDirectory(FilePath path, FilePath newPath);

        /// <summary>
        /// Deletes a directory and closes all opened files inside this directory.
        /// </summary>
        /// <param name="path"></param>
        void DeleteDirectory(FilePath path);
    }
}
