using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiteDevelop.Framework.FileSystem.Net.CSharp;
using LiteDevelop.Framework.FileSystem.Net.VisualBasic;
using LiteDevelop.Framework.Languages;

namespace LiteDevelop.Framework.FileSystem
{
    /// <summary>
    /// When derived from this class, provides information about a specific project type. 
    /// </summary>
    public abstract class ProjectDescriptor
    {
        static ProjectDescriptor()
        {
            ProjectDescriptors = new EventBasedCollection<ProjectDescriptor>()
            {
                new CSharpProjectDescriptor(),
                new VisualBasicProjectDescriptor(),
            };
        }

        /// <summary>
        /// Gets a collection of registered project descriptors.
        /// </summary>
        public static EventBasedCollection<ProjectDescriptor> ProjectDescriptors
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a project descriptor by its type.
        /// </summary>
        /// <typeparam name="T">The descriptor's type.</typeparam>
        /// <returns>A project descriptor of type <typeparamref name="T"/>.</returns>
        public static T GetDescriptor<T>() where T: ProjectDescriptor
        {
            return GetDescriptor(x => x is T) as T;
        }

        /// <summary>
        /// Gets a project descriptor by its project extension.
        /// </summary>
        /// <param name="extension">The extension of a project file of the requested project type.</param>
        /// <returns>A project descriptor with extension of <paramref name="extension"/>.</returns>
        public static ProjectDescriptor GetDescriptorByExtension(string extension)
        {
            return GetDescriptor(x => x.ProjectExtension.Equals(extension, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Gets the first occurance of a project descriptor that matches the given predicate.
        /// </summary>
        /// <param name="predicate">The predicate to match.</param>
        /// <returns>A project descriptor matching the given predicate.</returns>
        public static ProjectDescriptor GetDescriptor(Func<ProjectDescriptor, bool> predicate)
        {
            return ProjectDescriptors.FirstOrDefault(predicate);
        }

        /// <summary>
        /// Gets the default extension of a project file of this type.
        /// </summary>
        public abstract string ProjectExtension
        {
            get;
        }

        /// <summary>
        /// Gets the MSBuild target file if available.
        /// </summary>
        public abstract string MSBuildTargetsFile
        {
            get;
        }

        /// <summary>
        /// Gets the unique identifier used in solution files for defining a project of this type.
        /// </summary>
        public abstract Guid SolutionNodeGuid
        {
            get;
        }

        /// <summary>
        /// Gets a collection of languages which can be used in this kind of project.
        /// </summary>
        public abstract EventBasedCollection<LanguageDescriptor> Languages
        {
            get;
        }

        /// <summary>
        /// Loads a project from a file path using the format specific for this project kind.
        /// </summary>
        /// <param name="filePath">The file path to the project file.</param>
        /// <returns>The project that is loaded.</returns>
        public abstract Project LoadProject(FilePath filePath);

        /// <summary>
        /// Creates a new project of this project kind.
        /// </summary>
        /// <param name="name">The name of the new project.</param>
        /// <returns>The project that is created.</returns>
        public abstract Project CreateProject(string name);

    }
}
