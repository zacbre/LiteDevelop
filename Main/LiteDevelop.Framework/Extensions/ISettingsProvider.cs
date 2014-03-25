using System;
using System.Linq;
using LiteDevelop.Framework.Gui;

namespace LiteDevelop.Framework.Extensions
{
    /// <summary>
    /// Provides members to work with the settings dialog of LiteDevelop
    /// </summary>
    public interface ISettingsProvider
    {
        /// <summary>
        /// Occurs when settings are applied.
        /// </summary>
        event EventHandler AppliedSettings;

        /// <summary>
        /// Applies the current state of the settings.
        /// </summary>
        void ApplySettings();

        /// <summary>
        /// Loads the user defined presets of the settings.
        /// </summary>
        void LoadUserDefinedPresets();

        /// <summary>
        /// Resets the settings to its default state.
        /// </summary>
        void ResetSettings();

        /// <summary>
        /// Gets the root settings node to use in the settings dialog of LiteDevelop.
        /// </summary>
        SettingsNode RootSettingsNode { get; }
    }
}
