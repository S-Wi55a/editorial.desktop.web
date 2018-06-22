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

            var makeInSource = string.Empty;
            var modelInSource = string.Empty;

            var makeAspect = source.FirstOrDefault(a => a.Aspect == "Make");

            if (makeAspect != null)
            {
                makeInSource = makeAspect.Facet;

                var modelAspect = makeAspect.Children.FirstOrDefault(a => a.Aspect == "Model");

                if(modelAspect != null)
                {
                    modelInSource = modelAspect.Facet;
                }
            }
            
            return new PolarNativeAdsData
            {
                AreaName = areaName,
                MakeModel = $"{makeInSource}{(string.IsNullOrEmpty(modelInSource) ? "" : modelInSource)}".Replace("-", "").Replace(" ", "")
            };
        }
    }
}