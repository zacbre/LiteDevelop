using System;
using System.Linq;

namespace LiteDevelop.Framework.FileSystem
{
    /// <summary>
    /// Provides members for holding references.
    /// </summary>
    public interface IFileReferenceProvider
    {
        /// <summary>
        /// Gets a collection of references in string format.
        /// </summary>
        EventBasedCollection<string> References { get; }
    }
}
