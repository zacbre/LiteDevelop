using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using LiteDevelop.Framework.Debugging;
using LiteDevelop.Framework.Extensions;
using LiteDevelop.Framework.FileSystem.MSBuild;

namespace LiteDevelop.Framework.FileSystem
{
    /// <summary>
    /// Represents a solution file using the MSBuild format.
    /// </summary>
    public sealed class Solution : SolutionFolder, ISavableFile
    {
        /// <summary>
        /// Creates a new solution file using the MSBuild format.
        /// </summary>
        /// <param name = "name">The name of the solution to create.</param>
        public static Solution CreateSolution(string name)
        {
            var solution = MSBuildProjectFactory.CreateSolution(name);
            solution.InitializeEventHandlers();
            solution.GiveUnsavedData();
            return solution;
        }

        /// <summary>
        /// Opens an existing solution file and parses out the sub projects.
        /// </summary>
        /// <param name = "solutionFile">The file path to the solution file.</param>
        public static Solution OpenSolution(string solutionFile)
        {
            using (var reader = new StreamReader(solutionFile))
            {
                using (var solutionReader = new SolutionReader(reader))
                {
                    var solution = new Solution();
                    solution.FilePath = new FilePath(solutionFile);
                    solution.Name = solution.FilePath.FileName;
                    solutionReader.InitializeSolution(solution);
                    solution.InitializeEventHandlers();

                    var settingsPath = solution.FilePath.ChangeExtension(".litesettings").FullPath;
                    if (File.Exists(settingsPath))
                    {
                        try
                        {
                            var settings = SolutionSettings.ReadSettings(settingsPath);
                            solution.Settings = settings;
                        }
                        catch
                        {
                            // TODO: notify user
                        }
                    }

                    return solution;
                }
            }
        }

        public event CancelEventHandler BuildStarted;
        public event BuildResultEventHandler BuildCompleted;
        public event CancelEventHandler CleanStarted;
        public event BuildResultEventHandler CleanCompleted;
        public event EventHandler HasUnsavedDataChanged;

        private bool _hasUnsavedData = false;
        private readonly MSBuildInvoker _invoker;

        internal Solution()
        {
            GlobalSections = new EventBasedCollection<SolutionSection>();
            Settings = new SolutionSettings();
            _invoker = new MSBuildInvoker(this);
            _invoker.CompletedOperation += new BuildResultEventHandler(invoker_CompletedOperation);
        }

        /// <summary>
        /// Gets a collection of global sections defined in the solution file.
        /// </summary>
        public EventBasedCollection<SolutionSection> GlobalSections
        { 
            get; 
            private set;
        }

        /// <summary>
        /// Gets the solution file format version.
        /// </summary>
        public SolutionVersion Version 
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the associated msbuild invoker engine of this solution.
        /// </summary>
        public MSBuildInvoker Builder
        {
        	get { return _invoker; }
        }
        
