using System;
using System.Linq;

namespace LiteDevelop.Framework.Extensions
{
    /// <summary>
    /// Provides methods for doing operations that accesses or modifies the clipboard.
    /// </summary>
    public interface IClipboardHandler
    {
        /// <summary>
        /// Gets a value indicating whether the cut function. is available or not.
        /// </summary>
        bool IsCutEnabled { get; }

        /// <summary>
        /// Gets a value indicating whether the copy function. is available or not.
        /// </summary>
        bool IsCopyEnabled { get; }

        /// <summary>
        /// Gets a value indicating whether the paste function. is available or not.
        /// </summary>
        bool IsPasteEnabled { get; }

        /// <summary>
        /// Cuts the current selected data and places it onto the clipboard.
        /// </summary>
        void Cut();

        /// <summary>
        /// Copies the current selected data and places it onto the clipboard.
        /// </summary>
        void Copy();

        /// <summary>
        /// Pastes the clipboard data to the current position.
        /// </summary>
        void Paste();
    }
}
