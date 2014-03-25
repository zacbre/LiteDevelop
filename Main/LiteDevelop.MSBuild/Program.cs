using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;

namespace LiteDevelop.MSBuild
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("LiteDevelop MSBuild");

                var arguments = CommandLineArguments.Parse(args);

                if (!File.Exists(arguments.InputFile))
                    throw new ArgumentException("File does not exist.");

                var buildParameters = new BuildParameters();
                buildParameters.DetailedSummary = arguments.DetailedSummary;
                buildParameters.Loggers = new ILogger[] 
                { 
                    new ConsoleLogger() { Verbosity = arguments.Verbosity }
                };

                Dictionary<string, string> properties = new Dictionary<string, string>();

                var buildRequest = new BuildRequestData(arguments.InputFile, properties, null, new string[] { arguments.Target.ToString() }, null);
                
              //  Microsoft.Build.Execution.BuildManager.DefaultBuildManager.
                var results = Microsoft.Build.Execution.BuildManager.DefaultBuildManager.Build(buildParameters, buildRequest);

            }
            catch (Exception ex)
            {
                Console.WriteLine("error: line=0 column=0 file=\"LiteDevelop.MSBuild.exe\" => " + ex.Message);
                Console.WriteLine("This program should only be executed by LiteDevelop itself. If you believe this is a bug, please report the issue.");
            }
        }
    }
}
