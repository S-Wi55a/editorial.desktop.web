using System.Linq;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;

namespace Csn.Retail.Editorial.Web.Features.Listings.Mappings
{
    public interface IPolarNativeAdsDataMapper
    {
        PolarNativeAdsData Map(RyvussNavResultDto source);
    }

    [AutoBind]
    public class PolarNativeAdsDataMapper : IPolarNativeAdsDataMapper
    {
        private readonly ITenantProvider<TenantInfo> _tenantProvider;

        public PolarNativeAdsDataMapper(ITenantProvider<TenantInfo> tenantProvider)
        {
            _tenantProvider = tenantProvider;
        }

        public PolarNativeAdsData Map(RyvussNavResultDto source)
        {
            if (!_tenantProvider.Current().DisplayPolarAds)
            {
                return null;
            }

            var data = new PolarNativeAdsData
            {
                AreaName = "searchresults",
                MakeModel = ""
            };

            //if (source.Items != null && source.Items.Any())
            //{
            //    var item = source.Items.First();

            //    data.MakeModel = $"{item.Make}{(string.IsNullOrEmpty(item.Model) ? "" : item.Model)}".Replace("-", "").Replace(" ", "");
            //}

            return data;
        }
    }
}