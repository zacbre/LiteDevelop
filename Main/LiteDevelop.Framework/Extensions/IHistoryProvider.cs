using System;
using System.Linq;

namespace LiteDevelop.Framework.Extensions
{
    /// <summary>
    /// Provides methods for undo and redo operations.
    /// </summary>
    public interface IHistoryProvider
    {
        /// <summary>
        /// Undoes the last operation.
        /// </summary>
        void Undo();

        /// <summary>
        /// Redoes the last undone operation.
        /// </summary>
        void Redo();
    }
}
