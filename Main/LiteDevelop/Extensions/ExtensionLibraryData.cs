using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using LiteDevelop.Framework.FileSystem;

namespace LiteDevelop.Extensions
{
    [TypeConverter(typeof(ExtensionLibraryDataConverter))]
    public class ExtensionLibraryData : IXmlSerializable
    {
        public ExtensionLibraryData()
        {
        }

        public ExtensionLibraryData(string filePath)
        {
            if (IsRelative = filePath.StartsWith(Application.StartupPath))
                AssemblyPath = new FilePath(filePath).GetRelativePath(Application.StartupPath);
            else 
                AssemblyPath = filePath;
        }

        public string AssemblyPath { get; set; }

        public bool IsRelative { get; set; }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            IsRelative = bool.Parse(reader.GetAttribute("isRelative"));
            AssemblyPath = reader.ReadElementString();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("isRelative", IsRelative.ToString());
            writer.WriteString(AssemblyPath);
        }

        public string GetAbsolutePath()
        {
            if (IsRelative)
            {
                return new FilePath(Application.StartupPath, this.AssemblyPath).FullPath;
            }

            return this.AssemblyPath;
        }
    }

    public class ExtensionLibraryDataConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            string rawPath = value as string;

            if (Path.IsPathRooted(rawPath))
                return new ExtensionLibraryData(rawPath);
            else
                return new ExtensionLibraryData(new FilePath(Application.StartupPath, rawPath).FullPath);

        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            return (value as ExtensionLibraryData).AssemblyPath;
        }
    }
}
