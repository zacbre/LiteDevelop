using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using LiteDevelop.Framework;
using LiteDevelop.Framework.Extensions;
using LiteDevelop.Framework.FileSystem;

namespace LiteDevelop.Essentials.CodeEditor
{
    public class CodeEditorSettings : SettingsMap
    {
        public static CodeEditorSettings Default { get; private set; }

        private static FilePath _defaultPath = new FilePath(typeof(CodeEditorSettings).Assembly.Location).ParentDirectory.Combine("CodeEditor", "default_settings.xml");

        static CodeEditorSettings()
        {
            Default = new CodeEditorSettings(_defaultPath);
        }

        public CodeEditorSettings(FilePath filePath)
            : base(filePath)
        {
            FallbackMap = Default;
        }

        public override string DocumentRoot
        {
            get { return "Settings"; }
        }

        public static CodeEditorSettings LoadSettings(ISettingsManager manager)
        {
            string settingsFile = Path.Combine(manager.GetSettingsDirectory(CodeEditorExtension.Instance), "settings.xml");
            if (!File.Exists(settingsFile))
            {
                return new CodeEditorSettings(_defaultPath);
            }

            return new CodeEditorSettings(new FilePath(settingsFile));
        }

        public void SaveSettings(ISettingsManager manager)
        {
            string settingsFile = Path.Combine(manager.GetSettingsDirectory(CodeEditorExtension.Instance), "settings.xml");

            Save(new FilePath(settingsFile));
        }

    }
}
