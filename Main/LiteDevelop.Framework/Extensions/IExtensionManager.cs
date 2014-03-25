using System;
using System.Collections.Generic;
using System.Linq;
using LiteDevelop.Framework.Extensions;
using LiteDevelop.Framework.FileSystem;

namespace LiteDevelop.Framework.Extensions
{
    /// <summary>
    /// Provides members for getting extensions loaded in LiteDevelop.
    /// </summary>
    public interface IExtensionManager
    {
        /// <summary>
        /// Gets a collection of extensions loaded in LiteDevelop.
        /// </summary>
        IList<LiteExtension> LoadedExtensions { get; }

        /// <summary>
        /// Gets the extension by its type.
        /// </summary>
        /// <param name="extensionType">The type of the extension.</param>
        /// <returns>An instance of the extension type, or null when the extension is not loaded.</returns>
        LiteExtension GetLoadedExtension(Type extensionType);

        /// <summary>
        /// Gets the extension by its type.
        /// </summary>
        /// <typeparam name="T">The type of the extension.</typeparam>
        /// <returns>An instance of the extension type casted to <typeparamref name="T"/>, or null when the extension is not loaded.</returns>
        T GetLoadedExtension<T>() where T : LiteExtension;

        /// <summary>
        /// Gets a collection of extensions that implement the <see cref="LiteDevelop.Framework.Extensions.IFileHandler" /> interface and can be used for opening a specific file
        /// </summary>
        /// <param name="filePath">The file to open.</param>
        /// <returns>An enumerable collection of extensions implementing the <see cref="LiteDevelop.Framework.Extensions.IFileHandler" /> interface.</returns>
        IEnumerable<IFileHandler> GetFileHandlers(FilePath filePath);

        /// <summary>
        /// Gets the preferred extension to use for opening a specific file.
        /// </summary>
        /// <param name="filePath">The file to open.</param>
        /// <returns>An extension implementing the <see cref="LiteDevelop.Framework.Extensions.IFileHandler" /> interface.</returns>
        IFileHandler GetPreferredFileHandler(FilePath filePath);

        /// <summary>
        /// Gets a collection of all extensions that implement the <see cref="LiteDevelop.Framework.Extensions.IDebugger" /> interface and can be used for debugging any project.
        /// </summary>
        /// <returns>An enumerable collection of extensions implementing the <see cref="LiteDevelop.Framework.Extensions.IDebugger" /> interface.</returns>
        IEnumerable<IDebugger> GetDebuggers();

        /// <summary>
        /// Gets a collection of extensions that implement the <see cref="LiteDevelop.Framework.Extensions.IDebugger" /> interface and can be used for debugging a specific project.
        /// </summary>
        /// <param name="project">The project to debug.</param>
        /// <returns>An enumerable collection of extensions implementing the <see cref="LiteDevelop.Framework.Extensions.IDebugger" /> interface.</returns>
        IEnumerable<IDebugger> GetDebuggers(Project project);

        /// <summary>
        /// Gets the preferred extension to use for debugging a speciifc project.
        /// </summary>
        /// <param name="project">The project to debug.</param>
        /// <returns>An extension implementing the <see cref="LiteDevelop.Framework.Extensions.IDebugger" /> interface.</returns>
        IDebugger GetPreferredDebugger(Project project);
    }
}
