using System;
using System.Linq;

namespace LiteDevelop.Framework.FileSystem
{
    /// <summary>
    /// Represents a file which can be included in a project.
    /// </summary>
    public class ProjectFileEntry : IFilePathProvider
    {
        public event PathChangedEventHandler FilePathChanged;

        private Project _parent;
        private FilePath _filePath;

        internal ProjectFileEntry()
        {
            Dependencies = new EventBasedCollection<string>();
            IncludeInBuildEvent = true;
        }

        public ProjectFileEntry(FilePath path)
            : this()
        {
            FilePath = path;
        }

        public ProjectFileEntry(OpenedFile file)
            : this(file.FilePath)
        {
            Dependencies.AddRange(file.Dependencies);
        }

        /// <summary>
        /// Gets or sets the file path to the project file.
        /// </summary>
        public FilePath FilePath
        {
            get { return _filePath; }
            set
            {
                if (_filePath != value)
                {
                    var old = _filePath;
                    _filePath = value;
                    OnFilePathChanged(new PathChangedEventArgs(old, _filePath));
                }
            }
        }

        /// <summary>
        /// Gets the project holding this project file.
        /// </summary>
        public Project ParentProject
        {
            get { return _parent; }
            internal set
            {
                _parent = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whenever the file should be included in the building process of the project.
        /// </summary>
        public bool IncludeInBuildEvent
        {
            get;
            set;
        }
        
        /// <summary>
        /// Gets a collection of files this file is depending on.
        /// </summary>
        public EventBasedCollection<string> Dependencies
        {
            get;
            private set;
        }

        protected virtual void OnFilePathChanged(PathChangedEventArgs e)
        {
            if (FilePathChanged != null)
                FilePathChanged(this, e);
        }
    }
}
