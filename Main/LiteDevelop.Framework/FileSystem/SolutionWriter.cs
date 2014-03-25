using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LiteDevelop.Framework.FileSystem
{
    internal class SolutionWriter : IDisposable 
    {
        private TextWriter _writer;

        public SolutionWriter(TextWriter writer)
        {
            _writer = writer;
        }

        public void WriteSolution(Solution solution)
        {
            WriteSolutionHeader(solution.Version);

            foreach (var node in solution.Nodes)
            {
                WriteNode(node);
            }

            _writer.WriteLine("Global");

            foreach (var section in solution.GlobalSections)
            {
                WriteSection(section);
            }

            _writer.WriteLine("EndGlobal");

        }

        private void WriteSolutionHeader(SolutionVersion version)
        {
            _writer.WriteLine("Microsoft Visual Studio Solution File, Format Version {0}.00", (int)version);

            _writer.WriteLine("# LiteDevelop v{0}", System.Windows.Forms.Application.ProductVersion);
        }

        private void WriteNode(SolutionNode node)
        {
            _writer.WriteLine("Project(\"{0}\") = \"{1}\", \"{2}\", \"{3}\"", 
                node.TypeGuid.ToString("B").ToUpper(),
                node.Name,
                node.FilePath.GetRelativePath(node.Parent.FilePath.ParentDirectory.FullPath),
                node.ObjectGuid.ToString("B").ToUpper());

            foreach (var section in node.Sections)
            {
                WriteSection(section);
            }

            _writer.WriteLine("EndProject");

            if (node is SolutionFolder)
            {
                foreach (var subNode in (node as SolutionFolder).Nodes)
                {
                    WriteNode(subNode);
                }
            }
        }

        private void WriteSection(SolutionSection section)
        {
            _writer.WriteLine("\t{0}Section({1}) = {2}", section.SectionType, section.Name, section.Type);

            foreach (var entry in section)
            {
                _writer.WriteLine("\t\t{0} = {1}", entry.Key, entry.Value);
            }

            _writer.WriteLine("\tEnd{0}Section", section.SectionType);
        }

        public void Dispose()
        {
            _writer.Dispose();
        }
    }
}
