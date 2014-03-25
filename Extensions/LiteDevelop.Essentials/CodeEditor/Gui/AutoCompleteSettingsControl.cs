using System;
using System.Collections.Generic;
using System.Linq;
using LiteDevelop.Framework.Extensions;
using LiteDevelop.Framework.Gui;
using LiteDevelop.Framework.Mui;

namespace LiteDevelop.Essentials.CodeEditor.Gui
{
    public partial class AutoCompleteSettingsControl : SettingsControl
    {
        private Dictionary<object, string> _componentMuiIdentifiers;
        private ILiteExtensionHost _extensionHost;
        private MuiProcessor _muiProcessor;
        private CodeEditorSettings _settings;

        public AutoCompleteSettingsControl(ILiteExtensionHost extensionHost, MuiProcessor muiProcessor, CodeEditorSettings settings)
        {
            InitializeComponent();
            _settings = settings;

            _extensionHost = extensionHost;
            _muiProcessor = muiProcessor;

            _componentMuiIdentifiers = new Dictionary<object, string>()
            {
                {this.autoListMembersCheckBox, "AutoCompleteSettingsControl.AutoListMembers.Title"},
                {this.showSuggestionsListWhenLabel, "AutoCompleteSettingsControl.AutoListMembers.PopupTime.Title"},
                {this.commitSelectedItemWhenLabel, "AutoCompleteSettingsControl.AutoListMembers.CommitItemOnKeyChars"},
                {this.completeOnSpaceBarCheckBox, "AutoCompleteSettingsControl.AutoListMembers.CommitItemOnSpaceBar"},
                {this.autoCompleteCodeBlocksCheckBox, "AutoCompleteSettingsControl.AutoCompleteCodeBlocks"},
                {this.autoAddParanthesesCheckBox, "AutoCompleteSettingsControl.AutoAddParantheses"},
            };

            _extensionHost.UILanguageChanged += _extensionHost_UILanguageChanged;
        }
        
        public override void ApplySettings()
        {
            _settings.SetValue("AutoCompletion.AutoComplete", autoListMembersCheckBox.Checked);
            _settings.SetValue("AutoCompletion.ShowSuggestionsWhenTyping", showSuggestionsListComboBox.SelectedIndex == 0);
            _settings.SetValue("AutoCompletion.AutoCompleteCommitChars", autoCompleteCharsTextBox.Text.Trim());
            _settings.SetValue("AutoCompletion.AutoCompleteCommitOnSpaceBar", completeOnSpaceBarCheckBox.Checked);
            _settings.SetValue("AutoCompletion.AutoCompleteCodeBlocks", autoCompleteCodeBlocksCheckBox.Checked);
            _settings.SetValue("AutoCompletion.AutoCompleteMethodParantheses", autoAddParanthesesCheckBox.Checked);
        }

        public override void LoadUserDefinedPresets()
        {
            autoListMembersCheckBox.Checked = _settings.GetValue<bool>("AutoCompletion.AutoComplete");
            showSuggestionsListComboBox.SelectedIndex = Convert.ToInt32(!_settings.GetValue<bool>("AutoCompletion.ShowSuggestionsWhenTyping"));
            autoCompleteCharsTextBox.Text = _settings.GetValue<string>("AutoCompletion.AutoCompleteCommitChars");
            completeOnSpaceBarCheckBox.Checked = _settings.GetValue<bool>("AutoCompletion.AutoCompleteCommitOnSpaceBar");
            autoCompleteCodeBlocksCheckBox.Checked = _settings.GetValue<bool>("AutoCompletion.AutoCompleteCodeBlocks");
            autoAddParanthesesCheckBox.Checked = _settings.GetValue<bool>("AutoCompletion.AutoCompleteMethodParantheses");
        }

        private void autoCompleteCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            panel1.Enabled = autoListMembersCheckBox.Checked;
        }

        private void _extensionHost_UILanguageChanged(object sender, EventArgs e)
        {
            _muiProcessor.ApplyLanguageOnComponents(_componentMuiIdentifiers);
            var languagePack = this._muiProcessor.GetCurrentLanguagePack();
            showSuggestionsListComboBox.Items[0] = languagePack.GetValue("AutoCompleteSettingsControl.AutoListMembers.PopupTime.TypingAnyChar");
            showSuggestionsListComboBox.Items[1] = languagePack.GetValue("AutoCompleteSettingsControl.AutoListMembers.PopupTime.PressingCtrlSpace");

        }
    }
}
