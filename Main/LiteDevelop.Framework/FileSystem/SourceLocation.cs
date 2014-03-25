using System;
using System.Linq;

namespace LiteDevelop.Framework.FileSystem
{
    /// <summary>
    /// Represents a location in a source file.
    /// </summary>
    public class SourceLocation
    {
        public SourceLocation(FilePath filePath, int line, int column)
        {
            FilePath = filePath;
            Line = line;
            Column = column;
        }

        /// <summary>
        /// Gets the path to the source file.
        /// </summary>
        public FilePath FilePath
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the line of the location in the source file.
        /// </summary>
        public int Line
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the column of the location in the source file.
        /// </summary>
        public int Column
        {
            get;
            private set;
        }
    }

    public delegate void SourceLocationEventHandler(object sender, SourceLocationEventArgs e);

    public class SourceLocationEventArgs : EventArgs
    {
        public SourceLocationEventArgs(SourceLocation location)
        {
            Location = location;
        }

        public SourceLocation Location
        {
            get;
            private set;
        }
    }
}
