using System;
using System.Linq;
using System.Windows.Forms;
using LiteDevelop.Framework.Gui;

namespace LiteDevelop.Framework.Extensions
{
    /// <summary>
    /// Provides members for control interaction with LiteDevelop.
    /// </summary>
    public interface IControlManager
    {
        /// <summary>
        /// Gets a collecton of document view contents opened in LiteDevelop.
        /// </summary>
        EventBasedCollection<LiteDocumentContent> OpenDocumentContents { get; }

        /// <summary>
        /// Gets or sets the selected document view content in LiteDevelop.
        /// </summary>
        LiteDocumentContent SelectedDocumentContent { get; set; }

        /// <summary>
        /// Occurs when the user has switched to another document view content.
        /// </summary>
        event EventHandler SelectedDocumentContentChanged;

        /// <summary>
        /// Gets a collection of tool windows added by extensions in LiteDevelop.
        /// </summary>
        EventBasedCollection<LiteToolWindow> ToolWindows { get; }

        /// <summary>
        /// Gets a collection of menu items added by extensions in LiteDevelop.
        /// </summary>
        EventBasedCollection<ToolStripMenuItem> MenuItems { get; }

        /// <summary>
        /// Gets a collection of menu items added to the "Edit" menu by extensions in LiteDevelop.
        /// </summary>
        EventBasedCollection<ToolStripItem> EditMenuItems { get; }

        /// <summary>
        /// Gets a collection of menu items added to the "View" menu by extensions in LiteDevelop.
        /// </summary>
        EventBasedCollection<ToolStripItem> ViewMenuItems { get; }

        /// <summary>
        /// Gets a collection of menu items added to the "Debug" menu by extensions in LiteDevelop.
        /// </summary>
        EventBasedCollection<ToolStripItem> DebugMenuItems { get; }

        /// <summary>
        /// Gets a collection of menu items added to the "Tools" menu by extensions in LiteDevelop.
        /// </summary>
        EventBasedCollection<ToolStripItem> ToolsMenuItems { get; }

        /// <summary>
        /// Gets a collection of items viewed in the status bar added by extensions in LiteDevelop.
        /// </summary>
        EventBasedCollection<ToolStripItem> StatusBarItems { get; }

        /// <summary>
        /// Gets a collection of toolbars added by extensions in LiteDevelop.
        /// </summary>
        EventBasedCollection<ToolStrip> ToolBars { get; }

        /// <summary>
        /// Gets a collection of menu items added to the solution explorer context menu by extensions in LiteDevelop.
        /// </summary>
        EventBasedCollection<ToolStripItem> SolutionExplorerItems { get; }

        /// <summary>
        /// Gets the global appearance settings used by the main application.
        /// </summary>
        AppearanceMap GlobalAppearanceMap { get; }

        /// <summary>
        /// Gets the renderer being used by the menu- and toolbars of LiteDevelop.
        /// </summary>
        ToolStripRenderer MenuRenderer { get; }

        /// <summary>
        /// Occurs when the global appearance map or menu renderer has been changed by the user.
        /// </summary>
        event EventHandler AppearanceChanged;
    }
}
