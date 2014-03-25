using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Build.Construction;

namespace LiteDevelop.Framework.FileSystem.MSBuild
{
    internal static class MSBuildProjectFactory
    {
        public static Solution CreateSolution(string name)
        {
            var solution = new Solution();
            solution.Name = name;
            solution.Version = SolutionVersion.VS2010;

            var solutionConfigPlatforms = new SolutionSection()
            {
                Name = "SolutionConfigurationPlatforms",
                Type = "preSolution",
                SectionType = "Global",
            };

            solutionConfigPlatforms.Add(new KeyValuePair<string,string>("Debug|Any CPU", "Debug|Any CPU"));
            solutionConfigPlatforms.Add(new KeyValuePair<string,string>("Release|Any CPU", "Release|Any CPU"));

            solution.GlobalSections.Add(solutionConfigPlatforms);

            var solutionProperties = new SolutionSection()
            {
                Name = "SolutionProperties",
                Type = "preSolution",
                SectionType = "Global",
            };

            solutionProperties.Add(new KeyValuePair<string,string>("HideSolutionNode", "FALSE"));
            solution.GlobalSections.Add(solutionProperties);

            return solution;
        }

        public static ProjectRootElement CreateNetProject(string targetsFile)
        {
            var root = ProjectRootElement.Create();
            root.DefaultTargets = "Build";

            var importProject = root.AddImport(@"$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props");
            importProject.Condition = @"Exists('$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props')";
            root.AddImport(targetsFile);

            var globalProperties = root.AddPropertyGroup();
            var property = globalProperties.SetProperty("Configuration", "Debug");
            property.Condition = " '$(Configuration)' == '' ";
            property = globalProperties.SetProperty("Platform", "AnyCPU");
            property.Condition = " '$(Platform)' == '' ";
            globalProperties.SetProperty("ProjectGuid", Guid.NewGuid().ToString("B").ToUpper());
            globalProperties.SetProperty("OutputType", "Exe");
            
            var debugProperties = root.AddPropertyGroup();
            debugProperties.Condition = " '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' ";
            debugProperties.SetProperty("PlatformTarget", "AnyCPU");
            debugProperties.SetProperty("DebugSymbols", "true");
            debugProperties.SetProperty("DebugType", "full");
            debugProperties.SetProperty("Optimize", "false");
            debugProperties.SetProperty("OutputPath", @"bin\Debug\");
            debugProperties.SetProperty("DefineConstants", "DEBUG,TRACE");
            debugProperties.SetProperty("ErrorReport", "prompt");
            debugProperties.SetProperty("WarningLevel", "4");

            var releaseProperties = root.AddPropertyGroup();
            releaseProperties.Condition = " '$(Configuration)|$(Platform)' == 'Release|AnyCPU' ";
            releaseProperties.SetProperty("PlatformTarget", "AnyCPU");
            releaseProperties.SetProperty("DebugType", "pdbonly");
            releaseProperties.SetProperty("Optimize", "true");
            releaseProperties.SetProperty("OutputPath", @"bin\Release\");
            releaseProperties.SetProperty("DefineConstants", "TRACE");
            releaseProperties.SetProperty("ErrorReport", "prompt");
            releaseProperties.SetProperty("WarningLevel", "4");

            return root;
        }
    }
}
