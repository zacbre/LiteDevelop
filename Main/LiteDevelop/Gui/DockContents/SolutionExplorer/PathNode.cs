using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LiteDevelop.Framework.FileSystem;

namespace LiteDevelop.Gui.DockContents.SolutionExplorer
{
    public abstract class PathNode : AbstractNode, IFilePathProvider
    {
        private FilePath _path;

        public PathNode(FilePath path)
            : base(path.FileName + path.Extension)
        {
            FilePath = path;
        }

        #region IFilePathProvider Members

        public event PathChangedEventHandler FilePathChanged;

        public FilePath FilePath
        {
            get { return _path; }
            set
            {
                if (_path != value)
                {
                    var old = _path;
                    _path = value;
                    OnFilePathChanged(new PathChangedEventArgs(old, _path));
                }
            }
        }

        #endregion
        
        public abstract void RenameEntry(string newName);

        public abstract void DeleteEntry();

        public bool UpdateTextOnFilePathChanged
        {
            get;
            set;
        }

        protected virtual void OnFilePathChanged(PathChangedEventArgs e)
        {
            if (UpdateTextOnFilePathChanged)
                Text = FilePath.FileName + FilePath.Extension;

            if (FilePathChanged != null)
                FilePathChanged(this, e);
        }
    }
}
