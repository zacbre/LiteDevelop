using System;
using System.IO;
using System.Linq;
using Microsoft.Build.Construction;
using LiteDevelop.Framework.Languages;
using Microsoft.Build.Evaluation;

namespace LiteDevelop.Framework.FileSystem.MSBuild
{
    /// <summary>
    /// Represents a project that uses a Microsoft Build script to construct the output.
    /// </summary>
    public abstract class MSBuildProject : Project, IFileReferenceProvider 
    {
        private static readonly ProjectCollection _globalCollection = new ProjectCollection();

        public event EventHandler ApplicationTypeChanged;
        public event EventHandler ConfigurationChanged;
        public event EventHandler PlatformChanged;
        private readonly ProjectRootElement _msBuildProject;
        private readonly EventBasedCollection<string> _references = new EventBasedCollection<string>();

        public MSBuildProject()
        {
            _msBuildProject = ProjectRootElement.Create();
            SetupEventHandlers();
        }

        public MSBuildProject(ProjectRootElement project)
        {
            _msBuildProject = project;
            SetupEventHandlers();
        }
        
        public MSBuildProject(FilePath filePath)
        {
            FilePath = filePath;

            _msBuildProject = ProjectRootElement.Open(filePath.FullPath, _globalCollection);

            foreach (var item in _msBuildProject.Items)
            {
                if (item.ItemType == "Reference")
                    References.Add(item.Include);
                else if (item.ItemType == "Compile" || item.ItemType == "None")
                {
                    var entry = new ProjectFileEntry(new FilePath(this.ProjectDirectory, item.Include));

                    foreach (var element in item.Metadata)
                    {
                        if (element.Name == "DependentUpon")
                        {
                            entry.Dependencies.Add(element.Value);
                        }
                    }

                    entry.ParentProject = this;
                    AddFileEventHandlers(entry);
                    ProjectFiles.Add(entry);
                }
            }

            SetupEventHandlers();
            HasUnsavedData = false;
        }

        private void SetupEventHandlers()
        {
            ProjectFiles.InsertedItem += ProjectFiles_InsertedItem;
            ProjectFiles.RemovedItem += ProjectFiles_RemovedItem;

            _references.InsertedItem += References_InsertedItem;
            _references.RemovedItem += References_RemovedItem;
        }

        /// <inheritdoc />
        public override string Name
        {
            get { return GetProperty("AssemblyName"); }
            set 
            { 
                SetProperty("AssemblyName", value);
                OnNameChanged(EventArgs.Empty);
            }
        }

        /// <inheritdoc />
        public override string OutputDirectory
        {
            get { return Path.Combine(ProjectDirectory, GetProperty(Configuration, Platform, "OutputPath")); }
        }

        /// <summary>
        /// Gets the main language of the project.
        /// </summary>
        public abstract LanguageDescriptor Language 
        {
            get; 
        }

