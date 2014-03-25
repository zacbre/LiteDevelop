using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;

namespace LiteDevelop.Framework.FileSystem
{
    public enum BuildTarget
    {
        Build,
        Clean,
    }

    /// <summary>
    /// Provides information about a build event being completed or failed
    /// </summary>
    public class BuildResult
    {
        /// <summary>
        /// Converts an instance of <see cref="System.CodeDom.Compiler.CompilerResults"/> to a BuildResult instance.
        /// </summary>
        /// <param name="results">The instance to convert.</param>
        /// <returns>A build result holding the elements of the given <see cref="System.CodeDom.Compiler.CompilerResults"/> instance</returns>
        public static BuildResult FromCompilerResults(CompilerResults results)
        {
            var errors = new BuildError[results.Errors.Count];
            for (int i = 0; i < errors.Length; i++)
            {
                var currentError = results.Errors[i];
                errors[i] = new BuildError(
                    new SourceLocation(
                        new FilePath(currentError.FileName),
                        currentError.Line,
                        currentError.Column),
                    currentError.ErrorText,
                    currentError.IsWarning ? MessageSeverity.Warning : MessageSeverity.Error);
            }


            return new BuildResult(BuildTarget.Build, errors, Path.GetDirectoryName(results.PathToAssembly));
        }

        public BuildResult(BuildTarget target, BuildError[] errors, string outputDirectory)
        {
            Target = target;
            Errors = errors;
            OutputDirectory = outputDirectory;
        }

        /// <summary>
        /// Gets the target of the build operation that is completed.
        /// </summary>
        public BuildTarget Target
        {
            get;
            private set;
        }

        /// <summary>
        /// Indicates the build process has completed succesfully.
        /// </summary>
        public bool Success
        {
            get
            {
                if (Errors != null && Errors.Length > 0)
                {
                    foreach (var error in Errors)
                        if (error.Severity == MessageSeverity.Error)
                            return false;
                }
                return true;
            }
        }

        /// <summary>
        /// Gets all errors exposed in the build process.
        /// </summary>
        public BuildError[] Errors { get; private set; }

        /// <summary>
        /// Gets the output directory of the build process.
        /// </summary>
        public string OutputDirectory { get; private set; }
    }
}
