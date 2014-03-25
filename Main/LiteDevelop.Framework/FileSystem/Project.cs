using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using LiteDevelop.Framework.Debugging;
using LiteDevelop.Framework.Extensions;
using LiteDevelop.Framework.FileSystem.Net;
using LiteDevelop.Framework.Gui;

namespace LiteDevelop.Framework.FileSystem
{
    /// <summary>
    /// When derived from this class, represents a project that holds files and settings in order to create applications.
    /// </summary>
    public abstract class Project : IFilePathProvider, ISavableFile, IDisposable
    {
        /// <summary>
        /// Opens a project from a file path.
        /// </summary>
        /// <param name="file">The file to open.</param>
        /// <returns>The project instance that is opened.</returns>
        public static Project OpenProject(string file)
        {
            var filePath = new FilePath(file);

            var descriptor = ProjectDescriptor.GetDescriptorByExtension(filePath.Extension);

            if (descriptor == null)
                throw new NotSupportedException("Project file format is not supported.");

            return descriptor.LoadProject(filePath);
        }

        public event EventHandler NameChanged;
        public event BuildResultEventHandler ProjectBuilt;
        public event PathChangedEventHandler FilePathChanged;
        public event EventHandler HasUnsavedDataChanged;
        public event CancelEventHandler DebugStarted;

        private string _name = string.Empty;
        private FilePath _filePath;
        private bool _hasUnsavedData = false;

        internal Project()
        {
            ProjectFiles = new EventBasedCollection<ProjectFileEntry>();
            ProjectFiles.InsertedItem += ProjectFiles_InsertedItem;
            ProjectFiles.RemovedItem += ProjectFiles_RemovedItem;
            FilePath = new FilePath(string.Empty);
            GiveUnsavedData();
        }
        
        /// <summary>
        /// Gets or sets the project files.
        /// </summary>
        public EventBasedCollection<ProjectFileEntry> ProjectFiles { get; set; }

        /// <summary>
        /// Gets or sets the name of the project.
        /// </summary>
        public virtual string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                    _name = value;
                OnNameChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the file path of the project file.
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
        /// Gets or sets the absolute directory path of the project.
        /// </summary>
        public string ProjectDirectory
        {
            get 
            {
                if (FilePath.FullPath == string.Empty)
                    return string.Empty;
                return FilePath.ParentDirectory.FullPath;
            }
        }
        
        /// <summary>
        /// Gets an instance of a document view content that is being used for editing the properties of the project.
        /// </summary>
        public abstract LiteDocumentContent EditorContent
        {
            get;
        }

        /// <summary>
        /// Gets the output directory of this project.
        /// </summary>
        public abstract string OutputDirectory
        {
            get;
        }

        /// <summary>
        /// Gets the project GUID of this project.
        /// </summary>
        public virtual Guid ProjectGuid
        { 
            get;
            set;
        }

