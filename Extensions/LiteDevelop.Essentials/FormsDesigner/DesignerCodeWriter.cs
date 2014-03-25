using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.IO;
using System.Linq;
using LiteDevelop.Framework.Languages.Net;

namespace LiteDevelop.Essentials.FormsDesigner
{
    // TODO: 
    // - support of components of other libraries.
    // - finish serialization of code.

    /// <summary>
    /// Serializes designed components into Visual Basic or C#.
    /// </summary>
    public class DesignerCodeWriter
    {
        private CodeDomPropertySerializer propertySerializer = new CodeDomPropertySerializer();

        public DesignerCodeWriter(NetLanguageDescriptor language)
        {
            Language = language;
        }

        /// <summary>
        /// The language the serializer should use. 
        /// </summary>
        public NetLanguageDescriptor Language { get; set; }
        
        /// <summary>
        /// Serializes a component to a source code in the language specified in the <see cref="LiteDevelop.Essentials.FormsDesigner.DesignerCodeWriter.Language"/> property.
        /// </summary>
        /// <param name="namespace">The namespace to create the code in.</param>
        /// <param name="instance">The object to be serialized.</param>
        /// <param name="container">The container object.</param>
        /// <param name="includeBaseType">Include the base type of the instance in the to be generated source.</param>
        /// <returns></returns>
        public string SerializeCode(string @namespace, Component instance, IContainer container, bool includeBaseType)
        {
            // Create new namespace
            CodeNamespace codeNamespace = new CodeNamespace(@namespace);

            // Create new class.
            CodeTypeDeclaration controlType = new CodeTypeDeclaration(instance.Site.Name)
            {
                Attributes = MemberAttributes.Public,
                IsClass = true,
                IsPartial = true, 
            };

            if (includeBaseType)
                controlType.BaseTypes.Add(new CodeTypeReference(instance.GetType()));

            // generate fields for members.
            controlType.Members.AddRange(GenerateMembers(container));

            // generate initializecomponent().
            controlType.Members.Add(GenerateInitializeComponentMethod(instance, container));

            // add class to namespace.
            codeNamespace.Types.Add(controlType);

            string code = string.Empty;

            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    using (IndentedTextWriter codeWriter = new IndentedTextWriter(writer, "    "))
                    {
                        CodeDomProvider provider = Language.CodeProvider;

                        using (provider)
                        {
                            // generate source code.
                            provider.GenerateCodeFromNamespace(codeNamespace, codeWriter,
                                new CodeGeneratorOptions()
                                {
                                    BlankLinesBetweenMembers = true,
                                    BracingStyle = "C", // use new lines on brackets.
                                });
                        }

                    }

                    // write data to stream.
                    writer.Flush();

                    // read stream contents to get code.
                    stream.Position = 0;
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        code = reader.ReadToEnd();
                    }
                }

            }

            propertySerializer.TemporaryVariables.Clear();

            return code;
        }

        private CodeMemberField[] GenerateMembers(IContainer container)
        {
            CodeMemberField[] fields = new CodeMemberField[container.Components.Count - 1];

            for (int i = container.Components.Count - 1; i > 0 ; i--)
            {
                fields[i - 1] = new CodeMemberField(container.Components[i].GetType(), container.Components[i].Site.Name);

                if (container.Components[i] is IContainer)
                    GenerateMembers(container.Components[i] as IContainer);
            }

            return fields;
        }

        private CodeMemberMethod GenerateInitializeComponentMethod(Component instance, IContainer container)
        {
            // Define new method.
            CodeMemberMethod initializeComponentMethod = new CodeMemberMethod()
            {
                Name = "InitializeComponent",
                ReturnType = new CodeTypeReference(typeof(void)),
                Attributes = MemberAttributes.Private,
            };

            // initialize new components, excluding the root component.
            for (int i = container.Components.Count - 1; i > 0; i--)
            {
                Component component = container.Components[i] as Component;
                initializeComponentMethod.Statements.Add(new CodeAssignStatement(
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(),
                        container.Components[i].Site.Name),
                    CodeDomTypeFormatter.CreateCodeNewObjectExpression(component)));
            }

            // inverted order because the root must be at the very bottom.
            for (int i = container.Components.Count - 1; i >= 0;i--)
            {
                Component component = container.Components[i] as Component;
                CodeExpression componentExpression;
                if (i == 0)
                    componentExpression = new CodeThisReferenceExpression();
                else
                    componentExpression = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), component.Site.Name);

                // add comment indicating the next component initialization starts:
                initializeComponentMethod.Statements.Add(new CodeCommentStatement(new CodeComment(string.Format(" \r\n {0}\r\n ", component.Site.Name))));

                // serialize properties of the component.
                initializeComponentMethod.Statements.AddRange(propertySerializer.SerializeProperties(componentExpression, component));
            }

            return initializeComponentMethod;
        }


    }
}
