using System;
using System.Linq;

namespace LiteDevelop.Framework.FileSystem
{
    /// <summary>
    /// Provides members for saving data.
    /// </summary>
    public interface ISavableFile : IFilePathProvider
    {
        /// <summary>
        /// Occurs when the object changes its state.
        /// </summary>
        event EventHandler HasUnsavedDataChanged;

        /// <summary>
        /// Gets a value indicating whether the object has unsaved data.
        /// </summary>
        bool HasUnsavedData { get; }

        /// <summary>
        /// Marks the object as unsaved.
        /// </summary>
        void GiveUnsavedData();

        /// <summary>
        /// Saves the object and marks it as saved.
        /// </summary>
        /// <param name="progressReporter">The progress reporter to use for logging.</param>
        void Save(IProgressReporter progressReporter);
    }
}
