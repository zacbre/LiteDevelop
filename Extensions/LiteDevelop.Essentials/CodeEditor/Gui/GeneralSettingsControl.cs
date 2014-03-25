using System;
using System.Collections.Generic;
using System.Linq;
using LiteDevelop.Framework.Extensions;
using LiteDevelop.Framework.Gui;
using LiteDevelop.Framework.Mui;

namespace LiteDevelop.Essentials.CodeEditor.Gui
{
    public partial class GeneralSettingsControl : SettingsControl
    {
        private Dictionary<object, string> _componentMuiIdentifiers;
        private ILiteExtensionHost _extensionHost;
        private MuiProcessor _muiProcessor;
        private CodeEditorSettings _settings;

        public GeneralSettingsControl(ILiteExtensionHost extensionHost, MuiProcessor muiProcessor, CodeEditorSettings settings)
        {
            InitializeComponent();
            _settings = settings;

            _extensionHost = extensionHost;
            _muiProcessor = muiProcessor;

            _componentMuiIdentifiers = new Dictionary<object, string>()
            {
                {this.lineNumbersCheckBox, "GeneralSettingsControl.ShowLineNumbers"},
                {this.wordWrapCheckBox, "GeneralSettingsControl.WordWrap"},
                {this.syntaxHighlightingCheckBox, "GeneralSettingsControl.SyntaxHighlighting"},
                {this.trackChangesCheckBox, "GeneralSettingsControl.TrackUnsavedChanges"},
                {this.highLightCurrentLineCheckBox, "GeneralSettingsControl.HighlightCurrentLine"},
                {this.documentMiniMapCheckBox, "GeneralSettingsControl.ShowDocumentMiniMap"},
            };

            _extensionHost.UILanguageChanged += _extensionHost_UILanguageChanged;
        }

        public override void ApplySettings()
        {
            _settings.SetValue("General.LineNumbers", lineNumbersCheckBox.Checked);
            _settings.SetValue("General.WordWrap", wordWrapCheckBox.Checked);
            _settings.SetValue("General.SyntaxHighlighting", syntaxHighlightingCheckBox.Checked);
            _settings.SetValue("General.TrackUnsavedChanges", trackChangesCheckBox.Checked);
            _settings.SetValue("General.HighlightCurrentLine", highLightCurrentLineCheckBox.Checked);
            _settings.SetValue("General.ShowDocumentMiniMap", documentMiniMapCheckBox.Checked);
        }

        public override void LoadUserDefinedPresets()
        {
            lineNumbersCheckBox.Checked = _settings.GetValue<bool>("General.LineNumbers");
            wordWrapCheckBox.Checked = _settings.GetValue<bool>("General.WordWrap");
            syntaxHighlightingCheckBox.Checked = _settings.GetValue<bool>("General.SyntaxHighlighting");
            trackChangesCheckBox.Checked = _settings.GetValue<bool>("General.TrackUnsavedChanges");
            highLightCurrentLineCheckBox.Checked = _settings.GetValue<bool>("General.HighlightCurrentLine");
            documentMiniMapCheckBox.Checked = _settings.GetValue<bool>("General.ShowDocumentMiniMap");
        }

        private void _extensionHost_UILanguageChanged(object sender, EventArgs e)
        {
            _muiProcessor.ApplyLanguageOnComponents(_componentMuiIdentifiers);
        }
    }
}
