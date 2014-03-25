using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace LiteDevelop.Framework.Languages.Net
{
    /// <summary>
    /// Generates initialization code for objects by parsing their properties and serialize them to code.
    /// </summary>
    public class CodeDomPropertySerializer
    {
        public CodeDomPropertySerializer()
        {
            TemporaryVariables = new List<string>();
        }

        /// <summary>
        /// Gets a list of temporary variables that are currently being used.
        /// </summary>
        public List<string> TemporaryVariables { get; private set; }

        /// <summary>
        /// Serializes properties of a certain component.
        /// </summary>
        /// <param name="componentHolder">The parent expression that holds the instance.</param>
        /// <param name="instance">The component to take away the values from.</param>
        /// <returns>A collection of code statements initiating the properties of the given object to their values.</returns>
        public CodeStatementCollection SerializeProperties(CodeExpression componentHolder, object instance)
        {
            CodeStatementCollection statements = new CodeStatementCollection();

            // Iterate through properties and add property + value to statements.
            foreach (PropertyDescriptor property in GetMustSerializeProperties(instance))
            {
                statements.AddRange(SerializeProperty(componentHolder, instance, property));
            }

            return statements;
        }

        /// <summary>
        /// Serializes a specific property to code.
        /// </summary>
        /// <param name="instanceHolder">The parent expression that holds the instance.</param>
        /// <param name="instance">The instance to take away the values from.</param>
        /// <param name="property">The property to serialize.</param>
        /// <returns>A collection of code statements initiating the given property of the given object to its value.</returns>
        public CodeStatementCollection SerializeProperty(CodeExpression instanceHolder, object instance, PropertyDescriptor property)
        {
            CodeStatementCollection statements = new CodeStatementCollection();

            var propertyExpression = new CodePropertyReferenceExpression(
                    instanceHolder,
                    property.Name);

            object propertyValue = property.GetValue(instance);
            if (property.PropertyType.HasInterface(typeof(ICollection)))
            {
                // If collection, create property.Add(...) statements.
                statements.AddRange(CreateCollectionInitialization(propertyExpression, instance, property));
            }
            else if (CodeDomTypeFormatter.IsFormattableWithAssignment(property.PropertyType)) 
            {
                // else, create normal property = value statement.
                statements.Add(CreatePropertyAssignmentExpression(propertyExpression, instance, property));
            }

            if (!CodeDomTypeFormatter.IsFormattableWithAssignment(property.PropertyType))
            {
                // serialize child properties.
                if (property.PropertyType.IsValueType)
                {
                    // value types uses a temp var.to access properties.
                    statements.AddRange(CreateValueTypeInitialization(propertyExpression, instance, property));
                }
                else
                {
                    // access properties directly.
                    statements.AddRange(SerializeProperties(propertyExpression, propertyValue));
                }
            }

            return statements;
        }

        private CodeAssignStatement CreatePropertyAssignmentExpression(CodeExpression propertyExpression, object parentInstance, PropertyDescriptor property)
        {
            CodeExpression valueExpression = CodeDomTypeFormatter.FormatValue(property.GetValue(parentInstance));
            return new CodeAssignStatement(propertyExpression, valueExpression);
        }

        private CodeStatementCollection CreateCollectionInitialization(CodeExpression propertyExpression, object parentInstance, PropertyDescriptor property)
        {
            // get collection and define expressions.
            ICollection collection = property.GetValue(parentInstance) as ICollection;
            CodeStatement[] expressions = new CodeStatement[collection.Count];
            
            int counter = 0;
            int maximum = expressions.Length;

            foreach (object value in collection)
            {
                // format value.
                var formattedValue = CodeDomTypeFormatter.FormatValue(value);

                if (formattedValue == null)
                {
                    // remove from list.
                    maximum--;
                }
                else
                {
                    // create property.Add(value) statement.
                    expressions[counter] = CodeDomTypeFormatter.CreateAddToCollectionExpression(propertyExpression, formattedValue);
                    counter++;
                }
            }

            if (maximum != expressions.Length)
                Array.Resize(ref expressions, maximum);

            return new CodeStatementCollection(expressions);
        }

        private CodeStatementCollection CreateValueTypeInitialization(CodeExpression propertyExpression, object parentInstance, PropertyDescriptor property)
        {
            // value types (structs) cannot be modified from a property, so we need to create a temp var.
            string tempVariable = CreateNewVariable(property.PropertyType);
            object propertyValue = property.GetValue(parentInstance);

            CodeStatementCollection statements = new CodeStatementCollection();

            // declare temp var.
            statements.Add(new CodeVariableDeclarationStatement(property.PropertyType, tempVariable, CodeDomTypeFormatter.FormatValue(propertyValue)));

            CodeVariableReferenceExpression varReference = new CodeVariableReferenceExpression(tempVariable);
            
            // serialize properties of var.
            statements.AddRange(SerializeProperties(varReference, propertyValue));

            // set property to var.
            statements.Add(new CodeAssignStatement(propertyExpression, varReference));

            return statements;
        }

        private string CreateNewVariable(Type type)
        {
            // generate base name by getting type name and make first char to lower.
            string baseName = char.ToLower(type.Name[0]) + type.Name.Substring(1);

            // add number in order to prevent variables with same name.
            int counter = 1;
            while (TemporaryVariables.Contains(baseName + counter.ToString()))
                counter++;

            // return variable name.
            string varName = baseName + counter.ToString();
            TemporaryVariables.Add(varName);
            return varName;
        }

        private PropertyDescriptor[] GetMustSerializeProperties(object instance)
        {
            List<PropertyDescriptor> properties = new List<PropertyDescriptor>();
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(instance))
            {
                if (!property.DesignTimeOnly && property.SerializationVisibility != DesignerSerializationVisibility.Hidden && property.ShouldSerializeValue(instance))
                    properties.Add(property);
            }
            return properties.ToArray();
        }
    }
}
