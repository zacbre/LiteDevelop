using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LiteDevelop.Framework;
using LiteDevelop.Framework.Debugging;
using LiteDevelop.Framework.Extensions;
using LiteDevelop.Framework.FileSystem;
using LiteDevelop.Framework.Mui;
using LiteDevelop.Gui.DockContents;

namespace LiteDevelop.Extensions
{
    public class LiteExtensionHost : ILiteExtensionHost
    {
        private UILanguage _language;

        public LiteExtensionHost()
        {
        }

        internal ProgressBar ProgressBar
        {
            get;
            set;
        }

        internal OutputContent OutputContent
        {
            get;
            set;
        }
        
        public UILanguage UILanguage
        {
            get { return _language; }
            internal set
            {
                if (value != _language)
                {
                    _language = value;
                    DispatchUILanguageChanged(EventArgs.Empty);
                }
            }
        }

        public Solution CurrentSolution
        {
            get;
            private set;
        }

        IExtensionManager ILiteExtensionHost.ExtensionManager
        {
            get { return ExtensionManager; }
        }

        public ExtensionManager ExtensionManager
        {
            get;
            internal set;
        }

        IControlManager ILiteExtensionHost.ControlManager
        {
            get { return ControlManager; }
        }

        public ControlManager ControlManager
        {
            get;
            internal set;
        }

        IFileService ILiteExtensionHost.FileService
        {
            get { return FileService; }
        }

        public FileService FileService
        {
            get;
            internal set;
        }

        IBookmarkManager ILiteExtensionHost.BookmarkManager
        {
            get { return BookmarkManager; }
        }

        public BookmarkManager BookmarkManager
        {
            get;
            internal set;
        }

        IErrorManager ILiteExtensionHost.ErrorManager
        {
            get { return ErrorManager; }
        }

        public ErrorManager ErrorManager
        {
            get;
            internal set;
        }

        ISettingsManager ILiteExtensionHost.SettingsManager
        {
            get { return SettingsManager; }
        }

        public ExtensionSettingsManager SettingsManager
        {
            get;
            internal set;
        }

        public IList<INamedProgressReporter> ProgressReporters
        {
            get { return OutputContent.NamedReporters.AsReadOnly(); }
        }

        public INamedProgressReporter CreateOrGetReporter(string id)
        {
            var reporter = OutputContent.NamedReporters.GetReporterById(id);

            if (reporter == null)
            {
                OutputContent.NamedReporters.Add(reporter = new OutputProgressReporter(id, OutputContent, ProgressBar));

                if (string.IsNullOrEmpty(id))
                {
                    reporter.DisplayName = "[Default]";
                }
                else
                {
                    reporter.DisplayName = id;
                }
            }

            return reporter;
        }

        public ICredentialManager CredentialManager
        {
            get;
            internal set;
        }

        public bool IsDebugging
        {
            get { return CurrentDebuggerSession != null; }
        }

        public DebuggerSession CurrentDebuggerSession
        {
            get;
            internal set;
        }

        public Project GetCurrentSelectedProject()
        {
            if (CurrentSolution != null)
            {
                var documentContent = ControlManager.SelectedDocumentContent;
                if (documentContent != null && documentContent.AssociatedFile != null)
                {
                    var file = SearchProjectFileEntry(CurrentSolution, documentContent.AssociatedFile.FilePath);
                    if (file != null)
                        return file.ParentProject;
                }
                return (CurrentSolution.GetSolutionNode(x => x is ProjectEntry) as ProjectEntry).Project;
            }
            return null;
        }

        public static ProjectFileEntry SearchProjectFileEntry(SolutionFolder folder, FilePath path)
        {
            foreach (var node in folder.Nodes)
            {
                if (node is ProjectEntry)
                {
                    var projectEntry = node as ProjectEntry;
                    if (projectEntry.HasProject)
                    {
                        var fileEntry = projectEntry.Project.GetProjectFile(path.FullPath, false);
                        if (fileEntry != null)
                            return fileEntry;
                    }
                }

                if (node is SolutionFolder)
                {
                    var fileEntry = SearchProjectFileEntry(node as SolutionFolder, path);
                    if (fileEntry != null)
                        return fileEntry;
                }
            }

            return null;
        }

        public event SolutionEventHandler SolutionCreated;
        public void DispatchSolutionCreated(SolutionEventArgs e)
        {
            CurrentSolution = e.TargetSolution;
            if (SolutionCreated != null)
                SolutionCreated(this, e);
        }

        public event SolutionEventHandler SolutionLoad;
        public void DispatchSolutionLoad(SolutionEventArgs e)
        {
            CurrentSolution = e.TargetSolution;
            if (SolutionLoad != null)
                SolutionLoad(this, e);
        }

        public event SolutionEventHandler SolutionUnload;
        public void DispatchSolutionUnload(SolutionEventArgs e)
        {
            CurrentSolution.Dispose();
            CurrentSolution = null;
            if (SolutionUnload != null)
                SolutionUnload(this, e);
        }

        public event EventHandler UILanguageChanged;
        public void DispatchUILanguageChanged(EventArgs e)
        {
            if (UILanguageChanged != null)
                UILanguageChanged(this, e);
        }

        public event EventHandler DebugStarted;
        public void DispatchDebugStarted(EventArgs e)
        {
            if (DebugStarted != null)
                DebugStarted(this, e);
        }
    }

}
