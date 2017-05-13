using Bolt.Common.Extensions;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Constants;

namespace Csn.Retail.Editorial.Web.Infrastructure.Redirects
{
    /// <summary>
    /// This is really bad code. We should not be writing tenant specific code anywhere in this
    /// web application! But, until we form a better URL redirect alternative, this will have
    /// to do.
    /// </summary>
    public class CarMobiDetailsUrlBuilder : IMobiDetailsUrlBuilder
    {
        private readonly ITenantProvider<TenantInfo> _tenantProvider;

        public CarMobiDetailsUrlBuilder(ITenantProvider<TenantInfo> tenantProvider)
        {
            _tenantProvider = tenantProvider;
        }

        public string Build(string editorialId)
        {
            var tenant = _tenantProvider.Current();

            return $"{tenant.MobiSiteUrl}/editorial/details/{editorialId}";
        }

        public bool IsSupported(string tenant)
        {
            return tenant.IsSame(TenantNames.Carsales);
        }
    }
}