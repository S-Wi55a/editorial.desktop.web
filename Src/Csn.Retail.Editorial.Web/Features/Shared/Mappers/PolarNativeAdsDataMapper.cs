using System.Collections.Generic;
using System.Linq;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;

namespace Csn.Retail.Editorial.Web.Features.Shared.Mappers
{
    public interface IPolarNativeAdsDataMapper
    {
        PolarNativeAdsData Map(IList<BreadCrumbDto> source, string areaName);
    }

    [AutoBind]
    public class PolarNativeAdsDataMapper : IPolarNativeAdsDataMapper
    {
        private readonly ITenantProvider<TenantInfo> _tenantProvider;

        public PolarNativeAdsDataMapper(ITenantProvider<TenantInfo> tenantProvider)
        {
            _tenantProvider = tenantProvider;
        }

        public PolarNativeAdsData Map(IList<BreadCrumbDto> source, string areaName)
        {
            if (!_tenantProvider.Current().DisplayPolarAds)
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
                AreaName = areaName,
                MakeModel = $"{makeInSource}{(string.IsNullOrEmpty(modelInSource) ? "" : modelInSource)}".Replace("-", "").Replace(" ", "")
            };
        }
    }
}