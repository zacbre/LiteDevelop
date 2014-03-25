using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace LiteDevelop.Framework
{
    public static class Constants
    {
        public static readonly string AppDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LiteDevelop");
        public static readonly string ExtensionSettingsDirectory = Path.Combine(AppDataDirectory, "Settings");
    }

    internal static class RegexUtilities
    {
        public static string BuildRegexAlternativeRange(string[] input)
        {
            return string.Format("({0})", string.Join("|", input));
        }
    }

    internal static class TypeExtensions
    {

        public static T[] MergeWith<T>(this T[] array1, T[] array2)
        {
            if (array1 == null || array1.Length == 0)
                return array2;
            if (array2 == null || array2.Length == 0)
                return array1;

            T[] newArray = new T[array1.Length + array2.Length];
            Array.Copy(array1, 0, newArray, 0, array1.Length);
            Array.Copy(array2, 0, newArray, array1.Length, array2.Length);
            return newArray;
        }

        public static bool IsBasedOn(this Type type, Type baseType)
        {
            do
            {
                if (type == baseType)
                    return true;
                type = type.BaseType;
            } while (type != null);

            return false;
        }

        public static bool HasInterface(this Type type, Type @interface)
        {
            Type[] interfaces = type.GetInterfaces();
            foreach (Type i in interfaces)
                if (i == @interface)
                    return true;
            return false;
        }

        public static bool HasCustomAttribute(this Type type, Type attribute)
        {
            return type.GetCustomAttributesData().FirstOrDefault(x => x.Constructor.DeclaringType == attribute) != null;
        }

        public static bool HasConstructor(this Type type, params Type[] arguments)
        {
            foreach (ConstructorInfo constructor in type.GetConstructors())
            {
                ParameterInfo[] parameters = constructor.GetParameters();
                bool mismatch = parameters.Length != arguments.Length;
                if (!mismatch)
                {
                    for (int i = 0; i < parameters.Length; i++)
                        if (parameters[i].ParameterType.TypeHandle.Value != arguments[i].TypeHandle.Value)
                        {
                            mismatch = true;
                            break;
                        }
                }

                if (!mismatch)
                    return true;
            }
            return false;
        }

    }
}
