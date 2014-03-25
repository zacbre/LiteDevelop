using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiteDevelop.Framework.FileSystem;

namespace LiteDevelop.Framework.Debugging
{
    public class BreakpointBookmark : Bookmark
    {
        public BreakpointBookmark(FilePath filePath, int line, int column)
            : base(filePath, line, column)
        {
            IsActive = true;
        }

        public bool IsActive
        {
            get;
            set;
        }

        public string Condition
        {
            get;
            set;
        }
    }
}
