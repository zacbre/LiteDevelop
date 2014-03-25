using System;
using System.CodeDom;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace LiteDevelop.Framework.Languages.Net
{
    // TODO: 
    // - support more standard constructors.
    // - specify properties of custom types.

    /// <summary>
    /// Provides functions to format values to instances of the <see cref="System.CodeDom.CodeExpression" /> class to use in source generators.
    /// </summary>
    public class CodeDomTypeFormatter
    {
        /// <summary>
        /// Formats an object to a code expression.
        /// </summary>
        /// <param name="value">The value to be formatted</param>
        /// <returns>A code expression representing the given value.</returns>
        public static CodeExpression FormatValue(object value)
        {
            // TODO: support more types.

            if (value == null)
                return new CodePrimitiveExpression(null);

            Type valueType = value.GetType();

            if (valueType.IsPrimitive || value is string || value is decimal)
                return new CodePrimitiveExpression(value);
            else if (valueType.IsEnum)
                return FormatEnumValue(value as Enum);
            else if (value is Color)
                return FormatColor((Color)value);
            else if (value is Control || value is IComponent)
            {
                string name = TypeDescriptor.GetComponentName(value) ?? ((value is Control) ? (value as Control).Name : null);

                if (string.IsNullOrEmpty(name))
                    return null;

                return new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), TypeDescriptor.GetComponentName(value));
            }
            else
                return CreateCodeNewObjectExpression(value);
        }

        /// <summary>
        /// Formats an enum value to a code expression.
        /// </summary>
        /// <param name="value">The value to be formatted</param>
        /// <returns>A code expression representing one single enumeration value, or a collection of two or more values concatenated with the <c>Or</c> operator.</returns>
        public static CodeExpression FormatEnumValue(Enum value)
        {
            Type valueType = value.GetType();
            bool isFlags = valueType.HasCustomAttribute(typeof(FlagsAttribute));
            
            long enumValue = Convert.ToInt64(value);

            if (isFlags && enumValue > 0)
            {
                CodeExpression returnExpression = null;

                foreach (FieldInfo field in valueType.GetFields())
                {
                    if (field.IsLiteral)
                    {
                        long fieldValue = Convert.ToInt64(field.GetValue(null));

                        if ((enumValue & fieldValue) == fieldValue)
                        {
                            CodeFieldReferenceExpression fieldExpression = new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(valueType), field.Name);

                            if (returnExpression == null)
                                returnExpression = fieldExpression;
                            else
                                returnExpression = new CodeBinaryOperatorExpression(returnExpression, CodeBinaryOperatorType.BitwiseOr, fieldExpression);

                            enumValue ^= fieldValue;
                        }
                    }
                }

                if (enumValue == 0)
                    return returnExpression;
                else
                    return new CodeCastExpression(valueType, new CodePrimitiveExpression(Convert.ToInt64(value)));
            }
            else
            {
                foreach (FieldInfo field in valueType.GetFields())
                {
                    if (field.IsLiteral)
                    {
                        long fieldValue = Convert.ToInt64(field.GetValue(null));
                        if (fieldValue == enumValue)
                            return new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(valueType), field.Name);
                    }
                }
            }

            return new CodeCastExpression(valueType, new CodePrimitiveExpression(Convert.ToInt64(value)));
        }

        /// <summary>
        /// Formats a color instance to a code expression.
        /// </summary>
        /// <param name="color">The color to format.</param>
        /// <returns>A code expression referencing to a property defined in the <see cref="System.Drawing.Color"/> class, 
        /// a call to <see cref="System.Drawing.Color.FromKnownColor(KnownColor)"/>, or a call to 
        /// <see cref="System.Drawing.Color.FromArgb(Int32,Int32,Int32)"/>.</returns>
        public static CodeExpression FormatColor(Color color)
        {
            var colorTypeExpression = new CodeTypeReferenceExpression(typeof(Color));

            if (color.IsKnownColor)
                return new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(colorTypeExpression, "FromKnownColor"), 
                    new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(typeof(KnownColor)), color.ToKnownColor().ToString()));
            
            if (color.IsNamedColor)
                return new CodePropertyReferenceExpression(colorTypeExpression, color.Name);
            
            return new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(colorTypeExpression, "FromArgb"),
                new CodePrimitiveExpression(color.A), 
                new CodePrimitiveExpression(color.R), 
                new CodePrimitiveExpression(color.G), 
                new CodePrimitiveExpression(color.B));
        }

        /// <summary>
        /// Creates a code statement that adds a value to the specified collection.
        /// </summary>
        /// <param name="parentExpression">The property that holds this collection.</param>
        /// <param name="value">The value to be added to the collection.</param>
        /// <returns>A code statement which adds a specific element to a collection.</returns>
        public static CodeStatement CreateAddToCollectionExpression(CodeExpression parentExpression, CodeExpression value)
        {
            CodeMethodReferenceExpression methodRefExpression = new CodeMethodReferenceExpression(
                parentExpression,
                "Add");

            CodeMethodInvokeExpression addExpression = new CodeMethodInvokeExpression(
                methodRefExpression, 
                value);

            return new CodeExpressionStatement(addExpression);
        }

        /// <summary>
        /// Creates an expression that creates a new instance of the specified value.
        /// </summary>
        /// <param name="value">The value to be created.</param>
        /// <returns>A code expression creating a new object.</returns>
        public static CodeExpression CreateCodeNewObjectExpression(object value)
        {
            object[] paramValues = new object[0];

            if (value is Point)
            {
                Point point = (Point)value;
                paramValues = new object[] { point.X, point.Y };
            }

            if (value is Size)
            {
                Size size = (Size)value;
                paramValues = new object[] { size.Width, size.Height };
            }

            if (value is Font)
            {
                Font font = (Font)value;
                paramValues = new object[] { font.Name, font.Size, font.Style, font.Unit };
            }

            return CreateConstructor(value.GetType(), paramValues);
        }

        /// <summary>
        /// Determines whether a specific value type can be initiated by one statement.
        /// </summary>
        /// <param name="valueType">The value type to check.</param>
        /// <returns><c>True</c> if it can be initiated by one statement, <c>False</c> otherwise.</returns>
        public static bool IsFormattableWithAssignment(Type valueType)
        {
            return valueType == typeof(string) ||
                valueType == typeof(decimal) ||
                valueType == typeof(Point) || 
                valueType == typeof(Size) || 
                valueType == typeof(Font) || 
                valueType == typeof(Color) ||
                valueType.IsPrimitive || 
                valueType.IsEnum || 
                valueType.IsBasedOn(typeof(Control)) || 
                (!valueType.IsValueType && valueType.HasConstructor());
        }
        
        private static CodeExpression CreateConstructor(Type type, params object[] parameterValues)
        {
            // format parameters.
            CodeExpression[] parameterExpressions = new CodeExpression[parameterValues.Length];
            for (int i = 0; i < parameterExpressions.Length; i++)
                parameterExpressions[i] = FormatValue(parameterValues[i]);

            // create constructor call.
            CodeObjectCreateExpression createObject = new CodeObjectCreateExpression(new CodeTypeReference(type), parameterExpressions);
            return createObject;
        }

    }
}
