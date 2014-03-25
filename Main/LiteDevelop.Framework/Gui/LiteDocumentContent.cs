using System;
using System.IO;
using System.Linq;
using LiteDevelop.Framework.Extensions;
using LiteDevelop.Framework.FileSystem;

namespace LiteDevelop.Framework.Gui
{
    /// <summary>
    /// A document view content base in LiteDevelop.
    /// </summary>
    public abstract class LiteDocumentContent : LiteViewContent
    {
        public event EventHandler AssociatedFileChanged;

        private OpenedFile _associatedFile;
        private LiteExtension _parent;

        public LiteDocumentContent()
        {
        }

        public LiteDocumentContent(LiteExtension parent)
        {
            this._parent = parent;
        }


        /// <summary>
        /// Gets or sets the file associated with this document view content.
        /// </summary>
        public OpenedFile AssociatedFile
        {
            get
            {
                return _associatedFile;
            }
            set
            {
                if (_associatedFile != value)
                {
                   if (_associatedFile != null)
                       _associatedFile.UnRegisterDocumentContent(this);

                    _associatedFile = value;

                    if (_associatedFile != null)
                        _associatedFile.RegisterDocumentContent(this);

                    OnAssociatedFileChanged(EventArgs.Empty);
                }
            }
        }

        public LiteExtension ParentExtension
        {
            get { return _parent; }
        }

        public abstract void Save(Stream stream);

        /// <inheritdoc />
        public override void Dispose()
        {
            base.Dispose();
            AssociatedFile = null;
        }

        protected virtual void OnAssociatedFileChanged(EventArgs e)
        {
            if (AssociatedFileChanged != null)
                AssociatedFileChanged(this, e);
        }

    }
}
