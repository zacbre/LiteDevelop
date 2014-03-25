using System;
using System.IO;
using System.Linq;
using System.Xml;

namespace LiteDevelop.Framework.Mui
{
    /// <summary>
    /// Represents a language for the Multilingual User Interface (MUI) of LiteDevelop.
    /// </summary>
    public class UILanguage
    {
        private static readonly UILanguage[] _installedLanguages;
        
        static UILanguage()
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(Path.Combine(Path.GetDirectoryName(typeof(UILanguage).Assembly.Location), "MUI", "languages.xml"));

            var nodes = xmlDocument.GetElementsByTagName("Language");
            _installedLanguages = new UILanguage[nodes.Count];

            for (int i = 0; i < nodes.Count; i++)
            {
                _installedLanguages[i] = new UILanguage(nodes[i].Attributes["name"].Value, nodes[i].Attributes["id"].Value);
            }
        }

        public static UILanguage[] InstalledLanguages
        {
            get { return _installedLanguages; }
        }
        
        public static UILanguage Default
        {
            get { return GetLanguageByName("english"); }
        }

        public static UILanguage GetLanguageByName(string name)
        {
            return _installedLanguages.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public static UILanguage GetLanguageById(string id)
        {
            return _installedLanguages.FirstOrDefault(x => x.PackIdentifier.Equals(id, StringComparison.OrdinalIgnoreCase));
        }

        public UILanguage(string name, string packid)
        {
            Name = name;
            PackIdentifier = packid;
        }

        public string Name
        {
            get;
            private set;
        }

        public string PackIdentifier
        {
            get;
            private set;
        }
    }
}
