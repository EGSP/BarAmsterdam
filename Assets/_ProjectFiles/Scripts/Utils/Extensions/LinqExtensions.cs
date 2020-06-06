using System;
using System.Collections.Generic;
using System.Linq;

namespace Gasanov.Extensions
{
    public static class LinqExtensions
    {
        /// <summary>
        /// Возвращает индекс наибольшего элемента.
        /// Если в коллекции нет элементов, то будет возвращено -1
        /// </summary>
        public static int MaxIndex<T>(this IEnumerable<T> sequence, Func<T,IComparable> selector)
        {
            var maxIndex = -1;
            var maxValue = default(T);

            var index = 0;
            foreach (var value in sequence)
            {
                if (selector(value).CompareTo(maxValue) > 0 || maxIndex == -1)
                {
                    maxIndex = index;
                    maxValue = value;
                }
                index++;
            }
            return maxIndex;
        }
        
        /// <summary>
        /// Возвращает индекс наименьшего элемента.
        /// Если в коллекции нет элементов, то будет возвращено -1
        /// </summary>
        public static int MinIndex<T>(this IEnumerable<T> sequence, Func<T,IComparable> selector)
        {
            // Изменить Func - сделать сравнение в Func
            
            var minIndex = -1;
            var minValue = default(T);

            var index = 0;
            foreach (var value in sequence)
            {
                if (minValue == null)
                {
                    minValue = value;
                    minIndex = index;
                }
                 else if (selector(value).CompareTo(selector(minValue)) < 0 || minIndex == -1)
                {
                    minIndex = index;
                    minValue = value;
                }
                index++;
            }
            return minIndex;
        }

        /// <summary>
        /// Возвращает случайный элемент
        /// </summary>
        public static T Random<T>(this IEnumerable<T> collection)
        {
            var randomIndex = new System.Random().Next(0,collection.Count());

            return collection.ElementAt(randomIndex);
        }
    }
}