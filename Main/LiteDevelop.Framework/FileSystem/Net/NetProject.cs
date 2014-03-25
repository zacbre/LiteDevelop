using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using LiteDevelop.Framework.Debugging;
using LiteDevelop.Framework.Extensions;
using LiteDevelop.Framework.FileSystem.MSBuild;
using LiteDevelop.Framework.Gui;
using LiteDevelop.Framework.Languages;
using LiteDevelop.Framework.Languages.Net;

namespace LiteDevelop.Framework.FileSystem.Net
{
    /// <summary>
    /// Represents a project using a language specific .NET compiler to build the source files.
    /// </summary>
    public abstract class NetProject : MSBuildProject
    {
        public event EventHandler RootNamespaceChanged;
        public event EventHandler TargetFrameworkChanged;

        private LiteDocumentContent _editor;

        /// <summary>
        /// Creates a new .NET MSBuild project with the default values.
        /// </summary>
        public NetProject(string name, ProjectDescriptor projectDescriptor)
            : base(MSBuildProjectFactory.CreateNetProject(projectDescriptor.MSBuildTargetsFile))
        {
            Name = RootNamespace = name;
        }

        /// <summary>
        /// Opens a .NET MSBuild project file and creates a new instance of the NetProject class.
        /// </summary>
        /// <param name="filePath">The file path to the msbuild project to open.</param>
        public NetProject(FilePath filePath)
            : base(filePath)
        {
        }

        /// <inheritdoc />
        public override LiteDocumentContent EditorContent
        {
            get
            {
                if (_editor == null)
                    _editor = new NetProjectSettingsContent(this);
                return _editor;
            }
        }

        /// <inheritdoc />
        public override Guid ProjectGuid
        {
            get { return Guid.Parse(GetProperty("ProjectGuid")); }
            set { SetProperty("PropertyGuid", value.ToString("B").ToUpper()); }
        }

        /// <summary>
        /// Gets or sets the default namespace of this project.
        /// </summary>
        public string RootNamespace
        {
            get { return GetProperty("RootNamespace"); }
            set 
            {
                SetProperty("RootNamespace", value);
                OnRootNamespaceChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the target framework used by this project.
        /// </summary>
        public FrameworkVersion TargetFramework
        {
            get
            {
                string versionValue = GetProperty("TargetFrameworkVersion");
                bool isClient = GetProperty("TargetFrameworkProfile") == "Client";
                var installedVersions = FrameworkDetector.GetInstalledVersions();

                var version = installedVersions.FirstOrDefault(x =>
                    x.DisplayVersion == versionValue &&
                    x.InstallationType == (isClient ? FrameworkInstallationType.ClientProfile : FrameworkInstallationType.Full));

                return version ?? installedVersions[installedVersions.Length - 1];
            }
            set
            {
                SetProperty("TargetFrameworkVersion", value.DisplayVersion);
                SetProperty("TargetFrameworkProfile", value.InstallationType == FrameworkInstallationType.ClientProfile ? "Client" : string.Empty);
                OnTargetFrameworkChanged(EventArgs.Empty);
            }
        }

        /// <inheritdoc />
        public override bool IsExecutable
        {
            get { return ApplicationType == SubSystem.Console || ApplicationType == SubSystem.Windows; }
        }

        /// <inheritdoc />
        protected override void OnExecuteProject()
        {
            Process.Start(Path.Combine(OutputDirectory, Name + ".exe"));
        }

        /// <inheritdoc />
        protected override void OnDebugProject(DebuggerSession session)
        {
            var info = new ProcessStartInfo()
            {
                FileName = Path.Combine(OutputDirectory, Name + ".exe"),
            };
            
            session.Start(info);
        }

        protected virtual void OnRootNamespaceChanged(EventArgs e)
        {
            if (RootNamespaceChanged != null)
                RootNamespaceChanged(this, e);
        }

        protected virtual void OnTargetFrameworkChanged(EventArgs e)
        {
            if (TargetFrameworkChanged != null)
                TargetFrameworkChanged(this, e);
        }
    }
}
