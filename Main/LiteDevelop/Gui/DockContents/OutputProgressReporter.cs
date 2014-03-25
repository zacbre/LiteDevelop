using System;
using System.Linq;
using System.Windows.Forms;
using LiteDevelop.Framework;
using LiteDevelop.Framework.Extensions;
using LiteDevelop.Gui.DockContents;
using LiteDevelop.Gui.Forms;

namespace LiteDevelop.Gui.DockContents
{
    public class OutputProgressReporter : INamedProgressReporter
    {
        private OutputContent _outputWindow;
        private ProgressBar _progressBar;
        private string _displayName;

        public OutputProgressReporter(string id, OutputContent outputWindow, ProgressBar progressBar)
        {
            Identifier = DisplayName = id;
            _outputWindow = outputWindow;
            _progressBar = progressBar;
        }


        #region INamedProgressReporter Members

        public event EventHandler DisplayNameChanged;

        public string Identifier
        {
            get;
            private set;
        }

        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                if (_displayName != value)
                {
                    _displayName = value;
                    if (DisplayNameChanged != null)
                        DisplayNameChanged(this, EventArgs.Empty);
                }
            }
        }

        #endregion

        #region IProgressReporter Members

        public void Report(MessageSeverity severity, string message)
        {
            _outputWindow.Report(Identifier, severity, message);
        }

        public int ProgressPercentage
        {
            get
            {
                return (int)_outputWindow.Invoke(new Func<int>(() => { return _progressBar.Value; }));
            }
            set
            {
                _outputWindow.Invoke(new Action(() => { _progressBar.Value = value; }));
            }
        }

        public bool ProgressVisible
        {
            get
            {
                return (bool)_outputWindow.Invoke(new Func<bool>(() => { return _progressBar.Visible; }));
            }
            set
            {
                _outputWindow.Invoke(new Action(() => { _progressBar.Visible = value; }));
            }
        }
        
        #endregion
    }
}
