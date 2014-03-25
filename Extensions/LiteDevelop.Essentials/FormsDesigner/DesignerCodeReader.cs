using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using LiteDevelop.Framework.Extensions;
using LiteDevelop.Framework.FileSystem;
using LiteDevelop.Framework.Languages.Net;

namespace LiteDevelop.Essentials.FormsDesigner
{
    // TODO: 
    // - support of components from third-party dlls
    // - search parent control not by name.

    /// <summary>
    /// Deserializes Visual Basic or C# source code to an object for the Windows Forms Designer.
    /// </summary>
    public class DesignerCodeReader
    {
        private ILiteExtensionHost _extensionHost;
        private static Regex _namespaceRegex = new Regex(@"(?<start>(n|N)amespace\s+[\w+\.]+(\s+{)?)\s+(?<end>(End Namespace|}))");

        public DesignerCodeReader(ILiteExtensionHost extensionHost, NetLanguageDescriptor language)
        {
            _extensionHost = extensionHost;
            Language = language;
        }

        /// <summary>
        /// The language the serializer should use. 
        /// </summary>
        public NetLanguageDescriptor Language { get; set; }
        
        /// <summary>
        /// Parses a source code and creates a new design surface.
        /// </summary>
        /// <param name="serviceContainer"></param>
        /// <param name="surfaceManager"></param>
        /// <param name="file">The source file to deserialize.</param>
        /// <returns></returns>
        public DesignSurface Deserialize(DesignSurfaceManager surfaceManager, IServiceContainer serviceContainer, OpenedFile file)
        {
            DesignSurface surface = surfaceManager.CreateDesignSurface(serviceContainer);
            IDesignerHost designerHost = surface.GetService(typeof(IDesignerHost)) as IDesignerHost;
            
            Type componentType = CompileTypeFromFile(file);

            // load base type.
            surface.BeginLoad(componentType.BaseType);
            
            // get instance to copy components and properties from.
            Control instance = Activator.CreateInstance(componentType) as Control;

            // add components
            var components = CreateComponents(componentType, instance, designerHost);

            InitializeComponents(components, designerHost);

            Control rootControl = designerHost.RootComponent as Control;

            Control parent = rootControl.Parent;
            ISite site = rootControl.Site;
 
            // copy instance properties to root control.
            CopyProperties(instance, designerHost.RootComponent);

            rootControl.AllowDrop = true;
            rootControl.Parent = parent;
            rootControl.Visible = true;
            rootControl.Site = site;
            designerHost.RootComponent.Site.Name = instance.Name;
            return surface;
        }

        private Type CompileTypeFromFile(OpenedFile file)
        {
            string sourceCode = file.GetContentsAsString();

            var projectFile = _extensionHost.CurrentSolution.FindProjectFile(file.FilePath);

            if (projectFile == null)
                throw new FileNotFoundException("Cannot find project file " + file.FilePath.FullPath);

            CompilerParameters parameters = new CompilerParameters()
            {
                GenerateInMemory = true,
                GenerateExecutable = false,
            };
            
            // standard references.
            parameters.ReferencedAssemblies.AddRange(new string[] {"mscorlib.dll", "System.dll", "System.Drawing.dll", "System.Windows.Forms.dll"});

            CodeDomProvider provider = Language.CodeProvider;

            // validate source
            var sourceSnapshot = Language.CreateSourceSnapshot(sourceCode) as NetSourceSnapshot;
            if (sourceSnapshot.Types.Length != 1)
            {
                throw new ArgumentException("Source must contain exactly one type.");
            }

            string @namespace;

            if (sourceSnapshot.Namespaces.Length == 0)
            {
                // TODO: add temp namespace ....
                throw new NotImplementedException("Cannot handle classes without a namespace yet.");
            }
            else
            {
                @namespace = sourceSnapshot.Namespaces[0].Name;
            }

            // create stub class
            var writer = new StringWriter();
            var stubClass = new CodeTypeDeclaration(sourceSnapshot.Types[0].Name) { IsPartial = true };
            stubClass.Members.Add(GenerateConstructor());

            // check if snapshot class doesn't have a base type.
            if (string.IsNullOrEmpty(sourceSnapshot.Types[0].ValueType))
            {
                // try to find in dependencies
                foreach (var dependency in file.Dependencies)
                    if (Path.GetExtension(dependency) == file.FilePath.Extension)
                    {
                        var dependentFile = _extensionHost.FileService.OpenFile(projectFile.FilePath.ParentDirectory.Combine(dependency));
                        var dependencySnapshot = Language.CreateSourceSnapshot(dependentFile.GetContentsAsString()) as NetSourceSnapshot;
                        if (dependencySnapshot.Types[0].Name == sourceSnapshot.Types[0].Name && !string.IsNullOrEmpty(dependencySnapshot.Types[0].ValueType))
                        {
                            var baseType = dependencySnapshot.GetTypeByName(dependencySnapshot.Types[0].ValueType);
                            if (baseType != null)
                            {
                                stubClass.BaseTypes.Add(baseType);
                                break;
                            }
                        }
                    }
            }

            // write stub class
            var stubNamespace = new CodeNamespace(@namespace);
            stubNamespace.Types.Add(stubClass);
            provider.GenerateCodeFromNamespace(stubNamespace, writer, new CodeGeneratorOptions());
                       
            
            // compile.
            CompilerResults results = provider.CompileAssemblyFromSource(parameters, sourceCode, writer.GetStringBuilder().ToString());
    
            if (results.Errors.Count == 0)
            {
                // find target type.
                foreach (Type type in results.CompiledAssembly.GetTypes())
                    if (type.IsBasedOn(typeof(Component)))
                        return type;

                throw new Exception("The type based on " + typeof(Component).FullName + " was not found.");
            }
            else
            {
                
                throw new BuildException("The compiler did not succeed in compiling the source.", BuildResult.FromCompilerResults(results));
            }
            
        }

