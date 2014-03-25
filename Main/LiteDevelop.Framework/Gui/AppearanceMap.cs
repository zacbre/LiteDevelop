using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace LiteDevelop.Framework.Gui
{
    /// <summary>
    /// Provides descriptors of visual styles.
    /// </summary>
    public sealed class AppearanceMap : ICloneable 
    {
        private static readonly XmlSerializer _serializer = new XmlSerializer(typeof(AppearanceMap));
        private AppearanceProcessor _processor;

        public AppearanceMap()
        {
            Descriptions = new EventBasedCollection<AppearanceDescription>();
        }

        /// <summary>
        /// LOads an appearance map from a file.
        /// </summary>
        /// <param name="filepath">The file to open.</param>
        /// <returns>The appearance map with data stored in the specified file.</returns>
        public static AppearanceMap LoadFromFile(string filepath)
        {
            using (var fileStream = File.OpenRead(filepath))
            {
                return _serializer.Deserialize(fileStream) as AppearanceMap;
            }
        }
        
        /// <summary>
        /// Gets or sets the name of this appearance map.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a collection of appearance descriptions defined in this mapping.
        /// </summary>
        public EventBasedCollection<AppearanceDescription> Descriptions
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a description by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the description to look for.</param>
        /// <returns>A descriptor with an identifier specified in <paramref name="id"/>, or null if none can be found.</returns>
        public AppearanceDescription GetDescriptionById(string id)
        {
            return Descriptions.FirstOrDefault(x => x.ID == id);
        }

        /// <summary>
        /// Saves the mapping to a specific file path.
        /// </summary>
        /// <param name="filepath">The destination file path.</param>
        public void Save(string filepath)
        {
            if (File.Exists(filepath))
                File.Delete(filepath);

            using (var fileStream = File.OpenWrite(filepath))
            {
                _serializer.Serialize(fileStream, this);
            }
        }

        [XmlIgnore]
        public AppearanceProcessor Processor
        {
            get
            {
                if (_processor == null)
                    _processor = new AppearanceProcessor(this);
                return _processor;
            }
        }

        public void CopyTo(AppearanceMap destination)
        {
            destination.Descriptions.Clear();
            
            for (int i = 0; i < Descriptions.Count; i++)
            {
                var description = new AppearanceDescription();
                this.Descriptions[i].CopyTo(description);
                destination.Descriptions.Add(description);
            }
        }

        #region ICloneable Members

        public object Clone()
        {
            var newMap = new AppearanceMap()
            {
                Name = this.Name
            };

            foreach (var description in Descriptions)
            {
                var newDescription = new AppearanceDescription();
                description.CopyTo(newDescription);
                newMap.Descriptions.Add(newDescription);
            }

            return newMap;
        }

        #endregion
    }
}
