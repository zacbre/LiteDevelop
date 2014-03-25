using System;
using System.Collections.Generic;
using System.Linq;

namespace LiteDevelop.Framework
{
    /// <summary>
    /// Helper class for array comparisons.
    /// </summary>
    public static class ArrayComparer
    {
        /// <summary>
        /// Compares one array with another and determines which elements are added to and removed from the first array.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the array.</typeparam>
        /// <param name="arrayA">The source array.</param>
        /// <param name="arrayB">The array to compare the source with.</param>
        /// <param name="comparer">The comparer instance to use for determining whether the elements are equal to each other or not.</param>
        /// <returns>An instance of an <see cref="LiteDevelop.Framework.ArrayComparisonResult{T}"/> holding the comparison result.</returns>
        public static ArrayComparisonResult<T> CompareArrays<T>(T[] arrayA, T[] arrayB, IEqualityComparer<T> comparer)
        {
            var elementsAdded = new List<T>();
            for (int i = 0; i < arrayB.Length; i++)
            {
                if (!arrayA.Contains(arrayB[i], comparer))
                {
                    elementsAdded.Add(arrayB[i]);
                }
            }

            var elementsMissing = new List<T>();
            for (int i = 0; i < arrayA.Length; i++)
            {
                if (!arrayB.Contains(arrayA[i], comparer))
                {
                    elementsMissing.Add(arrayA[i]);
                }
            }

            return new ArrayComparisonResult<T>(elementsAdded.ToArray(), elementsMissing.ToArray());
        }


    }

    /// <summary>
    /// Represents a result of an array comparison.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the arrays that are compared to each other.</typeparam>
    public class ArrayComparisonResult<T>
    {
        public ArrayComparisonResult(T[] elementsAdded, T[] elementsMissing)
        {
            ElementsAdded = elementsAdded;
            ElementsMissing = elementsMissing;
        }

        /// <summary>
        /// Gets a collection of elements being added to the second array relative to the first one.
        /// </summary>
        public T[] ElementsAdded { get; private set; }

        /// <summary>
        /// Gets a collection of elements being removed to the second array relative to the first one.
        /// </summary>
        public T[] ElementsMissing { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the arrays are equal or not.
        /// </summary>
        public bool ArraysAreEqual
        {
            get { return ElementsMissing.Length == 0 && ElementsAdded.Length == 0; }
        }
    }
}
