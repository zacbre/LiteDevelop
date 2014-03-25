using System;
using System.Linq;

namespace LiteDevelop.Framework.FileSystem
{
    /// <summary>
    /// Provides members and events for holding a file path.
    /// </summary>
    public interface IFilePathProvider
    {
        /// <summary>
        /// Occurs when the file path has changed.
        /// </summary>
        event PathChangedEventHandler FilePathChanged;

        /// <summary>
        /// Gets or sets the file path of this object.
        /// </summary>
        FilePath FilePath { get; set; }
    }
}
