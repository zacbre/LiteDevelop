using System;
using System.Drawing;
using System.Linq;
using System.Text;
using LiteDevelop.Framework.Extensions;
using LiteDevelop.Framework.FileSystem;

namespace LiteDevelop.Framework.FileSystem
{
    public class SourceFileTemplate : FileTemplate
    {
        public SourceFileTemplate(string name, Bitmap icon, IFileHandler extensionToUse, string baseSource)
            : this(name, icon, extensionToUse, baseSource, (f) => { })
        {
        }

        public SourceFileTemplate(string name, Bitmap icon, IFileHandler extensionToUse, string baseSource, Action<OpenedFile> action)
            : base(name, icon, extensionToUse)
        {
            BaseSource = baseSource;
            Action = action;
        }

        /// <inheritdoc />
        public override bool ProjectRequired
        {
            get { return false; }
        }

        public string BaseSource
        { 
            get; 
            set;
        }

        public Action<OpenedFile> Action
        {
            get;
            set;
        }

        /// <inheritdoc />
        protected override TemplateResult CreateFileCore(IFileService fileService, Project parentProject, FilePath filePath)
        {
            var file = fileService.CreateFile(filePath, Encoding.UTF8.GetBytes(BaseSource));
            Action(file);
            return new TemplateResult(new CreatedFile(file, ExtensionToUse));
        }
    }
}
