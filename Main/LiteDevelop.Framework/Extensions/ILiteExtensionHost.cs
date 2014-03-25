using System;
using System.Collections.Generic;
using System.Linq;
using LiteDevelop.Framework.Debugging;
using LiteDevelop.Framework.FileSystem;
using LiteDevelop.Framework.Mui;

namespace LiteDevelop.Framework.Extensions
{
    /// <summary>
    /// Provides functions and properties of the LiteDevelop extension host to use for an extension.
    /// </summary>
    public interface ILiteExtensionHost
    {
        /// <summary>
        /// Occurs when a solution has been created in LiteDevelop.
        /// </summary>
        event SolutionEventHandler SolutionCreated;

        /// <summary>
        /// Occurs when a solution has been loaded in LiteDevelop.
        /// </summary>
        event SolutionEventHandler SolutionLoad;

        /// <summary>
        /// Occurs when a solution has been unloaded in LiteDevelop.
        /// </summary>
        event SolutionEventHandler SolutionUnload;

        /// <summary>
        /// Occurs when the current language of the UI of LiteDevelop has changed.
        /// </summary>
        event EventHandler UILanguageChanged;

        /// <summary>
        /// Occurs when the user starts debugging an application.
        /// </summary>
        event EventHandler DebugStarted;
        
        /// <summary>
        /// Gets the current language of the UI of LiteDevelop.
        /// </summary>
        UILanguage UILanguage { get; }
        
        /// <summary>
        /// Gets the current loaded solution.
        /// </summary>
        Solution CurrentSolution { get; }

        /// <summary>
        /// Gets the current selected project container.
        /// </summary>
        Project GetCurrentSelectedProject();

        /// <summary>
        /// Gets the extension manager of LiteDevelop.
        /// </summary>
        IExtensionManager ExtensionManager { get; }

        /// <summary>
        /// Gets an instance of an object that handles operations such as adding and removing controls.
        /// </summary>
        IControlManager ControlManager { get; }

        /// <summary>
        /// Gets the file service of LiteDevelop.
        /// </summary>
        IFileService FileService { get; }

        /// <summary>
        /// Gets the bookmark manager of LiteDevelop.
        /// </summary>
        IBookmarkManager BookmarkManager { get; }

        /// <summary>
        /// Gets an instance of an object that handles operations such as reporting errors.
        /// </summary>
        IErrorManager ErrorManager { get; }

        /// <summary>
        /// Gets the settings manager of LiteDevelop.
        /// </summary>
        ISettingsManager SettingsManager { get; }

        /// <summary>
        /// Gets a collection of registered named progress reporters viewed in the output window of LiteDevelop.
        /// </summary>
        IList<INamedProgressReporter> ProgressReporters { get; }

        /// <summary>
        /// Creates or gets a reporter attached to the output window of LiteDevelop by its identifier.
        /// </summary>
        /// <param name="id">The reporter id.</param>
        /// <returns>A named progress reporter attached to the output window of LiteDevelop.</returns>
        INamedProgressReporter CreateOrGetReporter(string id);

        /// <summary>
        /// Gets the manager that handles credential requests.
        /// </summary>
        ICredentialManager CredentialManager { get; }

        /// <summary>
        /// Gets a value indicating whether the user is debugging the current solution.
        /// </summary>
        bool IsDebugging { get; }

        /// <summary>
        /// Gets the debugger session that is currently in use.
        /// </summary>
        DebuggerSession CurrentDebuggerSession { get; }
    }
}
