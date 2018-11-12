using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Csn.MultiTenant.Configs;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;

namespace Csn.Retail.Editorial.Web.Infrastructure.MultiTenant
{
    public interface ITenantListProvider
    {
        List<string> GetTenants();
    }

    [AutoBindAsSingleton]
    public class TenantListProvider : ITenantListProvider
    {
        private readonly List<string> _tenants = new List<string>();

        public TenantListProvider()
        {
            var configSection = ConfigurationManager.GetSection("TenantsSettings") as TenantConfigSection;

            if (configSection == null) return;

            foreach (var tenantElement in configSection.Tenants.Cast<TenantSettingsElement>())
            {
                _tenants.Add(tenantElement.Name);
            }
        }

        public List<string> GetTenants()
        {
            return _tenants;
        }
    }
}