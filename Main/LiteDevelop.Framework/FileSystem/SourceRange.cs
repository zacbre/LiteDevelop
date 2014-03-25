using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiteDevelop.Framework.FileSystem
{
    public class SourceRange : SourceLocation 
    {
        public SourceRange(FilePath filePath, int startLine, int startColumn, int endLine, int endColumn)
            : base(filePath, startLine, startColumn)
        {
            EndLine = endLine;
            EndColumn = endColumn;
        }

        public int EndLine
        {
            get;
            private set;
        }

        public int EndColumn
        {
            get;
            private set;
        }
    }


    public delegate void SourceRangeEventHandler(object sender, SourceRangeEventArgs e);

    public class SourceRangeEventArgs : EventArgs
    {
        public SourceRangeEventArgs(SourceRange range)
        {
            Range = range;
        }

        public SourceRange Range
        {
            get;
            private set;
        }
    }

}
