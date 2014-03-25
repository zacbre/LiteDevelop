using System;
using System.Linq;

namespace LiteDevelop.Framework.Languages
{
    public abstract class SnapshotMember
    {
        public abstract string Name { get; set; }
        public abstract int Start { get; set; }
        public abstract int End { get; set; }
        public int Length { get { return End - Start; } }
    }
}
