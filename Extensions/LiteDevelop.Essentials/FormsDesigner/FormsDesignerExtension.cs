using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Windows.Forms;
using LiteDevelop.Essentials.CodeEditor;
using LiteDevelop.Essentials.FormsDesigner.Gui;
using LiteDevelop.Essentials.FormsDesigner.Services;
using LiteDevelop.Framework;
using LiteDevelop.Framework.Extensions;
using LiteDevelop.Framework.FileSystem;
using LiteDevelop.Framework.FileSystem.Net;
using LiteDevelop.Framework.Gui;
using LiteDevelop.Framework.Languages;
using LiteDevelop.Framework.Languages.Net;

namespace LiteDevelop.Essentials.FormsDesigner
{
    public class FormsDesignerExtension : LiteExtension, IFileHandler
    {
        private Dictionary<OpenedFile, FormsDesignerContent> _formEditors;

        public FormsDesignerExtension()
        {
            _formEditors = new Dictionary<OpenedFile, FormsDesignerContent>();
            ToolBoxBuilder = new FormsToolBoxBuilder();
            DesignerSurfaceManager = new DesignSurfaceManager();
        }

        #region LiteExtension Members

        public override string Name
        {
            get { return "Windows Forms Designer"; }
        }

        public override string Description
        {
            get { return "Default graphical user interface designer for windows applications."; }
        }

        public override string Author
        {
            get { return "Jerre S."; }
        }

        public override Version Version
        {
            get { return new Version(0, 9, 0, 0); }
        }

        public override string Copyright
        {
            get { return "Copyright © Jerre S. 2014"; }
        }

        public override void Initialize(ILiteExtensionHost extensionHost)
        {
            ExtensionHost = extensionHost;
            SetupFileTemplates();
            SetupProjectTemplates();
        }

        public override void Dispose()
        {
            foreach (var keyValuePair in _formEditors)
                keyValuePair.Value.Close(true);

            DesignerSurfaceManager.Dispose();
            base.Dispose();
        }

        #endregion
        
        #region IFileHandler Members

        public bool CanOpenFile(FilePath filePath)
        {
            if (ExtensionHost.CurrentSolution != null)
            {
                var projectFile = ExtensionHost.CurrentSolution.FindProjectFile(filePath);
                if (projectFile != null)
                {
                    return projectFile.Dependencies.Count != 0;
                }
            }
            return false;
        }

        public void OpenFile(OpenedFile file)
        {
            FormsDesignerContent tab;
            if (!_formEditors.TryGetValue(file, out tab))
            {
                tab = new FormsDesignerContent(this, file);
                tab.Closed += tab_Closed;
                _formEditors.Add(file, tab);
                ExtensionHost.ControlManager.OpenDocumentContents.Add(tab);
            }

            ExtensionHost.ControlManager.SelectedDocumentContent = tab;
           
        }

        #endregion

        public ILiteExtensionHost ExtensionHost
        {
            get;
            private set;
        }
        public DesignSurfaceManager DesignerSurfaceManager
        {
            get;
            private set;
        }

        public FormsToolBoxBuilder ToolBoxBuilder
        {
            get;
            private set;
        }

        private void SetupFileTemplates()
        {
            var formDesignerClassTemplate = new NetAstFileTemplate(
                    "%file%.Designer",
                    "%file%",
                    null,
                    this,
                    CodeDomUnitFactory.CreateFormDesignerClass("%folder%", "%file%"));

            var formClassTemplate = new NetAstFileTemplate(
                    "%file%",
                    null,
                    null,
                    CodeEditorExtension.Instance,
                    CodeDomUnitFactory.CreateFormClass("%folder%", "%file%"));

            var formTemplate = new NetFormTemplate(
                "Form", 
                Properties.Resources.window_new,
                this, 
                formDesignerClassTemplate, formClassTemplate);

            LanguageDescriptor.GetLanguage<CSharpLanguage>().Templates.Add(formTemplate);
            LanguageDescriptor.GetLanguage<VisualBasicLanguage>().Templates.Add(formTemplate);
        }

        private void SetupProjectTemplates()
        {
            var programClassTemplate = new NetAstFileTemplate("Program", "Program", null, CodeEditorExtension.Instance, CodeDomUnitFactory.CreateEntryPointModuleUnit("%folder%", "Program"));

            var method = programClassTemplate.CompileUnit.Namespaces[1].Types[0].Members[0] as CodeMemberMethod;
            method.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(STAThreadAttribute))));
            method.Statements.AddRange(new CodeStatement[]
            {
                new CodeExpressionStatement(
                    new CodeMethodInvokeExpression(
                        new CodeMethodReferenceExpression(
                            new CodeTypeReferenceExpression(typeof(Application)),
                            "EnableVisualStyles"))),
                new CodeExpressionStatement(
                    new CodeMethodInvokeExpression(
                        new CodeMethodReferenceExpression(
                            new CodeTypeReferenceExpression(typeof(Application)),
                            "SetCompatibleTextRenderingDefault"),
                        new CodePrimitiveExpression(false))),
                new CodeExpressionStatement(
                    new CodeMethodInvokeExpression(
                        new CodeMethodReferenceExpression(
                            new CodeTypeReferenceExpression(typeof(Application)),
                            "Run"),
                        new CodeObjectCreateExpression("Form1"))),
            });

            var formClassTemplate = new NetAstFileTemplate(
                    "Form1",
                    "Form1",
                    null,
                    CodeEditorExtension.Instance,
                    CodeDomUnitFactory.CreateFormClass("%folder%", "Form1")); 
            var formDesignerClassTemplate = new NetAstFileTemplate(
                    "Form1.Designer",
                    "Form1",
                    null,
                    this,
                    CodeDomUnitFactory.CreateFormDesignerClass("%folder%", "Form1"));

            var formTemplate = new NetFormTemplate(
                "Form1",
                Properties.Resources.window_new,
                this,
                formDesignerClassTemplate, formClassTemplate);

            var csProject = new NetProjectTemplate(
                "Windows Forms Application",
                Properties.Resources.window_list,
                LanguageDescriptor.GetLanguage<CSharpLanguage>(),
                SubSystem.Windows,
                programClassTemplate,
                formTemplate);

            csProject.References.AddRange(new string[] { "System.Drawing.dll", "System.Windows.Forms.dll" });
            
            LanguageDescriptor.GetLanguage<CSharpLanguage>().Templates.Add(csProject);

            var vbProject = new NetProjectTemplate(
                "Windows Forms Application",
                Properties.Resources.window_list,
                LanguageDescriptor.GetLanguage<VisualBasicLanguage>(),
                SubSystem.Windows,
                programClassTemplate, 
                formTemplate);

            vbProject.References.AddRange(new string[] { "System.Drawing.dll", "System.Windows.Forms.dll" });
            
            LanguageDescriptor.GetLanguage<VisualBasicLanguage>().Templates.Add(vbProject);
        }

        private void tab_Closed(object sender, FormClosedEventArgs e)
        {
            _formEditors.Remove((sender as LiteDocumentContent).AssociatedFile);
        }

    }
}
