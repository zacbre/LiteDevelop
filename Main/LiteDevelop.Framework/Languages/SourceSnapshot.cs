using System;
using System.Linq;

namespace LiteDevelop.Framework.Languages
{
    public abstract class SourceSnapshot
    {
        public SourceSnapshot(string source)
        {
            this.Source = source;
        }

        public abstract LanguageDescriptor Language { get; }

        public string Source { get; private set; }

        public abstract SnapshotMember GetMemberByName(string name);
    }
}
