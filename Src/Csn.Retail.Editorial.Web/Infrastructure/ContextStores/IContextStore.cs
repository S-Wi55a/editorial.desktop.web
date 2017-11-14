using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Web;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;

namespace Csn.Retail.Editorial.Web.Infrastructure.ContextStores
{
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