
using System;
using System.Linq;

namespace LiteDevelop.Framework.Languages
{
    public interface ISnapshotProvider
    {
        event EventHandler SnapshotUpdated;

        /// <summary>
        /// Gets or sets the current snapshot of the source code that should be used to parse members.
        /// </summary>
        SourceSnapshot CurrentSnapshot
        {
            get;
            set;
        }

        /// <summary>
        /// Creates and updates the snapshot with the specific source code.
        /// </summary>
        /// <param name="source"></param>
        void UpdateSnapshot(string source);
    }
}
