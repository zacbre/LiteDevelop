using System;
using System.CodeDom;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace LiteDevelop.Framework.Languages.Net
{
    public static class CodeDomUnitFactory
    {
        private static string[] _standardImports = new string[] { "System", "System.IO", "System.Text", "System.Collections.Generic" };
        public static CodeCompileUnit CreateClassUnit(string @namespace, string className)
        {
            return CreateClassUnit(@namespace, className, null);
        }

        public static CodeCompileUnit CreateClassUnit(string @namespace, string className, CodeTypeReference baseType)
        {
            return CreateClassUnit(@namespace, className, baseType, _standardImports);
        }

        public static CodeCompileUnit CreateClassUnit(string @namespace, string className, CodeTypeReference baseType, params string[] imports)
        {
            var unit = new CodeCompileUnit();

            // let vb users learn good habits by turning option strict on >:D
            unit.UserData.Add("AllowLateBound", false);

            var globalNamespaceUnit = new CodeNamespace();
            foreach (var import in imports)
                globalNamespaceUnit.Imports.Add(new CodeNamespaceImport(import));

            var namespaceUnit = globalNamespaceUnit;

            if (!string.IsNullOrEmpty(@namespace))
            {
                unit.Namespaces.Add(globalNamespaceUnit);
                namespaceUnit = new CodeNamespace(@namespace);
            }

            var classUnit = new CodeTypeDeclaration(@className);
            classUnit.TypeAttributes = TypeAttributes.Public;

            if (baseType != null)
            {
                classUnit.BaseTypes.Add(baseType);
            }

            namespaceUnit.Types.Add(classUnit);
            unit.Namespaces.Add(namespaceUnit);

            return unit;
        }

        public static CodeCompileUnit CreateEntryPointModuleUnit(string @namespace, string className)
        {
            var compileUnit = CreateClassUnit(@namespace, className, null);
            var method = new CodeMemberMethod()
            {
                Name = "Main",
                ReturnType = new CodeTypeReference(typeof(void)),
                Attributes = MemberAttributes.Public | MemberAttributes.Static,
            };
            compileUnit.Namespaces[1].Types[0].Members.Add(method);
            return compileUnit;
        }

        public static CodeCompileUnit CreateFormClass(string @namespace, string className)
        {
            var compileUnit = CreateClassUnit(@namespace, className,  new CodeTypeReference(typeof(Form)), _standardImports.MergeWith(new string[] { "System.Windows.Forms" }));
            var method = new CodeConstructor()
            {
                Attributes = MemberAttributes.Public
            };

            method.Statements.AddRange(new CodeStatement[] 
            {
                new CodeCommentStatement("Required for the forms designer"),
                new CodeExpressionStatement(new CodeMethodInvokeExpression(
                    new CodeThisReferenceExpression(),
                    "InitializeComponent")),
            });
            var type = compileUnit.Namespaces[1].Types[0];
            type.Members.Add(method);
            type.IsPartial = true;
            return compileUnit;
        }

        public static CodeCompileUnit CreateFormDesignerClass(string @namespace, string className)
        {
            var compileUnit = CreateClassUnit(@namespace, className);
            var method = new CodeMemberMethod()
            {
                Name = "InitializeComponent",
                Attributes = MemberAttributes.Private,
            };

            method.Statements.AddRange(new CodeStatement[] 
            {
                new CodeAssignStatement(
                    new CodePropertyReferenceExpression(
                        new CodeThisReferenceExpression(), "Text"),
                        new CodePrimitiveExpression(className)),
                new CodeAssignStatement(
                    new CodePropertyReferenceExpression(
                        new CodeThisReferenceExpression(), "Name"),
                        new CodePrimitiveExpression(className)),
            });
            var type = compileUnit.Namespaces[1].Types[0];
            type.Members.Add(method);
            type.IsPartial = true;
            return compileUnit;
        }
    }
}
