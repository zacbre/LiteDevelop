using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LiteDevelop.Framework.Gui;

namespace LiteDevelop.Gui.Settings
{
    public partial class GeneralSettingsEditor : SettingsControl
    {
        private readonly System.Collections.Generic.Dictionary<object, string> _componentMuiIdentifiers;
        private readonly LiteDevelopSettings _settings;

        public GeneralSettingsEditor(LiteDevelopSettings settings)
        {
            InitializeComponent();
            _settings = settings;

            _componentMuiIdentifiers = new Dictionary<object, string>()
            {
                {this.defaultProjectsPathLabel, "GeneralSettingsEditor.DefaultProjectsPath"},
                {this.browseButton, "Common.Browse"},
                {this.outputWindowCheckBox, "GeneralSettingsEditor.ShowOutput"},
                {this.errorListCheckBox, "GeneralSettingsEditor.ShowErrorList"},
            };

            LiteDevelopApplication.Current.ExtensionHost.UILanguageChanged += ExtensionHost_UILanguageChanged;
            ExtensionHost_UILanguageChanged(null, null);
        }

        public override void ApplySettings()
        {
            _settings.SetValue("Projects.DefaultProjectsPath", defaultPathTextBox.Text);
            _settings.SetValue("Projects.ShowOutputWhenBuilding", outputWindowCheckBox.Checked);
            _settings.SetValue("Projects.ShowErrorsWhenBuildFailed", errorListCheckBox.Checked);
        }

        public override void LoadUserDefinedPresets()
        {
            defaultPathTextBox.Text = _settings.GetValue("Projects.DefaultProjectsPath");
            outputWindowCheckBox.Checked = _settings.GetValue<bool>("Projects.ShowOutputWhenBuilding");
            errorListCheckBox.Checked = _settings.GetValue<bool>("Projects.ShowErrorsWhenBuildFailed");
        }

        private void ExtensionHost_UILanguageChanged(object sender, EventArgs e)
        {
            LiteDevelopApplication.Current.MuiProcessor.ApplyLanguageOnComponents(_componentMuiIdentifiers);
        }
        
		private void browseButton_Click(object sender, System.EventArgs e)
		{
			using (var dialog = new FolderBrowserDialog())
			{
				dialog.SelectedPath = defaultPathTextBox.Text;
				if (dialog.ShowDialog() == DialogResult.OK)
				{
					defaultPathTextBox.Text = dialog.SelectedPath;
				}
			}
		}
    }
}
