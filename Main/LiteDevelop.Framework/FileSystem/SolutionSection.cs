using System;
using System.Collections.Generic;
using System.Linq;

namespace LiteDevelop.Framework.FileSystem
{
    public class SolutionSection : EventBasedCollection<KeyValuePair<string,string>>
    {
        public string Name { get; set; }
        public string SectionType { get; set; }
        public string Type { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format("Name={0}, Type={1}, Entries={2}", Name, Type, Count);
        }
        
    }
}
