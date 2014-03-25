using System;
using System.Linq;
using LiteDevelop.Framework.FileSystem;

namespace LiteDevelop.Framework.Extensions
{
    /// <summary>
    /// Defines methods to open project files.
    /// </summary>
    public interface IFileHandler
    {
        /// <summary>
        /// Determines whether this file handler can open a specific file.
        /// </summary>
        /// <param name="filePath">The path to the file to check.</param>
        /// <returns><c>True</c> if the file handler can open the file, otherwise <c>False</c>.</returns>
        bool CanOpenFile(FilePath filePath);

        /// <summary>
        /// Loads a file into the extension.
        /// </summary>
        /// <param name="file">The file to be loaded in the extension.</param>
        void OpenFile(OpenedFile file);
    }
}
