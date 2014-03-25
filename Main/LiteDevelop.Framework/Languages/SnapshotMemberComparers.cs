using System;
using System.Collections.Generic;
using System.Linq;

namespace LiteDevelop.Framework.Languages
{
    public class SnapshotMemberNameComparer : IComparer<SnapshotMember>, IEqualityComparer<SnapshotMember>
    {
        public int Compare(SnapshotMember x, SnapshotMember y)
        {
            return x.Name.CompareTo(y.Name);
        }

        public bool Equals(SnapshotMember x, SnapshotMember y)
        {
            return x.Name.Equals(y.Name);
        }

        public int GetHashCode(SnapshotMember obj)
        {
            return obj.Name.GetHashCode();
        }
    }

    public class SnapshotMemberIndexComparer : IComparer<SnapshotMember>, IEqualityComparer<SnapshotMember>
    {
        public int Compare(SnapshotMember x, SnapshotMember y)
        {
            return x.Start.CompareTo(y.Start);
        }

        public bool Equals(SnapshotMember x, SnapshotMember y)
        {
            return x.Start == y.Start;
        }

        public int GetHashCode(SnapshotMember obj)
        {
            return obj.Start.GetHashCode();
        }
    }
}
