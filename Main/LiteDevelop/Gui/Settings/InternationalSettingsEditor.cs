using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using LiteDevelop.Framework.Gui;
using LiteDevelop.Framework.Mui;

namespace LiteDevelop.Gui.Settings
{
    public partial class InternationalSettingsEditor : SettingsControl
    {
        private LiteDevelopSettings _settings;

        public InternationalSettingsEditor(LiteDevelopSettings settings)
        {
            InitializeComponent();
            _settings = settings;
            foreach (var language in UILanguage.InstalledLanguages)
                comboBox1.Items.Add(language.Name);
        }

        public override void ApplySettings()
        {
            var language = UILanguage.GetLanguageByName(comboBox1.Text);

            string path = Path.Combine(Application.StartupPath, "MUI", language.PackIdentifier + ".xml");
            if (!File.Exists(path))
                MessageBox.Show(string.Format("The language pack for {0} is not installed for the main application of LiteDevelop. Therefore some parts of the program might contain english texts instead of texts in the selected language.", comboBox1.Text), "LiteDevelop", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            _settings.SetValue("Application.LanguageID", language.PackIdentifier);

        }

        public override void LoadUserDefinedPresets()
        {
            var language = UILanguage.GetLanguageById(_settings.GetValue("Application.LanguageID"));
            comboBox1.SelectedItem = language.Name;
        }
    }
}
