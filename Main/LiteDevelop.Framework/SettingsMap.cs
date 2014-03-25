using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using LiteDevelop.Framework.FileSystem;

namespace LiteDevelop.Framework
{
    /// <summary>
    /// Represents a dynamic settings map which can be serialized to an xml file.
    /// </summary>
    public abstract class SettingsMap : ICloneable
    {
        private static readonly Regex _parametersRegex = new Regex(@"\$\((?<parameter>[\w\.]+)\)");
        private Dictionary<string, object> _nodes = new Dictionary<string, object>();
        
        public SettingsMap()
        {

        }

        public SettingsMap(FilePath filePath)
        {
            using (var fileStream = File.OpenRead(filePath.FullPath))
            {
                using (var reader = XmlReader.Create(fileStream))
                {
                    ReadItems(reader, string.Empty);
                }
            }
        }

        /// <summary>
        /// Gets the document root element name of the xml file.
        /// </summary>
        public virtual string DocumentRoot
        {
            get { return "Settings"; }
        }

        /// <summary>
        /// Gets or sets the settings map to use when a value cannot be found using the <see cref="GetValue" /> method.
        /// </summary>
        public SettingsMap FallbackMap
        {
            get;
            set;
        }

        private void ReadItems(XmlReader reader, string folder)
        {
            reader.ReadStartElement();
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name == "ItemGroup")
                    {
                        string groupName = reader.GetAttribute("Id");
                        using (var subReader = reader.ReadSubtree())
                        {
                            ReadItems(subReader, GetFullName(folder, groupName));
                        }
                    }
                    else if (reader.Name == "ItemArray")
                    {
                        string itemName = reader.GetAttribute("Id");
                        string[] elements = null;

                        using (var subReader = reader.ReadSubtree())
                        {
                            elements = ReadArray(subReader);
                        }

                        _nodes.Add(GetFullName(folder, itemName), elements);
                    }
                    else if (reader.Name == "Item")
                    {
                        string itemName = reader.GetAttribute("Id");
                        _nodes.Add(GetFullName(folder, itemName), reader.ReadElementString());
                    }
                }
            }
        }

        private string[] ReadArray(XmlReader reader)
        {
            List<string> elements = new List<string>();
            reader.ReadStartElement();

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "Element")
                {
                    elements.Add(reader.ReadElementString());
                }
            }

            return elements.ToArray();
        }

        private string GetFullName(string folder, string id)
        {
            return (string.IsNullOrEmpty(folder) ? id : folder + "." + id);
        }
        
        /// <summary>
        /// Gets a settings value by its settings path in string format.
        /// </summary>
        /// <param name="path">The path to get the setting from.</param>
        /// <returns>The string instance of the setting defined in the settings mapping.</returns>
        public string GetValue(string path)
        {
            return GetValue<string>(path);
        }

        /// <summary>
        /// Gets a settings value by its settings path in a specific type format.
        /// </summary>
        /// <typeparam name="T">The type of the requested settings value.</typeparam>
        /// <param name="path">The path to get the setting from.</param>
        /// <returns>The casted object instance of of the setting of type <typeparamref name="T"/> defined in the settings mapping.</returns>
        public virtual T GetValue<T>(string path)
        {
            T value;
            if (!TryGetValue<T>(path, out value) && FallbackMap != null)
                FallbackMap.TryGetValue<T>(path, out value);
            return value;
        }

        /// <summary>
        /// Tries to get a settings value by its settings path in a specific type format.
        /// </summary>
        /// <typeparam name="T">The type of the requested settings value.</typeparam>
        /// <param name="path">The path to get the setting from.</param>
        /// <param name="value">The casted object instance of of the setting of type <typeparamref name="T"/> defined in the settings mapping.</param>
        /// <returns><c>True</c> if the settings value could be found and converted, otherwise <c>False</c>.</returns>
        public virtual bool TryGetValue<T>(string path, out T value)
        {
            var converter = GetTypeConverter(typeof(T));
            value = default(T);

            object rawValue;
            if (_nodes.TryGetValue(path, out rawValue))
            {
                if (converter != null && rawValue is string && converter.CanConvertFrom(typeof(string)))
                {
                    try
                    {
                        value = (T)converter.ConvertFrom(rawValue);
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Gets an array of elements of a specific type defined by the settings path.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="path">The path to the requested settings array.</param>
        /// <returns>An enumerable collection of elements of type <typeparamref name="T"/>.</returns>
        public virtual IEnumerable<T> GetArray<T>(string path)
        {
            var converter = GetTypeConverter(typeof(T));

            object rawValue;
            if (!_nodes.TryGetValue(path, out rawValue))
            {
                if (FallbackMap != null)
                    foreach (var element in FallbackMap.GetArray<T>(path))
                        yield return element;
            }
            else
            {
                var rawArray = rawValue as string[];

                if( rawArray != null)
                {
                    for (int i = 0; i < rawArray.Length; i++)
                    {
                        bool yieldElement = true;
                        T element = default(T);

                        try
                        {
                            element = (T)converter.ConvertFrom(rawArray[i]);
                            yieldElement = true;
                        }
                        catch
                        {
                            yieldElement = false;
                        }

                        if (yieldElement)
                            yield return element;
                    }
                }
            }
        }

        /// <summary>
        /// Assigns a specific value to a setting by its path.
        /// </summary>
        /// <typeparam name="T">The type of the setting.</typeparam>
        /// <param name="path">The path of the setting to be set.</param>
        /// <param name="value">The value to assign the setting to.</param>
        public virtual void SetValue<T>(string path, T value)
        {
            var converter = GetTypeConverter(typeof(T));

            string rawValue = (string)converter.ConvertTo(value, typeof(string));

            if (_nodes.ContainsKey(path))
                _nodes[path] = rawValue;
            else
                _nodes.Add(path, rawValue);
        }

        /// <summary>
        /// Assigns a specific array to a setting by its path.
        /// </summary>
        /// <typeparam name="T">The type of the elements to set.</typeparam>
        /// <param name="path">The path of the setting to be set.</param>
        /// <param name="value">The collection to assign the setting to.</param>
        public virtual void SetArray<T>(string path, IEnumerable<T> value)
        {
            var converter = GetTypeConverter(typeof(T));

            var elements = value.ToArray();
            var rawElements = new string[elements.Length];

            for (int i = 0; i < elements.Length; i++)
            {
                rawElements[i] = (string)converter.ConvertTo(elements[i], typeof(string));
            }

            if (_nodes.ContainsKey(path))
                _nodes[path] = rawElements;
            else
                _nodes.Add(path, rawElements);
        }

        private TypeConverter GetTypeConverter(Type type)
        {
            var converter = TypeDescriptor.GetConverter(type);
            if (converter == null || !converter.CanConvertFrom(typeof(string)) || !converter.CanConvertTo(typeof(string)))
                throw new ArgumentException(string.Format("Cannot find a suitable TypeConverter for type '{0}'.", type));
            return converter;
        }

        /// <summary>
        /// Removes a specific setting from the mapping.
        /// </summary>
        /// <param name="path">The settings path of the setting to remove.</param>
        public void RemoveValue(string path)
        {
            if (_nodes.ContainsKey(path))
                _nodes.Remove(path);
        }

        /// <summary>
        /// Serializes the settings mapping to a file in xml format.
        /// </summary>
        /// <param name="filePath">The file path to save the file to.</param>
        public void Save(FilePath filePath)
        {
            using (var stream = File.Create(filePath.FullPath))
            {
                Save(stream);
            }
        }

        /// <summary>
        /// Serializes the settings mapping to a stream in xml format.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        public void Save(Stream stream)
        {
            var document = GenerateDocument();
            document.Save(stream);
        }

        /// <summary>
        /// Applies parameter values to a specific string.
        /// </summary>
        /// <param name="text">The text to apply parameters to.</param>
        /// <param name="parameters">The parameters to use.</param>
        /// <returns></returns>
        protected string ParseString(string text, IDictionary<string, string> parameters)
        {
            StringBuilder builder = new StringBuilder(text);
            int offset = 0;

            foreach (Match match in _parametersRegex.Matches(text))
            {
                string value = parameters[match.Groups["parameter"].Value];
                builder.Remove(match.Index + offset, match.Length);
                builder.Insert(match.Index + offset, value);
                offset -= match.Length;
                offset += value.Length;
            }

            return builder.ToString();
        }

        private XmlDocument GenerateDocument()
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(string.Format("<?xml version=\"1.0\" encoding=\"utf-8\" ?><{0}></{0}>", DocumentRoot));

            Dictionary<string, XmlNode> folderNodes = new Dictionary<string, XmlNode>();

            foreach (var keyValuePair in _nodes)
            {
                int index = keyValuePair.Key.LastIndexOf('.');

                string folder, name;

                if (index == -1)
                {
                    folder = string.Empty;
                    name = keyValuePair.Key;
                }
                else
                {
                    folder = keyValuePair.Key.Remove(index);
                    name = keyValuePair.Key.Remove(0, index + 1);
                }

                XmlNode folderNode = GetFolderNode(document, folderNodes, folder);
                XmlNode valueNode;

                if (keyValuePair.Value is string)
                {
                    valueNode = document.CreateNode(XmlNodeType.Element, "Item", null);
                    valueNode.InnerText = keyValuePair.Value as string;
                }
                else if (keyValuePair.Value is string[])
                {
                    valueNode = document.CreateNode(XmlNodeType.Element, "ItemArray", null);
                    foreach (var elementNode in GetArrayNodes(document, keyValuePair.Value as string[]))
                    {
                        valueNode.AppendChild(elementNode);
                    }
                }
                else
                    throw new NotSupportedException();

                valueNode.Attributes.Append(document.CreateAttribute("Id")).Value = name;
                folderNode.AppendChild(valueNode);
            }

            return document;
        }

        private XmlNode GetFolderNode(XmlDocument document, Dictionary<string, XmlNode> folderNodes, string fullPath)
        {
            int index = fullPath.LastIndexOf('.');
            string name = index == -1 ? fullPath : fullPath.Remove(0, index + 1);
            string parent = index == -1 ? null : fullPath.Remove(index);
            
            XmlNode folderNode;
            if (!folderNodes.TryGetValue(fullPath, out folderNode))
            {
                folderNode = document.CreateNode(XmlNodeType.Element, "ItemGroup", null);
                folderNode.Attributes.Append(document.CreateAttribute("Id")).Value = name;

                if (string.IsNullOrEmpty(parent))
                {
                    document.DocumentElement.AppendChild(folderNode);
                }
                else
                {
                    GetFolderNode(document, folderNodes, parent).AppendChild(folderNode);
                }

                folderNodes.Add(fullPath, folderNode);
            }

            return folderNode;
        }

        private IEnumerable<XmlNode> GetArrayNodes(XmlDocument document, string[] elements)
        {
            for (int i = 0; i < elements.Length; i++)
            {
                var node = document.CreateNode(XmlNodeType.Element, "Element", null);
                node.InnerText = elements[i];
                yield return node;
            }
        }

        #region ICloneable Members

        /// <inheritdoc />
        public object Clone()
        {
            return MemberwiseClone();
        }

        #endregion
    }
}
