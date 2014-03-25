using System;
using System.Linq;

namespace LiteDevelop.Framework.FileSystem
{
    /// <summary>
    /// Represents an error occured at building a project.
    /// </summary>
    public class BuildError
    {
        public BuildError(SourceLocation location, string message, MessageSeverity severity)
        {
            Location = location;
            Message = message;
            Severity = severity;
        }

        /// <summary>
        /// Gets the source location of the error.
        /// </summary>
        public SourceLocation Location
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        public string Message
        {
            get;
            private set;
        }
        
        /// <summary>
        /// Gets the severity level of this error.
        /// </summary>
        public MessageSeverity Severity
        {
            get;
            private set;
        }
    }

}
