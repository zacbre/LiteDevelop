using System;
using System.IO;
using System.Linq;
using LiteDevelop.Framework;
using LiteDevelop.Framework.Extensions;

namespace LiteDevelop.Extensions
{
    public class ExtensionSettingsManager : ISettingsManager
    {
        public ExtensionSettingsManager()
        {
        }

        public string GetSettingsDirectory(LiteExtension extension)
        {
            string path = Path.Combine(Constants.ExtensionSettingsDirectory, string.Format("{0} by {1}", extension.Name, extension.Author));

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path;
        }

    }
}
