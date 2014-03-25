using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace LiteDevelop.Framework.FileSystem
{
    public class Bookmark : SourceLocation
    {
        public Bookmark(FilePath filePath, int line, int column)
            : base(filePath, line, column)
        {
        }

        public string Tooltip
        {
            get;
            set;
        }
    }
}
