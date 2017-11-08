using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Csn.MultiTenant;

namespace Csn.Retail.Editorial.Web.Infrastructure.MultiTenant
{
    /// <summary>
    /// This class is built based on the original Csn.MultiTenant.Impl.TenantDataMapper. We needed to add
    /// support for comma separated values for a given key
    /// </summary>
    public class TenantDataMapper<T> : ITenantDataMapper<T> where T : ITenant, new()
    {
        private readonly ILogger _logger;

        public TenantDataMapper(ILogger logger)
        {
            _logger = logger;
        }

        public T Map(ITenantSettings settings)
        {
            var result = new T();

            var properties = result.GetType().GetProperties().Where(x => x.CanWrite);

            foreach (var property in properties)
            {
                if (string.Equals(property.Name, "name", StringComparison.OrdinalIgnoreCase))
                {
                    property.SetValue(result, settings.Name, null);
                }
                else
                {
                    if (settings.Settings.TryGetValue(property.Name, out string value))
                    {
                        if (property.PropertyType == typeof(List<string>))
                        {
                            property.SetValue(result, value.Split(',').ToList(), null);
                        }
                        else if (property.PropertyType == typeof(CultureInfo))
                        {
                            property.SetValue(result, CultureInfo.CreateSpecificCulture(value), null);
                        }
                        else
                        {
                            property.SetValue(result, Convert.ChangeType(value, property.PropertyType), null);
                        }
                    }
                    else
                    {
                        _logger.Warn("No value defined for property {0}", property.Name);
                    }
                }
            }

            return result;
        }
    }
}