using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;

namespace Csn.Retail.Editorial.Web.Infrastructure.ContextStores
{
    public interface IContextStore
    {
        object Get(string name);

        void Set(string name, object value);

        bool Exists(string key);

        T GetOrFetch<T>(string key, Func<T> fetch);

        Task<T> GetOrFetchAsync<T>(string key, Func<Task<T>> fetch);
    }

    [AutoBindAsPerRequest]
    public class ContextStore : IContextStore
    {
        private readonly ConcurrentDictionary<string, object> store;

        public ContextStore()
        {
            this.store = new ConcurrentDictionary<string, object>();
        }

        [DebuggerStepThrough]
        public object Get(string name)
        {
            object result;
            return store.TryGetValue(name, out result) ? result : null;
        }

        [DebuggerStepThrough]
        public void Set(string name, object value)
        {
            store.TryAdd(name, value);
        }

        public bool Exists(string key)
        {
            return HttpContext.Current != null && HttpContext.Current.Items.Contains(key);
        }

        public T GetOrFetch<T>(string key, Func<T> fetch)
        {
            T result;

            if (Exists(key))
            {
                result = (T)Get(key);
            }
            else
            {
                result = fetch.Invoke();

                Set(key, result);
            }

            return result;
        }

        public async Task<T> GetOrFetchAsync<T>(string key, Func<Task<T>> fetch)
        {
            T result;

            if (Exists(key))
            {
                result = (T)Get(key);
            }
            else
            {
                result = await fetch.Invoke();

                Set(key, result);
            }

            return result;
        }
    }

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
            this.value = data;
        }
    }
}