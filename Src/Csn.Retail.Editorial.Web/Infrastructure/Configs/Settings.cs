namespace Csn.Retail.Editorial.Web.Infrastructure.Configs
{
    public interface ISettings<out T> where T : class, new()
    {
        T Value { get; }
    }

    public abstract class SettingsBase<T> : ISettings<T> where T : class, new()
    {
        private static readonly object _lock = new object();
        private static T _settings;

        public T Value
        {
            get
            {
                if (_settings != null) return _settings;

                lock (_lock)
                {
                    if (_settings != null) return _settings;

                    _settings = SettingsHelper.GetSettings<T>(SectionName);

                    return _settings;
                }
            }
        }

        protected abstract string SectionName { get; }
    }
}