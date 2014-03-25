using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiteDevelop.Framework.Languages;
using LiteDevelop.Framework.Languages.Net;

namespace LiteDevelop.Framework.FileSystem.Net.CSharp
{
    /// <summary>
    /// Provides information about a C# project. 
    /// </summary>
    public class CSharpProjectDescriptor : ProjectDescriptor 
    {
        private static EventBasedCollection<LanguageDescriptor> _languages = new EventBasedCollection<LanguageDescriptor>()
        {
            LanguageDescriptor.GetLanguage<CSharpLanguage>(),
        };

        /// <inheritdoc />
        public override string ProjectExtension
        {
            get { return ".csproj"; }
        }

        /// <inheritdoc />
        public override string MSBuildTargetsFile
        {
            get { return @"$(MSBuildToolsPath)\Microsoft.CSharp.targets"; }
        }

        /// <inheritdoc />
        public override Guid SolutionNodeGuid
        {
            get { return new Guid("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"); }
        }

        /// <inheritdoc />
        public override EventBasedCollection<LanguageDescriptor> Languages
        {
            get { return _languages; }
        }

        /// <inheritdoc />
        public override Project LoadProject(FilePath filePath)
        {
            return new CSharpProject(filePath);
        }

        /// <inheritdoc />
        public override Project CreateProject(string name)
        {
            return new CSharpProject(name);
        }
    }
}
