using System;
using System.Linq;

namespace LiteDevelop.Framework.FileSystem
{
    /// <summary>
    /// Represents a project entry in a solution.
    /// </summary>
    public sealed class ProjectEntry : SolutionFolder
    {
        private bool _handlePathChangedEvent = true;
        private Project _project;

        public ProjectEntry()
        {
        }

        public ProjectEntry(Project project)
        {
            Project = project;
            Name = project.Name;
            TypeGuid = project.ProjectDescriptor.SolutionNodeGuid;
            ObjectGuid = project.ProjectGuid;
            FilePath = project.FilePath;
        }

        /// <summary>
        /// Gets a value indicating whether the project entry has loaded the underlying project.
        /// </summary>
        public override bool IsLoaded
        {
            get { return HasProject; }
        }

        /// <summary>
        /// Gets a value indicating whether the project entry has an underlying project.
        /// </summary>
        public bool HasProject
        {
            get { return Project != null; }
        }

        /// <summary>
        /// Gets the underlying project.
        /// </summary>
        public Project Project
        {
            get { return _project; }
            set
            {
                if (_project != value)
                {
                    if (_project != null)
                        _project.FilePathChanged -= _project_FilePathChanged;

                    _project = value;

                    if (_project != null)
                        _project.FilePathChanged += _project_FilePathChanged;
                }
            }
        }

        /// <summary>
        /// Loads the underlying project.
        /// </summary>
        /// <param name = "reporter">The progress reporter to use for logging</param>
        public override void Load(IProgressReporter reporter)
        {
            foreach (var node in Nodes)
                node.Load(reporter);

            try
            {
                Project = Project.OpenProject(FilePath.FullPath);
                OnLoadComplete(new SolutionNodeLoadEventArgs());

            }
            catch (Exception ex)
            {
                OnLoadComplete(new SolutionNodeLoadEventArgs(ex));
            }
        }

        /// <summary>
        /// Saves the underlying project.
        /// </summary>
        /// <param name = "reporter">The progress reporter to use for logging</param>
        public override void Save(IProgressReporter reporter)
        {
            if (Project != null)
                Project.Save(reporter);
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            if (HasProject)
                Project.Dispose();

            base.Dispose();
        }

        protected override void OnFilePathChanged(PathChangedEventArgs e)
        {
            _handlePathChangedEvent = false;
            if (HasProject)
                Project.FilePath = FilePath;
            _handlePathChangedEvent = true;

            base.OnFilePathChanged(e);
        }

        private void _project_FilePathChanged(object sender, PathChangedEventArgs e)
        {
            if (_handlePathChangedEvent)
                FilePath = Project.FilePath;
        }
    }
}
