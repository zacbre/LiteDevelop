using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FastColoredTextBoxNS;

namespace LiteDevelop.Essentials
{
    public static class Globals
    {

        public static T GetService<T>(this IServiceProvider provider) where T : class
        {
            return provider.GetService(typeof(T)) as T;
        }

        public static Range GetPreviousFragment(Range fragment, string pattern)
        {
            Range selectionRange = null;
            if (fragment.Start.iChar > 0)
            {
                selectionRange = fragment.tb.GetRange(new Place(
                    fragment.Start.iChar - 1,
                    fragment.Start.iLine),
                    fragment.Start);
            }
            else if (fragment.Start.iLine > 0)
            {
                selectionRange = fragment.tb.GetRange(new Place(
                    fragment.tb.Lines[fragment.Start.iLine - 1].Length,
                    fragment.Start.iLine - 1),
                    fragment.Start);
            }

            if (selectionRange != null)
                return selectionRange.GetFragment(pattern);

            return null;
        }

        public static IEnumerator<Type> GetTypesInNamespace(this Assembly assembly, string @namespace)
        {
            return GetTypesInNamespace(assembly.GetTypes(), @namespace);
        }

        public static IEnumerator<Type> GetTypesInNamespace(this Type[] types, string @namespace)
        {
            foreach (var type in types)
                if (type.IsPublic && type.Namespace == @namespace)
                    yield return type;
        }

        public static IEnumerator<T> CombineWith<T>(this IEnumerator<T> enumerator1, params IEnumerator<T>[] enumerators)
        {
            foreach (var enumerator in (new IEnumerator<T>[] { enumerator1 }.MergeWith(enumerators)))
            {
                while (enumerator.MoveNext())
                    yield return enumerator.Current;
            }
        }

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

        public static IEnumerator<T> GetSortedEnumerator<T>(this IEnumerator<T> enumerator, Comparison<T> comparison)
        {
            List<T> list = new List<T>();

            while (enumerator.MoveNext())
                list.Add(enumerator.Current);

            list.Sort(comparison);

            return list.GetEnumerator();
        }
    }
}
