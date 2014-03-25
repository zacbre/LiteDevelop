using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Drawing.Design;
using System.IO;
using System.Text;
using System.Windows.Forms;
using LiteDevelop.Framework.Extensions;
using LiteDevelop.Framework.FileSystem;
using LiteDevelop.Framework.Gui;
using LiteDevelop.Framework.Languages;
using LiteDevelop.Framework.Languages.Net;
using LiteDevelop.Essentials.FormsDesigner.Gui;
using LiteDevelop.Essentials.FormsDesigner.Services;

namespace LiteDevelop.Essentials.FormsDesigner.Gui
{
    public class FormsDesignerContent : LiteDocumentContent, IClipboardHandler, IPropertyContainerProvider, IToolboxServiceProvider 
    {
        private DesignSurfaceManager _surfaceManager;
        private ServiceContainer _serviceContainer;
        private DesignSurface _surface;
        private IDesignerHost _designerHost;

        private ErrorControl _errorControl;
        private Control _viewControl;
        private ILiteExtensionHost _extensionHost;
        private DesignerCodeWriter _codeWriter;
        private DesignerCodeReader _codeReader;
        private PropertyContainer _propertyContainer;
        private NetLanguageDescriptor _language;
        private string _namespace;
        private bool _includeBaseType;
        private ToolboxService _toolboxService;

        public FormsDesignerContent(FormsDesignerExtension parent, OpenedFile sourceFile)
            : base(parent)
        {
            _language = (NetLanguageDescriptor)LanguageDescriptor.GetLanguageByPath(sourceFile.FilePath);
            if (!(_language is NetLanguageDescriptor))
                throw new ArgumentException("File must be a .NET source file.");
            
            _extensionHost = parent.ExtensionHost;
            _extensionHost.ControlManager.AppearanceChanged += ControlManager_AppearanceChanged;

            _propertyContainer = new PropertyContainer();

            _serviceContainer = new ServiceContainer();
            _surfaceManager = parent.DesignerSurfaceManager;

            _codeReader = new DesignerCodeReader(_extensionHost, _language);
            _codeWriter = new DesignerCodeWriter(_language);

            this.Text = sourceFile.FilePath.FileName + sourceFile.FilePath.Extension + " [Design]";
            this.AssociatedFile = sourceFile;
            this.AssociatedFile.FilePathChanged += AssociatedFile_FilePathChanged;
            
            _errorControl = new ErrorControl()
            {
                Dock = DockStyle.Fill,
            };
            _errorControl.ReloadRequested += _errorControl_ReloadRequested;

            SetupDesigner();
        }

        #region LiteDocumentContent Members

        public override void Save(Stream stream)
        {
            // write byte order mask.
            byte[] bytes = Encoding.UTF8.GetPreamble();
            stream.Write(bytes, 0, bytes.Length); 
            
            // write source.
            string source = _codeWriter.SerializeCode(_namespace, (Component)_designerHost.RootComponent, _designerHost.Container, _includeBaseType);
            bytes = Encoding.UTF8.GetBytes(source);
            stream.Write(bytes, 0, bytes.Length);
        }

        #endregion

        #region IClipboardHandler Members

        public bool IsCutEnabled
        {
            get { return true; }
        }

        public bool IsCopyEnabled
        {
            get { return true; }
        }

        public bool IsPasteEnabled
        {
            get { return true; }
        }

        public void Cut()
        {
            _surface.GetService<IMenuCommandService>().GlobalInvoke(StandardCommands.Cut);
        }

        public void Copy()
        {
            _surface.GetService<IMenuCommandService>().GlobalInvoke(StandardCommands.Copy);
        }

        public void Paste()
        {
            _surface.GetService<IMenuCommandService>().GlobalInvoke(StandardCommands.Paste);
        }

        #endregion

        #region IPropertyContainerProvider Members

        public PropertyContainer PropertyContainer
        {
            get { return _propertyContainer; }
        }

        #endregion

        #region IToolboxServiceProvider Members

        public ToolboxService ToolboxService
        {
            get { return _toolboxService; }
        }

        #endregion

