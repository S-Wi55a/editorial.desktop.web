using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;

namespace Csn.Retail.Editorial.Web.Features.Listings.Mappings
{
    public interface ISeoDataMapper
    {
        SeoData Map(RyvussNavResultDto ryvussNavResult);
    }

    [AutoBind]
    public class SeoDataMapper : ISeoDataMapper
    {
        private readonly ITenantProvider<TenantInfo> _tenantProvider;

        public SeoDataMapper(ITenantProvider<TenantInfo> tenantProvider)
        {
            _tenantProvider = tenantProvider;
        }

        public SeoData Map(RyvussNavResultDto ryvussNavResult)
        {
            return new SeoData()
            {
                AllowSeoIndexing = _tenantProvider.Current().AllowSeoIndexOfListing,
                Title = ryvussNavResult.Metadata == null ? _tenantProvider.Current().DefaultPageTitle : ryvussNavResult.Metadata.Title,
                CanonicalUrl = $"{_tenantProvider.Current().UrlProtocol}://{_tenantProvider.Current().SiteDomain}/editorial/results{(string.IsNullOrEmpty(ryvussNavResult.Metadata.Seo) ? "/" : ryvussNavResult.Metadata.Seo)}"
            };
        }
    }
}