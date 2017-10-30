using System.Collections.Generic;
using System.Linq;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;

namespace Csn.Retail.Editorial.Web.Features.Listings.Mappings
{
    public interface IPolarNativeAdsDataMapper
    {
        PolarNativeAdsData Map(IList<BreadCrumbDto> source);
    }

    [AutoBind]
    public class PolarNativeAdsDataMapper : IPolarNativeAdsDataMapper
    {
        private readonly ITenantProvider<TenantInfo> _tenantProvider;

        public PolarNativeAdsDataMapper(ITenantProvider<TenantInfo> tenantProvider)
        {
            _tenantProvider = tenantProvider;
        }

        public PolarNativeAdsData Map(IList<BreadCrumbDto> source)
        {
            if (!_tenantProvider.Current().DisplayPolarAds || !source.Any())
            {
                return null;
            }

            string makeInSource = null;
            if (source.Any(a => a.Aspect == "Make"))
            {
                makeInSource = source.First(a => a.Aspect == "Make").Facet;
            }
            string modelInSource = null;

            if (source.Any(a => a.Aspect == "Model"))
            {
                modelInSource = source.First(a => a.Aspect == "Model").Facet;
            }

            return new PolarNativeAdsData
            {
                AreaName = "searchresults",
                MakeModel = $"{makeInSource}{(string.IsNullOrEmpty(modelInSource) ? "" : modelInSource)}".Replace("-", "").Replace(" ", "")
            };
        }
    }
}