using System;
using System.Collections.Generic;
using System.Linq;
using LiteDevelop.Framework.Extensions;

namespace LiteDevelop.Framework.FileSystem
{
    public class TemplateResult
    {
        public TemplateResult(params CreatedFile[] files)
        {
            CreatedFiles = new List<CreatedFile>();
            CreatedFiles.AddRange(files);
        }

        public TemplateResult(params TemplateResult[] results)
        {
            CreatedFiles = new List<CreatedFile>();
            foreach (var result in results)
            {
                CreatedFiles.AddRange(result.CreatedFiles);
            }
        }

        public List<CreatedFile> CreatedFiles
        {
            get;
            private set;
        }
    }

    public class ProjectTemplateResult : TemplateResult
    {
        public ProjectTemplateResult(Project project, params TemplateResult[] results)
            : base(results)
        {
            Project = project;
        }

        public Project Project
        {
            get;
            private set;
        }
    }

    public class CreatedFile
    {
        public CreatedFile(OpenedFile file, IFileHandler handler)
        {
            File = file;
            ExtensionToUse = handler;
        }

        public OpenedFile File
        {
            get;
            private set;
        }

        public IFileHandler ExtensionToUse
        {
            get;
            set;
        }
    }

    public delegate void TemplateResultEventHandler(object sender, TemplateResultEventArgs e);

    public class TemplateResultEventArgs : EventArgs 
    {
        public TemplateResultEventArgs(TemplateResult result)
        {
            Result = result;
        }

        public TemplateResult Result
        {
            get;
            private set;
        }
    }

}
