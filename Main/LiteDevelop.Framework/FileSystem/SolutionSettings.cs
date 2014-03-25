using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace LiteDevelop.Framework.FileSystem
{

	public sealed class SolutionSettings
	{
		private static XmlSerializer serializer = new XmlSerializer(typeof(SolutionSettings));

		public SolutionSettings()
		{
			StartupProjects = new EventBasedCollection<Guid>();
		}

		public static SolutionSettings ReadSettings(string file)
		{
			using (var fileStream = File.OpenRead(file))
			{
				return serializer.Deserialize(fileStream) as SolutionSettings;
			}
		}

        public EventBasedCollection<Guid> StartupProjects
        {
            get;
            private set;
        }
        
		public void Save(string file)
		{
			using (var fileStream = File.OpenWrite(file))
			{
				serializer.Serialize(fileStream, this);
			}
		}
	}
}