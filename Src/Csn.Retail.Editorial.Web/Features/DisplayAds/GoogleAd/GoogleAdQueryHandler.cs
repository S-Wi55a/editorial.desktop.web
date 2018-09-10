using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.DisplayAds.GoogleAd.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.SimpleCqrs;
using Newtonsoft.Json;

namespace Csn.Retail.Editorial.Web.Features.DisplayAds.GoogleAd
{
    [AutoBind]
    public class GoogleAdQueryHandler : IQueryHandler<DisplayAdQuery, GoogleAdViewModel>
    {
        private readonly ITenantProvider<TenantInfo> _tenantProvider;

        public GoogleAdQueryHandler(ITenantProvider<TenantInfo> tenantProvider)
        {
            _tenantProvider = tenantProvider;
        }
        public GoogleAdViewModel Handle(DisplayAdQuery displayAdsQuery)
        {
            if (!GoogleAdSettings.GoogleAdTypes.TryGetValue(displayAdsQuery.AdPlacement, out var adSetting))
            {
                return null;
            }

            return new GoogleAdViewModel()
            {
                Description = displayAdsQuery.AdPlacement.ToString(),
                Dimensions = JsonConvert.SerializeObject(adSetting.DisplayAdsAdSize.Dimensions()),
                AdNetworkCode = _tenantProvider.Current().GoogleAdsNetworkCode,
                AdUnitId = adSetting.UnitId,
                AdSlotId = adSetting.SlotId,
                DisplayAdsSource = DisplayAdsSource.GoogleAd
            };
        }
    }
}