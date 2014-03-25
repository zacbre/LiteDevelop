using System;
using System.Drawing;
using System.Linq;

namespace LiteDevelop.Framework.FileSystem
{
    /// <summary>
    /// Represents a template which can be used by users creating a new file or project in LiteDevelop.
    /// </summary>
    public abstract class Template
    {
        internal Template(string name, Bitmap icon)
        {
            Name = name;
            Icon = icon;
        }

        /// <summary>
        /// Gets or sets the name of the template.
        /// </summary>
        public virtual string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the icon being used in the template list.
        /// </summary>
        public virtual Bitmap Icon
        {
            get;
            set;
        }
    }
}
