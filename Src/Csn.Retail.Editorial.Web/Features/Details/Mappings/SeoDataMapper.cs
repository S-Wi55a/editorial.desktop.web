using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;

namespace Csn.Retail.Editorial.Web.Features.Details.Mappings
{
    public interface ISeoDataMapper
    {
        SeoData Map(Shared.Proxies.EditorialApi.SeoData article);
    }

    [AutoBind]
    public class SeoDataMapper : ISeoDataMapper
    {
        private readonly ITenantProvider<TenantInfo> _tenantInfo;

        public SeoDataMapper(ITenantProvider<TenantInfo> tenantInfo)
        {
            _tenantInfo = tenantInfo;
        }

        public SeoData Map(Shared.Proxies.EditorialApi.SeoData seoData)
        {
            if (seoData == null)
            {
                return new SeoData()
                {
                    AllowSeoIndexing = _tenantInfo.Current().AllowSeoIndexOfDetails
                };
            }

            return new SeoData()
            {
                Title = seoData.Title,
                AlternateUrl = seoData.AlternateUrl,
                CanonicalUrl = seoData.CanonicalUrl,
                CanonicalAmpUrl = seoData.CanonicalAmpUrl,
                Description = seoData.Description,
                Keywords = seoData.Keywords,
                AllowSeoIndexing = _tenantInfo.Current().AllowSeoIndexOfDetails
            };
        }
    }
}