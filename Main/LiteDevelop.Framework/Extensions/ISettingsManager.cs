using System;
using System.Linq;

namespace LiteDevelop.Framework.Extensions
{
    /// <summary>
    /// Provides members for getting file and folder locations of settings.
    /// </summary>
    public interface ISettingsManager
    {
        /// <summary>
        /// Gets the settings directory of a specific extension.
        /// </summary>
        /// <param name="extension">The extension to get the directory from.</param>
        /// <returns>The path to a directory.</returns>
        string GetSettingsDirectory(LiteExtension extension);
    }
}
