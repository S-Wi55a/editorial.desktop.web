using System.Linq;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;

namespace Csn.Retail.Editorial.Web.Features.Details.Mappings
{
    public interface IPolarNativeAdsDataMapper
    {
        PolarNativeAdsData Map(ArticleDetailsDto source);
    }

    [AutoBind]
    public class PolarNativeAdsDataMapper : IPolarNativeAdsDataMapper
    {
        private readonly ITenantProvider<TenantInfo> _tenantProvider;

        public PolarNativeAdsDataMapper(ITenantProvider<TenantInfo> tenantProvider)
        {
            _tenantProvider = tenantProvider;
        }

        public PolarNativeAdsData Map(ArticleDetailsDto source)
        {
            if (!_tenantProvider.Current().DisplayPolarAds)
            {
                return null;
            }

            var data = new PolarNativeAdsData
            {
                AreaName = "editorials_details",
                MakeModel = ""
            };

            if (source.Items != null && source.Items.Any())
            {
                var item = source.Items.First();

                data.MakeModel = $"{item.Make}{(string.IsNullOrEmpty(item.Model) ? "" : item.Model)}".Replace("-", "").Replace(" ", "");
            }

            return data;
        }
    }
}