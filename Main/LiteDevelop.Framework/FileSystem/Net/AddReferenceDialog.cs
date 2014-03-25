using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace LiteDevelop.Framework.FileSystem.Net
{
    public partial class AddReferenceDialog : Form
    {
        private static List<string> _assemblyCache = new List<string>();

        static AddReferenceDialog()
        {
            foreach (var file in Directory.GetFiles(Path.GetDirectoryName(typeof(object).Assembly.Location)))
            {
                var name = Path.GetFileName(file);
                if (Path.GetExtension(file) == ".dll" && name.StartsWith("System") || name.StartsWith("Microsoft"))
                {
                    _assemblyCache.Add(name);
                }
            }
        }

        public AddReferenceDialog()
        {
            InitializeComponent();
            assemblyListBox.Items.AddRange(_assemblyCache.ToArray());
        }

        public string SelectedAssembly
        { 
            get 
            {
                return (string)assemblyListBox.SelectedItem; 
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog()
            {
                Filter = "Assemblies|*.exe;*.dll"
            };
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                assemblyListBox.Items.Add(ofd.FileName);
            }
        }
    }
}
