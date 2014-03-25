using System;
using System.Linq;
using LiteDevelop.Framework.FileSystem;

namespace LiteDevelop.Framework.FileSystem
{
    public delegate void PathEventHandler(object sender, PathEventArgs e);

    public class PathEventArgs : EventArgs
    {
        public PathEventArgs(FilePath sourcePath)
        {
            SourcePath = sourcePath;
        }

        public FilePath SourcePath
        {
            get;
            private set;
        }
    }

    public delegate void PathChangedEventHandler(object sender, PathChangedEventArgs e);

    public class PathChangedEventArgs : PathEventArgs
    {
        public PathChangedEventArgs(FilePath sourcePath, FilePath newPath)
            : base(sourcePath)
        {
            NewPath = newPath;
        }

        public FilePath NewPath
        {
            get;
            private set;
        }
    }

    public delegate void FileEventHandler(object sender, FileEventArgs e);

    public class FileEventArgs : EventArgs
    {
        public FileEventArgs(OpenedFile file)
        {
            TargetFile = file;
        }

        public OpenedFile TargetFile
        {
            get;
            private set;
        }
    }

    public delegate void FileMovedEventHandler(object sender, FileMovedEventArgs e);

    public class FileMovedEventArgs : FileEventArgs
    {
        public FileMovedEventArgs(OpenedFile file, string oldPath, string newPath)
            : base(file)
        {
            OldPath = oldPath;
            NewPath = newPath;
        }
        
        public string OldPath
        {
            get;
            private set;
        }

        public string NewPath
        {
            get;
            private set;
        }
    }

    public delegate void SolutionEventHandler(object sender, SolutionEventArgs e);

    public class SolutionEventArgs : EventArgs
    {
        public SolutionEventArgs(Solution file)
        {
            TargetSolution = file;
        }

        public Solution TargetSolution
        {
            get;
            private set;
        }
    }

    public delegate void ProjectEventHandler(object sender, ProjectEventArgs e);

    public class ProjectEventArgs : EventArgs
    {
        public ProjectEventArgs(Project file)
        {
            TargetProject = file;
        }

        public Project TargetProject
        {
            get;
            private set;
        }
    }

    public delegate void SolutionNodeLoadEventHandler(object sender, SolutionNodeLoadEventArgs e);

    public class SolutionNodeLoadEventArgs : EventArgs
    {
        public SolutionNodeLoadEventArgs()
        {
        }

        public SolutionNodeLoadEventArgs(Exception error)
        {
            Error = error;
        }

        public Exception Error { get; private set; }
        public bool Success { get { return Error == null; } }
    }

    public delegate void BuildErrorEventHandler(object sender, BuildErrorEventArgs e);

    public class BuildErrorEventArgs : EventArgs
    {
        public BuildErrorEventArgs(BuildError error)
        {
            Error = error;
        }

        public BuildError Error
        {
            get;
            private set;
        }
    }

    public delegate void BuildResultEventHandler(object sender, BuildResultEventArgs e);

    public class BuildResultEventArgs : EventArgs
    {
        public BuildResultEventArgs(BuildResult result)
        {
            Result = result;
        }

        public BuildResult Result
        {
            get;
            private set;
        }
    }

    public delegate void ReloadRequestedEventHandler(object sender, ReloadRequestedEventArgs e);

    public class ReloadRequestedEventArgs : EventArgs
    {
        public ReloadRequestedEventArgs(ReloadRequestReason reason)
        {
            Reason = reason;
        }

        public bool Handled
        {
            get;
            set;
        }

        public ReloadRequestReason Reason
        {
            get;
            private set;
        }
    }

    public enum ReloadRequestReason
    {
        FileOutOfDate,
    }
}
