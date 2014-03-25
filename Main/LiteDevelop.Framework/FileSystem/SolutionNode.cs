using System;
using System.Linq;
using System.Threading;

namespace LiteDevelop.Framework.FileSystem
{
    /// <summary>
    /// Represents a node in a solution file.
    /// </summary>
    public abstract class SolutionNode : IFilePathProvider
    {
        public event SolutionNodeLoadEventHandler LoadComplete;
        public event PathChangedEventHandler FilePathChanged;

        private FilePath _filePath;

        public SolutionNode()
        {
            Sections = new EventBasedCollection<SolutionSection>();
        }

        public SolutionFolder Parent { get; set; }
        public Guid TypeGuid { get; set; }
        public string Name { get; set; }

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

        public Guid ObjectGuid { get; set; }

        public abstract bool IsLoaded
        {
            get;
        }

        public EventBasedCollection<SolutionSection> Sections { get; set; }

        public void BeginLoad(IProgressReporter reporter)
        {
            new Thread(() => Load(reporter)).Start();
        }

        public abstract void Load(IProgressReporter reporter);

        public abstract void Save(IProgressReporter reporter);

        protected virtual void OnLoadComplete(SolutionNodeLoadEventArgs e)
        {
            if (LoadComplete != null)
                LoadComplete(this, e);
        }

        protected virtual void OnFilePathChanged(PathChangedEventArgs e)
        {
            if (FilePathChanged != null)
                FilePathChanged(this, e);
        }

        public SolutionFolder GetRoot()
        {
            SolutionNode node = this;
            while (node.Parent != null)
                node = node.Parent;
            return node as SolutionFolder;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format("TypeGuid={0}, Name={1}, HintPath={2}, ProjectGuid={3}", TypeGuid, Name, FilePath, ObjectGuid);
        }
    }
}
