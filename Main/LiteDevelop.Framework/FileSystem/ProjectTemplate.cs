using System;
using System.Drawing;
using System.Linq;
using LiteDevelop.Framework.Extensions;

namespace LiteDevelop.Framework.FileSystem
{
    /// <summary>
    /// Represents a project template which can be used by a LiteDevelop user.
    /// </summary>
    public abstract class ProjectTemplate : Template
    {
        public event TemplateResultEventHandler ProjectCreated;

        public ProjectTemplate(string name, Bitmap icon, params FileTemplate[] fileTemplates)
            : base(name, icon)
        {
            Files = fileTemplates;
        }

        /// <summary>
        /// Gets the file templates of the project template.
        /// </summary>
        public FileTemplate[] Files
        {
            get;
            private set;
        }

        /// <summary>
        /// Cretes a new project using this template.
        /// </summary>
        /// <param name = "fileService">The file service to use.</param>
        /// <param name = "filePath">The target file path service to use.</param>
        public ProjectTemplateResult CreateProject(IFileService fileService, FilePath filePath)
        {
            var result = CreateProjectCore(fileService, filePath);
            OnProjectCreated(new TemplateResultEventArgs(result));
            return result;
        }

        protected abstract ProjectTemplateResult CreateProjectCore(IFileService fileService, FilePath filePath);

        protected virtual void OnProjectCreated(TemplateResultEventArgs e)
        {
            if (ProjectCreated != null)
                ProjectCreated(this, e);
        }
    }
}
