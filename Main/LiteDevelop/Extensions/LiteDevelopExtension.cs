using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using LiteDevelop.Framework;
using LiteDevelop.Framework.Extensions;
using LiteDevelop.Framework.Gui;
using LiteDevelop.Framework.Mui;
using LiteDevelop.Gui.Settings;

namespace LiteDevelop.Extensions
{
    public class LiteDevelopExtension : LiteExtension, ISettingsProvider, IAppearanceMapProvider 
    {
        private Dictionary<object, string> _componentMuiIdentifiers;
        private SettingsNode _settingsNode;
        private LiteDevelopSettings _settings;
        private LiteExtensionHost _extensionHost;
        private AppearanceMap _appearanceMap;
        private AppearanceMap _defaultAppearanceMap;
        private string _appearanceMapPath;

        public LiteDevelopExtension()
        {
        }

        #region LiteExtension Members

        public override string Name
        {
            get { return "LiteDevelop"; }
        }

        public override string Description
        {
            get { return "Free and open source IDE."; }
        }

        public override string Author
        {
            get { return "Jerre S."; }
        }

        public override Version Version
        {
            get { return Version.Parse(Application.ProductVersion); }
        }

        public override string Copyright
        {
            get { return "Copyright © Jerre S. 2014"; }
        }

        public override void Initialize(ILiteExtensionHost extensionHost)
        {
            _extensionHost = (LiteExtensionHost)extensionHost;

            var generalSettingsEditorControl = new GeneralSettingsEditor(_settings = LiteDevelopSettings.Instance) { Dock = DockStyle.Fill };
            var internationalSettingsEditorControl = new InternationalSettingsEditor(_settings) { Dock = DockStyle.Fill };
            var appearanceEditorControl = new AppearanceEditor() { Dock = DockStyle.Fill };

            var generalSettingsNode = new SettingsNode("General", generalSettingsEditorControl);
            var internationalSettingsNode = new SettingsNode("International", internationalSettingsEditorControl);
            var appearanceNode = new SettingsNode("Appearance", appearanceEditorControl);

            _settingsNode = new SettingsNode("LiteDevelop", generalSettingsEditorControl);
            _settingsNode.Nodes.AddRange(new TreeNode[] 
            { 
                generalSettingsNode, 
                appearanceNode,
                internationalSettingsNode, 
            });

            _componentMuiIdentifiers = new Dictionary<object, string>()
            {
                {generalSettingsNode, "LiteDevelopExtension.GeneralSettings"},
                {appearanceNode, "LiteDevelopExtension.Appearance"},
                {internationalSettingsNode, "LiteDevelopExtension.InternationalSettings"},
            };

            extensionHost.UILanguageChanged += extensionHost_UILanguageChanged;
            extensionHost_UILanguageChanged(null, null);

            _appearanceMapPath = Path.Combine(Constants.AppDataDirectory, "appearance.xml");
            _defaultAppearanceMap  = AppearanceMap.LoadFromFile(Path.Combine(
                    Path.GetDirectoryName(typeof(LiteDevelopExtension).Assembly.Location),
                    "default_appearance.xml"));

            try { _appearanceMap = AppearanceMap.LoadFromFile(_appearanceMapPath); }
            catch { _appearanceMap = _defaultAppearanceMap; }
        }

        #endregion

        #region ISettingsProvider Members

        public event EventHandler AppliedSettings;

        public void ApplySettings()
        {
            RootSettingsNode.ApplySettingsInAllNodes();
            _extensionHost.UILanguage = UILanguage.GetLanguageById(_settings.GetValue("Application.LanguageID"));
            _settings.Save();

            _appearanceMap.Save(_appearanceMapPath);

            if (AppliedSettings != null)
                AppliedSettings(this, EventArgs.Empty);
        }

        public void LoadUserDefinedPresets()
        {
            RootSettingsNode.LoadUserDefinedPresetsInAllNodes();
        }

        public void ResetSettings()
        {
            LiteDevelopSettings.Reset();
            _defaultAppearanceMap.CopyTo(_appearanceMap);
        }

        public SettingsNode RootSettingsNode
        {
            get { return _settingsNode; }
        }

        #endregion

        #region IAppearanceMapProvider Members

        public AppearanceMap CurrentAppearanceMap
        {
            get { return _appearanceMap; }
        }

        public AppearanceMap DefaultAppearanceMap
        {
            get { return _defaultAppearanceMap; }
        }
        
        #endregion

        private void extensionHost_UILanguageChanged(object sender, EventArgs e)
        {
            LiteDevelopApplication.Current.MuiProcessor.ApplyLanguageOnComponents(_componentMuiIdentifiers);
        }
    }
}
