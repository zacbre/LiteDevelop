using System;
using System.Linq;
using LiteDevelop.Framework.Extensions;
using LiteDevelop.Framework.FileSystem;
using LiteDevelop.Framework.Gui;

namespace LiteDevelop.Extensions
{
    public class FileServiceOpenedFile : OpenedFile
    {
        private ILiteExtensionHost _extensionHost;

        public FileServiceOpenedFile(ILiteExtensionHost extensionHost, FilePath filePath)
            : base(filePath)
        {
            _extensionHost = extensionHost;
            extensionHost.ControlManager.SelectedDocumentContentChanged += ControlManager_SelectedDocumentContentChanged;
        }

        private void ControlManager_SelectedDocumentContentChanged(object sender, EventArgs e)
        {
            if (CurrentDocumentContent != _extensionHost.ControlManager.SelectedDocumentContent)
            {
                foreach (var documentContent in RegisteredDocumentContents)
                    if (documentContent == _extensionHost.ControlManager.SelectedDocumentContent)
                    {
                        CurrentDocumentContent = documentContent;
                        break;
                    }

            }
        }

        public override void UnRegisterDocumentContent(LiteDocumentContent documentContent)
        {
            base.UnRegisterDocumentContent(documentContent);
            if (RegisteredDocumentContents.Count == 0)
                HasUnsavedData = false;
        }
    }
}
