using System.Web.Mvc;

namespace Csn.Retail.Editorial.Web.Infrastructure.Extensions
{
    public static class ModelBinderExtensions
    {
        public static int TryGetValueOrDefault(this IValueProvider valueProvider, string key, int defaultValue)
        {
            var stringValue = TryGetValueOrDefault(valueProvider, key, string.Empty);

            return int.TryParse(stringValue, out var value) ? value : defaultValue;
        }

        public static string TryGetValueOrDefault(this IValueProvider valueProvider, string key, string defaultValue)
        {
            if (valueProvider == null || !valueProvider.ContainsPrefix(key))
            {
                return defaultValue;
            }

            var valueProviderResult = valueProvider.GetValue(key);
            return valueProviderResult != null
                ? valueProviderResult.AttemptedValue
                : defaultValue;
        }
    }
}