using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Build.Framework;

namespace LiteDevelop.Framework.FileSystem.MSBuild
{
    /// <summary>
    /// Builds and cleans projects in MSBuild format.
    /// </summary>
    public class MSBuildInvoker
    {
        /// <summary>
        /// Occurs when the builder has either finished building or cleaning a solution.
        /// </summary>
        public event BuildResultEventHandler CompletedOperation;

        private static readonly Regex _outputMessageRegex = new Regex(@"(?<messagetype>(error|warning|message)):\s+(line=(?<line>\d+))\s+(column=(?<column>\d+))\s+(project=""(?<project>[^""]*)"")\s+(file=""(?<file>[^""]*)"")\s+=>\s+(?<message>[^\r\n]*)");
        private static readonly Regex _projectMessageRegex = new Regex(@"project (?<messagetype>(started|finished)):\s+(file=""(?<file>[^""]*)"")");

        private readonly List<BuildError> _lastErrors = new List<BuildError>();
        private readonly Solution _solution;
        
        private IProgressReporter _currentReporter;
        private BuildTarget? _currentTarget;
        private int _projectCount;
        private List<string> _projectsFinished = new List<string>();
        
        public MSBuildInvoker(Solution solution)
        {
            _solution = solution;
        }

        /// <summary>
        /// Gets a value indicating whether the builder is busy or not.
        /// </summary>
        public bool IsBusy
        {
            get { return _currentTarget.HasValue; }
        }

        /// <summary>
        /// Starts building the solution asynchronously.
        /// </summary>
        /// <param name="reporter">The progress reporter to use of logging.</param>
        public void BuildAsync(IProgressReporter reporter)
        {
            _currentReporter = reporter;
            StartProcess(BuildTarget.Build);
        }

        /// <summary>
        /// Starts cleaning the solution asynchronously.
        /// </summary>
        /// <param name="reporter">The progress reporter to use of logging.</param>
        public void CleanAsync(IProgressReporter reporter)
        {
            _currentReporter = reporter;
            StartProcess(BuildTarget.Clean);
        }

        private void AssertIsBusy()
        {
            if (IsBusy)
                throw new InvalidOperationException("The builder is already busy with another operation.");
        }

        private static int CountProjects(SolutionFolder folder)
        {
        	int count = 0;
        	
        	if (folder is ProjectEntry || folder is Solution)
        		count++;
        	
        	foreach (var node in folder.Nodes)
        	{
        		if (node is SolutionFolder)
	        	{
        			count += CountProjects(node as SolutionFolder);
	        	}
        	}
        	
        	return count;
        }
        
        private void StartProcess(BuildTarget target)
        {
            AssertIsBusy();
            
            _projectCount = CountProjects(_solution);
			_currentReporter.ProgressPercentage = 0;
			_projectsFinished.Clear();
            _lastErrors.Clear();
            _currentReporter.ProgressVisible = true;

            var process = new Process();
            process.StartInfo = new ProcessStartInfo("LiteDevelop.MSBuild.exe", string.Format("-target {0} -file \"{1}\" -verbosity {2} {3}", _currentTarget = target, _solution.FilePath.FullPath, LoggerVerbosity.Minimal, "-summary"))
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            process.EnableRaisingEvents = true;
            process.OutputDataReceived += process_OutputDataReceived;
            process.Exited += process_Exited;
            process.Start();
            process.BeginOutputReadLine();

        }

        private void process_Exited(object sender, EventArgs e)
        {
            var target = _currentTarget.Value;
            _currentTarget = null;
            _currentReporter.Report("Finished.");
            OnCompletedOperation(new BuildResultEventArgs(new BuildResult(target, _lastErrors.ToArray(), string.Empty)));
            _currentReporter.ProgressVisible = false;
        }

        private void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
        	if (!string.IsNullOrEmpty(e.Data))
            {
                MessageSeverity severity = MessageSeverity.Message;
                try
                {
                    var match = _outputMessageRegex.Match(e.Data);
                    if (match.Success)
                    {
                        var file = new FilePath(Path.GetDirectoryName(match.Groups["project"].Value), match.Groups["file"].Value);
                        string messageType = match.Groups["messagetype"].Value;
                        severity = (MessageSeverity)Enum.Parse(typeof(MessageSeverity), messageType, true);
                        _lastErrors.Add(new BuildError(
                            new SourceLocation(
                                file,
                                int.Parse(match.Groups["line"].Value),
                                int.Parse(match.Groups["column"].Value)),
                            match.Groups["message"].Value,
                            severity));
                    }
                    else if ((match = _projectMessageRegex.Match(e.Data)).Success)
                    {
                    	string file = match.Groups["file"].Value;
                    	bool finished = match.Groups["messagetype"].Value == "finished";
                    	if (finished && !_projectsFinished.Contains(file))
                    	{
                    		_projectsFinished.Add(file);
                    	}
                    	_currentReporter.ProgressPercentage = (int)(((double)_projectsFinished.Count / (double)_projectCount) * 100);
                    }
                }
                catch(Exception ex)
                {
                    _lastErrors.Add(new BuildError(
                        new SourceLocation(
                            new FilePath("LiteDevelop.Framework.dll"), 
                            0, 0),
                        ex.ToString(), 
                        MessageSeverity.Error));
                }
                 _currentReporter.Report(severity, e.Data);
            }
        }

        protected virtual void OnCompletedOperation(Exception ex)
        {
            OnCompletedOperation(new BuildResultEventArgs(
                new BuildResult(_currentTarget.Value,
                    new BuildError[] 
                    { 
                        new BuildError(
                            new SourceLocation(
                                new FilePath("LiteDevelop.Framework.dll"),
                                0,0),
                            ex.ToString(), 
                            MessageSeverity.Error) 
                    }, string.Empty)));
        }

        protected virtual void OnCompletedOperation(BuildResultEventArgs e)
        {
            if (CompletedOperation != null)
                CompletedOperation(this, e);
        }
    }
}
