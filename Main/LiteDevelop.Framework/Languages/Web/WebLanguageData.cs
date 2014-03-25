using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace LiteDevelop.Framework.Languages.Web
{
    [XmlRoot("WebLanguageData")]
    public class WebLanguageData : IXmlSerializable
    {
        public string[] Keywords;
        public Snippet[] Snippets;

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            var elementName = string.Empty;
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    elementName = reader.Name;
                    if (elementName == "Snippets")
                    {
                        var subReader = reader.ReadSubtree();
                        var snippets = new List<Snippet>();
                        while (subReader.ReadToFollowing("Snippet"))
                        {
                            var snippet = new Snippet();
                            snippet.ReadXml(subReader);
                            snippets.Add(snippet);
                        }
                        Snippets = snippets.ToArray();
                        break;
                    }

                }
                else if (reader.NodeType == XmlNodeType.Text)
                {
                    switch (elementName)
                    {
                        case "Keywords":
                            Keywords = reader.Value.Split(' ');
                            break;
                    }
                }
                else if (reader.NodeType == XmlNodeType.EndElement)
                {
                    elementName = string.Empty;
                }
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("Keywords", string.Join(" ", Keywords.ToArray()));

            writer.WriteStartAttribute("Snippets");
            foreach (var snippet in Snippets)
                snippet.WriteXml(writer);
            writer.WriteEndAttribute();

        }
    }
}
