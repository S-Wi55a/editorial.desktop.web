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