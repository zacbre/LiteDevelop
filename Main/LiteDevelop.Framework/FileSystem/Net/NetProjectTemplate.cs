using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using LiteDevelop.Framework;
using LiteDevelop.Framework.Extensions;
using LiteDevelop.Framework.FileSystem;
using LiteDevelop.Framework.FileSystem.Net;
using LiteDevelop.Framework.Languages.Net;

namespace LiteDevelop.Framework.FileSystem.Net
{
    public class NetProjectTemplate : ProjectTemplate
    {
        public NetProjectTemplate(string name, Bitmap icon,
            NetLanguageDescriptor language,
            SubSystem applicationType,
            params FileTemplate[] files)
            : base(name, icon, files)
        {
            Language = language;
            ApplicationType = applicationType;
            References = new List<string>()
            {
                "System.dll",
            };
        }

        public NetLanguageDescriptor Language
        {
            get;
            private set;
        }

        public SubSystem ApplicationType
        {
            get;
            private set;
        }

        public List<string> References
        {
            get;
            private set;
        }

        /// <inheritdoc />
        protected override ProjectTemplateResult CreateProjectCore(IFileService fileService, FilePath filePath)
        {
            var project = (NetProject)ProjectDescriptor.GetDescriptorByExtension(filePath.Extension).CreateProject(filePath.FileName);

            project.ApplicationType = ApplicationType;
            project.FilePath = filePath;

            var results = new List<TemplateResult>();

            foreach (var file in Files)
            {
                var result = file.CreateFile(
                    fileService,
                    project, 
                    new FilePath(project.ProjectDirectory, file.Name + Language.StandardFileExtension));

                foreach (var createdFile in result.CreatedFiles)
                {
                    project.ProjectFiles.Add(new ProjectFileEntry(createdFile.File as OpenedFile));
                }

                results.Add(result);
            }

            foreach (var reference in References)
                project.References.Add(reference);

            return new ProjectTemplateResult(project, results.ToArray());
        }
    }
}
