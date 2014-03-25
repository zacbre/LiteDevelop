using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiteDevelop.Framework.Languages.Net
{
    public class NetSnapshotMember : SnapshotMember
    {
        public NetSnapshotMember(string name, string[] modifiers, string type, int start, int end)
        {
            Name = name;
            ValueType = type;
            Modifiers = modifiers;
            Start = start;
            End = end;
        }

        /// <inheritdoc />
        public override string Name { get; set; }

        public string[] Modifiers { get; set; }

        public string ValueType { get; set; }

        /// <inheritdoc />
        public override int Start { get; set; }

        /// <inheritdoc />
        public override int End { get; set; }
        
        public string ConcatAllModifiers()
        {
            if (Modifiers == null || Modifiers.Length == 0)
                return string.Empty;

            return string.Join(" ", Modifiers);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format("{0} {1} {2}", ConcatAllModifiers(), ValueType, Name);
        }

    }
}
