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
    }
}