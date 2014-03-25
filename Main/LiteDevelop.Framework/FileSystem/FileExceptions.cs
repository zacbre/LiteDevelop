using System;
using System.Linq;

namespace LiteDevelop.Framework.FileSystem
{
    public class BuildException : Exception
    {
        public BuildException(BuildResult result)
            : this("Failed to build one or more projects.", null, result)
        {
        }

        public BuildException(string message, BuildResult result)
            : this(message, null, result)
        {
        }

        public BuildException(string message, Exception inner, BuildResult result)
            : base(message, inner)
        {
            this.Result = result;
        }

        public BuildResult Result { get; private set; }
    }

    public class ProjectLoadException : Exception
    {
        public ProjectLoadException(string message)
            : this(message, null)
        {
        }

        public ProjectLoadException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

    }
}
