using System.Collections.Generic;

namespace Csn.Retail.Editorial.Web.Infrastructure.ContextStores
{
    public static class ContextStoreExtensions
    {
        public static T GetAs<T>(this IContextStore source, string key)
        {
            var result = source.Get(key);

            return result == null ? default(T) : (T)result;
        }
    }
}