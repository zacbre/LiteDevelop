using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;

namespace LiteDevelop.Framework.FileSystem
{
    internal class SolutionReader : IDisposable
    {

        private static readonly Regex _solutionHeaderRegex = new Regex(@"Microsoft Visual Studio Solution File, Format Version (?<Version>\d+).00");
        private static readonly Regex _projectHeaderRegex = new Regex(@"(Project\(\""{(?<TypeGuid>.*)}\""\)\s*)=(\s*\""(?<Name>[^\""]*)\""\s*),(\s*\""(?<HintPath>[^\""]*)\""),(\s*\""(?<ProjectGuid>[^\""]*)\"")");
        private static readonly Regex _sectionHeaderRegex = new Regex(@"(?<SectionType>Project|Global)Section\s*\((?<Name>.*)\)\s*=\s*(?<Type>\w*)");
        private static readonly Regex _keyValuePairRegex = new Regex(@"(?<key>.+)\s*=\s*(?<value>[^\r\n]+)");
        
        private TextReader _reader;
        private string _currentLine;
        private int _currentLineIndex;

        public SolutionReader(TextReader reader)
        {
            _reader = reader;
        }

        public void InitializeSolution(Solution solution)
        {

            ReadNextLine();
            solution.Version = GetSolutionVersion(_currentLine);

            Match match;
            SolutionRootNodeType nodeType = SolutionRootNodeType.Unknown;

            ReadNextLine();

            while ((nodeType = GetCurrentRootNodeType(out match)) != SolutionRootNodeType.Unknown)
            {
                switch (nodeType)
                {
                    case SolutionRootNodeType.Project:
                        solution.Nodes.Add(ReadProjectEntry(solution, match));
                        break;
                    case SolutionRootNodeType.Property:
                        break;
                    case SolutionRootNodeType.Global:
                        ReadGlobalSection(solution);
                        break;
                }
                ReadNextLine();
            }

            FixNestedProjects(solution);
        }

        public void Dispose()
        {
            _reader.Dispose();
        }

        private void ThrowSyntaxError(string message)
        {
            throw new ProjectLoadException(string.Format("{0} At line {1}", message, _currentLineIndex));
        }

        private void FixNestedProjects(Solution solution)
        {
            var nestedProjectsSection = solution.GlobalSections.FirstOrDefault(x => x.Name == "NestedProjects");
            if (nestedProjectsSection != null)
            {
                foreach (var item in nestedProjectsSection)
                {
                    Guid projectGuid;
                    Guid folderGuid;

                    if (Guid.TryParse(item.Key, out projectGuid) && Guid.TryParse(item.Value, out folderGuid))
                    {   
                        var project = solution.GetSolutionNode(x => x.ObjectGuid == projectGuid);
                        var folder = (SolutionFolder)solution.GetSolutionNode(x => x.ObjectGuid == folderGuid);
                        if (project != null && folder != null)
                        {
                            project.Parent.Nodes.Remove(project);
                            folder.Nodes.Add(project);
                        }
                    }
                }
            }
        }

        private void ReadNextLine()
        {
            do
            {
                _currentLineIndex++;
                _currentLine = _reader.ReadLine();

                if (_currentLine == null)
                {
                    _currentLine = string.Empty;
                    break;
                }
                else
                    _currentLine = _currentLine.Trim();

            } while (_currentLine == string.Empty || _currentLine.StartsWith("#"));
        }

        private SolutionVersion GetSolutionVersion(string header)
        {
            var match = _solutionHeaderRegex.Match(header);

            if (match.Success)
            {
                return (SolutionVersion)int.Parse(match.Groups["Version"].Value);
            }

            throw new NotSupportedException("Unrecognized or unsupported solution file format.");
        }

        private SolutionRootNodeType GetCurrentRootNodeType(out Match match)
        {
            if (IsAtProjectEntry(out match))
                return SolutionRootNodeType.Project;
            if (IsAtKeyValuePair(out match))
                return SolutionRootNodeType.Property;
            if (_currentLine == "Global")
                return SolutionRootNodeType.Global;

            return SolutionRootNodeType.Unknown;
        }

        private bool IsAtProjectEntry(out Match match)
        {
            match = _projectHeaderRegex.Match(_currentLine);
            return match.Success;
        }

        private SolutionFolder ReadProjectEntry(Solution parent, Match match)
        {
            string path = match.Groups["HintPath"].Value;
            SolutionFolder entry;

            Guid typeID = Guid.Parse(match.Groups["TypeGuid"].Value);

            if (typeID == SolutionFolder.SolutionFolderGuid)
            {
                entry = new SolutionFolder();
            }
            else
            {
                entry = new ProjectEntry();
            }

            entry.TypeGuid = typeID;
            entry.Name = match.Groups["Name"].Value;
            entry.FilePath = new FilePath(parent.FilePath.ParentDirectory.FullPath, match.Groups["HintPath"].Value);
            entry.ObjectGuid = Guid.Parse(match.Groups["ProjectGuid"].Value);

            ReadNextLine();

            Match sectionMatch;
            while (IsAtSection(out sectionMatch))
            {
                entry.Sections.Add(ReadSection(sectionMatch));
                ReadNextLine();
            }
            
            if (_currentLine != "EndProject")
                ThrowSyntaxError("Expected \"EndProject\" keyword.");

            return entry;
        }

        private void ReadGlobalSection(Solution parent)
        {
            Match match;

            ReadNextLine();

            while (IsAtSection(out match))
            {
                var section = ReadSection(match);
                if (section.SectionType != "Global")
                    ThrowSyntaxError("Expected \"GlobalSection\" keyword.");

                parent.GlobalSections.Add(section);

                ReadNextLine();
            }

            if (_currentLine != "EndGlobal")
                ThrowSyntaxError("Expected \"EndGlobal\" keyword.");
        }

        private bool IsAtSection(out Match match)
        {
            match = _sectionHeaderRegex.Match(_currentLine);
            return match.Success;
        }

        private SolutionSection ReadSection(Match match)
        {
            var section = new SolutionSection();
            section.Name = match.Groups["Name"].Value;
            section.SectionType = match.Groups["SectionType"].Value;
            section.Type = match.Groups["Type"].Value;

            ReadNextLine();
            Match sectionMatch;
            while (IsAtKeyValuePair(out sectionMatch))
            {
                section.Add(ReadKeyValuePair(sectionMatch));
                ReadNextLine();
            }


            if (_currentLine != string.Format("End{0}Section", section.SectionType))
                ThrowSyntaxError(string.Format("Expected \"End{0}Section\" keyword.", section.SectionType));

            return section;
        }
        
        private bool IsAtKeyValuePair(out Match match)
        {
            match = _keyValuePairRegex.Match(_currentLine);
            return match.Success;
        }

        private KeyValuePair<string, string> ReadKeyValuePair(Match match)
        {
            return new KeyValuePair<string, string>(match.Groups["key"].Value, match.Groups["value"].Value);
        }

        private enum SolutionRootNodeType
        {
            Unknown,
            Property,
            Project,
            Global,
        }
    }
}
