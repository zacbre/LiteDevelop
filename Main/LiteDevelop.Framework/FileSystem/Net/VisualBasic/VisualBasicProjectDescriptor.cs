using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiteDevelop.Framework.Languages;
using LiteDevelop.Framework.Languages.Net;

namespace LiteDevelop.Framework.FileSystem.Net.VisualBasic
{
    /// <summary>
    /// Provides information about a Visual Basic project. 
    /// </summary>
    public class VisualBasicProjectDescriptor : ProjectDescriptor
    {
        private static EventBasedCollection<LanguageDescriptor> _languages = new EventBasedCollection<LanguageDescriptor>()
        {
            LanguageDescriptor.GetLanguage<VisualBasicLanguage>(),
        };

        /// <inheritdoc />
        public override string ProjectExtension
        {
            get { return ".vbproj"; }
        }

        /// <inheritdoc />
        public override string MSBuildTargetsFile
        {
            get { return @"$(MSBuildToolsPath)\Microsoft.VisualBasic.targets"; }
        }

        /// <inheritdoc />
        public override Guid SolutionNodeGuid
        {
            get { return new Guid("{F184B08F-C81C-45F6-A57F-5ABD9991F28F}"); }
        }

        /// <inheritdoc />
        public override EventBasedCollection<LanguageDescriptor> Languages
        {
            get { return _languages; }
        }

        /// <inheritdoc />
        public override Project LoadProject(FilePath filePath)
        {
            return new VisualBasicProject(filePath);
        }

        /// <inheritdoc />
        public override Project CreateProject(string name)
        {
            return new VisualBasicProject(name);
        }
    }
}
