using System;
using System.Drawing;
using System.Linq;
using LiteDevelop.Framework.Extensions;
using LiteDevelop.Framework.FileSystem;
using LiteDevelop.Framework.FileSystem.Net;

namespace LiteDevelop.Essentials.FormsDesigner
{
    public class NetFormTemplate : FileTemplate
    {
        public NetFormTemplate(string name, Bitmap icon, IFileHandler extensionToUse, NetAstFileTemplate designerTemplate, NetAstFileTemplate classTemplate)
            : base(name, icon, extensionToUse)
        {
            DesignerClassFile = designerTemplate;
            ClassFile = classTemplate;
        }

        public override bool ProjectRequired
        {
            get { return true; }
        }

        public NetAstFileTemplate DesignerClassFile
        {
            get;
            set;
        }

        public NetAstFileTemplate ClassFile
        {
            get;
            set;
        }

        protected override TemplateResult CreateFileCore(IFileService fileService, Project parentProject, FilePath filePath)
        {
            string directory = filePath.ParentDirectory.FullPath;
            string fileName = filePath.FileName;
            string extension = filePath.Extension;

            var classFileResult = ClassFile.CreateFile(fileService, parentProject, filePath);
            var designerFileResult = DesignerClassFile.CreateFile(fileService, parentProject, new FilePath(directory, DesignerClassFile.Name.Replace("%file%", fileName) + extension));

            var classFile = classFileResult.CreatedFiles[0].File as OpenedFile;
            var designerFile = designerFileResult.CreatedFiles[0].File as OpenedFile;

            designerFile.Dependencies.Add(classFile.FilePath.GetRelativePath(directory));
            designerFile.SetContents(designerFile.GetContentsAsString().Replace("%file%", fileName));
            designerFileResult.CreatedFiles[0].ExtensionToUse = ExtensionToUse;

            return new TemplateResult(classFileResult, designerFileResult);
        }
    }
}
