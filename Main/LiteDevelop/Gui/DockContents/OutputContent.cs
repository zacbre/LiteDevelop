using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using LiteDevelop.Framework;
using WeifenLuo.WinFormsUI.Docking;

namespace LiteDevelop.Gui.DockContents
{
    public partial class OutputContent : DockContent
    {
        private class ReporterContext
        {
            public ReporterContext(INamedProgressReporter reporter)
            {
                Reporter = reporter;
                Name = reporter.DisplayName;
                Builder = new StringBuilder();
            }

            public INamedProgressReporter Reporter
            {
                get;
                private set;
            }

            public string Name
            {
                get;
                set;
            }

            public StringBuilder Builder
            {
                get;
                private set;
            }
        }

        private Dictionary<object, string> _componentMuiIdentifiers;
        private Dictionary<INamedProgressReporter, ReporterContext> _reporterContexts = new Dictionary<INamedProgressReporter,ReporterContext>();

        public OutputContent()
        {
            InitializeComponent();
            this.HideOnClose = true;
            this.Icon = Icon.FromHandle(Properties.Resources.text.GetHicon());
            LiteDevelopApplication.Current.InitializedApplication += Current_InitializedApplication;
            NamedReporters = new ProgressReporterCollection<INamedProgressReporter>();
            NamedReporters.InsertedItem += NamedReporters_InsertedItem;
            NamedReporters.RemovedItem += NamedReporters_RemovedItem;
        }

        public ProgressReporterCollection<INamedProgressReporter> NamedReporters
        {
            get;
            private set;
        }

        private void NamedReporters_RemovedItem(object sender, CollectionChangedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                var reporter = e.TargetObject as INamedProgressReporter;
                _reporterContexts.Remove(reporter);
                outputSourcesToolStripComboBox.Items.RemoveAt(e.TargetIndex);
            }));
        }

        private void NamedReporters_InsertedItem(object sender, CollectionChangedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                var reporter = e.TargetObject as INamedProgressReporter;
                _reporterContexts.Add(reporter, new ReporterContext(reporter));

                outputSourcesToolStripComboBox.Items.Insert(e.TargetIndex, reporter.DisplayName);

                reporter.DisplayNameChanged += reporter_DisplayNameChanged;
            }));
        }

        private void reporter_DisplayNameChanged(object sender, EventArgs e)
        {
            var reporter = sender as INamedProgressReporter;
            var context = _reporterContexts[reporter];
            context.Name = (string)(outputSourcesToolStripComboBox.Items[outputSourcesToolStripComboBox.Items.IndexOf(context.Name)] = reporter.DisplayName);
        }

        private void Current_InitializedApplication(object sender, EventArgs e)
        {
            _componentMuiIdentifiers = new Dictionary<object, string>()
            {
                {this, "OutputContent.Title"},
                {showOutputFromLabel, "OutputContent.ShowOutputFrom"},
                {clearAllToolStripButton, "OutputContent.ClearAll"},
                {wordWrapToolStripButton, "OutputContent.ToggleWordWrap"},
            };
            
            LiteDevelopApplication.Current.ExtensionHost.UILanguageChanged += ExtensionHost_UILanguageChanged;
            LiteDevelopApplication.Current.ExtensionHost.ControlManager.AppearanceChanged += ControlManager_AppearanceChanged;
            ExtensionHost_UILanguageChanged(null, null);

            textBox1.WordWrap = wordWrapToolStripButton.Checked = LiteDevelopSettings.Instance.GetValue<bool>("OutputWindow.WordWrap");
        }

        public void Report(string id, MessageSeverity severity, string message)
        {
            Invoke(new Action(() =>
                {
                    var logger = NamedReporters.GetReporterById(id);
                    EnsureReporterIsSelected(logger);
                    UpdateOutputLog(logger);
                    AppendText(logger, severity, message + Environment.NewLine);
                }));
        }

        private void EnsureReporterIsSelected(INamedProgressReporter reporter)
        {
            var index = NamedReporters.IndexOf(reporter);

            if (outputSourcesToolStripComboBox.SelectedIndex != index)
            {
                outputSourcesToolStripComboBox.SelectedIndex = index;
                UpdateOutputLog(reporter);
            }
        }

        private void UpdateOutputLog(INamedProgressReporter reporter)
        {
            textBox1.Text = _reporterContexts[reporter].Builder.ToString();
            ScrollToEnd();
        }

        private void AppendText(INamedProgressReporter reporter, MessageSeverity severity, string message)
        {
            var builder = _reporterContexts[reporter].Builder;
            string formatted = string.Format("[{0}]: {1}", severity, message);
            builder.Append(formatted);

            if (outputSourcesToolStripComboBox.SelectedIndex == NamedReporters.IndexOf(reporter))
            {
                textBox1.AppendText(formatted);
                ScrollToEnd();
            }
        }

        private void ScrollToEnd()
        {
            textBox1.SelectionStart = textBox1.TextLength;
            textBox1.ScrollToCaret();
        }

        private void outputSourcesToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateOutputLog(NamedReporters[outputSourcesToolStripComboBox.SelectedIndex]);
        }

        private void wordWrapToolStripButton_Click(object sender, EventArgs e)
        {
            LiteDevelopSettings.Instance.SetValue("OutputWindow.WordWrap", textBox1.WordWrap = wordWrapToolStripButton.Checked);
            ScrollToEnd();
        }

        private void clearAllToolStripButton_Click(object sender, EventArgs e)
        {
            textBox1.Clear();

            foreach (var contexts in _reporterContexts.Values)
            {
                contexts.Builder.Clear();
            }
        }
        
        private void ExtensionHost_UILanguageChanged(object sender, EventArgs e)
        {
            LiteDevelopApplication.Current.MuiProcessor.ApplyLanguageOnComponents(_componentMuiIdentifiers);
        }

        private void ControlManager_AppearanceChanged(object sender, EventArgs e)
        {
            var processor = LiteDevelopApplication.Current.ExtensionHost.ControlManager.GlobalAppearanceMap.Processor;
            processor.ApplyAppearanceOnObject(this, Framework.Gui.DefaultAppearanceDefinition.Window);
            processor.ApplyAppearanceOnObject(textBox1, Framework.Gui.DefaultAppearanceDefinition.TextBox);
        }

    }
}
