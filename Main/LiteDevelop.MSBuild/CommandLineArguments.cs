using System;
using System.Linq;
using LiteDevelop.Framework.FileSystem;
using Microsoft.Build.Framework;

namespace LiteDevelop.MSBuild
{
    public class CommandLineArguments
    {
        public string InputFile;
        public BuildTarget Target;
        public LoggerVerbosity Verbosity;
        public bool DetailedSummary;

        public static CommandLineArguments Parse(string[] rawArguments)
        {
            var arguments = new CommandLineArguments();

            for (int i = 0; i < rawArguments.Length;i++)
            {
                string argument = rawArguments[i].ToLower();

                switch (argument)
                {
                    case "-file":
                        arguments.InputFile = rawArguments[i + 1].Replace("\"", "");
                        i++;
                        break;
                    case "-target":
                        arguments.Target = (BuildTarget)Enum.Parse(typeof(BuildTarget), rawArguments[i + 1], true);
                        i++;
                        break;
                    case "-verbosity":
                        arguments.Verbosity = (LoggerVerbosity)Enum.Parse(typeof(LoggerVerbosity), rawArguments[i + 1], true);
                        i++;
                        break;
                    case "-summary":
                        arguments.DetailedSummary = true;
                        break;
                    default:
                        throw new ArgumentException(string.Format("Unrecognized command line argument '{0}'. ", argument));
                }

            }

            return arguments;
        }
    }
}