        /// <summary>
        /// Gets the application output type of the project.
        /// </summary>
        public SubSystem ApplicationType
        {
            get 
            {
                SubSystem subSystem;
                TryParseOutputType(GetProperty("OutputType"), out subSystem);
                return subSystem;
            }
            set
            {
                SetProperty("OutputType", GetOutputTypeString(value));
                OnApplicationTypeChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets a collection of assembly references that are being used to compile the project.
        /// </summary>
        public EventBasedCollection<string> References
        {
            get { return _references; }
        }

        /// <summary>
        /// Gets the current configuration MSBuild should when building the project.
        /// </summary>
        public string Configuration
        {
            get { return GetProperty("Configuration"); }
            set 
            { 
                SetProperty("Configuration", value);
                OnConfigurationChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets the current platform MSBuild should when building the project.
        /// </summary>
        public string Platform
        {
            get { return GetProperty("Platform"); }
            set 
            {
                SetProperty("Platform", value);
                OnPlatformChanged(EventArgs.Empty);
            }
        }

        /// <inheritdoc />
        public override void Save(IProgressReporter progressReporter)
        {
            _msBuildProject.Save(FilePath.FullPath);
            HasUnsavedData = false;
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            _globalCollection.UnloadProject(_msBuildProject);
            base.Dispose();
        }

        /// <inheritdoc />
        protected override void OnBuildAsync(IProgressReporter progressReporter)
        {
        }

        private void invoker_ProjectBuilt(object sender, BuildResultEventArgs e)
        {
            OnProjectBuilt(e);
        }

        protected string GetProperty(string name)
        {
            return GetProperty(string.Empty, string.Empty, name);
        }

        protected virtual string GetProperty(string config, string platform, string name)
        {
            var property = GetPropertyGroup(config, platform).Properties.FirstOrDefault(x => x.Name == name);
            if (property != null)
                return property.Value;
            return string.Empty;
        }

        protected virtual void SetProperty(string name, string unevaluatedValue)
        {
            SetProperty(string.Empty, string.Empty, name, unevaluatedValue);
        }

        protected virtual void SetProperty(string config, string platform, string name, string unevaluatedValue)
        {
            GetPropertyGroup(config, platform).SetProperty(name, unevaluatedValue);
        }

        protected string GetOutputTypeString(SubSystem subSystem)
        {
            switch (subSystem)
            {
                case SubSystem.Library: return "Library";
                case SubSystem.Windows: return "WinExe";
                case SubSystem.Console: return "Exe";
            }
            throw new ArgumentException("Invalid sub system");
        }

        protected bool TryParseOutputType(string outputType, out SubSystem value)
        {
            value = SubSystem.Unknown;
            switch (outputType.ToLower())
            {
                case "library": value = SubSystem.Library; return true;
                case "winexe": value = SubSystem.Windows; return true;
                case "exe": value = SubSystem.Console; return true;
            }
            return false;
        }

        private string GetItemType(string path)
        {
            string extension = Path.GetExtension(path);
            string itemType = "None";

            if (extension == ".resx")
                itemType = "EmbeddedResource";
            else if (Language.FileExtensions.Contains(extension))
                itemType = "Compile";

            return itemType;
        }

        private ProjectPropertyGroupElement GetPropertyGroup(string config, string platform)
        {
            foreach (var propertyGroup in _msBuildProject.PropertyGroups)
            {
                if (string.IsNullOrEmpty(config) || string.IsNullOrEmpty(platform))
                {
                    if (string.IsNullOrEmpty(propertyGroup.Condition))
                        return propertyGroup;
                }

                // TODO: use a more reliable method of getting propertygroup
                if (propertyGroup.Condition.Contains((config + "|" + platform)))
                    return propertyGroup;
            }

            return _msBuildProject.PropertyGroups.First();
        }

        private void AddFileEventHandlers(ProjectFileEntry entry)
        {
            entry.FilePathChanged += new PathChangedEventHandler(ProjectFileEntry_FilePathChanged);
            entry.Dependencies.InsertedItem += DependantFiles_InsertedItem;
            entry.Dependencies.RemovedItem += DependantFiles_RemovedItem;
        }

        private void RemoveFileEventHandlers(ProjectFileEntry entry)
        {
            entry.FilePathChanged -= new PathChangedEventHandler(ProjectFileEntry_FilePathChanged);
            entry.Dependencies.InsertedItem -= DependantFiles_InsertedItem;
            entry.Dependencies.RemovedItem -= DependantFiles_RemovedItem;
        }

        protected virtual void OnApplicationTypeChanged(EventArgs e)
        {
            if (ApplicationTypeChanged != null)
                ApplicationTypeChanged(this, e);
        }

        protected virtual void OnConfigurationChanged(EventArgs e)
        {
            if (ConfigurationChanged != null)
                ConfigurationChanged(this, e);
        }

        protected virtual void OnPlatformChanged(EventArgs e)
        {
            if (PlatformChanged != null)
                PlatformChanged(this, e);
        }
        
        protected virtual void ProjectFiles_InsertedItem(object sender, CollectionChangedEventArgs e)
        {
            var file = (e.TargetObject as ProjectFileEntry);
            string hintPath = file.FilePath.GetRelativePath(file.ParentProject);
            var item = _msBuildProject.AddItem(GetItemType(hintPath), hintPath);
            AddFileEventHandlers(file);

            foreach (var dependency in file.Dependencies)
                item.AddMetadata("DependentUpon", dependency);
        }

        protected virtual void ProjectFiles_RemovedItem(object sender, CollectionChangedEventArgs e)
        {
            var file = (e.TargetObject as ProjectFileEntry);
            string hintPath = file.FilePath.GetRelativePath(this);

            var item = _msBuildProject.Items.FirstOrDefault(x => x.ItemType == GetItemType(hintPath) && x.Include.Equals(hintPath, StringComparison.OrdinalIgnoreCase));
            if (item != null)
            {
                item.Parent.RemoveChild(item);
            }

            RemoveFileEventHandlers(file);
        }

        private void ProjectFileEntry_FilePathChanged(object sender, PathChangedEventArgs e)
        {
            var file = (sender as ProjectFileEntry);
            string hintPath = e.SourcePath.GetRelativePath(file.ParentProject);
            var item = _msBuildProject.Items.FirstOrDefault(x => x.ItemType == GetItemType(hintPath) && x.Include.Equals(hintPath, StringComparison.OrdinalIgnoreCase));
            if (item != null)
                item.Include = e.NewPath.GetRelativePath(file.ParentProject);
        }

        private void DependantFiles_InsertedItem(object sender, CollectionChangedEventArgs e)
        {
            var file = (sender as ProjectFileEntry);
            string hintPath = file.FilePath.GetRelativePath(file.ParentProject);
            var item = _msBuildProject.Items.FirstOrDefault(x => x.ItemType == GetItemType(hintPath) && x.Include.Equals(hintPath, StringComparison.OrdinalIgnoreCase));
            if (item != null)
                item.AddMetadata("DependentUpon", e.TargetObject as string);
        }

        private void DependantFiles_RemovedItem(object sender, CollectionChangedEventArgs e)
        {
            var file = (sender as ProjectFileEntry);
            string hintPath = file.FilePath.GetRelativePath(file.ParentProject);
            var item = _msBuildProject.Items.FirstOrDefault(x => x.ItemType == GetItemType(hintPath) && x.Include.Equals(hintPath, StringComparison.OrdinalIgnoreCase));
            if (item != null)
                item.Metadata.Remove(item.Metadata.FirstOrDefault(x => x.Name == "DependentUpon" && x.Value == e.TargetObject as string));
        }

        protected virtual void References_InsertedItem(object sender, CollectionChangedEventArgs e)
        {
            _msBuildProject.AddItem("Reference", e.TargetObject as string);
        }

        protected virtual void References_RemovedItem(object sender, CollectionChangedEventArgs e)
        {
            var item = _msBuildProject.Items.FirstOrDefault(x => x.ItemType == "Reference" && x.Include == e.TargetObject as string);
            if (item != null)
                item.Parent.RemoveChild(item);
        }
    }
}