        /// <summary>
        /// Gets the general project descriptor of this kind of project.
        /// </summary>
        public abstract ProjectDescriptor ProjectDescriptor
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the project has unsaved data.
        /// </summary>
        /// <remarks>This excludes sub files.</remarks>
        public bool HasUnsavedData 
        {
            get { return _hasUnsavedData; }
            protected set
            {
                if (_hasUnsavedData != value)
                {
                    _hasUnsavedData = value;
                    OnHasUnsavedDataChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Reports the file has unsaved data that needs to be saved in order to conserve the changes that are made.
        /// </summary>
        public void GiveUnsavedData()
        {
            HasUnsavedData = true;
        }

        /// <summary>
        /// Saves the project and all its files.
        /// </summary>
        public abstract void Save(IProgressReporter progressReporter);

        /// <summary>
        /// Builds the project and saves the output to the output directory.
        /// </summary>
        public void BuildAsync(IProgressReporter progressReporter)
        {
            Save(progressReporter);
            OnBuildAsync(progressReporter);
        }

        protected virtual void OnBuildAsync(IProgressReporter progressReporter)
        {
            OnProjectBuilt(new BuildResultEventArgs(new BuildResult(BuildTarget.Build, null, ProjectDirectory)));
        }

        /// <summary>
        /// Gets a value indicating whether the project is executable or not.
        /// </summary>
        public abstract bool IsExecutable
        {
            get;
        }

        /// <summary>
        /// Starts the output file.
        /// </summary>
        /// <remarks>This will not build the application, only execute the last built output.</remarks>
        public void Execute()
        {
            if (IsExecutable)
                OnExecuteProject();
            else
                throw new NotSupportedException("Project is not executable.");
        }

        /// <summary>
        /// Starts debugging the project.
        /// </summary>
        /// <param name="session">The debugger session to host the debugging process.</param>
        public void Debug(DebuggerSession session)
        {
            var eventArgs = new CancelEventArgs();

            if (DebugStarted != null)
                DebugStarted(this, eventArgs);

            if (!eventArgs.Cancel)
            {
                OnDebugProject(session);
            }
        }

        /// <summary>
        /// Gets a project file by its hint path.
        /// </summary>
        /// <param name = "hintPath">The relative path to search.</param>
        public ProjectFileEntry GetProjectFile(string hintPath)
        {
            return GetProjectFile(hintPath, true);
        }

        /// <summary>
        /// Gets a project file by its path.
        /// </summary>
        /// <param name = "path">The path to search.</param>
        /// <param name = "useRelativePath">Indicates whether the specified path is a relative hint path or not.</param>
        public ProjectFileEntry GetProjectFile(string path, bool useRelativePath)
        {
            return ProjectFiles.FirstOrDefault(x => (useRelativePath ? x.FilePath.GetRelativePath(x.ParentProject) : x.FilePath.FullPath).Equals(path, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Gets all project files inside a specific directory.
        /// </summary>
        /// <param name = "hintPath">The hint path of the directory.</param>
        public ProjectFileEntry[] GetProjectFilesInDirectory(string hintPath)
        {
            return GetProjectFilesInDirectory(hintPath, true);
        }

        /// <summary>
        /// Gets all project files inside a specific directory.
        /// </summary>
        /// <param name = "directory">The path of the directory.</param>
        /// <param name = "useRelativePath">Indicates whether the specified path is a relative hint path or not.</param>
        public ProjectFileEntry[] GetProjectFilesInDirectory(string directory, bool useRelativePath)
        {
            return GetProjectFilesInDirectory(directory, useRelativePath, false);
        }

        /// <summary>
        /// Gets all project files inside a specific directory.
        /// </summary>
        /// <param name = "directory">The path of the directory.</param>
        /// <param name = "useRelativePath">Indicates whether the specified path is a relative hint path or not.</param>
        /// <param name="searchSubDirectories">Indicates whether sub directories should be included in the search.</param>
        public ProjectFileEntry[] GetProjectFilesInDirectory(string directory, bool useRelativePath, bool searchSubDirectories)
        {
            var directoryPath = useRelativePath ? new FilePath(this.FilePath.ParentDirectory.FullPath, directory).FullPath : directory;

            return ProjectFiles.Where(x => searchSubDirectories ? x.FilePath.ParentDirectory.FullPath.StartsWith(directoryPath) : x.FilePath.ParentDirectory.FullPath == directoryPath).ToArray();
        }

        /// <summary>
        /// Gets all directories inside a specific directory.
        /// </summary>
        /// <param name = "hintPath">The hint path of the directory.</param>
        public string[] GetDirectoriesInDirectory(string hintPath)
        {
            return GetDirectoriesInDirectory(hintPath, true);
        }

        /// <summary>
        /// Gets all directories files inside a specific directory.
        /// </summary>
        /// <param name = "directory">The path of the directory.</param>
        /// <param name = "useRelativePath">Indicates whether the specified path is a relative hint path or not.</param>
        public string[] GetDirectoriesInDirectory(string directory, bool useRelativePath)
        {
            var directoryPath = useRelativePath ? new FilePath(this.FilePath.ParentDirectory.FullPath, directory).FullPath : directory;

            var files = ProjectFiles.Where(x => x.FilePath.ParentDirectory.ParentDirectory.FullPath == directoryPath);
            var directories = new List<string>();

            foreach (var file in files)
            {
                var relativePath = file.FilePath.ParentDirectory.GetRelativePath(directoryPath);
                if (!directories.Contains(relativePath))
                {
                    directories.Add(relativePath);
                }
            }

            return directories.ToArray();
        }

        public virtual void Dispose()
        {
        }

        protected abstract void OnExecuteProject();

        protected abstract void OnDebugProject(DebuggerSession session);

        protected virtual void OnNameChanged(EventArgs e)
        {
            if (NameChanged != null)
                NameChanged(this, e);
        }

        protected virtual void OnFilePathChanged(PathChangedEventArgs e)
        {
            if (FilePathChanged != null)
                FilePathChanged(this, e);
        }

        protected virtual void OnProjectBuilt(BuildResultEventArgs e)
        {
            if (ProjectBuilt != null)
                ProjectBuilt(this, e);
        }

        protected virtual void OnHasUnsavedDataChanged(EventArgs e)
        {
            if (HasUnsavedDataChanged != null)
                HasUnsavedDataChanged(this, e);
        }

        private void ProjectFiles_RemovedItem(object sender, CollectionChangedEventArgs e)
        {
            var fileEntry = e.TargetObject as ProjectFileEntry;
            fileEntry.ParentProject = null;
            fileEntry.FilePathChanged -= fileEntry_FilePathChanged;
            GiveUnsavedData();
        }

        private void ProjectFiles_InsertedItem(object sender, CollectionChangedEventArgs e)
        {
            var fileEntry = e.TargetObject as ProjectFileEntry;
            fileEntry.ParentProject = this;
            fileEntry.FilePathChanged += fileEntry_FilePathChanged;
            GiveUnsavedData();
        }

        private void fileEntry_FilePathChanged(object sender, PathChangedEventArgs e)
        {
            GiveUnsavedData();
        }
    }
}
