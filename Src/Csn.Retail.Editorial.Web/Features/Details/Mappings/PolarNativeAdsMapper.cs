using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;

namespace Csn.Retail.Editorial.Web.Features.Details.Mappings
{

    public interface IPolarNativeAdsMapper
    {
        PolarNativeAds Map();
    }

    [AutoBind]
    public class PolarNativeAdsMapper : IPolarNativeAdsMapper
    {
        private readonly ITenantProvider<TenantInfo> _tenantInfo;

        public PolarNativeAdsMapper(ITenantProvider<TenantInfo> tenantInfo)
        {
            _tenantInfo = tenantInfo;
        }

        public PolarNativeAds Map()
        {
 
            return new PolarNativeAds
            {

                SectionName = _tenantInfo.Current().Name,
                SitePropertyId = _tenantInfo. Current().SitePropertyId,
                SiteName = _tenantInfo.Current().SiteName,
                //Title = seoData.Title,
                //AlternateUrl = seoData.AlternateUrl,
                //CanonicalUrl = seoData.CanonicalUrl,
                //Description = seoData.Description,
                //Keywords = seoData.Keywords,
                //AllowSeoIndexing = _tenantInfo.Current().AllowSeoIndexOfDetails
            };
        }
    }
}