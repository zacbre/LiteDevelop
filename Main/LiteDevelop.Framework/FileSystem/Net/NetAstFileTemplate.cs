using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using LiteDevelop.Framework.Extensions;
using LiteDevelop.Framework.FileSystem;
using LiteDevelop.Framework.Languages;
using LiteDevelop.Framework.Languages.Net;

namespace LiteDevelop.Framework.FileSystem.Net
{
    public class NetAstFileTemplate : FileTemplate
    {
        public NetAstFileTemplate(string name, string objectName, Bitmap icon, IFileHandler extensionToUse, CodeCompileUnit compileUnit)
            : base(name, icon, extensionToUse)
        {
            ObjectName = objectName;
            CompileUnit = compileUnit;
        }

        /// <inheritdoc />
        public override bool ProjectRequired
        {
            get { return false; }
        }
        
        public string ObjectName 
        { 
            get; 
            set; 
        }

        public CodeCompileUnit CompileUnit
        {
            get;
            set;
        }

        /// <inheritdoc />
        protected override TemplateResult CreateFileCore(IFileService fileService, Project parentProject, FilePath filePath)
        {
            string @namespace = "MyNamespace";
            
            if (parentProject != null && parentProject is NetProject)
            {
                @namespace = (parentProject as NetProject).RootNamespace;
                var relativePath = filePath.ParentDirectory.GetRelativePath(parentProject);
                if (!string.IsNullOrEmpty(relativePath))
                    @namespace += "." + relativePath.Replace(Path.DirectorySeparatorChar, '.');
            }
            
            var language = (LanguageDescriptor.GetLanguageByPath(filePath) as NetLanguageDescriptor);
            var codeProvider = language.CodeProvider;
            var fileName = filePath.FileName;

            using (var stringWriter = new StringWriter())
            {
                // generate source
                using (var writer = new IndentedTextWriter(stringWriter))
                {
                    codeProvider.GenerateCodeFromCompileUnit(CompileUnit, writer, new CodeGeneratorOptions()
                    {
                        BlankLinesBetweenMembers = true,
                        BracingStyle = "C",
                    });
                }

                string source = stringWriter.ToString();

                // replace auto generated message.
                var match = Regex.Match(source, string.Format("{0}[^\r\n]+\r\n[^{0}]", language.CommentPrefix));
                if (match.Success)
                {
                    source = source.Remove(0, match.Index + match.Length);
                }

                // VbCodeProvider adds two option statements which should be removed as well because they are specified by default in the project settings.
                if (language.Name == LanguageDescriptor.GetLanguage<VisualBasicLanguage>().Name)
                {
                    match = Regex.Match(source, @"Option\s+\w+\s+\w+\s+Option\s+\w+\s+\w+");
                    if (match.Success)
                        source = source.Remove(0, match.Index + match.Length);
                }

                // create file instance.
                source = source.Replace("%folder%", @namespace)
                    .Replace("%file%", (string.IsNullOrEmpty(ObjectName) ? fileName : ObjectName));

                var file = fileService.CreateFile(filePath, Encoding.UTF8.GetBytes(source));
                return new TemplateResult(new CreatedFile(file, ExtensionToUse));
            }
        }
    }
}