        private Dictionary<Component, Component> CreateComponents(Type componentType, object instance, IDesignerHost designerHost)
        {
            Dictionary<Component, Component> components = new Dictionary<Component, Component>();
            // get each field of type.
            foreach (FieldInfo field in componentType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                // check if it is defined in the type that is being compiled, and not a field in some base class.
                if (field.DeclaringType.TypeHandle.Value == componentType.TypeHandle.Value) 
                {
                    if (field.FieldType.HasConstructor())
                    {
                        object obj = field.GetValue(instance);

                        // create the component.
                        Component component = designerHost.CreateComponent(field.FieldType, field.Name) as Component;
                        components.Add(field.GetValue(instance) as Component, component);
                    }
                }
            }
            return components;
        }

        private void InitializeComponents(Dictionary<Component, Component> components, IDesignerHost designerHost)
        {
            foreach (var componentPair in components)
            {
                // copy significant properties.
                CopyProperties(componentPair.Key, componentPair.Value);

                if (componentPair.Key is Control)
                {
                    Control keyControl = componentPair.Key as Control;
                    Control valueControl = componentPair.Value as Control;

                    if (keyControl.Parent != null)
                    {
                        // set parent control.
                        if (keyControl.Parent.Parent == null)
                            valueControl.Parent = designerHost.RootComponent as Control;
                        else
                            valueControl.Parent = (GetComponentByName(components, keyControl.Parent.Name) as Control);
                    }
                }

            }
        }

        private Component GetComponentByName(Dictionary<Component, Component> components, string name)
        {
            return components.First(x => x.Value.Site.Name == name).Value;
        }

        private void CopyProperties(object original, object target)
        {
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(original))
            {
                if (property.SerializationVisibility != DesignerSerializationVisibility.Hidden && property.ShouldSerializeValue(original))
                    property.SetValue(target, property.GetValue(original));
            }
        }

        private CodeConstructor GenerateConstructor()
        {
            CodeConstructor constructor = new CodeConstructor();
            constructor.Attributes = MemberAttributes.Public;
            constructor.Statements.Add(new CodeMethodInvokeExpression(
                new CodeMethodReferenceExpression(
                    new CodeThisReferenceExpression(),
                    "InitializeComponent")));
            return constructor;
        }

        // temp for debugging purposes.
        private void DumpProperties(object instance, string path)
        {
            StringBuilder builder = new StringBuilder();
            foreach (PropertyInfo property in instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
               
                builder.Append(property.Name.PadRight(30));
                try
                {
                    object value = property.GetValue(instance, null);
                    if (value == null)
                        builder.Append("null");
                    else
                        builder.Append(value.ToString());
                }
                catch (Exception ex)
                {
                    builder.Append(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                }
                builder.AppendLine();
            }

            File.WriteAllText(path, builder.ToString());
        }
    }
}