        /// <summary>
        /// Gets additional settings defined by LiteDevelop.
        /// </summary>
        public SolutionSettings Settings
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether the solution file contains unsaved data.
        /// </summary>
        /// <remarks>This does not include unsaved data from sub projects and sub files</remarks>
        public bool HasUnsavedData 
        {
            get { return _hasUnsavedData; }
            private set
            {
                if (_hasUnsavedData != value)
                {
                    _hasUnsavedData = value;
                    OnHasUnsavedDataChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Reports the solution file has unsaved data that needs to be saved in order to conserve the changes that are made.
        /// </summary>
        public void GiveUnsavedData()
        {
            HasUnsavedData = true;
        }

        /// <summary>
        /// Saves the solution file.
        /// </summary>
        /// <param name = "reporter">The progress reporter to use for logging.</param>
        public override void Save(IProgressReporter reporter)
        {
            reporter.ProgressVisible = true;
            for (int i = 0; i < Nodes.Count; i++)
            {
                reporter.Report("Saving {0}.", Nodes[i].Name);
                Nodes[i].Save(reporter);
                //progressReporter.ProgressPercentage = (int)(((double)Nodes.Count / (double)i) * 100);
            }

            reporter.Report("Writing solution file.");

            using (var writer = new StreamWriter(FilePath.FullPath))
            {
                using (var solutionWriter = new SolutionWriter(writer))
                {
                    solutionWriter.WriteSolution(this);
                }
            }

            reporter.Report("Writing solution settings.");

            Settings.Save(FilePath.ChangeExtension(".litesettings").FullPath);

            reporter.Report("Finished saving solution.");
            reporter.ProgressVisible = false;
            HasUnsavedData = false;
        }

        /// <summary>
        /// Starts the building procedure of this solution.
        /// </summary>
        /// <param name = "progressReporter">The progress reporter to use for logging.</param>
        public void BuildAsync(IProgressReporter progressReporter)
        {
            Save(progressReporter);

            var eventArgs = new CancelEventArgs();
            OnBuildStarted(eventArgs);

            if (!eventArgs.Cancel)
                _invoker.BuildAsync(progressReporter);
        }

        /// <summary>
        /// Starts the cleaning procedure of the solution.
        /// </summary>
        /// <param name="progressReporter"></param>
        public void CleanAsync(IProgressReporter progressReporter)
        {
            var eventArgs = new CancelEventArgs();
            OnCleanStarted(eventArgs);

            if (!eventArgs.Cancel)
                _invoker.CleanAsync(progressReporter);
        }

        /// <summary>
        /// Searches through all the solution nodes and picks out the first node that meets the specified condition.
        /// </summary>
        /// <param name = "condition">The condition function to use</param>
        public SolutionNode GetSolutionNode(Func<SolutionNode, bool> condition)
        {
            return GetSolutionNode(Nodes, condition);
        }

        /// <summary>
        /// Gets the first executable project defined in this solution.
        /// </summary>
        /// <returns>A project that is executable, or null if none has been found.</returns>
        public Project GetFirstExecutableProject()
        {
            var node = GetSolutionNode((x) =>
            {
                var entry = x as ProjectEntry;
                if (entry != null && entry.HasProject)
                    return entry.Project.IsExecutable;
                return false;
            }) as ProjectEntry;
            if (node != null)
                return node.Project;
            return null;
        }

        /// <summary>
        /// Executes the solution by using the startup projects defined in the additional solution settings
        /// </summary>
        public void Execute()
        {
            EnsureStartupProjectSet();

            foreach (var projectGuid in Settings.StartupProjects)
            {
                var node = GetSolutionNode(x => x.ObjectGuid == projectGuid);
                if (node == null)
                    throw new ArgumentException(string.Format("Invalid solution settings. Guid {0} is not present in solution.", projectGuid));

                if (node is ProjectEntry)
                    (node as ProjectEntry).Project.Execute();
            }
        }

        /// <summary>
        /// Starts debugging the solution by using the startup projects defined in the additional solution settings
        /// </summary>
        /// <param name="session">The debugging session to host the debugging process.</param>
        public void Debug(DebuggerSession session)
        {
            EnsureStartupProjectSet();

            foreach (var projectGuid in Settings.StartupProjects)
            {
                var node = GetSolutionNode(x => x.ObjectGuid == projectGuid);
                if (node == null)
                    throw new ArgumentException(string.Format("Invalid solution settings. Guid {0} is not present in solution.", projectGuid));

                if (node is ProjectEntry)
                {
                    (node as ProjectEntry).Project.Debug(session);
                }
            }
            
        }

        /// <summary>
        /// Ensures a start up project is defined.
        /// </summary>
        public void EnsureStartupProjectSet()
        {
            if (Settings.StartupProjects.Count == 0)
            {
                var project = GetFirstExecutableProject();
                if (project == null)
                    throw new ArgumentException("No executable project detected.");
                Settings.StartupProjects.Add(project.ProjectGuid);
            }
        }

        /// <summary>
        /// Determines whether the solution has at least one debuggable project defined.
        /// </summary>
        /// <param name="extensionManager">The extension manager to get the debuggers from.</param>
        /// <returns><c>True</c> when a debuggable project is found, or <c>False</c> when there is none.</returns>
        public bool HasDebuggableProjects(IExtensionManager extensionManager)
        {
            return GetSolutionNode(x => x is ProjectEntry && extensionManager.GetDebuggers((x as ProjectEntry).Project).ToArray().Length != 0) != null;
        }

        /// <summary>
        /// Finds a project file defined in one of the projects in this solution by its file path.
        /// </summary>
        /// <param name="path">The file path of the project file to get.</param>
        /// <returns>A project file entry or null when none has been found.</returns>
        public ProjectFileEntry FindProjectFile(FilePath path)
        {
            return FindProjectFile(this, path);
        }

        private static ProjectFileEntry FindProjectFile(SolutionFolder folder, FilePath path)
        {
            if (folder is ProjectEntry)
            {
                var projectEntry = folder as ProjectEntry;
                if (projectEntry.HasProject)
                {
                    var file = projectEntry.Project.GetProjectFile(path.FullPath, false);
                    if (file != null)
                        return file;
                }
            }

            foreach (var subFolder in folder.Nodes)
            {
                if (subFolder is SolutionFolder)
                {
                    var file = FindProjectFile(subFolder as SolutionFolder, path);
                    if (file != null)
                        return file;
                }
            }

            return null;
        }

        private void InitializeEventHandlers()
        {
            Nodes.InsertedItem += Nodes_InsertedItem;
            Nodes.RemovedItem += Nodes_RemovedItemOrCleared;
            Nodes.ClearedCollection += Nodes_RemovedItemOrCleared;
        }

        private SolutionNode GetSolutionNode(IEnumerable<SolutionNode> folders, Func<SolutionNode, bool> condition)
        {
            foreach (var folder in folders)
            {
                if (condition(folder))
                    return folder;
                if (folder is SolutionFolder)
                {
                    var nestedFolder = GetSolutionNode((folder as SolutionFolder).Nodes, condition);
                    if (nestedFolder != null)
                        return nestedFolder;
                }
            }
            return null;
        }

        private void OnBuildStarted(CancelEventArgs e)
        {
            if (BuildStarted != null)
                BuildStarted(this, e);
        }

        private void OnBuildCompleted(BuildResultEventArgs e)
        {
            if (BuildCompleted != null)
                BuildCompleted(this, e);
        }

        private void OnCleanStarted(CancelEventArgs e)
        {
            if (CleanStarted != null)
                CleanStarted(this, e);
        }

        private void OnCleanCompleted(BuildResultEventArgs e)
        {
            if (CleanCompleted != null)
                CleanCompleted(this, e);
        }

        private void invoker_CompletedOperation(object sender, BuildResultEventArgs e)
        {
            switch (e.Result.Target)
            {
                case BuildTarget.Build:
                    OnBuildCompleted(e);
                    break;
                case BuildTarget.Clean:
                    OnCleanCompleted(e);
                    break;
            }
        }

        private void Nodes_InsertedItem(object sender, CollectionChangedEventArgs e)
        {
            var projectEntry = e.TargetObject as ProjectEntry;
            if (projectEntry != null)
            {
                var project = projectEntry.Project;

                var projectConfigPlatforms = GetGlobalSection("ProjectConfigurationPlatforms", "postSolution", true);

                projectConfigPlatforms.Add(new KeyValuePair<string,string>(project.ProjectGuid.ToString("B").ToUpper() + ".Debug|Any CPU.ActiveCfg", "Debug|Any CPU"));
                projectConfigPlatforms.Add(new KeyValuePair<string,string>(project.ProjectGuid.ToString("B").ToUpper() + ".Debug|Any CPU.Build.0", "Debug|Any CPU"));
                projectConfigPlatforms.Add(new KeyValuePair<string,string>(project.ProjectGuid.ToString("B").ToUpper() + ".Release|Any CPU.ActiveCfg", "Release|Any CPU"));
                projectConfigPlatforms.Add(new KeyValuePair<string,string>(project.ProjectGuid.ToString("B").ToUpper() + ".Release|Any CPU.Build.0", "Release|Any CPU"));
            }
            GiveUnsavedData();
        }

        private void Nodes_RemovedItemOrCleared(object sender, EventArgs e)
        {
            GiveUnsavedData();
        }

        private SolutionSection GetGlobalSection(string name, string type, bool createIfNotExist)
        {
            SolutionSection section;
            if ((section = GlobalSections.FirstOrDefault(x => x.Name == name && x.Type == type)) == null && createIfNotExist)
            {
                section = new SolutionSection()
                {
                    Name = name,
                    Type = type,
                    SectionType = "Global",
                };
                GlobalSections.Add(section);
            }
            return section;
        }

        private void OnHasUnsavedDataChanged(EventArgs e)
        {
            if (HasUnsavedDataChanged != null)
                HasUnsavedDataChanged(this, e);
        }
    }
}
