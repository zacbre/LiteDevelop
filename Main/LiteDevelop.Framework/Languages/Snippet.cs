using System;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace LiteDevelop.Framework.Languages
{
	public class Snippet : IXmlSerializable
	{
		public Snippet()
		{

		}

		public Snippet(string title, string code)
		{
			Title = title;
			Code = code;
		}

		public string Title 
		{
			get;
			set;
		}

		public string Code
		{
			get;
			set;
		}

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {

        	Title = reader.GetAttribute("title");
        	Code = reader.ReadString();
        }

        public void WriteXml(XmlWriter writer)
        {   
        	writer.WriteStartElement("Snippet"); 
			writer.WriteAttributeString("title", Title);
			writer.WriteString(Code);
			writer.WriteEndElement();
        }
	}
}