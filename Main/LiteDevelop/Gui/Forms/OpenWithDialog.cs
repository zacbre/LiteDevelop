using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LiteDevelop.Framework.Extensions;

namespace LiteDevelop.Gui.Forms
{
    public partial class OpenWithDialog : Form
    {
        public OpenWithDialog(IEnumerable<LiteExtension> extensions)
        {
            InitializeComponent();

            SetupMuiComponents();

            foreach (LiteExtension extension in extensions)
            {
                if (extension is IFileHandler)
                    listView1.Items.Add(new ListViewItem(new string[] { extension.Name, extension.Description }) { Tag = extension });
            }
        }

        public IFileHandler SelectedExtension
        {
            get;
            private set;
        }

        private void SetupMuiComponents()
        {
            var componentMuiIdentifiers = new Dictionary<object, string>()
            {
                {this, "OpenWithDialog.Title"},
                {this.columnHeader1, "OpenWithDialog.ListHeaders.Name"},
                {this.columnHeader2, "OpenWithDialog.ListHeaders.Description"},
                {this.button1, "Common.Ok"},
                {this.button2, "Common.Cancel"},
            };

            LiteDevelopApplication.Current.MuiProcessor.ApplyLanguageOnComponents(componentMuiIdentifiers);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                foreach (ListViewItem item in listView1.Items)
                {
                    item.ForeColor = listView1.ForeColor;
                    item.BackColor = listView1.BackColor;
                }

                listView1.SelectedItems[0].BackColor = SystemColors.Highlight;
                listView1.SelectedItems[0].ForeColor = SystemColors.HighlightText;
                SelectedExtension = listView1.SelectedItems[0].Tag as IFileHandler;
            }
        }
    }
}