        private void SetupDesigner()
        {
            try
            {
                var snapshot = _language.CreateSourceSnapshot(AssociatedFile.GetContentsAsString()) as NetSourceSnapshot;
                _includeBaseType = !string.IsNullOrEmpty(snapshot.Types[0].ValueType);
                _namespace = snapshot.Namespaces.Length == 0 ? string.Empty : snapshot.Namespaces[0].Name;

                _serviceContainer.AddService(typeof(INameCreationService), new NameCreationService());
                _surface = _codeReader.Deserialize(_surfaceManager, _serviceContainer, AssociatedFile);

                _viewControl = _surface.View as Control;
                _viewControl.Dock = DockStyle.Fill;
                this.Control = _viewControl;

                _designerHost = _surface.GetService<IDesignerHost>();

                var selectionService = _surface.GetService<ISelectionService>();
                selectionService.SelectionChanged += selectionService_SelectionChanged;

                var changeService = _designerHost.GetService<IComponentChangeService>();
                changeService.OnComponentChanging(_designerHost.RootComponent, TypeDescriptor.GetProperties(_designerHost.RootComponent)["Controls"]);
                changeService.ComponentChanging += changeService_ComponentChanging;
                changeService.ComponentAdded += changeService_ComponentsChanged;
                changeService.ComponentRemoved += changeService_ComponentsChanged;

                _serviceContainer.AddService(typeof(IMenuCommandService), new Services.MenuCommandService(_extensionHost, _designerHost));
                _serviceContainer.AddService(typeof(IEventBindingService), new Services.EventBindingService(_designerHost));
                _toolboxService = new Services.FormsToolBoxService((ParentExtension as FormsDesignerExtension).ToolBoxBuilder);
                _toolboxService.SelectedItemChanged += toolBoxService_SelectedItemChanged;
                _toolboxService.SelectedItemUsed += toolBoxService_SelectedItemUsed;
                _serviceContainer.AddService(typeof(IToolboxService), _toolboxService);
            }
            catch (BuildException ex)
            {
                _errorControl.SetBuildErrors(ex.Result.Errors);
                this.Control = _errorControl;
            }
            catch (Exception ex)
            {
                _errorControl.SetException(ex);
                this.Control = _errorControl;
            }
        }

        private void ControlManager_AppearanceChanged(object sender, EventArgs e)
        {
            var processor = _extensionHost.ControlManager.GlobalAppearanceMap.Processor;
            processor.ApplyAppearanceOnObject(_viewControl, DefaultAppearanceDefinition.Window);
        }

        private void _errorControl_ReloadRequested(object sender, EventArgs e)
        {
            SetupDesigner();
        }

        private void toolBoxService_SelectedItemUsed(object sender, ToolBoxItemEventArgs e)
        {
            (sender as IToolboxService).SetSelectedToolboxItem(null);
        }

        private void toolBoxService_SelectedItemChanged(object sender, ToolBoxItemEventArgs e)
        {
            if (e.Item == null)
                _viewControl.Cursor = Cursors.Default;
            else
                _viewControl.Cursor = Cursors.Cross;
        }

        private void changeService_ComponentsChanged(object sender, ComponentEventArgs e)
        {
            AssociatedFile.GiveUnsavedData();
        }

        private IComponent AddComponent(Type componentType)
        {
            var component = _designerHost.CreateComponent(componentType, _surface.GetService<INameCreationService>().CreateName(_designerHost.Container, componentType));
            if (component is Control)
            {
                Control control = component as Control;
                control.Parent = _designerHost.RootComponent as Control;
                control.Text = control.Name;
            }
            return component;
        }

        private void selectionService_SelectionChanged(object sender, EventArgs e)
        {
            var selectedComponents = _surface.GetService<ISelectionService>().GetSelectedComponents();
            var components = new object[selectedComponents.Count];
            selectedComponents.CopyTo(components, 0);
            _propertyContainer.SelectedObjects = components;
        }

        private void changeService_ComponentChanging(object sender, ComponentChangingEventArgs e)
        {
            AssociatedFile.GiveUnsavedData();
            _propertyContainer.SelectedObjects = new object[] { e.Component };
        }

        private void AssociatedFile_FilePathChanged(object sender, PathChangedEventArgs e)
        {
            Text = e.NewPath.FileName + e.NewPath.Extension + " [Design]";
        }

    }
}
