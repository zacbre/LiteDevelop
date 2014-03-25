using System;
using System.Drawing;
using System.Linq;
using LiteDevelop.Framework.Extensions;

namespace LiteDevelop.Framework.FileSystem
{
    /// <summary>
    /// Represents a file template that can be used by a LiteDevelop user.
    /// </summary>
    public abstract class FileTemplate : Template
    {
        public event TemplateResultEventHandler FileCreated;

        public FileTemplate(string name, Bitmap icon, IFileHandler extensionToUse)
            :base(name, icon)
        {
            ExtensionToUse = extensionToUse;
        }

        /// <summary>
        /// Gets or sets the extension to use when opening the new file.
        /// </summary>
        public IFileHandler ExtensionToUse
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether the template can only be used when the container project of the created file is specified.
        /// </summary>
        public abstract bool ProjectRequired { get; }

        /// <summary>
        /// Creates a file from current template.
        /// </summary>
        /// <param name="fileService">The file service the template should use to create files.</param>
        /// <param name="filePath">The file path to save the file to.</param>
        /// <returns>A template result holding information about the created files.</returns>
        public TemplateResult CreateFile(IFileService fileService, FilePath filePath)
        {
            return CreateFile(fileService, null, filePath);
        }

        /// <summary>
        /// Creates a file from the current template.
        /// </summary>
        /// <param name="fileService">The file service the template should use to create files.</param>
        /// <param name="parentProject">The project that will hold the new created file. This value can be null if ProjectRequired is set to false.</param>
        /// <param name="filePath">The file path to save the file to.</param>
        /// <returns>A template result holding information about the created files.</returns>
        public TemplateResult CreateFile(IFileService fileService, Project parentProject, FilePath filePath)
        {
            if (fileService == null)
                throw new ArgumentNullException("fileService");
            if (parentProject == null && ProjectRequired)
                throw new ArgumentNullException("parentProject");

            var result = CreateFileCore(fileService, parentProject, filePath);
            OnFileCreated(new TemplateResultEventArgs(result));
            return result;
        }

        /// <summary>
        /// Creates a file from the current template.
        /// </summary>
        /// <param name="fileService">The file service the template should use to create files.</param>
        /// <param name="parentProject">The project that will hold the new created file.</param>
        /// <param name="filePath">The file path to save the file to.</param>
        /// <returns>A template result holding information about the created files.</returns>
        protected abstract TemplateResult CreateFileCore(IFileService fileService, Project parentProject, FilePath filePath);

        /// <summary>
        /// Raises the <see cref="FileCreated"/> event.
        /// </summary>
        /// <param name="e">The event arguments to send.</param>
        protected virtual void OnFileCreated(TemplateResultEventArgs e)
        {
            if (FileCreated != null)
                FileCreated(this, e);
        }
    }
}
