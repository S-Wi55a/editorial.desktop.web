using System;
using System.Collections.Generic;
using System.Linq;

namespace Csn.Retail.Editorial.Web.Infrastructure.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || !source.Any();
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null) return;

            foreach (var item in source)
            {
                action.Invoke(item);
            }
        }

        public static string JoinWith(this IEnumerable<string> source, string seperator)
        {
            if (source == null) return string.Empty;

            return string.Join(seperator, source);
        }

        /// <summary>
        /// Recursively flattens IEnumerable<T> where children of type T can be obtained using recursion func
        /// </summary>
        public static IEnumerable<T> Flatten<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> recursion)
        {
            var result = new List<T>();

            foreach (var item in source)
            {
                result.Add(item);

                var recursiveItems = recursion(item);

                if (recursiveItems == null) continue;

                result.AddRange(recursiveItems.Flatten(recursion));
            }

            return result;
        }
    }
}