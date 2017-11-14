using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Web;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;

namespace Csn.Retail.Editorial.Web.Infrastructure.ContextStores
{
    //public interface IContextStore
    //{
    //    T GetOrFetch<T>(string key, Func<T> fetch);
    //}

    //[AutoBindAsPerRequest]
    //public class ContextStore : IContextStore
    //{
    //    private readonly ConcurrentDictionary<string, object> store;

    //    public ContextStore()
    //    {
    //        store = new ConcurrentDictionary<string, object>();
    //    }

    //    [DebuggerStepThrough]
    //    private object Get(string name)
    //    {
    //        return store.TryGetValue(name, out object result) ? result : null;
    //    }

    //    [DebuggerStepThrough]
    //    private void Set(string name, object value)
    //    {
    //        store.TryAdd(name, value);
    //    }

    //    private bool Exists(string key)
    //    {
    //        return HttpContext.Current != null && HttpContext.Current.Items.Contains(key);
    //    }

    //    public T GetOrFetch<T>(string key, Func<T> fetch)
    //    {
    //        T result;

    //        if (Exists(key))
    //        {
    //            result = (T)Get(key);
    //        }
    //        else
    //        {
    //            result = fetch.Invoke();

    //            Set(key, result);
    //        }

    //        return result;
    //    }
    //}

    public interface IContextStore<T>
    {
        T Get();
        void Set(T value);
    }

    public class ContextStore<T> : IContextStore<T>
    {
        private T value;

        public ContextStore()
        {
            value = default(T);
        }

        public T Get()
        {
            return value;
        }

        public void Set(T data)
        {
            value = data;
        }
    }
}