using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace LiteDevelop.Framework.Languages.Net
{
    [XmlRoot("NetLanguageData")]
    public class NetLanguageData : IXmlSerializable
    {
        public string[] Modifiers;
        public string[] Keywords;
        public string[] MemberIdentifiers;
        public NetTypeAlias[] TypeAliases;
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
                    switch (elementName)
                    {
                        case "TypeAliases":
                            {
                                var subReader = reader.ReadSubtree();
                                var aliases = new List<NetTypeAlias>();
                                while (subReader.ReadToFollowing("TypeAlias"))
                                {
                                    var aliasReader = subReader.ReadSubtree();
                                    var typeAlias = new NetTypeAlias();
                                    typeAlias.ReadXml(aliasReader);
                                    aliases.Add(typeAlias);
                                }
                                TypeAliases = aliases.ToArray();
                                break;
                            }
                        case "Snippets":
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
                }
                else if (reader.NodeType == XmlNodeType.Text)
                {
                    switch (elementName)
                    {
                        case "Modifiers":
                            Modifiers = reader.Value.Split(' ');
                            break;
                        case "MemberIdentifiers":
                            MemberIdentifiers = reader.Value.Split(' ');
                            break;
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
            writer.WriteElementString("Modifiers", string.Join(" ", Modifiers.ToArray()));
            writer.WriteElementString("MemberIdentifiers", string.Join(" ", MemberIdentifiers.ToArray()));
            writer.WriteElementString("Keywords", string.Join(" ", Keywords.ToArray()));
            var serializer = new XmlSerializer(typeof(List<NetTypeAlias>));
            serializer.Serialize(writer, TypeAliases);

            writer.WriteStartAttribute("Snippets");
            foreach (var snippet in Snippets)
                snippet.WriteXml(writer);
            writer.WriteEndAttribute();
            
        }
    }

    [XmlRoot("TypeAlias")]
    public struct NetTypeAlias : IXmlSerializable 
    {
        public NetTypeAlias(string newName, string original)
        {
            NewName = newName;
            OriginalType = original;
        }

        public string NewName;
        public string OriginalType;

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
                }
                if (reader.NodeType == XmlNodeType.Text)
                {
                    if (elementName == "NewName")
                        NewName = reader.Value;
                    else if (elementName == "Original")
                        OriginalType = reader.Value;
                }
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("Original", OriginalType);
            writer.WriteElementString("NewName", NewName);
        }
    }
}
